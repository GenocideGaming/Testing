using System;

namespace Server.Items
{
    public class Cauldron : Item
	{
		[Constructable]
        public Cauldron()
            : base(Utility.RandomList(0x0974, 0x0975))
		{
			Movable = true;
			Stackable = false;
		}

        public Cauldron(Serial serial)
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
