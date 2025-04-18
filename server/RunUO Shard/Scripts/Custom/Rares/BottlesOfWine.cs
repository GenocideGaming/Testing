using System;

namespace Server.Items
{
    public class BottlesOfWine : Item
	{
		[Constructable]
        public BottlesOfWine()
            : base(Utility.RandomList(0x09C4, 0x09C5, 0x09C6))
		{
			Movable = true;
			Stackable = false;
		}

        public BottlesOfWine(Serial serial)
            : base(serial)
		{            
		}

        public override void OnSingleClick(Mobile from)
        {            
            LabelTo(from, "bottles of wine");
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
