using System;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Scripts;

namespace Server.Items
{
	public class OrangePetals : Item
	{
		public override int LabelNumber { get { return 1053122; } } // orange petals

		public override double DefaultWeight
		{
			get { return 0.1; }
		}

		[Constructable]
		public OrangePetals() : this( 1 )
		{
		}

		[Constructable]
		public OrangePetals( int amount ) : base( 0x1021 )
		{
			Stackable = true;
			Hue = 0x2B;
			Amount = amount;
		}

		public OrangePetals( Serial serial ) : base( serial )
		{
		}

		public override bool CheckItemUse( Mobile from, Item item ) 
		{ 
			if ( item != this )
				return base.CheckItemUse( from, item );

			if ( from != this.RootParent )
			{
				from.SendLocalizedMessage( 1042038 ); // You must have the object in your backpack to use it.
				return false;
			}

			return base.CheckItemUse( from, item );
		}

		public override void OnDoubleClick( Mobile from )
		{
            OrangePetalsContext context = GetContext( from );

            if ( context != null )
            {
                from.LocalOverheadMessage( MessageType.Regular, 0x3B2, 1061904 );
                return;
            }

            from.LocalOverheadMessage( MessageType.Regular, 0x3B2, 1061905 );
            Timer timer = new OrangePetalsTimer( from );
            timer.Start();
            
            AddContext( from, new OrangePetalsContext( timer ) );
            this.Consume();

            /* Greater cure functionality
            if (from.Poisoned)
            {
                from.PlaySound(0x3B);

                // Rel Por cusomization: treat just like a greater cure potion
                bool cure = false;

                double cureEffectiveness = 0.0;
                double cureDifficulty = 0.0;

                //Potion Effectivness

                cureEffectiveness = FeatureList.Poison.GreaterCurePotionEffectivness;


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

                this.Consume();
            }
            else
			{
				from.SendLocalizedMessage( 1042000 ); // You are not poisoned.
			}*/
		}

		private static Hashtable m_Table = new Hashtable();

		private static void AddContext( Mobile m, OrangePetalsContext context )
		{
			m_Table[m] = context;
		}

		public static void RemoveContext( Mobile m )
		{
			OrangePetalsContext context = GetContext( m );

			if ( context != null )
				RemoveContext( m, context );
		}

		private static void RemoveContext( Mobile m, OrangePetalsContext context )
		{
			m_Table.Remove( m );

			context.Timer.Stop();
		}

		private static OrangePetalsContext GetContext( Mobile m )
		{
			return ( m_Table[m] as OrangePetalsContext );
		}

		public static bool UnderEffect( Mobile m )
		{
			return ( GetContext( m ) != null );
		}

		private class OrangePetalsTimer : Timer
		{
			private Mobile m_Mobile;

			public OrangePetalsTimer( Mobile from ) : base ( TimeSpan.FromMinutes( 5.0 ) )
			{
				m_Mobile = from;
			}

			protected override void OnTick()
			{
				if ( !m_Mobile.Deleted )
				{
					m_Mobile.LocalOverheadMessage( MessageType.Regular, 0x3F, true,
						"* You feel the effects of your poison resistance wearing off *" );
				}

				RemoveContext( m_Mobile );
			}
		}

		private class OrangePetalsContext
		{
			private Timer m_Timer;

			public Timer Timer{ get{ return m_Timer; } }

			public OrangePetalsContext( Timer timer )
			{
				m_Timer = timer;
			}
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
	}
}