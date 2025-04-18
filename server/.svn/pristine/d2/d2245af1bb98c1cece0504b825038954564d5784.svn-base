using System;
using Server;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;

namespace Server.Spells.First
{
	public class HealSpell : MagerySpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Heal", "In Mani",
				224,
				9061,
				Reagent.Garlic,
				Reagent.Ginseng,
				Reagent.SpidersSilk
			);

		public override SpellCircle Circle { get { return SpellCircle.First; } }

		public HealSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

		public override void OnCast()
		{
            if (Caster is BaseCreature && Caster.NetState == null)
            {
                this.Target(Caster);
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
			else if ( m.IsDeadBondedPet )
			{
				Caster.SendLocalizedMessage( 1060177 ); // You cannot heal a creature that is already dead!
			}
			else if ( m is BaseCreature && ((BaseCreature)m).IsAnimatedDead )
			{
				Caster.SendLocalizedMessage( 1061654 ); // You cannot heal that which is not alive.
			}
            else if (m is Golem || (m is BaseCreature && ((BaseCreature)m).Pseu_CanBeHealed == false))
			{
				Caster.LocalOverheadMessage( MessageType.Regular, 0x3B2, 500951 ); // You cannot heal that.
			}
			else if ( CheckBSequence( m ) )
			{
				SpellHelper.Turn( Caster, m );

				int toHeal;

				if (Caster is PlayerMobile)
				{
					PlayerMobile pm = (PlayerMobile)Caster;
					toHeal = pm.MiniHealStrength;
					if ( pm.MiniHealStrength >=2 )
						pm.MiniHealStrength -= 2;
				}
				else
				{
					toHeal = (int)(Caster.Skills[SkillName.Magery].Value * 0.05);
					toHeal += Utility.Random( 1, 5 );
				}
				//m.Heal( toHeal, Caster );
				SpellHelper.Heal( toHeal, m, Caster );

				m.FixedParticles( 0x376A, 9, 32, 5005, EffectLayer.Waist );
				m.PlaySound( 0x1F2 );
			}

			FinishSequence();
		}

		public class InternalTarget : Target
		{
			private HealSpell m_Owner;

			public InternalTarget( HealSpell owner ) : base( Core.ML ? 10 : 12, false, TargetFlags.Beneficial )
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