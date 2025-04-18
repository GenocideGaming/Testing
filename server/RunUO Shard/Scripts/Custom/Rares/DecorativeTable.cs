using System;

namespace Server.Items
{
    public class DecorativeTable : Item
	{
		[Constructable]
        public DecorativeTable()
            : base(Utility.RandomList(0x118B, 0x118C, 0x118D, 0x118E, 0x118F, 0x1191))
		{
			Movable = true;
			Stackable = false;
		}

        public DecorativeTable(Serial serial)
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
