using System;
using System.Collections.Generic;
using System.Text;
using Server.Mobiles;
using Server.Factions;
using Server.Scripts.Custom.Citizenship;

namespace Server.Items
{
    public class FlagLever : Item
    {
        private int mActivatingFlagValue; //the value of the flag that died to create us
        private WorldWarsFlag mSpawningFlag; //the flag we are spawning
        private WorldWarsController mController;
        private bool mActive;

        public bool Active { get { return mActive; } set { mActive = value; if (mActive) ItemID = 0x1093; } }
        public int ActivatingFlagValue { get { return mActivatingFlagValue; } set { mActivatingFlagValue = value; } }
        public WorldWarsFlag SpawningFlag { get { return mSpawningFlag; } set { mSpawningFlag = value; } }

        public FlagLever(WorldWarsController controller, WorldWarsFlag activatingFlag)
            : base(0x1093)
        {
            Movable = false;
            mController = controller;
            mActivatingFlagValue = activatingFlag.Value;
            mActive = true;
        }
        public FlagLever(Serial serial) : base(serial) { }

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
        public override void OnDoubleClick(Mobile from)
        {
            if (!mActive)
            {
                from.SendMessage("The flag is already being raised!");
                return;
            }
            if (Commonwealth.Find(from) != null)
            {
                ItemID = 0x1095;
                from.SendMessage("You begin to raise your militia's flag!");
                mController.SpawnNewFlag(this, from);
                mActive = false;
            }
            else
            {
                from.SendMessage("You must be a militia member to raise a flag!");
            }
        }
    }
}
