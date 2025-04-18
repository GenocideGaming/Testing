using System;
using System.Collections;
using Server;
using Server.Scripts;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;

namespace Server.Spells.Fifth
{
	public class MagicReflectSpell : MagerySpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Magic Reflection", "In Jux Sanct",
				242,
				9012,
				Reagent.Garlic,
				Reagent.MandrakeRoot,
				Reagent.SpidersSilk
			);

		public override SpellCircle Circle { get { return SpellCircle.Fifth; } }

		public MagicReflectSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

		public override bool CheckCast()
		{
			return true;
		}

		private static Hashtable m_Table = new Hashtable();

		public override void OnCast()
		{
            if (Caster is PlayerMobile && ReflectOnCooldown(Caster) && FeatureList.ScribeMage.Enabled)
            {
                Caster.SendMessage(FeatureList.ScribeMage.ReflectCooldownText);
            }

			else if ( CheckSequence() )
			{
                int minCircles = FeatureList.ScribeMage.MinimumCirclesToReflect;
                int maxCircles = FeatureList.ScribeMage.MaximumCirclesToReflect;

                if (FeatureList.ScribeMage.Enabled)
                {
                    SpellHelper.NullifyDefensiveSpells(Caster);

                    double skillValue = (Caster.Skills[SkillName.Magery].Value + 
                                         Caster.Skills[SkillName.Inscribe].Value) / 2;
                    double skillBonus = (skillValue / 100) * (double)(maxCircles - minCircles);

                    int totalBonus = (int)(minCircles + skillBonus);

                    Caster.MagicDamageAbsorb = totalBonus;

                    Caster.FixedParticles(0x375A, 10, 15, 5037, EffectLayer.Waist);
                    Caster.PlaySound(0x1E9);

                    if (Caster is PlayerMobile)
                    {
                        TimeSpan delay = TimeSpan.FromSeconds(FeatureList.ScribeMage.ReflectReuseDelay);
                        ((PlayerMobile)Caster).NextReflectAt = DateTime.Now + delay;
                    }
                }

                else
                {
                    CastUnmodifiedReflectSpell();
                }
			}
			FinishSequence();
		}

        private bool ReflectOnCooldown(Mobile Caster)
        {   
            PlayerMobile subject = Caster as PlayerMobile;
            if (subject == null) { return false; }

            if (subject.NextReflectAt > DateTime.Now)
                return true;
            else
                return false;
        }

        private void CastUnmodifiedReflectSpell()
        {
            int value = (int)(Caster.Skills[SkillName.Magery].Value + Caster.Skills[SkillName.EvalInt].Value);
            value = (int)(8 + (value / 200) * 7.0);//absorb from 8 to 15 "circles"

            Caster.MagicDamageAbsorb = value;

            Caster.FixedParticles(0x375A, 10, 15, 5037, EffectLayer.Waist);
            Caster.PlaySound(0x1E9);
        }

		public static void EndReflect( Mobile m )
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
				BuffInfo.RemoveBuff( m, BuffIcon.MagicReflection );
			}
		}
	}
}
