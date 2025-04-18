using System;
using System.Collections.Generic;
using System.Text;
using Server.Network;
using Server.Commands;
using Server.Scripts.Custom.Citizenship;

namespace Server.Gumps
{
    public class RelwisGump : Gump
    {
        Mobile caller;

        public static void Initialize()
        {
            CommandSystem.Register("relwis", AccessLevel.Player, new CommandEventHandler(relwis_OnCommand));

        }

        [Usage("relwis")]
        [Description("Opens the Rel Wis gump.")]
        public static void relwis_OnCommand(CommandEventArgs e)
        {
            if (e.Mobile.HasGump(typeof(RelwisGump)))
                e.Mobile.CloseGump(typeof(RelwisGump));
            e.Mobile.SendGump(new RelwisGump(e.Mobile));
        }

        public RelwisGump(Mobile from) : this()
        {
            caller = from;
        }

        public RelwisGump() : base( 0, 0 )
        {
            this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;

			AddPage(0);
			AddBackground(12, 12, 773, 452, 9200);
			AddLabel(35, 31, 370, @"Welcome to Rel Wis!");
			AddLabel(47, 56, 0, @"We hope you enjoy your time exploring and interacting with the world of Atria.");
			AddLabel(37, 81, 0, @"Any questions you may have about the world may very likely be already answered in our Frequently Asked Questions");
			AddLabel(34, 96, 0, @" or our Information Center. Beyond that, feel free to visit our forums for support or to interact with the community.");
			AddButton(61, 133, 1154, 1153, (int)Buttons.VisitInformationCenter, GumpButtonType.Reply, 0);
			AddButton(61, 193, 1154, 1153, (int)Buttons.VisitForums, GumpButtonType.Reply, 0);
			AddLabel(96, 132, 50, @"Visit Rel Wis Information Center");
			AddButton(61, 163, 1154, 1153, (int)Buttons.VisitFAQ, GumpButtonType.Reply, 0);
			AddLabel(96, 163, 50, @"Visit Rel Wis Frequently Asked Questions");
			AddLabel(96, 191, 50, @"Visit Rel Wis Forums");
			AddLabel(40, 234, 40, @"You can pull up this gump at any time by typing        in game.");
			AddLabel(69, 270, 0, @"Quick Actions:");
			AddRadio(80, 300, 209, 208, false, (int)Buttons.BecomeACitizenRadio);
			AddLabel(110, 298, 0, @"Become a citizen of this township.");
			AddLabel(331, 297, 970, @"(May only be executed within the borders of a democratic town.)");
			AddRadio(80, 329, 209, 208, false, (int)Buttons.CitizenshipStatusRadio);
			AddLabel(110, 329, 0, @"Check your citizenship status.");
			AddRadio(80, 354, 209, 208, false, (int)Buttons.RevokeCitizenshipRadio);
			AddLabel(110, 356, 0, @"Revoke your citizenship.");
			AddLabel(344, 233, 0, @"[relpor");
			AddButton(74, 416, 247, 248, (int)Buttons.Okay, GumpButtonType.Reply, 0);
			AddRadio(80, 382, 209, 208, false, (int)Buttons.HelpInfoRadio);
			AddLabel(110, 384, 0, @"Display guide for additional unique Rel Wis [helpinfo commands.");
			AddButton(733, 25, 1151, 1150, (int)Buttons.CancelX, GumpButtonType.Reply, 0);
			AddButton(147, 416, 243, 242, (int)Buttons.Cancel, GumpButtonType.Reply, 0);
			

            
        }

        		public enum Buttons
		{
			VisitInformationCenter,
			VisitForums,
			VisitFAQ,
			BecomeACitizenRadio,
			CitizenshipStatusRadio,
			RevokeCitizenshipRadio,
			Okay,
			HelpInfoRadio,
			CancelX,
			Cancel,
		}


        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            switch(info.ButtonID)
            {
                case (int)Buttons.VisitInformationCenter:
				{
                    from.LaunchBrowser(@"http://relwis.com/?page_id=406");
					break;
				}
				case (int)Buttons.VisitForums:
				{
                    from.LaunchBrowser(@"http://relwis.com/?post_type=forum");
					break;
				}
				case (int)Buttons.VisitFAQ:
				{
                    from.LaunchBrowser(@"http://relwis.com/?page_id=7");
					break;
				}
			
				case (int)Buttons.Okay:
				{
                    for (int i = 0; i < info.Switches.Length; i++)
                    {
                        int radioButton = info.Switches[i];
                        bool isSelected = info.IsSwitched(radioButton);
                        if (!isSelected)
                            continue;

                        switch (radioButton)
                        {
                            case (int)Buttons.BecomeACitizenRadio:
                                {
                                    Commonwealth.OnCommand_JoinCommonwealth(new CommandEventArgs(from, "jointownship", string.Empty, null));
                                    break;
                                }
                            case (int)Buttons.CitizenshipStatusRadio:
                                {
                                    Commonwealth.OnCommand_CitizenshipStatus(new CommandEventArgs(from, "citizenshipstatus", string.Empty, null));
                                    break;
                                }
                            case (int)Buttons.RevokeCitizenshipRadio:
                                {
                                    Commonwealth.OnCommand_RevokeCitizenship(new CommandEventArgs(from, "revokecitizenship", string.Empty, null));
                                    break;
                                }
                            case (int)Buttons.HelpInfoRadio:
                                {
                                    HelpInfo.HelpInfo_OnCommand(new CommandEventArgs(from, "helpinfo", string.Empty, null));
                                    break;
                                }
                        }
                    }
                    break;
				}
				
				case (int)Buttons.CancelX:
				{
                    from.CloseGump(typeof(RelwisGump));
					break;
				}
				case (int)Buttons.Cancel:
				{
                    from.CloseGump(typeof(RelwisGump));
					break;
				}

            }
        }
    }
}
