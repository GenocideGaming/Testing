using System;

namespace Server.Items
{
    public class GrandfatherClock : Item
	{
		[Constructable]
        public GrandfatherClock()
            : base(Utility.RandomList(0x4767, 0x4768))
		{
			Movable = true;
			Stackable = false;
		}

        public GrandfatherClock(Serial serial)
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
