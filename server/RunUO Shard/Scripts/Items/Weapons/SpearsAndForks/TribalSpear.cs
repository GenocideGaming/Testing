using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0xF62, 0xF63 )]
	public class TribalSpear : BaseSpear
	{
		public override int OldStrengthReq { get { return 30; } }

        public override double OldSpeed { get { return 3.75; } }

        public override int OldBaseDamage { get { return 15; } }
        public override int OldDamageVariation { get { return 7; } }
        public override int OldArmorDrain { get { return 0; } }
        public override int OldStaminaDrain { get { return 6; } }

        public override int InitMinHits { get { return 25; } }
        public override int InitMaxHits { get { return 50; } }

		public override string DefaultName
		{
			get { return "a tribal spear"; }
		}

		[Constructable]
		public TribalSpear() : base( 0xF62 )
		{
			Weight = 7.0;
			Hue = 837;
		}

		public TribalSpear( Serial serial ) : base( serial )
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