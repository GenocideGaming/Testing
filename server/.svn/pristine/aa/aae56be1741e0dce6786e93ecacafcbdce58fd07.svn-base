using System;

namespace Server.Items
{
    public class SilverwareSet : Item
	{
		[Constructable]
        public SilverwareSet()
            : base(Utility.RandomList(0x09BD, 0x09BE, 0x09D4, 0x09D5, 0x09D7, 0x09DA))
		{
			Movable = true;
			Stackable = false;
		}

        public SilverwareSet(Serial serial)
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
