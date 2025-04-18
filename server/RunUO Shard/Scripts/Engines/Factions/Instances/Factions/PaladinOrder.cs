using System;
using System.Collections.Generic;
using System.Text;
using Server.Factions;
using Server.Scripts.Custom.Citizenship;

namespace Server.Scripts.Engines.Factions.Instances.Factions
{
    public class PaladinOrder : Faction
    {
        private static Faction mInstance;

        public static Faction Instance { get { return mInstance; } }

        public PaladinOrder()
        {
            mInstance = this;

            Definition =
                new FactionDefinition(
                    1,
                    2141, //primary hue
                    1150, //secondary hue
                    2141, //join stone gold
                    2141, //broadcast gold
                    0x11C, 0x2D9C, // Charger of the Fallen
                    "Paladins", "PaL", //name and abbreviation
                    new TextDefinition("PALADIN ORDER"),
                    new TextDefinition("Paladin Order"),
                    new TextDefinition("<center>PALADIN ORDER</center>"),
                    new TextDefinition(FeatureList.Militias.PaladinOrderAboutText),
                    new TextDefinition("This faction is controlled by the Paladin Order."),
                    new TextDefinition("The faction signup stone for the Paladin Order."),
                    new TextDefinition("The faction stone of the Paladin Order"),
                    new TextDefinition(": Paladin Order"),
                    new TextDefinition("Members of the Paladin Order will now be ignored."),
                    new TextDefinition("Members of the Paladin Order will now be warned of their impending doom."),
                    new TextDefinition("Members of the Paladin Order will now be attacked on sight."),
                    new RankDefinition[]
                    {
                        //format is rank#, required silver, wearable items, title text
                        new RankDefinition(10, 991, 8, new TextDefinition(FeatureList.Militias.POTier5TitleText)),
                        new RankDefinition(9, 950, 7, new TextDefinition(FeatureList.Militias.POTier4TitleText)),
                        new RankDefinition(8, 900, 6, new TextDefinition(FeatureList.Militias.POTier4TitleText)),
                        new RankDefinition(7, 800, 6, new TextDefinition(FeatureList.Militias.POTier3TitleText)),
                        new RankDefinition(6, 700, 5, new TextDefinition(FeatureList.Militias.POTier3TitleText)),
                        new RankDefinition(5, 600, 5, new TextDefinition(FeatureList.Militias.POTier2TitleText)),
                        new RankDefinition(4, 500, 5, new TextDefinition(FeatureList.Militias.POTier2TitleText)),
                        new RankDefinition(3, 400, 4, new TextDefinition(FeatureList.Militias.POTier1TitleText)),
                        new RankDefinition(2, 200, 4, new TextDefinition(FeatureList.Militias.POTier1TitleText)),
                        new RankDefinition(1, 0, 4, new TextDefinition(FeatureList.Militias.POTier0TitleText)),
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
                    "Trinsic",
                    new Point3D(1905, 2717, 20));//faction stone pos


            OwningCommonwealth = Commonwealth.Find(Definition.TownshipName);
        }
    }
}
