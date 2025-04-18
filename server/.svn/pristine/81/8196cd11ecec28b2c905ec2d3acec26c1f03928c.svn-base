using System;

namespace Server.Items
{
    public class Pegboard : Item
	{
		[Constructable]
        public Pegboard()
            : base(Utility.RandomList(0x0C39, 0x0C3A))
		{
			Movable = true;
			Stackable = false;
		}

        public Pegboard(Serial serial)
            : base(serial)
		{            
		}

        public override void OnSingleClick(Mobile from)
        {            
            LabelTo(from, "a pegboard");
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
