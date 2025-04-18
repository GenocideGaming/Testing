using System;

namespace Server.Items
{
    public class Gamepieces : Item
	{
		[Constructable]
        public Gamepieces()
            : base(Utility.RandomList(0x0E12, 0x0E13, 0x0E14, 0x0E15, 0x0E16, 0x0E17, 0x0E18, 0x0E19, 0x0E1A, 0x0E1B, 0x0FA2, 0x0FA3, 0x0FA4, 0x0FA5, 0x0FA8 ))
		{
			Movable = true;
			Stackable = false;
		}

        public Gamepieces(Serial serial)
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
