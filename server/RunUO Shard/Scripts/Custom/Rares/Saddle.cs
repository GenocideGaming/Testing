using System;

namespace Server.Items
{
    public class Saddle : Item
	{
		[Constructable]
        public Saddle()
            : base(Utility.RandomList(0x0F37, 0x0F38, 0x1374, 0x1375))
		{
			Movable = true;
			Stackable = false;
		}

        public Saddle(Serial serial)
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
