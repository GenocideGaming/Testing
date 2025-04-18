using System;
using Server.Items;
using Server.Network;

namespace Server.Items
{
	[FlipableAttribute( 0xF43, 0xF44 )]
	public class Hatchet : BaseAxe
	{
        public override int OldStrengthReq { get { return 35; } }

        public override double OldSpeed { get { return 4.29; } }

        public override int OldBaseDamage { get { return 10; } }
        public override int OldDamageVariation { get { return 5; } }
        public override int OldArmorDrain { get { return 0; } }
        public override int OldStaminaDrain { get { return 10; } }

		public override int InitMinHits{ get{ return 31; } }
		public override int InitMaxHits{ get{ return 80; } }

		[Constructable]
		public Hatchet() : base( 0xF43 )
		{
			Weight = 4.0;
		}

		public Hatchet( Serial serial ) : base( serial )
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