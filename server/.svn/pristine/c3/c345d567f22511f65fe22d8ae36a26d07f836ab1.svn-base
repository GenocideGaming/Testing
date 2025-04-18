using System;

namespace Server.Items
{
    public class Gravestone : Item
	{
		[Constructable]
        public Gravestone()
            : base(Utility.RandomList(0x0ED4, 0x0ED5, 0x0ED6, 0x0ED7, 0x0ED8, 0x0ED9, 0x0EDA, 0x0EDB, 0x0EDC, 0x0EDD, 0x0EDE, 0x1165, 0x1166, 0x1167, 0x1168, 0x1169, 0x116A, 0x116B, 0x116C, 0x116D, 0x116E, 0x116F, 0x1170, 0x1171, 0x1172, 0x1173, 0x1174, 0x1175, 0x1176, 0x1177, 0x1178, 0x1179, 0x117A, 0x117B, 0x117C, 0x117D, 0x117E, 0x117F, 0x1180, 0x1181, 0x1182, 0x1183, 0x1184))
		{
			Movable = true;
			Stackable = false;
		}

        public Gravestone(Serial serial)
            : base(serial)
		{            
		}

        public override void OnSingleClick(Mobile from)
        {            
            LabelTo(from, "a gravestone");
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
