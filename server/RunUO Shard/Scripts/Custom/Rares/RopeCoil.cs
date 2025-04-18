using System;

namespace Server.Items
{
    public class RopeCoil : Item
	{
		[Constructable]
        public RopeCoil()
            : base(Utility.RandomList(0x14F8, 0x14FA))
		{
			Movable = true;
			Stackable = false;
		}

        public RopeCoil(Serial serial)
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
