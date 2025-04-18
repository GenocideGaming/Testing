using System;
using Server;
using Server.Mobiles;

namespace Server.Scripts
{
    public class ResignationCheck
    {
        public static void Initialize()
        {
            EventSink.Speech += new SpeechEventHandler(EventSink_Speech);
        }

        private static void EventSink_Speech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!e.Handled && from is PlayerMobile && Insensitive.Contains(e.Speech, FeatureList.BountyHunters.ResignText) )
            {
                PlayerMobile pm = from as PlayerMobile;
                if (pm == null) { return; }

                if (pm.BountyHunter)
                {
                    pm.SendMessage(FeatureList.BountyHunters.ConfirmResignationText);
                    pm.QuittingBountyHuntingAt = DateTime.Now + FeatureList.BountyHunters.ResignDelay; // leave this in just in case the server crashes before the timer goes off
                    Timer.DelayCall(TimeSpan.FromHours(0.25), pm.QuitBountyHunter);
                }
            }
        }


    }
}
