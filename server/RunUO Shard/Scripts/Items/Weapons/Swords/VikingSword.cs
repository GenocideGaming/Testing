using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x13B9, 0x13Ba )]
	public class VikingSword : BaseSword
	{
        public override int OldStrengthReq { get { return 25; } }

        public override double OldSpeed { get { return 3.33; } }

        public override int OldBaseDamage { get { return 16; } }
        public override int OldDamageVariation { get { return 5; } }
        public override int OldArmorDrain { get { return 0; } }
        public override int OldStaminaDrain { get { return 5; } }

		public override int DefHitSound{ get{ return 0x237; } }
		public override int DefMissSound{ get{ return 0x23A; } }

		public override int InitMinHits{ get{ return 31; } }
		public override int InitMaxHits{ get{ return 100; } }

		[Constructable]
		public VikingSword() : base( 0x13B9 )
		{
			Weight = 6.0;
		}

		public VikingSword( Serial serial ) : base( serial )
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