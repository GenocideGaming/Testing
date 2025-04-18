using System;
using Server;
using Server.Items;

namespace Server.Spells
{
    public abstract class MagerySpell : Spell
    {
        public MagerySpell(Mobile caster, Item scroll, SpellInfo info)
            : base(caster, scroll, info)
        {
        }

        public abstract SpellCircle Circle { get; }

        public override bool ConsumeReagents()
        {
            if (base.ConsumeReagents())
                return true;

            if (ArcaneGem.ConsumeCharges(Caster, (Core.SE ? 1 : 1 + (int)Circle)))
                return true;

            return false;
        }             

        public override void GetCastSkills(out double min, out double max)
        {
            int circle = (int)Circle;

            if (Scroll != null)
            {                
                circle -= 2;
            }

            double avg = circle;

            //Override for Circle 1
            if (circle == 0)
            {
                min = 0;
            }

            else
            {
                min = (circle + 1) * 10;
            }

            //If Casting Off Recall Scroll (Override)
            if (Scroll != null && Scroll.GetType() == typeof(RecallScroll))
            {                
                min = 3;
                max = 28;
            }

            //All Other Casting
            else
            {
                max = ((circle + 1) * 10) + 25;     
            }
        }

        private static int[] m_ManaTable = new int[] { 4, 6, 9, 11, 14, 20, 40, 50 };

        public override int GetMana()
        {
            if (Scroll is BaseWand)
                return 0;

            return m_ManaTable[(int)Circle];
        }

        public override TimeSpan GetCastDelay()
        {
            if (Scroll is BaseWand)
            	return TimeSpan.FromSeconds(.25);

            return TimeSpan.FromSeconds(0.5 + (0.25 * (int)Circle));
        }

        public override TimeSpan CastDelayBase
        {
            get
            {
                return TimeSpan.FromSeconds((3 + (int)Circle) * CastDelaySecondsPerTick);
            }
        }
    }
}
