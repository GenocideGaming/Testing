using System;

namespace Server.Items
{
    public class SheetMusic : Item
	{
		[Constructable]
        public SheetMusic()
            : base(Utility.RandomList(0x0EBD, 0x0EBE, 0x0EBF, 0x0EC0))
		{
			Movable = true;
			Stackable = false;
		}

        public SheetMusic(Serial serial)
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
