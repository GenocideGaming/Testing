using System;
using System.Collections.Generic;
using Server.Network;
using Server.Items;
using Server.Targeting;
using Server.Mobiles;
using Server.Scripts;
using Server.Commands;

namespace Server.Spells.Eighth
{
	public class EarthquakeSpell : MagerySpell
	{
        public static bool Weak = false;
        public static new void Initialize()
        {
            // weak earthquakes do a maximum of 55% of a mobs life (rather than a static amount of damage)
            CommandSystem.Register("earthquakeweak", AccessLevel.GameMaster, new CommandEventHandler(EarthquakeWeakCommand));
            CommandSystem.Register("earthquakestrong", AccessLevel.GameMaster, new CommandEventHandler(EarthquakeStrongCommand));
        }

        public static void EarthquakeWeakCommand(CommandEventArgs e)
        {
            Weak = true;
        }

        public static void EarthquakeStrongCommand(CommandEventArgs e)
        {
            Weak = false;
        }
        
        private static SpellInfo m_Info = new SpellInfo(
				"Earthquake", "In Vas Por",
				233,
				9012,
				false,
				Reagent.Bloodmoss,
				Reagent.Ginseng,
				Reagent.MandrakeRoot,
				Reagent.SulfurousAsh
			);

        public override int BaseDamage { get { return 55; } }
        public override int DamageVariation { get { return 5; } }
        public override double InterruptChance { get { return 100; } }
        public override double ResistDifficulty { get { return 25; } }

		public override SpellCircle Circle { get { return SpellCircle.Eighth; } }

		public EarthquakeSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

		public override bool DelayedDamage{ get{ return true; } }

		public override void OnCast()
		{
			if ( SpellHelper.CheckTown( Caster, Caster ) && CheckSequence() )
			{
				List<Mobile> targets = new List<Mobile>();

				Map map = Caster.Map;

				if ( map != null )
					foreach ( Mobile m in Caster.GetMobilesInRange( 1 + (int)(Caster.Skills[SkillName.Magery].Value / 15.0) ) )
                        if (Caster != m && SpellHelper.ValidIndirectTarget(Caster, m) && Caster.CanBeHarmful(m, false) && (Caster.InLOS(m)))
                        {
                            // either hitting monsters OR pseudoseers can hit players with earthquake
                            if (!m.Player 
                                || (!Caster.Deleted 
                                    && Caster is BaseCreature 
                                    && Caster.NetState != null
                                    && ((BaseCreature)Caster).Pseu_EQPlayerAllowed))
                                targets.Add(m);
                        }

				Caster.PlaySound( 0x220 );

				for ( int i = 0; i < targets.Count; ++i )
				{
					Mobile m = targets[i];

                    double damage = BaseDamage;

                    //Manually Double Damage for Creatures
                    if (m is BaseCreature)
                    {
                        damage *= FeatureList.SpellChanges.BaseCreatureSpellDamageMultiplier;
                    }

                    bool interrupt = CheckInterrupt(m); 

					Caster.DoHarmful( m );
                    if (!Caster.Deleted && Caster is BaseCreature && ((BaseCreature)Caster).Pseu_EQPlayerAllowed && Caster.NetState != null)
                    {
                        damage = Math.Min(m.Hits * 0.4, damage); // pseudoseer does 40% of their hit points with EQ
                    }
                    else if (Weak) // do hit point percentage or BaseDamage, whichever is lower
                    {
                        damage = Math.Min(m.Hits * 0.55, damage);
                    }
                    SpellHelper.Damage(TimeSpan.Zero, m, Caster, damage, interrupt);                   
				}
			}

			FinishSequence();
		}
	}
}