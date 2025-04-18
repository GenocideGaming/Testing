using System;
using Server;
using Server.Mobiles;
using Server.Scripts;

namespace Server.Misc
{
	public enum DFAlgorithm
	{
		Standard,
		PainSpike
	}

	public class WeightOverloading
	{
		public static void Initialize()
		{
			EventSink.Movement += new MovementEventHandler( EventSink_Movement );
		}

		private static DFAlgorithm m_DFA;

		public static DFAlgorithm DFA
		{
			get{ return m_DFA; }
			set{ m_DFA = value; }
		}	

		public const int OverloadAllowance = 4; // We can be four stones overweight without getting fatigued

		public static int GetMaxWeight( Mobile m )
		{
			//return ((( Core.ML && m.Race == Race.Human) ? 100 : 40 ) + (int)(3.5 * m.Str));
			//Moved to core virtual method for use there

			return m.MaxWeight;
		}

		public static void EventSink_Movement( MovementEventArgs e )
		{
			Mobile from = e.Mobile;

			if ( !from.Alive || from.AccessLevel > AccessLevel.Player  )
				return;

			int maxWeight = GetMaxWeight( from ) + OverloadAllowance;
			int overWeight = (Mobile.BodyWeight + from.TotalWeight) - maxWeight;

			if ( overWeight > 0 )
			{
				from.Stam -= GetStamLoss( from, overWeight, (e.Direction & Direction.Running) != 0 );

				if ( from.Stam == 0 )
				{
					from.SendLocalizedMessage( 500109 ); // You are too fatigued to move, because you are carrying too much weight!
					e.Blocked = true;
					return;
				}
			}

            //Restrict Running when at 1 or less stamina
            if (((e.Direction & Direction.Running) != 0) && from.Stam < FeatureList.RegenRates.StaminaForceWalk)
            {  
                Direction newDirection = new Direction();

                switch (e.Direction)
                {
                    case Direction.North: 
                        newDirection = Direction.North;
                    break;
                    case Direction.Up:
                         newDirection = Direction.Up;
                        break;
                    case Direction.East:
                         newDirection = Direction.East;
                        break;
                    case Direction.Right: 
                         newDirection = Direction.Right;
                        break;
                    case Direction.South:
                         newDirection = Direction.South;
                        break;
                    case Direction.Down:
                         newDirection = Direction.Down;
                        break;
                    case Direction.West:
                         newDirection = Direction.West;
                        break;
                    case Direction.Left: 
                         newDirection = Direction.Left;
                   break;
                }

                from.SetDirection(newDirection);
               
                return;
            }
		}

		public static int GetStamLoss( Mobile from, int overWeight, bool running )
		{
			int loss = 5 + (overWeight / 25);

			if ( from.Mounted )
				loss /= 3;

			if ( running )
				loss *= 2;

			return loss;
		}

		public static bool IsOverloaded( Mobile m )
		{
			if ( !m.Player || !m.Alive || m.AccessLevel > AccessLevel.Player )
				return false;

			return ( (Mobile.BodyWeight + m.TotalWeight) > (GetMaxWeight( m ) + OverloadAllowance) );
		}
	}
}