using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x143B, 0x143A )]
	public class Maul : BaseBashing
	{
        public override int OldStrengthReq { get { return 35; } }

        public override double OldSpeed { get { return 3.33; } }

        public override int OldBaseDamage { get { return 14; } }
        public override int OldDamageVariation { get { return 3; } }
        public override int OldArmorDrain { get { return 2; } }
        public override int OldStaminaDrain { get { return 14; } }

		public override int InitMinHits{ get{ return 31; } }
		public override int InitMaxHits{ get{ return 80; } }

		[Constructable]
		public Maul() : base( 0x143B )
		{
			Weight = 10.0;
		}

		public Maul( Serial serial ) : base( serial )
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

			if ( Weight == 14.0 )
				Weight = 10.0;
		}
	}
}