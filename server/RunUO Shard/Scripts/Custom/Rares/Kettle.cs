using System;

namespace Server.Items
{
    public class Kettle : Item
	{
		[Constructable]
        public Kettle()
            : base(Utility.RandomList(0x09DC, 0x09E0, 0x09E8, 0x09F3, 0x09ED))
		{
			Movable = true;
			Stackable = false;
		}

        public Kettle(Serial serial)
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
