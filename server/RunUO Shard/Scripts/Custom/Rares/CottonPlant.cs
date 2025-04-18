using System;

namespace Server.Items
{
    public class CottonPlant : Item
	{
		[Constructable]
        public CottonPlant()
            : base(Utility.RandomList(0x0C4F, 0x0C50, 0x0C51, 0x0C52, 0x0C53, 0x0C54))
		{
			Movable = true;
			Stackable = false;
		}

        public CottonPlant(Serial serial)
            : base(serial)
		{            
		}

        public override void OnSingleClick(Mobile from)
        {            
            LabelTo(from, "a cotton plant");
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
