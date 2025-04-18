using System;

namespace Server.Items
{
    public class DisplayShelf : Item
	{
		[Constructable]
        public DisplayShelf()
            : base(Utility.RandomList(0x4769, 0x476A, 0x476B, 0x476C, 0x476D))
		{
			Movable = true;
			Stackable = false;
		}

        public DisplayShelf(Serial serial)
            : base(serial)
		{            
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
