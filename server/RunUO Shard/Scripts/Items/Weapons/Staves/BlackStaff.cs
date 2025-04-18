using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0xDF1, 0xDF0 )]
	public class BlackStaff : BaseStaff
	{
        public override int OldStrengthReq { get { return 30; } }

        public override double OldSpeed { get { return 3.00; } }

        public override int OldBaseDamage { get { return 14; } }
        public override int OldDamageVariation { get { return 5; } }
        public override int OldArmorDrain { get { return 2; } }
        public override int OldStaminaDrain { get { return 12; } }

		public override int InitMinHits{ get{ return 31; } }
		public override int InitMaxHits{ get{ return 70; } }

		[Constructable]
		public BlackStaff() : base( 0xDF0 )
		{
			Weight = 6.0;
		}

		public BlackStaff( Serial serial ) : base( serial )
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