using System;
using System.Collections.Generic;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.Gumps
{
    class AllowGuildInviteGump : Gump
    {
        Mobile caller;

        public AllowGuildInviteGump(Mobile from)
            : this()
        {
            caller = from;

            PlayerMobile pm = caller as PlayerMobile;
            if(pm != null && pm.AcceptGuildInvites)
                AddCheck(59, 62, 9728, 9729, true, 0);
            else if(pm != null && !pm.AcceptGuildInvites)
                AddCheck(59, 62, 9728, 9729, false, 0);
        }

        public AllowGuildInviteGump()
            : base(100, 100)
        {
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;

            AddPage(0);
            AddBackground(0, 3, 231, 104, 9250);
            
            AddLabel(50, 27, 0, @"Accept guild invites?");
            AddButton(151, 67, 238, 239, 0, GumpButtonType.Reply, 0);
            AddLabel(25, 67, 0, @"Yes");

        }



        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;
            PlayerMobile player = from as PlayerMobile;

            switch (info.ButtonID)
            {
                case 0:
                {
                    for (int i = 0; i < info.Switches.Length; i++)
                    {
                        int checkBox = info.Switches[i];
                        if (info.IsSwitched(checkBox))
                        {

                            if (player != null)
                                player.AcceptGuildInvites = true;
                            return;
                        }
                       
                    }
                    if (player != null)
                        player.AcceptGuildInvites = false;  
                    break;
                }

            }
        }
    }
}
