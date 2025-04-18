using System;

namespace Server.Items
{
    public class HorseDung : Item
	{
		[Constructable]
        public HorseDung()
            : base(Utility.RandomList(0x0F3B, 0x0F3C))
		{
			Movable = true;
			Stackable = false;
		}

        public HorseDung(Serial serial)
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
