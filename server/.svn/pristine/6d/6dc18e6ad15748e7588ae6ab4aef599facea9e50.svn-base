using System;
using System.Collections.Generic;
using System.Text;
using Server.Commands;
using Server.Network;
using Server.Scripts.Custom.WebService;

namespace Server.Gumps
{
    public class FeedbackGump : Gump
    {
        Mobile mCaller;
        object mTarget;
        public FeedbackGump(Mobile from, object target)
            : this()
        {
            mCaller = from;
            mTarget = target;
        }

        public FeedbackGump()
            : base(0, 0)
        {
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;

            AddPage(0);
            AddImage(60, 65, 1140);
            AddButton(90, 275, 242, 243, (int)Buttons.Cancel, GumpButtonType.Reply, 0);
            AddButton(355, 275, 247, 248, (int)Buttons.Okay, GumpButtonType.Reply, 0);
            AddLabel(155, 253, 0x3E9, @"Let us know what you think.");
            AddTextEntry(113, 110, 283, 133, 0, 0, @"");

        }

        public enum Buttons
        {
            Cancel,
            Okay,
        }


        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            switch (info.ButtonID)
            {
                case (int)Buttons.Cancel:
                    {
                        from.CloseGump(typeof(FeedbackGump));
                        break;
                    }
                case (int)Buttons.Okay:
                    {
                        TextRelay entry0 = info.GetTextEntry(0);
                        string text0 = (entry0 == null ? "" : entry0.Text.Trim());
                        XmlUploader uploader = new XmlUploader(FeedbackXmlGenerator.GenerateXmlString(from, mTarget, text0), "http://falling-sunset-5782.herokuapp.com/feedback_posts");
                        break;
                    }

            }
        }
    }
}
