using System;
using System.Collections.Generic;
using System.Text;
using Server.Factions;
using Server.Mobiles;
using Server.Scripts.Custom.Citizenship.CommonwealthBonuses;
using System.IO;
using Server.Scripts.Custom.WebService;
using System.Timers;

namespace Server.Scripts.Custom.Citizenship
{
    public class CommonwealthState
    {
        private ICommonwealth mCommonwealth;
        private Mobile mMinister;
        //TODO Bonuses
        private List<PlayerCitizenshipState> mMembers;
        private CommonwealthElection mElection;
        private List<CommonwealthBonus> mBonuses;
        private int mGuardSetting;

        private int mExileCredits;
        private List<Mobile> mExileList;

        private VendorLevels mVendorLevel;
        private int mHeroSetting;

        private int mAvailablePoints; //the bonus points left to use
        private const int mTotalPoints = 12;

        private TownHero mHero;
        private List<TownHero> mCapturedHeroes;

        public Mobile Minister 
        {
            get
            {
                return mMinister;
            }
            set
            {
                if (mMinister != null)
                {
                    mMinister.InvalidateProperties();
                }
                mMinister = value;

                if (mMinister != null)
                {
                    mMinister.SendMessage("You have been elected mayor of your town!");

                    mMinister.InvalidateProperties();
                }
            }
        }

        public List<PlayerCitizenshipState> Members { get { return mMembers; } set { mMembers = value; } }
        public CommonwealthElection Election { get { return mElection; } set { mElection = value; } }
        public List<CommonwealthBonus> Bonuses { get { return mBonuses; } }
        public int GuardSettings { get { return mGuardSetting; } set { mGuardSetting = value; } }
        public int HeroSettings { get { return mHeroSetting; } set { mHeroSetting = value; } }
        public VendorLevels VendorLevel { get { return mVendorLevel; } set { mVendorLevel = value; } }
        public int ExileCredits { get { return mExileCredits; } set { mExileCredits = value; } }
        public List<Mobile> ExileList { get { return mExileList; } set { mExileList = value; } }
        public int AvailablePoints { get { return mAvailablePoints; } set { mAvailablePoints = value; } }

        public TownHero Hero { get { return mHero; } set { mHero = value; } }
        public List<TownHero> CapturedHeroes { get { return mCapturedHeroes; } set { mCapturedHeroes = value; } }

        public CommonwealthState(Commonwealth citizenship)
        {
            mCommonwealth = citizenship;
            mMembers = new List<PlayerCitizenshipState>();
            mBonuses = new List<CommonwealthBonus>();
            mAvailablePoints = mTotalPoints;
            mElection = new CommonwealthElection(citizenship);
            mExileList = new List<Mobile>();
            mCapturedHeroes = new List<TownHero>();
        }

        public CommonwealthState(GenericReader reader)
        {
            int version = reader.ReadEncodedInt();
            switch (version)
            {
                case 1:
                    bool hasMinister = reader.ReadBool();
                    if (hasMinister)
                        mMinister = reader.ReadMobile();
                    goto case 0;
                case 0:
                    mElection = new CommonwealthElection(reader);
                    mCommonwealth = Commonwealth.ReadReference(reader);
            
                    int memberCount = reader.ReadEncodedInt();
                    mMembers = new List<PlayerCitizenshipState>();

                    for (int i = 0; i < memberCount; i++)
                    {
                        PlayerCitizenshipState pcs = new PlayerCitizenshipState(reader, mCommonwealth);

                        if (pcs.Mobile != null)
                        {
                            mMembers.Add(pcs);
                            PlayerMobile pm = pcs.Mobile as PlayerMobile;
                            if(pm != null)
                                pm.CitizenshipPlayerState = pcs;
                        }
                    }

                    int bonusCount = reader.ReadEncodedInt();
                    mBonuses = new List<CommonwealthBonus>();
                    mAvailablePoints = mTotalPoints;
                    for (int i = 0; i < bonusCount; i++)
                    {
                        int bonusType = reader.ReadEncodedInt();
                        switch (bonusType)
                        {
                            case (int)BonusTypes.Skill:
                                CommonwealthSkillBonus csb = new CommonwealthSkillBonus(reader);
                                mBonuses.Add(csb);
                                mAvailablePoints -= csb.PointCost;
                                break;
                            case (int)BonusTypes.Town:
                                CommonwealthTownBonus ctb = new CommonwealthTownBonus(reader);
                                mBonuses.Add(ctb);
                                mAvailablePoints -= ctb.PointCost; 
                                break;
                            case (int)BonusTypes.Combat:
                                CommonwealthCombatBonus ccb = new CommonwealthCombatBonus(reader);
                                mBonuses.Add(ccb);
                                mAvailablePoints -= ccb.PointCost;
                                break;
                            case (int)BonusTypes.Resource:
                                CommonwealthResourceBonus crb = new CommonwealthResourceBonus(reader);
                                mBonuses.Add(crb);
                                mAvailablePoints -= crb.PointCost;
                                break;
                            case (int)BonusTypes.Hero:
                                CommonwealthHeroBonus chb = new CommonwealthHeroBonus(reader);
                                mBonuses.Add(chb);
                                mAvailablePoints -= chb.PointCost;
                                break;
                        }
                
                    }
                    mGuardSetting = reader.ReadEncodedInt();
                    mVendorLevel = (VendorLevels)reader.ReadEncodedInt();

                    mExileCredits = reader.ReadEncodedInt();

                    mAvailablePoints -= (mExileCredits - 1) >= 0 ? mExileCredits -1 : 0; //we remove the credits spent on exiling, except for the first one for purchasing the bonus

                    int exileCount = reader.ReadEncodedInt();
                    mExileList = new List<Mobile>();
                    for (int i = 0; i < exileCount; i++)
                    {
                        mExileList.Add(reader.ReadMobile());
                    }

                    mHeroSetting = reader.ReadEncodedInt();

                    mHero = reader.ReadMobile() as TownHero;

                    int capturedHeroCount = reader.ReadEncodedInt();
                    mCapturedHeroes = new List<TownHero>();
                    for (int i = 0; i < capturedHeroCount; i++)
                    {
                        mCapturedHeroes.Add((TownHero)reader.ReadMobile());
                    }
                    mCommonwealth.State = this;
                    break;
            }
            
           
        }

        public void Serialize(GenericWriter writer)
        {
            writer.WriteEncodedInt((int)1); //version
            //version 1
            if (mMinister != null)
            {
                writer.Write((bool)true);
                writer.Write((Mobile)mMinister);
            }
            else
                writer.Write((bool)false);
            //version 0
            mElection.Serialize(writer);
            Commonwealth.WriteReference(writer, mCommonwealth);
            writer.WriteEncodedInt((int)mMembers.Count);
            for (int i = 0; i < mMembers.Count; i++)
            {
                PlayerCitizenshipState pcs = (PlayerCitizenshipState)mMembers[i];
                pcs.Serialize(writer);
            }

            writer.WriteEncodedInt((int)mBonuses.Count);
            for (int i = 0; i < mBonuses.Count; i++)
            {
                writer.WriteEncodedInt((int)mBonuses[i].Type);
                mBonuses[i].Serialize(writer);
            }

            writer.WriteEncodedInt((int)mGuardSetting);
            writer.WriteEncodedInt((int)mVendorLevel);

            writer.WriteEncodedInt((int)mExileCredits);
            writer.WriteEncodedInt((int)mExileList.Count);
            for (int i = 0; i < mExileList.Count; i++)
            {
                writer.Write((Mobile)mExileList[i]);
            }

            writer.WriteEncodedInt((int)mHeroSetting);

            if (mHero == null)
            {
                LogHeroNullError(mCommonwealth);
                World.Broadcast(0, false, "Couldn't find hero of "+ mCommonwealth.Definition.TownName + " please report this to a member of staff.");
            }

            writer.Write((Mobile)mHero);

            writer.WriteEncodedInt((int)mCapturedHeroes.Count);
            foreach (TownHero hero in mCapturedHeroes)
            {
                writer.Write((Mobile)hero);
            }
        }

        public bool AddNewBonus(CommonwealthBonus cb)
        {
            mAvailablePoints -= cb.PointCost;
            if (mAvailablePoints < 0)
            {
                mAvailablePoints += cb.PointCost;
                return false; //we failed
            }
            else
            {
                if (!Bonuses.Contains(cb))
                {
                    Bonuses.Add(cb.GetShallowCopy());
                    CommonwealthTownBonus townBonus = cb as CommonwealthTownBonus;
                    if (townBonus != null)
                    {
                        if (townBonus.BonusType == TownBonusTypes.MinistersGuards)
                        {
                            mExileCredits = 1;
                            mGuardSetting |= (int)GuardTypes.MinistersGuards;
                        }
                        else if (townBonus.BonusType == TownBonusTypes.DiscplinedGuards)
                        {
                            mGuardSetting |= (int)GuardTypes.DisciplinedGuards;
                        }
                        else if (townBonus.BonusType == TownBonusTypes.WatchfulGuards)
                        {
                            mGuardSetting |= (int)GuardTypes.WatchfulGuards;
                        }
                        else
                        {
                            mVendorLevel = VendorLevels.DiligentMerchants;
                        }
                    }

                    CommonwealthHeroBonus heroBonus = cb as CommonwealthHeroBonus;
                    if (heroBonus != null)
                    {
                        if (heroBonus.BonusType == HeroBonusTypes.Defiance)
                        {
                            mHeroSetting |= (int)HeroBonusTypes.Defiance;
                        }
                        else if (heroBonus.BonusType == HeroBonusTypes.SiegeTools)
                        {
                            mHeroSetting |= (int)HeroBonusTypes.SiegeTools;
                        }
                        else if (heroBonus.BonusType == HeroBonusTypes.Intimidation)
                        {
                            mHeroSetting |= (int)HeroBonusTypes.Intimidation;
                        }
                        else if (heroBonus.BonusType == HeroBonusTypes.StrongDoors)
                        {
                            mHeroSetting |= (int)HeroBonusTypes.StrongDoors;
                        }
                    }
                    return true;
                }
                return false;
            }
            
        }

        public void ClearBonuses()
        {
            mBonuses.Clear();
            mAvailablePoints = mTotalPoints;
            mGuardSetting = 0;
            mHeroSetting = 0;
        }

        public bool AddNewBonusSet(List<CommonwealthBonus> bonuses)
        {
            foreach (CommonwealthBonus cb in bonuses)
            {
                if (!AddNewBonus(cb))
                    return false;
            }

            //DatabaseController.UpdateTownshipBonuses(mCommonwealth);

            return true;
        }

        public void LogHeroNullError(ICommonwealth cw)
        {
            using (StreamWriter writer = new StreamWriter("heronulllog.txt", true))
            {
                string message = "A hero is null upon saving for "+ cw.Definition.TownName + " at "+ DateTime.Now;
                writer.WriteLine(message);
            }
        }
    }
}
