using System;
using System.Collections.Generic;
using System.Text;
using Server.Scripts.Custom.Citizenship;
using Server.Scripts.Custom.Citizenship.CommonwealthBonuses;
using Server.Network;

namespace Server.Gumps
{
    public class TownInfoGump : Gump
    {
        ICommonwealth township;

        public TownInfoGump(Mobile from, ICommonwealth commonwealth)
            : base(100, 100)
        {
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;

            township = commonwealth;
            if (township == null)
                return;

            AddPage(0);
            AddBackground(1, 1, 282, 490, 9250);
            AddLabel(88, 18, 0, @"Town Information");
            AddLabel(39, 406, 0, @"View Election Information");
            AddButton(18, 409, 1210, 1209, (int)Buttons.ElectionButton, GumpButtonType.Reply, 0);
            AddLabel(40, 456, 0, @"Revoke Citizenship");
            AddButton(18, 459, 1210, 1209, (int)Buttons.RevokeButton, GumpButtonType.Reply, 0);
            AddLabel(20, 362, 0, @"Township Population: "+ township.State.Members.Count);
            if (township.Minister != null)
            {
                AddLabel(20, 43, 0, @"Under rule of " + township.Minister.Name);
            }
            else
            {
                AddLabel(20, 43, 0, @"Under rule of no one.");
            }
            AddLabel(20, 68, 0, @"Current Town Bonuses:");
            int y = 0;
            foreach (CommonwealthBonus bonus in township.State.Bonuses)
            {
                AddLabel(20, 95+ y * 20, 0, bonus.DisplayName);
                y++;
            }
            

            AddLabel(20, 319, 0, @"The town hero is currently:");
            string heroPlacement = township.Hero != null ? (township.Hero.IsHome ? "At home" : "Captured in " + township.Hero.Owner.Definition.TownName) : "Gone";
            AddLabel(20, 340, 0, heroPlacement);

            AddButton(18, 434, 1210, 1209, (int)Buttons.JoinButton, GumpButtonType.Reply, 0);
            AddLabel(39, 431, 0, @"Join Township");

            if (township.Minister != null && township.Minister == from)
            {
                AddButton(18, 384, 1210, 1209, (int)Buttons.MinisterExileButton, GumpButtonType.Reply, 0);
                AddLabel(39, 381, 0, @"Minister Menu");
            }
            else
            {
                AddButton(18, 384, 1210, 1209, (int)Buttons.MinisterExileButton, GumpButtonType.Reply, 0);
                AddLabel(39, 381, 0, @"Exile List");
            }

        }

        public enum Buttons
        {
            ElectionButton = 1,
            RevokeButton,
            JoinButton,
            MinisterExileButton,
        }


        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            switch (info.ButtonID)
            {
                case (int)Buttons.ElectionButton:
                    {
                        from.SendGump(new CommonwealthElectionGump(township, from));
                        break;
                    }
                case (int)Buttons.RevokeButton:
                    {
                        from.SendGump(new RevokeCitizenshipGump(from));
                        break;
                    }
                case (int)Buttons.JoinButton:
                    {
                        Commonwealth.OnCommand_JoinCommonwealth(new Commands.CommandEventArgs(from, "jointownship", string.Empty, null));
                        break;
                    }
                case (int)Buttons.MinisterExileButton: //this button has two functions, if a minister calls it's control gump otherwise we show exiles
                    {
                        if (township.IsMinister(from))
                            from.SendGump(new CommonwealthControlGump(from, Commonwealth.Find(from)));
                        else
                            from.SendGump(new ExileGump(from, township));
                        break;
                    }
            }
        }
    }
}
