using System;

namespace Server.Items
{
    public class Behive : Item
	{
		[Constructable]
        public Behive()
            : base(0x091A)
		{
			Movable = true;
			Stackable = false;
		}

        public Behive(Serial serial)
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
