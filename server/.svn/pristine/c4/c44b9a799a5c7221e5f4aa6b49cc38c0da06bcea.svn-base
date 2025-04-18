using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x1441, 0x1440 )]
	public class Cutlass : BaseSword
	{
        public override int OldStrengthReq { get { return 10; } }

        public override double OldSpeed { get { return 2.73; } }

        public override int OldBaseDamage { get { return 13; } }
        public override int OldDamageVariation { get { return 4; } }
        public override int OldArmorDrain { get { return 0; } }
        public override int OldStaminaDrain { get { return 3; } }

		public override int DefHitSound{ get{ return 0x23B; } }
		public override int DefMissSound{ get{ return 0x23A; } }

		public override int InitMinHits{ get{ return 31; } }
		public override int InitMaxHits{ get{ return 70; } }

		[Constructable]
		public Cutlass() : base( 0x1441 )
		{
			Weight = 8.0;
		}

		public Cutlass( Serial serial ) : base( serial )
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