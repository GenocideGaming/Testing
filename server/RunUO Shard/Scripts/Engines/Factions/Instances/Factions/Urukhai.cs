using System;
using System.Collections.Generic;
using System.Text;
using Server.Factions;
using Server.Scripts.Custom.Citizenship;

namespace Server.Scripts.Engines.Factions.Instances.Factions
{
    public class Urukhai : Faction
    {
        private static Faction mInstance;

        public static Faction Instance { get { return mInstance; } }

        public Urukhai()
        {
            mInstance = this;

            Definition =
                new FactionDefinition(
                    2,
                    1507, //primary hue 2137
                    2212, //secondary hue 2112
                    2137, //join stone gold
                    2137, //broadcast gold
                    0xE0, 0xE0, // dire wolf
                    "Uruk", "Urk", //name and abbreviation
                    new TextDefinition("URUK-HAI"),
                    new TextDefinition("Uruk-hai"),
                    new TextDefinition("<center>URUK-HAI</center>"),
                    new TextDefinition(FeatureList.Militias.UrukhaiAboutText),
                    new TextDefinition("This faction is controlled by the Uruk-hai."),
                    new TextDefinition("The faction signup stone for the Uruk-hai."),
                    new TextDefinition("The faction stone of the Uruk-hai"),
                    new TextDefinition(": Uruk-hai"),
                    new TextDefinition("Members of the Uruk-hai will now be ignored."),
                    new TextDefinition("Members of the Uruk-hai will now be warned of their impending doom."),
                    new TextDefinition("Members of the Uruk-hai will now be attacked on sight."),
                    new RankDefinition[]
                    {
                        //format is rank#, required silver, wearable items, title text
                        new RankDefinition(10, 991, 8, new TextDefinition(FeatureList.Militias.UHTier5TitleText)),
                        new RankDefinition(9, 950, 7, new TextDefinition(FeatureList.Militias.UHTier4TitleText)),
                        new RankDefinition(8, 900, 6, new TextDefinition(FeatureList.Militias.UHTier4TitleText)),
                        new RankDefinition(7, 800, 6, new TextDefinition(FeatureList.Militias.UHTier3TitleText)),
                        new RankDefinition(6, 700, 5, new TextDefinition(FeatureList.Militias.UHTier3TitleText)),
                        new RankDefinition(5, 600, 5, new TextDefinition(FeatureList.Militias.UHTier2TitleText)),
                        new RankDefinition(4, 500, 5, new TextDefinition(FeatureList.Militias.UHTier2TitleText)),
                        new RankDefinition(3, 400, 4, new TextDefinition(FeatureList.Militias.UHTier1TitleText)),
                        new RankDefinition(2, 200, 4, new TextDefinition(FeatureList.Militias.UHTier1TitleText)),
                        new RankDefinition(1, 0, 4, new TextDefinition(FeatureList.Militias.UHTier0TitleText)),
                    },
                    new GuardDefinition[]
                    {
                        //format is type, itemid, price, upkeep, maximum?, header def, label when purchasing i think
                        new GuardDefinition(typeof(FactionHenchman), 0x1403, FeatureList.Militias.HenchmanGuardSilverCost, 0, 5, new TextDefinition("Henchman"), new TextDefinition("Hire Henchman")),
                        new GuardDefinition(typeof(FactionMercenary), 0x0F62, FeatureList.Militias.MercenaryGuardSilverCost, 0, 5, new TextDefinition("Mercenary"), new TextDefinition("Hire Mercenary")),
                        new GuardDefinition(typeof(FactionSorceress), 0x0E89, FeatureList.Militias.SorceressGuardSilverCost, 0, 5, new TextDefinition("Sorceress"), new TextDefinition("Hire Sorceress")),
                        new GuardDefinition(typeof(FactionWizard), 0x13F8, FeatureList.Militias.WizardGuardSilverCost, 0, 4, new TextDefinition("Wizard"), new TextDefinition("Hire Wizard")),
                        new GuardDefinition(typeof(FactionKnight), 0x0F4D, FeatureList.Militias.KnightGuardSilverCost, 0, 4, new TextDefinition("Knight"), new TextDefinition("Hire Knight")),

                    },
                    "Blackrock",
                    new Point3D(656, 1694, 0));//militia stone pos


            OwningCommonwealth = Commonwealth.Find(Definition.TownshipName);
        }
    }
}
