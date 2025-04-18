using System;

namespace Server.Items
{
	public class DrowGland : Item
	{
		public override int LabelNumber{ get{ return 1063582; } } // drider gland

		[Constructable]
		public DrowGland() : this( 1 )
		{
		}

		[Constructable]
		public DrowGland( int amount ) : base( 0x0F79 )
		{
			Weight = 1.0;
			Stackable = true;
			Amount = amount;
			Hue = 1102;
		}

		public DrowGland( Serial serial ) : base( serial )
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