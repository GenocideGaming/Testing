using System;
using Server.Items;
using Server.Network;
using Server.Targeting;
using Server.Mobiles;
using Server.Spells.Second;
using System.Collections.Generic;
using Server.Scripts.Custom.Citizenship.CommonwealthBonuses;
using Server.Scripts;
using Server.Commands;
using System.IO;
using Server.Factions;
using Server.Scripts.Custom.Citizenship;

namespace Server.Spells
{
    public abstract class Spell : ISpell
    {
        private Mobile m_Caster;
        private Item m_Scroll;
        private SpellInfo m_Info;
        private SpellState m_State;
        private DateTime m_StartCastTime;

        public SpellState State { get { return m_State; } set { m_State = value; } }
        public Mobile Caster { get { return m_Caster; } }
        public SpellInfo Info { get { return m_Info; } }
        public string Name { get { return m_Info.Name; } }
        public string Mantra { get { return m_Info.Mantra; } }
        public Type[] Reagents { get { return m_Info.Reagents; } }
        public Item Scroll { get { return m_Scroll; } }
        public DateTime StartCastTime { get { return m_StartCastTime; } }

        private static TimeSpan AnimateDelay = TimeSpan.FromSeconds(1.5);

        public virtual SkillName CastSkill { get { return SkillName.Magery; } }
        public virtual SkillName DamageSkill { get { return SkillName.EvalInt; } }

        private int m_BaseDamage = 0;
        private int m_DamageVariation = 0;
        private double m_InterruptChance = 0;
        private double m_ResistDifficulty = 0;
        private double m_DamageDelay = 0.0;

        public virtual bool RevealOnCast { get { return true; } }
        public virtual bool ClearHandsOnCast { get { return true; } }
        public virtual bool ShowHandMovement { get { return true; } }

        public virtual bool DelayedDamage { get { return false; } }
        public virtual double DamageDelay { get { return m_DamageDelay; } set { m_DamageDelay = value; } }

        public virtual bool DelayedDamageStacking { get { return true; } }

        private static Dictionary<Type, DelayedDamageContextWrapper> m_ContextTable = new Dictionary<Type, DelayedDamageContextWrapper>();

        
        public const string WatchListFileName = "WatchListPlayerNames.txt";
        public const string WatchListErrorFileName = "ERROR-WatchList.txt";
        public const string WatchListOutputFileName = "WATCH_LIST_OUTPUT.txt";
        public static List<string> WatchList = new List<string>();
        public static void Initialize()
        {
            LoadExemptions(null);
            // add command listener
            CommandSystem.Register("WatchList", AccessLevel.GameMaster, new CommandEventHandler(WatchList_Command));
        }
        
        public static void LoadExemptions(Mobile gm)
        {
            WatchList.Clear();
            // open IPExemptions.txt, fill in Exemptions
            string line;

            // Read the file and display it line by line.
            try
            {
                System.IO.StreamReader file =
                   new System.IO.StreamReader(WatchListFileName);
                while ((line = file.ReadLine()) != null)
                {
                    string[] args = (line.Trim()).Split();
                    if (args.Length < 1 || args[0].StartsWith("#"))
                    {
                        continue;
                    }
                    try
                    {

                        WatchList.Add(args[0]); // the rest of the line is ignored
                        if (gm != null) { gm.SendMessage("Watching: " + args[0]); }
                    }
                    catch (Exception e)
                    {
                        using (StreamWriter writer = new StreamWriter(WatchListErrorFileName, true))
                        {
                            writer.WriteLine("ERROR: " + e.Message);
                        }
                    }
                }
                file.Close();
                if (gm != null) { gm.SendMessage("Successfully loaded Watch List"); }
            }
            catch (Exception e)
            {
                using (StreamWriter writer = new StreamWriter(WatchListErrorFileName, true))
                {
                    writer.WriteLine("ERROR: " + e.Message);
                }
                if (gm != null) { gm.SendMessage("Error: " + e.Message); }
            }
        }

        public static void WatchList_Command(CommandEventArgs e)
        {
            string[] args = e.Arguments;
            string error_msg = null;
            if (args != null)
            {
                if (args.Length > 0)
                {
                    if (args[0].ToLower() == "reload")
                    {
                        LoadExemptions(e.Mobile);
                    }
                    else if (args[0].ToLower() == "add" && args.Length > 1)
                    {
                        try
                        {
                            string watchEntry = null;
                            using (StreamWriter writer = new StreamWriter(WatchListFileName, true))
                            {
                                watchEntry = args[1] + " \t#";
                                for (int i = 2; i < args.Length; i++)
                                {
                                    watchEntry += args[i] + " ";
                                }
                                writer.WriteLine(watchEntry);
                            }
                            e.Mobile.SendMessage("Successfully added entry to " + WatchListFileName + ": " + watchEntry);
                        }
                        catch (Exception exception)
                        {
                            error_msg = "Watchlist command error: " + exception.Message;
                        }
                    }
                    else
                    {
                        error_msg = "Must supply arguments: either 'reload' or 'add [name] [description]', e.g. [watchlist add Red looks like EUO";
                    }
                }
                else
                {
                    error_msg = "Must supply arguments: either 'reload' or 'add [name] [description]', e.g. [watchlist add Red looks like EUO";
                }
            }
            if (error_msg != null)
            {
                e.Mobile.SendMessage(error_msg);
            }
        }

        public virtual int BaseDamage
        {
            get
            {
                return m_BaseDamage;
            }

            set
            {
                m_BaseDamage = value;
            }
        }

        public virtual int DamageVariation
        {
            get
            {
                return m_DamageVariation;
            }

            set
            {
                m_DamageVariation = value;
            }
        }

        public virtual double InterruptChance
        {
            get
            {
                return m_InterruptChance;
            }

            set
            {
                m_InterruptChance = value;
            }
        }

        public virtual double ResistDifficulty
        {
            get
            {
                return m_ResistDifficulty;
            }

            set
            {
                m_ResistDifficulty = value;
            }
        }

        public virtual double GetDamage(Mobile target)
        {
            return DetermineDamage(target, 1.0);
        }

        public virtual double GetDamage(Mobile target , double multiplier)
        {
            return DetermineDamage(target, multiplier);
        }

        public virtual double DetermineDamage(Mobile target, double multiplier)
        {
            double damage = 0.0;

            int min = BaseDamage - DamageVariation;
            int max = BaseDamage + DamageVariation;

            damage = Utility.RandomMinMax(min, max);

            //Spell Multiplier (Conditional)
            damage *= multiplier;

            //Scale Damage With Magery
            double mageryFactor = (100 + ((m_Caster.Skills.Magery.Value - 100) * FeatureList.SpellChanges.MageryDamageMod)) / 100;

            //Scale Damage With Eval Int
            double evalIntFactor = (100 + ((m_Caster.Skills.EvalInt.Value - 100) * FeatureList.SpellChanges.EvalIntDamageMod)) / 100;
                       
            damage *= mageryFactor;
            damage *= evalIntFactor; 

            // Double Magery Damage to Monsters
            if (target is BaseCreature && ((BaseCreature)target).TakesNormalDamage == false)
            {
                damage *= FeatureList.SpellChanges.BaseCreatureSpellDamageMultiplier;
            }

            //Citizenship
            PlayerMobile pmCaster = m_Caster as PlayerMobile;
            PlayerMobile pmTarget = target as PlayerMobile;

            //Citizenship Damage Bonus
            if (pmCaster != null && pmCaster.CitizenshipPlayerState != null)
            {
                //if the caster has magic damage bonus we increase the scalar by 10%
                CommonwealthCombatBonus ccb = pmCaster.CitizenshipPlayerState.GetCombatBonus(CombatBonusTypes.MagicDamage);

                if (ccb != null && pmCaster.CitizenshipPlayerState.Commonwealth.TownRegion.Contains(pmCaster.Location) && ccb.Active)
                {
                    damage *= FeatureList.Citizenship.MagicDamageBonus;
                }
            }

            //Region Modifier
            target.Region.SpellDamageScalar(m_Caster, target, ref damage);             

            //Total Raw Damage (For Display Purposes)
            damage = Math.Round(damage);
            double rawDamage = damage;

            //Reduce Damage for Resist
            double resistFactor = (100 - (target.Skills.MagicResist.Value * FeatureList.SpellChanges.MagicResistDamageReductionMod)) / 100;
            
            damage *= resistFactor;

            //Citizenship Resist Bonus
            if (pmTarget != null && pmTarget.CitizenshipPlayerState != null)
            {
                CommonwealthCombatBonus ccb = pmTarget.CitizenshipPlayerState.GetCombatBonus(CombatBonusTypes.MagicResist);

                if (ccb != null && pmTarget.CitizenshipPlayerState.Commonwealth.TownRegion.Contains(pmTarget.Location) && ccb.Active)
                {
                    //when we get a magic resist bonus we lower the damage by 10%
                    damage *= FeatureList.Citizenship.MagicDamageReductionBonus;
                }
            }            

            //Total Damage Resisted (For Display Purposes)
            damage = Math.Round(damage);
            double damageResisted = rawDamage - damage;

            //Damage Response From Caster
            if (m_Caster is BaseCreature)
                ((BaseCreature)m_Caster).AlterDamageScalarTo(target, ref damage);

            //Damage Response From Target
            if (target is BaseCreature)
                ((BaseCreature)target).AlterDamageScalarFrom(m_Caster, ref damage);

            //Display Spell Data
            if (FeatureList.CombatTesting.ShowDamageFormula)
            {
               m_Caster.SendMessage("Your spell hits. " + rawDamage.ToString() + " - " + damageResisted.ToString() + " = " + damage.ToString());
            }

            return damage;
        }

        public virtual int GetCircle(Mobile m)
        {
            int circle = 1;

            return circle;
        }

        public virtual bool CheckResist(Mobile target)
        {
            return DetermineResist(target, 1.0);
        }

        public virtual bool CheckResist(Mobile target, double multiplier)
        {
            return DetermineResist(target, multiplier);
        }

        public virtual bool DetermineResist(Mobile target, double multiplier)
        {
            double resistChance = ResistDifficulty * (target.Skills[SkillName.MagicResist].Value / 100) * FeatureList.SpellChanges.ResistFullModifier;

            //Modifier BaseCreature Chance for Resist
            if (target is BaseCreature)
            {
                resistChance *= FeatureList.SpellChanges.BaseCreatureResistMultiplier;
            }

            //Check if Resisted
            if (((resistChance) / 100) > Utility.RandomDouble())
            {
                return true;
            }            

            return false;
        }

        public virtual bool CheckInterrupt(Mobile target)
        {
            return DetermineInterrupt(target, 1.0);
        }

        public virtual bool CheckInterrupt(Mobile target, double chance)
        {
            return DetermineInterrupt(target, chance);
        }

        public virtual bool DetermineInterrupt(Mobile target, double chance)
        {
            double interruptChance = InterruptChance * chance;

            if ((interruptChance / 100) > Utility.RandomDouble())
            {
                return true;
            }

            return false;
        }

        private class DelayedDamageContextWrapper
        {
            private Dictionary<Mobile, Timer> m_Contexts = new Dictionary<Mobile, Timer>();

            public void Add(Mobile m, Timer t)
            {
                Timer oldTimer;
                if (m_Contexts.TryGetValue(m, out oldTimer))
                {
                    oldTimer.Stop();
                    m_Contexts.Remove(m);
                }

                m_Contexts.Add(m, t);
            }

            public void Remove(Mobile m)
            {
                m_Contexts.Remove(m);
            }
        }

        public void StartDelayedDamageContext(Mobile m, Timer t)
        {
            if (DelayedDamageStacking)
                return;

            DelayedDamageContextWrapper contexts;

            if (!m_ContextTable.TryGetValue(GetType(), out contexts))
            {
                contexts = new DelayedDamageContextWrapper();
                m_ContextTable.Add(GetType(), contexts);
            }

            contexts.Add(m, t);
        }

        public void RemoveDelayedDamageContext(Mobile m)
        {
            DelayedDamageContextWrapper contexts;

            if (!m_ContextTable.TryGetValue(GetType(), out contexts))
                return;

            contexts.Remove(m);
        }

        public void HarmfulSpell(Mobile m)
        {
            if (m is BaseCreature)
                ((BaseCreature)m).OnHarmfulSpell(m_Caster);
        }

        public Spell(Mobile caster, Item scroll, SpellInfo info)
        {
            m_Caster = caster;
            m_Scroll = scroll;
            m_Info = info;
        }

        public virtual bool IsCasting { get { return m_State == SpellState.Casting; } }

        public virtual void OnCasterHurt()
        {
            this.OnCasterHurt(true);
        }

        public virtual void OnCasterHurt(bool interruptCasting)
        {
            //Confirm: Monsters and pets cannot be disturbed.
            if (!Caster.Player 
                 && Caster is BaseCreature
                 && !(Caster.NetState != null && ((BaseCreature)Caster).Pseu_AllowInterrupts == true))
                return;

            if (!interruptCasting)
                return;

            if (IsCasting)
            {
                object o = ProtectionSpell.Registry[m_Caster];
                bool disturb = true;

                if (o != null && o is double)
                {
                    double roll = Utility.RandomDouble() * 100.0;

                    if (roll < ((double)o))
                    {
                        disturb = false;
                    }
                }

                if (disturb)
                {
                    Disturb(DisturbType.Hurt, false, true);
                }
            }
        }

        public virtual void OnCasterKilled()
        {
            Disturb(DisturbType.Kill);
        }

        public virtual void OnConnectionChanged()
        {
            FinishSequence();
        }

        public virtual bool OnCasterMoving(Direction d)
        {
            if (IsCasting && BlocksMovement)
            {
                m_Caster.SendLocalizedMessage(500111); // You are frozen and can not move.
                return false;
            }

            return true;
        }

        public virtual bool OnCasterEquiping(Item item)
        {
            if (IsCasting)
            {
                Disturb(DisturbType.EquipRequest);
            }

            return true;
        }

        public virtual bool OnCasterUsingObject(object o)
        {
            if (m_State == SpellState.Sequencing)
            {
                Disturb(DisturbType.UseRequest);
            }

            return true;
        }

        public virtual bool OnCastInTown(Region r)
        {
            return m_Info.AllowTown;
        }

        public virtual bool ConsumeReagents()
        {
            BaseCreature bc = m_Caster as BaseCreature;
            if (bc != null)
            {
                if (!bc.Deleted && bc.NetState != null)
                {
                    if (!bc.Pseu_ConsumeReagents) { return true; }
                }
                else
                {
                    return true;
                }
            }
            if (m_Scroll != null)
                return true;

            if (AosAttributes.GetValue(m_Caster, AosAttribute.LowerRegCost) > Utility.Random(100))
                return true;

            Container pack = m_Caster.Backpack;

            if (pack == null)
                return false;

            if (pack.ConsumeTotal(m_Info.Reagents, m_Info.Amounts) == -1)
                return true;

            return false;
        }

        public virtual void DoFizzle()
        {
            m_Caster.LocalOverheadMessage(MessageType.Regular, 0x3B2, 502632); // The spell fizzles.

            if (m_Caster.Player)
            {
                m_Caster.FixedEffect(0x3735, 6, 30);
                m_Caster.PlaySound(0x5C);
            }
        }

        private CastTimer m_CastTimer;
        private AnimTimer m_AnimTimer;

        public void Disturb(DisturbType type)
        {
            Disturb(type, true, false);
        }

        public virtual bool CheckDisturb(DisturbType type, bool firstCircle, bool resistable)
        {
            if (resistable && m_Scroll is BaseWand)
                return false;

            return true;
        }

        public void Disturb(DisturbType type, bool firstCircle, bool resistable)
        {
            if (!CheckDisturb(type, firstCircle, resistable))
                return;

            if (m_State == SpellState.Casting)
            {  
                if (!firstCircle && this is MagerySpell && ((MagerySpell)this).Circle == SpellCircle.First)
                    return;

                m_State = SpellState.None;
                m_Caster.Spell = null;

                OnDisturb(type, true);

                if (m_CastTimer != null)
                    m_CastTimer.Stop();

                if (m_AnimTimer != null)
                    m_AnimTimer.Stop();

                m_Caster.NextSpellTime = DateTime.Now + GetDisturbRecovery();
            }

            else if (m_State == SpellState.Sequencing)
            {
                if (!firstCircle && !Core.AOS && this is MagerySpell && ((MagerySpell)this).Circle == SpellCircle.First)
                    return;

                m_State = SpellState.None;
                m_Caster.Spell = null;

                OnDisturb(type, false);               
                              
                Targeting.Target.Cancel(m_Caster);
            }
        }

        public virtual void DoHurtFizzle()
        {
            m_Caster.FixedEffect(0x3735, 6, 30);
            m_Caster.PlaySound(0x5C);
        }

        public virtual void OnDisturb(DisturbType type, bool message)
        {
            if (message)
                m_Caster.SendLocalizedMessage(500641); // Your concentration is disturbed, thus ruining thy spell.
        }

        public virtual bool CheckCast()
        {
            return true;
        }

        public virtual void SayMantra()
        {
            if (m_Scroll is BaseWand)
                return;

            if (m_Info.Mantra != null && m_Info.Mantra.Length > 0 
                && (m_Caster.Player 
                    || (m_Caster is BaseCreature && ((BaseCreature)m_Caster).PowerWords)))
                m_Caster.PublicOverheadMessage(MessageType.Spell, m_Caster.SpeechHue, true, m_Info.Mantra, false);
        }

        public virtual bool BlockedByHorrificBeast { get { return true; } }
        public virtual bool BlockedByAnimalForm { get { return true; } }
        public virtual bool BlocksMovement { get { return true; } }

        public virtual bool CheckNextSpellTime { get { return !(m_Scroll is BaseWand); } }

        public bool Cast()
        {
            m_StartCastTime = DateTime.Now;            

            if (!m_Caster.CheckAlive())
            {
                return false;
            }

            else if (m_Caster.Spell != null && m_Caster.Spell.IsCasting)
            { 
                m_Caster.SendLocalizedMessage(502642); // You are already casting a spell.
            }

            else if (!(m_Scroll is BaseWand) && (m_Caster.Paralyzed || m_Caster.Frozen))
            {
                m_Caster.SendLocalizedMessage(502643); // You can not cast a spell while frozen.
            }

            else if (CheckNextSpellTime && DateTime.Now < m_Caster.NextSpellTime)
            {                
                m_Caster.SendLocalizedMessage(502644); // You have not yet recovered from casting a spell.
            }

            else if (m_Caster is PlayerMobile && ((PlayerMobile)m_Caster).PeacedUntil > DateTime.Now)
            {
                m_Caster.SendLocalizedMessage(1072060); // You cannot cast a spell while calmed.
            }

            else if (m_Caster.Mana >= ScaleMana(GetMana()))
            {               
                if (m_Caster.Spell == null && m_Caster.CheckSpellCast(this) && CheckCast() && m_Caster.Region.OnBeginSpellCast(m_Caster, this))
                {
                    m_State = SpellState.Casting;
                    m_Caster.Spell = this;                    

                    if (RevealOnCast)
                        m_Caster.RevealingAction();

                    SayMantra();

                    TimeSpan castDelay = this.GetCastDelay();                                       

                    int count = (int)Math.Ceiling(castDelay.TotalSeconds / AnimateDelay.TotalSeconds);  
                  
                    if (ShowHandMovement && m_Caster.Body.IsHuman)
                    {                      
                        if (count != 0)
                        {
                            m_AnimTimer = new AnimTimer(this, count);
                            m_AnimTimer.Start();
                        }

                        if (m_Info.LeftHandEffect > 0)
                            Caster.FixedParticles(0, 10, 5, m_Info.LeftHandEffect, EffectLayer.LeftHand);

                        if (m_Info.RightHandEffect > 0)
                            Caster.FixedParticles(0, 10, 5, m_Info.RightHandEffect, EffectLayer.RightHand);
                    }

                    //BaseCreature Animations For Casting
                    else if (m_Caster is BaseCreature)
                    {
                        if (count != 0)
                        {
                            m_AnimTimer = new AnimTimer(this, count);
                            m_AnimTimer.Start();
                        }
                    }

                    //NPCs keep weapons equipped when casting
                    if (ClearHandsOnCast && (m_Caster.Player || (m_Caster is BaseCreature && ((BaseCreature)m_Caster).ClearHandsOnCast)))
                    {
                        m_Caster.ClearHands();
                    }

                    //Increase SwingDelay for BaseCreatures While Casting (IF they aren't pseudoseer control)
                    if (!m_Caster.Deleted && m_Caster is BaseCreature && m_Caster.NetState == null)
                    {
                        BaseWeapon weapon = m_Caster.Weapon as BaseWeapon;
                       
                        if (weapon != null)
                        {
                            m_Caster.NextCombatTime = DateTime.Now + weapon.GetDelay(m_Caster) + TimeSpan.FromSeconds((double)count);                           
                        }
                    }                   

                    m_CastTimer = new CastTimer(this, castDelay);
                    m_CastTimer.Start();                   

                    OnBeginCast();
                    return true;
                }

                else
                {
                    return false;
                }
            }

            else
            {
                m_Caster.LocalOverheadMessage(MessageType.Regular, 0x22, 502625); // Insufficient mana
            }

            return false;
        }

        public abstract void OnCast();

        public virtual void OnBeginCast()
        {
            LogSpellCast(true);
        }

        public virtual void GetCastSkills(out double min, out double max)
        {
            min = max = 0;	//Intended but not required for overriding.
        }

        public virtual bool CheckFizzle()
        {
            BaseCreature creatureCaster = Caster as BaseCreature;

            if (creatureCaster != null && (creatureCaster.NetState == null || creatureCaster.Pseu_AllowFizzle == false)) // non-possessed creature don't fizzle
            {
                return true;
            }

            else
            {
                if (m_Scroll is BaseWand)
                    return true;

                double minSkill, maxSkill;

                GetCastSkills(out minSkill, out maxSkill);

                //Eval Int Skill Gain Check
                if (DamageSkill != CastSkill)
                    Caster.CheckSkill(DamageSkill, 0.0, Caster.Skills[DamageSkill].Cap);

                //Skill Gain Check
                Caster.CheckSkill(CastSkill, minSkill, maxSkill);

                double casterSkill = Caster.Skills[CastSkill].Value;
                double chance = (casterSkill - minSkill) * .04;
                
                bool success = false;               

                if (chance >= Utility.RandomDouble())
                    success = true;

                return success;
            }                        
        }

        public abstract int GetMana();

        public virtual int ScaleMana(int mana)
        {
            double scalar = 1.0;

            // Lower Mana Cost = 40%
            int lmc = AosAttributes.GetValue(m_Caster, AosAttribute.LowerManaCost);

            if (lmc > 40)
                lmc = 40;

            scalar -= (double)lmc / 100;

            return (int)(mana * scalar);
        }

        public virtual TimeSpan GetDisturbRecovery()
        {
            return FeatureList.SpellDelays.DisruptDelay;
        }

        public virtual int CastRecoveryBase { get { return 6; } }
        public virtual int CastRecoveryFastScalar { get { return 1; } }
        public virtual int CastRecoveryPerSecond { get { return 4; } }
        public virtual int CastRecoveryMinimum { get { return 0; } }

        public virtual TimeSpan GetCastRecovery()
        {
            return FeatureList.SpellDelays.RecastDelay;
        }

        public abstract TimeSpan CastDelayBase { get; }

        public virtual double CastDelayFastScalar { get { return 1; } }
        public virtual double CastDelaySecondsPerTick { get { return 0.25; } }
        public virtual TimeSpan CastDelayMinimum { get { return TimeSpan.FromSeconds(0.25); } }

        public virtual TimeSpan GetCastDelay()
        {
            return CastDelayBase;
        }

        public virtual void FinishSequence()
        {
            LogSpellCast(false);
            if (m_Caster is BaseCreature && !m_Caster.Deleted && m_Caster.NetState == null) // NOTE: can slow down pseudoseer cast time by removing this netstate check.. with it in, they can cast as fast as players
            {
                BaseCreature creature = m_Caster as BaseCreature;

                double SpellDelay = Utility.RandomMinMax(creature.SpellDelayMin, creature.SpellDelayMax);

                creature.NextSpellTime = DateTime.Now + TimeSpan.FromSeconds(SpellDelay);
            }

            m_State = SpellState.None;

            if (m_Caster.Spell == this)
                m_Caster.Spell = null;
        }

        public void LogSpellCast(bool begin)
        {
            if (m_Caster == null) return;
            if (WatchList.Contains(m_Caster.Name))
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(WatchListOutputFileName, true))
                    {
                        DateTime now = DateTime.Now;
                        writer.WriteLine(now.ToLongTimeString() + now.Millisecond + "ms\t" + m_Caster.Name + "\t" + this.Name + "\t" + (begin ? "begin" : "end"));
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("SPELL LOG ERROR: " + e.Message + "\n" + e.StackTrace);
                }
            }
        }

        public virtual int ComputeKarmaAward()
        {
            return 0;
        }

        public virtual bool CheckSequence()
        {
            int mana = ScaleMana(GetMana());

            if (m_Caster.Deleted || !m_Caster.Alive || m_Caster.Spell != this || m_State != SpellState.Sequencing)
            {               
                DoFizzle();
            }

            else if (m_Scroll != null && !(m_Scroll is Runebook) && (m_Scroll.Amount <= 0 || m_Scroll.Deleted || m_Scroll.RootParent != m_Caster || (m_Scroll is BaseWand && (((BaseWand)m_Scroll).Charges <= 0 || m_Scroll.Parent != m_Caster))))
            {               
                DoFizzle();
            }

            else if (!ConsumeReagents())
            {               
                m_Caster.LocalOverheadMessage(MessageType.Regular, 0x22, 502630); // More reagents are needed for this spell.
            }

            else if (m_Caster.Mana < mana)
            {               
                m_Caster.LocalOverheadMessage(MessageType.Regular, 0x22, 502625); // Insufficient mana for this spell.
            }

            else if (m_Caster is PlayerMobile && ((PlayerMobile)m_Caster).PeacedUntil > DateTime.Now)
            {
                m_Caster.SendLocalizedMessage(1072060); // You cannot cast a spell while calmed.
                DoFizzle();
            }

            else if (CheckFizzle())
            {
                m_Caster.Mana -= mana;

                if (m_Scroll is SpellScroll)
                    m_Scroll.Consume();

                else if (m_Scroll is BaseWand)
                    ((BaseWand)m_Scroll).ConsumeCharge(m_Caster);

                if (m_Scroll is BaseWand)
                {
                    bool m = m_Scroll.Movable;

                    m_Scroll.Movable = false;

                    //Clear Hands for Player
                    if (ClearHandsOnCast && m_Caster.Player)
                        m_Caster.ClearHands();

                    m_Scroll.Movable = m;
                }

                else
                {
                    if (ClearHandsOnCast && m_Caster.Player)
                        m_Caster.ClearHands();
                }

                int karma = ComputeKarmaAward();

                if (karma != 0)
                    Misc.Titles.AwardKarma(Caster, karma, true);

                return true;
            }

            else
            {
                DoFizzle();
            }

            return false;
        }

        public bool CheckBSequence(Mobile target)
        {
            return CheckBSequence(target, false);
        }

        public bool CheckBSequence(Mobile target, bool allowDead)
        {
            if (!target.Alive && !allowDead)
            {
                m_Caster.SendLocalizedMessage(501857); // This spell won't work on that!
                return false;
            }

            else if (Caster.CanBeBeneficial(target, true, allowDead) && CheckSequence())
            {
                Caster.DoBeneficial(target);
                return true;
            }

            else
            {
                return false;
            }
        }

        public bool CheckHSequence(Mobile target)
        {
            bool CanBeHarmful = false;
            bool CheckSequenceSuccess = false;                        

            if (!target.Alive)
            {
                m_Caster.SendLocalizedMessage(501857); // This spell won't work on that!
                return false;
            }

            else
            {
                CanBeHarmful = Caster.CanBeHarmful(target);
                CheckSequenceSuccess = CheckSequence();

                if (CanBeHarmful && CheckSequenceSuccess)
                {
                    Caster.DoHarmful(target);

                    return true;
                }                
            }

            return false;
        }

        public Mobile GetMilitiaRestrictedMobileIfAttackable(Mobile from, DamageableItemMilitiaRestricted target)
        {
            Faction militia = Faction.Find(from);

            if (militia == null)
            {
                if (WorldWarsController.Instance != null && WorldWarsController.Instance.Active)
                {
                    ICommonwealth commonwealth = Commonwealth.Find(from);
                    if (commonwealth == null)
                    {
                        from.SendMessage("Only citizens can attack this item!");
                    }
                    else if (commonwealth.Militia == target.Militia)
                    {
                        from.SendMessage("You cannot attack this item, it belongs to your militia!");
                        return null;
                    }
                    else
                    {
                        return (Mobile)(target.Link);
                    }
                }
                else
                {
                    from.SendMessage("Only militia members can attack this item!");
                    return null;
                }
            }
            else if (militia != null && militia == target.Militia)
            {
                from.SendMessage("You cannot attack this item, it belongs to your militia!");
                return null;
            }
            else
            {
                return (Mobile)(target.Link);
            }
            return null;
        }

        private class AnimTimer : Timer
        {
            private Spell m_Spell;

            public AnimTimer(Spell spell, int count)
                : base(TimeSpan.Zero, AnimateDelay, count)
            {
                m_Spell = spell;

                Priority = TimerPriority.FiftyMS;
            }

            protected override void OnTick()
            {
                if (m_Spell.State != SpellState.Casting || m_Spell.m_Caster.Spell != m_Spell)
                {
                    Stop();
                    return;
                }

                if (!m_Spell.Caster.Mounted && m_Spell.Caster.Body.IsHuman && m_Spell.m_Info.Action >= 0)
                    m_Spell.Caster.Animate(m_Spell.m_Info.Action, 7, 1, true, false, 0);
                                    
                else if (m_Spell.Caster is BaseCreature && m_Spell.m_Info.Action >= 0)
                {
                    BaseCreature creature = m_Spell.Caster as BaseCreature;

                    m_Spell.Caster.Animate(creature.SpellCastAnimation, creature.SpellCastFrameCount, 1, true, false, 0);
                }

                if (!Running)
                    m_Spell.m_AnimTimer = null;
            }
        }

        private class CastTimer : Timer
        {
            private Spell m_Spell;

            public CastTimer(Spell spell, TimeSpan castDelay)
                : base(castDelay)
            {
                m_Spell = spell;

                Priority = TimerPriority.TwentyFiveMS;
            }

            protected override void OnTick()
            {
                if (m_Spell.m_State == SpellState.Casting && m_Spell.m_Caster.Spell == m_Spell)
                {
                    m_Spell.m_State = SpellState.Sequencing;
                    m_Spell.m_CastTimer = null;
                    m_Spell.m_Caster.OnSpellCast(m_Spell);
                    m_Spell.m_Caster.Region.OnSpellCast(m_Spell.m_Caster, m_Spell);
                    m_Spell.m_Caster.NextSpellTime = DateTime.Now + m_Spell.GetCastRecovery();// Spell.NextSpellDelay;
                    if (!m_Spell.m_Caster.Deleted 
                        && m_Spell.m_Caster.NetState != null 
                        && m_Spell.m_Caster is BaseCreature) // pseudoseer controlled
                    {
                        m_Spell.m_Caster.NextSpellTime += ((BaseCreature)m_Spell.m_Caster).Pseu_SpellDelay;
                    }

                    Target originalTarget = m_Spell.m_Caster.Target;
                    m_Spell.OnCast();

                    if ((m_Spell.m_Caster.Player || m_Spell.m_Caster.NetState != null) && m_Spell.m_Caster.Target != originalTarget && m_Spell.Caster.Target != null)
                        m_Spell.m_Caster.Target.BeginTimeout(m_Spell.m_Caster, TimeSpan.FromSeconds(30.0));

                    m_Spell.m_CastTimer = null;
                }
            }
        }
    }
}