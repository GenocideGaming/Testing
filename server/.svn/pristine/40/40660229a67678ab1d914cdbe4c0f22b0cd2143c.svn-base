using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0xF62, 0xF63 )]
	public class Spear : BaseSpear
	{
		public override int OldStrengthReq{ get{ return 30; } }

		public override double OldSpeed{ get{ return 3.75; } }

        public override int OldBaseDamage { get { return 18; } }
        public override int OldDamageVariation { get { return 8; } }
        public override int OldArmorDrain { get { return 0; } }
        public override int OldStaminaDrain { get { return 6; } }

		public override int InitMinHits{ get{ return 31; } }
		public override int InitMaxHits{ get{ return 80; } }

		[Constructable]
		public Spear() : base( 0xF62 )
		{
			Weight = 7.0;
		}

		public Spear( Serial serial ) : base( serial )
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