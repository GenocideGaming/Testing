using System;

namespace Server.Items
{
    public class BottlesOfLiquor : Item
	{
		[Constructable]
        public BottlesOfLiquor()
            : base(Utility.RandomList(0x099C, 0x099D, 0x099E))
		{
			Movable = true;
			Stackable = false;
		}

        public BottlesOfLiquor(Serial serial)
            : base(serial)
		{            
		}

        public override void OnSingleClick(Mobile from)
        {            
            LabelTo(from, "bottles of liquor");
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
