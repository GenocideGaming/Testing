using System;

namespace Server.Items
{
    public class Carcass : Item
	{
		[Constructable]
        public Carcass()
            : base(Utility.RandomList(0x1871, 0x1872, 0x1873, 0x1874, 0x1E88, 0x1E89, 0x1E8B, 0x1E8E, 0x1E8F, 0x1E90, 0x1E91, 0x1E92, 0x1E93))
		{
			Movable = true;
			Stackable = false;
		}

        public Carcass(Serial serial)
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
