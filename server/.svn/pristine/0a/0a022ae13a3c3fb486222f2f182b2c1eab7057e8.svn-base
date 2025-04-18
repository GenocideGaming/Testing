using System;

namespace Server.Items
{
	public class Heart : Item
	{
		[Constructable]
        public Heart()
            : base(0x024B)
		{
			Movable = true;
			Stackable = false;
		}

		public Heart( Serial serial ) : base( serial )
		{            
		}

        public override void OnSingleClick(Mobile from)
        {            
            LabelTo(from, "a bloody heart");
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
