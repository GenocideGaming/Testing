using System;

namespace Server.Items
{
    public class SwordDisplay : Item
	{
		[Constructable]
        public SwordDisplay()
            : base(Utility.RandomList(0x2842, 0x2843, 0x2844, 0x2845))
		{
			Movable = true;
			Stackable = false;
		}

        public SwordDisplay(Serial serial)
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
