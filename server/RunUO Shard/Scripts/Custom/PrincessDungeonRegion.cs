using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Server.Mobiles;
using Server.Scripts.Custom.Citizenship;

namespace Server.Regions
{
    public class PrincessDungeonRegion : BaseRegion
    {
        TownHero mPrincess;
        CaptureTimer mCaptureTimer;
        ICommonwealth mOwner;

        public ICommonwealth Owner { get { return mOwner; } set { mOwner = value; } }

        public PrincessDungeonRegion(XmlElement xml, Map map, Region parent)
            : base(xml, map, parent)
        {
            mPrincess = null;
            XmlElement townshipElement = xml["townshipinfo"];
            string townshipName = string.Empty;
            ReadString(townshipElement, "name", ref townshipName);
            mOwner = Commonwealth.Find(townshipName);
        }

        public override void OnEnter(Mobile m)
        {
            m.SendMessage("Entering a PrincessDungeonRegion");
            TownHero princess = m as TownHero;
            if (princess != null)
            {
                //we only add this princess if we dont have another
                if (mPrincess == null)
                {
                    Console.WriteLine("Princess entering a princessdungeonregion, starting capture timer");
                    mPrincess = princess;
                    mCaptureTimer = new CaptureTimer(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(10), this);
                    mCaptureTimer.Start();
                }
            }
        }

        public override void OnExit(Mobile m)
        {
            m.SendMessage("Exiting a PrincessDungeonRegion");
            TownHero princess = m as TownHero;

            if (princess != null && princess == mPrincess && mCaptureTimer != null)
            {
                mCaptureTimer.Stop();
                mCaptureTimer = null;
                Console.WriteLine("Princess exiting a princessdungeonregion, turning off timer");
                mPrincess = null;
            }
            if (princess != null && princess == mPrincess)
            {
                Console.WriteLine("Princess EXIT with no timer");
                mPrincess = null;
            }
        }

        public void CapturePrincess()
        {
            mCaptureTimer.Stop();
            mCaptureTimer = null;
            mPrincess.OnCapture(mOwner);
        }
        private class CaptureTimer : Timer
        {
            TimeSpan mCaptureTime;
            DateTime mStartTime;
            PrincessDungeonRegion mMyRegion;

            public CaptureTimer(TimeSpan delay, TimeSpan interval, TimeSpan captureTime, PrincessDungeonRegion region)
                : base(delay, interval)
            {
                mCaptureTime = captureTime;
                mStartTime = DateTime.Now;
                mMyRegion = region;
            }

            protected override void OnTick()
            {
                base.OnTick();
                if (mStartTime.Add(DateTime.Now - mStartTime) > mStartTime.Add(mCaptureTime))
                {
                    Console.WriteLine("Princess succesfully captured!");
                    mMyRegion.CapturePrincess();
                }
            }
        }
    }
}
