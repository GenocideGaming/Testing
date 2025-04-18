using System;

namespace Server.Items
{
    public class CoiledWhip : Item
	{
		[Constructable]
        public CoiledWhip()
            : base(Utility.RandomList(0x166E, 0x166F))
		{
			Movable = true;
			Stackable = false;
		}

        public CoiledWhip(Serial serial)
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
