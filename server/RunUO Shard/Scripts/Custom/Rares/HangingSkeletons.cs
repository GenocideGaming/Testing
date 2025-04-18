using System;

namespace Server.Items
{
    public class HangingSkeletons : Item
	{
		[Constructable]
        public HangingSkeletons()
            : base(Utility.RandomList(0x1A01, 0x1A02, 0x1A03, 0x1A04, 0x1A05, 0x1A06, 0x1A09, 0x1A0A, 0x1A0B, 0x1A0C, 0x1A0D, 0x1A0E, 0x1B1D, 0x1B1E, 0x1B7C, 0x1B7D, 0x1B7F, 0x1B80))
		{
			Movable = true;
			Stackable = false;
		}

        public HangingSkeletons(Serial serial)
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
