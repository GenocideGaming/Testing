using System;

namespace Server.Items
{
    public class WickerBasket : Item
	{
		[Constructable]
        public WickerBasket()
            : base(Utility.RandomList(0x0990, 0x09AC, 0x09B1))
		{
			Movable = true;
			Stackable = false;
		}

        public WickerBasket(Serial serial)
            : base(serial)
		{            
		}

        public override void OnSingleClick(Mobile from)
        {            
            LabelTo(from, "a wicker basket");
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
