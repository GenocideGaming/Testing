using System;

namespace Server.Items
{
    public class BloodyHealing : Item
	{
		[Constructable]
        public BloodyHealing()
            : base(Utility.RandomList(0x0E20, 0x0E21, 0x0E22, 0x0E23))
		{
			Movable = true;
			Stackable = false;
		}

        public BloodyHealing(Serial serial)
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
