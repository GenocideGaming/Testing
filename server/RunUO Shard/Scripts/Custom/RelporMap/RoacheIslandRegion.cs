using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Server.Regions
{
    class RoacheIslandRegion : BaseRegion
    {
        public RoacheIslandRegion(XmlElement element, Map map, Region parent) : base(element, map, parent) { }

        public override bool AllowHousing(Mobile from, Point3D p)
        {
            return true;
        }

        public override void OnEnter(Mobile m)
        {
            m.SendMessage("All who enter Buccaneer's Den are considered criminals.");
            RoacheCriminalTimer criminalTimer = new RoacheCriminalTimer(m);
            criminalTimer.Start();

            base.OnEnter(m);
        }

        public override void OnExit(Mobile m)
        {
            m.SendMessage("You have left Buccaneer's Den.");
            base.OnExit(m);
        }

        private class RoacheCriminalTimer : Timer
        {
            private Mobile player;

            public RoacheCriminalTimer(Mobile m)
                : base(TimeSpan.Zero, TimeSpan.FromSeconds(1.0))
            {
                player = m;
            }

            protected override void OnTick()
            {
                if (player.Region.IsPartOf("Buccaneer's Den"))
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
