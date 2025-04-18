using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0xEC4, 0xEC5 )]
	public class SkinningKnife : BaseKnife
	{
		public override int OldStrengthReq{ get{ return 5; } }
		
		public override double OldSpeed{ get{ return 2.31; } }

        public override int OldBaseDamage { get { return 3; } }
        public override int OldDamageVariation { get { return 2; } }
        public override int OldArmorDrain { get { return 0; } }
        public override int OldStaminaDrain { get { return 0; } }

		public override int InitMinHits{ get{ return 31; } }
		public override int InitMaxHits{ get{ return 40; } }

		[Constructable]
		public SkinningKnife() : base( 0xEC4 )
		{
			Weight = 1.0;
		}

		public SkinningKnife( Serial serial ) : base( serial )
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