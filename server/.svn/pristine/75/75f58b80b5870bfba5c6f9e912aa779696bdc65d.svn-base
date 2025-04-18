using System;

namespace Server.Items
{
	public class CrystalBall : Item
	{

		[Constructable]
		public CrystalBall() : base( 0xE2E )
		{
			Movable = true;
			Stackable = false;
		}

		public CrystalBall( Serial serial ) : base( serial )
		{            
		}

        public override void OnSingleClick(Mobile from)
        {            
            LabelTo(from, "a crystal ball");
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
