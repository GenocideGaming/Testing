using System;

namespace Server.Items
{
	public class BloodCoin : Item
	{
		public override double DefaultWeight
		{
			get { return ( Core.ML ? ( 0.02 / 3 ) : 0.02 ); }
		}

		[Constructable]
		public BloodCoin() : this( 1 )
		{
		}

		[Constructable]
		public BloodCoin( int amountFrom, int amountTo ) : this( Utility.RandomMinMax( amountFrom, amountTo ) )
		{
		}

		[Constructable]
		public BloodCoin( int amount ) : base( 0x0EEA )
		{
			Stackable = true;
			Amount = amount;
			Hue = 1194;
			Name = "blood coin";
		}

		public BloodCoin( Serial serial ) : base( serial )
		{
		}

		public override int GetDropSound()
		{
			if ( Amount <= 1 )
				return 0x2E4;
			else if ( Amount <= 5 )
				return 0x2E5;
			else
				return 0x2E6;
		}

		/* 
		protected override void OnAmountChange( int oldValue )
		{
			int newValue = this.Amount;

			UpdateTotal( this, TotalType.BloodCoin, newValue - oldValue );
		}

		public override int GetTotal( TotalType type )
		{
			int baseTotal = base.GetTotal( type );

			if ( type == TotalType.BloodCoin )
				baseTotal += this.Amount;

			return baseTotal;
		}
 		*/
		

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}