using System;
using System.Collections.Generic;
using System.Text;
using Server.Mobiles;
using Server.Items;
using Server.Scripts.Custom.Citizenship;
using Server.Factions;
using Server.Scripts.Custom.Citizenship.CommonwealthBonuses;

namespace Server.Scripts.Custom
{
    public class DestructableDoor : DamageableItemMilitiaRestricted
    {
        protected ICommonwealth mCommonwealth;
        private RefreshTimer mRefreshTimer;
        private HealTimer mHealTimer;
        protected DestructableDoor mPartner;
        protected bool mDying;
        protected int mNextHealthMessage;
        protected int mHealthMessageBuffer = 100;
        public DestructableDoor Partner { get { return mPartner; } set { mPartner = value; } }
        public bool Dying { get { return mDying; } }

        public DestructableDoor(Commonwealth township, DestructableDoor partner, int startID, int halfID, int deadID) : base(startID, halfID, deadID, township.Militia)
        {
            Movable = false;
            Level = ItemLevel.Hard;
            mCommonwealth = township;
            mPartner = partner;
            mRefreshTimer = new RefreshTimer(this);
            mRefreshTimer.Start();
            mHealTimer = new HealTimer(this);
            mHealTimer.Start();
        }

        public DestructableDoor(Commonwealth township)
            : base(0x06B9, 0x0C2D, 0x0C2D, township.Militia)
        {
            Movable = false;
            Level = ItemLevel.Easy;
            mCommonwealth = township;
            mNextHealthMessage = HitsMax - mHealthMessageBuffer; //give it a small buffer so slight health changes don't trigger a message
            mRefreshTimer = new RefreshTimer(this);
            mRefreshTimer.Start();
            mHealTimer = new HealTimer(this);
            mHealTimer.Start();
        }
        public DestructableDoor(Serial serial) : base(serial) { }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            mCommonwealth = Commonwealth.ReadReference(reader);
            mPartner = (DestructableDoor)reader.ReadItem();

            if (Hits < HitsMax)
                mNextHealthMessage = Hits - (int)(HitsMax * 0.1); //i don't love this but it shouldn't be an issue, it just fires every 10% after this value
            else
                mNextHealthMessage = HitsMax - mHealthMessageBuffer; 

            mRefreshTimer = new RefreshTimer(this);
            mRefreshTimer.Start();
            mHealTimer = new HealTimer(this);
            mHealTimer.Start();

        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            Commonwealth.WriteReference(writer, mCommonwealth);
            writer.WriteItem<DestructableDoor>(mPartner);
        }

        public override bool Destroy()
        {
            return base.Destroy();
        }

        public void Refresh()
        {
            int heroBonusSettings = mCommonwealth.GetHeroSettings();
            int baseHits = (int)(100 + (double)(((int)Level * 100.0) * ((int)Level * 5)));
            if ((heroBonusSettings & (int)HeroBonusTypes.StrongDoors) != 0)
            {
                int alteredMax = baseHits;
                for (int i = 0; i < mCommonwealth.CapturedHeroes.Count; i++)
                {
                    alteredMax += (int)(baseHits * FeatureList.Citizenship.HeroBonusIncreasePerCapturedHero);
                }

                if (!mCommonwealth.Hero.IsHome)
                {
                    alteredMax -= (int)(baseHits * FeatureList.Citizenship.AbsentHeroHeroBonusReduction);
                }

                HitsMax = alteredMax;
            }
            else
            {
                int alteredMax = baseHits;
                if (!mCommonwealth.Hero.IsHome)
                {
                    alteredMax -= (int)(baseHits * FeatureList.Citizenship.AbsentHeroHeroBonusReduction);
                }

                HitsMax = alteredMax;
            }

            ItemID = IDStart;
            Hits = HitsMax;
            mNextHealthMessage = HitsMax - mHealthMessageBuffer;
        }
        public void RegenHealth()
        {
            if (mCommonwealth == null || mCommonwealth.HeroHomeRegion == null) return;
            List<Mobile> players = mCommonwealth.HeroHomeRegion.GetPlayers();
            foreach (Mobile p in players)
            {
                PlayerMobile player = p as PlayerMobile;
                if (player.CitizenshipPlayerState != null && player.CitizenshipPlayerState.Commonwealth == mCommonwealth)
                {
                    int regen = (int) FeatureList.Heroes.HeroDoorHealthRegenPerMinute * HitsMax;
                    Hits += regen;
                    if (Hits >= (HitsMax * 0.5))
                    {
                        ItemID = IDStart;
                    }
                    else
                    {
                        ItemID = IDHalfHits;
                    }
                    InvalidateProperties();
                    return;
                }
            }
        }
        public void BroadcastAttack()
        {
            mCommonwealth.Broadcast(0x30, FeatureList.Heroes.DoorReportAttackMessage);
            foreach (Commonwealth commonwealth in Commonwealth.Commonwealths)
            {
                if (mCommonwealth != commonwealth)
                {
                    commonwealth.Militia.Broadcast(0x30, mCommonwealth.TownRegion.Name + "'s " + FeatureList.Heroes.OtherFactionDoorReportAttackMessage); // 0xFE = light colored gold message.. 0x30 = yellow
                }
            }
            mNextHealthMessage -= (int)(HitsMax * 0.1);
        }
        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            base.OnDamage(amount, from, willKill);
        }
        public override void Damage(int amount, Mobile from, bool willKill)
        {
            base.Damage(amount, from, willKill);
            mRefreshTimer.Restart();

            if (Hits < mNextHealthMessage)
            {
                BroadcastAttack();
            }
        }
        private class HealTimer : Timer
        {
            private DestructableDoor mDoor;
            public HealTimer(DestructableDoor door)
                : base(TimeSpan.FromSeconds(1), TimeSpan.FromMinutes(1))
            {
                mDoor = door;
            }

            protected override void OnTick()
            {
                mDoor.RegenHealth();
                base.OnTick();
            }
        }
        private class RefreshTimer : Timer
        {
            private DestructableDoor mDoor;
            private DateTime mStartTime;

            public RefreshTimer(DestructableDoor door)
                : base(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(300))
            {
                mDoor = door;
                mStartTime = DateTime.Now;
            }

            public void Restart()
            {
                mStartTime = DateTime.Now;
            }
            protected override void OnTick()
            {
                base.OnTick();
                if (DateTime.Now - mStartTime >= TimeSpan.FromMinutes(FeatureList.Heroes.HeroDoorRegenTimerInMinutes))
                {
                    mDoor.Refresh();
                    mStartTime = DateTime.Now;
                }
            }
        }

    }

    public class HeroProtectionDoorLeft : DestructableDoor
    {
        public HeroProtectionDoorLeft(Commonwealth township, HeroProtectionDoorRight partner)
            : base(township, partner, 0x06B9, 0x0C2D, 0x0C2D)
        {
        }

        public HeroProtectionDoorLeft(Serial serial) : base(serial) { }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override bool Destroy()
        {
            //Had to throw in a fix for this as it gets called once for each door, starting two timers, we probably need a better solution to this
            if (mCommonwealth != null && mCommonwealth.Hero != null)
            {
                mCommonwealth.Hero.OnDropProtection();
            }
            if (mPartner != null && !mPartner.Dying)
            {
                mDying = true;
                mPartner.Destroy();
            }

            mPartner = null;

            return base.Destroy();
        }
    }

    public class HeroProtectionDoorRight : DestructableDoor
    {

        public HeroProtectionDoorRight(Commonwealth township, HeroProtectionDoorLeft partner)
            : base(township, partner, 0x06BB, 0x0C2D, 0x0C2D)
        {
        }

        public HeroProtectionDoorRight(Serial serial) : base(serial) { }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override bool Destroy()
        {
            //Had to throw in a fix for this as it gets called once for each door, starting two timers, we probably need a better solution to this
            if (mCommonwealth != null && mCommonwealth.Hero != null)
            {
                mCommonwealth.Hero.OnDropProtection();
            }
            if (mPartner != null && !mPartner.Dying)
            {
                mDying = true;
                mPartner.Destroy();
            }

            mPartner = null;

            return base.Destroy();
        }
    }

    public class HeroCellDoor : DestructableDoor
    {
        private TownHero mHero;
        public TownHero Hero { get { return mHero; } set { mHero = value; } }
        public HeroCellDoor(Commonwealth township)
            : base(township, null, 0x1FEE, 0x0C2D, 0x0C2D)
        {
        }

        public HeroCellDoor(Serial serial) : base(serial) { }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            mHero = (TownHero)reader.ReadMobile();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write((TownHero)mHero);
        }

        public override bool Destroy()
        {
            if (mHero != null)
            {
                mHero.OnDropProtection();
            }
            return base.Destroy();
        }
    }
}
