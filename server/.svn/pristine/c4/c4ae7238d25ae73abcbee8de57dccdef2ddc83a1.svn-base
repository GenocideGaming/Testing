using System;
using System.Collections.Generic;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Commands;
using Server.Scripts.Custom.VorshunWarsEquipment;
using Server.Mobiles;
using Server.Scripts.Custom.Citizenship;
using Server.Items;

namespace Server.Scripts.Custom.VorshunWarsEquipment
{
    public static class Lookup
    {
        public const int male_offset = 50000;

        // We have to store this somewhere, hardcoding is as terrible a choice as any, but at least it's fast
        public static int ArtFromItemID(int itemID)
        {
            switch (itemID)
            {
                // Helm
                case 0x1718: { return 409 + male_offset; }                      // Wizard's hat
                case 0x1412: { return 563 + male_offset; }                      // plate helm
                case 0x140E: { return 564 + male_offset; }                      // norse helm
                case 0x13BB: { return 561 + male_offset; }              
                case 0x1DB9: { return 560 + male_offset; }                      // leather cap
                // Neck
                case 0x13C7: { return 546 + male_offset; }  // leather gorget
                case 0x13D6: { return 525 + male_offset; }  // studded gorget
                case 0x1413: { return 531 + male_offset; }  // plate gorget
                // Chest
                case 0x1415:
                case 0x1416: { return 527 + male_offset; }      // plate
                case 0x13C4:
                case 0x13BF: { return 538 + male_offset; }      // chainmail
                case 0x13ED:
                case 0x13EC: { return 548 + male_offset; }      // ringmail
                case 0x13DB:
                case 0x13E2: { return 521 + male_offset; }      // studded
                case 0x13CC:
                case 0x13D3: { return 542 + male_offset; }      // leather
                // Arms
                case 0x1410:
                case 0x1417: { return 528 + male_offset; }  // plate arms
                case 0x13EE: { return 550 + male_offset; }  // ringmail sleeves
                case 0x13DC:
                case 0x13D4: { return 523 + male_offset; }   // studded sleeves
                case 0x13C5:
                case 0x13CD: { return 544 + male_offset; }  // leather sleeves
                // Hands
                case 0x1414:
                case 0x1418: { return 530 + male_offset; }  // plate gloves
                case 0x13EB:
                case 0x13F2: { return 545 + male_offset; }  // ringmail gloves
                case 0x13D5:
                case 0x13DD: { return 524 + male_offset; }  // studded gloves
                case 0x13C6:
                case 0x13CE: { return 545 + male_offset; }  // leather gloves
                // Legs
                case 0x1411:
                case 0x141A: { return 529 + male_offset; }  // plate legs
                case 0x13BE:
                case 0x13C3: { return 540 + male_offset; }  // chain legs
                case 0x13F0:
                case 0x13F1: { return 549 + male_offset; }  // ring legs
                case 0x13DA:
                case 0x13E1: { return 522 + male_offset; }  // studded legs
                case 0x13CB:
                case 0x13D2: { return 543 + male_offset; }  // leather legs
                // Clothing
                case 0x1517: { return 434 + male_offset; }       // Shirt
                case 0x170B: { return 477 + male_offset; }       // Boots
                case 0x1539: { return 431 + male_offset; }       // Pants

                default: { return 0; }
            }
        }

        public static Item ItemFromItemID(int itemID)
        {
            switch (itemID)
            {
                // Helm
                case 0x1718: { return new WizardsHat(); }                      // Wizard's hat
                case 0x1412: { return new PlateHelm(); }                      // plate helm
                case 0x140E: { return new NorseHelm(); }                      // norse helm
                case 0x13BB: { return new ChainCoif(); }                      // chain coif
                case 0x1DB9: { return new LeatherCap(); }                      // leather cap
                // Neck
                case 0x13C7: { return new LeatherGorget(); }  // leather gorget
                case 0x13D6: { return new StuddedGorget(); }  // studded gorget
                case 0x1413: { return new PlateGorget(); }  // plate gorget
                // Chest
                case 0x1415:
                case 0x1416: { return new PlateChest(); }      // plate
                case 0x13C4:
                case 0x13BF: { return new ChainChest(); }        // chainmail
                case 0x13ED:
                case 0x13EC: { return new RingmailChest(); }      // ringmail
                case 0x13DB:
                case 0x13E2: { return new StuddedChest(); }      // studded
                case 0x13CC:
                case 0x13D3: { return new LeatherChest(); }      // leather
                // Arms
                case 0x1410:
                case 0x1417: { return new PlateArms(); }  // plate arms
                case 0x13EE: { return new RingmailArms();  }  // ringmail sleeves
                case 0x13DC:
                case 0x13D4: { return new StuddedArms(); }   // studded sleeves
                case 0x13C5:
                case 0x13CD: { return new LeatherArms(); }  // leather sleeves
                // Hands
                case 0x1414:
                case 0x1418: { return new PlateGloves(); }  // plate gloves
                case 0x13EB:
                case 0x13F2: { return new RingmailGloves(); }  // ringmail gloves
                case 0x13D5:
                case 0x13DD: { return new StuddedGloves(); }  // studded gloves
                case 0x13C6:
                case 0x13CE: { return new LeatherGloves(); }  // leather gloves
                // Legs
                case 0x1411:
                case 0x141A: { return new PlateLegs(); }  // plate legs
                case 0x13BE:
                case 0x13C3: { return new ChainLegs(); }  // chain legs
                case 0x13F0:
                case 0x13F1: { return new RingmailLegs(); }  // ring legs
                case 0x13DA:
                case 0x13E1: { return new StuddedLegs(); }  // studded legs
                case 0x13CB:
                case 0x13D2: { return new LeatherLegs(); }  // leather legs
                // Clothing
                case 0x1517: { return new Shirt(); }       // Shirt
                case 0x170B: { return new Boots(); }       // Boots
                case 0x1539: { return new LongPants(); }       // Pants

                default: { throw new ArgumentException("ItemFromItemID() was passed an invalid item ID"); }
            }
        }
    }
}
