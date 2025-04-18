using System;
using Server;
using Server.Gumps;
using Server.Guilds;
using Server.Mobiles;
using Server.Network;

namespace Server.Factions
{
	public class LeaveFactionGump : FactionGump
	{
		private PlayerMobile m_From;
		private Faction m_Faction;

		public LeaveFactionGump( PlayerMobile from, Faction faction ) : base( 20, 30 )
		{
			m_From = from;
			m_Faction = faction;

			AddBackground( 0, 0, 270, 120, 5054 );
			AddBackground( 10, 10, 250, 100, 3000 );

			if ( from.Guild is Guild && ((Guild)from.Guild).Leader == from )
				AddHtmlLocalized( 20, 15, 230, 60, 1018057, true, true ); // Are you sure you want your entire guild to leave this faction?
			else
				AddHtmlLocalized( 20, 15, 230, 60, 1018063, true, true ); // Are you sure you want to leave this faction?

			AddHtmlLocalized( 55, 80, 75, 20, 1011011, false, false ); // CONTINUE
			AddButton( 20, 80, 4005, 4007, 1, GumpButtonType.Reply, 0 );

			AddHtmlLocalized( 170, 80, 75, 20, 1011012, false, false ); // CANCEL
			AddButton( 135, 80, 4005, 4007, 2, GumpButtonType.Reply, 0 );
		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			switch ( info.ButtonID )
			{
				case 1: // continue
				{
                    PlayerState pl = PlayerState.Find(m_From);
                    
                    if (pl != null)
                    {
                        // if they are already in the queue to leave, let them know how long they have left
                        if (pl != null && pl.IsLeaving) // copied from Server.Factions.Keywords
                        {
                            if (Faction.CheckLeaveTimer(m_From))
                                break;

                            TimeSpan remaining = (pl.Leaving + Faction.LeavePeriod) - DateTime.Now;

                            if (remaining.TotalDays >= 1)
                                m_From.SendLocalizedMessage(1042743, remaining.TotalDays.ToString("N0"));// Your term of service will come to an end in ~1_DAYS~ days.
                            else if (remaining.TotalHours >= 1)
                                m_From.SendLocalizedMessage(1042741, remaining.TotalHours.ToString("N0")); // Your term of service will come to an end in ~1_HOURS~ hours.
                            else
                                m_From.SendLocalizedMessage(1042742); // Your term of service will come to an end in less than one hour.
                        } else {
                            pl.Leaving = DateTime.Now;

                            if (Faction.LeavePeriod == TimeSpan.FromDays(3.0))
                                m_From.SendLocalizedMessage(1005065); // You will be removed from the faction in 3 days
                            else
                                m_From.SendMessage("You will be removed from the faction in {0} days.", Faction.LeavePeriod.TotalDays);
                        }
                    }

					break;
				}
				case 2: // cancel
				{
					m_From.SendLocalizedMessage( 500737 ); // Canceled resignation.
					break;
				}
			}
		}
	}
}