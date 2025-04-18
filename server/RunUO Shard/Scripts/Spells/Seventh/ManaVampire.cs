using System;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;
using Server.Scripts;

namespace Server.Spells.Seventh
{
	public class ManaVampireSpell : MagerySpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Mana Vampire", "Ort Sanct",
				221,
				9032,
				Reagent.BlackPearl,
				Reagent.Bloodmoss,
				Reagent.MandrakeRoot,
				Reagent.SpidersSilk
			);

        public override int BaseDamage { get { return 20; } }
        public override int DamageVariation { get { return 5; } }
        public override double InterruptChance { get { return 100; } }
        public override double ResistDifficulty { get { return 10; } }

		public override SpellCircle Circle { get { return SpellCircle.Seventh; } }

		public ManaVampireSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
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

                     //Determine Mana Loss
                     int min = BaseDamage - DamageVariation;
                     int max = BaseDamage + DamageVariation;
                     int manaLoss = Utility.RandomMinMax(min, max);

                     //Manually Double Damage for Creatures + Bonus
                     if (m is BaseCreature)
                     {
                         manaLoss *= (int)(Math.Round(FeatureList.SpellChanges.BaseCreatureSpellDamageMultiplier * 2));
                     }

                     //Target Has Enough Mana to Lose
                     if (m.Mana > manaLoss)
                     {
                         m.Mana -= manaLoss;
                     }

                     //Target Will Lose Remaining Mana and Takes Mana Unavailable As Damage
                     else
                     {
                         int damage = manaLoss - m.Mana;
                         m.Damage(damage);
                         m.Mana = 0;
                     }
                }

				m.FixedParticles( 0x374A, 10, 15, 5054, EffectLayer.Head );
				m.PlaySound( 0x1F9 );

                m.Paralyzed = false;

		        HarmfulSpell( m );
			}

			FinishSequence();
		}

		private class InternalTarget : Target
		{
			private ManaVampireSpell m_Owner;

			public InternalTarget( ManaVampireSpell owner ) : base(12, false, TargetFlags.Harmful )
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