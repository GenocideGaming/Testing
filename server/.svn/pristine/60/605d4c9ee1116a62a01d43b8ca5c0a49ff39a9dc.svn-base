using System;
using Server.Misc;
using Server.Items;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;
using Server.Spells.Fifth;
using Server.Spells.Seventh;

namespace Server.Spells.Sixth
{
	public class DispelSpell : MagerySpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Dispel", "An Ort",
				218,
				9002,
				Reagent.Garlic,
				Reagent.MandrakeRoot,
				Reagent.SulfurousAsh
			);

		public override SpellCircle Circle { get { return SpellCircle.Sixth; } }

		public DispelSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
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

        public void Target(Mobile m)
        {
            //Player
            if (m is PlayerMobile)
            {
                if (!Caster.CanSee(m) || m.Hidden)
                {
                    Caster.SendLocalizedMessage(500237); // Target can not be seen.
                }

                PlayerMobile pm = (PlayerMobile)m;

                if (CheckHSequence(pm))
                {
                    SpellHelper.Turn(Caster, pm);

                    if (pm.MagicDamageAbsorb > 0)
                    {
                        pm.MagicDamageAbsorb = 0;
                        BuffInfo.RemoveBuff(pm, BuffIcon.MagicReflection);

                        Caster.DoHarmful(pm);

                        Effects.SendLocationParticles(EffectItem.Create(pm.Location, pm.Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5015);
                        Effects.PlaySound(pm.Location, pm.Map, 0x22C);
                    }

                    if (pm.IsBodyMod || pm.NameMod != null)
                    {
                        pm.BodyMod = 0;
                        pm.HueMod = -1;
                        pm.NameMod = null;
                        pm.SetHairMods(-1, -1);

                        BaseArmor.ValidateMobile(pm);
                        BaseClothing.ValidateMobile(pm);

                        pm.EndAction(typeof(PolymorphSpell));
                        pm.EndAction(typeof(IncognitoSpell));

                        Caster.DoHarmful(pm);
                        Effects.SendLocationParticles(EffectItem.Create(pm.Location, pm.Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5015);
                        Effects.PlaySound(pm.Location, pm.Map, 0x22C);
                    }
                }
            }

            //Creature
            else if (m is Mobile)
            {                
                BaseCreature bc = m as BaseCreature;

                //If Null
                if (bc == null)
                    return;

                //If Can't See
                if (!Caster.CanSee(bc))
                {
                    Caster.SendLocalizedMessage(500237); // Target can not be seen.
                }

                else if (CheckHSequence(bc))
                {
                    SpellHelper.Turn(Caster, bc);

                    Effects.SendLocationParticles(EffectItem.Create(bc.Location, bc.Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5015);
                    Effects.PlaySound(bc.Location, bc.Map, 0x22C);

                    //Remove Magic Reflect If Up
                    if (bc.MagicDamageAbsorb > 0)
                    {
                        bc.MagicDamageAbsorb = 0;
                        BuffInfo.RemoveBuff(bc, BuffIcon.MagicReflection);

                        Caster.DoHarmful(bc);                        
                    }

                    //If Not Dispellable
                    if (!bc.IsDispellable)
                    {                        
                        Caster.SendLocalizedMessage(1005049); // That cannot be dispelled.

                        return;
                    }

                    double creatureValue = GetMobileDifficulty.GetDifficultyValue(bc);
                    double dispelSkill = Caster.Skills[SkillName.Magery].Value;
                    double resistBonus = (bc.Skills[SkillName.MagicResist].Value / 10) / 100;

                    double chance = .50 + ((dispelSkill - creatureValue) / 100) - resistBonus;
                   
                    //Successful Dispel
                    if (chance >= Utility.RandomDouble())
                    {
                        bc.Delete();
                        Caster.Combatant = null;
                    }

                    //Failed Dispel
                    else
                    {
                        double damage = dispelSkill / 2;

                        SpellHelper.Damage(this, bc, damage, false);
                    }                                      
                }

                FinishSequence();
            }
        }

		public class InternalTarget : Target
		{
			private DispelSpell m_Owner;

			public InternalTarget( DispelSpell owner ) : base( 12, false, TargetFlags.Harmful )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{                
				if ( o is PlayerMobile )
				{
					if ( !from.CanSee( o ) )
					{
						from.SendLocalizedMessage( 500237 ); // Target can not be seen.
					}
				
					PlayerMobile target = (PlayerMobile)o;
					
					if ( m_Owner.CheckHSequence(target) )
					{
						SpellHelper.Turn( from, target );
						
						if (target.MagicDamageAbsorb > 0)
						{
							target.MagicDamageAbsorb = 0;
							BuffInfo.RemoveBuff( target, BuffIcon.MagicReflection );
							
							from.DoHarmful(target);
							Effects.SendLocationParticles( EffectItem.Create( target.Location, target.Map, EffectItem.DefaultDuration ), 0x376A, 9, 32, 5015 );
							Effects.PlaySound( target.Location, target.Map, 0x22C );
						}
						
						if (target.IsBodyMod || target.NameMod != null)
						{
							target.BodyMod = 0;
							target.HueMod = -1;
							target.NameMod = null;
							target.SetHairMods( -1, -1 );

							BaseArmor.ValidateMobile( target );
							BaseClothing.ValidateMobile( target );
							
							target.EndAction( typeof( PolymorphSpell ) );
							target.EndAction( typeof( IncognitoSpell ) );
							
							from.DoHarmful(target);
							Effects.SendLocationParticles( EffectItem.Create( target.Location, target.Map, EffectItem.DefaultDuration ), 0x376A, 9, 32, 5015 );
							Effects.PlaySound( target.Location, target.Map, 0x22C );
						}
					}
				}

				else if ( o is Mobile )
				{
					Mobile m = (Mobile)o;
					BaseCreature bc = m as BaseCreature;

                    if (!from.CanSee(m) && !m.Hidden)
					{
						from.SendLocalizedMessage( 500237 ); // Target can not be seen.
					}

					else if ( bc == null || !bc.IsDispellable )
					{
						from.SendLocalizedMessage( 1005049 ); // That cannot be dispelled.
					}

					else if ( m_Owner.CheckHSequence( m ) )
					{
						SpellHelper.Turn( from, m );

						double dispelChance = (50.0 + ((100 * (from.Skills.Magery.Value - bc.DispelDifficulty)) / (bc.DispelFocus*2))) / 100;

						Effects.SendLocationParticles( EffectItem.Create( m.Location, m.Map, EffectItem.DefaultDuration ), 0x3728, 8, 20, 5042 );
						Effects.PlaySound( m, m.Map, 0x201 );

						m.Delete();
					}
				}                
			}

			protected override void OnTargetFinish( Mobile from )
			{
				m_Owner.FinishSequence();
			}
		}
	}
}