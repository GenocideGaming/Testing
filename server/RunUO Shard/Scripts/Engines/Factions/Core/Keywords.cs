using System;
using System.Collections;
using Server;
using Server.Network;
using Server.Factions;
using Server.Mobiles;

namespace Server.Factions
{
	public class Keywords
	{
		public static void Initialize()
		{
			EventSink.Speech += new SpeechEventHandler( EventSink_Speech );
		}

		private static void ShowScore_Sandbox( object state )
		{
			PlayerState pl = (PlayerState)state;

			if ( pl != null )
				pl.Mobile.PublicOverheadMessage( MessageType.Regular, pl.Mobile.SpeechHue, true, pl.KillPoints.ToString( "N0" ) ); // NOTE: Added 'N0'
		}

		private static void EventSink_Speech( SpeechEventArgs e )
		{
			Mobile from = e.Mobile;
			int[] keywords = e.Keywords;

            if (e.Mobile is PlayerMobile && e.Speech.ToLower().StartsWith("i wish to resign from the faction"))
            {
                PlayerMobile pm = e.Mobile as PlayerMobile;
                if (pm.FactionPlayerState != null && pm.FactionPlayerState.Faction != null)
                {
                    pm.SendGump(new LeaveFactionGump(pm, pm.FactionPlayerState.Faction));
                }
            }
			for ( int i = 0; i < keywords.Length; ++i )
			{
				switch ( keywords[i] )
				{
					case 0x00E4: // *i wish to access the city treasury*
					{
						Town town = Town.FromRegion( from.Region );

						if ( FactionGump.Exists( from ) )
							from.SendLocalizedMessage( 1042160 ); // You already have a faction menu open.


						break;
					}
					case 0x0ED: // *i am sheriff*
					{
						break;
					}
					case 0x00EF: // *you are fired*
					{
						Town town = Town.FromRegion( from.Region );

						if ( town == null )
							break;

						break;
					}
					case 0x00E5: // *i wish to resign as finance minister*
					{
						break;
					}
					case 0x00EE: // *i wish to resign as sheriff*
					{
						break;
					}
					case 0x00E9: // *what is my faction term status*
					{
						PlayerState pl = PlayerState.Find( from );

						if ( pl != null && pl.IsLeaving )
						{
							if ( Faction.CheckLeaveTimer( from ) )
								break;

							TimeSpan remaining = ( pl.Leaving + Faction.LeavePeriod ) - DateTime.Now;

							if( remaining.TotalDays >= 1 )
								from.SendLocalizedMessage( 1042743, remaining.TotalDays.ToString( "N0" ) ) ;// Your term of service will come to an end in ~1_DAYS~ days.
							else if( remaining.TotalHours >= 1 )
								from.SendLocalizedMessage( 1042741, remaining.TotalHours.ToString( "N0" ) ); // Your term of service will come to an end in ~1_HOURS~ hours.
							else
								from.SendLocalizedMessage( 1042742 ); // Your term of service will come to an end in less than one hour.
						}
						else if ( pl != null )
						{
							from.SendLocalizedMessage( 1042233 ); // You are not in the process of quitting the faction.
						}

						break;
					}
					case 0x00EA: // *message faction*
					{
						Faction faction = Faction.Find( from );

						if ( faction == null || !faction.IsCommander( from ) )
							break;

						if ( from.AccessLevel == AccessLevel.Player && !faction.FactionMessageReady )
							from.SendLocalizedMessage( 1010264 ); // The required time has not yet passed since the last message was sent
						else
							faction.BeginBroadcast( from );

						break;
					}
					case 0x00EC: // *showscore*
					{
						PlayerState pl = PlayerState.Find( from );

						if ( pl != null )
							Timer.DelayCall( TimeSpan.Zero, new TimerStateCallback( ShowScore_Sandbox ), pl );

						break;
					}
					case 0x0178: // i honor your leadership
					{
						Faction faction = Faction.Find( from );

						if ( faction != null )
							faction.BeginHonorLeadership( from );

						break;
					}
				}
			}
		}
	}
}