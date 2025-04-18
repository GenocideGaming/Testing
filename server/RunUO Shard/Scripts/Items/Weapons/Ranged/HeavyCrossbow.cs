using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x13FD, 0x13FC )]
	public class HeavyCrossbow : BaseRanged
	{
		public override int EffectID{ get{ return 0x1BFE; } }
		public override Type AmmoType{ get{ return typeof( Bolt ); } }
		public override Item Ammo{ get{ return new Bolt(); } }
		
		public override int OldStrengthReq { get { return 40; } }

		public override double OldSpeed{ get{ return 6.0; } }

        public override int OldBaseDamage { get { return 32; } }
        public override int OldDamageVariation { get { return 9; } }
        public override int OldArmorDrain { get { return 0; } }
        public override int OldStaminaDrain { get { return 10; } }

		public override int DefMaxRange{ get{ return 6; } }

		public override int InitMinHits{ get{ return 31; } }
		public override int InitMaxHits{ get{ return 100; } }

		[Constructable]
		public HeavyCrossbow() : base( 0x13FD )
		{
			Weight = 9.0;
			Layer = Layer.TwoHanded;
		}

		public HeavyCrossbow( Serial serial ) : base( serial )
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