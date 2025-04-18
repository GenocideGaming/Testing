using System;
using System.Collections.Generic;
using Server.Network;
using Server.Targeting;
using Server.Mobiles;
using Server.Scripts;

namespace Server.Spells.Fourth
{
	public class ManaDrainSpell : MagerySpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Mana Drain", "Ort Rel",
				215,
				9031,
				Reagent.BlackPearl,
				Reagent.MandrakeRoot,
				Reagent.SpidersSilk
			);

        public override int BaseDamage { get { return 5; } }
        public override int DamageVariation { get { return 2; } }
        public override double InterruptChance { get { return 100; } }
        public override double ResistDifficulty { get { return 0; } }

		public override SpellCircle Circle { get { return SpellCircle.Fourth; } }

		public ManaDrainSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
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

		private static Dictionary<Mobile, Timer> m_Table = new Dictionary<Mobile, Timer>();
        
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
                        m.Damage(damage, Caster);
                        m.Mana = 0;
                    }
                }

                m.Paralyzed = false;

                m.FixedParticles(0x374A, 10, 15, 5032, EffectLayer.Head);
                m.PlaySound(0x1F8);

                HarmfulSpell(m);				
			}

			FinishSequence();
		}

		private class InternalTarget : Target
		{
			private ManaDrainSpell m_Owner;

			public InternalTarget( ManaDrainSpell owner ) : base( Core.ML ? 10 : 12, false, TargetFlags.Harmful )
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