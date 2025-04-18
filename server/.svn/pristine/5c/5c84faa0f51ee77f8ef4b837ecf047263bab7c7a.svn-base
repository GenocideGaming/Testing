using System;

namespace Server.Items
{
    public class DriedFlowers : Item
	{
		[Constructable]
        public DriedFlowers()
            : base(Utility.RandomList(0x0C3B, 0x0C3C, 0x0C3D, 0x0C3E, 0x0C3F, 0x0C40, 0x0C41, 0x0C42))
		{
			Movable = true;
			Stackable = false;
		}

        public DriedFlowers(Serial serial)
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
