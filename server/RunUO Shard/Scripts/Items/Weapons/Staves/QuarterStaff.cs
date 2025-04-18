using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0xE89, 0xE8a )]
	public class QuarterStaff : BaseStaff
	{
		public override int OldStrengthReq{ get{ return 30; } }
		
		public override double OldSpeed{ get{ return 3.00; } }

        public override int OldBaseDamage { get { return 14; } }
        public override int OldDamageVariation { get { return 5; } }
        public override int OldArmorDrain { get { return 2; } }
        public override int OldStaminaDrain { get { return 12; } }

		public override int InitMinHits{ get{ return 31; } }
		public override int InitMaxHits{ get{ return 60; } }

		[Constructable]
		public QuarterStaff() : base( 0xE89 )
		{
			Weight = 4.0;
		}

		public QuarterStaff( Serial serial ) : base( serial )
		{
		}

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