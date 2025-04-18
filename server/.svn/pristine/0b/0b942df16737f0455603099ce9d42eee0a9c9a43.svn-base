using System;
using Server.Items;
using Server.Network;

namespace Server.Items
{
	[FlipableAttribute( 0x1443, 0x1442 )]
	public class TwoHandedAxe : BaseAxe
	{
        public override int OldStrengthReq { get { return 35; } }

		public override double OldSpeed{ get{ return 4.29; } }

        public override int OldBaseDamage { get { return 20; } }
        public override int OldDamageVariation { get { return 6; } }
        public override int OldArmorDrain { get { return 0; } }
        public override int OldStaminaDrain { get { return 5; } }

		public override int InitMinHits{ get{ return 31; } }
		public override int InitMaxHits{ get{ return 90; } }

		[Constructable]
		public TwoHandedAxe() : base( 0x1443 )
		{
			Weight = 8.0;
		}

		public TwoHandedAxe( Serial serial ) : base( serial )
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