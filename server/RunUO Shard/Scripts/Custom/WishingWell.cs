using System;
using System.Collections.Generic;
using System.Text;
using Server.Items;

namespace Server.Multis
{
    class WishingWell : BaseMulti
    {
        private int mSound;
        private static Type[] ItemTypes = new Type[] { };

        public int Sound { get { return mSound; } set { mSound = value; } }

        [Constructable]
        public WishingWell() : base(0x56) { }

        public WishingWell(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            dropped.Delete();
            Console.WriteLine("Someone dropped something  on me");
            return base.OnDragDrop(from, dropped);
        }
    }
}
