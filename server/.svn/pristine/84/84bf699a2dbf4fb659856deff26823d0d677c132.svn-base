using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Gumps;

namespace Server.Scripts.Custom.AntiRail
{
    static class ResponseTimerRegistry
    {
        static Dictionary<Serial, ResponseTimer> m_Timers = new Dictionary<Serial, ResponseTimer>();

        public static void AddTimer(Mobile player)
        {
            RemoveTimer(player.Serial);

            m_Timers.Add(player.Serial, new ResponseTimer(player));
            m_Timers[player.Serial].Start();
        }

        public static void RemoveTimer(Mobile player)
        {
            RemoveTimer(player.Serial);
        }

        public static void RemoveTimer(Serial serial)
        {
            if (m_Timers.ContainsKey(serial))
            {
                if (m_Timers[serial].Running)
                    m_Timers[serial].Stop();

                m_Timers.Remove(serial);
            }
        }

    }

        public class ResponseTimer : Timer
		{
			private Mobile m_Mobile;
			private static TimeSpan delay = TimeSpan.FromMinutes(3.0);
				
			public ResponseTimer( Mobile m ) : base( delay ) 
			{
				m_Mobile = m;
			}
	
			protected override void OnTick()
			{
                PlayerMobile pm = m_Mobile as PlayerMobile;
                if (pm == null) { return; }

                if (pm.NetState == null || !pm.NetState.CheckAlive())
                    return;

				Point3D jailedAt;
            	switch (Utility.Random(4))
            	{
	                case 0: jailedAt = new Point3D(5333, 1708, 0); break;
	                case 1: jailedAt = new Point3D(5349, 1708, 0); break;
	                case 2: jailedAt = new Point3D(5333, 1727, 0); break;
	                default: jailedAt = new Point3D(5349, 1727, 0); break;
            	}

            	m_Mobile.Location = jailedAt;

                if (pm.HasGump(typeof(AntiRailGump)))
                    pm.CloseGump(typeof(AntiRailGump));
            	
				m_Mobile.SendMessage("You have been placed in jail for not responding to the anti-macro test.");            	
			}
		}
}
