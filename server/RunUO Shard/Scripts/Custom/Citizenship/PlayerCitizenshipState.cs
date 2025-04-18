using System;
using System.Collections.Generic;
using System.Text;
using Server.Factions;
using Server.Mobiles;
using Server.Scripts.Custom.Citizenship.CommonwealthBonuses;

namespace Server.Scripts.Custom.Citizenship
{
    public class PlayerCitizenshipState : IComparable
    {
        private Mobile mMobile;
        private ICommonwealth mCommonwealth;
        private Town mMayor;
        private bool mLockedToCommonwealth; //if this is true we are locked to this commonwealth for a few days, but not actually a citizen
        private DateTime mLeftTownTime; //The datetime that the player left the commonwealth
        private List<CommonwealthSkillBonus> mSkillBonuses;
        private List<CommonwealthResourceBonus> mResourceBonuses;
        private List<CommonwealthCombatBonus> mCombatBonuses;
        private int mHeroSettings;

        public Mobile Mobile { get { return mMobile; }}
        public ICommonwealth Commonwealth { get { return mCommonwealth; } set { mCommonwealth = value; } }
        public Town Mayor { get { return mMayor; } set { mMayor = value; Invalidate(); } }
        public DateTime LeftTownTime { get { return mLeftTownTime; } set { mLeftTownTime = value; } }
        public bool IsLeaving 
        { 
            get 
            { 
                //We check if the time is between Now - (2 days + buffer) to a bit after Now in case they are just waiting for the update timer to tick
                return mLeftTownTime >= DateTime.Now.Subtract(TimeSpan.FromHours(FeatureList.Citizenship.LeaveTimeInHours + 1.0)) 
                    && mLeftTownTime <= DateTime.Now.AddHours(1.0); 
            } 
        }
        public bool LockedToCommonwealth { get { return mLockedToCommonwealth; } set { mLockedToCommonwealth = value; } }
        public List<CommonwealthSkillBonus> SkillBonuses { get { return mSkillBonuses; } set { mSkillBonuses = value; } }
        public List<CommonwealthResourceBonus> ResourceBonuses { get { return mResourceBonuses; } set { mResourceBonuses = value; } }
        public List<CommonwealthCombatBonus> CombatBonuses { get { return mCombatBonuses; } set { mCombatBonuses = value; } }
        public int HeroSettings { get { return mHeroSettings; } set { mHeroSettings = value; } }

        public PlayerCitizenshipState(Mobile mob, ICommonwealth commonwealth)
        {
            mMobile = mob;
            mCommonwealth = commonwealth;
            mLeftTownTime = DateTime.MinValue;
            mLockedToCommonwealth = false;

            mSkillBonuses = commonwealth.GetSkillBonuses();
            mResourceBonuses = commonwealth.GetResourceBonuses();
            mCombatBonuses = commonwealth.GetCombatBonuses();
            mHeroSettings = commonwealth.GetHeroSettings();

            Attach();
            Invalidate();
        }
        public PlayerCitizenshipState(GenericReader reader, ICommonwealth commonwealth)
        {
            mCommonwealth = commonwealth;
            int version = reader.ReadEncodedInt();
            mMobile = reader.ReadMobile();
            mLeftTownTime = reader.ReadDateTime();
            mLockedToCommonwealth = reader.ReadBool();
            mSkillBonuses = mCommonwealth.GetSkillBonuses();
            mResourceBonuses = mCommonwealth.GetResourceBonuses();
            mCombatBonuses = mCommonwealth.GetCombatBonuses();
            mHeroSettings = mCommonwealth.GetHeroSettings();

            Attach();
            Invalidate();
        }
        public void Attach()
        {
            if (mMobile is PlayerMobile)
                ((PlayerMobile)mMobile).CitizenshipPlayerState = this;
        }
        private void Invalidate()
        {
            if (mMobile is PlayerMobile)
            {
                PlayerMobile pm = (PlayerMobile)mMobile;
                pm.InvalidateProperties();
                pm.InvalidateMyRunUO();
            }
        }

        public static PlayerCitizenshipState Find(Mobile mob)
        {
            if (mob is PlayerMobile)
                return ((PlayerMobile)mob).CitizenshipPlayerState;

            return null;
        }

        public void Serialize(GenericWriter writer)
        {
            writer.WriteEncodedInt((int)0); //version
            writer.Write((Mobile)mMobile);
            writer.Write((DateTime)mLeftTownTime);
            writer.Write((bool)mLockedToCommonwealth);
        }

        public int CompareTo(object obj)
        {
            return 0; //NOT IMPLEMENTED YET
        }
        public void UpdateBonuses(List<CommonwealthResourceBonus> resourceBonuses, List<CommonwealthSkillBonus> skillBonuses, List<CommonwealthCombatBonus> combatBonuses, int heroSettings)
        {
            mResourceBonuses.Clear();
            mSkillBonuses.Clear();
            mCombatBonuses.Clear();

            mResourceBonuses = resourceBonuses;
            mSkillBonuses = skillBonuses;
            mCombatBonuses = combatBonuses;
            mHeroSettings = heroSettings;

            mMobile.SendMessage("Your township has received new bonuses!");
        }
        public CommonwealthResourceBonus GetResourceBonus(ResourceBonusTypes type)
        {
            foreach (CommonwealthResourceBonus crb in ResourceBonuses)
            {
                if (crb.ResourceType == type)
                {
                    //Do a random roll 1/4 to see if we get this bonus
                    int rand = Utility.Random(FeatureList.Citizenship.ResourceRollMax);
                    if (rand == 0)
                        return crb;
                    else
                        return null;
                }
            }

            return null;
        }

        public CommonwealthCombatBonus GetCombatBonus(CombatBonusTypes type)
        {
            foreach (CommonwealthCombatBonus ccb in CombatBonuses)
            {
              
                if (ccb.CombatType == type)
                {
                    return ccb;
                }
            }
            return null;
        }
    }
}
