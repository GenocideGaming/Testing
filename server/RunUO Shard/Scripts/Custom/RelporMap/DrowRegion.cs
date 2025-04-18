using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Server.Regions
{
    class DrowRegion : NoHousingRegion
    {
        public DrowRegion(XmlElement element, Map map, Region parent) : base(element, map, parent) { }

        public override void OnEnter(Mobile m)
        {
            m.SendMessage("You are entering Drow territory!");
            base.OnEnter(m);
        }

        public override void OnExit(Mobile m)
        {
            m.SendMessage("You are exiting Drow territory.");
            base.OnExit(m);
        }
    }
}
