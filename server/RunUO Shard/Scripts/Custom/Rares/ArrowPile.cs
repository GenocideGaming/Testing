using System;

namespace Server.Items
{
    public class ArrowPile : Item
	{
		[Constructable]
        public ArrowPile()
            : base(Utility.RandomList(0x0F40, 0x0F41, 0x1BFC, 0x1BFD))
		{
			Movable = true;
			Stackable = false;
		}

        public ArrowPile(Serial serial)
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
