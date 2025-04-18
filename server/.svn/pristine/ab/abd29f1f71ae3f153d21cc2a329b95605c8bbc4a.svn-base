using System;

namespace Server.Items
{
    public class GuttedFish : Item
	{
		[Constructable]
        public GuttedFish()
            : base(Utility.RandomList(0x1E15, 0x1E16, 0x1E17, 0x1E18))
		{
			Movable = true;
			Stackable = false;
		}

        public GuttedFish(Serial serial)
            : base(serial)
		{            
		}

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "a gutted fish");
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
