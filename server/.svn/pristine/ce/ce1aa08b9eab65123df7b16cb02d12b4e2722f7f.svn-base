using System;

namespace Server.Items
{
    public class TarotCards : Item
	{
		[Constructable]
        public TarotCards()
            : base(Utility.RandomList(0x12A5, 0x12A6, 0x12A7, 0x12A8, 0x12A9, 0x12AA, 0x12AB, 0x12AC))
		{
			Movable = true;
			Stackable = false;
		}

        public TarotCards(Serial serial)
            : base(serial)
		{            
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
