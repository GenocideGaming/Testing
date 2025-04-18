using System;

namespace Server.Items
{
    public class IngotPiles : Item
	{
		[Constructable]
        public IngotPiles()
            : base(Utility.RandomList(0x1BE4, 0x1BE5, 0x1BE7, 0x1BE8, 0x1BEA, 0x1BEB, 0x1BED, 0x1BEE, 0x1BF0, 0x1BF1, 0x1BF3, 0x1BF4, 0x1BF6, 0x1BF7, 0x1BF9, 0x1BFA))
		{
			Movable = true;
			Stackable = false;
		}

        public IngotPiles(Serial serial)
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
