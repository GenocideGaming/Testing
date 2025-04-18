using System;
using Server.Network;
using Server.Gumps;

namespace Server.Menus.Questions
{
	public class StuckMenuEntry
	{
		private int m_Name;
		private Point3D[] m_Locations;

		public int Name{ get{ return m_Name; } }
		public Point3D[] Locations{ get{ return m_Locations; } }

		public StuckMenuEntry( int name, Point3D[] locations )
		{
			m_Name = name;
			m_Locations = locations;
		}
	}

	public class StuckMenu : Gump
	{
		private static StuckMenuEntry[] m_Entries = new StuckMenuEntry[]
			{
                //Galven
				new StuckMenuEntry(1063524, new Point3D[]{ new Point3D(1317, 1381, 0) }),
                //Roache
                new StuckMenuEntry(1063530, new Point3D[]{ new Point3D(1393, 1946, 0) })
			};

		private static bool IsInSecondAgeArea( Mobile m )
		{
			if ( m.Map != Map.Trammel && m.Map != Map.Felucca )
				return false;

			if ( m.X >= 5120 && m.Y >= 2304 )
				return true;

			if ( m.Region.IsPartOf( "Terathan Keep" ) )
				return true;

			return false;
		}

		private Mobile m_Mobile, m_Sender;
		private bool m_MarkUse;

		private Timer m_Timer;

		public StuckMenu( Mobile beholder, Mobile beheld, bool markUse ) : base( 150, 50 )
		{
			m_Sender = beholder;
			m_Mobile = beheld;
			m_MarkUse = markUse;

			Closable = false; 
			Dragable = false; 
			Disposable = false;

			AddBackground( 0, 0, 270, 320, 2600 );

			AddHtmlLocalized( 50, 20, 250, 35, 1011027, false, false ); // Chose a town:

			StuckMenuEntry[] entries = m_Entries;

			for ( int i = 0; i < entries.Length; i++ )
			{
				StuckMenuEntry entry = entries[i];

				AddButton( 50, 55 + 35 * i, 208, 209, i + 1, GumpButtonType.Reply, 0 );
				AddHtmlLocalized( 75, 55 + 35 * i, 335, 40, entry.Name, false, false );
			}

			AddButton( 55, 263, 4005, 4007, 0, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 90, 265, 200, 35, 1011012, false, false ); // CANCEL
		}

		public void BeginClose()
		{
			StopClose();

			m_Timer = new CloseTimer( m_Mobile );
			m_Timer.Start();

			m_Mobile.Frozen = true;
		}

		public void StopClose()
		{
			if ( m_Timer != null )
				m_Timer.Stop();

			m_Mobile.Frozen = false;
		}

		public override void OnResponse( NetState state, RelayInfo info )
		{
			StopClose();

            if ( info.ButtonID == 0 )
			{
				if ( m_Mobile == m_Sender )
					m_Mobile.SendLocalizedMessage( 1010588 ); // You choose not to go to any city.
			}
			else
			{
				int index = info.ButtonID - 1;
				StuckMenuEntry[] entries = m_Entries;

				if ( index >= 0 && index < entries.Length )
					Teleport( entries[index] );
				
				m_Mobile.RevealingAction();
			}
		}

		private void Teleport( StuckMenuEntry entry )
		{
			if ( m_MarkUse ) 
			{
				m_Mobile.SendLocalizedMessage( 1010589 ); // You will be teleported within the next two minutes.

				new TeleportTimer( m_Mobile, entry, TimeSpan.FromSeconds( 10.0 + (Utility.RandomDouble() * 110.0) ) ).Start();

				m_Mobile.UsedStuckMenu();
			}
			else
			{
				new TeleportTimer( m_Mobile, entry, TimeSpan.Zero ).Start();
			}
		}

		private class CloseTimer : Timer
		{
			private Mobile m_Mobile;
			private DateTime m_End;

			public CloseTimer( Mobile m ) : base( TimeSpan.Zero, TimeSpan.FromSeconds( 1.0 ) )
			{
				m_Mobile = m;
				m_End = DateTime.Now + TimeSpan.FromMinutes( 3.0 );
			}

			protected override void OnTick()
			{
				if ( m_Mobile.NetState == null || DateTime.Now > m_End )
				{
					m_Mobile.Frozen = false;
					m_Mobile.CloseGump( typeof( StuckMenu ) );

					Stop();
				}
				else
				{
					m_Mobile.Frozen = true;
				}
			} 
		} 

		private class TeleportTimer : Timer
		{
			private Mobile m_Mobile;
			private StuckMenuEntry m_Destination;
			private DateTime m_End;

			public TeleportTimer( Mobile mobile, StuckMenuEntry destination, TimeSpan delay ) : base( TimeSpan.Zero, TimeSpan.FromSeconds( 1.0 ) )
			{
				Priority = TimerPriority.TwoFiftyMS;

				m_Mobile = mobile;
				m_Destination = destination;
				m_End = DateTime.Now + delay;
			}

			protected override void OnTick()
			{
				if ( DateTime.Now < m_End )
				{
					m_Mobile.Frozen = true;
				}
				else
				{
					m_Mobile.Frozen = false;
					Stop();

					int idx = Utility.Random( m_Destination.Locations.Length );
					Point3D dest = m_Destination.Locations[idx];

					Map destMap = Map.Felucca;

					Mobiles.BaseCreature.TeleportPets( m_Mobile, dest, destMap );
					m_Mobile.MoveToWorld( dest, destMap );
				}
			}
		}
	}
}