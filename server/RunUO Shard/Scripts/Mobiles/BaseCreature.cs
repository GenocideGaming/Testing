using System;
using System.Collections.Generic;
using Server;
using Server.Regions;
using Server.Targeting;
using Server.Network;
using Server.Multis;
using Server.Spells;
using Server.Misc;
using Server.Items;
using Server.ContextMenus;
using Server.Engines.Quests;
using Server.Engines.PartySystem;
using Server.Factions;
using Server.Scripts.Custom.Citizenship.CommonwealthBonuses;
using Server.Scripts.Engines.Factions.Instances.Factions;
using Server.Scripts;
using Server.Scripts.Custom.Citizenship;
using Server.Games;
using Server.Engines.XmlSpawner2;
using System.IO;

namespace Server.Mobiles
{
    #region Enums
    /// <summary>
    /// Summary description for MobileAI.
    /// </summary>
    ///
    public enum FightMode
    {
        None,			// Never focus on others
        Aggressor,		// Only attack aggressors
        Strongest,		// Attack the strongest
        Weakest,		// Attack the weakest
        Closest, 		// Attack the closest
        Evil			// Only attack aggressor -or- negative karma
    }

    public enum OrderType
    {
        None,			//When no order, let's roam
        Come,			//"(All/Name) come"  Summons all or one pet to your location.
        Drop,			//"(Name) drop"  Drops its loot to the ground (if it carries any).
        Follow,			//"(Name) follow"  Follows targeted being.
        //"(All/Name) follow me"  Makes all or one pet follow you.
        Friend,			//"(Name) friend"  Allows targeted player to confirm resurrection.
        Unfriend,		// Remove a friend
        Guard,			//"(Name) guard"  Makes the specified pet guard you. Pets can only guard their owner.
        //"(All/Name) guard me"  Makes all or one pet guard you.
        Attack,			//"(All/Name) kill",
        //"(All/Name) attack"  All or the specified pet(s) currently under your control attack the target.
        Patrol,			//"(Name) patrol"  Roves between two or more guarded targets.
        Release,		//"(Name) release"  Releases pet back into the wild (removes "tame" status).
        Stay,			//"(All/Name) stay" All or the specified pet(s) will stop and stay in current spot.
        Stop,			//"(All/Name) stop Cancels any current orders to attack, guard or follow.
        Transfer		//"(Name) transfer" Transfers complete ownership to targeted player.
    }

    [Flags]
    public enum FoodType
    {
        None = 0x0000,
        Meat = 0x0001,
        FruitsAndVegies = 0x0002,
        GrainsAndHay = 0x0004,
        Fish = 0x0008,
        Eggs = 0x0010,
        Gold = 0x0020
    }

    [Flags]
    public enum PackInstinct
    {
        None = 0x0000,
        Canine = 0x0001,
        Ostard = 0x0002,
        Feline = 0x0004,
        Arachnid = 0x0008,
        Daemon = 0x0010,
        Bear = 0x0020,
        Equine = 0x0040,
        Bull = 0x0080
    }

    public enum ScaleType
    {
        Red,
        Yellow,
        Black,
        Green,
        White,
        Blue,
        All
    }

    public enum MeatType
    {
        Ribs,
        Bird,
        LambLeg
    }

    public enum HideType
    {
        Regular,
        Spined,
        Horned,
        Barbed
    }

    #endregion

    public class BaseCreature : Mobile
    {
        public const int MaxLoyalty = 100;

        #region Var declarations
        private BaseAI m_AI;					// THE AI

        private AIType m_CurrentAI;			// The current AI
        private AIType m_DefaultAI;			// The default AI

        private Mobile m_FocusMob;				// Use focus mob instead of combatant, maybe we don't whan to fight
        private FightMode m_FightMode;			// The style the mob uses

        private int m_iRangePerception;		// The view area
        private int m_iRangeFight;			// The fight distance

        private bool m_bDebugAI;				// Show debug AI messages

        private int m_iTeam;				// Monster Team

        private double m_dActiveSpeed;			// Timer speed when active
        private double m_dPassiveSpeed;		// Timer speed when not active
        private double m_dCurrentSpeed;		// The current speed, lets say it could be changed by something; 

        private Mobile m_SpellTarget;
        private Mobile m_HealTarget;

        private int m_SpellDelayMin = FeatureList.BaseCreatureAI.SpellDelayMin;   //Minimum Delay Before Mobile is allowed to cast next spell 
        private int m_SpellDelayMax = FeatureList.BaseCreatureAI.SpellDelayMax;   //Maximum Delay Before Mobile will cast next spell

        private DateTime m_NextCombatSpecialActionAllowed;   //When Mobile is allowed to use a special action in combat
        private int m_CombatSpecialActionDelay = FeatureList.BaseCreatureAI.CombatSpecialActionDelay; //Minimum Delay Between Special Actions

        private DateTime m_NextCombatHealActionAllowed;   //When Mobile is allowed to use a heal action in combat
        private int m_CombatHealActionDelay = FeatureList.BaseCreatureAI.CombatHealActionDelay; //Minimum Delay Between heal Actions

        private DateTime m_NextWanderActionAllowed;   //When Mobile is allowed to use a heal action in combat
        private int m_WanderActionDelay = FeatureList.BaseCreatureAI.WanderActionDelay; //Minimum Delay Between Wander Actions

        private DateTime m_NextPoisonEffectAllowed;   //When Mobile is allowed to inflict poison again

        public DateTime m_NextMovementTime = DateTime.MinValue;

        private Point3D m_pHome;				// The home position of the creature, used by some AI
        private Point3D m_LastEnemyLocation;    // Last known location of enemy combatant
        private int m_iRangeHome = FeatureList.BaseCreatureAI.DefaultHomeRange;		// The home range of the creature

        List<Type> m_arSpellAttack;		// List of attack spell/power
        List<Type> m_arSpellDefense;		// List of defensive spell/power

        private bool m_bControlled;		// Is controlled
        private Mobile m_ControlMaster;	// My master
        private Mobile m_ControlTarget;	// My target mobile
        private Point3D m_ControlDest;		// My target destination 
        private OrderType m_ControlOrder;		// My order

        private int m_Loyalty;

        private double m_dMinTameSkill;
        private bool m_bTamable;

        private bool m_bSummoned = false;
        private DateTime m_SummonEnd;
        private int m_iControlSlots = 1;

        private bool m_SuperPredator = false;
        private bool m_Predator = false;
        private bool m_Prey = false;

        private bool m_DoingBandage = false; //If Doing an AI Bandage Action
        private bool m_BandageOtherReady = false; //Changed When Attempting to Reach a Target to Bandage
        private DateTime m_BandageTimeout; //Timer Checking if Bandage Action Has Occured Yet

        private bool m_bBardProvoked = false;
        private bool m_bBardPacified = false;
        private Mobile m_bBardMaster = null;
        private Mobile m_bBardTarget = null;
        private DateTime m_timeBardEnd;
        private WayPoint m_CurrentWayPoint = null;
        private IPoint2D m_TargetLocation = null;

        private Mobile m_SummonMaster;

        private int m_HitsMax = -1;
        private int m_StamMax = -1;
        private int m_ManaMax = -1;
        private int m_DamageMin = -1;
        private int m_DamageMax = -1;

        private int m_PhysicalResistance, m_PhysicalDamage = 100;
        private int m_FireResistance, m_FireDamage;
        private int m_ColdResistance, m_ColdDamage;
        private int m_PoisonResistance, m_PoisonDamage;
        private int m_EnergyResistance, m_EnergyDamage;
        private int m_ChaosDamage;
        private int m_DirectDamage;

        private List<Mobile> m_Owners;
        private List<Mobile> m_Friends;

        private bool m_IsStabled;

        private bool m_HasGeneratedLoot; // have we generated our loot yet?

        private bool m_Paragon;

        private bool m_IsPrisoner;

        private DateTime m_NextDecisionTime;
        private bool m_AIActionInProgress = false;

        public bool HasBeenPseudoseerControlled = false; // cancel gold bonus

        private int m_LastDamageAmount = 0;
        [CommandProperty(AccessLevel.GameMaster)]
        public int LastDamageAmount { get { return m_LastDamageAmount; } set { m_LastDamageAmount = value; } }

        #endregion

        private int m_SpellCastAnimation = 4;   //Animation to Use for Spellcasting
        private int m_SpellCastFrameCount = 4;   //Frame Count in Spellcasting Animation

        public const int DefaultRangePerception = 12;
        public const int OldRangePerception = 10;

        private AIGroup m_AIGroup = AIGroup.None;
        private AISubgroup m_AISubgroup = AISubgroup.None;

        public BaseCreature(AIType ai,
            FightMode mode,
            int iRangePerception,
            int iRangeFight,
            double dActiveSpeed,
            double dPassiveSpeed)
        {
            if (iRangePerception == OldRangePerception)
                iRangePerception = DefaultRangePerception;

            AITeamList.SetDefaultTeam(this);

            // always give it a pack right away--prevents a client crash bug with pseudoseer system (when the
            // client auto-opens the backpack after "logging in" as an NPC, and the NPC has no backpack,
            // the client crashes
            Backpack pack = new Backpack();
            pack.Movable = false;
            AddItem(pack);

            m_Loyalty = MaxLoyalty; // Wonderfully Happy

            m_CurrentAI = ai;
            m_DefaultAI = ai;

            m_iRangePerception = iRangePerception;
            m_iRangeFight = iRangeFight;

            m_FightMode = mode;

            m_iTeam = 0;

            SpeedInfo.GetSpeeds(this, ref dActiveSpeed, ref dPassiveSpeed);

            m_dActiveSpeed = dActiveSpeed;
            m_dPassiveSpeed = dPassiveSpeed;
            m_dCurrentSpeed = dPassiveSpeed;

            m_bDebugAI = false;

            m_arSpellAttack = new List<Type>();
            m_arSpellDefense = new List<Type>();

            m_bControlled = false;
            m_ControlMaster = null;
            m_ControlTarget = null;
            m_ControlOrder = OrderType.None;

            m_bTamable = false;

            m_Owners = new List<Mobile>();

            m_NextReacquireTime = DateTime.Now + ReacquireDelay;

            ChangeAIType(AI);

            InhumanSpeech speechType = this.SpeechType;

            if (speechType != null && Speaks)
                speechType.OnConstruct(this);

            Timer.DelayCall(TimeSpan.FromSeconds(1.0), new TimerCallback(GenerateLoot));

            AICreatureList.GetAI(this);
            UpdateAI();
        }

        //Combat AI
        public Dictionary<CombatTargeting, int> DictCombatTargeting = new Dictionary<CombatTargeting, int>();
        public Dictionary<CombatTargetingWeight, int> DictCombatTargetingWeight = new Dictionary<CombatTargetingWeight, int>();
        public Dictionary<CombatRange, int> DictCombatRange = new Dictionary<CombatRange, int>();
        public Dictionary<CombatFlee, int> DictCombatFlee = new Dictionary<CombatFlee, int>();
        public Dictionary<CombatAction, int> DictCombatAction = new Dictionary<CombatAction, int>();
        public Dictionary<CombatSpell, int> DictCombatSpell = new Dictionary<CombatSpell, int>();
        public Dictionary<CombatHealSelf, int> DictCombatHealSelf = new Dictionary<CombatHealSelf, int>();
        public Dictionary<CombatHealOther, int> DictCombatHealOther = new Dictionary<CombatHealOther, int>();
        public Dictionary<CombatSpecialAction, int> DictCombatSpecialAction = new Dictionary<CombatSpecialAction, int>();

        //Guard AI
        public Dictionary<GuardAction, int> DictGuardAction = new Dictionary<GuardAction, int>();

        //Wander AI
        public Dictionary<WanderAction, int> DictWanderAction = new Dictionary<WanderAction, int>();

        //Interact AI
        public Dictionary<InteractAction, int> DictInteractAction = new Dictionary<InteractAction, int>();

        //Set BaseAI Values
        public virtual void PopulateAI()
        {
            SuperPredator = false;
            Predator = false;
            Prey = false;

            //Combat AI
            DictCombatTargeting.Clear();
            DictCombatTargeting.Add(CombatTargeting.OpposingFaction, 1);
            DictCombatTargeting.Add(CombatTargeting.PlayerGood, 0);
            DictCombatTargeting.Add(CombatTargeting.PlayerCriminal, 0);
            DictCombatTargeting.Add(CombatTargeting.PlayerAny, 0);
            DictCombatTargeting.Add(CombatTargeting.SuperPredator, 0);
            DictCombatTargeting.Add(CombatTargeting.Predator, 0);
            DictCombatTargeting.Add(CombatTargeting.Prey, 0);
            DictCombatTargeting.Add(CombatTargeting.Good, 0);
            DictCombatTargeting.Add(CombatTargeting.Neutral, 0);
            DictCombatTargeting.Add(CombatTargeting.Evil, 0);
            DictCombatTargeting.Add(CombatTargeting.Aggressor, 2);
            DictCombatTargeting.Add(CombatTargeting.Any, 0);
            DictCombatTargeting.Add(CombatTargeting.None, 0);

            DictCombatTargetingWeight.Clear();
            DictCombatTargetingWeight.Add(CombatTargetingWeight.CurrentCombatant, 3);
            DictCombatTargetingWeight.Add(CombatTargetingWeight.Closest, 3);
            DictCombatTargetingWeight.Add(CombatTargetingWeight.HighestHitPoints, 0);
            DictCombatTargetingWeight.Add(CombatTargetingWeight.LowestHitPoints, 0);
            DictCombatTargetingWeight.Add(CombatTargetingWeight.HighestArmor, 0);
            DictCombatTargetingWeight.Add(CombatTargetingWeight.LowestArmor, 0);
            DictCombatTargetingWeight.Add(CombatTargetingWeight.Ranged, 0);
            DictCombatTargetingWeight.Add(CombatTargetingWeight.Spellcaster, 0);
            DictCombatTargetingWeight.Add(CombatTargetingWeight.Summoned, 0);
            DictCombatTargetingWeight.Add(CombatTargetingWeight.Poisoner, 0);

            DictCombatRange.Clear();
            DictCombatRange.Add(CombatRange.WeaponAttackRange, 1);
            DictCombatRange.Add(CombatRange.SpellRange, 0);
            DictCombatRange.Add(CombatRange.Withdraw, 0);

            DictCombatFlee.Clear();
            DictCombatFlee.Add(CombatFlee.Flee75, 0);
            DictCombatFlee.Add(CombatFlee.Flee50, 0);
            DictCombatFlee.Add(CombatFlee.Flee25, 0);
            DictCombatFlee.Add(CombatFlee.Flee10, 0);

            DictCombatAction.Clear();
            DictCombatAction.Add(CombatAction.AttackOnly, 1);
            DictCombatAction.Add(CombatAction.CombatSpell, 0);
            DictCombatAction.Add(CombatAction.CombatHealSelf, 0);
            DictCombatAction.Add(CombatAction.CombatHealOther, 0);
            DictCombatAction.Add(CombatAction.CombatSpecialAction, 0);

            DictCombatSpell.Clear();
            DictCombatSpell.Add(CombatSpell.SpellDamage1, 0);
            DictCombatSpell.Add(CombatSpell.SpellDamage2, 0);
            DictCombatSpell.Add(CombatSpell.SpellDamage3, 0);
            DictCombatSpell.Add(CombatSpell.SpellDamage4, 0);
            DictCombatSpell.Add(CombatSpell.SpellDamage5, 0);
            DictCombatSpell.Add(CombatSpell.SpellDamage6, 0);
            DictCombatSpell.Add(CombatSpell.SpellDamage7, 0);
            DictCombatSpell.Add(CombatSpell.SpellDamageAOE7, 0);
            DictCombatSpell.Add(CombatSpell.SpellPoison, 0);
            DictCombatSpell.Add(CombatSpell.SpellNegative1to3, 0);
            DictCombatSpell.Add(CombatSpell.SpellNegative4to7, 0);
            DictCombatSpell.Add(CombatSpell.SpellSummon5, 0);
            DictCombatSpell.Add(CombatSpell.SpellSummon8, 0);
            DictCombatSpell.Add(CombatSpell.SpellDispelSummon, 0);
            DictCombatSpell.Add(CombatSpell.SpellHarmfulField, 0);
            DictCombatSpell.Add(CombatSpell.SpellNegativeField, 0);
            DictCombatSpell.Add(CombatSpell.SpellBeneficial1to2, 0);
            DictCombatSpell.Add(CombatSpell.SpellBeneficial3to4, 0);
            DictCombatSpell.Add(CombatSpell.SpellBeneficial5, 0);

            DictCombatHealSelf.Clear();
            DictCombatHealSelf.Add(CombatHealSelf.SpellHealSelf75, 0);
            DictCombatHealSelf.Add(CombatHealSelf.SpellHealSelf50, 0);
            DictCombatHealSelf.Add(CombatHealSelf.SpellHealSelf25, 0);
            DictCombatHealSelf.Add(CombatHealSelf.SpellCureSelf, 0);
            DictCombatHealSelf.Add(CombatHealSelf.PotionHealSelf75, 0);
            DictCombatHealSelf.Add(CombatHealSelf.PotionHealSelf50, 0);
            DictCombatHealSelf.Add(CombatHealSelf.PotionHealSelf25, 0);
            DictCombatHealSelf.Add(CombatHealSelf.PotionCureSelf, 0);
            DictCombatHealSelf.Add(CombatHealSelf.BandageHealSelf75, 0);
            DictCombatHealSelf.Add(CombatHealSelf.BandageHealSelf50, 0);
            DictCombatHealSelf.Add(CombatHealSelf.BandageHealSelf25, 0);
            DictCombatHealSelf.Add(CombatHealSelf.BandageCureSelf, 0);

            DictCombatHealOther.Clear();
            DictCombatHealOther.Add(CombatHealOther.SpellHealOther75, 0);
            DictCombatHealOther.Add(CombatHealOther.SpellHealOther50, 0);
            DictCombatHealOther.Add(CombatHealOther.SpellHealOther25, 0);
            DictCombatHealOther.Add(CombatHealOther.SpellCureOther, 0);
            DictCombatHealOther.Add(CombatHealOther.BandageHealOther75, 0);
            DictCombatHealOther.Add(CombatHealOther.BandageHealOther50, 0);
            DictCombatHealOther.Add(CombatHealOther.BandageHealOther25, 0);
            DictCombatHealOther.Add(CombatHealOther.BandageCureOther, 0);

            DictCombatSpecialAction.Clear();
            DictCombatSpecialAction.Add(CombatSpecialAction.ApplyWeaponPoison, 0);
            DictCombatSpecialAction.Add(CombatSpecialAction.PoisonHit, 0);
            DictCombatSpecialAction.Add(CombatSpecialAction.ThrowBomb, 0);
            DictCombatSpecialAction.Add(CombatSpecialAction.BreathAttack, 0);
            DictCombatSpecialAction.Add(CombatSpecialAction.ThrowingKnife, 0);
            DictCombatSpecialAction.Add(CombatSpecialAction.None, 0);

            DictWanderAction.Clear();
            DictWanderAction.Add(WanderAction.DetectHidden, 0);
            DictWanderAction.Add(WanderAction.SpellReveal, 0);
            DictWanderAction.Add(WanderAction.Tracking, 0);
            DictWanderAction.Add(WanderAction.Stealth, 0);
            DictWanderAction.Add(WanderAction.SpellHealSelf100, 0);
            DictWanderAction.Add(WanderAction.BandageHealSelf100, 0);
            DictWanderAction.Add(WanderAction.PotionHealSelf100, 0);
            DictWanderAction.Add(WanderAction.SpellCureSelf, 0);
            DictWanderAction.Add(WanderAction.BandageCureSelf, 0);
            DictWanderAction.Add(WanderAction.PotionCureSelf, 0);
            DictWanderAction.Add(WanderAction.SpellHealOther100, 0);
            DictWanderAction.Add(WanderAction.BandageHealOther100, 0);
            DictWanderAction.Add(WanderAction.SpellCureOther, 0);
            DictWanderAction.Add(WanderAction.BandageCureOther, 0);
            DictWanderAction.Add(WanderAction.None, 0);
        }

        public virtual void UpdateAI()
        {
            //ResetAI Libraries
            PopulateAI();

            //Looks Up Group/Subgroup and Overrides Values
            AIDefinitions AIObject = new AIDefinitions();
            AIObject.UpdateAI(this);

            //Default to WanderMode on AI Change
            m_AI.WanderMode();
        }

        public BaseCreature(Serial serial)
            : base(serial)
        {
            m_arSpellAttack = new List<Type>();
            m_arSpellDefense = new List<Type>();

            m_bDebugAI = false;
        }

        private bool m_BardImmuneCustom = false;
        [CommandProperty(AccessLevel.GameMaster)]
        public virtual bool BardImmuneCustom
        {
            get { return m_BardImmuneCustom; }
            set { m_BardImmuneCustom = value; }
        }

        private bool m_Pseu_EQPlayerAllowed = false;
        [CommandProperty(AccessLevel.GameMaster)]
        public virtual bool Pseu_EQPlayerAllowed
        {
            get { return m_Pseu_EQPlayerAllowed; }
            set { m_Pseu_EQPlayerAllowed = value; }
        }

        private bool m_Pseu_SpellBookRequired = false;
        [CommandProperty(AccessLevel.GameMaster)]
        public virtual bool Pseu_SpellBookRequired
        {
            get { return m_Pseu_SpellBookRequired; }
            set { m_Pseu_SpellBookRequired = value; }
        }

        private bool m_Pseu_KeepKillCredit = true; // if true, RegisterKill uses the controlled basecreature as the LastKiller, otherwise it uses the Last controlled pseudoseer playermobile as the LastKiller
        [CommandProperty(AccessLevel.GameMaster)]
        public virtual bool Pseu_KeepKillCredit
        {
            get { return m_Pseu_KeepKillCredit; }
            set { m_Pseu_KeepKillCredit = value; }
        }

        private bool m_Pseu_AllowFizzle = true;
        [CommandProperty(AccessLevel.GameMaster)]
        public virtual bool Pseu_AllowFizzle
        {
            get { return m_Pseu_AllowFizzle; }
            set { m_Pseu_AllowFizzle = value; }
        }

        private bool m_Pseu_AllowInterrupts = false;
        [CommandProperty(AccessLevel.GameMaster)]
        public virtual bool Pseu_AllowInterrupts
        {
            get { return m_Pseu_AllowInterrupts; }
            set { m_Pseu_AllowInterrupts = value; }
        }

        private TimeSpan m_Pseu_SpellDelay = TimeSpan.Zero;
        [CommandProperty(AccessLevel.GameMaster)]
        public virtual TimeSpan Pseu_SpellDelay
        {
            get { return m_Pseu_SpellDelay; }
            set { m_Pseu_SpellDelay = value; }
        }

        private bool m_Pseu_TeleWallDelay = true;
        [CommandProperty(AccessLevel.GameMaster)]
        public virtual bool Pseu_TeleWallDelay
        {
            get { return m_Pseu_TeleWallDelay; }
            set { m_Pseu_TeleWallDelay = value; }
        }

        private bool m_Pseu_CanBeHealed = true;
        [CommandProperty(AccessLevel.GameMaster)]
        public virtual bool Pseu_CanBeHealed
        {
            get { return m_Pseu_CanBeHealed; }
            set { m_Pseu_CanBeHealed = value; }
        }

        private bool m_Pseu_ConsumeReagents = false;
        [CommandProperty(AccessLevel.GameMaster)]
        public virtual bool Pseu_ConsumeReagents
        {
            get { return m_Pseu_ConsumeReagents; }
            set { m_Pseu_ConsumeReagents = value; }
        }

        private DateTime m_NextTeleportAt = DateTime.MinValue;
        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime NextTeleportAt
        {
            get { return m_NextTeleportAt; }
            set { m_NextTeleportAt = value; }
        }

        private DateTime m_NextWallOfStone = DateTime.MinValue;
        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime NextWallOfStone
        {
            get { return m_NextWallOfStone; }
            set { m_NextWallOfStone = value; }
        }

        private bool m_PowerWords = false;
        [CommandProperty(AccessLevel.GameMaster)]
        public virtual bool PowerWords
        {
            get { return m_PowerWords; }
            set { m_PowerWords = value; }
        }

        private bool m_ClearHandsOnCast = false;
        [CommandProperty(AccessLevel.GameMaster)]
        public virtual bool ClearHandsOnCast
        {
            get { return m_ClearHandsOnCast; }
            set { m_ClearHandsOnCast = value; }
        }

        private bool m_WeaponDamage = false;
        [CommandProperty(AccessLevel.GameMaster)]
        public virtual bool WeaponDamage
        {
            get { return m_WeaponDamage; }
            set { m_WeaponDamage = value; }
        }

        private bool m_TakesNormalDamage = false;
        [CommandProperty(AccessLevel.GameMaster)]
        public virtual bool TakesNormalDamage
        {
            get { return m_TakesNormalDamage; }
            set { m_TakesNormalDamage = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public virtual ActionType AIAction
        {
            get
            {
                if (m_AI != null) { return m_AI.Action; }
                else { return ActionType.None; }
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public virtual string PseudoSeerPermissions
        {
            get
            {
                if (PseudoSeerStone.Instance == null || this.NetState == null)
                {
                    return null;
                }
                return PseudoSeerStone.Instance.GetPermissionsFor(this.NetState.Account);
            }
        }

        private bool m_KillCriminals = false;
        [CommandProperty(AccessLevel.GameMaster)]
        public bool KillCriminals { get { return m_KillCriminals; } set { m_KillCriminals = true; } }

        private bool m_KillMurderers = false;
        [CommandProperty(AccessLevel.GameMaster)]
        public bool KillMurderers { get { return m_KillMurderers; } set { m_KillMurderers = true; } }

        private bool m_InnocentDefault = false;
        [CommandProperty(AccessLevel.GameMaster)]
        public bool InnocentDefault
        {
            get { return m_InnocentDefault; }
            set { m_InnocentDefault = value; Delta(MobileDelta.Noto); InvalidateProperties(); }
        }

        private bool m_GuardImmune = false;
        [CommandProperty(AccessLevel.GameMaster)]
        public bool GuardImmune { get { return m_GuardImmune; } set { m_GuardImmune = value; } }

        private bool m_ForceWaypoint = false; // just for the current waypoint, not for all waypoints
        [CommandProperty(AccessLevel.GameMaster)]
        public bool ForceWaypoint
        {
            get { return m_ForceWaypoint; }
            set { m_ForceWaypoint = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public virtual string TeamFlagsReadable
        {
            get
            {
                string output = "";
                ulong i = 1UL;
                for (int j = 0; j < 64; j++)
                {
                    if ((m_TeamFlags & i) > 0)
                    {
                        output += Enum.ToObject(typeof(AITeamList.TeamFlags), (m_TeamFlags & i)).ToString() + ", ";
                    }
                    i = i << 1;
                }
                return output == "" ? "None" : output;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public virtual AITeamList.TeamFlags TeamFlagsAdd { get { return AITeamList.TeamFlags.None; } set { m_TeamFlags |= (ulong)value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public virtual AITeamList.TeamFlags TeamFlagsDelete { get { return AITeamList.TeamFlags.None; } set { m_TeamFlags &= ~(ulong)value; } }

        [CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
        public bool DoingBandage
        {
            get { return m_DoingBandage; }
            set
            {
                m_DoingBandage = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
        public bool BandageOtherReady
        {
            get { return m_BandageOtherReady; }
            set
            {
                m_BandageOtherReady = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
        public DateTime BandageTimeout
        {
            get { return m_BandageTimeout; }
            set
            {
                m_BandageTimeout = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
        public AIGroup Group
        {
            get { return m_AIGroup; }
            set
            {
                m_AIGroup = value;

                UpdateAI();
            }
        }

        [CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
        public AISubgroup Subgroup
        {
            get { return m_AISubgroup; }
            set
            {
                m_AISubgroup = value;
                UpdateAI();
            }
        }

        [CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
        public bool SuperPredator
        {
            get { return m_SuperPredator; }
            set
            {
                m_SuperPredator = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
        public bool Predator
        {
            get { return m_Predator; }
            set
            {
                m_Predator = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
        public bool Prey
        {
            get { return m_Prey; }
            set
            {
                m_Prey = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
        public DateTime NextDecisionTime
        {
            get
            {
                return m_NextDecisionTime;
            }

            set
            {
                m_NextDecisionTime = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
        public int SpellCastAnimation
        {
            get
            {
                return m_SpellCastAnimation;
            }

            set
            {
                m_SpellCastAnimation = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
        public int SpellCastFrameCount
        {
            get
            {
                return m_SpellCastFrameCount;
            }

            set
            {
                m_SpellCastFrameCount = value;
            }
        }

        public virtual InhumanSpeech SpeechType { get { return null; } }

        [CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
        public DateTime NextCombatSpecialActionAllowed
        {
            get { return m_NextCombatSpecialActionAllowed; }
            set
            {
                m_NextCombatSpecialActionAllowed = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
        public int CombatSpecialActionDelay
        {
            get { return m_CombatSpecialActionDelay; }
            set
            {
                m_CombatSpecialActionDelay = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
        public DateTime NextCombatHealActionAllowed
        {
            get { return m_NextCombatHealActionAllowed; }
            set
            {
                m_NextCombatHealActionAllowed = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
        public int CombatHealActionDelay
        {
            get { return m_CombatHealActionDelay; }
            set
            {
                m_CombatHealActionDelay = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
        public DateTime NextWanderActionAllowed
        {
            get { return m_NextWanderActionAllowed; }
            set
            {
                m_NextWanderActionAllowed = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
        public int WanderActionDelay
        {
            get { return m_WanderActionDelay; }
            set
            {
                m_WanderActionDelay = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
        public DateTime NextPoisonEffectAllowed
        {
            get { return m_NextPoisonEffectAllowed; }
            set
            {
                m_NextPoisonEffectAllowed = value;
            }
        }


        [CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
        public int SpellDelayMin
        {
            get { return m_SpellDelayMin; }
            set
            {
                m_SpellDelayMin = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
        public int SpellDelayMax
        {
            get { return m_SpellDelayMax; }
            set
            {
                m_SpellDelayMax = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
        public bool IsStabled
        {
            get { return m_IsStabled; }
            set
            {
                m_IsStabled = value;
                if (m_IsStabled)
                    StopDeleteTimer();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool AIActionInProgress
        {
            get { return m_AIActionInProgress; }
            set { m_AIActionInProgress = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool IsPrisoner
        {
            get { return m_IsPrisoner; }
            set { m_IsPrisoner = value; }
        }

        protected DateTime SummonEnd
        {
            get { return m_SummonEnd; }
            set { m_SummonEnd = value; }
        }

        public virtual Faction FactionAllegiance { get { return null; } }
        public virtual int FactionSilverWorth { get { return 30; } }

        #region Bonding
        public const bool BondingEnabled = true;

        public virtual bool IsNecromancer { get { return (Skills[SkillName.Necromancy].Value > 50); } }

        public virtual bool IsBondable { get { return (BondingEnabled && !Summoned); } }
        public virtual TimeSpan BondingDelay { get { return TimeSpan.FromDays(7.0); } }
        public virtual TimeSpan BondingAbandonDelay { get { return TimeSpan.FromDays(1.0); } }

        public override bool CanRegenHits { get { return !m_IsDeadPet && base.CanRegenHits; } }
        public override bool CanRegenStam { get { return !m_IsDeadPet && base.CanRegenStam; } }
        public override bool CanRegenMana { get { return !m_IsDeadPet && base.CanRegenMana; } }

        public override bool IsDeadBondedPet { get { return m_IsDeadPet; } }

        private bool m_IsBonded;
        private bool m_IsDeadPet;
        private DateTime m_BondingBegin;
        private DateTime m_OwnerAbandonTime;

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile LastOwner
        {
            get
            {
                if (m_Owners == null || m_Owners.Count == 0)
                    return null;

                return m_Owners[m_Owners.Count - 1];
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool IsBonded
        {
            get { return m_IsBonded; }
            set { m_IsBonded = value; InvalidateProperties(); }
        }

        public bool IsDeadPet
        {
            get { return m_IsDeadPet; }
            set { m_IsDeadPet = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime BondingBegin
        {
            get { return m_BondingBegin; }
            set { m_BondingBegin = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime OwnerAbandonTime
        {
            get { return m_OwnerAbandonTime; }
            set { m_OwnerAbandonTime = value; }
        }
        #endregion

        #region Delete Previously Tamed Timer
        private DeleteTimer m_DeleteTimer;

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan DeleteTimeLeft
        {
            get
            {
                if (m_DeleteTimer != null && m_DeleteTimer.Running)
                    return m_DeleteTimer.Next - DateTime.Now;

                return TimeSpan.Zero;
            }
        }

        private class DeleteTimer : Timer
        {
            private Mobile m;

            public DeleteTimer(Mobile creature, TimeSpan delay)
                : base(delay)
            {
                m = creature;
                Priority = TimerPriority.OneMinute;
            }

            protected override void OnTick()
            {
                m.Delete();
            }
        }

        public void BeginDeleteTimer()
        {
            if (!(this is BaseEscortable) && !Summoned && !Deleted && !IsStabled)
            {
                StopDeleteTimer();
                m_DeleteTimer = new DeleteTimer(this, TimeSpan.FromHours(FeatureList.Followers.ReleaseDeleteTimer));
                m_DeleteTimer.Start();
            }
        }

        public void StopDeleteTimer()
        {
            if (m_DeleteTimer != null)
            {
                m_DeleteTimer.Stop();
                m_DeleteTimer = null;
            }
        }

        #endregion

        public virtual double WeaponAbilityChance { get { return 0.4; } }

        public virtual WeaponAbility GetWeaponAbility()
        {
            return null;
        }

        #region Elemental Resistance/Damage

        public override int BasePhysicalResistance { get { return m_PhysicalResistance; } }
        public override int BaseFireResistance { get { return m_FireResistance; } }
        public override int BaseColdResistance { get { return m_ColdResistance; } }
        public override int BasePoisonResistance { get { return m_PoisonResistance; } }
        public override int BaseEnergyResistance { get { return m_EnergyResistance; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int PhysicalResistanceSeed { get { return m_PhysicalResistance; } set { m_PhysicalResistance = value; UpdateResistances(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int FireResistSeed { get { return m_FireResistance; } set { m_FireResistance = value; UpdateResistances(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int ColdResistSeed { get { return m_ColdResistance; } set { m_ColdResistance = value; UpdateResistances(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int PoisonResistSeed { get { return m_PoisonResistance; } set { m_PoisonResistance = value; UpdateResistances(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int EnergyResistSeed { get { return m_EnergyResistance; } set { m_EnergyResistance = value; UpdateResistances(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int PhysicalDamage { get { return m_PhysicalDamage; } set { m_PhysicalDamage = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int FireDamage { get { return m_FireDamage; } set { m_FireDamage = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int ColdDamage { get { return m_ColdDamage; } set { m_ColdDamage = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int PoisonDamage { get { return m_PoisonDamage; } set { m_PoisonDamage = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int EnergyDamage { get { return m_EnergyDamage; } set { m_EnergyDamage = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int ChaosDamage { get { return m_ChaosDamage; } set { m_ChaosDamage = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int DirectDamage { get { return m_DirectDamage; } set { m_DirectDamage = value; } }

        #endregion

        [CommandProperty(AccessLevel.GameMaster)]
        public bool IsParagon
        {
            get { return m_Paragon; }
            set
            {
                if (m_Paragon == value)
                    return;
                else if (value)
                    Paragon.Convert(this);
                else
                    Paragon.UnConvert(this);

                m_Paragon = value;

                InvalidateProperties();
            }
        }

        public virtual FoodType FavoriteFood { get { return FoodType.Meat; } }
        public virtual PackInstinct PackInstinct { get { return PackInstinct.None; } }

        public List<Mobile> Owners { get { return m_Owners; } }

        public virtual int LootLevel { get { return 0; } }
        public virtual bool AllowMaleTamer { get { return true; } }
        public virtual bool AllowFemaleTamer { get { return true; } }
        public virtual bool SubdueBeforeTame { get { return false; } }
        public virtual bool StatLossAfterTame { get { return SubdueBeforeTame; } }
        public virtual bool ReduceSpeedWithDamage { get { return true; } }
        public virtual bool IsSubdued { get { return SubdueBeforeTame && (Hits < (HitsMax / 10)); } }

        public virtual bool Commandable { get { return true; } }


        public virtual Poison HitPoison { get { return null; } }
        public virtual double HitPoisonChance { get { return 0.5; } }
        public virtual Poison PoisonImmune { get { return null; } }


        private Poison m_PoisonCustomImmune = null;
        [CommandProperty(AccessLevel.GameMaster)]
        public Poison PoisonCustomImmune
        {
            get { return m_PoisonCustomImmune; }
            set { m_PoisonCustomImmune = value; }
        }
        private Poison m_PoisonCustomHit = null;
        [CommandProperty(AccessLevel.GameMaster)]
        public Poison PoisonCustomHit
        {
            get { return m_PoisonCustomHit; }
            set { m_PoisonCustomHit = value; }
        }
        private double m_PoisonCustomChance = -1.0;
        [CommandProperty(AccessLevel.GameMaster)]
        public double PoisonCustomChance
        {
            get { return m_PoisonCustomChance; }
            set { m_PoisonCustomChance = value; }
        }


        public virtual bool BardImmune { get { return false; } }
        public virtual bool Unprovokable { get { return BardImmune || BardImmuneCustom || m_IsDeadPet; } }
        public virtual bool Uncalmable { get { return BardImmune || BardImmuneCustom || m_IsDeadPet; } }
        public virtual bool AreaPeaceImmune { get { return BardImmune || BardImmuneCustom || m_IsDeadPet; } }

        public virtual bool BleedImmune { get { return false; } }
        public virtual double BonusPetDamageScalar { get { return 1.0; } }

        public virtual bool DeathAdderCharmable { get { return false; } }

        //TODO: Find the pub 31 tweaks to the DispelDifficulty and apply them of course.
        public virtual double DispelDifficulty { get { return 0.0; } } // at this skill level we dispel 50% chance
        public virtual double DispelFocus { get { return 20.0; } } // at difficulty - focus we have 0%, at difficulty + focus we have 100%
        public virtual bool DisplayWeight { get { return Backpack is StrongBackpack; } }

        public bool m_ReturnsHome = true; //Set to True, and is Overriden and Set to False for BladeSpirits / EV / and BaseEscortable

        #region Breath ability, like dragon fire breath
        private DateTime m_NextBreathTime;
        // Add getter for the [breath command (when a dragon is being controlled by a player)
        public DateTime NextBreathTime { get { return m_NextBreathTime; } set { m_NextBreathTime = value; } }

        // Must be overriden in subclass to enable
        public virtual bool HasBreath { get { return false; } }

        private double m_BreathCustomDelay = -1.0;
        [CommandProperty(AccessLevel.GameMaster)]
        public double BreathCustomDelay { get { return m_BreathCustomDelay; } set { m_BreathCustomDelay = value; } }
        // Min/max seconds until next breath
        public virtual double BreathMinDelay { get { return 10.0; } }
        public virtual double BreathMaxDelay { get { return 15.0; } }

        // Creature stops moving for 1.0 seconds while breathing
        public virtual double BreathStallTime { get { return 2.0; } }

        // Effect is sent 1.3 seconds after BreathAngerSound and BreathAngerAnimation is played
        public virtual double BreathEffectDelay { get { return 1.5; } }

        // Damage is given 1.0 seconds after effect is sent
        public virtual double BreathDamageDelay { get { return 1.5; } }

        public virtual int BreathRange { get { return 8; } }

        // Damage types
        public virtual int BreathPhysicalDamage { get { return 0; } }
        public virtual int BreathFireDamage { get { return 100; } }
        public virtual int BreathColdDamage { get { return 0; } }
        public virtual int BreathPoisonDamage { get { return 0; } }
        public virtual int BreathEnergyDamage { get { return 0; } }

        // Is immune to breath damages
        public virtual bool BreathImmune { get { return false; } }

        // Effect details and sound
        public virtual int BreathEffectItemID { get { return 0x36D4; } }
        public virtual int BreathEffectSpeed { get { return 5; } }
        public virtual int BreathEffectDuration { get { return 0; } }
        public virtual bool BreathEffectExplodes { get { return false; } }
        public virtual bool BreathEffectFixedDir { get { return false; } }
        public virtual int BreathEffectHue { get { return 0; } }
        public virtual int BreathEffectRenderMode { get { return 0; } }

        public virtual int BreathEffectSound { get { return 0x227; } }

        // Anger sound/animations
        public virtual int BreathAngerSound { get { return GetAngerSound(); } }
        public virtual int BreathAngerAnimation { get { return 12; } }

        public virtual void BreathStart(Mobile target)
        {
            BreathStallMovement();
            BreathPlayAngerSound();
            BreathPlayAngerAnimation();

            this.Direction = this.GetDirectionTo(target);

            Timer.DelayCall(TimeSpan.FromSeconds(BreathEffectDelay), new TimerStateCallback(BreathEffect_Callback), target);
        }

        public virtual void BreathStallMovement()
        {
            if (m_AI != null)
                m_AI.NextMove = DateTime.Now + TimeSpan.FromSeconds(BreathStallTime);
        }

        public virtual void BreathPlayAngerSound()
        {
            PlaySound(BreathAngerSound);
        }

        public virtual void BreathPlayAngerAnimation()
        {
            Animate(BreathAngerAnimation, 5, 1, true, false, 0);
        }

        public virtual void BreathEffect_Callback(object state)
        {
            Mobile target = (Mobile)state;

            if (!target.Alive || !CanBeHarmful(target))
                return;



            BreathPlayEffectSound();
            BreathPlayEffect(target);

            Timer.DelayCall(TimeSpan.FromSeconds(BreathDamageDelay), new TimerStateCallback(BreathDamage_Callback), target);
        }

        public virtual void BreathPlayEffectSound()
        {
            PlaySound(BreathEffectSound);
        }

        public virtual void BreathPlayEffect(Mobile target)
        {
            Effects.SendMovingEffect(this, target, BreathEffectItemID,
                BreathEffectSpeed, BreathEffectDuration, BreathEffectFixedDir,
                BreathEffectExplodes, BreathEffectHue, BreathEffectRenderMode);
        }

        public virtual void BreathDamage_Callback(object state)
        {
            Mobile target = (Mobile)state;

            if (target is BaseCreature && ((BaseCreature)target).BreathImmune)
                return;

            if (CanBeHarmful(target))
            {
                DoHarmful(target);
                BreathDealDamage(target);
            }
        }

        public virtual void BreathDealDamage(Mobile target)
        {
            if (BreathDamageCustom > -1)
                target.Damage(BreathDamageCustom, this);
            else
                target.Damage(BreathComputeDamage(), this);
        }

        public virtual int BreathComputeDamage()
        {
            int damage = (int)(HitsMax * FeatureList.BaseCreatureCombat.CreatureMaxHPBreathDamageScalar);

            return damage;
        }

        #endregion

        #region Spill Acid

        public void SpillAcid(int Amount)
        {
            SpillAcid(null, Amount);
        }

        public void SpillAcid(Mobile target, int Amount)
        {
            if ((target != null && target.Map == null) || this.Map == null)
                return;

            for (int i = 0; i < Amount; ++i)
            {
                Point3D loc = this.Location;
                Map map = this.Map;
                Item acid = NewHarmfulItem();

                if (target != null && target.Map != null && Amount == 1)
                {
                    loc = target.Location;
                    map = target.Map;
                }
                else
                {
                    bool validLocation = false;
                    for (int j = 0; !validLocation && j < 10; ++j)
                    {
                        loc = new Point3D(
                            loc.X + (Utility.Random(0, 3) - 2),
                            loc.Y + (Utility.Random(0, 3) - 2),
                            loc.Z);
                        loc.Z = map.GetAverageZ(loc.X, loc.Y);
                        validLocation = map.CanFit(loc, 16, false, false);
                    }
                }
                acid.MoveToWorld(loc, map);
            }
        }

        /*
            Solen Style, override me for other mobiles/items:
            kappa+acidslime, grizzles+whatever, etc.
        */

        public virtual Item NewHarmfulItem()
        {
            return new PoolOfAcid(TimeSpan.FromSeconds(10), 30, 30);
        }

        #endregion

        public BaseAI AIObject { get { return m_AI; } }

        public const int MaxOwners = 5;

        public override bool OnMoveOver(Mobile m)
        {
            if (m is BaseCreature && !m.Deleted && m.NetState == null)
            {
                if (!((BaseCreature)m).Controlled)
                {
                    return (!Alive || !m.Alive || IsDeadBondedPet || m.IsDeadBondedPet) || (Hidden && m.AccessLevel > AccessLevel.Player);
                }
            }

            return base.OnMoveOver(m);
        }

        public override int DetermineFollowersMax()
        {
            return FeatureList.Followers.DefaultBaseCreatureFollowers;
        }

        public virtual OppositionGroup OppositionGroup
        {
            get { return null; }
        }

        #region Friends
        public List<Mobile> Friends { get { return m_Friends; } }

        public virtual bool AllowNewPetFriend
        {
            get { return (m_Friends == null || m_Friends.Count < 5); }
        }

        public virtual bool IsPetFriend(Mobile m)
        {
            return (m_Friends != null && m_Friends.Contains(m));
        }

        public virtual void AddPetFriend(Mobile m)
        {
            if (m_Friends == null)
                m_Friends = new List<Mobile>();

            m_Friends.Add(m);
        }

        public virtual void RemovePetFriend(Mobile m)
        {
            if (m_Friends != null)
                m_Friends.Remove(m);
        }

        public virtual bool IsFriend(Mobile m)
        {
            OppositionGroup g = this.OppositionGroup;

            if (g != null && g.IsEnemy(this, m))
                return false;

            if (!(m is BaseCreature))
                return false;

            BaseCreature c = (BaseCreature)m;

            return (m_iTeam == c.m_iTeam && ((m_bSummoned || m_bControlled) == (c.m_bSummoned || c.m_bControlled))/* && c.Combatant != this */);
        }

        #endregion

        #region Allegiance
        public virtual Ethics.Ethic EthicAllegiance { get { return null; } }

        public enum Allegiance
        {
            None,
            Ally,
            Enemy
        }

        public virtual Allegiance GetFactionAllegiance(Mobile mob)
        {
            if (mob == null || mob.Map != Faction.Facet || FactionAllegiance == null)
                return Allegiance.None;

            Faction fac = Faction.Find(mob, true);

            if (fac == null)
                return Allegiance.None;

            return (fac == FactionAllegiance ? Allegiance.Ally : Allegiance.Enemy);
        }

        public virtual Allegiance GetEthicAllegiance(Mobile mob)
        {
            if (mob == null || mob.Map != Faction.Facet || EthicAllegiance == null)
                return Allegiance.None;

            Ethics.Ethic ethic = Ethics.Ethic.Find(mob, true);

            if (ethic == null)
                return Allegiance.None;

            return (ethic == EthicAllegiance ? Allegiance.Ally : Allegiance.Enemy);
        }

        #endregion

        public virtual bool IsEnemy(Mobile m)
        {
            bool foundEnemy = false;

            OppositionGroup opGroup = this.OppositionGroup;

            if (opGroup != null && opGroup.IsEnemy(this, m))
            {
                foundEnemy = true;
            }

            return foundEnemy;

            /*
            OppositionGroup g = this.OppositionGroup;

            if (g != null && g.IsEnemy(this, m))
            {               
                return true;
            }               

            if (m is BaseGuard)
                return false;

            if (GetFactionAllegiance(m) == Allegiance.Ally)
                return false;

            Ethics.Ethic ourEthic = EthicAllegiance;
            Ethics.Player pl = Ethics.Player.Find(m, true);

            if (pl != null && pl.IsShielded && (ourEthic == null || ourEthic == pl.Ethic))
                return false;

            if (!(m is BaseCreature))
            {
                m.Say("IsEnemy");
                return true;
            }

            BaseCreature c = (BaseCreature)m;

            return (m_iTeam != c.m_iTeam || ((m_bSummoned || m_bControlled) != (c.m_bSummoned || c.m_bControlled)) );
            */
        }

        public override string ApplyNameSuffix(string suffix)
        {
            if (IsParagon)
            {
                if (suffix.Length == 0)
                    suffix = "(Paragon)";
                else
                    suffix = String.Concat(suffix, " (Paragon)");
            }

            return base.ApplyNameSuffix(suffix);
        }

        public virtual bool CheckControlChance(Mobile m)
        {
            //If Both AnimalTaming and AnimalLore Exceed Minimum Taming Requirement, Control Success 
            double TargetTamingLore = MinTameSkill;

            //Summoned Creatures 100% Chance
            if (this.Summoned)
                return true;

            //Max 100 Taming / Lore Needed For Creatures (Some Creatures May 'require' more than 100 taming for difficulty purposes)
            if (TargetTamingLore > 100)
                TargetTamingLore = 100;

            if (m.Skills[SkillName.AnimalTaming].Value >= TargetTamingLore && m.Skills[SkillName.AnimalLore].Value >= TargetTamingLore)
            {
                return true;
            }

            //Insufficient AnimalTaming and AnimalLore
            else
            {
                /*
                PlaySound(GetAngerSound());

                if (Body.IsAnimal)
                    Animate(10, 5, 1, true, false, 0);
                else if (Body.IsMonster)
                    Animate(18, 5, 1, true, false, 0);
                 */
            }

            return false;

            /*
            if (GetControlChance(m) > Utility.RandomDouble())
            {
                Loyalty += 1;
                return true;
            }

            PlaySound(GetAngerSound());

            if (Body.IsAnimal)
                Animate(10, 5, 1, true, false, 0);
            else if (Body.IsMonster)
                Animate(18, 5, 1, true, false, 0);

            Loyalty -= 3;
            return false;
            */
        }

        public virtual bool CanBeControlledBy(Mobile m)
        {
            return (GetControlChance(m) > 0.0);
        }

        public double GetControlChance(Mobile m)
        {
            return GetControlChance(m, false);
        }

        public virtual double GetControlChance(Mobile m, bool useBaseSkill)
        {
            if (m_dMinTameSkill <= 29.1 || m_bSummoned || m.AccessLevel >= AccessLevel.GameMaster)
                return 1.0;

            double dMinTameSkill = m_dMinTameSkill;

            if (dMinTameSkill > -24.9 && Server.SkillHandlers.AnimalTaming.CheckMastery(m, this))
                dMinTameSkill = -24.9;

            int taming = (int)((useBaseSkill ? m.Skills[SkillName.AnimalTaming].Base : m.Skills[SkillName.AnimalTaming].Value) * 10);
            int lore = (int)((useBaseSkill ? m.Skills[SkillName.AnimalLore].Base : m.Skills[SkillName.AnimalLore].Value) * 10);
            int bonus = 0, chance = 700;

            int difficulty = (int)(dMinTameSkill * 10);
            int weighted = ((taming * 4) + lore) / 5;
            bonus = weighted - difficulty;

            if (bonus <= 0)
                bonus *= 14;
            else
                bonus *= 6;

            chance += bonus;

            if (chance >= 0 && chance < 200)
                chance = 200;
            else if (chance > 990)
                chance = 990;

            chance -= (MaxLoyalty - m_Loyalty) * 10;

            return ((double)chance / 1000);
        }

        public override bool Move(Direction d)
        {
            if (this.Deleted || this.NetState == null)
            {
                return base.Move(d);
            }

            NetState ns = this.NetState;

            TimeSpan speed = ComputeMovementSpeed(d);

            bool res;
            res = base.Move(d);

            if (!res)
                return false;

            m_NextMovementTime += speed;
            return true;
        }

        private static Type[] m_AnimateDeadTypes = new Type[]
            {
                typeof( HellSteed ), typeof( SkeletalMount ),
                typeof( Wraith ), typeof( SkeletalDragon ),
                typeof( LichLord ), typeof( Lich ),
                typeof( SkeletalKnight ), typeof( BoneKnight ), typeof( Mummy ),
                typeof( SkeletalMage ), typeof( BoneMagi )
            };

        public virtual bool IsAnimatedDead
        {
            get
            {
                if (!Summoned)
                    return false;

                Type type = this.GetType();

                bool contains = false;

                for (int i = 0; !contains && i < m_AnimateDeadTypes.Length; ++i)
                    contains = (type == m_AnimateDeadTypes[i]);

                return contains;
            }
        }

        public virtual bool IsNecroFamiliar
        {
            get
            {
                if (!Summoned)
                    return false;

                return false;
            }
        }

        public override void Damage(int amount, Mobile from)
        {
            int oldHits = this.Hits;

            if (Core.AOS && !this.Summoned && this.Controlled && 0.2 > Utility.RandomDouble())
                amount = (int)(amount * BonusPetDamageScalar);


            base.Damage(amount, from);

            if (SubdueBeforeTame && !Controlled)
            {
                if ((oldHits > (this.HitsMax / 10)) && (this.Hits <= (this.HitsMax / 10)))
                    PublicOverheadMessage(MessageType.Regular, 0x3B2, false, "* The creature has been beaten into subjugation! *");
            }
        }

        public override void CalculateEquippedArmor()
        {
            double rating = 0.0;

            // This, I believe, is the source of the polymorph => 0 AR bug
            //if (this.Body.IsHuman)
            //{
            AddArmorRating(ref rating, NeckArmor);
            AddArmorRating(ref rating, HandArmor);
            AddArmorRating(ref rating, HeadArmor);
            AddArmorRating(ref rating, ArmsArmor);
            AddArmorRating(ref rating, LegsArmor);
            AddArmorRating(ref rating, ChestArmor);

            BaseArmor shield = ShieldArmor as BaseArmor;

            if (shield != null)
            {
                double arShield = FeatureList.ArmorChanges.ShieldARWithoutParry * shield.ArmorRating;
                double arShieldSkill = FeatureList.ArmorChanges.ShieldARParryBonus * (Skills.Parry.Value / 100) * shield.ArmorRating;

                rating += arShield;
                rating += arShieldSkill;
            }
            //}

            EquipmentArmor = rating;
            Delta(MobileDelta.Armor);
        }

        private void AddArmorRating(ref double rating, Item armor)
        {
            BaseArmor ar = armor as BaseArmor;

            if (ar != null && (ar.ArmorAttributes.MageArmor == 0))
            {
                rating += ar.ArmorRating;
            }
        }

        public virtual bool DeleteCorpseOnDeath
        {
            get
            {
                return !Core.AOS && m_bSummoned;
            }
        }

        public override void SetLocation(Point3D newLocation, bool isTeleport)
        {
            base.SetLocation(newLocation, isTeleport);

            if (isTeleport && m_AI != null)
                m_AI.OnTeleported();
        }

        public override void OnBeforeSpawn(Point3D location, Map m)
        {
            if (Paragon.CheckConvert(this, location, m))
                IsParagon = true;

            base.OnBeforeSpawn(location, m);
        }

        public override ApplyPoisonResult ApplyPoison(Mobile from, Poison poison)
        {
            if (!Alive || IsDeadPet)
                return ApplyPoisonResult.Immune;

            ApplyPoisonResult result = base.ApplyPoison(from, poison);

            if (from != null && result == ApplyPoisonResult.Poisoned && PoisonTimer is PoisonImpl.PoisonTimer)
                (PoisonTimer as PoisonImpl.PoisonTimer).From = from;

            return result;
        }

        public override bool CheckPoisonImmunity(Mobile from, Poison poison)
        {
            if (base.CheckPoisonImmunity(from, poison))
                return true;

            Poison p = this.PoisonCustomImmune;
            if (p == null) p = this.PoisonImmune;

            return (p != null && p.Level >= poison.Level);
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Loyalty
        {
            get
            {
                return m_Loyalty;
            }
            set
            {
                m_Loyalty = Math.Min(Math.Max(value, 0), MaxLoyalty);
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public WayPoint CurrentWayPoint
        {
            get
            {
                return m_CurrentWayPoint;
            }
            set
            {
                if (m_CurrentWayPoint == value) { return; }

                if (m_CurrentWayPoint != null) { m_CurrentWayPoint.Unsubscribe(this); }
                m_CurrentWayPoint = value;
                if (m_CurrentWayPoint != null)
                {
                    m_CurrentWayPoint.Subscribe(this);
                    if (ReturnsHome == true) // if assigned a waypoint, don't return home anymore
                    {
                        ReturnsHome = false;
                    }
                }
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public IPoint2D TargetLocation
        {
            get
            {
                return m_TargetLocation;
            }
            set
            {
                m_TargetLocation = value;
                if (value != null && ReturnsHome == true) { ReturnsHome = false; }
            }
        }

        public virtual Mobile ConstantFocus { get { return null; } }

        public virtual bool DisallowAllMoves
        {
            get
            {
                return false;
            }
        }

        public virtual bool InitialInnocent
        {
            get
            {
                return false;
            }
        }

        public virtual bool AlwaysMurderer
        {
            get
            {
                return false;
            }
        }

        public virtual bool AlwaysAttackable
        {
            get
            {
                return false;
            }
        }

        public override void OnNetStateChanged()
        {
            // pseudoseers logging out--don't make them disappear!
            if (this.m_LogoutTimer != null)
            {
                this.m_LogoutTimer.Stop();
            }
            this.m_LogoutTimer = null;
        }

        protected bool m_Speaks = true;
        [CommandProperty(AccessLevel.GameMaster)]
        public bool Speaks
        {
            get { return m_Speaks; }
            set { m_Speaks = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public virtual int DamageMin { get { return m_DamageMin; } set { m_DamageMin = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public virtual int DamageMax { get { return m_DamageMax; } set { m_DamageMax = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public override int HitsMax
        {
            get
            {
                if (m_HitsMax > 0)
                {
                    //int value = m_HitsMax + GetStatOffset( StatType.Str );
                    int value = m_HitsMax;

                    if (value < 1)
                        value = 1;
                    else if (value > 65000)
                        value = 65000;

                    return value;
                }

                return Str;
            }
        }


        [CommandProperty(AccessLevel.GameMaster)]
        public virtual bool ReturnsHome
        {
            get { return m_ReturnsHome; }
            set { m_ReturnsHome = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int HitsMaxSeed
        {
            get { return m_HitsMax; }
            set { m_HitsMax = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public override int StamMax
        {
            get
            {
                if (m_StamMax > 0)
                {
                    //int value = m_StamMax + GetStatOffset(StatType.Dex);
                    int value = m_StamMax + GetStatOffset(StatType.Dex);

                    if (value < 1)
                        value = 1;
                    else if (value > 65000)
                        value = 65000;

                    return value;
                }

                return Dex;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int StamMaxSeed
        {
            get { return m_StamMax; }
            set { m_StamMax = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public override int ManaMax
        {
            get
            {
                if (m_ManaMax > 0)
                {
                    //int value = m_ManaMax + GetStatOffset(StatType.Int);
                    int value = m_ManaMax + GetStatOffset(StatType.Int);

                    if (value < 1)
                        value = 1;
                    else if (value > 65000)
                        value = 65000;

                    return value;
                }

                return Int;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int ManaMaxSeed
        {
            get { return m_ManaMax; }
            set { m_ManaMax = value; }
        }

        public virtual bool CanOpenDoors
        {
            get
            {
                return !this.Body.IsAnimal && !this.Body.IsSea;
            }
        }

        public virtual bool CanMoveOverObstacles
        {
            get
            {
                return this.Body.IsMonster;
            }
        }

        public virtual bool CanDestroyObstacles
        {
            get
            {
                // to enable breaking of furniture, 'return CanMoveOverObstacles;'
                return true;
            }
        }

        public void Unpacify()
        {
            BardEndTime = DateTime.Now;
            BardPacified = false;
        }

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            LastDamageAmount = amount;
            if (BardPacified)
            {
                BardEndTime -= TimeSpan.FromSeconds(1.0);
            }

            int disruptThreshold;
            //NPCs can use bandages too!
            disruptThreshold = 0;

            if (from != null && from.Player)
                disruptThreshold = 18;
            else
                disruptThreshold = 25;

            if (amount > disruptThreshold)
            {
                BandageContext c = BandageContext.GetContext(this);

                if (c != null)
                    c.Slip();
            }

            InhumanSpeech speechType = this.SpeechType;

            if (speechType != null && !willKill && Speaks)
                speechType.OnDamage(this, amount);

            if (willKill && from is PlayerMobile)
                Timer.DelayCall(TimeSpan.FromSeconds(10), new TimerCallback(((PlayerMobile)from).RecoverAmmo));

            XmlAttach.CheckOnHit(this, from);
            base.OnDamage(amount, from, willKill);
        }

        public virtual void OnDamagedBySpell(Mobile from)
        {
        }

        public virtual void OnHarmfulSpell(Mobile from)
        {
        }

        #region Alter[...]Damage From/To

        public virtual void AlterDamageScalarFrom(Mobile caster, ref double scalar)
        {
        }

        public virtual void AlterDamageScalarTo(Mobile target, ref double scalar)
        {
        }

        public virtual void AlterSpellDamageFrom(Mobile from, ref int damage)
        {
        }

        public virtual void AlterSpellDamageTo(Mobile to, ref int damage)
        {
        }

        public virtual void AlterMeleeDamageFrom(Mobile from, ref int damage)
        {
        }

        public virtual void AlterMeleeDamageTo(Mobile to, ref int damage)
        {
        }

        #endregion

        public virtual void CheckReflect(Mobile caster, ref bool reflect)
        {
        }

        public virtual void OnCarve(Mobile from, Corpse corpse, Item with)
        {
            int feathers = Feathers;
            int wool = Wool;
            int meat = Meat;
            int hides = Hides;
            int scales = Scales;

            if ((feathers == 0 && wool == 0 && meat == 0 && hides == 0 && scales == 0) || Summoned || IsBonded || corpse.Animated)
            {
                if (corpse.Animated)
                    corpse.SendLocalizedMessageTo(from, 500464);	// Use this on corpses to carve away meat and hide
                else
                    from.SendLocalizedMessage(500485); // You see nothing useful to carve from the corpse.
            }
            else
            {
                if (corpse.Map == Map.Felucca)
                {
                    feathers = (int)(feathers * 1.68);
                    wool = (int)(wool * 1.68);
                    hides = (int)(hides * 1.68);

                }

                //Citizenship bonus if applicable
                PlayerMobile pm = from as PlayerMobile;
                if (pm != null && pm.CitizenshipPlayerState != null)
                {
                    CommonwealthResourceBonus crb = pm.CitizenshipPlayerState.GetResourceBonus(ResourceBonusTypes.Hunting);

                    if (crb != null)
                    {
                        Console.WriteLine("{0} has recieved {1} of feathers, meat or wool because they are a citizen of ", from.Name, crb.HarvestBonus, pm.CitizenshipPlayerState.Commonwealth.Definition.TownName);
                        feathers += crb.HarvestBonus;
                        wool += crb.HarvestBonus;
                        hides += crb.HarvestBonus;

                    }
                }

                if (pm != null)
                {
                    pm.HidesSkinnedThisSession += hides;
                    pm.FeathersPluckedThisSession += feathers;
                }

                new Blood(0x122D).MoveToWorld(corpse.Location, corpse.Map);

                if (feathers != 0)
                {
                    corpse.AddCarvedItem(new Feather(feathers), from);
                    from.SendLocalizedMessage(500479); // You pluck the bird. The feathers are now on the corpse.
                }

                if (wool != 0)
                {
                    corpse.AddCarvedItem(new Wool(wool), from);
                    from.SendLocalizedMessage(500483); // You shear it, and the wool is now on the corpse.
                }

                if (meat != 0)
                {
                    if (MeatType == MeatType.Ribs)
                        corpse.AddCarvedItem(new RawRibs(meat), from);
                    else if (MeatType == MeatType.Bird)
                        corpse.AddCarvedItem(new RawBird(meat), from);
                    else if (MeatType == MeatType.LambLeg)
                        corpse.AddCarvedItem(new RawLambLeg(meat), from);

                    from.SendLocalizedMessage(500467); // You carve some meat, which remains on the corpse.
                }

                if (hides != 0)
                {
                    Item holding = from.Weapon as Item;
                    if (Core.AOS && (holding is SkinningKnife /* TODO: || holding is ButcherWarCleaver || with is ButcherWarCleaver */ ))
                    {
                        Item leather = null;

                        switch (HideType)
                        {
                            case HideType.Regular: leather = new Leather(hides); break;
                            case HideType.Spined: leather = new SpinedLeather(hides); break;
                            case HideType.Horned: leather = new HornedLeather(hides); break;
                            case HideType.Barbed: leather = new BarbedLeather(hides); break;
                        }

                        if (leather != null)
                        {
                            if (!from.PlaceInBackpack(leather))
                            {
                                corpse.DropItem(leather);
                                from.SendLocalizedMessage(500471); // You skin it, and the hides are now in the corpse.
                            }
                            else
                                from.SendLocalizedMessage(1073555); // You skin it and place the cut-up hides in your backpack.
                        }
                    }
                    else
                    {
                        if (HideType == HideType.Regular)
                            corpse.DropItem(new Hides(hides));
                        else if (HideType == HideType.Spined)
                            corpse.DropItem(new SpinedHides(hides));
                        else if (HideType == HideType.Horned)
                            corpse.DropItem(new HornedHides(hides));
                        else if (HideType == HideType.Barbed)
                            corpse.DropItem(new BarbedHides(hides));

                        from.SendLocalizedMessage(500471); // You skin it, and the hides are now in the corpse.
                    }
                }

                corpse.Carved = true;

                if (corpse.IsCriminalAction(from))
                    from.CriminalAction(true, false); //carving corpses does not result in guard whack
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)31); // version

            writer.Write((int)m_CurrentAI);
            writer.Write((int)m_DefaultAI);

            writer.Write((int)m_iRangePerception);
            writer.Write((int)m_iRangeFight);

            writer.Write((int)m_iTeam);

            writer.Write((double)m_dActiveSpeed);
            writer.Write((double)m_dPassiveSpeed);
            writer.Write((double)m_dCurrentSpeed);

            writer.Write((int)m_pHome.X);
            writer.Write((int)m_pHome.Y);
            writer.Write((int)m_pHome.Z);

            writer.Write((bool)m_SuperPredator);
            writer.Write((bool)m_Predator);
            writer.Write((bool)m_Prey);

            // Version 1
            writer.Write((int)m_iRangeHome);

            int i = 0;

            writer.Write((int)m_arSpellAttack.Count);
            for (i = 0; i < m_arSpellAttack.Count; i++)
            {
                writer.Write(m_arSpellAttack[i].ToString());
            }

            writer.Write((int)m_arSpellDefense.Count);
            for (i = 0; i < m_arSpellDefense.Count; i++)
            {
                writer.Write(m_arSpellDefense[i].ToString());
            }

            // Version 2
            writer.Write((int)m_FightMode);

            writer.Write((bool)m_bControlled);
            writer.Write((Mobile)m_ControlMaster);
            writer.Write((Mobile)m_ControlTarget);
            writer.Write((Point3D)m_ControlDest);
            writer.Write((int)m_ControlOrder);
            writer.Write((double)m_dMinTameSkill);
            // Removed in version 9
            //writer.Write( (double) m_dMaxTameSkill );
            writer.Write((bool)m_bTamable);
            writer.Write((bool)m_bSummoned);

            if (m_bSummoned)
                writer.WriteDeltaTime(m_SummonEnd);

            writer.Write((int)m_iControlSlots);

            // Version 3
            writer.Write((int)m_Loyalty);

            // Version 4
            writer.Write(m_CurrentWayPoint);

            // Verison 5
            writer.Write(m_SummonMaster);

            // Version 6
            writer.Write((int)m_HitsMax);
            writer.Write((int)m_StamMax);
            writer.Write((int)m_ManaMax);
            writer.Write((int)m_DamageMin);
            writer.Write((int)m_DamageMax);

            // Version 7
            writer.Write((int)m_PhysicalResistance);
            writer.Write((int)m_PhysicalDamage);

            writer.Write((int)m_FireResistance);
            writer.Write((int)m_FireDamage);

            writer.Write((int)m_ColdResistance);
            writer.Write((int)m_ColdDamage);

            writer.Write((int)m_PoisonResistance);
            writer.Write((int)m_PoisonDamage);

            writer.Write((int)m_EnergyResistance);
            writer.Write((int)m_EnergyDamage);

            // Version 8
            writer.Write(m_Owners, true);

            // Version 10
            writer.Write((bool)m_IsDeadPet);
            writer.Write((bool)m_IsBonded);
            writer.Write((DateTime)m_BondingBegin);
            writer.Write((DateTime)m_OwnerAbandonTime);

            // Version 11
            writer.Write((bool)m_HasGeneratedLoot);

            // Version 12
            writer.Write((bool)m_Paragon);

            // Version 13
            writer.Write((bool)(m_Friends != null && m_Friends.Count > 0));

            if (m_Friends != null && m_Friends.Count > 0)
                writer.Write(m_Friends, true);

            // Version 14
            writer.Write((bool)m_RemoveIfUntamed);
            writer.Write((int)m_RemoveStep);

            // Version 17
            if (IsStabled || (Controlled && ControlMaster != null))
                writer.Write(TimeSpan.Zero);
            else
                writer.Write(DeleteTimeLeft);

            // Version 18
            writer.Write((bool)false);  // dummy, for compatibility

            // Version 19
            writer.Write((ulong)m_TeamFlags);

            // Version 20
            writer.Write((bool)m_KillCriminals);

            // Version 21
            writer.Write((bool)m_KillMurderers);

            // Version 22
            writer.Write((bool)m_InnocentDefault);

            // Version 23
            writer.Write((bool)m_GuardImmune);

            // Version 24
            writer.Write((bool)m_Pseu_KeepKillCredit);

            // Version 25
            writer.Write((double)m_PoisonCustomChance);
            if (m_PoisonCustomImmune == null) writer.Write((int)-1); else writer.Write((int)m_PoisonCustomImmune.Level);
            if (m_PoisonCustomHit == null) writer.Write((int)-1); else writer.Write((int)m_PoisonCustomHit.Level);
            writer.Write((bool)m_CanBreathCustom);
            writer.Write((int)m_BreathDamageCustom);

            // Version 26
            writer.Write((double)m_BreathCustomDelay);

            // Version 27
            writer.Write((TimeSpan)m_Pseu_SpellDelay);
            writer.Write((bool)m_BardImmuneCustom);
            writer.Write((bool)m_Pseu_EQPlayerAllowed);
            writer.Write((bool)m_Pseu_AllowFizzle);
            writer.Write((bool)m_Pseu_AllowInterrupts);
            writer.Write((bool)m_Pseu_TeleWallDelay);
            writer.Write((bool)m_Pseu_CanBeHealed);

            // Version 28
            writer.Write((bool)m_WeaponDamage);
            writer.Write((bool)m_TakesNormalDamage);

            // Version 29
            writer.Write((bool)m_PowerWords);
            writer.Write((bool)m_ClearHandsOnCast);

            // Version 30
            writer.Write((bool)m_Pseu_ConsumeReagents);

            // Version 31
            writer.Write((bool)m_Pseu_SpellBookRequired);
        }

        private static double[] m_StandardActiveSpeeds = new double[]
            {
                0.175, 0.1, 0.15, 0.2, 0.25, 0.3, 0.4, 0.5, 0.6, 0.8
            };

        private static double[] m_StandardPassiveSpeeds = new double[]
            {
                0.350, 0.2, 0.4, 0.5, 0.6, 0.8, 1.0, 1.2, 1.6, 2.0
            };

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            m_CurrentAI = (AIType)reader.ReadInt();
            m_DefaultAI = (AIType)reader.ReadInt();

            m_iRangePerception = reader.ReadInt();
            m_iRangeFight = reader.ReadInt();

            m_iTeam = reader.ReadInt();

            m_dActiveSpeed = reader.ReadDouble();
            m_dPassiveSpeed = reader.ReadDouble();
            m_dCurrentSpeed = reader.ReadDouble();

            if (m_iRangePerception == OldRangePerception)
                m_iRangePerception = DefaultRangePerception;

            m_pHome.X = reader.ReadInt();
            m_pHome.Y = reader.ReadInt();
            m_pHome.Z = reader.ReadInt();

            m_SuperPredator = reader.ReadBool();
            m_Predator = reader.ReadBool();
            m_Prey = reader.ReadBool();

            if (version >= 1)
            {
                m_iRangeHome = reader.ReadInt();

                int i, iCount;

                iCount = reader.ReadInt();
                for (i = 0; i < iCount; i++)
                {
                    string str = reader.ReadString();
                    Type type = Type.GetType(str);

                    if (type != null)
                    {
                        m_arSpellAttack.Add(type);
                    }
                }

                iCount = reader.ReadInt();
                for (i = 0; i < iCount; i++)
                {
                    string str = reader.ReadString();
                    Type type = Type.GetType(str);

                    if (type != null)
                    {
                        m_arSpellDefense.Add(type);
                    }
                }
            }
            else
            {
                m_iRangeHome = 0;
            }

            if (version >= 2)
            {
                m_FightMode = (FightMode)reader.ReadInt();

                m_bControlled = reader.ReadBool();
                m_ControlMaster = reader.ReadMobile();
                m_ControlTarget = reader.ReadMobile();
                m_ControlDest = reader.ReadPoint3D();
                m_ControlOrder = (OrderType)reader.ReadInt();

                m_dMinTameSkill = reader.ReadDouble();

                if (version < 9)
                    reader.ReadDouble();

                m_bTamable = reader.ReadBool();
                m_bSummoned = reader.ReadBool();

                if (m_bSummoned)
                {
                    m_SummonEnd = reader.ReadDeltaTime();
                    new UnsummonTimer(m_ControlMaster, this, m_SummonEnd - DateTime.Now).Start();
                }

                m_iControlSlots = reader.ReadInt();
            }
            else
            {
                m_FightMode = FightMode.Closest;

                m_bControlled = false;
                m_ControlMaster = null;
                m_ControlTarget = null;
                m_ControlOrder = OrderType.None;
            }

            if (version >= 3)
                m_Loyalty = reader.ReadInt();
            else
                m_Loyalty = MaxLoyalty; // Wonderfully Happy

            if (version >= 4)
                CurrentWayPoint = reader.ReadItem() as WayPoint; // changed to setter so that it subscribes to the waypoint

            if (version >= 5)
                m_SummonMaster = reader.ReadMobile();

            if (version >= 6)
            {
                m_HitsMax = reader.ReadInt();
                m_StamMax = reader.ReadInt();
                m_ManaMax = reader.ReadInt();
                m_DamageMin = reader.ReadInt();
                m_DamageMax = reader.ReadInt();
            }

            if (version >= 7)
            {
                m_PhysicalResistance = reader.ReadInt();
                m_PhysicalDamage = reader.ReadInt();

                m_FireResistance = reader.ReadInt();
                m_FireDamage = reader.ReadInt();

                m_ColdResistance = reader.ReadInt();
                m_ColdDamage = reader.ReadInt();

                m_PoisonResistance = reader.ReadInt();
                m_PoisonDamage = reader.ReadInt();

                m_EnergyResistance = reader.ReadInt();
                m_EnergyDamage = reader.ReadInt();
            }

            if (version >= 8)
                m_Owners = reader.ReadStrongMobileList();
            else
                m_Owners = new List<Mobile>();

            if (version >= 10)
            {
                m_IsDeadPet = reader.ReadBool();
                m_IsBonded = reader.ReadBool();
                m_BondingBegin = reader.ReadDateTime();
                m_OwnerAbandonTime = reader.ReadDateTime();
            }

            if (version >= 11)
                m_HasGeneratedLoot = reader.ReadBool();
            else
                m_HasGeneratedLoot = true;

            if (version >= 12)
                m_Paragon = reader.ReadBool();
            else
                m_Paragon = false;

            if (version >= 13 && reader.ReadBool())
                m_Friends = reader.ReadStrongMobileList();
            else if (version < 13 && m_ControlOrder >= OrderType.Unfriend)
                ++m_ControlOrder;

            if (version < 16 && Loyalty != MaxLoyalty)
                Loyalty *= 10;

            double activeSpeed = m_dActiveSpeed;
            double passiveSpeed = m_dPassiveSpeed;

            SpeedInfo.GetSpeeds(this, ref activeSpeed, ref passiveSpeed);

            bool isStandardActive = false;
            for (int i = 0; !isStandardActive && i < m_StandardActiveSpeeds.Length; ++i)
                isStandardActive = (m_dActiveSpeed == m_StandardActiveSpeeds[i]);

            bool isStandardPassive = false;
            for (int i = 0; !isStandardPassive && i < m_StandardPassiveSpeeds.Length; ++i)
                isStandardPassive = (m_dPassiveSpeed == m_StandardPassiveSpeeds[i]);

            if (isStandardActive && m_dCurrentSpeed == m_dActiveSpeed)
                m_dCurrentSpeed = activeSpeed;
            else if (isStandardPassive && m_dCurrentSpeed == m_dPassiveSpeed)
                m_dCurrentSpeed = passiveSpeed;

            if (isStandardActive && !m_Paragon)
                m_dActiveSpeed = activeSpeed;

            if (isStandardPassive && !m_Paragon)
                m_dPassiveSpeed = passiveSpeed;

            if (version >= 14)
            {
                m_RemoveIfUntamed = reader.ReadBool();
                m_RemoveStep = reader.ReadInt();
            }

            TimeSpan deleteTime = TimeSpan.Zero;

            if (version >= 17)
                deleteTime = reader.ReadTimeSpan();

            if (deleteTime > TimeSpan.Zero || LastOwner != null && !Controlled && !IsStabled)
            {
                if (deleteTime == TimeSpan.Zero)
                    deleteTime = TimeSpan.FromDays(3.0);

                m_DeleteTimer = new DeleteTimer(this, deleteTime);
                m_DeleteTimer.Start();
            }

            if (version >= 18)
            {
                reader.ReadBool();      // dummy, for campatibility
            }
            if (version >= 19)
            {
                m_TeamFlags = reader.ReadULong();
            }
            else
            {
                AITeamList.SetDefaultTeam(this);
            }
            if (version >= 20)
            {
                m_KillCriminals = reader.ReadBool();
            }
            if (version >= 21)
            {
                m_KillMurderers = reader.ReadBool();
            }
            if (version >= 22)
            {
                m_InnocentDefault = reader.ReadBool();
            }
            if (version >= 23)
            {
                m_GuardImmune = reader.ReadBool();
            }
            if (version >= 24)
            {
                m_Pseu_KeepKillCredit = reader.ReadBool();
            }
            if (version >= 25)
            {
                // Version 25
                m_PoisonCustomChance = reader.ReadDouble();
                m_PoisonCustomImmune = Poison.GetPoison(reader.ReadInt());
                m_PoisonCustomHit = Poison.GetPoison(reader.ReadInt());
                m_CanBreathCustom = reader.ReadBool();
                m_BreathDamageCustom = reader.ReadInt();
            }
            if (version >= 26)
            {
                m_BreathCustomDelay = reader.ReadDouble();
            }
            if (version >= 27)
            {
                m_Pseu_SpellDelay = reader.ReadTimeSpan();
                m_BardImmuneCustom = reader.ReadBool();
                m_Pseu_EQPlayerAllowed = reader.ReadBool();
                m_Pseu_AllowFizzle = reader.ReadBool();
                m_Pseu_AllowInterrupts = reader.ReadBool();
                m_Pseu_TeleWallDelay = reader.ReadBool();
                m_Pseu_CanBeHealed = reader.ReadBool();
            }
            if (version >= 28)
            {
                m_WeaponDamage = reader.ReadBool();
                m_TakesNormalDamage = reader.ReadBool();
            }
            if (version >= 29)
            {
                m_PowerWords = reader.ReadBool();
                m_ClearHandsOnCast = reader.ReadBool();
            }
            if (version >= 30)
            {
                m_Pseu_ConsumeReagents = reader.ReadBool();
            }
            if (version >= 31)
            {
                m_Pseu_SpellBookRequired = reader.ReadBool();
            }

            if (version <= 14 && m_Paragon && Hue == 0x31)
            {
                Hue = Paragon.Hue; //Paragon hue fixed, should now be 0x501.
            }

            CheckStatTimers();

            ChangeAIType(m_CurrentAI);

            AddFollowers();

            AICreatureList.GetAI(this);
            UpdateAI();
        }

        public virtual bool IsHumanInTown()
        {
            return (Body.IsHuman && Region.IsPartOf(typeof(Regions.GuardedRegion)));
        }

        public virtual bool CheckGold(Mobile from, Item dropped)
        {
            if (dropped is Gold)
                return OnGoldGiven(from, (Gold)dropped);

            return false;
        }

        public virtual bool OnGoldGiven(Mobile from, Gold dropped)
        {
            if (CheckTeachingMatch(from))
            {
                if (Teach(m_Teaching, from, dropped.Amount, true))
                {
                    dropped.Delete();
                    return true;
                }
            }
            else if (IsHumanInTown())
            {
                Direction = GetDirectionTo(from);

                int oldSpeechHue = this.SpeechHue;

                this.SpeechHue = 0x23F;
                SayTo(from, "Thou art giving me gold?");

                if (dropped.Amount >= 400)
                    SayTo(from, "'Tis a noble gift.");
                else
                    SayTo(from, "Money is always welcome.");

                this.SpeechHue = 0x3B2;
                SayTo(from, 501548); // I thank thee.

                this.SpeechHue = oldSpeechHue;

                dropped.Delete();
                return true;
            }

            return false;
        }

        public override bool ShouldCheckStatTimers { get { return false; } }

        #region Food
        private static Type[] m_Eggs = new Type[]
            {
                typeof( FriedEggs ), typeof( Eggs )
            };

        private static Type[] m_Fish = new Type[]
            {
                typeof( FishSteak ), typeof( RawFishSteak )
            };

        private static Type[] m_GrainsAndHay = new Type[]
            {
                typeof( BreadLoaf ), typeof( FrenchBread ), typeof( SheafOfHay )
            };

        private static Type[] m_Meat = new Type[]
            {
				/* Cooked */
				typeof( Bacon ), typeof( CookedBird ), typeof( Sausage ),
                typeof( Ham ), typeof( Ribs ), typeof( LambLeg ),
                typeof( ChickenLeg ),

				/* Uncooked */
				typeof( RawBird ), typeof( RawRibs ), typeof( RawLambLeg ),
                typeof( RawChickenLeg ),

				/* Body Parts */
				typeof( Head ), typeof( LeftArm ), typeof( LeftLeg ),
                typeof( Torso ), typeof( RightArm ), typeof( RightLeg )
            };

        private static Type[] m_FruitsAndVegies = new Type[]
            {
                typeof( HoneydewMelon ), typeof( YellowGourd ), typeof( GreenGourd ),
                typeof( Banana ), typeof( Bananas ), typeof( Lemon ), typeof( Lime ),
                typeof( Dates ), typeof( Grapes ), typeof( Peach ), typeof( Pear ),
                typeof( Apple ), typeof( Watermelon ), typeof( Squash ),
                typeof( Cantaloupe ), typeof( Carrot ), typeof( Cabbage ),
                typeof( Onion ), typeof( Lettuce ), typeof( Pumpkin )
            };

        private static Type[] m_Gold = new Type[]
            {
				// white wyrms eat gold..
				typeof( Gold )
            };

        public virtual bool CheckFoodPreference(Item f)
        {
            if (CheckFoodPreference(f, FoodType.Eggs, m_Eggs))
                return true;

            if (CheckFoodPreference(f, FoodType.Fish, m_Fish))
                return true;

            if (CheckFoodPreference(f, FoodType.GrainsAndHay, m_GrainsAndHay))
                return true;

            if (CheckFoodPreference(f, FoodType.Meat, m_Meat))
                return true;

            if (CheckFoodPreference(f, FoodType.FruitsAndVegies, m_FruitsAndVegies))
                return true;

            if (CheckFoodPreference(f, FoodType.Gold, m_Gold))
                return true;

            return false;
        }

        public virtual bool CheckFoodPreference(Item fed, FoodType type, Type[] types)
        {
            if ((FavoriteFood & type) == 0)
                return false;

            Type fedType = fed.GetType();
            bool contains = false;

            for (int i = 0; !contains && i < types.Length; ++i)
                contains = (fedType == types[i]);

            return contains;
        }

        public virtual bool CheckFeed(Mobile from, Item dropped)
        {
            if (!IsDeadPet && Controlled && (ControlMaster == from || IsPetFriend(from)) && (dropped is Food || dropped is Gold || dropped is CookableFood || dropped is Head || dropped is LeftArm || dropped is LeftLeg || dropped is Torso || dropped is RightArm || dropped is RightLeg))
            {
                Item f = dropped;

                if (CheckFoodPreference(f))
                {
                    int amount = f.Amount;

                    if (amount > 0)
                    {
                        int stamGain;

                        if (f is Gold)
                            stamGain = amount - 50;
                        else
                            stamGain = (amount * 15) - 50;

                        if (stamGain > 0)
                            Stam += stamGain;

                        if (Core.SE)
                        {
                            if (m_Loyalty < MaxLoyalty)
                            {
                                m_Loyalty = MaxLoyalty;
                            }
                        }
                        else
                        {
                            for (int i = 0; i < amount; ++i)
                            {
                                if (m_Loyalty < MaxLoyalty && 0.5 >= Utility.RandomDouble())
                                {
                                    m_Loyalty += 10;
                                }
                            }
                        }

                        /* if ( happier )*/
                        // looks like in OSI pets say they are happier even if they are at maximum loyalty
                        SayTo(from, 502060); // Your pet looks happier.

                        if (Body.IsAnimal)
                            Animate(3, 5, 1, true, false, 0);
                        else if (Body.IsMonster)
                            Animate(17, 5, 1, true, false, 0);

                        if (IsBondable && !IsBonded)
                        {
                            Mobile master = m_ControlMaster;

                            if (master != null && master == from)	//So friends can't start the bonding process
                            {
                                if (m_dMinTameSkill <= 29.1 || master.Skills[SkillName.AnimalTaming].Base >= m_dMinTameSkill || OverrideBondingReqs() || (Core.ML && master.Skills[SkillName.AnimalTaming].Value >= m_dMinTameSkill))
                                {
                                    if (BondingBegin == DateTime.MinValue)
                                    {
                                        BondingBegin = DateTime.Now;
                                    }
                                    else if ((BondingBegin + BondingDelay) <= DateTime.Now)
                                    {
                                        IsBonded = true;
                                        BondingBegin = DateTime.MinValue;
                                        from.SendLocalizedMessage(1049666); // Your pet has bonded with you!
                                    }
                                }
                                else if (Core.ML)
                                {
                                    from.SendLocalizedMessage(1075268); // Your pet cannot form a bond with you until your animal taming ability has risen.
                                }
                            }
                        }

                        dropped.Delete();
                        return true;
                    }
                }
            }

            return false;
        }

        #endregion

        public virtual bool OverrideBondingReqs()
        {
            return false;
        }

        public virtual bool CanAngerOnTame { get { return false; } }

        #region OnAction[...]

        public virtual void OnActionWander()
        {
        }

        public virtual void OnActionCombat()
        {
        }

        public virtual void OnActionGuard()
        {
        }

        public virtual void OnActionFlee()
        {
        }

        public virtual void OnActionInteract()
        {
        }

        public virtual void OnActionBackoff()
        {
        }

        #endregion

        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            if (CheckFeed(from, dropped))
                return true;
            else if (CheckGold(from, dropped))
                return true;

            return base.OnDragDrop(from, dropped);
        }

        protected virtual BaseAI ForcedAI { get { return null; } }

        public void ChangeAIType(AIType NewAI)
        {
            if (m_AI != null)
                m_AI.m_Timer.Stop();

            if (ForcedAI != null)
            {
                m_AI = ForcedAI;
                return;
            }

            m_AI = null;

            switch (NewAI)
            {
                case AIType.AI_Generic:
                    m_AI = new GenericAI(this);
                    break;

                case AIType.AI_Melee:
                    m_AI = new MeleeAI(this);
                    break;
                case AIType.AI_Animal:
                    m_AI = new AnimalAI(this);
                    break;
                case AIType.AI_Berserk:
                    m_AI = new BerserkAI(this);
                    break;
                case AIType.AI_Archer:
                    m_AI = new ArcherAI(this);
                    break;
                case AIType.AI_Healer:
                    m_AI = new HealerAI(this);
                    break;
                case AIType.AI_Vendor:
                    m_AI = new VendorAI(this);
                    break;
                case AIType.AI_Mage:
                    m_AI = new MageAI(this);
                    break;
                case AIType.AI_Predator:
                    //m_AI = new PredatorAI(this);
                    m_AI = new MeleeAI(this);
                    break;
                case AIType.AI_Thief:
                    m_AI = new ThiefAI(this);
                    break;
            }
        }

        public void ChangeAIToDefault()
        {
            ChangeAIType(m_DefaultAI);
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public AIType AI
        {
            get
            {
                return m_CurrentAI;
            }
            set
            {
                m_CurrentAI = value;

                if (m_CurrentAI == AIType.AI_Use_Default)
                {
                    m_CurrentAI = m_DefaultAI;
                }

                ChangeAIType(m_CurrentAI);
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Debug
        {
            get
            {
                return m_bDebugAI;
            }
            set
            {
                m_bDebugAI = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Team
        {
            get
            {
                return m_iTeam;
            }
            set
            {
                m_iTeam = value;

                OnTeamChange();
            }
        }

        public virtual void OnTeamChange()
        {
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile FocusMob
        {
            get
            {
                return m_FocusMob;
            }
            set
            {
                m_FocusMob = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public FightMode FightMode
        {
            get
            {
                return m_FightMode;
            }
            set
            {
                m_FightMode = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int RangePerception
        {
            get
            {
                return m_iRangePerception;
            }
            set
            {
                m_iRangePerception = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int RangeFight
        {
            get
            {
                return m_iRangeFight;
            }
            set
            {
                m_iRangeFight = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int RangeHome
        {
            get
            {
                return m_iRangeHome;
            }
            set
            {
                m_iRangeHome = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public double ActiveSpeed
        {
            get
            {
                return m_dActiveSpeed;
            }
            set
            {
                m_dActiveSpeed = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public double PassiveSpeed
        {
            get
            {
                return m_dPassiveSpeed;
            }
            set
            {
                m_dPassiveSpeed = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public double CurrentSpeed
        {
            get
            {
                if (m_TargetLocation != null)
                    return 0.3;

                return m_dCurrentSpeed;
            }
            set
            {
                if (m_dCurrentSpeed != value)
                {
                    m_dCurrentSpeed = value;

                    if (m_AI != null)
                        m_AI.OnCurrentSpeedChanged();
                }
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D Home
        {
            get
            {
                if (m_pHome.X == 0 && m_pHome.Y == 0)
                {
                    m_pHome = this.Location;
                }

                return m_pHome;
            }

            set
            {
                m_pHome = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D LastEnemyLocation
        {
            get
            {

                return m_LastEnemyLocation;
            }

            set
            {
                m_LastEnemyLocation = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Controlled
        {
            get
            {
                return m_bControlled;
            }
            set
            {
                if (m_bControlled == value)
                    return;

                m_bControlled = value;
                Delta(MobileDelta.Noto);

                InvalidateProperties();
            }
        }

        public override void RevealingAction()
        {
            Spells.Sixth.InvisibilitySpell.RemoveTimer(this);

            IsStealthing = false;

            base.RevealingAction();
        }

        public void RemoveFollowers()
        {
            if (m_ControlMaster != null)
            {
                m_ControlMaster.Followers -= ControlSlots;
                if (m_ControlMaster is PlayerMobile)
                {
                    ((PlayerMobile)m_ControlMaster).AllFollowers.Remove(this);
                    if (((PlayerMobile)m_ControlMaster).AutoStabled.Contains(this))
                        ((PlayerMobile)m_ControlMaster).AutoStabled.Remove(this);
                }
            }
            else if (m_SummonMaster != null)
            {
                m_SummonMaster.Followers -= ControlSlots;
                if (m_SummonMaster is PlayerMobile)
                {
                    ((PlayerMobile)m_SummonMaster).AllFollowers.Remove(this);
                }
            }

            if (m_ControlMaster != null && m_ControlMaster.Followers < 0)
                m_ControlMaster.Followers = 0;

            if (m_SummonMaster != null && m_SummonMaster.Followers < 0)
                m_SummonMaster.Followers = 0;
        }

        public void AddFollowers()
        {
            if (m_ControlMaster != null)
            {
                m_ControlMaster.Followers += ControlSlots;
                if (m_ControlMaster is PlayerMobile)
                {
                    ((PlayerMobile)m_ControlMaster).AllFollowers.Add(this);
                }
            }
            else if (m_SummonMaster != null)
            {
                m_SummonMaster.Followers += ControlSlots;
                if (m_SummonMaster is PlayerMobile)
                {
                    ((PlayerMobile)m_SummonMaster).AllFollowers.Add(this);
                }
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile ControlMaster
        {
            get
            {
                return m_ControlMaster;
            }
            set
            {
                if (m_ControlMaster == value || this == value)
                    return;

                RemoveFollowers();
                m_ControlMaster = value;
                AddFollowers();

                if (m_ControlMaster != null)
                    StopDeleteTimer();

                Delta(MobileDelta.Noto);
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile SummonMaster
        {
            get
            {
                return m_SummonMaster;
            }
            set
            {
                if (m_SummonMaster == value || this == value)
                    return;

                RemoveFollowers();
                m_SummonMaster = value;
                AddFollowers();

                Delta(MobileDelta.Noto);
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile SpellTarget
        {
            get
            {
                return m_SpellTarget;
            }
            set
            {
                m_SpellTarget = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile HealTarget
        {
            get
            {
                return m_HealTarget;
            }
            set
            {
                m_HealTarget = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile ControlTarget
        {
            get
            {
                return m_ControlTarget;
            }
            set
            {
                m_ControlTarget = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D ControlDest
        {
            get
            {
                return m_ControlDest;
            }
            set
            {
                m_ControlDest = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public OrderType ControlOrder
        {
            get
            {
                return m_ControlOrder;
            }

            set
            {
                m_ControlOrder = value;

                if (m_AI != null)
                    m_AI.OnCurrentOrderChanged();

                InvalidateProperties();

                if (m_ControlMaster != null)
                    m_ControlMaster.InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool BardProvoked
        {
            get
            {
                return m_bBardProvoked;
            }
            set
            {
                m_bBardProvoked = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool BardPacified
        {
            get
            {
                return m_bBardPacified;
            }
            set
            {
                m_bBardPacified = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile BardMaster
        {
            get
            {
                return m_bBardMaster;
            }
            set
            {
                m_bBardMaster = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile BardTarget
        {
            get
            {
                return m_bBardTarget;
            }
            set
            {
                m_bBardTarget = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime BardEndTime
        {
            get
            {
                return m_timeBardEnd;
            }
            set
            {
                m_timeBardEnd = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public double MinTameSkill
        {
            get
            {
                return m_dMinTameSkill;
            }
            set
            {
                m_dMinTameSkill = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Tamable
        {
            get
            {
                return m_bTamable && !m_Paragon;
            }
            set
            {
                m_bTamable = value;
            }
        }

        [CommandProperty(AccessLevel.Administrator)]
        public bool Summoned
        {
            get
            {
                return m_bSummoned;
            }
            set
            {
                if (m_bSummoned == value)
                    return;

                m_NextReacquireTime = DateTime.Now;

                m_bSummoned = value;
                Delta(MobileDelta.Noto);

                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.Administrator)]
        public int ControlSlots
        {
            get
            {
                return m_iControlSlots;
            }
            set
            {
                m_iControlSlots = value;
            }
        }

        public virtual bool NoHouseRestrictions { get { return false; } }
        public virtual bool IsHouseSummonable { get { return false; } }

        #region Corpse Resources
        public virtual int Feathers { get { return 0; } }
        public virtual int Wool { get { return 0; } }

        public virtual MeatType MeatType { get { return MeatType.Ribs; } }
        public virtual int Meat { get { return 0; } }

        public virtual int Hides { get { return 0; } }
        public virtual HideType HideType { get { return HideType.Regular; } }

        public virtual int Scales { get { return 0; } }
        public virtual ScaleType ScaleType { get { return ScaleType.Red; } }
        #endregion

        public virtual bool AutoDispel { get { return false; } }
        public virtual double AutoDispelChance { get { return ((Core.SE) ? .10 : 1.0); } }

        public virtual bool IsScaryToPets { get { return false; } }
        public virtual bool IsScaredOfScaryThings { get { return true; } }

        public virtual bool CanRummageCorpses { get { return false; } }

        public virtual void OnGotMeleeAttack(Mobile attacker)
        {
            if (AutoDispel && attacker is BaseCreature && ((BaseCreature)attacker).IsDispellable && AutoDispelChance > Utility.RandomDouble())
                Dispel(attacker);
        }

        public virtual void Dispel(Mobile m)
        {
            Effects.SendLocationParticles(EffectItem.Create(m.Location, m.Map, EffectItem.DefaultDuration), 0x3728, 8, 20, 5042);
            Effects.PlaySound(m, m.Map, 0x201);

            m.Delete();
        }

        public virtual bool DeleteOnRelease { get { return m_bSummoned; } }

        public virtual void OnGaveMeleeAttack(Mobile defender)
        {
            Poison p = PoisonCustomHit;
            if (p == null) p = HitPoison;
            double hitPoisonChance = PoisonCustomChance;
            if (hitPoisonChance == -1.0) { hitPoisonChance = HitPoisonChance; }

            if (m_Paragon)
                p = PoisonImpl.IncreaseLevel(p);

            if (p != null && hitPoisonChance >= Utility.RandomDouble())
            {
                if (OrangePetals.UnderEffect(defender))
                {
                    defender.LocalOverheadMessage(MessageType.Emote, 0x3F, true,
                        "* You feel yourself resisting the effects of the poison *");

                    defender.NonlocalOverheadMessage(MessageType.Emote, 0x3F, true,
                        String.Format("* {0} seems resistant to the poison *", defender.Name));
                }
                else
                {
                    double poisonDelay = 0;

                    //If New Poison Hit Allowable
                    if (DateTime.Now >= this.NextPoisonEffectAllowed)
                    {
                        defender.ApplyPoison(this, p);

                        poisonDelay = FeatureList.Poison.TamedCreaturesPoisonCooldownPerLevel * ((double)p.Level + 1);

                        //If Tamed Creature and Tamed Creature Poison Cooldown Mechanic is Active
                        if (this.Controlled && this.ControlMaster != null && this.ControlMaster.Player && FeatureList.Poison.TamedCreaturesPoisonCooldown)
                        {

                            this.NextPoisonEffectAllowed = DateTime.Now + TimeSpan.FromSeconds(poisonDelay);
                        }

                        //If Normal Creature and Creature Poison Cooldown Mechanic is Active
                        else if (FeatureList.Poison.CreaturesPoisonCooldown)
                        {
                            this.NextPoisonEffectAllowed = DateTime.Now + TimeSpan.FromSeconds(poisonDelay);
                        }
                    }

                    if (this.Controlled)
                        this.CheckSkill(SkillName.Poisoning, 0, this.Skills[SkillName.Poisoning].Cap);
                }
            }

            if (AutoDispel && defender is BaseCreature && ((BaseCreature)defender).IsDispellable && AutoDispelChance > Utility.RandomDouble())
                Dispel(defender);
        }

        public override void OnAfterDelete()
        {
            if (m_AI != null)
            {
                if (m_AI.m_Timer != null)
                    m_AI.m_Timer.Stop();

                m_AI = null;
            }

            if (m_DeleteTimer != null)
            {
                m_DeleteTimer.Stop();
                m_DeleteTimer = null;
            }

            FocusMob = null;

            base.OnAfterDelete();
        }

        public void DebugSay(string text)
        {
            if (m_bDebugAI)
                this.PublicOverheadMessage(MessageType.Regular, 41, false, text);
        }

        public void DebugSay(string format, params object[] args)
        {
            if (m_bDebugAI)
                this.PublicOverheadMessage(MessageType.Regular, 41, false, String.Format(format, args));
        }

        /*
         * This function can be overriden.. so a "Strongest" mobile, can have a different definition depending
         * on who check for value
         * -Could add a FightMode.Prefered
         *
         */

        public virtual double GetFightModeRanking(Mobile m, FightMode acqType, bool bPlayerOnly)
        {
            if ((bPlayerOnly && m.Player) || !bPlayerOnly)
            {
                switch (acqType)
                {
                    case FightMode.Strongest:
                        return (m.Skills[SkillName.Tactics].Value + m.Str); //returns strongest mobile

                    case FightMode.Weakest:
                        return -m.Hits; // returns weakest mobile

                    default:
                        return -GetDistanceToSqrt(m); // returns closest mobile
                }
            }
            else
            {
                return double.MinValue;
            }
        }

        // Turn, - for left, + for right
        // Basic for now, needs work
        public virtual void Turn(int iTurnSteps)
        {
            int v = (int)Direction;

            Direction = (Direction)((((v & 0x7) + iTurnSteps) & 0x7) | (v & 0x80));
        }

        public virtual void TurnInternal(int iTurnSteps)
        {
            int v = (int)Direction;

            SetDirection((Direction)((((v & 0x7) + iTurnSteps) & 0x7) | (v & 0x80)));
        }

        public bool IsHurt()
        {
            return (Hits != HitsMax);
        }

        public double GetHomeDistance()
        {
            return GetDistanceToSqrt(m_pHome);
        }

        public virtual int GetTeamSize(int iRange)
        {
            int iCount = 0;

            foreach (Mobile m in this.GetMobilesInRange(iRange))
            {
                if (m is BaseCreature)
                {
                    if (((BaseCreature)m).Team == Team)
                    {
                        if (!m.Deleted)
                        {
                            if (m != this)
                            {
                                if (CanSee(m))
                                {
                                    iCount++;
                                }
                            }
                        }
                    }
                }
            }

            return iCount;
        }

        private class TameEntry : ContextMenuEntry
        {
            private BaseCreature m_Mobile;

            public TameEntry(Mobile from, BaseCreature creature)
                : base(6130, 6)
            {
                m_Mobile = creature;

                Enabled = Enabled && (from.Female ? creature.AllowFemaleTamer : creature.AllowMaleTamer);
            }

            public override void OnClick()
            {
                if (!Owner.From.CheckAlive())
                    return;

                Owner.From.TargetLocked = true;
                SkillHandlers.AnimalTaming.DisableMessage = true;

                if (Owner.From.UseSkill(SkillName.AnimalTaming))
                    Owner.From.Target.Invoke(Owner.From, m_Mobile);

                SkillHandlers.AnimalTaming.DisableMessage = false;
                Owner.From.TargetLocked = false;
            }
        }

        #region Teaching
        public virtual bool CanTeach { get { return false; } }

        public virtual bool CheckTeach(SkillName skill, Mobile from)
        {
            if (!CanTeach)
                return false;

            if (skill == SkillName.Stealth && from.Skills[SkillName.Hiding].Base < ((Core.SE) ? 50.0 : 80.0))
                return false;

            if (!Core.AOS && (skill == SkillName.Focus || skill == SkillName.Chivalry || skill == SkillName.Necromancy))
                return false;

            return true;
        }

        public enum TeachResult
        {
            Success,
            Failure,
            KnowsMoreThanMe,
            KnowsWhatIKnow,
            SkillNotRaisable,
            NotEnoughFreePoints
        }

        public virtual TeachResult CheckTeachSkills(SkillName skill, Mobile m, int maxPointsToLearn, ref int pointsToLearn, bool doTeach)
        {
            if (!CheckTeach(skill, m) || !m.CheckAlive())
                return TeachResult.Failure;

            Skill ourSkill = Skills[skill];
            Skill theirSkill = m.Skills[skill];

            if (ourSkill == null || theirSkill == null)
                return TeachResult.Failure;

            int baseToSet = ourSkill.BaseFixedPoint / 3;

            if (baseToSet > 420)
                baseToSet = 420;
            else if (baseToSet < 200)
                return TeachResult.Failure;

            if (baseToSet > theirSkill.CapFixedPoint)
                baseToSet = theirSkill.CapFixedPoint;

            pointsToLearn = baseToSet - theirSkill.BaseFixedPoint;

            if (maxPointsToLearn > 0 && pointsToLearn > maxPointsToLearn)
            {
                pointsToLearn = maxPointsToLearn;
                baseToSet = theirSkill.BaseFixedPoint + pointsToLearn;
            }

            if (pointsToLearn < 0)
                return TeachResult.KnowsMoreThanMe;

            if (pointsToLearn == 0)
                return TeachResult.KnowsWhatIKnow;

            if (theirSkill.Lock != SkillLock.Up)
                return TeachResult.SkillNotRaisable;

            int freePoints = m.Skills.Cap - m.Skills.Total;
            int freeablePoints = 0;

            if (freePoints < 0)
                freePoints = 0;

            for (int i = 0; (freePoints + freeablePoints) < pointsToLearn && i < m.Skills.Length; ++i)
            {
                Skill sk = m.Skills[i];

                if (sk == theirSkill || sk.Lock != SkillLock.Down)
                    continue;

                freeablePoints += sk.BaseFixedPoint;
            }

            if ((freePoints + freeablePoints) == 0)
                return TeachResult.NotEnoughFreePoints;

            if ((freePoints + freeablePoints) < pointsToLearn)
            {
                pointsToLearn = freePoints + freeablePoints;
                baseToSet = theirSkill.BaseFixedPoint + pointsToLearn;
            }

            if (doTeach)
            {
                int need = pointsToLearn - freePoints;

                for (int i = 0; need > 0 && i < m.Skills.Length; ++i)
                {
                    Skill sk = m.Skills[i];

                    if (sk == theirSkill || sk.Lock != SkillLock.Down)
                        continue;

                    if (sk.BaseFixedPoint < need)
                    {
                        need -= sk.BaseFixedPoint;
                        sk.BaseFixedPoint = 0;
                    }
                    else
                    {
                        sk.BaseFixedPoint -= need;
                        need = 0;
                    }
                }

                /* Sanity check */
                if (baseToSet > theirSkill.CapFixedPoint || (m.Skills.Total - theirSkill.BaseFixedPoint + baseToSet) > m.Skills.Cap)
                    return TeachResult.NotEnoughFreePoints;

                theirSkill.BaseFixedPoint = baseToSet;
            }

            return TeachResult.Success;
        }

        public virtual bool CheckTeachingMatch(Mobile m)
        {
            if (m_Teaching == (SkillName)(-1))
                return false;

            if (m is PlayerMobile)
                return (((PlayerMobile)m).Learning == m_Teaching);

            return true;
        }

        private SkillName m_Teaching = (SkillName)(-1);

        public virtual bool Teach(SkillName skill, Mobile m, int maxPointsToLearn, bool doTeach)
        {
            int pointsToLearn = 0;
            TeachResult res = CheckTeachSkills(skill, m, maxPointsToLearn, ref pointsToLearn, doTeach);

            switch (res)
            {
                case TeachResult.KnowsMoreThanMe:
                    {
                        Say(501508); // I cannot teach thee, for thou knowest more than I!
                        break;
                    }
                case TeachResult.KnowsWhatIKnow:
                    {
                        Say(501509); // I cannot teach thee, for thou knowest all I can teach!
                        break;
                    }
                case TeachResult.NotEnoughFreePoints:
                case TeachResult.SkillNotRaisable:
                    {
                        // Make sure this skill is marked to raise. If you are near the skill cap (700 points) you may need to lose some points in another skill first.
                        m.SendLocalizedMessage(501510, "", 0x22);
                        break;
                    }
                case TeachResult.Success:
                    {
                        if (doTeach)
                        {
                            Say(501539); // Let me show thee something of how this is done.
                            m.SendLocalizedMessage(501540); // Your skill level increases.

                            m_Teaching = (SkillName)(-1);

                            if (m is PlayerMobile)
                                ((PlayerMobile)m).Learning = (SkillName)(-1);
                        }
                        else
                        {
                            // I will teach thee all I know, if paid the amount in full.  The price is:
                            Say(1019077, AffixType.Append, String.Format(" {0}", pointsToLearn), "");
                            Say(1043108); // For less I shall teach thee less.

                            m_Teaching = skill;

                            if (m is PlayerMobile)
                                ((PlayerMobile)m).Learning = skill;
                        }

                        return true;
                    }
            }

            return false;
        }

        #endregion


        public override void AggressiveAction(Mobile aggressor, bool criminal)
        {
            if (aggressor == null)
                return;

            if (aggressor.Deleted)
                return;

            if (aggressor == this)
                return;

            if (this.Blessed)
                return;

            if (aggressor.Blessed)
                return;

            if (m_AI == null)
                return;

            Mobile controller = this.ControlMaster as Mobile;

            base.AggressiveAction(aggressor, criminal);

            if (this.ControlMaster != null)
                if (NotorietyHandlers.CheckAggressor(this.ControlMaster.Aggressors, aggressor))
                    aggressor.Aggressors.Add(AggressorInfo.Create(this, aggressor, true));

            m_AI.OnAggressiveAction(aggressor);
        }

        public virtual void AddCustomContextEntries(Mobile from, List<ContextMenuEntry> list)
        {
        }

        public virtual bool CanDrop { get { return IsBonded; } }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);

            if (m_AI != null && Commandable)
                m_AI.GetContextMenuEntries(from, list);

            if (m_bTamable && !m_bControlled && from.Alive)
                list.Add(new TameEntry(from, this));

            AddCustomContextEntries(from, list);

            if (CanTeach && from.Alive)
            {
                Skills ourSkills = this.Skills;
                Skills theirSkills = from.Skills;

                for (int i = 0; i < ourSkills.Length && i < theirSkills.Length; ++i)
                {
                    Skill skill = ourSkills[i];
                    Skill theirSkill = theirSkills[i];

                    if (skill != null && theirSkill != null && skill.Base >= 60.0 && CheckTeach(skill.SkillName, from))
                    {
                        double toTeach = skill.Base / 3.0;

                        if (toTeach > 42.0)
                            toTeach = 42.0;

                        list.Add(new TeachEntry((SkillName)i, this, from, (toTeach > theirSkill.Base)));
                    }
                }
            }
        }

        public override bool HandlesOnSpeech(Mobile from)
        {
            InhumanSpeech speechType = this.SpeechType;

            if (speechType != null && (speechType.Flags & IHSFlags.OnSpeech) != 0 && from.InRange(this, 3))
                return true;

            return (m_AI != null && m_AI.HandlesOnSpeech(from) && from.InRange(this, m_iRangePerception));
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            InhumanSpeech speechType = this.SpeechType;

            if (speechType != null && speechType.OnSpeech(this, e.Mobile, e.Speech))
                e.Handled = true;

            else if (!e.Handled && m_AI != null && e.Mobile.InRange(this, m_iRangePerception))
                m_AI.OnSpeech(e);
        }

        public override bool IsHarmfulCriminal(Mobile target)
        {
            if ((Controlled && target == m_ControlMaster) || (Summoned && target == m_SummonMaster))
                return false;

            if (target is BaseCreature && ((BaseCreature)target).InitialInnocent && !((BaseCreature)target).Controlled)
                return false;

            if (target is PlayerMobile && ((PlayerMobile)target).PermaFlags.Count > 0)
                return false;

            return base.IsHarmfulCriminal(target);
        }

        public override void CriminalAction(bool message, bool isGuardCallingAction)
        {
            base.CriminalAction(message, isGuardCallingAction);

            if (Controlled || Summoned)
            {
                if (m_ControlMaster != null && m_ControlMaster.Player)
                    m_ControlMaster.CriminalAction(false, true);
                else if (m_SummonMaster != null && m_SummonMaster.Player)
                    m_SummonMaster.CriminalAction(false, true);
            }
        }

        public override void DoHarmful(Mobile target, bool indirect)
        {
            base.DoHarmful(target, indirect);

            if (target == this || target == m_ControlMaster || target == m_SummonMaster || (!Controlled && !Summoned))
                return;

            List<AggressorInfo> list = this.Aggressors;

            for (int i = 0; i < list.Count; ++i)
            {
                AggressorInfo ai = list[i];

                if (ai.Attacker == target)
                    return;
            }

            list = this.Aggressed;

            for (int i = 0; i < list.Count; ++i)
            {
                AggressorInfo ai = list[i];

                if (ai.Defender == target)
                {
                    if (m_ControlMaster != null && m_ControlMaster.Player && m_ControlMaster.CanBeHarmful(target, false))
                        m_ControlMaster.DoHarmful(target, true);
                    else if (m_SummonMaster != null && m_SummonMaster.Player && m_SummonMaster.CanBeHarmful(target, false))
                        m_SummonMaster.DoHarmful(target, true);

                    return;
                }
            }
        }

        private static Mobile m_NoDupeGuards;

        public void ReleaseGuardDupeLock()
        {
            m_NoDupeGuards = null;
        }

        public void ReleaseGuardLock()
        {
            EndAction(typeof(GuardedRegion));
        }

        private DateTime m_IdleReleaseTime;

        public virtual bool CheckIdle()
        {
            if (Combatant != null)
                return false; // in combat.. not idling

            if (m_IdleReleaseTime > DateTime.MinValue)
            {
                // idling...

                if (DateTime.Now >= m_IdleReleaseTime)
                {
                    m_IdleReleaseTime = DateTime.MinValue;
                    return false; // idle is over
                }

                return true; // still idling
            }

            if (95 > Utility.Random(100))
                return false; // not idling, but don't want to enter idle state

            m_IdleReleaseTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 15));

            if (Body.IsHuman)
            {
                switch (Utility.Random(2))
                {
                    case 0: Animate(5, 5, 1, true, true, 1); break;
                    case 1: Animate(6, 5, 1, true, false, 1); break;
                }
            }
            else if (Body.IsAnimal)
            {
                switch (Utility.Random(3))
                {
                    case 0: Animate(3, 3, 1, true, false, 1); break;
                    case 1: Animate(9, 5, 1, true, false, 1); break;
                    case 2: Animate(10, 5, 1, true, false, 1); break;
                }
            }
            else if (Body.IsMonster)
            {
                switch (Utility.Random(2))
                {
                    case 0: Animate(17, 5, 1, true, false, 1); break;
                    case 1: Animate(18, 5, 1, true, false, 1); break;
                }
            }

            PlaySound(GetIdleSound());
            return true; // entered idle state
        }

        protected override void OnLocationChange(Point3D oldLocation)
        {
            Map map = this.Map;

            if (PlayerRangeSensitive && m_AI != null && map != null && map != Map.Internal) // && map.GetSector(this.Location).Active)  // for some reason this makes it so when subgrouped (not subgoup 0), mobs do not start their AI going.. hmm
                m_AI.Activate();

            base.OnLocationChange(oldLocation);
        }

        protected override bool OnMove(Direction d)
        {
            if (this.Deleted) return true;

            if (this.NetState == null)
            {
                if (Hidden)
                {
                    if (CheckStealthSkillForRevealingAction())
                    {
                        RevealingAction();
                    }

                    else
                    {
                        double footprintChance = Utility.RandomDouble();

                        if (footprintChance < FeatureList.BaseCreatureAI.StealthFootprintChance)
                        {
                            new Footsteps(d).MoveToWorld(this.Location, this.Map);
                        }
                    }
                }

                return true;
            }
            // Otherwise, it's a pseudoseer controlled mob, so treat it like a player's stealth
            bool running = (d & Direction.Running) != 0;

            if (running)
            {
                // technically would check if stamina has been lost for too many running steps
            }

            if (Hidden && DesignContext.Find(this) == null)	//Hidden & NOT customizing a house
            {
                if (!Mounted)
                {
                    if (running || CheckStealthSkillForRevealingAction())
                    {
                        RevealingAction();
                    }
                }

                else
                {
                    RevealingAction();
                }
            }
            return true;
        }

        private bool CheckStealthSkillForRevealingAction()
        {
            if (!m_IsStealthing)
                return true;

            //Creature Has Stealth Steps Available (No Check)           
            if (AllowedStealthSteps > 0)
            {
                AllowedStealthSteps--;
                return false;
            }

            //No Free Steps Remaining
            else
            {
                double BaseSuccessPercent = FeatureList.SkillChanges.StealthStepSuccessBasePercent;
                double BonusSuccessPerecent = FeatureList.SkillChanges.StealthStepSkillBonusDivider;

                double chance = (BaseSuccessPercent + (this.Skills[SkillName.Stealth].Value / BonusSuccessPerecent)) / 100;

                if (chance >= Utility.RandomDouble())
                {
                    return false;
                }
            }

            return true;
        }

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (ReacquireOnMovement || m_Paragon)
                ForceReacquire();

            InhumanSpeech speechType = this.SpeechType;

            if (speechType != null && Speaks)
                speechType.OnMovement(this, m, oldLocation);

            /* Begin notice sound */
            if ((!m.Hidden || m.AccessLevel == AccessLevel.Player) && m.Player && m_FightMode != FightMode.Aggressor && m_FightMode != FightMode.None && Combatant == null && !Controlled && !Summoned)
            {
                // If this creature defends itself but doesn't actively attack (animal) or
                // doesn't fight at all (vendor) then no notice sounds are played..
                // So, players are only notified of aggressive monsters

                // Monsters that are currently fighting are ignored

                // Controlled or summoned creatures are ignored

                if (InRange(m.Location, 18) && !InRange(oldLocation, 18))
                {
                    if (Body.IsMonster)
                        Animate(11, 5, 1, true, false, 1);

                    PlaySound(GetAngerSound());
                }
            }
            /* End notice sound */

            if (m_NoDupeGuards == m)
                return;

            if (!Body.IsHuman || Kills >= 5 || AlwaysMurderer || AlwaysAttackable || m.Kills < 5 || !m.InRange(Location, 12) || !m.Alive || m.Hidden)
                return;

            GuardedRegion guardedRegion = (GuardedRegion)this.Region.GetRegion(typeof(GuardedRegion));

            if (guardedRegion != null)
            {
                if (!guardedRegion.IsDisabled() && guardedRegion.IsGuardCandidate(m) && BeginAction(typeof(GuardedRegion)))
                {
                    Say(1013037 + Utility.Random(16));
                    guardedRegion.CallGuards(this.Location);

                    Timer.DelayCall(TimeSpan.FromSeconds(5.0), new TimerCallback(ReleaseGuardLock));

                    m_NoDupeGuards = m;
                    Timer.DelayCall(TimeSpan.Zero, new TimerCallback(ReleaseGuardDupeLock));
                }
            }
        }

        public void AddSpellAttack(Type type)
        {
            m_arSpellAttack.Add(type);
        }

        public void AddSpellDefense(Type type)
        {
            m_arSpellDefense.Add(type);
        }

        public Spell GetAttackSpellRandom()
        {
            if (m_arSpellAttack.Count > 0)
            {
                Type type = m_arSpellAttack[Utility.Random(m_arSpellAttack.Count)];

                object[] args = { this, null };
                return Activator.CreateInstance(type, args) as Spell;
            }
            else
            {
                return null;
            }
        }

        public Spell GetDefenseSpellRandom()
        {
            if (m_arSpellDefense.Count > 0)
            {
                Type type = m_arSpellDefense[Utility.Random(m_arSpellDefense.Count)];

                object[] args = { this, null };
                return Activator.CreateInstance(type, args) as Spell;
            }
            else
            {
                return null;
            }
        }

        public Spell GetSpellSpecific(Type type)
        {
            int i;

            for (i = 0; i < m_arSpellAttack.Count; i++)
            {
                if (m_arSpellAttack[i] == type)
                {
                    object[] args = { this, null };
                    return Activator.CreateInstance(type, args) as Spell;
                }
            }

            for (i = 0; i < m_arSpellDefense.Count; i++)
            {
                if (m_arSpellDefense[i] == type)
                {
                    object[] args = { this, null };
                    return Activator.CreateInstance(type, args) as Spell;
                }
            }

            return null;
        }

        #region Set[...]

        public void SetDamage(int val)
        {
            m_DamageMin = val;
            m_DamageMax = val;
        }

        public void SetDamage(int min, int max)
        {
            m_DamageMin = min;
            m_DamageMax = max;
        }

        public void SetHits(int val)
        {
            m_HitsMax = val;
            Hits = HitsMax;

            /*
			if ( val < 1000 && !Core.AOS )
				val = (val * 100) / 60;

			m_HitsMax = val;
			Hits = HitsMax;
            */
        }

        public void SetHits(int min, int max)
        {
            m_HitsMax = Utility.RandomMinMax(min, max);
            Hits = HitsMax;

            /*
			if ( min < 1000 && !Core.AOS )
			{
				min = (min * 100) / 60;
				max = (max * 100) / 60;
			}

			m_HitsMax = Utility.RandomMinMax( min, max );
			Hits = HitsMax;
            */
        }

        public void SetStam(int val)
        {
            m_StamMax = val;
            Stam = StamMax;
        }

        public void SetStam(int min, int max)
        {
            m_StamMax = Utility.RandomMinMax(min, max);
            Stam = StamMax;
        }

        public void SetMana(int val)
        {
            m_ManaMax = val;
            Mana = ManaMax;
        }

        public void SetMana(int min, int max)
        {
            m_ManaMax = Utility.RandomMinMax(min, max);
            Mana = ManaMax;
        }

        public void SetStr(int val)
        {
            RawStr = val;
            Hits = HitsMax;
        }

        public void SetStr(int min, int max)
        {
            RawStr = Utility.RandomMinMax(min, max);
            Hits = HitsMax;
        }

        public void SetDex(int val)
        {
            RawDex = val;
            Stam = StamMax;
        }

        public void SetDex(int min, int max)
        {
            RawDex = Utility.RandomMinMax(min, max);
            Stam = StamMax;
        }

        public void SetInt(int val)
        {
            RawInt = val;
            Mana = ManaMax;
        }

        public void SetInt(int min, int max)
        {
            RawInt = Utility.RandomMinMax(min, max);
            Mana = ManaMax;
        }

        public void SetDamageType(ResistanceType type, int min, int max)
        {
            SetDamageType(type, Utility.RandomMinMax(min, max));
        }

        public void SetDamageType(ResistanceType type, int val)
        {
            switch (type)
            {
                case ResistanceType.Physical: m_PhysicalDamage = val; break;
                case ResistanceType.Fire: m_FireDamage = val; break;
                case ResistanceType.Cold: m_ColdDamage = val; break;
                case ResistanceType.Poison: m_PoisonDamage = val; break;
                case ResistanceType.Energy: m_EnergyDamage = val; break;
            }
        }

        public void SetResistance(ResistanceType type, int min, int max)
        {
            SetResistance(type, Utility.RandomMinMax(min, max));
        }

        public void SetResistance(ResistanceType type, int val)
        {
            switch (type)
            {
                case ResistanceType.Physical: m_PhysicalResistance = val; break;
                case ResistanceType.Fire: m_FireResistance = val; break;
                case ResistanceType.Cold: m_ColdResistance = val; break;
                case ResistanceType.Poison: m_PoisonResistance = val; break;
                case ResistanceType.Energy: m_EnergyResistance = val; break;
            }

            UpdateResistances();
        }

        public void SetSkill(SkillName name, double val)
        {
            Skills[name].BaseFixedPoint = (int)(val * 10);

            if (Skills[name].Base > Skills[name].Cap)
            {
                if (Core.SE)
                    this.SkillsCap += (Skills[name].BaseFixedPoint - Skills[name].CapFixedPoint);

                Skills[name].Cap = Skills[name].Base;
            }
        }

        public void SetSkill(SkillName name, double min, double max)
        {
            int minFixed = (int)(min * 10);
            int maxFixed = (int)(max * 10);

            Skills[name].BaseFixedPoint = Utility.RandomMinMax(minFixed, maxFixed);

            if (Skills[name].Base > Skills[name].Cap)
            {
                if (Core.SE)
                    this.SkillsCap += (Skills[name].BaseFixedPoint - Skills[name].CapFixedPoint);

                Skills[name].Cap = Skills[name].Base;
            }
        }

        public void SetFameLevel(int level)
        {
            switch (level)
            {
                case 1: Fame = Utility.RandomMinMax(0, 1249); break;
                case 2: Fame = Utility.RandomMinMax(1250, 2499); break;
                case 3: Fame = Utility.RandomMinMax(2500, 4999); break;
                case 4: Fame = Utility.RandomMinMax(5000, 9999); break;
                case 5: Fame = Utility.RandomMinMax(10000, 10000); break;
            }
        }

        public void SetKarmaLevel(int level)
        {
            switch (level)
            {
                case 0: Karma = -Utility.RandomMinMax(0, 624); break;
                case 1: Karma = -Utility.RandomMinMax(625, 1249); break;
                case 2: Karma = -Utility.RandomMinMax(1250, 2499); break;
                case 3: Karma = -Utility.RandomMinMax(2500, 4999); break;
                case 4: Karma = -Utility.RandomMinMax(5000, 9999); break;
                case 5: Karma = -Utility.RandomMinMax(10000, 10000); break;
            }
        }

        #endregion

        public static void Cap(ref int val, int min, int max)
        {
            if (val < min)
                val = min;
            else if (val > max)
                val = max;
        }

        #region Pack & Loot

        public void PackPotion()
        {
            PackItem(Loot.RandomPotion());
        }

        public void PackScroll(int minCircle, int maxCircle)
        {
            PackScroll(Utility.RandomMinMax(minCircle, maxCircle));
        }

        public void PackScroll(int circle)
        {
            int min = (circle - 1) * 8;

            PackItem(Loot.RandomScroll(min, min + 7, SpellbookType.Regular));
        }
        //TODO add in crafting artifact 
        public void PackCraftingArtifact(int level)
        {
            throw new NotImplementedException();
        }

        public virtual void DropBackpack()
        {
            if (Backpack != null)
            {
                if (Backpack.Items.Count > 0)
                {
                    Backpack b = new CreatureBackpack(Name);

                    List<Item> list = new List<Item>(Backpack.Items);
                    foreach (Item item in list)
                    {
                        b.DropItem(item);
                    }

                    BaseHouse house = BaseHouse.FindHouseAt(this);
                    if (house != null)
                        b.MoveToWorld(house.BanLocation, house.Map);
                    else
                        b.MoveToWorld(Location, Map);
                }
            }
        }

        protected bool m_Spawning;
        protected int m_KillersLuck;

        public int GetRandomMinMax(int min, int max)
        {
            int result;

            result = min + (int)(Math.Round(Utility.RandomDouble() * (max - min)));

            return result;
        }

        public void PackAddGoldTier(int tier)
        {
            int minAmount = LootBag.GoldTiers[tier - 1, 0];
            int maxAmount = LootBag.GoldTiers[tier - 1, 1];

            // Check if the current region is part of the DungeonRegion
            if (Region.IsPartOf(typeof(Server.Regions.DungeonRegion)))
            {
                // Double minAmount and maxAmount for the dungeon region
                minAmount *= 2;
                maxAmount *= 2;
            }

            int amount = GetRandomMinMax(minAmount, maxAmount);

            if (amount < 1)
            {
                return;
            }
            if (PseudoSeerStone.Instance != null)
                amount = (int)Math.Round(amount * PseudoSeerStone.Instance.CreatureLootDropMultiplier);
            PackItem(new Gold(amount));
        }

        public void PackAddArtifactChanceTier(int tier)
        {
            double chance = LootBag.ArtifactTiers[0, tier - 1, 0];
            int minArtifactLevel = (int)(LootBag.ArtifactTiers[0, tier - 1, 1]);
            int maxArtifactLevel = (int)(LootBag.ArtifactTiers[0, tier - 1, 2]);

            int artifactLevel = GetRandomMinMax(minArtifactLevel, maxArtifactLevel);

            if (chance >= Utility.RandomDouble())
            {
                double check = Utility.RandomDouble();

                //Slayer
                if (check >= 0 && check < .30)
                {
                    Item artifact = Loot.RandomSlayerArtifact();
                    PackItem(artifact);
                }

                //Damage
                if (check >= .30 && check < .45)
                {
                    int ArtifactOffset = 0; //Damage Offset (0-3)

                    Item artifact = Loot.RandomArtifact(ArtifactOffset + minArtifactLevel - 1, ArtifactOffset + maxArtifactLevel - 1);
                    PackItem(artifact);
                }

                //Accuracy
                if (check >= .45 && check < .60)
                {
                    int ArtifactOffset = 4; //Accuracy Offset (4-7)

                    Item artifact = Loot.RandomArtifact(ArtifactOffset + minArtifactLevel - 1, ArtifactOffset + maxArtifactLevel - 1);
                    PackItem(artifact);
                }

                //Armor
                if (check >= .60)
                {
                    int ArtifactOffset = 8; //Armor Offset (8-11)

                    Item artifact = Loot.RandomArtifact(ArtifactOffset + minArtifactLevel - 1, ArtifactOffset + maxArtifactLevel - 1);
                    PackItem(artifact);
                }
            }
        }

        public void PackAddMapChanceTier(int tier)
        {
            double chance = LootBag.MapTiers[tier - 1, 0];
            int mapValue = (int)(LootBag.MapTiers[tier - 1, 1]);

            if (chance >= Utility.RandomDouble())
            {
                PackItem(new TreasureMap(mapValue, Map));
            }
        }

        public void PackLeatherDyeTubTier(int tier)
        {
            int chance = LootBag.LeatherDyeTubTiers[tier - 1, 0];
            int mDyeTubGroup = LootBag.LeatherDyeTubTiers[tier - 1, 1];



            if (chance >= Utility.RandomMinMax(1, 100))
            {
                if (mDyeTubGroup == 0)
                {
                    PackItem(Loot.RandomLeatherDyeTubTypes1());
                }
                else if (mDyeTubGroup == 1)
                {
                    PackItem(Loot.RandomLeatherDyeTubTypes2());
                }
                else if (mDyeTubGroup == 2)
                {
                    PackItem(Loot.RandomLeatherDyeTubTypes3());
                }
                else if (mDyeTubGroup == 3)
                {
                    PackItem(Loot.RandomLeatherDyeTubTypesAll());
                }

            }


        }

        public void PackAddReagentTier(int tier)
        {
            int minAmount = LootBag.ReagentTiers[tier - 1, 0];
            int maxAmount = LootBag.ReagentTiers[tier - 1, 1];

            int amount = GetRandomMinMax(minAmount, maxAmount);

            if (amount < 1)
            {
                return;
            }

            Item reagent = Loot.RandomReagent();

            reagent.Amount = amount;

            PackItem(reagent);
        }

        public void PackAddIngotTier(int tier)
        {
            int minAmount = LootBag.IngotTiers[tier - 1, 0];
            int maxAmount = LootBag.IngotTiers[tier - 1, 1];

            int amount = GetRandomMinMax(minAmount, maxAmount);

            if (amount < 1)
            {
                return;
            }

            PackItem(new IronIngot(amount));
        }

        public void PackAddBoardTier(int tier)
        {
            int minAmount = LootBag.BoardTiers[tier - 1, 0];
            int maxAmount = LootBag.BoardTiers[tier - 1, 1];

            int amount = GetRandomMinMax(minAmount, maxAmount);

            if (amount < 1)
            {
                return;
            }

            PackItem(new Board(amount));
        }

        public void PackAddBandageTier(int tier)
        {
            int minAmount = LootBag.BandageTiers[tier - 1, 0];
            int maxAmount = LootBag.BandageTiers[tier - 1, 1];

            int amount = GetRandomMinMax(minAmount, maxAmount);

            if (amount < 1)
            {
                return;
            }

            PackItem(new Bandage(amount));
        }

        public void PackAddArrowTier(int tier)
        {
            int minAmount = LootBag.ArrowTiers[tier - 1, 0];
            int maxAmount = LootBag.ArrowTiers[tier - 1, 1];

            int amount = GetRandomMinMax(minAmount, maxAmount);

            if (amount < 1)
            {
                return;
            }

            PackItem(new Arrow(amount));
        }

        public void PackAddBoltTier(int tier)
        {
            int minAmount = LootBag.ArrowTiers[tier - 1, 0];
            int maxAmount = LootBag.ArrowTiers[tier - 1, 1];

            int amount = GetRandomMinMax(minAmount, maxAmount);

            if (amount < 1)
            {
                return;
            }

            PackItem(new Bolt(amount));
        }

        public void PackAddGemTier(int tier)
        {
            int minAmount = LootBag.GemTiers[tier - 1, 0];
            int maxAmount = LootBag.GemTiers[tier - 1, 1];

            int amount = GetRandomMinMax(minAmount, maxAmount);

            if (amount < 0)
            {
                return;
            }

            //Split Into Multipe Types At Some Point

            Item gem = Loot.RandomGem();

            gem.Amount = amount;

            PackItem(gem);
        }

        public void PackAddScrollTier(int tier)
        {
            int minLevel = LootBag.SpellScrollTiers[0, tier - 1, 0];
            int maxLevel = LootBag.SpellScrollTiers[0, tier - 1, 1];
            int amount = LootBag.SpellScrollTiers[0, tier - 1, 2];

            int minIndex = (minLevel - 1) * 8;
            int maxIndex = minIndex + ((maxLevel - minLevel + 1) * 8) - 1;

            for (int i = 0; i < amount; ++i)
            {
                PackItem(Loot.RandomScroll(minIndex, maxIndex, SpellbookType.Regular));
            }
        }

        public void PackAddPoisonPotionTier(int tier)
        {
            int poisonLevel = LootBag.PoisonPotionTiers[0, tier - 1, 0];
            int minAmount = LootBag.PoisonPotionTiers[0, tier - 1, 1];
            int maxAmount = LootBag.PoisonPotionTiers[0, tier - 1, 2];

            int amount = GetRandomMinMax(minAmount, maxAmount);

            Item poisonPotion;

            switch (poisonLevel)
            {
                case 0:
                    poisonPotion = new LesserPoisonPotion();
                    break;

                case 1:
                    poisonPotion = new PoisonPotion();
                    break;

                case 2:
                    poisonPotion = new GreaterPoisonPotion();
                    break;

                case 3:
                    poisonPotion = new DeadlyPoisonPotion();
                    break;

                default:
                    poisonPotion = new LesserPoisonPotion();
                    break;
            }

            if (poisonPotion != null)
            {
                if (amount < 1)
                {
                    return;
                }

                poisonPotion.Amount = amount;

                PackItem(poisonPotion);
            }
        }

        public void PackAddRandomPotionTier(int tier)
        {
            int potionLevel = LootBag.RandomPotionTiers[0, tier - 1, 0];
            int minAmount = LootBag.RandomPotionTiers[0, tier - 1, 1];
            int maxAmount = LootBag.RandomPotionTiers[0, tier - 1, 2];

            int amount = GetRandomMinMax(minAmount, maxAmount);

            if (potionLevel == 0)
            {
                for (int i = 0; i < amount; ++i)
                {
                    Item potion = Loot.RandomPotion();

                    if (potion != null)
                    {
                        PackItem(potion);
                    }
                }
            }

            else if (potionLevel == 1)
            {
                for (int i = 0; i < amount; ++i)
                {
                    Item potion = Loot.RandomGreaterPotion();

                    if (potion != null)
                    {
                        PackItem(potion);
                    }
                }
            }
        }

        public void PackItem(Item item)
        {
            if (Summoned || item == null)
            {
                if (item != null)
                    item.Delete();

                return;
            }

            Container pack = Backpack;

            if (pack == null)
            {
                pack = new Backpack();

                pack.Movable = false;

                AddItem(pack);
            }

            if (!item.Stackable || !pack.TryDropItem(this, item, false)) // try stack
                pack.DropItem(item); // failed, drop it anyway
        }

        #endregion

        //OLD LOOT METHODS
        # region Old Loot

        public virtual void GenerateLoot()
        {
            /*
            List<Item> loot = new List<Item>(LootBag.LootBagGenerate(this, this.LootLevel));

            foreach (Item item in loot)
                PackItem(item);
            */
        }

        public virtual void AddLoot(LootPack pack, int amount)
        {
            for (int i = 0; i < amount; ++i)
                AddLoot(pack);
        }

        public virtual void AddLoot(LootPack pack)
        {
            if (Summoned)
                return;

            Container backpack = Backpack;

            if (backpack == null)
            {
                backpack = new Backpack();

                backpack.Movable = false;

                AddItem(backpack);
            }

            pack.Generate(this, backpack, m_Spawning, m_KillersLuck);
        }

        public bool PackArmor(int minLevel, int maxLevel)
        {
            return PackArmor(minLevel, maxLevel, 1.0);
        }

        public bool PackArmor(int minLevel, int maxLevel, double chance)
        {
            if (chance <= Utility.RandomDouble())
                return false;

            Cap(ref minLevel, 0, 5);
            Cap(ref maxLevel, 0, 5);

            BaseArmor armor = Loot.RandomArmorOrShield();

            if (armor == null)
                return false;

            armor.ProtectionLevel = (ArmorProtectionLevel)RandomMinMaxScaled(minLevel, maxLevel);
            armor.Durability = (ArmorDurabilityLevel)RandomMinMaxScaled(minLevel, maxLevel);

            PackItem(armor);

            return true;
        }

        public static void GetRandomAOSStats(int minLevel, int maxLevel, out int attributeCount, out int min, out int max)
        {
            int v = RandomMinMaxScaled(minLevel, maxLevel);

            if (v >= 5)
            {
                attributeCount = Utility.RandomMinMax(2, 6);
                min = 20; max = 70;
            }
            else if (v == 4)
            {
                attributeCount = Utility.RandomMinMax(2, 4);
                min = 20; max = 50;
            }
            else if (v == 3)
            {
                attributeCount = Utility.RandomMinMax(2, 3);
                min = 20; max = 40;
            }
            else if (v == 2)
            {
                attributeCount = Utility.RandomMinMax(1, 2);
                min = 10; max = 30;
            }
            else
            {
                attributeCount = 1;
                min = 10; max = 20;
            }
        }

        public static int RandomMinMaxScaled(int min, int max)
        {
            if (min == max)
                return min;

            if (min > max)
            {
                int hold = min;
                min = max;
                max = hold;
            }

            /* Example:
             *    min: 1
             *    max: 5
             *  count: 5
             *
             * total = (5*5) + (4*4) + (3*3) + (2*2) + (1*1) = 25 + 16 + 9 + 4 + 1 = 55
             *
             * chance for min+0 : 25/55 : 45.45%
             * chance for min+1 : 16/55 : 29.09%
             * chance for min+2 :  9/55 : 16.36%
             * chance for min+3 :  4/55 :  7.27%
             * chance for min+4 :  1/55 :  1.81%
             */

            int count = max - min + 1;
            int total = 0, toAdd = count;

            for (int i = 0; i < count; ++i, --toAdd)
                total += toAdd * toAdd;

            int rand = Utility.Random(total);
            toAdd = count;

            int val = min;

            for (int i = 0; i < count; ++i, --toAdd, ++val)
            {
                rand -= toAdd * toAdd;

                if (rand < 0)
                    break;
            }

            return val;
        }

        public bool PackSlayer()
        {
            return PackSlayer(0.05);
        }

        public bool PackSlayer(double chance)
        {
            if (chance <= Utility.RandomDouble())
                return false;

            if (Utility.RandomBool())
            {
                BaseInstrument instrument = Loot.RandomInstrument();

                if (instrument != null)
                {
                    instrument.Slayer = SlayerGroup.GetLootSlayerType(GetType());
                    PackItem(instrument);
                }
            }
            else if (!Core.AOS)
            {
                BaseWeapon weapon = Loot.RandomWeapon();

                if (weapon != null)
                {
                    weapon.Slayer = SlayerGroup.GetLootSlayerType(GetType());
                    PackItem(weapon);
                }
            }

            return true;
        }

        public bool PackWeapon(int minLevel, int maxLevel)
        {
            return PackWeapon(minLevel, maxLevel, 1.0);
        }

        public bool PackWeapon(int minLevel, int maxLevel, double chance)
        {
            if (chance <= Utility.RandomDouble())
                return false;

            Cap(ref minLevel, 0, 5);
            Cap(ref maxLevel, 0, 5);

            if (Core.AOS)
            {
                Item item = Loot.RandomWeaponOrJewelry();

                if (item == null)
                    return false;

                int attributeCount, min, max;
                GetRandomAOSStats(minLevel, maxLevel, out attributeCount, out min, out max);

                if (item is BaseWeapon)
                    BaseRunicTool.ApplyAttributesTo((BaseWeapon)item, attributeCount, min, max);
                else if (item is BaseJewel)
                    BaseRunicTool.ApplyAttributesTo((BaseJewel)item, attributeCount, min, max);

                PackItem(item);
            }
            else
            {
                BaseWeapon weapon = Loot.RandomWeapon();

                if (weapon == null)
                    return false;

                if (0.05 > Utility.RandomDouble())
                    weapon.Slayer = SlayerName.Silver;

                weapon.DamageLevel = (WeaponDamageLevel)RandomMinMaxScaled(minLevel, maxLevel);
                weapon.AccuracyLevel = (WeaponAccuracyLevel)RandomMinMaxScaled(minLevel, maxLevel);
                weapon.DurabilityLevel = (WeaponDurabilityLevel)RandomMinMaxScaled(minLevel, maxLevel);

                PackItem(weapon);
            }

            return true;
        }

        public void PackGold(int amount)
        {
            if (amount > 0)
                PackItem(new Gold(amount));
        }

        public void PackGold(int min, int max)
        {
            PackGold(Utility.RandomMinMax(min, max));
        }

        public void PackStatue(int min, int max)
        {
            PackStatue(Utility.RandomMinMax(min, max));
        }

        public void PackStatue(int amount)
        {
            for (int i = 0; i < amount; ++i)
                PackStatue();
        }

        public void PackStatue()
        {
            PackItem(Loot.RandomStatue());
        }

        public void PackGem()
        {
            PackGem(1);
        }

        public void PackGem(int min, int max)
        {
            PackGem(Utility.RandomMinMax(min, max));
        }

        public void PackGem(int amount)
        {
            if (amount <= 0)
                return;

            Item gem = Loot.RandomGem();

            gem.Amount = amount;

            PackItem(gem);
        }

        public void PackReg(int min, int max)
        {
            PackReg(Utility.RandomMinMax(min, max));
        }

        public void PackReg(int amount)
        {
            if (amount <= 0)
                return;

            Item reg = Loot.RandomReagent();

            reg.Amount = amount;

            PackItem(reg);
        }

        #endregion

        public override void OnDoubleClick(Mobile from)
        {
            if (from.AccessLevel >= AccessLevel.GameMaster && !Body.IsHuman)
            {
                Container pack = this.Backpack;

                if (pack != null)
                    pack.DisplayTo(from);
            }

            base.OnDoubleClick(from);
        }

        private class DeathAdderCharmTarget : Target
        {
            private BaseCreature m_Charmed;

            public DeathAdderCharmTarget(BaseCreature charmed)
                : base(-1, false, TargetFlags.Harmful)
            {
                m_Charmed = charmed;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (!m_Charmed.DeathAdderCharmable || m_Charmed.Combatant != null || !from.CanBeHarmful(m_Charmed, false))
                    return;

                Mobile targ = targeted as Mobile;
                if (targ == null || !from.CanBeHarmful(targ, false))
                    return;

                from.RevealingAction();
                from.DoHarmful(targ, true);

                m_Charmed.Combatant = targ;

                if (m_Charmed.AIObject != null)
                    m_Charmed.AIObject.Action = ActionType.Combat;
            }
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);

            if (Core.ML)
            {
                if (DisplayWeight)
                    list.Add(TotalWeight == 1 ? 1072788 : 1072789, TotalWeight.ToString()); // Weight: ~1_WEIGHT~ stones

                if (m_ControlOrder == OrderType.Guard)
                    list.Add(1080078); // guarding
            }

            if (Summoned && !IsAnimatedDead && !IsNecroFamiliar)
                list.Add(1049646); // (summoned)
            else if (Controlled && Commandable)
            {
                if (IsBonded)	//Intentional difference (showing ONLY bonded when bonded instead of bonded & tame)
                    list.Add(1049608); // (bonded)
                else
                    list.Add(502006); // (tame)
            }
        }

        public override void OnSingleClick(Mobile from)
        {
            if (Controlled && Commandable)
            {
                int number;

                if (Summoned)
                    number = 1049646; // (summoned)
                else if (IsBonded)
                    number = 1049608; // (bonded)
                else
                    number = 502006; // (tame)

                PrivateOverheadMessage(MessageType.Regular, 0x3B2, number, from.NetState);
            }

            base.OnSingleClick(from);
        }

        public virtual bool IgnoreYoungProtection { get { return false; } }

        public override bool OnBeforeDeath()
        {
            /*
            int treasureLevel = TreasureMapLevel;

            if (!Summoned && !NoKillAwards && !IsBonded && treasureLevel >= 0)
            {
                if (m_Paragon && Paragon.ChestChance > Utility.RandomDouble())
                    PackItem(new ParagonChest(this.Name, treasureLevel));
                else if ((Map == Map.Felucca || Map == Map.Trammel) && TreasureMap.LootChance >= Utility.RandomDouble())
                    PackItem(new TreasureMap(treasureLevel, Map));
            }

            if (!Summoned && !NoKillAwards && !m_HasGeneratedLoot)
            {
                m_HasGeneratedLoot = true;
            }
            */

            //Start Zombiex edit
            if (LastKiller is Zombiex)
            {
                Zombiex zomb = new Zombiex();
                zomb.NewZombie(this);
            }
            //End Zombiex edit

            // if they are controlled by a pseudoseer, attempt to reconnect:
            if (this.NetState != null && !CreaturePossession.AttemptReturnToOriginalBody(this.NetState))
            {
                this.NetState.Dispose();
            }

            // find out whether most recent aggressor is in factions & how many heros their town has
            // and give a 10-40% gold bonus accordingly
            // NOTE: if it is desired to make this gold bonus go for anyone in a party with a faction
            // member, and not just when a faction member specifically gets the kill shot,
            // then this should be handled in the same place gold is boosted for Adventurer bonus (see below)
            PlayerMobile slayer = this.LastKiller as PlayerMobile;
            if (!HasBeenPseudoseerControlled  // plug gold duping potential
                && !(this is PackLlama) // plug gold duping potential
                && !(this is PackHorse) // plug gold duping potential
                && slayer != null
                && (slayer.FactionPlayerState != null
                    && slayer.FactionPlayerState.Faction != null))
            {
                //Console.WriteLine("slayer=" + slayer);
                //Console.WriteLine("slayer.FactionPlayerState=" + slayer.FactionPlayerState);
                //Console.WriteLine("slayer.FactionPlayerState.Faction=" + slayer.FactionPlayerState.Faction);
                double extraGoldFraction = 0.1; // free extra 10% for being in factions
                CommonwealthState owningCommonwealthState = slayer.FactionPlayerState.Faction.OwningCommonwealth.State;
                if (owningCommonwealthState.Hero != null
                    && (owningCommonwealthState.Hero.IsHome))
                {
                    extraGoldFraction += 0.1; // extra 10% if their own hero is ok
                }
                foreach (TownHero capturedHero in owningCommonwealthState.CapturedHeroes)
                {
                    extraGoldFraction += 0.1; // extra 10% for every captured hero
                }
                //Console.WriteLine("extraGoldFraction=" + extraGoldFraction);
                //Console.WriteLine("before TotalGold=" + TotalGold);
                PackGold((int)(this.TotalGold * extraGoldFraction));
                //Console.WriteLine("after TotalGold=" + TotalGold);
            }
            if (IsAnimatedDead)
                Effects.SendLocationEffect(Location, Map, 0x3728, 13, 1, 0x461, 4);

            InhumanSpeech speechType = this.SpeechType;
            CurrentWayPoint = null;

            if (speechType != null && Speaks)
                speechType.OnDeath(this);

            return base.OnBeforeDeath();
        }

        private bool m_NoKillAwards;

        public bool NoKillAwards
        {
            get { return m_NoKillAwards; }
            set { m_NoKillAwards = value; }
        }

        public int ComputeBonusDamage(List<DamageEntry> list, Mobile m)
        {
            int bonus = 0;

            for (int i = list.Count - 1; i >= 0; --i)
            {
                DamageEntry de = list[i];

                if (de.Damager == m || !(de.Damager is BaseCreature))
                    continue;

                BaseCreature bc = (BaseCreature)de.Damager;
                Mobile master = null;

                master = bc.GetMaster();

                if (master == m)
                    bonus += de.DamageGiven;
            }

            return bonus;
        }

        public Mobile GetMaster()
        {
            if (Controlled && ControlMaster != null)
                return ControlMaster;
            else if (Summoned && SummonMaster != null)
                return SummonMaster;

            return null;
        }

        private class FKEntry
        {
            public Mobile m_Mobile;
            public int m_Damage;

            public FKEntry(Mobile m, int damage)
            {
                m_Mobile = m;
                m_Damage = damage;
            }
        }

        public static List<DamageStore> GetLootingRights(List<DamageEntry> damageEntries, int hitsMax)
        {
            List<DamageStore> rights = new List<DamageStore>();

            for (int i = damageEntries.Count - 1; i >= 0; --i)
            {
                if (i >= damageEntries.Count)
                    continue;

                DamageEntry de = damageEntries[i];

                if (de.HasExpired)
                {
                    damageEntries.RemoveAt(i);
                    continue;
                }

                int damage = de.DamageGiven;

                List<DamageEntry> respList = de.Responsible;

                if (respList != null)
                {
                    for (int j = 0; j < respList.Count; ++j)
                    {
                        DamageEntry subEntry = respList[j];
                        Mobile master = subEntry.Damager;

                        if (master == null || master.Deleted || !master.Player)
                            continue;

                        bool needNewSubEntry = true;

                        for (int k = 0; needNewSubEntry && k < rights.Count; ++k)
                        {
                            DamageStore ds = rights[k];

                            if (ds.m_Mobile == master)
                            {
                                ds.m_Damage += subEntry.DamageGiven;
                                needNewSubEntry = false;
                            }
                        }

                        if (needNewSubEntry)
                            rights.Add(new DamageStore(master, subEntry.DamageGiven));

                        damage -= subEntry.DamageGiven;
                    }
                }

                Mobile m = de.Damager;

                if (m == null || m.Deleted || !m.Player)
                    continue;

                if (damage <= 0)
                    continue;

                bool needNewEntry = true;

                for (int j = 0; needNewEntry && j < rights.Count; ++j)
                {
                    DamageStore ds = rights[j];

                    if (ds.m_Mobile == m)
                    {
                        ds.m_Damage += damage;
                        needNewEntry = false;
                    }
                }

                if (needNewEntry)
                    rights.Add(new DamageStore(m, damage));
            }

            if (rights.Count > 0)
            {
                rights[0].m_Damage = (int)(rights[0].m_Damage * 1.25);	//This would be the first valid person attacking it.  Gets a 25% bonus.  Per 1/19/07 Five on Friday

                if (rights.Count > 1)
                    rights.Sort(); //Sort by damage

                int topDamage = rights[0].m_Damage;
                int minDamage;

                if (hitsMax >= 3000)
                    minDamage = topDamage / 16;
                else if (hitsMax >= 1000)
                    minDamage = topDamage / 8;
                else if (hitsMax >= 200)
                    minDamage = topDamage / 4;
                else
                    minDamage = topDamage / 2;

                for (int i = 0; i < rights.Count; ++i)
                {
                    DamageStore ds = rights[i];

                    ds.m_HasRight = (ds.m_Damage >= minDamage);
                }
            }

            return rights;
        }

        public virtual void OnKilledBy(Mobile mob)
        {
            if (m_Paragon && Paragon.CheckArtifactChance(mob, this))
                Paragon.GiveArtifactTo(mob);
        }

        public override void OnDeath(Container c)
        {
            MeerMage.StopEffect(this, false);

            if (IsBonded)
            {
                int sound = this.GetDeathSound();

                if (sound >= 0)
                    Effects.PlaySound(this, this.Map, sound);

                Warmode = false;

                Poison = null;
                Combatant = null;

                Hits = 0;
                Stam = 0;
                Mana = 0;

                IsDeadPet = true;
                ControlTarget = ControlMaster;
                ControlOrder = OrderType.Follow;

                ProcessDeltaQueue();
                SendIncomingPacket();
                SendIncomingPacket();

                List<AggressorInfo> aggressors = this.Aggressors;

                for (int i = 0; i < aggressors.Count; ++i)
                {
                    AggressorInfo info = aggressors[i];

                    if (info.Attacker.Combatant == this)
                        info.Attacker.Combatant = null;
                }

                List<AggressorInfo> aggressed = this.Aggressed;

                for (int i = 0; i < aggressed.Count; ++i)
                {
                    AggressorInfo info = aggressed[i];

                    if (info.Defender.Combatant == this)
                        info.Defender.Combatant = null;
                }

                Mobile owner = this.ControlMaster;

                if (owner == null || owner.Deleted || owner.Map != this.Map || !owner.InRange(this, 12) || !this.CanSee(owner) || !this.InLOS(owner))
                {
                    if (this.OwnerAbandonTime == DateTime.MinValue)
                        this.OwnerAbandonTime = DateTime.Now;
                }
                else
                {
                    this.OwnerAbandonTime = DateTime.MinValue;
                }

                CheckStatTimers();
            }

            else
            {
                if (!Summoned && !m_NoKillAwards)
                {
                    int totalFame = Fame / 100;
                    int totalKarma = -Karma / 100;

                    if (Map == Map.Felucca)
                    {
                        totalFame += ((totalFame / 10) * 3);
                        totalKarma += ((totalKarma / 10) * 3);
                    }

                    List<DamageStore> list = GetLootingRights(this.DamageEntries, this.HitsMax);
                    List<Mobile> titles = new List<Mobile>();
                    List<int> fame = new List<int>();
                    List<int> karma = new List<int>();

                    bool givenQuestKill = false;
                    bool givenFactionKill = false;

                    for (int i = 0; i < list.Count; ++i)
                    {
                        DamageStore ds = list[i];

                        if (!ds.m_HasRight)
                            continue;

                        Party party = Engines.PartySystem.Party.Get(ds.m_Mobile);

                        //In Party
                        if (party != null)
                        {
                            int nearbyPartyMembers = 0;

                            //Determine Nearby Party Members
                            for (int j = 0; j < party.Members.Count; ++j)
                            {
                                PartyMemberInfo info = party.Members[j] as PartyMemberInfo;

                                //If Party Member is Valid and Within Maximum Distance of Corpse for Fame/Karma Sharing
                                if (info != null && info.Mobile != null && info.Mobile.GetDistanceToSqrt(this) <= FeatureList.FameKarma.PartyFameKarmaSharingDisance)
                                {
                                    nearbyPartyMembers++;
                                }
                            }

                            //Set to 1 if Not Valid (Won't Be Distributed Anyways)
                            if (nearbyPartyMembers <= 0)
                            {
                                nearbyPartyMembers = 1;
                            }

                            int divedFame = totalFame / nearbyPartyMembers;
                            int divedKarma = totalKarma / nearbyPartyMembers;

                            //Divide Up Fame and Karma
                            for (int j = 0; j < party.Members.Count; ++j)
                            {
                                PartyMemberInfo info = party.Members[j] as PartyMemberInfo;

                                //If Party Member is Valid and Within Maximum Distance of Corpse for Fame/Karma Sharing
                                if (info != null && info.Mobile != null && info.Mobile.GetDistanceToSqrt(this) <= FeatureList.FameKarma.PartyFameKarmaSharingDisance)
                                {
                                    int index = titles.IndexOf(info.Mobile);

                                    if (index == -1)
                                    {
                                        titles.Add(info.Mobile);
                                        fame.Add(divedFame);
                                        karma.Add(divedKarma);
                                    }

                                    else
                                    {
                                        fame[index] += divedFame;
                                        karma[index] += divedKarma;
                                    }
                                }
                            }
                        }

                        //Solo
                        else
                        {
                            titles.Add(ds.m_Mobile);
                            fame.Add(totalFame);
                            karma.Add(totalKarma);
                        }

                        OnKilledBy(ds.m_Mobile);

                        if (!givenFactionKill)
                        {
                            givenFactionKill = true;
                            Faction.HandleDeath(this, ds.m_Mobile);
                        }

                        Region region = ds.m_Mobile.Region;

                        if (givenQuestKill)
                            continue;

                        PlayerMobile pm = ds.m_Mobile as PlayerMobile;

                        if (pm != null)
                        {
                            if (pm != null)
                                pm.KillsThisSession++;

                            QuestSystem qs = pm.Quest;

                            if (qs != null)
                            {
                                qs.OnKill(this, c);
                                givenQuestKill = true;
                            }
                        }

                        //Check if we need to increase the amount of gold this creature drops - ths happens when a citizen with adventurer bonus kills it
                        if (!HasBeenPseudoseerControlled // plug gold duping hole
                            && !(this is PackLlama) // plug gold duping hole
                            && !(this is PackHorse) // plug gold duping hole
                            && pm.CitizenshipPlayerState != null)
                        {
                            CommonwealthResourceBonus crb = pm.CitizenshipPlayerState.GetResourceBonus(ResourceBonusTypes.Adventurer);

                            if (crb != null)
                            {
                                double goldMultiplier = ((double)crb.HarvestBonus / 100);
                                //Never allow more than a 25% gain
                                if (goldMultiplier > 0.25)
                                    goldMultiplier = 0.25;

                                Gold gold = (Gold)c.FindItemByType(typeof(Gold));

                                if (gold != null)
                                {
                                    double oldGold = (double)gold.Amount;

                                    gold.Amount += (int)(Math.Floor(oldGold * goldMultiplier));
                                }
                            }
                        }
                    }

                    for (int i = 0; i < titles.Count; ++i)
                    {
                        Titles.AwardFame(titles[i], fame[i], true);
                        Titles.AwardKarma(titles[i], karma[i], true);
                        // modification to support XmlQuest Killtasks... cancel this: useful for KILLS quest, but
                        // does not use the "LastKiller" as the TRIGMOB in XmlDeathAction
                        // ... commenting this out might screw up party KILL quests???
                        //i'm moving it outside of this scope (see below)
                        //XmlQuest.RegisterKill(this, titles[i]);  

                    }
                }

                XmlQuest.RegisterKill(this, this.LastKiller);



                base.OnDeath(c);

                if (DeleteCorpseOnDeath)
                    c.Delete();
            }
        }

        /* To save on cpu usage, RunUO creatures only reacquire creatures under the following circumstances:
         *  - 10 seconds have elapsed since the last time it tried
         *  - The creature was attacked
         *  - Some creatures, like dragons, will reacquire when they see someone move
         *
         * This functionality appears to be implemented on OSI as well
         */

        private DateTime m_NextReacquireTime;

        public DateTime NextReacquireTime { get { return m_NextReacquireTime; } set { m_NextReacquireTime = value; } }

        public virtual TimeSpan ReacquireDelay { get { return TimeSpan.FromSeconds(10.0); } }
        public virtual bool ReacquireOnMovement { get { return false; } }

        public void ForceReacquire()
        {
            m_NextReacquireTime = DateTime.MinValue;
        }

        public override void OnDelete()
        {
            Mobile m = m_ControlMaster;

            SetControlMaster(null);
            SummonMaster = null;
            DoingBandage = false;
            CurrentWayPoint = null;

            if (this.BankBox != null)
                this.BankBox.Delete();

            base.OnDelete();

            if (m != null)
                m.InvalidateProperties();
        }

        public override bool CanBeHarmful(Mobile target, bool message, bool ignoreOurBlessedness)
        {
            if (target is BaseFactionGuard)
                return false;

            if (this.Controlled && this.ControlMaster != null)
            {
                PlayerMobile pm = (PlayerMobile)this.ControlMaster;

                // Prevent players from farming their own Champions or Champion spawns
                Faction playerFaction = Faction.Find(pm, true);

                Type[] forsakenTypes = new Type[] { typeof(ForsakenMinion), typeof(ForsakenDK), typeof(ForsakenNecromancer), typeof(Chandrian), typeof(Haliax) };
                Type[] urukhaiTypes = new Type[] { typeof(UrukGrunt), typeof(UrukRaider), typeof(UrukShaman), typeof(UrukScout), typeof(Gojira) };
                Type[] paladinTypes = new Type[] { typeof(POSquire), typeof(POKnight), typeof(POPaladin), typeof(Amyr), typeof(DupreThePaladin) };

                if ((playerFaction == Forsaken.Instance || pm.Citizenship == "Yew") && Array.Exists(forsakenTypes, element => element == target.GetType())) // forsaken , yew
                {
                    this.ControlOrder = OrderType.Stop;
                    this.ControlOrder = OrderType.Follow;
                    return false;
                }
                else if ((playerFaction == Urukhai.Instance || pm.Citizenship == "Blackrock") && Array.Exists(urukhaiTypes, element => element == target.GetType())) // urukhai , blackrock
                {
                    this.ControlOrder = OrderType.Stop;
                    this.ControlOrder = OrderType.Follow;
                    return false;
                }
                else if ((playerFaction == PaladinOrder.Instance || pm.Citizenship == "Trinsic") && Array.Exists(paladinTypes, element => element == target.GetType())) // palading , trinsic
                {
                    this.ControlOrder = OrderType.Stop;
                    this.ControlOrder = OrderType.Follow;
                    return false;
                }
            }

            if ((target is BaseVendor && ((BaseVendor)target).IsInvulnerable) || target is PlayerVendor || target is TownCrier || target is FactionRewardVendor)
            {
                if (message)
                {
                    if (target.Title == null)
                        SendMessage("{0} the vendor cannot be harmed.", target.Name);
                    else
                        SendMessage("{0} {1} cannot be harmed.", target.Name, target.Title);
                }

                return false;
            }

            return base.CanBeHarmful(target, message, ignoreOurBlessedness);
        }

        public override bool CanBeRenamedBy(Mobile from)
        {
            bool ret = base.CanBeRenamedBy(from);

            if (Controlled && from == ControlMaster && !from.Region.IsPartOf(typeof(Jail)))
                ret = true;

            return ret;
        }

        public bool SetControlMaster(Mobile m)
        {
            if (m == null)
            {
                ControlMaster = null;
                Controlled = false;
                ControlTarget = null;
                ControlOrder = OrderType.None;
                Guild = null;
                AITeamList.SetDefaultTeam(this);
                Delta(MobileDelta.Noto);
            }

            else
            {
                ISpawner se = this.Spawner;
                if (se != null && se.UnlinkOnTaming)
                {
                    this.Spawner.Remove(this);
                    this.Spawner = null;
                }

                if (m.Followers + ControlSlots > m.FollowersMax)
                {
                    m.SendLocalizedMessage(1049607); // You have too many followers to control that creature.
                    return false;
                }

                CurrentWayPoint = null;//so tamed animals don't try to go back

                ControlMaster = m;
                Controlled = true;
                ControlTarget = null;

                if (ControlMaster.Player)
                {
                    ControlOrder = OrderType.Stop;
                }

                else
                {
                    ControlOrder = OrderType.Guard;
                }

                Guild = null;

                if (m_DeleteTimer != null)
                {
                    m_DeleteTimer.Stop();
                    m_DeleteTimer = null;
                }
                this.TeamFlags = m.TeamFlags;
                Delta(MobileDelta.Noto);
            }

            InvalidateProperties();

            return true;
        }

        public override void OnRegionChange(Region Old, Region New)
        {
            base.OnRegionChange(Old, New);

            if (this.Controlled)
            {
                SpawnEntry se = this.Spawner as SpawnEntry;

                if (se != null && !se.UnlinkOnTaming && (New == null || !New.AcceptsSpawnsFrom(se.Region)))
                {
                    this.Spawner.Remove(this);
                    this.Spawner = null;
                }
            }
        }

        private static bool m_Summoning;

        public static bool Summoning
        {
            get { return m_Summoning; }
            set { m_Summoning = value; }
        }

        public static bool Summon(BaseCreature creature, Mobile caster, Point3D p, int sound, TimeSpan duration)
        {
            return Summon(creature, true, caster, p, sound, duration);
        }

        public static bool Summon(BaseCreature creature, bool controlled, Mobile caster, Point3D p, int sound, TimeSpan duration)
        {
            if (caster.Followers + creature.ControlSlots > caster.FollowersMax)
            {
                caster.SendLocalizedMessage(1049645); // You have too many followers to summon that creature.
                creature.Delete();

                return false;
            }

            m_Summoning = true;

            if (controlled)
            {
                creature.SetControlMaster(caster);
            }

            creature.RangeHome = 10;
            creature.Summoned = true;
            creature.SummonMaster = caster;

            Container pack = creature.Backpack;

            if (pack != null)
            {
                for (int i = pack.Items.Count - 1; i >= 0; --i)
                {
                    if (i >= pack.Items.Count)
                        continue;

                    pack.Items[i].Delete();
                }
            }

            new UnsummonTimer(caster, creature, duration).Start();
            creature.m_SummonEnd = DateTime.Now + duration;

            creature.MoveToWorld(p, caster.Map);

            if ((creature is EnergyVortex || creature is BladeSpirits) && creature != null && creature.Alive)
            {
                creature.ControlOrder = OrderType.None;
                if (creature.Debug) { creature.DebugSay("Summon to Guardmode"); }
                creature.m_AI.GuardMode();
            }

            else
            {
                creature.ControlOrder = OrderType.Guard;
            }

            Effects.PlaySound(p, creature.Map, sound);

            m_Summoning = false;

            return true;
        }

        private static bool EnableRummaging = false;

        private const double ChanceToRummage = 0.5; // 50%

        private const double MinutesToNextRummageMin = 1.0;
        private const double MinutesToNextRummageMax = 4.0;

        private const double MinutesToNextChanceMin = 0.25;
        private const double MinutesToNextChanceMax = 0.75;

        private DateTime m_NextRummageTime;

        private bool m_CanBreathCustom = false;
        [CommandProperty(AccessLevel.GameMaster)]
        public bool CanBreathCustom
        {
            get { return m_CanBreathCustom; }
            set { m_CanBreathCustom = value; }
        }
        private int m_BreathDamageCustom = -1;
        [CommandProperty(AccessLevel.GameMaster)]
        public int BreathDamageCustom
        {
            get { return m_BreathDamageCustom; }
            set { m_BreathDamageCustom = value; }
        }

        public virtual bool CanBreath { get { return CanBreathCustom || (HasBreath && !Summoned); } }
        public virtual bool IsDispellable { get { return Summoned && !IsAnimatedDead; } }

        #region Healing
        public virtual bool CanHeal { get { return false; } }
        public virtual bool CanHealOwner { get { return false; } }
        public virtual double HealScalar { get { return 1.0; } }

        public virtual int HealSound { get { return 0x57; } }
        public virtual int HealStartRange { get { return 2; } }
        public virtual int HealEndRange { get { return RangePerception; } }
        public virtual double HealTrigger { get { return 0.78; } }
        public virtual double HealDelay { get { return 6.5; } }
        public virtual double HealInterval { get { return 0.0; } }
        public virtual bool HealFully { get { return true; } }
        public virtual double HealOwnerTrigger { get { return 0.78; } }
        public virtual double HealOwnerDelay { get { return 6.5; } }
        public virtual double HealOwnerInterval { get { return 30.0; } }
        public virtual bool HealOwnerFully { get { return false; } }

        private DateTime m_NextHealTime = DateTime.Now;
        private DateTime m_NextHealOwnerTime = DateTime.Now;
        private Timer m_HealTimer = null;

        public bool IsHealing { get { return (m_HealTimer != null); } }

        public virtual void HealStart(Mobile patient)
        {
            bool onSelf = (patient == this);

            //DoBeneficial( patient );

            RevealingAction();

            if (!onSelf)
            {
                patient.RevealingAction();
                patient.SendLocalizedMessage(1008078, false, Name); //  : Attempting to heal you.
            }

            double seconds = (onSelf ? HealDelay : HealOwnerDelay) + (patient.Alive ? 0.0 : 5.0);

            m_HealTimer = Timer.DelayCall(TimeSpan.FromSeconds(seconds), new TimerStateCallback(Heal_Callback), patient);
        }

        private void Heal_Callback(object state)
        {
            if (state is Mobile)
                Heal((Mobile)state);
        }

        public virtual void Heal(Mobile patient)
        {
            if (!Alive || this.Map == Map.Internal || !CanBeBeneficial(patient, true, true) || patient.Map != this.Map || !InRange(patient, HealEndRange))
            {
                StopHeal();
                return;
            }

            bool onSelf = (patient == this);

            if (!patient.Alive)
            {
            }
            else if (patient.Poisoned)
            {
                int poisonLevel = patient.Poison.Level;

                double healing = Skills.Healing.Value;
                double anatomy = Skills.Anatomy.Value;
                double chance = (healing - 30.0) / 50.0 - poisonLevel * 0.1;

                if ((healing >= 60.0 && anatomy >= 60.0) && chance > Utility.RandomDouble())
                {
                    if (patient.CurePoison(this))
                    {
                        patient.SendLocalizedMessage(1010059); // You have been cured of all poisons.

                        CheckSkill(SkillName.Healing, 0.0, 60.0 + poisonLevel * 10.0); // TODO: Verify formula
                        CheckSkill(SkillName.Anatomy, 0.0, 100.0);
                    }
                }
            }
            else if (BleedAttack.IsBleeding(patient))
            {
                patient.SendLocalizedMessage(1060167); // The bleeding wounds have healed, you are no longer bleeding!
                BleedAttack.EndBleed(patient, false);
            }
            else
            {
                double healing = Skills.Healing.Value;
                double anatomy = Skills.Anatomy.Value;
                double chance = (healing + 10.0) / 100.0;

                if (chance > Utility.RandomDouble())
                {
                    double min, max;

                    min = (anatomy / 10.0) + (healing / 6.0) + 4.0;
                    max = (anatomy / 8.0) + (healing / 3.0) + 4.0;

                    if (onSelf)
                        max += 10;

                    double toHeal = min + (Utility.RandomDouble() * (max - min));

                    toHeal *= HealScalar;

                    patient.Heal((int)toHeal);

                    CheckSkill(SkillName.Healing, 0.0, 90.0);
                    CheckSkill(SkillName.Anatomy, 0.0, 100.0);
                }
            }

            HealEffect(patient);

            StopHeal();

            if ((onSelf && HealFully && Hits >= HealTrigger * HitsMax && Hits < HitsMax) || (!onSelf && HealOwnerFully && patient.Hits >= HealOwnerTrigger * patient.HitsMax && patient.Hits < patient.HitsMax))
                HealStart(patient);
        }

        public virtual void StopHeal()
        {
            if (m_HealTimer != null)
                m_HealTimer.Stop();

            m_HealTimer = null;
        }

        public virtual void HealEffect(Mobile patient)
        {
            patient.PlaySound(HealSound);
        }

        #endregion
        public virtual void SetNextBreathTime()
        {
            if (m_BreathCustomDelay < 0)
            {
                m_NextBreathTime = DateTime.Now + TimeSpan.FromSeconds(BreathMinDelay + (Utility.RandomDouble() * BreathMaxDelay));
            }
            else
            {
                m_NextBreathTime = DateTime.Now + TimeSpan.FromSeconds(BreathCustomDelay);
            }
        }

        public virtual void OnThink()
        {
            if (EnableRummaging && CanRummageCorpses && !Summoned && !Controlled && DateTime.Now >= m_NextRummageTime)
            {
                double min, max;

                if (ChanceToRummage > Utility.RandomDouble() && Rummage())
                {
                    min = MinutesToNextRummageMin;
                    max = MinutesToNextRummageMax;
                }
                else
                {
                    min = MinutesToNextChanceMin;
                    max = MinutesToNextChanceMax;
                }

                double delay = min + (Utility.RandomDouble() * (max - min));
                m_NextRummageTime = DateTime.Now + TimeSpan.FromMinutes(delay);
            }

            if (CanBreath && DateTime.Now >= m_NextBreathTime) // tested: controlled dragons do breath fire, what about summoned skeletal dragons?
            {
                Mobile target = this.Combatant;

                if (target != null && target.Alive && !target.IsDeadBondedPet && CanBeHarmful(target) && target.Map == this.Map && !IsDeadBondedPet && target.InRange(this, BreathRange) && InLOS(target) && !BardPacified)
                {
                    if ((DateTime.Now - m_NextBreathTime) < TimeSpan.FromSeconds(30))
                    {
                        BreathStart(target);
                    }
                    SetNextBreathTime();
                }
            }

            if ((CanHeal || CanHealOwner) && Alive && !IsHealing && !BardPacified)
            {
                Mobile owner = this.ControlMaster;

                if (owner != null && CanHealOwner && DateTime.Now >= m_NextHealOwnerTime && CanBeBeneficial(owner, true, true) && owner.Map == this.Map && InRange(owner, HealStartRange) && InLOS(owner) && owner.Hits < HealOwnerTrigger * owner.HitsMax)
                {
                    HealStart(owner);

                    m_NextHealOwnerTime = DateTime.Now + TimeSpan.FromSeconds(HealOwnerInterval);
                }
                else if (CanHeal && DateTime.Now >= m_NextHealTime && CanBeBeneficial(this) && (Hits < HealTrigger * HitsMax || Poisoned))
                {
                    HealStart(this);

                    m_NextHealTime = DateTime.Now + TimeSpan.FromSeconds(HealInterval);
                }
            }
        }

        public virtual bool Rummage()
        {
            Corpse toRummage = null;

            foreach (Item item in this.GetItemsInRange(2))
            {
                if (item is Corpse && item.Items.Count > 0)
                {
                    toRummage = (Corpse)item;
                    break;
                }
            }

            if (toRummage == null)
                return false;

            Container pack = this.Backpack;

            if (pack == null)
                return false;

            List<Item> items = toRummage.Items;

            bool rejected;
            LRReason reason;

            for (int i = 0; i < items.Count; ++i)
            {
                Item item = items[Utility.Random(items.Count)];

                Lift(item, item.Amount, out rejected, out reason);

                if (!rejected && Drop(this, new Point3D(-1, -1, 0)))
                {
                    // *rummages through a corpse and takes an item*
                    PublicOverheadMessage(MessageType.Emote, 0x3B2, 1008086);
                    //TODO: Instancing of Rummaged stuff.
                    return true;
                }
            }

            return false;
        }

        public void Pacify(Mobile master, DateTime endtime)
        {
            BardPacified = true;
            BardProvoked = false;

            Warmode = false;
            Combatant = null;

            BardEndTime = endtime;
        }

        public override Mobile GetDamageMaster(Mobile damagee)
        {
            if (m_bBardProvoked && damagee == m_bBardTarget)
                return m_bBardMaster;
            else if (m_bControlled && m_ControlMaster != null)
                return m_ControlMaster;
            else if (m_bSummoned && m_SummonMaster != null)
                return m_SummonMaster;

            return base.GetDamageMaster(damagee);
        }

        public void Provoke(Mobile master, Mobile target, bool bSuccess)
        {
            if (master == null || master.Deleted)
                return;

            if (target == null || target.Deleted)
                return;

            if (this.Deleted)
                return;

            try // i can't identify why there is still a NullReferenceException in here, hence the lame try catch 
            {
                BardProvoked = true;
                BardPacified = false;

                this.PublicOverheadMessage(MessageType.Emote, EmoteHue, false, "*looks furious*");

                if (bSuccess)
                {
                    PlaySound(GetIdleSound());

                    this.Aggressors.Clear();
                    target.Aggressors.Clear();

                    //Add Bard to Each Target's Aggressors List
                    master.DoHarmful(this);
                    master.DoHarmful(target);

                    BardMaster = master;
                    BardTarget = target;
                    BardEndTime = DateTime.Now + TimeSpan.FromSeconds(FeatureList.BardChanges.ProvocationDuration);

                    if (this.AIObject != null)
                    {
                        if (target != null)
                        {
                            this.Combatant = target;
                            this.AIObject.CombatMode();
                        }
                    }

                    BaseCreature t = target as BaseCreature;

                    if (t != null)
                    {
                        if (t.Unprovokable || (t.IsParagon || GetMobileDifficulty.GetDifficultyValue(t) >= FeatureList.BardChanges.MaxProvocationDifficulty))
                            return;

                        t.BardProvoked = true;

                        t.BardMaster = master;
                        t.BardTarget = this;
                        t.BardEndTime = DateTime.Now + TimeSpan.FromSeconds(FeatureList.BardChanges.ProvocationDuration);

                        if (t.AIObject != null)
                        {
                            t.Combatant = this;
                            t.AIObject.CombatMode();
                        }
                    }
                }

                else
                {
                    PlaySound(GetAngerSound());

                    master.DoHarmful(this);
                    master.DoHarmful(target);

                    BardMaster = master;
                    BardTarget = target;
                }
            }
            catch
            {
                LogProvokeIssue("Provoke issue: master=" + master + "  this=" + this + "   target=" + target);
            }
        }

        public static void LogProvokeIssue(string msg)
        {
            using (StreamWriter writer = new StreamWriter("Mapissue.txt", true))
            {
                string message = msg + ":" + DateTime.Now;
                writer.WriteLine(message);
            }
        }

        public bool FindMyName(string str, bool bWithAll)
        {
            int i, j;

            string name = this.Name;

            if (name == null || str.Length < name.Length)
                return false;

            string[] wordsString = str.Split(' ');
            string[] wordsName = name.Split(' ');

            for (j = 0; j < wordsName.Length; j++)
            {
                string wordName = wordsName[j];

                bool bFound = false;
                for (i = 0; i < wordsString.Length; i++)
                {
                    string word = wordsString[i];

                    if (Insensitive.Equals(word, wordName))
                        bFound = true;

                    if (bWithAll && Insensitive.Equals(word, "all"))
                        return true;
                }

                if (!bFound)
                    return false;
            }

            return true;
        }
        public static void TeleportPetsButNotEscortables(Mobile master, Point3D loc)
        {
            List<Mobile> move = new List<Mobile>();

            foreach (Mobile m in master.GetMobilesInRange(3))
            {
                if (m is BaseCreature)
                {
                    BaseCreature pet = (BaseCreature)m;

                    if (pet.Controlled && pet.ControlMaster == master && !(pet is BaseEscortable))
                    {
                        if (pet.ControlOrder == OrderType.Guard || pet.ControlOrder == OrderType.Follow || pet.ControlOrder == OrderType.Come)
                            move.Add(pet);
                    }
                }
            }

            foreach (Mobile m in move)
                m.MoveToWorld(loc, Map.Felucca);
        }
        public static void TeleportPets(Mobile master, Point3D loc, Map map)
        {
            TeleportPets(master, loc, map, false);
        }

        public static void TeleportPets(Mobile master, Point3D loc, Map map, bool onlyBonded)
        {
            List<Mobile> move = new List<Mobile>();

            foreach (Mobile m in master.GetMobilesInRange(3))
            {
                if (m is BaseCreature)
                {
                    BaseCreature pet = (BaseCreature)m;

                    if (pet.Controlled && pet.ControlMaster == master)
                    {
                        if (!onlyBonded || pet.IsBonded)
                        {
                            if (pet.ControlOrder == OrderType.Guard || pet.ControlOrder == OrderType.Follow || pet.ControlOrder == OrderType.Come)
                                move.Add(pet);
                        }
                    }
                }
            }

            foreach (Mobile m in move)
                m.MoveToWorld(loc, map);
        }

        public virtual void ResurrectPet()
        {
            if (!IsDeadPet)
                return;

            OnBeforeResurrect();

            Poison = null;

            Warmode = false;

            Hits = 10;
            Stam = StamMax;
            Mana = 0;

            ProcessDeltaQueue();

            IsDeadPet = false;

            Effects.SendPacket(Location, Map, new BondedStatus(0, this.Serial, 0));

            this.SendIncomingPacket();
            this.SendIncomingPacket();

            OnAfterResurrect();

            Mobile owner = this.ControlMaster;

            if (owner == null || owner.Deleted || owner.Map != this.Map || !owner.InRange(this, 12) || !this.CanSee(owner) || !this.InLOS(owner))
            {
                if (this.OwnerAbandonTime == DateTime.MinValue)
                    this.OwnerAbandonTime = DateTime.Now;
            }
            else
            {
                this.OwnerAbandonTime = DateTime.MinValue;
            }

            CheckStatTimers();
        }

        public override bool CanBeDamaged()
        {
            if (IsDeadPet)
                return false;

            return base.CanBeDamaged();
        }

        public virtual bool PlayerRangeSensitive { get { return (this.CurrentWayPoint == null); } }	//If they are following a waypoint, they'll continue to follow it even if players aren't around

        public override void OnSectorDeactivate()
        {
            if (PlayerRangeSensitive && m_AI != null)
                m_AI.Deactivate();

            base.OnSectorDeactivate();
        }

        public override void OnSectorActivate()
        {
            if (PlayerRangeSensitive && m_AI != null)
                m_AI.Activate();

            base.OnSectorActivate();
        }

        private bool m_RemoveIfUntamed;

        // used for deleting untamed creatures [in houses]
        private int m_RemoveStep;

        [CommandProperty(AccessLevel.GameMaster)]
        public bool RemoveIfUntamed { get { return m_RemoveIfUntamed; } set { m_RemoveIfUntamed = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int RemoveStep { get { return m_RemoveStep; } set { m_RemoveStep = value; } }

        public bool CanInfect { get; internal set; }
    }

    public class LoyaltyTimer : Timer
    {
        private static TimeSpan InternalDelay = TimeSpan.FromMinutes(5.0);

        public static void Initialize()
        {
            new LoyaltyTimer().Start();
        }

        public LoyaltyTimer()
            : base(InternalDelay, InternalDelay)
        {
            m_NextHourlyCheck = DateTime.Now + TimeSpan.FromHours(1.0);
            Priority = TimerPriority.FiveSeconds;
        }

        private DateTime m_NextHourlyCheck;

        protected override void OnTick()
        {
            if (DateTime.Now >= m_NextHourlyCheck)
                m_NextHourlyCheck = DateTime.Now + TimeSpan.FromHours(1.0);
            else
                return;

            List<BaseCreature> toRelease = new List<BaseCreature>();

            // added array for wild creatures in house regions to be removed
            List<BaseCreature> toRemove = new List<BaseCreature>();

            foreach (Mobile m in World.Mobiles.Values)
            {
                if (m is BaseMount && ((BaseMount)m).Rider != null)
                {
                    ((BaseCreature)m).OwnerAbandonTime = DateTime.MinValue;
                    continue;
                }

                if (m is BaseCreature)
                {
                    BaseCreature c = (BaseCreature)m;

                    if (c.IsDeadPet)
                    {
                        Mobile owner = c.ControlMaster;

                        if (owner == null || owner.Deleted || owner.Map != c.Map || !owner.InRange(c, 12) || !c.CanSee(owner) || !c.InLOS(owner))
                        {
                            if (c.OwnerAbandonTime == DateTime.MinValue)
                                c.OwnerAbandonTime = DateTime.Now;
                            else if ((c.OwnerAbandonTime + c.BondingAbandonDelay) <= DateTime.Now)
                                toRemove.Add(c);
                        }

                        else
                        {
                            c.OwnerAbandonTime = DateTime.MinValue;
                        }
                    }

                    else if (c.Controlled && c.Commandable)
                    {
                        c.OwnerAbandonTime = DateTime.MinValue;

                        if (c.Map != Map.Internal)
                        {
                            c.Loyalty -= (BaseCreature.MaxLoyalty / 10);

                            if (c.Loyalty < (BaseCreature.MaxLoyalty / 10))
                            {
                                c.Say(1043270, c.Name); // * ~1_NAME~ looks around desperately *
                                c.PlaySound(c.GetIdleSound());
                            }

                            if (c.Loyalty <= 0)
                                toRelease.Add(c);
                        }
                    }

                    // added lines to check if a wild creature in a house region has to be removed or not
                    if ((!c.Controlled && (c.Region.IsPartOf(typeof(HouseRegion)) && c.CanBeDamaged()) || (c.RemoveIfUntamed && c.Spawner == null)))
                    {
                        c.RemoveStep++;

                        //Add to Remove List after 5 min x 12 = 60 Minutes
                        if (c.RemoveStep >= 12)
                            toRemove.Add(c);
                    }

                    else
                    {
                        c.RemoveStep = 0;
                    }
                }
            }

            foreach (BaseCreature c in toRelease)
            {
                c.Say(1043255, c.Name); // ~1_NAME~ appears to have decided that is better off without a master!
                c.Loyalty = BaseCreature.MaxLoyalty; // Wonderfully Happy
                c.IsBonded = false;
                c.BondingBegin = DateTime.MinValue;
                c.OwnerAbandonTime = DateTime.MinValue;
                c.ControlTarget = null;
                c.AIObject.DoOrderRelease(); // this will prevent no release of creatures left alone with AI disabled (and consequent bug of Followers)
                c.DropBackpack();
            }

            // added code to handle removing of wild creatures in house regions
            foreach (BaseCreature c in toRemove)
            {
                c.Delete();
            }
        }
    }

    public class DamageStore : IComparable
    {
        public Mobile m_Mobile;
        public int m_Damage;
        public bool m_HasRight;

        public DamageStore(Mobile m, int damage)
        {
            m_Mobile = m;
            m_Damage = damage;
        }

        public int CompareTo(object obj)
        {
            DamageStore ds = (DamageStore)obj;

            return ds.m_Damage - m_Damage;
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class FriendlyNameAttribute : Attribute
    {
        //future use: Talisman 'Protection/Bonus vs. Specific Creature
        private TextDefinition m_FriendlyName;

        public TextDefinition FriendlyName
        {
            get
            {
                return m_FriendlyName;
            }
        }

        public FriendlyNameAttribute(TextDefinition friendlyName)
        {
            m_FriendlyName = friendlyName;
        }

        public static TextDefinition GetFriendlyNameFor(Type t)
        {
            if (t.IsDefined(typeof(FriendlyNameAttribute), false))
            {
                object[] objs = t.GetCustomAttributes(typeof(FriendlyNameAttribute), false);

                if (objs != null && objs.Length > 0)
                {
                    FriendlyNameAttribute friendly = objs[0] as FriendlyNameAttribute;

                    return friendly.FriendlyName;
                }
            }

            return t.Name;
        }
    }
}