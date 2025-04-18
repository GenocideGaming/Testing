using System;

namespace Server.Items
{
    public class Books : Item
	{
		[Constructable]
        public Books()
            : base(Utility.RandomList(0x0FBD, 0x0FBE, 0x0FF4))
		{
			Movable = true;
			Stackable = false;
		}

        public Books(Serial serial)
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
