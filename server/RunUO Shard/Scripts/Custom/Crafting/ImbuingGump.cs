using System;
using System.Collections.Generic;
using Server.Commands;
using Server.Mobiles;
using Server.Accounting;
using Server.Network;
using Server.Gumps;
using Server.Targeting;
using Server.Commands.Generic;
using Server.Items;
using Server.Engines.Craft;
using Server.Scripts;

namespace Server.Scripts.Custom.Crafting
{
    public class ImbuingGump : Gump
    {

        public static readonly int GumpOffsetX = PropsConfig.GumpOffsetX;
        public static readonly int GumpOffsetY = PropsConfig.GumpOffsetY;

        public static readonly int TextHue = PropsConfig.TextHue;
        public static readonly int TextOffsetX = PropsConfig.TextOffsetX;

        public static readonly int NextWidth = PropsConfig.NextWidth;
        public static readonly int NextOffsetX = PropsConfig.NextOffsetX, NextOffsetY = PropsConfig.NextOffsetY;
        public static readonly int ArrowButtonID1 = PropsConfig.NextButtonID1;
        public static readonly int ArrowButtonID2 = PropsConfig.NextButtonID2;

        private const int LabelColor32 = 0xDDDDDD;
        private const int WIDTH = 400;
        private const int HEIGHT = 100;
        readonly public static int LEFT_EDGE = 10;
        readonly public static int TOP_EDGE = 10;
        readonly public static int ARROW_WIDTH = 20;
        readonly public static int LABEL_WIDTH = 20;

        private Mobile m_Owner;
        private ImbuingTool m_Tool;

        public ImbuingGump(Mobile owner, ImbuingTool tool)
            : base(GumpOffsetX, GumpOffsetY)
        {
            owner.CloseGump(typeof(ImbuingGump));
            m_Owner = owner;
            m_Tool = tool;

            InitializeGump();
        }

        public string Center(string text)
        {
            return String.Format("<CENTER>{0}</CENTER>", text);
        }
        public string Color(string text, int color)
        {
            return String.Format("<BASEFONT COLOR=#{0:X6}>{1}</BASEFONT>", color, text);
        }

        public enum BUTTON_IDS : int
        {
            Closed = 0,
            Level1 = 1,
            Level2 = 2,
            Level3 = 3,
        }

        public void InitializeGump()
        {
            if (m_Owner == null || m_Tool == null) { return; }

            int penX = LEFT_EDGE;
            int penY = TOP_EDGE;
            int lineHeight = 20;
            double successchance = 0.0;

            AddBackground(0, 0, WIDTH, HEIGHT, 5054);
            AddAlphaRegion(LEFT_EDGE, TOP_EDGE, WIDTH - 2 * LEFT_EDGE, HEIGHT - 2 * TOP_EDGE);

            AddHtml(penX, penY, WIDTH - 2 * LEFT_EDGE, lineHeight, Color(Center("<u>Imbuing Options</u>"), LabelColor32), false, false);
            penY += lineHeight;

            AddButton(penX, penY, ArrowButtonID1, ArrowButtonID2, (int)BUTTON_IDS.Level1, GumpButtonType.Reply, 0);
            penX += NextWidth;
            // Show chances for Tier 1
            AddLabel(penX, penY, 2101, "Imbue Tier 1 (1 charge)");
            penX = LEFT_EDGE + WIDTH / 4 * 3;
            successchance = Math.Round(ImbuingTool.GetSuccessChance(m_Owner.Skills[m_Tool.CraftSystem.MainSkill].Value, FeatureList.ArtifactCrafting.Level1ContractSkillRequired) * 1000.0) / 10.0;
            AddLabel(penX, penY, 2101, "Chance: " + successchance + "%");

            penX = LEFT_EDGE;
            penY += lineHeight;

            AddButton(penX, penY, ArrowButtonID1, ArrowButtonID2, (int)BUTTON_IDS.Level2, GumpButtonType.Reply, 0);
            penX += NextWidth;
            // Show chances for Tier 2
            AddLabel(penX, penY, 2101, "Imbue Tier 2 (2 charges)");
            penX = LEFT_EDGE + WIDTH / 4 * 3;
            successchance = Math.Round(ImbuingTool.GetSuccessChance(m_Owner.Skills[m_Tool.CraftSystem.MainSkill].Value, FeatureList.ArtifactCrafting.Level2ContractSkillRequired) * 1000.0) / 10.0;
            AddLabel(penX, penY, 2101, "Chance: " + successchance + "%");

            penX = LEFT_EDGE;
            penY += lineHeight;

            AddButton(penX, penY, ArrowButtonID1, ArrowButtonID2, (int)BUTTON_IDS.Level3, GumpButtonType.Reply, 0);
            penX += NextWidth;
            // Show chances for Tier 3
            AddLabel(penX, penY, 2101, "Imbue Tier 3 (3 charges)");
            penX = LEFT_EDGE + WIDTH / 4 * 3;
            successchance = Math.Round(ImbuingTool.GetSuccessChance(m_Owner.Skills[m_Tool.CraftSystem.MainSkill].Value, FeatureList.ArtifactCrafting.Level3ContractSkillRequired) * 1000.0) / 10.0;
            AddLabel(penX, penY, 2101, "Chance: " + successchance + "%");
        }

        public void ImbueItem(int level)
        {
            if (m_Owner == null || m_Tool == null || level < 1 || level > 3) { return; }

            // Example: Imbuing logic based on the level
            BaseWeapon weapon = m_Owner.FindItemOnLayer(Layer.OneHanded) as BaseWeapon ?? m_Owner.FindItemOnLayer(Layer.TwoHanded) as BaseWeapon;
            if (weapon == null)
            {
                m_Owner.SendMessage("You need to equip a weapon to imbue it.");
                return;
            }

            switch (level)
            {
                case 1:
                    weapon.Attributes.WeaponDamage += 5; // Example modification for Tier 1
                    break;
                case 2:
                    weapon.Attributes.WeaponDamage += 10; // Example modification for Tier 2
                    break;
                case 3:
                    weapon.Attributes.WeaponDamage += 15; // Example modification for Tier 3
                    break;
            }

            m_Owner.SendMessage("You have successfully imbued the weapon!");
        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
            Mobile from = state.Mobile;

            if (m_Tool == null) { return; }

            if (m_Tool.UsesRemaining < 1)
            {
                from.SendMessage("The tool didn't have anything left!");
                m_Tool.Consume();
                return;
            }

            switch ((BUTTON_IDS)info.ButtonID)
            {
                case BUTTON_IDS.Closed:
                    return;
                case BUTTON_IDS.Level1:
                    m_Tool.UsesRemaining -= 1;
                    if (!ImbuingTool.SuccessRoll(from.Skills[m_Tool.CraftSystem.MainSkill].Value, FeatureList.ArtifactCrafting.Level1ContractSkillRequired))
                    {
                        from.SendMessage("You failed to imbue the item.");
                    }
                    else
                    {
                        ImbueItem(1);
                    }
                    break;
                case BUTTON_IDS.Level2:
                    if (m_Tool.UsesRemaining < 2)
                    {
                        m_Owner.SendMessage("Your tool doesn't have enough durability to handle that imbuing!");
                        return;
                    }
                    m_Tool.UsesRemaining -= 2;
                    if (!ImbuingTool.SuccessRoll(from.Skills[m_Tool.CraftSystem.MainSkill].Value, FeatureList.ArtifactCrafting.Level2ContractSkillRequired))
                    {
                        from.SendMessage("You failed to imbue the item.");
                    }
                    else
                    {
                        ImbueItem(2);
                    }
                    break;
                case BUTTON_IDS.Level3:
                    if (m_Tool.UsesRemaining < 3)
                    {
                        m_Owner.SendMessage("Your tool doesn't have enough durability to handle that imbuing!");
                        return;
                    }
                    m_Tool.UsesRemaining -= 3;
                    if (!ImbuingTool.SuccessRoll(from.Skills[m_Tool.CraftSystem.MainSkill].Value, FeatureList.ArtifactCrafting.Level3ContractSkillRequired))
                    {
                        from.SendMessage("You failed to imbue the item.");
                    }
                    else
                    {
                        ImbueItem(3);
                    }
                    break;
            }

            if (m_Tool.UsesRemaining < 1)
            {
                from.SendMessage("The tool didn't have anything left!");
                m_Tool.Consume();
                return;
            }
        }
    }
}
