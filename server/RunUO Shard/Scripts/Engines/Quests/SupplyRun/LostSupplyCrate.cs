using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Items
{
    public class LostSupplyCrate : Item
    {
        private bool m_HasTriggeredAmbush; // Tracks if ambush has occurred

        [Constructable]
        public LostSupplyCrate() : base(0xE3F) // Crate graphic
        {
            Name = "lost supply crate";
            Hue = 1153;
            Weight = 10.0;
            Stackable = true; // Note: Stackable is set to true as per your version
            m_HasTriggeredAmbush = false;
        }

        public override bool OnDragLift(Mobile from)
        {
            if (from == null || !from.Alive)
                return false;

            PlayerMobile pm = from as PlayerMobile;
            if (pm == null)
                return false;

            if (!(pm.Quest is SupplyRecoveryQuest))
            {
                from.SendMessage("You must have the supply recovery quest to take this crate.");
                return false;
            }

            // Trigger ambush only if in world (Parent == null) and ambush hasn't occurred
            if (Parent == null && !m_HasTriggeredAmbush)
            {
                SpawnZombieAmbush(from);
                from.SendMessage("As you lift the crate, zombies emerge from the shadows!");
                m_HasTriggeredAmbush = true;
            }

            return true;
        }

        public override bool DropToMobile(Mobile from, Mobile target, Point3D p)
        {
            if (from == null || target == null || !from.Alive || from != target)
                return false;

            PlayerMobile pm = from as PlayerMobile;
            if (pm == null || !(pm.Quest is SupplyRecoveryQuest))
            {
                from.SendMessage("You must have the supply recovery quest to take this crate.");
                return false;
            }

            // Place in backpack
            if (pm.Backpack != null)
            {
                pm.Backpack.DropItem(this);
                return true;
            }

            from.SendMessage("You need a backpack to hold this crate.");
            return false;
        }

        private void SpawnZombieAmbush(Mobile from)
        {
            int zombieCount = Utility.RandomMinMax(2, 4);
            for (int i = 0; i < zombieCount; i++)
            {
                Mobile zombie = Utility.RandomBool() ? new Zombiex() : (Mobile)Activator.CreateInstance(Type.GetType("Server.Mobiles.LesserZombie") ?? typeof(Zombiex));
                Point3D loc = new Point3D(from.Location.X + Utility.RandomMinMax(-5, 5), from.Location.Y + Utility.RandomMinMax(-5, 5), from.Location.Z);
                zombie.MoveToWorld(loc, from.Map);
            }
        }

        public LostSupplyCrate(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1); // version
            writer.Write(m_HasTriggeredAmbush);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            if (version >= 1)
                m_HasTriggeredAmbush = reader.ReadBool();
            else
                m_HasTriggeredAmbush = false; // Default for version 0
        }
    }
}