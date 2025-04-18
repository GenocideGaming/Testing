using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x1405, 0x1404 )]
	public class WarFork : BaseSpear
	{
		public override int OldStrengthReq{ get{ return 35; } }

		public override double OldSpeed{ get{ return 3.00; } }

        public override int OldBaseDamage { get { return 14; } }
        public override int OldDamageVariation { get { return 7; } }
        public override int OldArmorDrain { get { return 0; } }
        public override int OldStaminaDrain { get { return 4; } }

		public override int DefHitSound{ get{ return 0x236; } }
		public override int DefMissSound{ get{ return 0x238; } }

		public override int InitMinHits{ get{ return 31; } }
		public override int InitMaxHits{ get{ return 110; } }

		public override WeaponAnimation DefAnimation{ get{ return WeaponAnimation.Pierce1H; } }

		[Constructable]
		public WarFork() : base( 0x1405 )
		{
			Weight = 9.0;
		}

		public WarFork( Serial serial ) : base( serial )
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