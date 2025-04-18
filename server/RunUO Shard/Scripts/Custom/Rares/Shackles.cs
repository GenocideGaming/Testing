using System;

namespace Server.Items
{
    public class Shackles : Item
	{
		[Constructable]
        public Shackles()
            : base(Utility.RandomList(0x1262, 0x1263, 0x1A07, 0x1A08))
		{
			Movable = true;
			Stackable = false;
		}

        public Shackles(Serial serial)
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
