using System;

namespace Server.Items
{
    public class EvilStatues : Item
	{
		[Constructable]
        public EvilStatues()
            : base(Utility.RandomList(0x42C5, 0x42C2, 0x42C3, 0x40BC))
		{
			Movable = true;
			Stackable = false;
		}

        public EvilStatues(Serial serial)
            : base(serial)
		{            
		}

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "an evil statue");
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
