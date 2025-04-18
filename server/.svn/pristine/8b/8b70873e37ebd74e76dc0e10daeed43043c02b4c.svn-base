using System;
using Server.Network;
using Server.Targeting;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0xF52, 0xF51 )]
	public class Dagger : BaseKnife
	{
        public override int OldStrengthReq { get { return 5; } }

        public override double OldSpeed { get { return 2.31; } }

        public override int OldBaseDamage { get { return 3; } }
        public override int OldDamageVariation { get { return 2; } }
        public override int OldArmorDrain { get { return 0; } }
        public override int OldStaminaDrain { get { return 0; } }

		public override int InitMinHits{ get{ return 31; } }
		public override int InitMaxHits{ get{ return 40; } }

		public override SkillName DefSkill{ get{ return SkillName.Fencing; } }
		public override WeaponType DefType{ get{ return WeaponType.Piercing; } }
		public override WeaponAnimation DefAnimation{ get{ return WeaponAnimation.Pierce1H; } }

		[Constructable]
		public Dagger() : base( 0xF52 )
		{
			Weight = 1.0;
		}

		public Dagger( Serial serial ) : base( serial )
		{
		}
		
		public override void OnHit( Mobile attacker, Mobile defender, double damageBonus )
		{
			base.OnHit( attacker, defender, damageBonus );
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