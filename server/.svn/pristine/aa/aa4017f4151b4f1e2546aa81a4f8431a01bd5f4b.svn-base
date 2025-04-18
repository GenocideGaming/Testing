using System;

namespace Server.Items
{
    public class BookPiles : Item
	{
		[Constructable]
        public BookPiles()
            : base(Utility.RandomList(0x0C16, 0x1E21,0x1E22 ,0x1E23, 0x1E24 ,0x1E25))
		{           
			Movable = true;
			Stackable = false;
		}

        public BookPiles(Serial serial)
            : base(serial)
		{            
		}

        public override void OnSingleClick(Mobile from)
        {            
            LabelTo(from, "a pile of ruined books");
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
