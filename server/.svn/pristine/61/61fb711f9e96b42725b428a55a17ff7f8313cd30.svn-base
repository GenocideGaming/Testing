using System;

namespace Server.Items
{
    public class Harrow : Item
	{
		[Constructable]
        public Harrow()
            : base(Utility.RandomList(0x1505, 0x1507))
		{
			Movable = true;
			Stackable = false;
		}

        public Harrow(Serial serial)
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
