using System;
using Server;
using Server.Spells;
using Server.Scripts;
using Server.Items;

namespace Server.Items
{
	public class CureLevelInfo
	{
		private Poison m_Poison;
		private double m_Chance;

		public Poison Poison
		{
			get{ return m_Poison; }
		}

		public double Chance
		{
			get{ return m_Chance; }
		}

		public CureLevelInfo( Poison poison, double chance )
		{
			m_Poison = poison;
			m_Chance = chance;
		}
	}

	public abstract class BaseCurePotion : BasePotion
	{
		public abstract CureLevelInfo[] LevelInfo{ get; }

		public BaseCurePotion( PotionEffect effect ) : base( 0xF07, effect )
		{
		}

		public BaseCurePotion( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}

		public void DoCure( Mobile from )
		{
            bool cure = false;

            double cureEffectiveness = 0.0;
            double cureDifficulty = 0.0;
                      
            //Potion Effectivness
            PotionEffect potionEffect = (PotionEffect)base.PotionEffect;

            if (potionEffect == PotionEffect.CureLesser)
            {
                cureEffectiveness = FeatureList.Poison.LesserCurePotionEffectivness;
            }

            else if (potionEffect == PotionEffect.Cure)
            {
                cureEffectiveness = FeatureList.Poison.CurePotionEffectivness;
            }

            else if (potionEffect == PotionEffect.CureGreater)
            {
                cureEffectiveness = FeatureList.Poison.GreaterCurePotionEffectivness;
            }

            //Poison Strength
            Poison poison = from.Poison as Poison;

            if (poison == Poison.Lesser)
            {
                cureDifficulty = FeatureList.Poison.LesserPoisonCureChance;
            }

            else if (poison == Poison.Regular)
            {
                cureDifficulty = FeatureList.Poison.RegularPoisonCureChance;
            }

            else if (poison == Poison.Greater)
            {
                cureDifficulty = FeatureList.Poison.GreaterPoisonCureChance;
            }

            else if (poison == Poison.Deadly)
            {
                cureDifficulty = FeatureList.Poison.DeadlyPoisonCureChance;
            }

            else if (poison == Poison.Lethal)
            {
                cureDifficulty = FeatureList.Poison.LethalPoisonCureChance;
            }        
   
            //Determine Success
            int cureChance = (int)(cureEffectiveness * cureDifficulty);

            if (cureChance > Utility.Random(100))
            {
                cure = true;
            }

            //Resolution
            if (cure && from.CurePoison(from))
            {
                from.SendLocalizedMessage(500231); // You feel cured of poison!

                from.FixedEffect(0x373A, 10, 15);
                from.PlaySound(0x1E0);
            }

            else if (!cure)
            {
                from.SendLocalizedMessage(500232); // That potion was not strong enough to cure your ailment!
            }           
		}

		public override void Drink( Mobile from )
		{
			
			if ( from.Poisoned )
			{
				DoCure( from );

				BasePotion.PlayDrinkEffect( from );

				from.FixedParticles( 0x373A, 10, 15, 5012, EffectLayer.Waist );
				from.PlaySound( 0x1E0 );

				this.Consume();
			}
			else
			{
				from.SendLocalizedMessage( 1042000 ); // You are not poisoned.
			}
		}
	}
}