using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x13B2, 0x13B1 )]
	public class Bow : BaseRanged
	{
		public override int EffectID{ get{ return 0xF42; } }
		public override Type AmmoType{ get{ return typeof( Arrow ); } }
		public override Item Ammo{ get{ return new Arrow(); } }

        public override int OldStrengthReq { get { return 18; } }

		public override double OldSpeed{ get{ return 3.0; } }

        public override int OldBaseDamage { get { return 18; } }
        public override int OldDamageVariation { get { return 6; } }
        public override int OldArmorDrain { get { return 0; } }
        public override int OldStaminaDrain { get { return 5; } }

		public override int DefMaxRange{ get{ return 10; } }

		public override int InitMinHits{ get{ return 31; } }
		public override int InitMaxHits{ get{ return 60; } }

		public override WeaponAnimation DefAnimation{ get{ return WeaponAnimation.ShootBow; } }

		[Constructable]
		public Bow() : base( 0x13B2 )
		{
			Weight = 6.0;
			Layer = Layer.TwoHanded;
		}

		public Bow( Serial serial ) : base( serial )
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

			if ( Weight == 7.0 )
				Weight = 6.0;
		}
	}
}