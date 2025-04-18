using System;

namespace Server.Items
{
    public class BasketOfFruit : Item
	{
		[Constructable]
        public BasketOfFruit()
            : base(0x0993)
		{
			Movable = true;
			Stackable = false;
		}

        public BasketOfFruit(Serial serial)
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
