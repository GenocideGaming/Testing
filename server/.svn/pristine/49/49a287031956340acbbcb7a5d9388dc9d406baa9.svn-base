using System;

namespace Server.Items
{
    public class TallCandlabra : Item
	{
		[Constructable]
        public TallCandlabra()
            : base(0x0A29)
		{
			Movable = true;
			Stackable = false;
		}

        public TallCandlabra(Serial serial)
            : base(serial)
		{            
		}

        public override void OnSingleClick(Mobile from)
        {            
            LabelTo(from, "a tall candlabra");
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
