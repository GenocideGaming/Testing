using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Server.Scripts.Custom.Citizenship;

namespace Server.Regions
{
    
    public class CommonwealthRegion : GuardedRegion
    {
        private ICommonwealth mCommonwealth;
        public ICommonwealth MyCommonwealth { get { return mCommonwealth; } set { mCommonwealth = value; } }
        public CommonwealthRegion(XmlElement xml, Map map, Region parent) : base(xml, map, parent)
		{
            XmlElement townshipNameElement = xml["townshipinfo"];

            string townshipName = string.Empty;
            ReadString(townshipNameElement, "name", ref townshipName);
            mCommonwealth = Commonwealth.Find(townshipName);
            mCommonwealth.TownRegion = this;
		}

        public override bool AllowReds
        {
            get
            {
                int result = (mCommonwealth.State.GuardSettings & (int)GuardTypes.DisciplinedGuards);
                if (result == 0)
                    return true;
                else
                    return false;
            }
        }

        public override void OnEnter(Mobile m)
        {
            base.OnEnter(m);

            if (mCommonwealth.State.ExileList.Contains(m))
            {
                m.SendMessage("You are an exile of this township, you have been flagged as a criminal until you leave.");
                m.InExileRegion = true; //set it up so our criminal timer lasts for 7 days
                m.Criminal = true;
            }
        }

        public override void OnExit(Mobile m)
        {
            base.OnExit(m);

            if (mCommonwealth.State.ExileList.Contains(m))
            {
                m.Criminal = false;
                m.InExileRegion = false; //reset timer to two minutes
                m.Criminal = true;
                m.SendMessage("You have left a township you are exiled from, you will remain a criminal for 2 minutes.");
            }
        }
    }
    
}
