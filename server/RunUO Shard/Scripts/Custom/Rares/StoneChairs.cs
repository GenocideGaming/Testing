using System;

namespace Server.Items
{
    public class StoneChairs : Item
	{
		[Constructable]
        public StoneChairs()
            : base(Utility.RandomList(0x1218, 0x1219, 0x121A, 0x121B))
		{
			Movable = true;
			Stackable = false;
		}

        public StoneChairs(Serial serial)
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
