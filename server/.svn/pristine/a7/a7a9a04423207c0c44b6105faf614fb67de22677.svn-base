using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public abstract class BaseBashing : BaseMeleeWeapon
	{
		public override int DefHitSound{ get{ return 0x233; } }
		public override int DefMissSound{ get{ return 0x239; } }

		public override SkillName DefSkill{ get{ return SkillName.Macing; } }
		public override WeaponType DefType{ get{ return WeaponType.Bashing; } }
		public override WeaponAnimation DefAnimation{ get{ return WeaponAnimation.Bash1H; } }

		public BaseBashing( int itemID ) : base( itemID )
		{
		}

		public BaseBashing( Serial serial ) : base( serial )
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
                    switch (Utility.Random(7))
                    {
                        case 0:
                            animation = WeaponAnimation.Bash1H;
                            break;

                        case 1:
                            animation = WeaponAnimation.Bash1H;
                            break;

                        case 2:
                            animation = WeaponAnimation.Slash1H;
                            break;

                        case 3:
                            animation = WeaponAnimation.Slash2H;
                            break;

                        case 4:
                            animation = WeaponAnimation.Pierce1H;
                            break;

                        case 5:
                            animation = WeaponAnimation.Pierce2H;
                            break;

                        case 6:
                            animation = WeaponAnimation.Bash2H;
                            break;
                    }
                }
                
                else if (attacker.FindItemOnLayer(Layer.TwoHanded) != null)
                {
                    switch (Utility.Random(5))
                    {
                        case 0:
                            animation = WeaponAnimation.Bash2H;
                        break;

                        case 1:
                            animation = WeaponAnimation.Bash2H;
                        break;
                            
                        case 2:
                            animation = WeaponAnimation.Slash2H;
                        break;

                        case 3:
                            animation = WeaponAnimation.Bash2H;
                        break;

                        case 4:
                            animation = WeaponAnimation.Slash2H;
                        break;
                    }
                }

                else
                {
                    switch (Utility.Random(6))
                    {
                        case 0:
                            animation = WeaponAnimation.Bash1H;
                        break;

                        case 1:
                            animation = WeaponAnimation.Bash1H;
                        break;

                        case 2:
                            animation = WeaponAnimation.Slash1H;
                        break;

                        case 3:
                            animation = WeaponAnimation.Slash2H;
                        break;

                        case 4:
                            animation = WeaponAnimation.Pierce1H;
                        break;

                        case 5:
                            animation = WeaponAnimation.Bash2H;
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

			defender.Stam -= Utility.Random( 3, 3 ); // 3-5 points of stamina loss
		}

		public override double GetBaseDamage( Mobile attacker )
		{
			double damage = base.GetBaseDamage( attacker );

			return damage;
		}
	}
}