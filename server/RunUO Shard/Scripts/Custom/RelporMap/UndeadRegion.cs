using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Server.Regions
{
    class UndeadRegion : NoHousingRegion
    {
        public UndeadRegion(XmlElement element, Map map, Region parent) : base(element, map, parent) { }

        public override void OnEnter(Mobile m)
        {
            m.SendMessage("You are entering Undead territory!");
            base.OnEnter(m);
        }

        public override void OnExit(Mobile m)
        {
            m.SendMessage("You are exiting Undead territory.");
            base.OnExit(m);
        }
    }
}
