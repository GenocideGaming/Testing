using System;

namespace Server.Items
{
    public class DecorativeArmorStatue : Item
	{
		[Constructable]
        public DecorativeArmorStatue()
            : base(Utility.RandomList(0x1508, 0x151A, 0x3D86, 0x3DAA))
		{
			Movable = true;
			Stackable = false;
		}

        public DecorativeArmorStatue(Serial serial)
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
