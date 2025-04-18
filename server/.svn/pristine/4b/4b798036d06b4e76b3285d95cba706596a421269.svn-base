using System;

namespace Server.Items
{
    public class GruesomeStandard : Item
	{
		[Constructable]
        public GruesomeStandard()
            : base(Utility.RandomList(0x041F, 0x0420, 0x0428, 0x0429))
		{
			Movable = true;
			Stackable = false;
		}

        public GruesomeStandard(Serial serial)
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
