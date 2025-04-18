using System;

namespace Server.Items
{
    public class StretchedHide : Item
	{
		[Constructable]
        public StretchedHide()
            : base(Utility.RandomList(0x1069, 0x106A, 0x106B, 0x107A, 0x107B, 0x107C))
		{
			Movable = true;
			Stackable = false;
		}

        public StretchedHide(Serial serial)
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
