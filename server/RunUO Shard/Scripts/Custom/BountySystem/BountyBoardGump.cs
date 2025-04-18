using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Commands;
using Server.Mobiles;
using Server.Scripts.Custom.BountySystem;

namespace Server.Gumps
{
    public class BountyBoardGump : Gump
    {
        Mobile from;

        public static void Initialize()
        {
            CommandSystem.Register("bb", AccessLevel.Administrator, new CommandEventHandler(BB_OnCommand));
            CommandSystem.Register("clearbounties", AccessLevel.Administrator, new CommandEventHandler(WipeBounties_OnCommand));
        }

        [Usage("clearbounties")]
        [Description("Wipes the bounty board of all bounties posted.")]
        public static void WipeBounties_OnCommand(CommandEventArgs e)
        {
            BountyRegistry.Clear();
        }



        [Usage("bb")]
        [Description("Makes a call to the bounty board.")]
        public static void BB_OnCommand(CommandEventArgs e)
        {
            Mobile caller = e.Mobile;

            if (caller.HasGump(typeof(BountyBoardGump)))
                caller.CloseGump(typeof(BountyBoardGump));
            caller.SendGump(new BountyBoardGump(caller));
        }

        int currentPage;
        public BountyBoardGump(Mobile from) : this(from, 0) { }

        public BountyBoardGump(Mobile from, int currentPage)
            : base(0, 0)
        {
            this.from = from;
            this.currentPage = currentPage;

            DoGump();
        }

        const int entriesPerPage = 15;


        public void DoGump()
        {
            this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;

			AddPage(0);
			AddBackground(0, 0, 536, 524, 2600);
			AddBackground(23, 72, 484, 424, 9380);
			AddBackground(189, 20, 154, 42, 5120);
            AddLabel(227, 30, 2300, @"Bounty Board");
			AddLabel(75, 116, 2300, @"Name");
			AddLabel(239, 116, 2300, @"Bounty");
			AddLabel(356, 116, 2300, @"Last Murder");

            int entryOffset = currentPage * entriesPerPage;
            int entriesRemaining = BountyRegistry.Postings.Length - entryOffset;

            int numPostings;
            if (entriesRemaining < entriesPerPage)
                numPostings = entriesRemaining;
            else
                numPostings = entriesPerPage;

            for (int i = 0; i < numPostings; i++)
            {
                int offset = (i*20);
                string name = BountyRegistry.Postings[i+entryOffset].Killer.Name;
                int bounty = BountyRegistry.Postings[i+entryOffset].Bounty;
                string lastActive = BountyRegistry.Postings[i+entryOffset].LastActive;

                AddLabel(74, 142 + offset, 2300, name);
                AddLabel(240, 142 + offset, 2300, bounty.ToString() + " bc");
                AddLabel(357, 142 + offset, 2300, lastActive);
                AddImage(52, 144 + offset, 4035);
            }

            if (currentPage > 0)
                AddButton(438, 110, 4500, 4500, 1, GumpButtonType.Reply, 0);

            if (entriesRemaining > entriesPerPage)
                AddButton(438, 410, 4504, 4504, 2, GumpButtonType.Reply, 0);
        }   

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            switch(info.ButtonID)
            {
                case 0:
				{
					break;
				}
                case 1:
                {
                    if (from.HasGump(typeof(BountyBoardGump)))
                        from.CloseGump(typeof(BountyBoardGump));
                    from.SendGump(new BountyBoardGump(from, currentPage-1)); 

                    break;
                }
                case 2:
                {
                    if (from.HasGump(typeof(BountyBoardGump)))
                        from.CloseGump(typeof(BountyBoardGump));
                    from.SendGump(new BountyBoardGump(from, currentPage + 1)); 

                    break;
                }
            }
        }
    }
}