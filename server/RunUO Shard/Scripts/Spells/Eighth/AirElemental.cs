using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Scripts;

namespace Server.Spells.Eighth
{
    public class AirElementalSpell : MagerySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                "Air Elemental", "Kal Vas Xen Hur",
                269,
                9010,
                false,
                Reagent.Bloodmoss,
                Reagent.MandrakeRoot,
                Reagent.SpidersSilk
            );

        public override SpellCircle Circle { get { return SpellCircle.Eighth; } }


        public AirElementalSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(FeatureList.SpellChanges.EigthLevelSummonCastTime);
        }
        public override bool CheckCast()
        {
            if (!base.CheckCast())
                return false;

            if ((Caster.Followers + 2) > Caster.FollowersMax)
            {
                Caster.SendLocalizedMessage(1049645); // You have too many followers to summon that creature.
                return false;
            }

            return true;
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                TimeSpan duration;

                int baseDuration = FeatureList.Followers.SummonBaseDuration;
                int durationBonus = (int)((Caster.Skills[SkillName.SpiritSpeak].Value / 100) * FeatureList.Followers.SpiritSpeakSummonDurationBonus);

                duration = TimeSpan.FromSeconds(baseDuration + durationBonus);

                SpellHelper.Summon(new SummonedAirElemental(), Caster, 0x217, duration, false, false);
            }

            FinishSequence();
        }
    }
}