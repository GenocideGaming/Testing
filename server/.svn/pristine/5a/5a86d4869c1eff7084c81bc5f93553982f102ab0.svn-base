using System;
using System.Collections.Generic;
using System.Text;
using Server.Factions;
using Server.Scripts;
using Server.Scripts.Custom.Citizenship;

namespace Server.Items
{
    public class WorldWarsFlag : DamageableItemMilitiaRestricted
    {
        WorldWarsController mController;
        int mValue;
        private RaiseTimer mRaiseTimer;
        private FlagLever mSpawningLever; //the lever that spawned us

        public int Value { get { return mValue; } set { mValue = value; } }
        
        public WorldWarsFlag(WorldWarsController controller, int value, FlagLever spawningLever)
            : this(controller, null, value, spawningLever)
        {
        }

        public WorldWarsFlag(WorldWarsController controller, ICommonwealth commonwealth, int value, FlagLever spawningLever)
            : base(0x15C5, 0x0C30, 0x0C2D, commonwealth != null ? commonwealth.Militia : null)
        {
            Faction militia = commonwealth != null ? commonwealth.Militia : null;
            Name = "An attackable flag";
            Movable = false;
            Hue = militia != null ? militia.Definition.HuePrimary : 0;
            Level = ItemLevel.Easy;
            mController = controller;
            mValue = value;
            Hits = HitsMax / 2;
            mSpawningLever = spawningLever;
            mRaiseTimer = new RaiseTimer(this);
            mRaiseTimer.Start();
        }
        public WorldWarsFlag(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
        }
      
        public override void Damage(int amount, Mobile from, bool willKill)
        {
            if (willKill)
            {
                if (mRaiseTimer != null)
                {
                    if (mSpawningLever != null)
                    {
                        mSpawningLever.Active = true;
                    }
                    mRaiseTimer.Stop();
                    mRaiseTimer = null;
                }

                if (mSpawningLever == null)
                {
                    mController.ScorePointsForFlagCapture(from, this, true);
                }
                else
                {
                    mController.ScorePointsForFlagCapture(from, this, false);
                }
            }
            base.Damage(amount, from, willKill);
        }
        public void OnFlagRaised()
        {
            mRaiseTimer.Stop();
            mRaiseTimer = null;

            if (mSpawningLever != null)
            {
                mSpawningLever.Delete();
                mSpawningLever = null;
            }

            if (Militia != null)
                Militia.Broadcast("Your militia has raised a flag!");

            Hits = HitsMax;
        }
        private class RaiseTimer : Timer
        {
            private WorldWarsFlag mFlag;

            public RaiseTimer(WorldWarsFlag flag)
                : base(TimeSpan.FromSeconds(FeatureList.WorldWars.RaiseFlagTimeInSeconds), TimeSpan.FromSeconds(FeatureList.WorldWars.RaiseFlagTimeInSeconds))
            {
                mFlag = flag;
            }
            protected override void OnTick()
            {
                base.OnTick();
                mFlag.OnFlagRaised();
            }
        }
    }
}
