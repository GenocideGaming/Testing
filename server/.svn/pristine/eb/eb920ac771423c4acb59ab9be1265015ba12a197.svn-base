using System;

namespace Server.Items
{
    public class StringOfVines : Item
	{
		[Constructable]
        public StringOfVines()
            : base(Utility.RandomList(0x0C5E, 0x0C5F, 0x0C60))
		{
			Movable = true;
			Stackable = false;
		}

        public StringOfVines(Serial serial)
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
