using System;
using System.Collections.Generic;
using System.Text;
using Server.Factions;
using Server.Scripts.Custom.Citizenship;

namespace Server.Scripts.Engines.Factions.Instances.Towns
{
    public class Yew : Town
    {
        public Yew()
        {
            Definition = new TownDefinition(
                0,
                "Yew",
                "Yew",
                new TextDefinition("YEW"),
                new TextDefinition("TOWN STONE FOR YEW"), //header
                new TextDefinition("Faction Town Stone of Yew"),
                new Point3D(0, 0, 0));

            OwningCommonwealth = Commonwealth.Find(Definition.TownName);
            Militia = Faction.Find(Definition.FriendlyName);
        }
    }
}
