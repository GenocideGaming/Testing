using System;

namespace Server.Items
{
    public class FilledBookcase : Item
	{
		[Constructable]
        public FilledBookcase()
            : base(Utility.RandomList(0x0A97, 0x0A98, 0x0A99, 0x0A9A, 0x0A9B, 0x0A9C, 0x474D, 0x474E, 0x474F, 0x4750))
		{
			Movable = true;
			Stackable = false;
		}

        public FilledBookcase(Serial serial)
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
