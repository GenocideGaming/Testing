using System;

namespace Server.Items
{
    public class Wheat : Item
	{
		[Constructable]
        public Wheat()
            : base(Utility.RandomList(0x0C55, 0x0C56, 0x0C57, 0x0C58, 0x0C59, 0x0C5A, 0x0C5B, 0x1A92, 0x1A93, 0x1A94, 0x1A95, 0x1A96))
		{
			Movable = true;
			Stackable = false;
		}

        public Wheat(Serial serial)
            : base(serial)
		{            
		}

        public override void OnSingleClick(Mobile from)
        {            
            LabelTo(from, "wheat");
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
