using System;
using System.Collections.Generic;
using System.Text;
using Server.Commands;
using Server.Network;
using Server.Scripts.Custom;
using Server.Scripts.Custom.WebService;

namespace Server.Gumps
{
    public class RenamePlayerPrompt : Gump
    {
        private Mobile caller;

        public static void Initialize()
        {

            CommandSystem.Register("renamegump", AccessLevel.Administrator, new CommandEventHandler(renamegump_OnCommand));
        }

        [Usage("renamegump")]
        [Description("Makes a call to your custom gump.")]
        public static void renamegump_OnCommand(CommandEventArgs e)
        {

            if (e.Mobile.HasGump(typeof(PlayerRenameGump)))
                e.Mobile.CloseGump(typeof(PlayerRenameGump));
            e.Mobile.SendGump(new PlayerRenameGump(e.Mobile));
        }
        
        public RenamePlayerPrompt(Mobile from)
            : this()
        {
            caller = from;
        }

        public RenamePlayerPrompt()
            : base(0, 0)
        {
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;

            AddPage(0);
            AddBackground(0, 0, 336, 120, 9200);
            AddLabel(10, 10, 0, @"");
            AddLabel(10, 25, 0, @"");
            AddTextEntry(125, 50, 200, 20, 0, 0, @"");
            AddLabel(10, 50, 0, @"Enter name here:");
            AddButton(266, 80, 247, 248, (int)Buttons.Button1, GumpButtonType.Reply, 0);


        }

        public enum Buttons
        {
            Button1 = 1,
        }


        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            TextRelay entry0 = info.GetTextEntry(0);
            string text0 = (entry0 == null ? "" : entry0.Text.Trim());
            bool goodName = false;
            switch (info.ButtonID)
            {
                case (int)Buttons.Button1:
                    {
                        goodName = UniqueNameChecker.CheckName(from, text0);
                        break;
                    }
           
            }

            if (!goodName)
            {
                from.CloseGump(typeof(PlayerRenameGump));
                from.SendGump(new PlayerRenameGump(from));

            }
            else
            {
                from.SendMessage("You have successfully renamed your character!");
            }
        }
    }
}
