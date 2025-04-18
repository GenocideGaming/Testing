using System;
using System.Collections.Generic;
using System.Text;
using Server.Network;
using Server.Scripts.Custom.Citizenship;

namespace Server.Gumps
{
    public class RevokeCitizenshipGump : Gump
    {
        Mobile caller;

        public RevokeCitizenshipGump(Mobile from)
            : base(100, 100)
        {
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;

            caller = from;
            if (Commonwealth.Find(from) == null)
                return;

            AddPage(0);
            AddBackground(0, 3, 282, 115, 9250);
            AddLabel(69, 19, 0, @"Are you sure you wish to");
            AddLabel(66, 38, 0, @" revoke your citizenship?");
            AddButton(188, 78, 247, 248, (int)Buttons.OkayButton, GumpButtonType.Reply, 0);
            AddButton(34, 78, 241, 243, (int)Buttons.CancelButton, GumpButtonType.Reply, 0);



        }

        public enum Buttons
        {
            OkayButton = 1,
            CancelButton,
        }


        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            switch (info.ButtonID)
            {
                case (int)Buttons.OkayButton:
                    {
                        Commonwealth.OnCommand_RevokeCitizenship(new Commands.CommandEventArgs(from, "revokecitizenship", string.Empty, null));
                        break;
                    }
                case (int)Buttons.CancelButton:
                    {

                        break;
                    }

            }
        }
    }
}