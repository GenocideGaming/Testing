using System;

namespace Server.Items
{
    public class KnittingTools : Item
	{
		[Constructable]
        public KnittingTools()
            : base(0x0DF6)
		{
			Movable = true;
			Stackable = false;
		}

        public KnittingTools(Serial serial)
            : base(serial)
		{            
		}

        public override void OnSingleClick(Mobile from)
        {            
            LabelTo(from, "an arrow quiver");
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
