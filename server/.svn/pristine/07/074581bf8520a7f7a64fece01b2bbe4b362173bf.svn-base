using System;

namespace Server.Items
{
	public class BoneDust : Item
	{
		public override int LabelNumber{ get{ return 1063584; } } // bone dust

		[Constructable]
		public BoneDust() : this( 1 )
		{
		}

		[Constructable]
		public BoneDust( int amount ) : base( 0xF8F )
		{
			Weight = 1.0;
			Stackable = true;
			Amount = amount;
			Hue = 1882;
		}

		public BoneDust( Serial serial ) : base( serial )
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