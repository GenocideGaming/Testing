using System;
using Server.Targeting;
using Server.Network;
using Server.Scripts;
using Server.Items;
using Server.Factions;
using Server.Mobiles;
using Server.Misc;
using Server.Scripts.Custom.Citizenship;

namespace Server.Spells.Sixth
{
	public class ExplosionSpell : MagerySpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Explosion", "Vas Ort Flam",
				230,
				9041,
				Reagent.Bloodmoss,
				Reagent.MandrakeRoot
			);

        public override int BaseDamage { get { return 33; } }
        public override int DamageVariation { get { return 5; } }
        public override double InterruptChance { get { return 100; } }
        public override double ResistDifficulty { get { return 0; } }

		public override SpellCircle Circle { get { return SpellCircle.Sixth; } }

		public ExplosionSpell( Mobile caster, Item scroll )
			: base( caster, scroll, m_Info )
		{
		}

		public override bool DelayedDamageStacking { get { return !Core.AOS; } }

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

		public override bool DelayedDamage { get { return false; } }

        private const string AggressorFormat = "You are attacking {0}!";
        private const string AggressedFormat = "{0} is attacking you!";
        private const int Hue = 0x22;

      
		public void Target( Mobile m )
		{



            if (!Caster.CanSee(m) || m.Hidden)
			{
				Caster.SendLocalizedMessage( 500237 ); // Target can not be seen.
			}

			else if ( Caster.CanBeHarmful( m ) && CheckSequence() )
			{
				Mobile attacker = Caster, defender = m;

				SpellHelper.Turn( Caster, m );

				SpellHelper.CheckReflect( (int) this.Circle, Caster, ref m );

				InternalTimer t = new InternalTimer( this, attacker, defender, m );
				t.Start();
                // don't do message if it's not a player casting the spell or if they don't have any other explosions coming
                if (attacker is PlayerMobile 
                    && defender is PlayerMobile
                    && ((PlayerMobile)defender).LastExplosionAttacker != null) 
                {
                    //defender.LocalOverheadMessage(MessageType.Regular, Hue, true, String.Format(AggressedFormat, attacker.Name));   // on target, player will see message "name is attacking you"
                    defender.LocalOverheadMessage(MessageType.Regular, Hue, true, "The pressure dropped!"); 
                }
                if (attacker is PlayerMobile && defender is PlayerMobile)
                {
                    PlayerMobile pm = defender as PlayerMobile;
                    pm.LastExplosionAttacker = attacker as PlayerMobile;
                }
			}

			FinishSequence();
		}

		private class InternalTimer : Timer
		{
			private MagerySpell m_Spell;
			private Mobile m_Target;
			private Mobile m_Attacker, m_Defender;

			public InternalTimer( MagerySpell spell, Mobile attacker, Mobile defender, Mobile target )
				: base( TimeSpan.FromSeconds(2.25 ) )
			{
				m_Spell = spell;
				m_Attacker = attacker;
				m_Defender = defender;
				m_Target = target;

				if ( m_Spell != null )
					m_Spell.StartDelayedDamageContext( attacker, this );
                
				Priority = TimerPriority.FiftyMS;
			}

			protected override void OnTick()
			{
                if (m_Target is PlayerMobile)
                {
                    PlayerMobile pm = m_Target as PlayerMobile;
                    if (pm.LastExplosionAttacker == m_Attacker)
                    {
                        pm.LastExplosionAttacker = null;
                    }
                }
                if ( m_Attacker.HarmfulCheck( m_Defender ) )
				{
                    bool interrupt = m_Spell.CheckInterrupt(m_Defender);

                    double damage = m_Spell.GetDamage(m_Defender);					

					m_Target.FixedParticles( 0x36BD, 20, 10, 5044, EffectLayer.Head );
					m_Target.PlaySound( 0x307 );

                    SpellHelper.Damage(m_Spell, m_Target, damage, interrupt);

					if ( m_Spell != null )
						m_Spell.RemoveDelayedDamageContext( m_Attacker );

                    SpellHelper.CalculateDiminishingReturns(m_Spell, m_Attacker, m_Target);
				}
			}
		}

		private class InternalTarget : Target
		{
			private ExplosionSpell m_Owner;

			public InternalTarget( ExplosionSpell owner )
				: base(12, false, TargetFlags.Harmful )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is Mobile )
					m_Owner.Target( (Mobile) o );
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