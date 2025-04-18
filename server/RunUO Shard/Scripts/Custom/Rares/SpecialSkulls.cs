using System;

namespace Server.Items
{
    public class SpecialSkulls : Item
	{
		[Constructable]
        public SpecialSkulls()
            : base(Utility.RandomList(0x21FC, 0x2204, 0x42B5, 0x42BD))
		{
			Movable = true;
			Stackable = false;
		}

        public SpecialSkulls(Serial serial)
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
