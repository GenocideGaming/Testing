using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x1407, 0x1406 )]
	public class WarMace : BaseBashing
	{
        public override int OldStrengthReq { get { return 35; } }

        public override double OldSpeed { get { return 4.75; } }

        public override int OldBaseDamage { get { return 16; } }
        public override int OldDamageVariation { get { return 3; } }
        public override int OldArmorDrain { get { return 4; } }
        public override int OldStaminaDrain { get { return 20; } }

		public override int InitMinHits{ get{ return 31; } }
		public override int InitMaxHits{ get{ return 110; } }

		[Constructable]
		public WarMace() : base( 0x1407 )
		{
			Weight = 17.0;
		}

		public WarMace( Serial serial ) : base( serial )
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