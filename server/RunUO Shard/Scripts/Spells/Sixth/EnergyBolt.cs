using System;
using Server.Targeting;
using Server.Network;
using Server.Scripts;
using Server.Items;
using Server.Factions;
using Server.Mobiles;

namespace Server.Spells.Sixth
{
	public class EnergyBoltSpell : MagerySpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Energy Bolt", "Corp Por",
				230,
				9022,
				Reagent.BlackPearl,
				Reagent.Nightshade
			);

        public override int BaseDamage { get { return 32; } }
        public override int DamageVariation { get { return 5; } }
        public override double InterruptChance { get { return 100; } }
        public override double ResistDifficulty { get { return 0; } }
        public override double DamageDelay { get { return .25; } }

		public override SpellCircle Circle { get { return SpellCircle.Sixth; } }

		public EnergyBoltSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
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

		public override bool DelayedDamage{ get{ return true; } }

		public void Target( Mobile m )
		{
            if (!Caster.CanSee(m) || m.Hidden)
			{
				Caster.SendLocalizedMessage( 500237 ); // Target can not be seen.
			}

			else if ( CheckHSequence( m ) )
			{
				Mobile source = Caster;

				SpellHelper.Turn( Caster, m );

				SpellHelper.CheckReflect( (int)this.Circle, ref source, ref m );

                bool interrupt = CheckInterrupt(m);                

				double damage = GetDamage(m);

				// Do the effects
				source.MovingParticles( m, 0x379F, 7, 0, false, true, 3043, 4043, 0x211 );
				source.PlaySound( 0x20A );

				// Deal the damage
                SpellHelper.Damage(this, m, damage, interrupt);

                SpellHelper.CalculateDiminishingReturns(this, Caster, m);
			}

			FinishSequence();
		}

		private class InternalTarget : Target
		{
			private EnergyBoltSpell m_Owner;

			public InternalTarget( EnergyBoltSpell owner ) : base( Core.ML ? 10 : 12, false, TargetFlags.Harmful )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
                // This whole thing is ugly as balls and should probably be refactored at some point
                if (o is IDamageableItemMilitiaRestricted)
                    o = ((IDamageableItemMilitiaRestricted)o).Link;
                else if (o is DamageableItem)
                {
                    o = ((DamageableItem)o).Link;
                    m_Owner.Target((Mobile)o);
                    return;
                }
                else if (o is Mobile)
                {
                    m_Owner.Target((Mobile)o);
                    return;
                }

                if (o is DamageableItemMilitiaRestricted)
                {
                    Mobile newTarget = m_Owner.GetMilitiaRestrictedMobileIfAttackable(from, (DamageableItemMilitiaRestricted)o);
                    if (newTarget != null) m_Owner.Target(newTarget);
                }
			}

			protected override void OnTargetFinish( Mobile from )
			{
				m_Owner.FinishSequence();
			}
		}
	}
}