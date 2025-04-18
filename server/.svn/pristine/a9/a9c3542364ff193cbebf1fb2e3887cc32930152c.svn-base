using System;

namespace Server.Items
{
    public class BottlesOfAle : Item
	{
		[Constructable]
        public BottlesOfAle()
            : base(Utility.RandomList(0x09A0, 0x09A1, 0x09A2))
		{
			Movable = true;
			Stackable = false;
		}

        public BottlesOfAle(Serial serial)
            : base(serial)
		{            
		}

        public override void OnSingleClick(Mobile from)
        {            
            LabelTo(from, "bottles of ale");
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
