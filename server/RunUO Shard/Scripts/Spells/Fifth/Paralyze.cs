using System;
using Server.Mobiles;
using Server.Targeting;
using Server.Network;
using Server.Scripts;

namespace Server.Spells.Fifth
{
	public class ParalyzeSpell : MagerySpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Paralyze", "An Ex Por",
				218,
				9012,
				Reagent.Garlic,
				Reagent.MandrakeRoot,
				Reagent.SpidersSilk
			);

        public override int BaseDamage { get { return 0; } }
        public override int DamageVariation { get { return 0; } }
        public override double InterruptChance { get { return 100; } }
        public override double ResistDifficulty { get { return 20; } }

		public override SpellCircle Circle { get { return SpellCircle.Fifth; } }

		public ParalyzeSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
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

		public void Target( Mobile m )
		{
            if (!Caster.CanSee(m) && !m.Hidden )
			{
				Caster.SendLocalizedMessage( 500237 ); // Target can not be seen.
			}

			else if ( CheckHSequence( m ) )
			{
				SpellHelper.Turn( Caster, m );

				SpellHelper.CheckReflect( (int)this.Circle, Caster, ref m );

                bool interrupt = CheckInterrupt(m);

                if (m.Spell != null)
                {
                    m.Spell.OnCasterHurt(interrupt);
                }

                double duration;

                bool resisted = CheckResist(m);

                duration = FeatureList.SkillChanges.BaseParalyzeDuration + 
                    Caster.Skills[SkillName.Magery].Value / 100 * FeatureList.SkillChanges.MaxMageryParalyzeDuration - 
                    m.Skills[SkillName.MagicResist].Value / 100 * FeatureList.SkillChanges.MaxResistParalyzeDuration;                   

                if (!m.Player)
                {
                    duration *= FeatureList.SpellChanges.BaseCreatureParalyzeMultiplier; 
                }

                if (resisted)
                {
                    duration *= FeatureList.SkillChanges.ResistedParalyzeDurationReduction;
                }           

                m.Paralyze(TimeSpan.FromSeconds(duration));               				

				m.PlaySound( 0x204 );
				m.FixedEffect( 0x376A, 6, 1 );

				HarmfulSpell( m );

                SpellHelper.CalculateDiminishingReturns(this, Caster, m);
			}

			FinishSequence();
		}

		public class InternalTarget : Target
		{
			private ParalyzeSpell m_Owner;

			public InternalTarget( ParalyzeSpell owner ) : base( Core.ML ? 10 : 12, false, TargetFlags.Harmful )
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