using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Scripts;

namespace Server.Spells.Eighth
{
    public class FireElementalSpell : MagerySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                "Fire Elemental", "Kal Vas Xen Flam",
                269,
                9050,
                false,
                Reagent.Bloodmoss,
                Reagent.MandrakeRoot,
                Reagent.SpidersSilk,
                Reagent.SulfurousAsh
            );

        public override SpellCircle Circle { get { return SpellCircle.Eighth; } }

        public FireElementalSpell(Mobile caster, Item scroll)
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

                SpellHelper.Summon(new SummonedFireElemental(), Caster, 0x217, duration, false, false);
            }

            FinishSequence();
        }
    }
}