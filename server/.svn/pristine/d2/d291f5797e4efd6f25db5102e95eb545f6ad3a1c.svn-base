using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Server.Regions
{
    class RoacheRegion : NoHousingRegion
    {
        public RoacheRegion(XmlElement element, Map map, Region parent) : base(element, map, parent) { }

        public override void OnEnter(Mobile m)
        {
        	m.SendMessage("All who enter Roache are considered criminals.");
        	RoacheCriminalTimer criminalTimer = new RoacheCriminalTimer(m);
        	criminalTimer.Start();
        	
            base.OnEnter(m);
        }

        public override void OnExit(Mobile m)
        {
        	m.SendMessage("You have left the city of Roache.");
            base.OnExit(m);
        }
        
        private class RoacheCriminalTimer : Timer
        {
            private Mobile player;

            public RoacheCriminalTimer(Mobile m) : base(TimeSpan.Zero, TimeSpan.FromSeconds(1.0))
            {
            	player = m;
            }

            protected override void OnTick()
            {
            	if ( player.Region.IsPartOf("Roache") )
                	player.Criminal = true;
                else
                {
                	Timer.DelayCall(TimeSpan.FromSeconds(10.0), new TimerCallback(MakeUnCriminal));
                	this.Stop();
                }
            }
            
            private void MakeUnCriminal()
            {
            	player.SendMessage("You are no longer considered a criminal.");
            	player.Criminal = false;
            }
        }
    }
}
