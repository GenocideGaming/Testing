using System;

namespace Server.Items
{
    public class RuinedShelf : Item
	{
		[Constructable]
        public RuinedShelf()
            : base(Utility.RandomList(0x0C12, 0x0C13, 0x0C14, 0x0C15, 0x0C24, 0x0C25))
		{
			Movable = true;
			Stackable = false;
		}

        public RuinedShelf(Serial serial)
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
