using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0xE87, 0xE88 )]
	public class Pitchfork : BaseSpear
	{
        public override int OldStrengthReq { get { return 15; } }

        public override double OldSpeed { get { return 3.00; } }

        public override int OldBaseDamage { get { return 14; } }
        public override int OldDamageVariation { get { return 7; } }
        public override int OldArmorDrain { get { return 0; } }
        public override int OldStaminaDrain { get { return 6; } }

		public override int InitMinHits{ get{ return 31; } }
		public override int InitMaxHits{ get{ return 60; } }

		[Constructable]
		public Pitchfork() : base( 0xE87 )
		{
			Weight = 11.0;
		}

		public Pitchfork( Serial serial ) : base( serial )
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

			if ( Weight == 10.0 )
				Weight = 11.0;
		}
	}
}