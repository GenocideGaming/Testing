using System;
using System.Collections;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;

namespace Server.Spells.Fourth
{
	public class CurseSpell : MagerySpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Curse", "Des Sanct",
				227,
				9031,
				Reagent.Nightshade,
				Reagent.Garlic,
				Reagent.SulfurousAsh
			);

        public override int BaseDamage { get { return 0; } }
        public override int DamageVariation { get { return 0; } }
        public override double InterruptChance { get { return 100; } }
        public override double ResistDifficulty { get { return 25; } }

		public override SpellCircle Circle { get { return SpellCircle.Fourth; } }

		public CurseSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

		public override void OnCast()
		{
            BaseCreature casterCreature = Caster as BaseCreature;

            if (casterCreature != null && casterCreature.NetState == null)
            {
                if (casterCreature.SpellTarget != null)
                {
                    this.Target(casterCreature.SpellTarget);
                }
            }

            else
            {
                Caster.Target = new InternalTarget(this);
            }
		}

		private static Hashtable m_UnderEffect = new Hashtable();

		public static void RemoveEffect( object state )
		{
			Mobile m = (Mobile)state;

			m_UnderEffect.Remove( m );

			m.UpdateResistances();
		}

		public static bool UnderEffect( Mobile m )
		{
			return m_UnderEffect.Contains( m );
		}

		public void Target( Mobile m )
		{
            if (!Caster.CanSee(m) || m.Hidden)
			{
				Caster.SendLocalizedMessage( 500237 ); // Target can not be seen.
			}

			else if ( CheckHSequence( m ) )
			{
				SpellHelper.Turn( Caster, m );

				SpellHelper.CheckReflect( (int)this.Circle, Caster, ref m );

                bool resisted = CheckResist(m);

                if (resisted)
                {
                    m.SendLocalizedMessage(501783); // You feel yourself resisting magical energy.
                }

                else
                {
                    bool interrupt = CheckInterrupt(m);

                    if (m.Spell != null)
                    {
                        m.Spell.OnCasterHurt(interrupt);
                    }

                    SpellHelper.AddStatCurse(Caster, m, StatType.Str); SpellHelper.DisableSkillCheck = true;
                    SpellHelper.AddStatCurse(Caster, m, StatType.Dex);
                    SpellHelper.AddStatCurse(Caster, m, StatType.Int); SpellHelper.DisableSkillCheck = false;

                    Timer t = (Timer)m_UnderEffect[m];

                    if (Caster.Player && m.Player && t == null)	//On OSI you CAN curse yourself and get this effect.
                    {
                        TimeSpan duration = SpellHelper.GetDuration(Caster, m);
                        m_UnderEffect[m] = t = Timer.DelayCall(duration, new TimerStateCallback(RemoveEffect), m);
                        m.UpdateResistances();
                    }                             

                    int percentage = (int)(SpellHelper.GetOffsetScalar(Caster, m, true) * 100);
                    TimeSpan length = SpellHelper.GetDuration(Caster, m);

                    string args = String.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}", percentage, percentage, percentage, 10, 10, 10, 10);

                    BuffInfo.AddBuff(m, new BuffInfo(BuffIcon.Curse, 1075835, 1075836, length, m, args.ToString()));
                }

                m.Paralyzed = false;

                m.FixedParticles(0x374A, 10, 15, 5028, EffectLayer.Waist);
                m.PlaySound(0x1E1);

				HarmfulSpell( m );
			}

			FinishSequence();
		}

		private class InternalTarget : Target
		{
			private CurseSpell m_Owner;

			public InternalTarget( CurseSpell owner ) : base( Core.ML? 10 : 12, false, TargetFlags.Harmful )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is Mobile )
					m_Owner.Target( (Mobile)o );
			}

			protected override void OnTargetFinish( Mobile from )
			{
				m_Owner.FinishSequence();
			}
		}
	}
}