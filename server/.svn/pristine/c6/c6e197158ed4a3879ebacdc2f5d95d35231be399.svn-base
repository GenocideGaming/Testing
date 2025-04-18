using System;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.SkillHandlers
{
	public class ArmsLore
	{
		public static void Initialize()
		{
			SkillInfo.Table[(int)SkillName.ArmsLore].Callback = new SkillUseCallback( OnUse );
		}

		public static TimeSpan OnUse(Mobile m)
		{
			m.Target = new InternalTarget();

			m.SendLocalizedMessage( 500349 ); // What item do you wish to get information about?

			return TimeSpan.FromSeconds( 1.0 );
		}

		[PlayerVendorTarget]
		private class InternalTarget : Target
		{
			public InternalTarget() : base( 2, false, TargetFlags.None )
			{
				AllowNonlocal = true;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
                BaseWeapon weapon = targeted as BaseWeapon;
                BaseArmor armor = targeted as BaseArmor;

                string outputMessage = "";

                int HitPoints = 0;
                int MaxHitPoints = 0;
                
                //Pass Skill Check
                if( from.CheckTargetSkill(SkillName.ArmsLore, targeted, 0, 100) )
				{
                    //Weapon
                    if ( weapon != null )
                    {                
                        HitPoints = weapon.HitPoints;
                        MaxHitPoints = weapon.MaxHitPoints;

                        bool poisonable = (weapon.Type == WeaponType.Slashing || weapon.Type == WeaponType.Piercing);

                        outputMessage = "This weapon has " + HitPoints.ToString() + " out of " + MaxHitPoints.ToString() + " total hit points remaining"; 
                    
                        switch (weapon.DefSkill)
                        {
                            case SkillName.Swords:
                                if (poisonable)
                                {
                                    outputMessage += ", uses the Swordsmanship skill, and is poisonable.";
                                }

                                else
                                {
                                    outputMessage += " and uses the Swordsmanship skill.";
                                }
                            break;

                            case SkillName.Fencing:
                                if (poisonable)
                                {
                                    outputMessage += ", uses the Fencing skill, and is poisonable.";
                                }

                                else
                                {
                                    outputMessage += " and uses the Fencing skill.";
                                }
                            break;

                            case SkillName.Macing:
                                outputMessage += " and uses the Macefighting skill.";
                            break;

                            case SkillName.Archery:
                                outputMessage += " and uses the Archery skill.";
                            break;

                            case SkillName.Wrestling:
                                outputMessage += " and uses the Wrestling skill.";
                            break;
                        }                   

                        from.SendMessage(outputMessage);
                    }

                    //Armor
                    else if ( armor != null )
                    {
                        HitPoints = armor.HitPoints;
                        MaxHitPoints = armor.MaxHitPoints;

                        outputMessage = "This armor has " + HitPoints.ToString() + " out of " + MaxHitPoints.ToString() + " total hit points remaining "; 
                        outputMessage += "and provides a " + armor.ArmorRating.ToString() + " AR bonus.";

                        from.SendMessage(outputMessage);
                    }                    
                }

                //Fail Skill Check
                else
                {
                    from.SendLocalizedMessage( 500353 ); // You are not certain...
                }                              
			}
		}
	}
}