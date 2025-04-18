using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x1403, 0x1402 )]
	public class ShortSpear : BaseSpear
	{
        public override int OldStrengthReq { get { return 15; } }

        public override double OldSpeed { get { return 3.00; } }

        public override int OldBaseDamage { get { return 14; } }
        public override int OldDamageVariation { get { return 7; } }
        public override int OldArmorDrain { get { return 0; } }
        public override int OldStaminaDrain { get { return 5; } }

		public override int InitMinHits{ get{ return 31; } }
		public override int InitMaxHits{ get{ return 70; } }

		public override WeaponAnimation DefAnimation{ get{ return WeaponAnimation.Pierce1H; } }

		[Constructable]
		public ShortSpear() : base( 0x1403 )
		{
			Weight = 4.0;
		}

		public ShortSpear( Serial serial ) : base( serial )
		{
		}

        public override WeaponAnimation GetAnimation()
        {
            WeaponAnimation animation = WeaponAnimation.Slash1H;

            Mobile attacker = this.Parent as Mobile;

            if (attacker != null)
            {              
                switch (Utility.Random(6))
                {
                    case 0:
                        animation = WeaponAnimation.Slash1H;
                    break;

                    case 1:
                        animation = WeaponAnimation.Slash1H;
                    break;

                    case 2:
                        animation = WeaponAnimation.Slash2H;
                    break;

                    case 3:
                        animation = WeaponAnimation.Bash1H;
                    break;

                    case 4:
                        animation = WeaponAnimation.Bash2H;
                    break;

                    case 5:
                        animation = WeaponAnimation.Pierce2H;
                    break;
                }               
            }

            return animation;
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