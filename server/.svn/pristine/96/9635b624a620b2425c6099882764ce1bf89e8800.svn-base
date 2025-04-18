using System;

namespace Server.Items
{
    public class AnimalHeadTrophy : Item
	{
		[Constructable]
        public AnimalHeadTrophy()
            : base(Utility.RandomList(0x1E60, 0x1E61, 0x1E62, 0x1E63, 0x1E64, 0x1E65, 0x1E66, 0x1E67, 0x1E68, 0x1E69, 0x1E6A, 0x1E6B, 0x1E6C, 0x1E6D, 0x2234, 0x2235, 0x3158, 0x3159))
		{
			Movable = true;
			Stackable = false;
		}

        public AnimalHeadTrophy(Serial serial)
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
