using System;
using System.Collections.Generic;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Commands;
using Server.Scripts;
using Server.Mobiles;
using Server.Scripts.Custom.AntiRail;
using Server.Targeting;
using Server.Engines.Harvest;

namespace Server.Gumps
{
    public class AntiRailGump : Gump
    {
        static List<ItemSet> ItemSets = new List<ItemSet>();

        public static void Initialize()
        {
            //CommandSystem.Register("ar", AccessLevel.Administrator, new CommandEventHandler(AntiRail_OnCommand));

            ItemSets.Add(new ItemSet("bottles"));
            ItemSets.Add(new ItemSet("posts"));
            ItemSets.Add(new ItemSet("cookware"));
            ItemSets.Add(new ItemSet("lanterns"));
            ItemSets.Add(new ItemSet("chairs"));
            ItemSets.Add(new ItemSet("signs"));
            ItemSets.Add(new ItemSet("flowers"));
            ItemSets.Add(new ItemSet("wands"));
            ItemSets.Add(new ItemSet("jars"));
            ItemSets.Add(new ItemSet("scrolls"));
            ItemSets.Add(new ItemSet("instruments"));
            ItemSets.Add(new ItemSet("potions"));
            ItemSets.Add(new ItemSet("axes"));
            ItemSets.Add(new ItemSet("gravestones"));
            ItemSets.Add(new ItemSet("helmets"));
            ItemSets.Add(new ItemSet("bowls"));
            ItemSets.Add(new ItemSet("hats"));
            ItemSets.Add(new ItemSet("boards"));
            ItemSets.Add(new ItemSet("ingots"));
            ItemSets.Add(new ItemSet("shields"));
            ItemSets.Add(new ItemSet("cactii"));
            ItemSets.Add(new ItemSet("bolts"));
            ItemSets.Add(new ItemSet("decorativeshields"));
            ItemSets.Add(new ItemSet("skullcandles"));
            ItemSets.Add(new ItemSet("headtrophies"));
        }

        [Usage("ar")]
        [Description("DEBUG: Loads the anti-rail gump")]
        public static void AntiRail_OnCommand(CommandEventArgs e)
        {
            Mobile commandUser = e.Mobile;

            if (commandUser.HasGump(typeof(AntiRailGump)))
                commandUser.CloseGump(typeof(AntiRailGump));
            commandUser.SendGump(new AntiRailGump());    
        }

        private int oddOneOut;
        private HarvestTarget attemptedTarget = null;
        private object m_targeted = null;

        public AntiRailGump(Mobile from, HarvestTarget attemptedAction, object targeted)
            : this()
        {
            attemptedTarget = attemptedAction;
            m_targeted = targeted;

            ResponseTimerRegistry.AddTimer(from);
        }

        public AntiRailGump() : base( 200, 200 )
        {
            // TODO: Refactor this mess
            int oddItemSetIndex = Utility.Random(ItemSets.Count);
            int regularItemSetIndex;
            do
            {
                regularItemSetIndex = Utility.Random(ItemSets.Count);
            } while (regularItemSetIndex == oddItemSetIndex);

            int oddItemID = ItemSets[oddItemSetIndex].RandomItem();
            Stack<int> regularItemIDs = ItemSets[regularItemSetIndex].RandomSelection(3);

            oddOneOut = Utility.Random(4);

            this.Closable=true;
			this.Disposable=false;
			this.Dragable=true;
			this.Resizable=false;

			AddPage(0);
			AddBackground(0, 0, 482, 249, 9270);
			AddLabel(100, 18, 0x3E9, @"Which of these things is not like the others?");
			AddBackground(28, 45, 100, 100, 9250);
			AddBackground(138, 45, 100, 100, 9250);
			AddBackground(248, 45, 100, 100, 9250);
			AddBackground(358, 45, 100, 100, 9250);
			AddRadio(64, 150, 2151, 2153, false, 0);
			AddRadio(174, 150, 2151, 2153, false, 1);
			AddRadio(284, 150, 2151, 2153, false, 2);
			AddRadio(394, 150, 2151, 2153, false, 3);
			AddButton(398, 206, 247, 248, 1, GumpButtonType.Reply, 0);

            for (int i = 0; i < 4; i++)
            {
                int itemPosX = 58 + (110 * i) + RandomOffset();
                int itemPosY = 73 + RandomOffset();

                if (i == oddOneOut)
                    AddItem(itemPosX, itemPosY, oddItemID, GetHue());
                else
                    AddItem(itemPosX, itemPosY, regularItemIDs.Pop(), GetHue());
            }
        }

        private int RandomOffset()
        {
            return Utility.Random(-FeatureList.AntiRail.MovementOffset, FeatureList.AntiRail.MovementOffset*2);
        }

        private int GetHue()
        {
            if (FeatureList.AntiRail.RandomHue)
                return Utility.RandomDyedHue();
            else
                return 0;
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            switch(info.ButtonID)
            {
                case 0:
                    {
                        PlayerMobile pm = from as PlayerMobile;
                        if (pm == null) { break; }

                        ResponseTimerRegistry.RemoveTimer(pm);
                        break;
                    }

                case 1:
				{
                    PlayerMobile pm = from as PlayerMobile;
                    if (pm == null) { break; }

                    for (int i = 0; i < info.Switches.Length; i++)
                    {
                        int choice = info.Switches[i];

                        if (choice == oddOneOut && from is PlayerMobile)
                        {
                            pm.AntiRailResponseNeeded = false;
                            pm.ResourcesSinceLastChallenge = 0;
                            pm.IncorrectCaptchaResponses = 0;
                            ResponseTimerRegistry.RemoveTimer(pm);

                            if (attemptedTarget != null)
                                attemptedTarget.Invoke(from, m_targeted);
                        }
                        else
                        {
                            pm.IncorrectCaptchaResponses++;

                            if (pm.IncorrectCaptchaResponses >= FeatureList.AntiRail.MaxIncorrectGuesses)
                            {
                                PunishPlayer(pm);
                                break;
                            }

                            if (pm.HasGump(typeof(AntiRailGump)))
                                pm.CloseGump(typeof(AntiRailGump));

                            if (attemptedTarget != null)
                                pm.SendGump(new AntiRailGump(pm, attemptedTarget, m_targeted));
                            else
                                pm.SendGump(new AntiRailGump());
                        }

                    }

                    break;
				}
            }   
        }

        private void PunishPlayer(PlayerMobile pm)
        {
            Point3D jailedAt;
            switch (Utility.Random(4))
            {
                case 0: jailedAt = new Point3D(5333, 1708, 0); break;
                case 1: jailedAt = new Point3D(5349, 1708, 0); break;
                case 2: jailedAt = new Point3D(5333, 1727, 0); break;
                default: jailedAt = new Point3D(5349, 1727, 0); break;
            }

            pm.Location = jailedAt;

            if (pm.HasGump(typeof(AntiRailGump)))
                pm.CloseGump(typeof(AntiRailGump));

            ResponseTimerRegistry.RemoveTimer(pm);

            pm.SendMessage("You have been placed in jail for repeatedly failing the anti-macro test.");
            pm.SendMessage("You will remain here until processed by a member of staff.");
        }
    }
}