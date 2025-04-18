using System;

namespace Server.Items
{
    public class Mushrooms : Item
	{
		[Constructable]
        public Mushrooms()
            : base(Utility.RandomList(0x0D0C, 0x0D0D, 0x0D0E, 0x0D0F, 0x0D10, 0x0D11, 0x0D12, 0x0D13, 0x0D14, 0x0D15))
		{
			Movable = true;
			Stackable = false;
		}

        public Mushrooms(Serial serial)
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
