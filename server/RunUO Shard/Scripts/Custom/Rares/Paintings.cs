using System;

namespace Server.Items
{
    public class Paintings : Item
	{
		[Constructable]
        public Paintings()
            : base(Utility.RandomList(0x0E9F, 0x0EA0, 0x0EA1, 0x0EA2, 0x0EA3, 0x0EA4, 0x0EA5, 0x0EA6, 0x0EA7, 0x0EA8, 0x0EC8, 0x0EC9, 0x0EE7))
		{
			Movable = true;
			Stackable = false;
		}

        public Paintings(Serial serial)
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
