using System;

namespace Server.Items
{
    public class PaintingRuined : Item
	{
		[Constructable]
        public PaintingRuined()
            : base(0x0C2C)
		{
			Movable = true;
			Stackable = false;
		}

        public PaintingRuined(Serial serial)
            : base(serial)
		{            
		}

        public override void OnSingleClick(Mobile from)
        {            
            LabelTo(from, "a ruined painting");
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
