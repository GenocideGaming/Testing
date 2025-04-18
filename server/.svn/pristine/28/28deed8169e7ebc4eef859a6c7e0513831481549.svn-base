using System;

namespace Server.Items
{
    public class CactusPlants : Item
	{
		[Constructable]
        public CactusPlants()
            : base(Utility.RandomList(0x0CA9, 0x0D25, 0x0D26, 0x0D27, 0x0D28, 0x0D2A, 0x0D2C, 0x0D2E, 0x1E0F, 0x1E10, 0x1E12, 0x1E13, 0x1E14))
		{
			Movable = true;
			Stackable = false;
		}

        public CactusPlants(Serial serial)
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
