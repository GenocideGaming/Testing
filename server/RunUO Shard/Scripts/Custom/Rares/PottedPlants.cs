using System;

namespace Server.Items
{
    public class PottedPlants : Item
	{
		[Constructable]
        public PottedPlants()
            : base(Utility.RandomList(0x11C6, 0x11C7, 0x11C8, 0x11C9, 0x11CA, 0x11CB, 0x11CC, 0x47D6, 0x4791, 0x4790, 0x477C, 0x477B, 0x477A, 0x4779, 0x4778, 0x42B9, 0x42BA))
		{
			Movable = true;
			Stackable = false;
		}

        public PottedPlants(Serial serial)
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
