using System;

namespace Server.Items
{
    public class Horseshoes : Item
	{
		[Constructable]
        public Horseshoes()
            : base(Utility.RandomList(0x0FB6))
		{
			Movable = true;
			Stackable = false;
		}

        public Horseshoes(Serial serial)
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
