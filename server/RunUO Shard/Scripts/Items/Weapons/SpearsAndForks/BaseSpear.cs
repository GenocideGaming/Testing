using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public abstract class BaseSpear : BaseMeleeWeapon
	{
		public override int DefHitSound{ get{ return 0x23C; } }
		public override int DefMissSound{ get{ return 0x238; } }

		public override SkillName DefSkill{ get{ return SkillName.Fencing; } }
		public override WeaponType DefType{ get{ return WeaponType.Piercing; } }
		public override WeaponAnimation DefAnimation{ get{ return WeaponAnimation.Pierce2H; } }

		public BaseSpear( int itemID ) : base( itemID )
		{
		}

		public BaseSpear( Serial serial ) : base( serial )
		{
		}

        public override WeaponAnimation GetAnimation()
        {
            WeaponAnimation animation = WeaponAnimation.Slash1H;

            Mobile attacker = this.Parent as Mobile;

            if (attacker != null)
            {
                if (attacker.FindItemOnLayer(Layer.TwoHanded) is BaseShield)
                {
                    switch (Utility.Random(8))
                    {
                        case 0:
                            animation = WeaponAnimation.Pierce1H;
                            break;

                        case 1:
                            animation = WeaponAnimation.Pierce1H;
                            break;

                        case 2:
                            animation = WeaponAnimation.Bash1H;
                            break;

                        case 3:
                            animation = WeaponAnimation.Slash1H;
                            break;

                        case 4:
                            animation = WeaponAnimation.Wrestle;
                       break;

                        case 5:
                            animation = WeaponAnimation.Bash1H;
                            break;

                        case 6:
                            animation = WeaponAnimation.Slash1H;
                        break;

                        case 7:
                            animation = WeaponAnimation.Pierce1H;
                        break;
                    }
                }

                else if (attacker.FindItemOnLayer(Layer.TwoHanded) != null)
                {
                    switch (Utility.Random(4))
                    {
                        case 0:
                            animation = WeaponAnimation.Pierce2H;
                        break;

                        case 1:
                            animation = WeaponAnimation.Pierce2H;
                        break;

                        case 2:
                            animation = WeaponAnimation.Pierce2H;
                        break;

                        case 3:
                            animation = WeaponAnimation.Pierce2H;
                        break;
                    }
                }

                else
                {
                    switch (Utility.Random(7))
                    {
                        case 0:
                            animation = WeaponAnimation.Pierce1H;
                        break;

                        case 1:
                            animation = WeaponAnimation.Pierce1H;
                        break;

                        case 2:
                            animation = WeaponAnimation.Bash1H;
                        break;

                        case 3:
                            animation = WeaponAnimation.Slash1H;
                        break;

                        case 4:
                            animation = WeaponAnimation.Wrestle;
                        break;

                        case 5:
                            animation = WeaponAnimation.Bash2H;
                        break;

                        case 6:
                            animation = WeaponAnimation.Slash2H;
                        break;
                    }
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

		public override void OnHit( Mobile attacker, Mobile defender, double damageBonus )
		{
			base.OnHit( attacker, defender, damageBonus );
		}
	}
}