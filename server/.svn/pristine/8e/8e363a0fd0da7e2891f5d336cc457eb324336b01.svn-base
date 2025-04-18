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
using Server.Regions;

namespace Server.Gumps
{
    public class ChooseEquipmentGump : Gump
    {
        Mobile caller;
        Dictionary<Layer, int> currentEquipment;

        public static void Initialize()
        {
            CommandSystem.Register("wwequip", AccessLevel.Player, new CommandEventHandler(wwequip_OnCommand));
        }

        [Usage("wwequip")]
        [Description("Makes a call to your custom gump.")]
        public static void wwequip_OnCommand(CommandEventArgs e)
        {
            Mobile caller = e.Mobile;
            if (caller.Region is StagingAreaRegion)
            {
                if (caller.HasGump(typeof(ChooseEquipmentGump)))
                    caller.CloseGump(typeof(ChooseEquipmentGump));
                caller.SendGump(new ChooseEquipmentGump(caller));
            }
            else
            {
                caller.SendMessage("You have to be in the staging area to use that command!");
            }
        }

        public ChooseEquipmentGump(Mobile from, Dictionary<Layer, int> items)
            : base(0, 0)
        {
            caller = from;
            currentEquipment = items;

            DoGump();
        }

        public ChooseEquipmentGump(Mobile from)
            : base(0, 0)
        {
            caller = from;
            currentEquipment = ParseItemsWornBy(from);

            DoGump();
        }

        private Dictionary<Layer, int> ParseItemsWornBy(Mobile from)
        {
            Layer[] layers = { Layer.Pants, Layer.Helm, Layer.Neck, Layer.InnerTorso, Layer.Arms, 
                               Layer.Gloves, Layer.Shirt, Layer.Shoes };
            Dictionary<Layer, int> items = new Dictionary<Layer, int>();

            foreach (Layer layer in layers)
            {
                Item thisItem = from.FindItemOnLayer(layer);
                if (thisItem != null && thisItem.Movable == false && thisItem.Name == "eventsupply")
                    items.Add(layer, thisItem.ItemID);
            }

            return items;
        }

        int armorHue;
        private void DoGump()
        {
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;



            ICommonwealth town = Commonwealth.Find(caller);
            if (town != null)
                armorHue = CitizenDyeHue.Find(town.Definition.TownName);
            else
                armorHue = 0;

            DrawBackground();

            DrawPaperdollBackground();

            DrawItemsOnPaperdoll();

            DrawItemSelectionBoxes();

            DrawEquipmentSetBackground();

            DrawEquipmentSetBoxes();

            //DrawSuppliesCheckboxes(); // Don't need this--use xmlspawner event supply

            DrawOkayCancelButtons();
        }

        private void DrawItemsOnPaperdoll()
        {
            // Draw layer by layer for z-ordering reasons
            DrawLayer(Layer.Shirt);
            DrawLayer(Layer.Neck);
            DrawLayer(Layer.Arms);
            DrawLayer(Layer.Gloves);
            DrawLayer(Layer.Pants);
            DrawLayer(Layer.InnerLegs);
            DrawLayer(Layer.InnerTorso);
            DrawLayer(Layer.Helm);
            DrawLayer(Layer.Shoes);
        }

        private void DrawLayer(Layer layer)
        {
            if (currentEquipment.ContainsKey(layer))
                AddImage(147, 153, Lookup.ArtFromItemID(currentEquipment[layer]), armorHue);
        }

        private void DrawSuppliesCheckboxes()
        {

            AddLabel(452, 312, 0, @"Supplies:");

            AddLabel(486, 334, 0, @"Reagents");
            AddCheck(461, 334, 210, 211, false, (int)Buttons.chkReagents);

            AddLabel(486, 359, 0, @"Bandages");
            AddCheck(461, 359, 210, 211, false, (int)Buttons.chkBandages);

            AddLabel(486, 384, 0, @"Combat Potions");
            AddCheck(461, 384, 210, 211, false, (int)Buttons.chkPotions);

            AddLabel(486, 409, 0, @"Arrows and Bolts");
            AddCheck(461, 409, 210, 211, false, (int)Buttons.chkArrows);

            AddLabel(650, 334, 0, @"Flash Powder");
            AddCheck(625, 334, 210, 211, false, (int)Buttons.chkFlashPowder);

            AddLabel(650, 359, 0, @"Poison");
            AddCheck(625, 359, 210, 211, false, (int)Buttons.chkPoison);
        }

        private void DrawEquipmentSetBoxes()
        {
            AddBackground(450, 174, 301, 35, 9300);
            AddLabel(463, 181, 2130, EquipmentSetRegistry.GetName(caller, 0));
            AddButton(671, 180, 4011, 4012, (int)Buttons.SaveSet1, GumpButtonType.Reply, 0);
            if (EquipmentSetRegistry.Contains(caller, 0))
                AddButton(711, 180, 4005, 4006, (int)Buttons.UseSet1, GumpButtonType.Reply, 0);

            AddBackground(450, 215, 301, 35, 9300);
            AddLabel(463, 222, 2130, EquipmentSetRegistry.GetName(caller, 1));
            AddButton(671, 221, 4011, 4012, (int)Buttons.SaveSet2, GumpButtonType.Reply, 0);
            if (EquipmentSetRegistry.Contains(caller, 1))
                AddButton(711, 221, 4005, 4006, (int)Buttons.UseSet2, GumpButtonType.Reply, 0);

            AddBackground(450, 257, 301, 35, 9300);
            AddLabel(463, 264, 2130, EquipmentSetRegistry.GetName(caller, 2));
            AddButton(671, 263, 4011, 4012, (int)Buttons.SaveSet3, GumpButtonType.Reply, 0);
            if (EquipmentSetRegistry.Contains(caller, 2))
                AddButton(711, 263, 4005, 4006, (int)Buttons.UseSet3, GumpButtonType.Reply, 0);
        }

        private void DrawOkayCancelButtons()
        {
            AddButton(704, 488, 247, 248, (int)Buttons.Okay, GumpButtonType.Reply, 0);
            AddButton(629, 488, 242, 241, (int)Buttons.CloseGump, GumpButtonType.Reply, 0);
        }

        private void DrawEquipmentSetBackground()
        {
            AddBackground(423, 125, 348, 350, 9390);          // Big Grey Scroll
            AddLabel(670, 154, 2130, @"Save");
            AddLabel(714, 154, 2130, @"Use");
            AddImageTiled(501, 298, 195, 15, 58);             // Divider - Middle
            AddImage(471, 298, 57);                           // Divider - Left
            AddImage(692, 298, 59);                           // Divider - Right
        }

        private void DrawItemSelectionBoxes()
        {
            // Item Selection Boxes
            AddBackground(47, 146, 60, 50, 9200);
            AddLabel(60, 123, 2130, @"Head");
            AddButton(62, 190, 4005, 4006, (int)Buttons.Head, GumpButtonType.Reply, 0);
            if (currentEquipment.ContainsKey(Layer.Helm))
                AddItem(57, 151, currentEquipment[Layer.Helm], 0);

            AddBackground(47, 247, 60, 50, 9200);
            AddLabel(60, 224, 2130, @"Neck");
            AddButton(62, 291, 4005, 4006, (int)Buttons.Neck, GumpButtonType.Reply, 0);
            if (currentEquipment.ContainsKey(Layer.Neck))
                AddItem(57, 252, currentEquipment[Layer.Neck], 0);

            AddBackground(47, 349, 60, 50, 9200);
            AddLabel(59, 326, 2130, @"Chest");
            AddButton(63, 393, 4005, 4006, (int)Buttons.Chest, GumpButtonType.Reply, 0);
            if (currentEquipment.ContainsKey(Layer.InnerTorso))
                AddItem(57, 354, currentEquipment[Layer.InnerTorso], 0);

            AddBackground(358, 144, 60, 50, 9200);
            AddLabel(369, 121, 2130, @"Arms");
            AddButton(373, 188, 4005, 4006, (int)Buttons.Arms, GumpButtonType.Reply, 0);
            if (currentEquipment.ContainsKey(Layer.Arms))
                AddItem(368, 149, currentEquipment[Layer.Arms], 0);

            AddBackground(358, 245, 60, 50, 9200);
            AddLabel(371, 222, 2130, @"Hands");
            AddButton(373, 289, 4005, 4006, (int)Buttons.Hands, GumpButtonType.Reply, 0);
            if (currentEquipment.ContainsKey(Layer.Gloves))
                AddItem(368, 250, currentEquipment[Layer.Gloves], 0);

            AddBackground(359, 347, 60, 50, 9200);
            AddLabel(371, 324, 2130, @"Legs");
            AddButton(374, 391, 4005, 4006, (int)Buttons.Legs, GumpButtonType.Reply, 0);
            if (currentEquipment.ContainsKey(Layer.Pants))
                AddItem(368, 352, currentEquipment[Layer.Pants], 0);

            /* // don't need this-use XmlSpawners for this
            AddBackground(160, 442, 60, 50, 9200);
            AddLabel(158, 419, 2130, @"Main Hand");
            AddButton(175, 486, 4005, 4006, (int)Buttons.MainHand, GumpButtonType.Reply, 0);
            //AddItem(166, 445, 5122);

            AddBackground(247, 441, 60, 50, 9200);
            AddLabel(250, 419, 2130, @"Off Hand");
            AddButton(262, 485, 4005, 4006, (int)Buttons.OffHand, GumpButtonType.Reply, 0);
             */
        }

        private void DrawPaperdollBackground()
        {
            // Paperdoll Background
            AddBackground(129, 134, 212, 284, 9250);                // Paperdoll Border
            AddImageTiled(144, 149, 182, 256, 2702);                // Black Paperdoll Background
            AddImage(147, 153, 12, 1002);                           // Paperdoll Person
        }

        private void DrawBackground()
        {
            // General Layout
            AddPage(0);
            AddBackground(15, 54, 773, 474, 9270);                  // Big Main Border - Purple 
            AddBackground(309, 72, 210, 45, 9250);                  // Heading Border
            AddLabel(365, 84, 2130, @"Choose Equipment");
        }

        public enum Buttons
        {
            CloseGump,
            Hands,
            Head,
            Neck,
            Chest,
            Arms,
            Legs,
            MainHand,
            OffHand,
            Okay,
            SaveSet1,
            UseSet1,
            SaveSet2,
            UseSet2,
            chkReagents,
            chkBandages,
            chkPotions,
            chkArrows,
            chkFlashPowder,
            chkPoison,
            SaveSet3,
            UseSet3,
        }


        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            switch (info.ButtonID)
            {
                // Swap equipment slots
                case (int)Buttons.Head:
                    {
                        sender.Mobile.SendGump(new SelectItemGump(from, Layer.Helm, currentEquipment));
                        break;
                    }
                case (int)Buttons.Neck:
                    {
                        sender.Mobile.SendGump(new SelectItemGump(from, Layer.Neck, currentEquipment));
                        break;
                    }
                case (int)Buttons.Chest:
                    {
                        sender.Mobile.SendGump(new SelectItemGump(from, Layer.InnerTorso, currentEquipment));
                        break;
                    }
                case (int)Buttons.Arms:
                    {
                        sender.Mobile.SendGump(new SelectItemGump(from, Layer.Arms, currentEquipment));
                        break;
                    }
                case (int)Buttons.Hands:
                    {
                        sender.Mobile.SendGump(new SelectItemGump(from, Layer.Gloves, currentEquipment));
                        break;
                    }
                case (int)Buttons.Legs:
                    {
                        sender.Mobile.SendGump(new SelectItemGump(from, Layer.Pants, currentEquipment));
                        break;
                    }

                // Equipment sets
                case (int)Buttons.SaveSet1:
                    {

                        SaveSet(0);
                        break;
                    }
                case (int)Buttons.SaveSet2:
                    {
                        SaveSet(1);
                        break;
                    }
                case (int)Buttons.SaveSet3:
                    {
                        SaveSet(2);
                        break;
                    }
                case (int)Buttons.UseSet1:
                    {
                        GetSet(0);

                        break;
                    }
                case (int)Buttons.UseSet2:
                    {
                        GetSet(1);

                        break;
                    }

                case (int)Buttons.UseSet3:
                    {
                        GetSet(2);

                        break;
                    }
                // Okay / Cancel
                case (int)Buttons.Okay:
                    {
                        // Todo: Add a check to make sure you can actually equip this crap (and put in backpack if not)?
                        ProcessLayer(from, Layer.Helm);
                        ProcessLayer(from, Layer.Neck);
                        ProcessLayer(from, Layer.InnerTorso);
                        ProcessLayer(from, Layer.Arms);
                        ProcessLayer(from, Layer.Gloves);
                        ProcessLayer(from, Layer.Pants);

                        break;
                    }
                default: { return; }
            }
        }

        private void GetSet(int slot)
        {
            if (caller.HasGump(typeof(ChooseEquipmentGump)))
                caller.CloseGump(typeof(ChooseEquipmentGump));

            if (EquipmentSetRegistry.Contains(caller, slot))
            {
                Set newSet = EquipmentSetRegistry.Retrieve(caller, slot);
                caller.SendGump(new ChooseEquipmentGump(caller, newSet.Equipment));
            }
            else
                caller.SendGump(new ChooseEquipmentGump(caller, currentEquipment));
        }

        private void SaveSet(int slot)
        {
            if (EquipmentSetRegistry.Contains(caller, slot))
            {
                string name = EquipmentSetRegistry.GetName(caller, slot);
                caller.SendGump(new SetNameEntryGump(currentEquipment, slot, name));
            }
            else
                caller.SendGump(new SetNameEntryGump(currentEquipment, slot));
        }

        private void ProcessLayer(Mobile from, Layer layer)
        {
            if (currentEquipment.ContainsKey(layer))
            {
                Item armorPiece = Lookup.ItemFromItemID(currentEquipment[layer]);
                armorPiece.Hue = armorHue;
                armorPiece.Movable = false;
                //armorPiece.Name = "eventsupply";

                if (armorPiece is BaseArmor)
                {
                    ((BaseArmor)armorPiece).Quality = ArmorQuality.Exceptional;
                }

                VorshunArmory.PutConflictingItemInPack(armorPiece, from);
                
                from.EquipItem(armorPiece);
            }
            else
            {
                Item toRemove = from.FindItemOnLayer(layer);
                if (toRemove != null) { from.RemoveItem(toRemove); }
            }
        }
    }
}