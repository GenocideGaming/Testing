using System;

namespace Server.Items
{
    public class WoodPiles : Item
	{
		[Constructable]
        public WoodPiles()
            : base(Utility.RandomList(0x1BD5, 0x1BD6, 0x1BD8, 0x1BD9, 0x1BDB, 0x1BDC, 0x1BDE, 0x1BDF, 0x1BE1, 0x1BE2))
		{
			Movable = true;
			Stackable = false;
		}

        public WoodPiles(Serial serial)
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
