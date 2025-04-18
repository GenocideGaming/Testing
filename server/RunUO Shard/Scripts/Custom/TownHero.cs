using System;
using System.Collections.Generic;
using System.Text;
using Server.Mobiles;
using Server.Items;
using Server.Regions;
using Server.Scripts.Custom.Citizenship;
using Server.Scripts.Custom;
using Server.Factions;
using Server.Scripts;
using Server.Commands;
using Server.Targeting;
using Server.Scripts.Custom.Citizenship.CommonwealthBonuses;

namespace Server.Mobiles
{

    public class TownHero : BaseEscortable
    {
        private MilitiaStronghold m_HomeRegion;
        private ICommonwealth m_OriginalOwner;
        private ICommonwealth m_CurrentOwner;
        
        private Mobile m_Escorter;

        private InactiveTimer m_WindowOfOpportunityTimer;

        private CapturedTimer m_CapturedTimer;
        private DateTime m_CapturedTimerEnd = DateTime.MinValue;

        private CaptureImmunityTimer m_CaptureImmunityTimer;
        private DateTime m_CaptureImmunityTimerEnd = DateTime.MinValue;

        private HERO_STATES m_State;
        
        private bool m_IsHome;

        [CommandProperty(AccessLevel.GameMaster)]
        public string Escorter
        {
            get
            {
                return m_Escorter.Name;

            }
        }
        
        public override bool Commandable { get { return false; } }
        
        [CommandProperty(AccessLevel.GameMaster)]
        public MilitiaStronghold MyHomeRegion { get { return m_HomeRegion; } set { m_HomeRegion = value; } }
        
        [CommandProperty(AccessLevel.GameMaster)]
        public HERO_STATES State { get { return m_State; } }
        
        [CommandProperty(AccessLevel.GameMaster)]
        public ICommonwealth OriginalOwner { get { return m_OriginalOwner; } set { m_OriginalOwner = value; } }
        
        [CommandProperty(AccessLevel.GameMaster)]
        public ICommonwealth Owner { get { return m_CurrentOwner; } set { m_CurrentOwner = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public virtual bool IsHome
        {
            get
            {
                return CoreFactory.IsTest ? m_IsHome : m_HomeRegion.Contains(Location);
            }
            set { m_IsHome = value; }
 
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime CaptureTimerEnd { get { return m_CapturedTimerEnd; } }


        public static void Initialize()
        {
            CommandSystem.Register("ResetHero", AccessLevel.GameMaster, new CommandEventHandler(OnCommand_ResetHero));
        }

        public static void OnCommand_ResetHero(CommandEventArgs e)
        {
            e.Mobile.SendMessage("Target the town hero you wish to reset");
            e.Mobile.BeginTarget(15, false, TargetFlags.None, new TargetCallback(OnTarget_TownHero));
        }
        public TownHero(ICommonwealth township)
            : this()
        {
            m_OriginalOwner = township;
            m_CurrentOwner = township;
            m_HomeRegion = m_CurrentOwner.HeroHomeRegion;
            Name = "Hero of " + m_CurrentOwner.Definition.TownName;
            YellowHealthbar = true;
            Blessed = true;
        }

        public override bool Escortable { get { return false; } }

        public TownHero()
            : base()
        {
            m_State = HERO_STATES.Protected;
        }

        [Constructable]
        public TownHero(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            Commonwealth.WriteReference(writer, m_OriginalOwner);
            Commonwealth.WriteReference(writer, m_CurrentOwner);
            writer.Write((int)m_State);
            writer.Write((DateTime)m_CapturedTimerEnd);
            writer.Write((DateTime)m_CaptureImmunityTimerEnd);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_OriginalOwner = Commonwealth.ReadReference(reader);
            m_CurrentOwner = Commonwealth.ReadReference(reader);
            

            m_State = (HERO_STATES)reader.ReadInt();
            m_CapturedTimerEnd = reader.ReadDateTime();
            m_CaptureImmunityTimerEnd = reader.ReadDateTime();

            if (m_CapturedTimerEnd != DateTime.MinValue)
            {
                m_CapturedTimer = new CapturedTimer(this, m_CapturedTimerEnd);
                m_CapturedTimer.Start();
            }
            if (m_CaptureImmunityTimerEnd != DateTime.MinValue)
            {
                m_CaptureImmunityTimer = new CaptureImmunityTimer(this, m_CaptureImmunityTimerEnd);
                m_CaptureImmunityTimer.Start();
            }

            m_HomeRegion = m_OriginalOwner.HeroHomeRegion;
        }

        public override void InitBody()
        {
            SetStr(100, 100);
            SetDex(100, 100);
            SetInt(100, 100);

            Hue = Utility.RandomSkinHue();

            Body = 401; //always female

        }

        public override bool BardImmune { get { return true; } }
        public override bool CanBeDamaged()
        {
            return false;
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
           // base.OnSpeech(e); 

            if (CanFollow() && !e.Handled && e.Speech.Contains("capture"))
            {
                e.Handled = SaySomething(e.Mobile);

                if (e.Handled)
                    AcceptEscorter(e.Mobile);
            }
        }

        private bool SaySomething(Mobile mobile)
        {
            if (m_Escorter == null)
            {
                Say(FeatureList.Heroes.OnFollowSayText);
                return true;
            }
            return false;
        }
        public override bool AcceptEscorter(Mobile m)
        {
            if (m_State == HERO_STATES.Protected || m_State == HERO_STATES.Captured)
                return false;

            Mobile escorter = GetEscorter();

            if (escorter != null)
                return false;

            if (m is PlayerMobile)
            {
                PlayerMobile player = m as PlayerMobile;
                if (player.FactionPlayerState == null && player.AccessLevel == AccessLevel.Player)
                {
                    m.SendMessage("Only militia members can lead a hero!");
                    return false;
                }
                if (player.FactionPlayerState != null && player.FactionPlayerState.Faction == m_OriginalOwner.Militia && m_State == HERO_STATES.Protected)
                {
                    m.SendMessage("You cannot capture your own hero!");
                    return false;
                }
                if (m_State == HERO_STATES.WaitingAroundForRescue && player.AccessLevel == AccessLevel.Player && player.FactionPlayerState.Faction.OwningCommonwealth != m_HomeRegion.Owner)
                {
                    m.SendMessage("A hero can only be led by their own militia during a rescue!");
                    return false;
                }
            }
            if (SetControlMaster(m))
            {
                StartFollow();
                return true;
            }

            return false;
        }

        public override void StartFollow(Mobile escorter)
        {
            base.StartFollow(escorter);
            m_Escorter = escorter;
            if (Faction.Find(escorter) == m_OriginalOwner.Militia)
                ChangeState(HERO_STATES.FollowingFriend);
            else
                ChangeState(HERO_STATES.FollowingEnemy);

            ActiveSpeed = GetActiveSpeed(escorter);
            PassiveSpeed = 0.4;
            CurrentSpeed = ActiveSpeed;

        }
        public double GetActiveSpeed(Mobile escorter)
        {
            int myHeroSettings = m_OriginalOwner.GetHeroSettings();
            ICommonwealth escorterTownship = Commonwealth.Find(escorter);

            if (escorterTownship == null) //should never happen
                return FeatureList.Heroes.DefaultActiveSpeed;

            if (m_OriginalOwner == escorterTownship)
            {
                if ((myHeroSettings & (int)HeroBonusTypes.Intimidation) != 0)
                {
                    return FeatureList.Heroes.DefaultActiveSpeed + FeatureList.Heroes.IntimidatedActiveSpeedAdjuster;
                }
                else
                {
                    return FeatureList.Heroes.DefaultActiveSpeed;
                }
            }

            int escorterHeroSettings = escorterTownship.GetHeroSettings();

            if ((myHeroSettings & (int)HeroBonusTypes.Defiance) != 0 && (escorterHeroSettings & (int)HeroBonusTypes.Intimidation) != 0)
            {
                return FeatureList.Heroes.DefaultActiveSpeed + FeatureList.Heroes.DefiantActiveSpeedAdjuster + FeatureList.Heroes.IntimidatedActiveSpeedAdjuster;
            }
            else if ((myHeroSettings & (int)HeroBonusTypes.Defiance) != 0)
            {
                return FeatureList.Heroes.DefaultActiveSpeed + FeatureList.Heroes.DefiantActiveSpeedAdjuster;
            }
            else if((escorterHeroSettings & (int)HeroBonusTypes.Intimidation) != 0)
            {
                return FeatureList.Heroes.DefaultActiveSpeed + FeatureList.Heroes.IntimidatedActiveSpeedAdjuster;
            }
            else
            {
                return FeatureList.Heroes.DefaultActiveSpeed;
            }
        }
        public bool CanFollow()
        {
            if (m_State == HERO_STATES.WaitingAroundForCapture || m_State == HERO_STATES.WaitingAroundForRescue)
                return true;
            else
                return false;
        }

        protected override bool OnMove(Direction d)
        {
            if (m_State == HERO_STATES.FollowingEnemy || m_State == HERO_STATES.FollowingFriend)
                return true;
            else
                return false;
        }
        public override void StopFollow()
        {
            base.StopFollow();
            SetControlMaster(null);
            m_Escorter = null;
        }
        public override void OnThink()
        {
            CheckEscorter();
        }


        private void CheckEscorter()
        {
            Mobile master = GetEscorter();

            if (master == null)
                return;

            if (master.Deleted || master.Map != this.Map || !master.InRange(Location, 30) || !master.Alive || master.Hidden)
            {
                TimeSpan lastSeenDelay = DateTime.Now - m_LastSeenEscorter;
                if (lastSeenDelay >= TimeSpan.FromSeconds(10))
                {
                    master.SendLocalizedMessage(1042473); // You have lost the person you were escorting.
                    StopFollow();
                    SetControlMaster(null);

                    Console.WriteLine("Hero escorter is no longer alive or in range, going idle");
                    ChangeState(HERO_STATES.WaitingAroundForCapture);
                    return;
                }
                else
                {
                    ControlOrder = OrderType.Stay;
                    return;
                }
            }

            if (ControlOrder != OrderType.Follow)
                StartFollow(master);
            m_LastSeenEscorter = DateTime.Now;
        }

        public override Mobile GetEscorter()
        {
            if (!Controlled)
                return null;
            if (ControlMaster == null)
                return null;

            return ControlMaster;
        }
        public void ChangeState(HERO_STATES newState)
        {
            //this may fix the bug that i fixed with null checks and timer stopping a few lines down, i left the other stuff in though, for now
            if (newState == m_State)
                return;

            switch (newState)
            {
                case HERO_STATES.WaitingAroundForCapture:
                    //   no need for clearing the timers or the 1 minute spawn at home timer...
                    //   rely on the WindowOfOpportunity timer instead (30 minutes)
                    //ClearAllTimers();  // also, inactivity timer is not used anymore
                    //m_InactivityTimer = new InactiveTimer(this, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(InactivityTimeInSeconds));
                    //m_InactivityTimer.Start();
                    break;
                case HERO_STATES.CaptureImmune:
                    ClearAllTimers();
                    m_CaptureImmunityTimer = new CaptureImmunityTimer(this, TimeSpan.FromHours(FeatureList.Heroes.CaptureImmunityTimerLengthInHours));
                    m_CaptureImmunityTimer.Start();
                    m_CaptureImmunityTimerEnd = m_CaptureImmunityTimer.DropImmunityTime;
                    break;
                case HERO_STATES.Captured:
                    ClearAllTimers();
                    m_CapturedTimer = new CapturedTimer(this, TimeSpan.FromHours(FeatureList.Heroes.CapturedTimerLengthInHours));
                    m_CapturedTimer.Start();
                    m_CapturedTimerEnd = m_CapturedTimer.ReturnHomeTime;
                    break;
                case HERO_STATES.FollowingEnemy:
                    //ClearAllTimers(); // moved this to the OnDropProtection so it doesn't keep resetting
                    //m_WindowOfOpportunityTimer = new InactiveTimer(this, TimeSpan.FromSeconds(0), TimeSpan.FromMinutes(FeatureList.Heroes.WindowOfOpportunityTimerLengthInMinutes));
                    //m_WindowOfOpportunityTimer.Start();
                    break;
                case HERO_STATES.FollowingFriend:
                    //ClearAllTimers(); // moved this to the OnDropProtection so it doesn't keep resetting
                    //m_WindowOfOpportunityTimer = new InactiveTimer(this, TimeSpan.FromSeconds(0), TimeSpan.FromMinutes(FeatureList.Heroes.WindowOfOpportunityTimerLengthInMinutes));
                    //m_WindowOfOpportunityTimer.Start();
                    break;
                case HERO_STATES.Protected:
                    StopFollow();
                    ClearAllTimers();                    
                    break;
            }

            m_State = newState;
        }
        public void ReturnToCapturer()
        {
            Point3D goLocation = this.m_CurrentOwner.GetCellLocationForAlreadyCapturedHero(this);

            if (goLocation == new Point3D(0, 0, 0) || this.m_CurrentOwner == this.m_OriginalOwner)
            {
                Timer.DelayCall(TimeSpan.FromSeconds(1.0), new TimerCallback(ReturnHome));
                return;
            }
            Say(FeatureList.Heroes.OnCaptureSayText);
            StopFollow();

            MoveToWorld(goLocation, Map.Felucca);
            ChangeState(HERO_STATES.Captured);
            m_CurrentOwner.HookupCellDoor(this);
            m_CurrentOwner.OnOtherHeroCapture(m_OriginalOwner);
        }
        public void ReturnHome()
        {
            if (m_CurrentOwner != m_OriginalOwner)
                m_CurrentOwner.OnOtherHeroReturnHome(this);

            Say(FeatureList.Heroes.OnReturnHomeSayText);
            m_CurrentOwner = m_OriginalOwner;
            MoveToWorld(m_OriginalOwner.HeroGoLocation, Map);
            m_CurrentOwner.OnMyHeroReturnHome();
            ChangeState(HERO_STATES.Protected);
        }
        public void ReturnHomeAndGoCaptureImmune()
        {
            Say(FeatureList.Heroes.OnReturnHomeAfterCaptureSayText);
            if (m_CurrentOwner != m_OriginalOwner)
            {
                m_CurrentOwner.OnOtherHeroReturnHome(this);
            }
            m_CurrentOwner = m_OriginalOwner;
            MoveToWorld(m_OriginalOwner.HeroGoLocation, Map);
            m_CurrentOwner.OnMyHeroReturnHome();
            ChangeState(HERO_STATES.CaptureImmune);
           
        }
        public void OnCaptureImmunityEnd()
        {
            Say(FeatureList.Heroes.OnCaptureImmunityEndSayText);
            ChangeState(HERO_STATES.Protected);
        }
        public void OnCapture(ICommonwealth capturingCommonwealth)
        {
            Point3D goLocation = capturingCommonwealth.AcceptCaptive();

            if (goLocation == new Point3D(0, 0, 0))
            {
                Timer.DelayCall(TimeSpan.FromSeconds(1.0), new TimerCallback(ReturnHome));
                return;
            }

            Say(FeatureList.Heroes.OnCaptureSayText);
            PlayerMobile capturingPlayer = ControlMaster as PlayerMobile;
            if (capturingPlayer != null)
            {
                capturingPlayer.HeroesCapturedThisSession++;
            }
            StopFollow();
            MoveToWorld(goLocation, Map.Felucca);  
            ChangeState(HERO_STATES.Captured);
            m_CurrentOwner = capturingCommonwealth;
            m_CurrentOwner.HookupCellDoor(this);
            m_CurrentOwner.OnOtherHeroCapture(m_OriginalOwner);
            m_OriginalOwner.OnMyHeroCapture(capturingCommonwealth);
            
        }
        public void OnRescue()
        {
            Say(FeatureList.Heroes.OnRescueSayText);
            PlayerMobile rescuingPlayer = ControlMaster as PlayerMobile;
            if (rescuingPlayer != null)
            {
                rescuingPlayer.HeroesRescuedThisSession++;
            }
            StopFollow();
            ChangeState(HERO_STATES.Protected);
            if (m_CurrentOwner != m_OriginalOwner)
            {
                m_CurrentOwner.OnOtherHeroRescue(m_OriginalOwner);
            }
            m_CurrentOwner = m_OriginalOwner;
            m_CurrentOwner.OnMyHeroRescue();
            MoveToWorld(m_CurrentOwner.HeroGoLocation, Map.Felucca);
        }
        public void OnDropProtection()
        {
            if (m_CurrentOwner == m_OriginalOwner)
            {
                Say(FeatureList.Heroes.OnWaitingAroundForCaptureSayText);
                ChangeState(HERO_STATES.WaitingAroundForCapture);                
            }
            else
            {
                Say(FeatureList.Heroes.OnWaitingAroundForRescueSayText);
                ChangeState(HERO_STATES.WaitingAroundForRescue);
            }
            ClearAllTimers();
            m_WindowOfOpportunityTimer = new InactiveTimer(this, TimeSpan.FromSeconds(0), TimeSpan.FromMinutes(FeatureList.Heroes.WindowOfOpportunityTimerLengthInMinutes));
            m_WindowOfOpportunityTimer.Start();
        }
        public void ClearAllTimers()
        {
            if (m_WindowOfOpportunityTimer != null)
            {
                m_WindowOfOpportunityTimer.Stop();
                m_WindowOfOpportunityTimer = null;
            }
            if (m_CapturedTimer != null)
            {
                m_CapturedTimer.Stop();
                m_CapturedTimer = null;
                m_CapturedTimerEnd = DateTime.MinValue;
            }
            if (m_CaptureImmunityTimer != null)
            {
                m_CaptureImmunityTimer.Stop();
                m_CaptureImmunityTimer = null;
                m_CaptureImmunityTimerEnd = DateTime.MinValue;
            }
        }
        public override void OnRegionChange(Region Old, Region New)
        {
            base.OnRegionChange(Old, New);
            if (New is MilitiaStronghold)
            {
                MilitiaStronghold stronghold = New as MilitiaStronghold;
                if (stronghold == m_HomeRegion && stronghold.Owner.Militia != null && m_CurrentOwner != m_OriginalOwner && m_Escorter != null)
                {
                    stronghold.Owner.Militia.AwardSilverForHeroRescue(this);
                    StopFollow();
                    OnRescue();
                    return;
                }

                if (stronghold != m_HomeRegion && stronghold.Owner.Militia != null && m_Escorter != null)
                {
                    if (m_CurrentOwner != null && m_CurrentOwner == stronghold.Owner) 
                    {
                        // already had the hero captured, bringing back into the stronghold from outside
                        m_Escorter.SendMessage(0xFE, "You must have the hero in your stronghold when the 30 minute timer expires (from the time the door was broken) in order to recapture it!");
                    }
                    else
                    {
                        stronghold.Owner.Militia.AwardSilverForHeroCapture(this);
                        OnCapture(stronghold.Owner);
                    }
                }
            }

           /* if(Old is MilitiaStronghold)
            {
                if(Old == m_HomeRegion)
                {
                    
                }
                else //we're being rescued
                {
                    m_CurrentOwner
                }
            }*/
        }
        public override bool CanBeBeneficial(Mobile target)
        {
            return false;
        }
        public override bool CanBeHarmful(Mobile target)
        {
            return false;
        }
        private static void OnTarget_TownHero(Mobile from, object obj)
        {
            if (obj is TownHero)
            {

                TownHero hero = obj as TownHero;
                hero.ReturnHome();
            }
            else
                from.SendMessage("That is not a town hero");
        }
        
        public class InactiveTimer : Timer
        {
            private TownHero mHero;
            private DateTime mReturnHomeTime;
            private DateTime mStartTime;

            public DateTime ReturnHomeTime { get { return mReturnHomeTime; } }
            public InactiveTimer(TownHero m, TimeSpan delay, TimeSpan timerLength)
                : base(delay, TimeSpan.FromSeconds(10))
            {
                mHero = m;
                Priority = TimerPriority.OneSecond;
                mStartTime = DateTime.Now;
                mReturnHomeTime = mStartTime.Add(timerLength);

            }

            protected override void OnTick()
            {
                base.OnTick();

                if (DateTime.Now > mReturnHomeTime)
                {
                    // if the hero is in the captor's stronghold when the timer runs out, he pops back into the cell
                    if (mHero.m_CurrentOwner != null && mHero.m_CurrentOwner != mHero.m_OriginalOwner && mHero.Region == mHero.m_CurrentOwner.HeroHomeRegion)
                    {
                        mHero.ReturnToCapturer();
                    }
                    else
                    {
                        mHero.ReturnHome();
                    }
                }
            }
        }

        private class CapturedTimer : Timer
        {
            private TownHero mHero;
            private DateTime mReturnHomeTime;
            private DateTime mStartTime;

            public DateTime ReturnHomeTime { get { return mReturnHomeTime; } }
            public CapturedTimer(TownHero m, TimeSpan timerLength)
                : base(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(30))
            {
                mHero = m;
                mStartTime = DateTime.Now;
                mReturnHomeTime = mStartTime.Add(timerLength);
            }
            public CapturedTimer(TownHero m, DateTime returnHomeTime)
                : base(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(30))
            {
                mHero = m;
                mReturnHomeTime = returnHomeTime;
            }
            protected override void OnTick()
            {
                base.OnTick();

                if (DateTime.Now > mReturnHomeTime)
                {
                    if (!mHero.IsHome && mHero.OriginalOwner != null)
                        mHero.ReturnHomeAndGoCaptureImmune();
                    else
                        this.Stop();
                }
            }
        
        }

        private class CaptureImmunityTimer : Timer
        {
            private TownHero mHero;
            private DateTime mDropImmunityTime;
            private DateTime mStartTime;

            public DateTime DropImmunityTime { get { return mDropImmunityTime; } }
            public CaptureImmunityTimer(TownHero m, TimeSpan timerLength)
                : base(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(10))
            {
                mHero = m;
                mStartTime = DateTime.Now;
                mDropImmunityTime = mStartTime.Add(timerLength);
            }
            public CaptureImmunityTimer(TownHero hero, DateTime dropImmunityTime)
                : base(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(10))
            {
                mHero = hero;
                mDropImmunityTime = dropImmunityTime;
            }
            protected override void OnTick()
            {
                base.OnTick();
                if (DateTime.Now > mDropImmunityTime)
                {
                    mHero.OnCaptureImmunityEnd();
                    this.Stop();
                }
            }
        }
    }
    public enum HERO_STATES
    {
        Protected,
        WaitingAroundForCapture,
        WaitingAroundForRescue,
        FollowingEnemy,
        FollowingFriend,
        Captured,
        CaptureImmune
    }

}