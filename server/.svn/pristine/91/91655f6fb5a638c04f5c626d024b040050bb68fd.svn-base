using System;

namespace Server.Items
{
    public class TropicalFruit : Item
	{
		[Constructable]
        public TropicalFruit()
            : base(Utility.RandomList(0x1721, 0x1722, 0x1725, 0x1729, 0x172B))
		{
			Movable = true;
			Stackable = false;
		}

        public TropicalFruit(Serial serial)
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
