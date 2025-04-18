using System;

namespace Server.Items
{   
    public class GemCase : Item
	{
		[Constructable]
        
        public GemCase()
            : base(Utility.RandomList(0x407C, 0x407D))
		{
			Movable = true;
			Stackable = false;
		}

        public GemCase(Serial serial)
            : base(serial)
		{            
		}

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "a gem display case");
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
