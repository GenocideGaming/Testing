using System;

namespace Server.Items
{
    public class DisplayCarts : Item
	{
		[Constructable]
        public DisplayCarts()
            : base(Utility.RandomList(0x47D7, 0x47D8, 0x47D9, 0x47DA, 0x47DB, 0x47DC, 0x47DD, 0x47DE))
		{
			Movable = true;
			Stackable = false;
		}

        public DisplayCarts(Serial serial)
            : base(serial)
		{            
		}

        public override void OnSingleClick(Mobile from)
        {            
            LabelTo(from, "a display cart");
        }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}
