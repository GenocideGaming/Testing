using System;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;

namespace Server.Spells.First
{
	public class ClumsySpell : MagerySpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Clumsy", "Uus Jux",
				212,
				9031,
				Reagent.Bloodmoss,
				Reagent.Nightshade
			);

        public override int BaseDamage { get { return 0; } }
        public override int DamageVariation { get { return 0; } }
        public override double InterruptChance { get { return 0.0; } }
        public override double ResistDifficulty { get { return 0.0; } }

		public override SpellCircle Circle { get { return SpellCircle.First; } }

		public ClumsySpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
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

                bool resisted = CheckResist(m);

                if (resisted)
                {                   
                    m.SendLocalizedMessage(501783); // You feel yourself resisting magical energy.
                }

                else
                {
                    bool interrupt = CheckInterrupt(m);

                    //Automatic Disrupt if Not Already Affected
                    if (!AlreadyAfflicted(m))
                    {
                        interrupt = true;
                    }

                    if (m.Spell != null)
                    {
                        m.Spell.OnCasterHurt(interrupt);
                    }

                    SpellHelper.AddStatCurse(Caster, m, StatType.Dex);

                    m.Paralyzed = false;				

				    int percentage = (int)(SpellHelper.GetOffsetScalar( Caster, m, true ) * 100);
				    TimeSpan length = SpellHelper.GetDuration( Caster, m );

				    BuffInfo.AddBuff( m, new BuffInfo( BuffIcon.Clumsy, 1075831, length, m, percentage.ToString() ) );
                }

                m.FixedParticles(0x3779, 10, 15, 5002, EffectLayer.Head);
                m.PlaySound(0x1DF);

				HarmfulSpell( m );
			}

			FinishSequence();
		}

        private bool AlreadyAfflicted(Mobile m)
        {
            System.Collections.Generic.List<StatMod> mods = m.StatMods;

            foreach (StatMod mod in mods)
            {
                if (!mod.HasElapsed() && mod.Name == "[Magic] Dex Offset")  // see comment below
                { 
                    return true; 
                }

                /*  this is a shitty way of checking if the mod applies but runUO doesn't provide anything better.
                 *  checking for the string is similar to how runuo does it internally but i'd prefer to add a register
                 *  like the one used by the Curse spell code */
            }

            return false;
        }

		private class InternalTarget : Target
		{
			private ClumsySpell m_Owner;

			public InternalTarget( ClumsySpell owner ) : base( 12, false, TargetFlags.Harmful )
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