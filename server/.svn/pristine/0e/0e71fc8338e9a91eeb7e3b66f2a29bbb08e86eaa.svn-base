using System;
using System.Collections;
using Server;
using Server.Network;
using Server.Spells;
using Server.Scripts;

namespace Server.Items
{
	public class BaseShield : BaseArmor
	{
		public override ArmorMaterialType MaterialType{ get{ return ArmorMaterialType.Plate; } }

		public BaseShield( int itemID ) : base( itemID )
		{
		}

		public BaseShield( Serial serial ) : base(serial)
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 );//version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if ( version < 1 )
			{
				if ( this is Aegis )
					return;

				// The 15 bonus points to resistances are not applied to shields on OSI.
				PhysicalBonus = 0;
				FireBonus = 0;
				ColdBonus = 0;
				PoisonBonus = 0;
				EnergyBonus = 0;
			}
		}
		
		public override int OldDexReq {
			get { return 0; }
		}
        [CommandProperty(AccessLevel.GameMaster)]
		public override double ArmorRating
		{
            get
            {
                double ar = base.ArmorRating;
                
                return ar;
            }
		}

		public override int OnHit( BaseWeapon weapon, int damage )
		{
            Mobile owner = this.Parent as Mobile;

            if (owner == null)
                return damage;

            double ar = 0.0;

            double arShield = FeatureList.ArmorChanges.ShieldARWithoutParry * ArmorRating;
            double arShieldSkill = FeatureList.ArmorChanges.ShieldARParryBonus * (owner.Skills.Parry.Value / 100) * ArmorRating;

            ar += arShield;
            ar += arShieldSkill;

            //Determine Chance to Melee Parry
            double chance = (owner.Skills[SkillName.Parry].Value / FeatureList.ParryChanges.ParryMeleeChanceDivisor) / 100.0;

            if (chance < 0.01)
                chance = 0.01;

            //Check For Successful Parry
            if (owner.CheckSkill(SkillName.Parry, chance))
            {
                double BasePercent = FeatureList.ArmorChanges.ParryBaseReductionPercent;
                double ARMultiplier = FeatureList.ArmorChanges.ParryReductionARMultiplier;

                //Determine Percent to Reduce            
                double reductionPercent = ((BasePercent * 100) + (ar * ARMultiplier)) / 100;
                                
                if (reductionPercent > 1.0)
                {
                    reductionPercent = 1.0;
                }

                //Determine Reduction
                int damageReduction = (int)(Math.Round((double)reductionPercent * (double)damage));

                //Reduce Damage
                damage -= damageReduction;

                owner.FixedEffect(0x37B9, 10, 16);                

                //Shield durability check                
                if (MaxHitPoints > 0 && FeatureList.ArmorChanges.DurablilityLossChance >= Utility.RandomDouble()) // Chance to lower durability
                {
                    //Reduce Hitpoints
                    if (HitPoints > 0)
                    {
                        --HitPoints;
                    }

                    //Warning of Possible Breakage
                    if (HitPoints <= FeatureList.ArmorChanges.HitPointsBreakWarning)
                    {
                        if (Parent is Mobile)
                            ((Mobile)Parent).LocalOverheadMessage(MessageType.Regular, 0x3B2, 1061121); // Your equipment is severely damaged.
                    }
                }
            }

            return damage;
		}

        public int OnSpellHit(Spell spell, int damage)
        {
            Mobile owner = this.Parent as Mobile;

            if (owner == null)
                return damage;

            double ar = 0.0;

            double arShield = FeatureList.ArmorChanges.ShieldARWithoutParry * ArmorRating;
            double arShieldSkill = FeatureList.ArmorChanges.ShieldARParryBonus * (owner.Skills.Parry.Value / 100) * ArmorRating;

            ar += arShield;
            ar += arShieldSkill;

            //Determine Chance to Spell Parry
            double chance = (owner.Skills[SkillName.Parry].Value / FeatureList.ParryChanges.ParryMagicChanceDivisor) / 100.0; // 25% chance

            if (chance < 0.01)
                chance = 0.01;

            //Check For Successful Parry
            if (owner.CheckSkill(SkillName.Parry, chance))
            {
                double BasePercent = FeatureList.ArmorChanges.ParryBaseReductionPercent; // 50%
                double ARMultiplier = FeatureList.ArmorChanges.ParryReductionARMultiplier; // 0.5

                //Determine Percent to Reduce            
                double reductionPercent = ((BasePercent * 100) + (ar * ARMultiplier)) / 100; // somewhere b/w 50-58%
                                
                if (reductionPercent > 1.0)
                {
                    reductionPercent = 1.0;
                }

                //Determine Reduction
                int damageReduction = (int)(Math.Round((double)reductionPercent * (double)damage));

                //Reduce Damage
                damage -= damageReduction;
                                
                owner.FixedEffect(0x5683, 10, 16);                
            }

            return damage;
        }
	}
}
