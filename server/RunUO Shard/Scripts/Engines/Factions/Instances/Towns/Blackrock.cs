using System;
using System.Collections.Generic;
using System.Text;
using Server.Factions;
using Server.Scripts.Custom.Citizenship;

namespace Server.Scripts.Engines.Factions.Instances.Towns
{
    public class Blackrock : Town
    {
        public Blackrock()
        {
            Definition = new TownDefinition(
                2,
                "Blackrock",
                "Blackrock",
                new TextDefinition("BLACKROCK"),
                new TextDefinition("TOWN STONE FOR BLACKROCK"), //header
                new TextDefinition("Faction Town Stone of Blackrock"),
                new Point3D(0, 0, 0));

            OwningCommonwealth = Commonwealth.Find(Definition.TownName);
            Militia = Faction.Find(Definition.FriendlyName);
        }
    }
}
