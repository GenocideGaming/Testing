using System;
using Server.Targeting;
using Server.Network;
using Server.Scripts;
using Server.Mobiles;

namespace Server.Spells.Second
{
	public class CureSpell : MagerySpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Cure", "An Nox",
				212,
				9061,
				Reagent.Garlic,
				Reagent.Ginseng
			);

		public override SpellCircle Circle { get { return SpellCircle.Second; } }

		public CureSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
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
            if (!Caster.CanSee(m) || m.Hidden)
			{
				Caster.SendLocalizedMessage( 500237 ); // Target can not be seen.
			}

			else if ( CheckBSequence( m ) )
			{
				SpellHelper.Turn( Caster, m );

				Poison poison = m.Poison;

                if (poison != null)
				{
                    double cureEffectiveness = Caster.Skills[SkillName.Magery].Value;
                    double cureDifficulty = 0.0;

                    if (poison == Poison.Lesser)
                    {
                        cureDifficulty = FeatureList.Poison.LesserPoisonCureChance;
                    }

                    else if (poison == Poison.Regular)
                    {
                        cureDifficulty = FeatureList.Poison.RegularPoisonCureChance;
                    }

                    else if (poison == Poison.Greater)
                    {
                        cureDifficulty = FeatureList.Poison.GreaterPoisonCureChance;
                    }

                    else if (poison == Poison.Deadly)
                    {
                        cureDifficulty = FeatureList.Poison.DeadlyPoisonCureChance;
                    }

                    else if (poison == Poison.Lethal)
                    {
                        cureDifficulty = FeatureList.Poison.LethalPoisonCureChance;
                    }

                    int cureChance = (int)(cureEffectiveness * cureDifficulty);

                    if (cureChance > Utility.Random(100))
                    {
                        if (m.CurePoison(Caster))
                        {
                            if (Caster != m)
                                Caster.SendLocalizedMessage(1010058); // You have cured the target of all poisons!

                            m.SendLocalizedMessage(1010059); // You have been cured of all poisons.
                        }
                    }

                    else
                    {
                        m.SendLocalizedMessage(1010060); // You have failed to cure your target!
                    }
				}

				m.FixedParticles( 0x373A, 10, 15, 5012, EffectLayer.Waist );
				m.PlaySound( 0x1E0 );
			}

			FinishSequence();
		}

		public class InternalTarget : Target
		{
			private CureSpell m_Owner;

			public InternalTarget( CureSpell owner ) : base( Core.ML ? 10 : 12, false, TargetFlags.Beneficial )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is Mobile )
				{
					m_Owner.Target( (Mobile)o );
				}
			}

			protected override void OnTargetFinish( Mobile from )
			{
				m_Owner.FinishSequence();
			}
		}
	}
}