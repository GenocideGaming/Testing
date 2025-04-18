using System;

namespace Server.Items
{
    public class HangingArmor : Item
	{
		[Constructable]
        public HangingArmor()
            : base(Utility.RandomList(0x13BC, 0x13BD, 0x13C1, 0x13C2, 0x13C8, 0x13C9, 0x13CA, 0x13CF, 0x13D0, 0x13D1, 0x13D7, 0x13D8, 0x13D9, 0x13DE, 0x13DF, 0x13E0, 0x13E5, 0x13E6, 0x13E7, 0x13E8, 0x13E9, 0x13EA, 0x473C, 0x473D, 0x470E, 0x470F, 0x4710, 0x4711, 0x4712, 0x4713, 0x4714, 0x4715, 0x4716, 0x4717, 0x4718, 0x4721, 0x4722, 0x4723))
		{
			Movable = true;
			Stackable = false;
		}

        public HangingArmor(Serial serial)
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
