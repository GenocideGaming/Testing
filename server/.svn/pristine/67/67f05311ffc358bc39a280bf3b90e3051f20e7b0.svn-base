using System;
using Server.Items;
using Server.Network;
using Server.Engines.Harvest;

namespace Server.Items
{
	[FlipableAttribute( 0xE86, 0xE85 )]
	public class Pickaxe : BaseAxe, IUsesRemaining
	{
		public override HarvestSystem HarvestSystem{ get{ return Mining.System; } }		

        public override int OldStrengthReq { get { return 25; } }

		public override double OldSpeed{ get{ return 3.33; } }

        public override int OldBaseDamage { get { return 13; } }
        public override int OldDamageVariation { get { return 4; } }
        public override int OldArmorDrain { get { return 0; } }
        public override int OldStaminaDrain { get { return 5; } }

		public override int InitMinHits{ get{ return 31; } }
		public override int InitMaxHits{ get{ return 60; } }

		public override WeaponAnimation DefAnimation{ get{ return WeaponAnimation.Slash1H; } }

		[Constructable]
		public Pickaxe() : base( 0xE86 )
		{
			Weight = 11.0;
			UsesRemaining = 50;
			ShowUsesRemaining = true;
		}

		public Pickaxe( Serial serial ) : base( serial )
		{
		}

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Durability : {0}", this.UsesRemaining);
            LabelTo(from, "pick axe");                        
        }

        public override WeaponAnimation GetAnimation()
        {
            WeaponAnimation animation = WeaponAnimation.Slash1H;

            Mobile attacker = this.Parent as Mobile;

            if (attacker != null)
            {
                switch (Utility.Random(4))
                {
                    case 0:
                        animation = WeaponAnimation.Bash1H;
                    break;

                    case 1:
                        animation = WeaponAnimation.Bash2H;
                    break;

                    case 2:
                        animation = WeaponAnimation.Slash1H;
                    break;

                    case 3:
                        animation = WeaponAnimation.Slash2H;
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
			ShowUsesRemaining = true;
		}
	}
}