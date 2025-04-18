using System;
using System.Collections.Generic;
using System.Text;
using Server.Factions;
using Server.Scripts.Custom.Citizenship;

namespace Server.Scripts.Engines.Factions.Instances.Towns
{
    public class Trinsic : Town
    {
        public Trinsic() 
        {
            Definition = new TownDefinition(
                1,
                "Trinsic",
                "Trinsic",
                new TextDefinition("TRINSIC"),
                new TextDefinition("TOWN STONE FOR TRINSIC"), //header
                new TextDefinition("Faction Town Stone of Trinsic"),
                new Point3D(0, 0, 0));

            OwningCommonwealth = Commonwealth.Find(Definition.TownName);
            Militia = Faction.Find(Definition.FriendlyName);

        }
    }
}
