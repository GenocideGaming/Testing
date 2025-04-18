using System;
using Server.Targeting;
using Server.Network;
using Server.Regions;
using Server.Items;
using Server.Mobiles;
using Server.Scripts;

namespace Server.Spells.Third
{
	public class TeleportSpell : MagerySpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Teleport", "Rel Por",
				215,
				9031,
				Reagent.Bloodmoss,
				Reagent.MandrakeRoot
			);

		public override SpellCircle Circle { get { return SpellCircle.Third; } }

		public TeleportSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

		public override bool CheckCast()
		{
			if ( Server.Misc.WeightOverloading.IsOverloaded( Caster ) )
			{
				Caster.SendLocalizedMessage( 502359, "", 0x22 ); // Thou art too encumbered to move.
				return false;
			}

            if (!SpellHelper.CheckTravel(Caster, TravelCheckType.TeleportFrom))
                return false;

            BaseCreature bc = Caster as BaseCreature;
            if (bc != null && bc.NetState != null && bc.Pseu_TeleWallDelay && bc.NextTeleportAt > DateTime.Now) // pseudoseer
            {
                bc.SendMessage("You must wait before casting that spell again.");
                return false;
            }

            PlayerMobile pm = Caster as PlayerMobile;
            if (pm == null)
                return true;

            if (pm.NextTeleportAt > DateTime.Now)
            {
                pm.SendMessage("You must wait before casting that spell again.");
                return false;
            }

            return true;
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

		public void Target( IPoint3D p )
		{
			IPoint3D orig = p;
			Map map = Caster.Map;

			SpellHelper.GetSurfaceTop( ref p );

            if ( Server.Misc.WeightOverloading.IsOverloaded( Caster ) )
			{
				Caster.SendLocalizedMessage( 502359, "", 0x22 ); // Thou art too encumbered to move.
			}
			else if ( !SpellHelper.CheckTravel( Caster, TravelCheckType.TeleportFrom ) )
			{
			}
			else if ( !SpellHelper.CheckTravel( Caster, map, new Point3D( p ), TravelCheckType.TeleportTo ) )
			{
			}
			else if ( map == null || !map.CanSpawnMobile( p.X, p.Y, p.Z ) )
			{
				Caster.SendLocalizedMessage( 501942 ); // That location is blocked.
			}
			else if ( SpellHelper.CheckMulti( new Point3D( p ), map ) )
			{
				Caster.SendLocalizedMessage( 501942 ); // That location is blocked.
			}
            else if (CheckSequence())
            {
                SpellHelper.Turn(Caster, orig);

                Mobile m = Caster;

                Point3D from = m.Location;
                Point3D to = new Point3D(p);

                m.Location = to;
                m.ProcessDelta();

                BaseCreature bc = Caster as BaseCreature;
                PlayerMobile pm = Caster as PlayerMobile;
                if (bc != null)
                    bc.NextTeleportAt = DateTime.Now + TimeSpan.FromSeconds(FeatureList.SpellChanges.TeleportRecastDelay);
            	if (pm != null)
                	pm.NextTeleportAt = DateTime.Now + TimeSpan.FromSeconds(FeatureList.SpellChanges.TeleportRecastDelay);

                if (m.Player)
                {
                    Effects.SendLocationParticles(EffectItem.Create(from, m.Map, EffectItem.DefaultDuration), 0x3728, 10, 10, 2023);
                    Effects.SendLocationParticles(EffectItem.Create(to, m.Map, EffectItem.DefaultDuration), 0x3728, 10, 10, 5023);
                }
                else
                {
                    m.FixedParticles(0x376A, 9, 32, 0x13AF, EffectLayer.Waist);
                }

                m.PlaySound(0x1FE);

                IPooledEnumerable eable = m.GetItemsInRange(0);

                foreach (Item item in eable)
                {
                    if (item is Server.Spells.Sixth.ParalyzeFieldSpell.InternalItem || item is Server.Spells.Fifth.PoisonFieldSpell.InternalItem || item is Server.Spells.Fourth.FireFieldSpell.FireFieldItem)
                        item.OnMoveOver(m);
                }

                eable.Free();
            }

			FinishSequence();
		}

		public class InternalTarget : Target
		{
			private TeleportSpell m_Owner;

			public InternalTarget( TeleportSpell owner ) : base( FeatureList.SpellChanges.TeleportRange, true, TargetFlags.None )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				IPoint3D p = o as IPoint3D;

				if ( p != null )
					m_Owner.Target( p );
			}

			protected override void OnTargetFinish( Mobile from )
			{
				m_Owner.FinishSequence();
			}
		}
	}
}