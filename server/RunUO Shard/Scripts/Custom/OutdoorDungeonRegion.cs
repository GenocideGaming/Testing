using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Server.Regions
{
    class OutdoorDungeonRegion : DungeonRegion
    {
        public OutdoorDungeonRegion(XmlElement xml, Map map, Region parent)
            : base(xml, map, parent)
        {
        }

        public override void AlterLightLevel(Mobile m, ref int global, ref int personal)
        {
            global = LightCycle.OutdoorDungeonLevel;
        }
    }
}
