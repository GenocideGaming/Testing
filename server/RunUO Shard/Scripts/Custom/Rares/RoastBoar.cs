using System;

namespace Server.Items
{
    public class RoastBoar : Item
	{

		[Constructable]
        public RoastBoar()
            : base(Utility.RandomList(0x09BB, 0x09BC))
		{
			Movable = true;
			Stackable = false;
		}

        public RoastBoar(Serial serial)
            : base(serial)
		{            
		}

        public override void OnSingleClick(Mobile from)
        {            
            LabelTo(from, "a roast boar");
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
