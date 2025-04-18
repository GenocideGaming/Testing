using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x143D, 0x143C )]
	public class HammerPick : BaseBashing
	{
        public override int OldStrengthReq { get { return 35; } }

        public override double OldSpeed { get { return 3.33; } }

        public override int OldBaseDamage { get { return 13; } }
        public override int OldDamageVariation { get { return 4; } }
        public override int OldArmorDrain { get { return 2; } }
        public override int OldStaminaDrain { get { return 14; } }

        public override int InitMinHits { get { return 31; } }
        public override int InitMaxHits { get { return 80; } }

		[Constructable]
		public HammerPick() : base( 0x143D )
		{
			Weight = 9.0;
			Layer = Layer.OneHanded;
		}

		public HammerPick( Serial serial ) : base( serial )
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