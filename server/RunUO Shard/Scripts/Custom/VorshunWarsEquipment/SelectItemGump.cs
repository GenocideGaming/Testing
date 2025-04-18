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
    public class SelectItemGump : Gump
    {
        const int LABEL_X_OFFSET = 155;
        const int Y_OFFSET = 261;
        const int BUTTON_X_OFFSET = 323;
        const int GAP = 27;

        const int WINDOW_MIN = 95;

        Mobile m_From;
        Layer m_Layer;
        Dictionary<Layer, int> m_ItemChoices;

        public SelectItemGump(Mobile from, Layer layer, Dictionary<Layer, int> items)
            : base(0, 0)
        {
            m_From = from;
            this.m_Layer = layer;
            m_ItemChoices = items;

            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;

            AddPage(0);
            AddBackground(131, 179, 244, GetGumpHeight(), 9250);

            DrawHeader();

            DrawSelection();
        }

        private int GetGumpHeight()
        {
            switch (m_Layer)
            {
                case Layer.Helm: { return WINDOW_MIN + (GAP * 6); }
                case Layer.Neck: { return WINDOW_MIN + (GAP * 4); }
                case Layer.InnerTorso: { return WINDOW_MIN + (GAP * 6); }
                case Layer.Arms: { return WINDOW_MIN + (GAP * 5); }
                case Layer.Gloves: { return WINDOW_MIN + (GAP * 5); }
                case Layer.Pants: { return WINDOW_MIN + (GAP * 6); }
                case Layer.TwoHanded: { return WINDOW_MIN + (GAP * 6); }
                case Layer.OneHanded: { return WINDOW_MIN + (GAP * 6); }
                default: { return WINDOW_MIN; }
            }
        }

        private void DrawSelection()
        {
            switch (m_Layer)
            {
                case Layer.Helm:
                    {
                        DrawHelmSelection();
                        return;
                    }
                case Layer.Neck:
                    {
                        DrawNeckSelection();
                        return;
                    }
                case Layer.InnerTorso:
                    {
                        DrawChestSelection();
                        return;
                    }
                case Layer.Arms:
                    {
                        DrawSleeveSelection();
                        return;
                    }
                case Layer.Gloves:
                    {
                        DrawGloveSelection();
                        return;
                    }
                case Layer.Pants:
                    {
                        DrawLegSelection();
                        return;
                    }
            }
        }

        private void DrawHelmSelection()
        {
            int index = 0;
            AddLabel(LABEL_X_OFFSET, Y_OFFSET + (GAP * index), 2130, @"Wizard's Hat");
            AddButton(BUTTON_X_OFFSET, Y_OFFSET + (GAP * index), 4023, 4024, 0x1718, GumpButtonType.Reply, 0);
            index++;

            AddLabel(LABEL_X_OFFSET, Y_OFFSET + (GAP * index), 2130, @"Plate Helm");
            AddButton(BUTTON_X_OFFSET, Y_OFFSET + (GAP * index), 4023, 4024, 0x1412, GumpButtonType.Reply, 0);
            index++;

            AddLabel(LABEL_X_OFFSET, Y_OFFSET + (GAP * index), 2130, @"Norse Helm");
            AddButton(BUTTON_X_OFFSET, Y_OFFSET + (GAP * index), 4023, 4024, 0x140E, GumpButtonType.Reply, 0);
            index++;

            AddLabel(LABEL_X_OFFSET, Y_OFFSET + (GAP * index), 2130, @"Chainmail Coif");
            AddButton(BUTTON_X_OFFSET, Y_OFFSET + (GAP * index), 4023, 4024, 0x13BB, GumpButtonType.Reply, 0);
            index++;

            AddLabel(LABEL_X_OFFSET, Y_OFFSET + (GAP * index), 2130, @"Leather Cap");
            AddButton(BUTTON_X_OFFSET, Y_OFFSET + (GAP * index), 4023, 4024, 0x1DB9, GumpButtonType.Reply, 0);
            index++;

            AddLabel(LABEL_X_OFFSET, Y_OFFSET + (GAP * index), 2130, @"Nothing (empty slot)");
            AddButton(BUTTON_X_OFFSET, Y_OFFSET + (GAP * index), 4023, 4024, -1, GumpButtonType.Reply, 0);
            index++;
        }

        private void DrawNeckSelection()
        {
            int index = 0;
            AddLabel(LABEL_X_OFFSET, Y_OFFSET + (GAP * index), 2130, @"Plate Gorget");
            AddButton(BUTTON_X_OFFSET, Y_OFFSET + (GAP * index), 4023, 4024, 0x1413, GumpButtonType.Reply, 0);
            index++;

            AddLabel(LABEL_X_OFFSET, Y_OFFSET + (GAP * index), 2130, @"Studded Gorget");
            AddButton(BUTTON_X_OFFSET, Y_OFFSET + (GAP * index), 4023, 4024, 0x13D6, GumpButtonType.Reply, 0);
            index++;

            AddLabel(LABEL_X_OFFSET, Y_OFFSET + (GAP * index), 2130, @"Leather Gorget");
            AddButton(BUTTON_X_OFFSET, Y_OFFSET + (GAP * index), 4023, 4024, 0x13C7, GumpButtonType.Reply, 0);
            index++;

            AddLabel(LABEL_X_OFFSET, Y_OFFSET + (GAP * index), 2130, @"Nothing (empty slot)");
            AddButton(BUTTON_X_OFFSET, Y_OFFSET + (GAP * index), 4023, 4024, -1, GumpButtonType.Reply, 0);
            index++;
        }

        private void DrawChestSelection()
        {
            int index = 0;
            AddLabel(LABEL_X_OFFSET, Y_OFFSET + (GAP * index), 2130, @"Plate Chest");
            AddButton(BUTTON_X_OFFSET, Y_OFFSET + (GAP * index), 4023, 4024, 0x1415, GumpButtonType.Reply, 0);
            index++;

            AddLabel(LABEL_X_OFFSET, Y_OFFSET + (GAP * index), 2130, @"Chainmail Tunic");
            AddButton(BUTTON_X_OFFSET, Y_OFFSET + (GAP * index), 4023, 4024, 0x13C4, GumpButtonType.Reply, 0);
            index++;

            AddLabel(LABEL_X_OFFSET, Y_OFFSET + (GAP * index), 2130, @"Ringmail Chest");
            AddButton(BUTTON_X_OFFSET, Y_OFFSET + (GAP * index), 4023, 4024, 0x13ED, GumpButtonType.Reply, 0);
            index++;

            AddLabel(LABEL_X_OFFSET, Y_OFFSET + (GAP * index), 2130, @"Studded Chest");
            AddButton(BUTTON_X_OFFSET, Y_OFFSET + (GAP * index), 4023, 4024, 0x13E2, GumpButtonType.Reply, 0);
            index++;

            AddLabel(LABEL_X_OFFSET, Y_OFFSET + (GAP * index), 2130, @"Leather Chest");
            AddButton(BUTTON_X_OFFSET, Y_OFFSET + (GAP * index), 4023, 4024, 0x13D3, GumpButtonType.Reply, 0);
            index++;

            AddLabel(LABEL_X_OFFSET, Y_OFFSET + (GAP * index), 2130, @"Nothing (empty slot)");
            AddButton(BUTTON_X_OFFSET, Y_OFFSET + (GAP * index), 4023, 4024, -1, GumpButtonType.Reply, 0);
            index++;
        }

        private void DrawSleeveSelection()
        {
            int index = 0;
            AddLabel(LABEL_X_OFFSET, Y_OFFSET + (GAP * index), 2130, @"Plate Arms");
            AddButton(BUTTON_X_OFFSET, Y_OFFSET + (GAP * index), 4023, 4024, 0x1417, GumpButtonType.Reply, 0);
            index++;

            AddLabel(LABEL_X_OFFSET, Y_OFFSET + (GAP * index), 2130, @"Ringmail Sleeves");
            AddButton(BUTTON_X_OFFSET, Y_OFFSET + (GAP * index), 4023, 4024, 0x13EE, GumpButtonType.Reply, 0);
            index++;

            AddLabel(LABEL_X_OFFSET, Y_OFFSET + (GAP * index), 2130, @"Studded Sleeves");
            AddButton(BUTTON_X_OFFSET, Y_OFFSET + (GAP * index), 4023, 4024, 0x13DC, GumpButtonType.Reply, 0);
            index++;

            AddLabel(LABEL_X_OFFSET, Y_OFFSET + (GAP * index), 2130, @"Leather Sleeves");
            AddButton(BUTTON_X_OFFSET, Y_OFFSET + (GAP * index), 4023, 4024, 0x13CD, GumpButtonType.Reply, 0);
            index++;

            AddLabel(LABEL_X_OFFSET, Y_OFFSET + (GAP * index), 2130, @"Nothing (empty slot)");
            AddButton(BUTTON_X_OFFSET, Y_OFFSET + (GAP * index), 4023, 4024, -1, GumpButtonType.Reply, 0);
            index++;
        }

        private void DrawGloveSelection()
        {
            int index = 0;
            AddLabel(LABEL_X_OFFSET, Y_OFFSET + (GAP * index), 2130, @"Plate Gloves");
            AddButton(BUTTON_X_OFFSET, Y_OFFSET + (GAP * index), 4023, 4024, 0x1414, GumpButtonType.Reply, 0);
            index++;

            AddLabel(LABEL_X_OFFSET, Y_OFFSET + (GAP * index), 2130, @"Ringmail Gloves");
            AddButton(BUTTON_X_OFFSET, Y_OFFSET + (GAP * index), 4023, 4024, 0x13EB, GumpButtonType.Reply, 0);
            index++;

            AddLabel(LABEL_X_OFFSET, Y_OFFSET + (GAP * index), 2130, @"Studded Gloves");
            AddButton(BUTTON_X_OFFSET, Y_OFFSET + (GAP * index), 4023, 4024, 0x13D5, GumpButtonType.Reply, 0);
            index++;

            AddLabel(LABEL_X_OFFSET, Y_OFFSET + (GAP * index), 2130, @"Leather Gloves");
            AddButton(BUTTON_X_OFFSET, Y_OFFSET + (GAP * index), 4023, 4024, 0x13C6, GumpButtonType.Reply, 0);
            index++;

            AddLabel(LABEL_X_OFFSET, Y_OFFSET + (GAP * index), 2130, @"Nothing (empty slot)");
            AddButton(BUTTON_X_OFFSET, Y_OFFSET + (GAP * index), 4023, 4024, -1, GumpButtonType.Reply, 0);
            index++;
        }

        private void DrawLegSelection()
        {
            int index = 0;
            AddLabel(LABEL_X_OFFSET, Y_OFFSET + (GAP * index), 2130, @"Plate Legs");
            AddButton(BUTTON_X_OFFSET, Y_OFFSET + (GAP * index), 4023, 4024, 0x1411, GumpButtonType.Reply, 0);
            index++;

            AddLabel(LABEL_X_OFFSET, Y_OFFSET + (GAP * index), 2130, @"Chainmail Legs");
            AddButton(BUTTON_X_OFFSET, Y_OFFSET + (GAP * index), 4023, 4024, 0x13BE, GumpButtonType.Reply, 0);
            index++;

            AddLabel(LABEL_X_OFFSET, Y_OFFSET + (GAP * index), 2130, @"Ringmail Legs");
            AddButton(BUTTON_X_OFFSET, Y_OFFSET + (GAP * index), 4023, 4024, 0x13F0, GumpButtonType.Reply, 0);
            index++;

            AddLabel(LABEL_X_OFFSET, Y_OFFSET + (GAP * index), 2130, @"Studded Legs");
            AddButton(BUTTON_X_OFFSET, Y_OFFSET + (GAP * index), 4023, 4024, 0x13DA, GumpButtonType.Reply, 0);
            index++;

            AddLabel(LABEL_X_OFFSET, Y_OFFSET + (GAP * index), 2130, @"Leather Legs");
            AddButton(BUTTON_X_OFFSET, Y_OFFSET + (GAP * index), 4023, 4024, 0x13CB, GumpButtonType.Reply, 0);
            index++;

            AddLabel(LABEL_X_OFFSET, Y_OFFSET + (GAP * index), 2130, @"Pants");
            AddButton(BUTTON_X_OFFSET, Y_OFFSET + (GAP * index), 4023, 4024, 0x1539, GumpButtonType.Reply, 0);
            index++;
        }

        private void DrawHeader()
        {
            AddBackground(154, 197, 203, 45, 9260);

            string layerName;

            if (m_Layer == Layer.Helm) { layerName = "Head"; }
            else if (m_Layer == Layer.Neck) { layerName = "Neck"; }
            else if (m_Layer == Layer.InnerTorso) { layerName = "Chest"; }
            else if (m_Layer == Layer.Arms) { layerName = "Arms"; }
            else if (m_Layer == Layer.Gloves) { layerName = "Hands"; }
            else if (m_Layer == Layer.Pants) { layerName = "Legs"; }
            else { layerName = "LAYER NOT FOUND"; }

            AddLabel(237, 209, 0, layerName);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            PlayerMobile pm = sender.Mobile as PlayerMobile;
            if (pm == null) return;

            if (info.ButtonID == 0)     // Cancelled gump without selecting anything, resend existing choices
            {
                if (pm.HasGump(typeof(ChooseEquipmentGump)))
                    pm.CloseGump(typeof(ChooseEquipmentGump));
                pm.SendGump(new ChooseEquipmentGump(m_From, m_ItemChoices));

                return;
            }

            if (UserSelectedNoArmor(info))
            {
                if (m_ItemChoices.ContainsKey(m_Layer))
                    m_ItemChoices.Remove(m_Layer);
            }
            else
            {
                if (m_ItemChoices.ContainsKey(m_Layer))
                    m_ItemChoices[m_Layer] = info.ButtonID;
                else
                    m_ItemChoices.Add(m_Layer, info.ButtonID);
            }

            if (pm.HasGump(typeof(ChooseEquipmentGump)))
                pm.CloseGump(typeof(ChooseEquipmentGump));
            pm.SendGump(new ChooseEquipmentGump(m_From, m_ItemChoices));
        }

        private static bool UserSelectedNoArmor(RelayInfo info)
        {
            return info.ButtonID == -1;
        }
    }
}
