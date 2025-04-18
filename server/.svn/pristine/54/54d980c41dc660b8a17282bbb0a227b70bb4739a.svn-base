using System;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Factions;
using Server.Scripts;

namespace Server
{
    public class GetMobileDifficulty
    {
        public GetMobileDifficulty()
        {           
        }

        public static double GetDifficultyValue(Mobile target)
        {
            double val = 0;

            BaseCreature m_Mobile = target as BaseCreature;

            //If Not a Creature
            if (m_Mobile == null)
                return val;

            //Active Speed
            double speedValue = .6 - m_Mobile.CurrentSpeed;

            val += speedValue * 25;

            //Can't Move
            if (m_Mobile.DisallowAllMoves)
                val -= 25;

            //Hits            
            val += ((double)m_Mobile.HitsMax / 15);

            //SetDamage
            double damageAverage = ((double)m_Mobile.DamageMin + (double)m_Mobile.DamageMax) / 2;
            val += damageAverage / 2;

            //Weapon Equipped: Currently Disabled

            //Find Best Combat Skill
            double bestSkillValue = 0;

            if (m_Mobile.Skills[SkillName.Wrestling].Value > bestSkillValue)
                bestSkillValue = m_Mobile.Skills[SkillName.Wrestling].Value;

            if (m_Mobile.Skills[SkillName.Archery].Value > bestSkillValue)
                bestSkillValue = m_Mobile.Skills[SkillName.Archery].Value;

            if (m_Mobile.Skills[SkillName.Fencing].Value > bestSkillValue)
                bestSkillValue = m_Mobile.Skills[SkillName.Fencing].Value;

            if (m_Mobile.Skills[SkillName.Macing].Value > bestSkillValue)
                bestSkillValue = m_Mobile.Skills[SkillName.Macing].Value;

            if (m_Mobile.Skills[SkillName.Swords].Value > bestSkillValue)
                bestSkillValue = m_Mobile.Skills[SkillName.Swords].Value;

            val += bestSkillValue / 8;

            //VirtualArmor
            val += ((double)m_Mobile.VirtualArmor / 2);

            //Casts Spells
            if (m_Mobile.DictCombatAction[CombatAction.CombatSpell] > 0)
            {
                double magerySkill = m_Mobile.Skills[SkillName.Magery].Value;
                val += magerySkill / 4;

                double evalIntSkill = m_Mobile.Skills[SkillName.EvalInt].Value;
                val += evalIntSkill / 20;
            }

            //MagicResist
            double magicResistSkill = m_Mobile.Skills[SkillName.MagicResist].Value;
            val += magicResistSkill / 15;

            //Will Heal Self
            if (m_Mobile.DictCombatAction[CombatAction.CombatHealSelf] > 0)
            {
                val += 10;
            }

            //Will Heal Others
            if (m_Mobile.DictCombatAction[CombatAction.CombatHealOther] > 0)
            {
                val += 10;
            }

            //Has Special Actions
            if (m_Mobile.DictCombatAction[CombatAction.CombatSpecialAction] > 0)
            {
                //Apply Weapon Poison
                if (m_Mobile.DictCombatSpecialAction[CombatSpecialAction.ApplyWeaponPoison] > 0)
                {
                    double poisoningSkill = m_Mobile.Skills[SkillName.Poisoning].Value;
                    int WeaponPoisonValue = (int)(Math.Floor(poisoningSkill / 25)) + 1;

                    val += (((double)WeaponPoisonValue * (double)WeaponPoisonValue));
                }

                //Poison Hits
                if (m_Mobile.DictCombatSpecialAction[CombatSpecialAction.PoisonHit] > 0)
                {
                    //Currently Not Implemented
                }

                //Throws Bombs
                if (m_Mobile.DictCombatSpecialAction[CombatSpecialAction.ThrowBomb] > 0)
                {
                    val += 4;
                }

                //Breath Attack
                if (m_Mobile.DictCombatSpecialAction[CombatSpecialAction.BreathAttack] > 0)
                {
                    //Currently Not Implemented

                    double breathDamage = ((double)m_Mobile.HitsMax * .04);
                    val += (breathDamage / 4);
                }
            }

            //Causes Poison (Temporary: Will Move to CombatSpecialAction)
            Poison poisonHit = m_Mobile.PoisonCustomHit;
            if (poisonHit == null) poisonHit = m_Mobile.HitPoison;

            if (poisonHit != null)
            {
                int poisonHitValue = poisonHit.Level + 1;
                val += (((double)poisonHitValue * (double)poisonHitValue)); //1, 4, 9, 16, 25
            }

            //Poison Immunity
            Poison poisonImmunity = m_Mobile.PoisonCustomImmune;
            if (poisonImmunity == null) poisonImmunity = m_Mobile.PoisonImmune;

            if (poisonImmunity != null)
            {
                int poisonImmunityValue = poisonImmunity.Level;
                val += (((double)poisonImmunityValue * (double)poisonImmunityValue) / 2); //0, .5, 2, 4.5, 8
            }

            //BreathAttack (Temporary: Will Move to CombatSpecialAction)
            if (m_Mobile.HasBreath)
            {
                double breathDamage = ((double)m_Mobile.HitsMax * .04);
                val += (breathDamage / 4);
            }

            return val;

            /* OLD FORMULA
		    double val = (targ.HitsMax) + targ.StamMax + targ.ManaMax;

		    val += targ.SkillsTotal / 10;

		    if ( val > 700 )
			    val = 700 + (int)((val - 700) * (3.0 / 11));

		    BaseCreature bc = targ as BaseCreature;

		    if ( IsMageryCreature( bc ) )
			    val += 100;

		    if ( IsFireBreathingCreature( bc ) )
			    val += 100;

		    if ( IsPoisonImmune( bc ) )
			    val += 100;

		    val += GetPoisonLevel( bc ) * 20;

		    val /= 10;

		    if ( bc != null && bc.IsParagon )
			    val += 40.0;
             
            return val
            */
        }
    }
}