using System;
using Server;
using Server.Items;
using Server.Spells;
using Server.Mobiles;
using Server.Scripts;

namespace Server.Misc
{
    public class RegenRates
    {
        [CallPriority(10)]
        public static void Configure()
        {
            Mobile.DefaultHitsRate = TimeSpan.FromSeconds(FeatureList.RegenRates.DefaultHitsRate);
            Mobile.DefaultStamRate = TimeSpan.FromSeconds(FeatureList.RegenRates.DefaultStamRate);
            Mobile.DefaultManaRate = TimeSpan.FromSeconds(FeatureList.RegenRates.DefaultManaRate);

            Mobile.ManaRegenRateHandler = new RegenRateHandler(Mobile_ManaRegenRate);
        }

        private static void CheckBonusSkill(Mobile m, int cur, int max, SkillName skill)
        {
            if (!m.Alive)
                return;

            double n = (double)cur / max;
            double v = Math.Sqrt(m.Skills[skill].Value * 0.005);

            n *= (1.0 - v);
            n += v;

            m.CheckSkill(skill, n);
        }

        private static bool CheckTransform(Mobile m, Type type)
        {
            return TransformationSpellHelper.UnderTransformation(m, type);
        }

        private static TimeSpan Mobile_HitsRegenRate(Mobile from)
        {
            int points = 0;

            if (from is BaseCreature && !((BaseCreature)from).IsAnimatedDead)
                points += 4;

            if ((from is BaseCreature && ((BaseCreature)from).IsParagon) || from is Leviathan)
                points += 40;

            return TimeSpan.FromSeconds(1.0 / (0.1 * (1 + points)));
        }

        private static TimeSpan Mobile_StamRegenRate(Mobile from)
        {
            if (from.Skills == null)
                return Mobile.DefaultStamRate;

            CheckBonusSkill(from, from.Stam, from.StamMax, SkillName.Focus);

            int points = (int)(from.Skills[SkillName.Focus].Value * 0.1);

            if ((from is BaseCreature && ((BaseCreature)from).IsParagon) || from is Leviathan)
                points += 40;

            if (points < -1)
                points = -1;

            return TimeSpan.FromSeconds(1.0 / (0.1 * (2 + points)));
        }

        private static TimeSpan Mobile_ManaRegenRate(Mobile from)
        {
            if (from.Skills == null)
                return Mobile.DefaultManaRate;

            double rate = FeatureList.RegenRates.DefaultManaRate;
            double armorPenalty = GetArmorPenalties(from);
            double meditationFactor = from.Skills[SkillName.Meditation].Value / 100;

            rate *= (0.50 + (0.50 * armorPenalty)); //Rate 50% with no armor, rate 100% with full armor

            if (meditationFactor > 1.0) //Meditation skill benefit capped at 100 skill
                meditationFactor = 1.0;

            rate *= (1 - (0.5 * meditationFactor)); //Rate 50% with 100 Meditation Skill

            if (from.Meditating) //Rate 50% with active meditation
                rate *= 0.5;                               

            return TimeSpan.FromSeconds(rate);            
        }

        public static double GetArmorPenalties(Mobile from)
        {          
            double armorMedPenalty = 0.0;

            //Different locations effect meditation penalty more
            armorMedPenalty += GetArmorMeditationValue(from.NeckArmor as BaseArmor) * .07;
            armorMedPenalty += GetArmorMeditationValue(from.HandArmor as BaseArmor) * .07;
            armorMedPenalty += GetArmorMeditationValue(from.ArmsArmor as BaseArmor) * .14;
            armorMedPenalty += GetArmorMeditationValue(from.HeadArmor as BaseArmor) * .15;
            armorMedPenalty += GetArmorMeditationValue(from.LegsArmor as BaseArmor) * .22;
            armorMedPenalty += GetArmorMeditationValue(from.ChestArmor as BaseArmor) * .35;

            BaseShield shield = from.ShieldArmor as BaseShield;

            bool ChaosOrder = false;

            if (shield != null)
            {
                if (shield.GetType() == typeof(OrderShield))
                {
                    ChaosOrder = true;
                }

                if (shield.GetType() == typeof(ChaosShield))
                {
                    ChaosOrder = true;
                } 
              
                //If Not Chaos or Order Shield
                if (!ChaosOrder)
                {
                    armorMedPenalty += 1 * .35; //Treat Any Shield as Plate Chest for Meditation Purposes                   
                }
            }           

            return armorMedPenalty;
        }

        private static double GetArmorMeditationValue(BaseArmor ar)
        {
            if (ar == null || ar.ArmorAttributes.MageArmor != 0 || ar.Attributes.SpellChanneling != 0)
                return 0;

            switch (ar.MeditationAllowance)
            {
                case ArmorMeditationAllowance.All: return 0;
                case ArmorMeditationAllowance.ThreeQuarter: return .25;
                case ArmorMeditationAllowance.Half: return .5;
                case ArmorMeditationAllowance.Quarter: return .75;
                case ArmorMeditationAllowance.None: return 1;
                default: return 0;
            }
        }
    }
}