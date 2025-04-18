using System;
using System.Collections;
using Server.Targeting;
using Server.Network;
using Server.Scripts;
using Server.Mobiles;

namespace Server.Spells.First
{
	public class ReactiveArmorSpell : MagerySpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Reactive Armor", "Flam Sanct",
				236,
				9011,
				Reagent.Garlic,
				Reagent.SpidersSilk,
				Reagent.SulfurousAsh
			);

		public override SpellCircle Circle { get { return SpellCircle.First; } }

		public ReactiveArmorSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

		public override bool CheckCast()
		{
			return true;
		}

		private static Hashtable m_Table = new Hashtable();

		public override void OnCast()
		{
            if (Caster is PlayerMobile && ReactiveArmorOnCooldown(Caster) && FeatureList.ScribeMage.Enabled)
            {
                Caster.SendMessage(FeatureList.ScribeMage.ArmorCooldownText);
            }

			else if ( CheckSequence() )
			{
                if (FeatureList.ScribeMage.Enabled)
                {
                    SpellHelper.NullifyDefensiveSpells(Caster);

                    int minBonus = FeatureList.ScribeMage.MinimumArmorBoost;
                    int maxBonus = FeatureList.ScribeMage.MaximumArmorBoost;

                    double skillValue = (Caster.Skills[SkillName.Inscribe].Value + 
                                         Caster.Skills[SkillName.Magery].Value) / 2;
                    double skillBonus = (skillValue / 100) * (double)(maxBonus - minBonus);
                    int totalBonus = (int)(minBonus + skillBonus);

                    Caster.MeleeDamageAbsorb = totalBonus;

                    Caster.FixedParticles(0x376A, 9, 32, 5008, EffectLayer.Waist);
                    Caster.PlaySound(0x1F2);

                    if (Caster is PlayerMobile)
                    {
                        TimeSpan delay = TimeSpan.FromSeconds(FeatureList.ScribeMage.ArmorReuseDelay);
                        ((PlayerMobile)Caster).NextReactiveArmorAt = DateTime.Now + delay;
                    }
                }

                else
                {
                    CastOldReactiveArmor();
                }
			}

			FinishSequence();
		}

        private bool ReactiveArmorOnCooldown(Mobile Caster)
        {
            PlayerMobile subject = Caster as PlayerMobile;
            if (subject == null) { return false; }

            if (subject.NextReactiveArmorAt > DateTime.Now)
                return true;
            else
                return false;
        }

        private void CastOldReactiveArmor()
        {
            int value = (int)(Caster.Skills[SkillName.Magery].Value + Caster.Skills[SkillName.Meditation].Value + Caster.Skills[SkillName.EvalInt].Value);
            value /= 3;

            if (value < 0)
                value = 1;
            else if (value > 75)
                value = 75;

            Caster.MeleeDamageAbsorb = value;

            Caster.FixedParticles(0x376A, 9, 32, 5008, EffectLayer.Waist);
            Caster.PlaySound(0x1F2);
        }

		public static void EndArmor( Mobile m )
		{
			if ( m_Table.Contains( m ) )
			{
				ResistanceMod[] mods = (ResistanceMod[]) m_Table[ m ];

				if ( mods != null )
				{
					for ( int i = 0; i < mods.Length; ++i )
						m.RemoveResistanceMod( mods[ i ] );
				}

				m_Table.Remove( m );
				BuffInfo.RemoveBuff( m, BuffIcon.ReactiveArmor );
			}
		}
	}
}