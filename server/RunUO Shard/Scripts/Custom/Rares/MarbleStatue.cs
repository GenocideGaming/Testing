using System;

namespace Server.Items
{
    public class MarbleStatue : Item
	{
		[Constructable]
        public MarbleStatue()
            : base(Utility.RandomList(0x1224, 0x1225, 0x1226, 0x1227, 0x1228, 0x12CA, 0x12CB, 0x139A, 0x139B, 0x139C, 0x139D, 0x42BB, 0x42BC))
		{
			Movable = true;
			Stackable = false;
		}

        public MarbleStatue(Serial serial)
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
