using System;
using System.Collections.Generic;
using System.Text;
using Server.Targeting;
using Server.Gumps;

namespace Server.Commands
{
    public class Feedback
    {
        public static void Initialize()
        {
            CommandSystem.Register("feedback", AccessLevel.Player, new CommandEventHandler(OnCommand_Feedback));
        }

        public static void OnCommand_Feedback(CommandEventArgs e)
        {
            e.Mobile.SendMessage("Target the object you wish to give feedback on, or cancel your target to report general feedback. Thanks!");
            e.Mobile.Target = new FeedbackTarget();
        }

        private class FeedbackTarget : Target
        {
            public FeedbackTarget() : base(10, true, TargetFlags.None) { }

            protected override void OnTargetCancel(Mobile from, TargetCancelType cancelType)
            {
                base.OnTargetCancel(from, cancelType);
                from.SendGump(new FeedbackGump(from, null));
            }
            protected override void OnTarget(Mobile from, object targeted)
            {
                base.OnTarget(from, targeted);

                from.SendGump(new FeedbackGump(from, targeted));
            }
        }
    }
}
