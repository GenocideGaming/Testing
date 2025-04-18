using System;

namespace Server.Items
{
    public class DecorativeWeapons : Item
	{
		[Constructable]
        public DecorativeWeapons()
            : base(Utility.RandomList(0x155C, 0x155D, 0x155E, 0x155F, 0x1560, 0x1561, 0x1562, 0x1563, 0x1568, 0x1569, 0x156A, 0x156B))
		{
			Movable = true;
			Stackable = false;
		}

        public DecorativeWeapons(Serial serial)
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
