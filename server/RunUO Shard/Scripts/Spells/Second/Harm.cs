using System;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;
using Server.Scripts;
using Server.Items;
using Server.Factions;

namespace Server.Spells.Second
{
	public class HarmSpell : MagerySpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Harm", "An Mani",
				212,
				Core.AOS ? 9001 : 9041,
				Reagent.Nightshade,
				Reagent.SpidersSilk
			);

        public override int BaseDamage { get { return 7; } }
        public override int DamageVariation { get { return 1; } }
        public override double InterruptChance { get { return 100.0; } } 
        public override double ResistDifficulty { get { return 0; } }

		public override SpellCircle Circle { get { return SpellCircle.Second; } }

		public HarmSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
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

		public override bool DelayedDamage{ get{ return false; } }
        

		public void Target( Mobile m )
		{
            if (!Caster.CanSee(m) || m.Hidden)
			{
				Caster.SendLocalizedMessage( 500237 ); // Target can not be seen..
			}

			else if ( CheckHSequence( m ) )
			{
				SpellHelper.Turn( Caster, m );
				SpellHelper.CheckReflect( (int)this.Circle, Caster, ref m );

				bool interrupt = false;
				int harmCount = m.HarmsTaken;
				
				if (harmCount < 1)
                	interrupt = true;

				double damage = GetDamage(m);
				
				m.FixedParticles( 0x374A, 10, 15, 5013, EffectLayer.Waist );
				m.PlaySound( 0x1F1 );
				
                SpellHelper.Damage(this, m, damage, interrupt);
                
                SpellHelper.CalculateDiminishingReturns(this, Caster, m); //Increase Harm Count
			}

			FinishSequence();
		}

		private class InternalTarget : Target
		{
			private HarmSpell m_Owner;

			public InternalTarget( HarmSpell owner ) : base( Core.ML ? 10 : 12, false, TargetFlags.Harmful )
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