using System;

namespace Server.Items
{
	public class OrcishHeart : Item
	{
		public override int LabelNumber{ get{ return 1063580; } } // orcish heart

		[Constructable]
		public OrcishHeart() : this( 1 )
		{
		}

		[Constructable]
		public OrcishHeart( int amount ) : base( 0x0F91 )
		{
			Weight = 1.0;
			Stackable = true;
			Amount = amount;
			Hue = 1451;
		}

		public OrcishHeart( Serial serial ) : base( serial )
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