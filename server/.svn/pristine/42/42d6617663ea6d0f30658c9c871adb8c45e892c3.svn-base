using System;

namespace Server.Items
{
    public class BoxFlowers : Item
	{
		[Constructable]
        public BoxFlowers()
            : base(Utility.RandomList(0x0C83, 0x0C84, 0x0C85, 0x0C86, 0x0C87, 0x0C88, 0x0C89, 0x0C8A, 0x0C8B, 0x0C8C, 0x0C8D, 0x0C8E, 0x0CBE, 0x0CBF, 0x0CC0, 0x0CC1))
		{
			Movable = true;
			Stackable = false;
		}

        public BoxFlowers(Serial serial)
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
