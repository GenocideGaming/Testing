using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x143E, 0x143F )]
	public class Halberd : BasePoleArm
	{
        public override int OldStrengthReq { get { return 45; } }

		public override double OldSpeed{ get{ return 6.0; } }

        public override int OldBaseDamage { get { return 28; } }
        public override int OldDamageVariation { get { return 7; } }
        public override int OldArmorDrain { get { return 1; } }
        public override int OldStaminaDrain { get { return 6; } }

		public override int InitMinHits{ get{ return 31; } }
		public override int InitMaxHits{ get{ return 80; } }

		[Constructable]
		public Halberd() : base( 0x143E )
		{
			Weight = 10.0;
		}

		public Halberd( Serial serial ) : base( serial )
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