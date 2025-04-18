using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Scripts.Custom.Citizenship.CommonwealthBonuses
{
    public class CommonwealthSkillBonus : CommonwealthBonus
    {
        private SkillBonusTypes mBonusType;

        private List<SkillName> mSkillNames;
        private float mChanceBonus;

        public SkillBonusTypes BonusType { get { return mBonusType; } set { mBonusType = value; } }
        public List<SkillName> SkillNames { get { return mSkillNames; } set { mSkillNames = value; } }
        public float ChanceBonus { get { return mChanceBonus; } set { mChanceBonus = value; } }

        public CommonwealthSkillBonus(SkillBonusTypes bonusType, List<SkillName> skillNames, int pointCost, float chanceBonus, string dictKey, string displayName) :
            base(BonusTypes.Skill, pointCost, dictKey, displayName)
        {
            mSkillNames = skillNames;
            mChanceBonus = chanceBonus;
        }

        public CommonwealthSkillBonus(GenericReader reader)
            : base(reader)
        {
            int version = reader.ReadInt();
            int skillCount = reader.ReadInt();

            mSkillNames = new List<SkillName>();

            for (int i = 0; i < skillCount; i++)
            {
                mSkillNames.Add((SkillName)reader.ReadInt());
            }

            mChanceBonus = reader.ReadFloat();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); //version

            writer.Write((int)mSkillNames.Count);

            for (int i = 0; i < mSkillNames.Count; i++)
            {
                writer.Write((int)mSkillNames[i]);
            }

            writer.Write((float)mChanceBonus);
        }

        public override string ToString()
        {
            return base.ToString() + " SkillType:" + mDictKey;
        }

        public override CommonwealthBonus GetShallowCopy()
        {
            return (CommonwealthSkillBonus) this.MemberwiseClone();
        }
    }

    public enum SkillBonusTypes
    {
        Alchemy = 1,
        Archery = 2,
        Smithing = 4,
        Warrior = 8,
        Carpentry = 16,
        Tailoring = 32,
        Tinkering = 64,
        Mage = 128,
        Thieving = 256,
        Bard = 512,
        Poisoning = 1024,
        Scribe = 2048,
        Tamer = 4096
    }
    //my intention was to make this class static, but you need .net 3.5 to initialize dictionaries for that, so instead commonwealth keeps a class of all the skillbonuses
    public class CommonwealthSkillBonuses
    {

        public Dictionary<string, CommonwealthSkillBonus> Bonuses;

        public CommonwealthSkillBonuses()
        {
            Bonuses = new Dictionary<string, CommonwealthSkillBonus>();
            Bonuses.Add("Alchemy",new CommonwealthSkillBonus(SkillBonusTypes.Alchemy, new List<SkillName>(new SkillName[]{ SkillName.Alchemy }), 2, FeatureList.Citizenship.SkillChanceBonus, "Alchemy", "Alchemy Bonus"));
            Bonuses.Add("Archery", new CommonwealthSkillBonus(SkillBonusTypes.Archery, new List<SkillName>(new SkillName[]{ SkillName.Archery }), 1, FeatureList.Citizenship.SkillChanceBonus, "Archery", "Archery Bonus"));
            Bonuses.Add("Smithing", new CommonwealthSkillBonus(SkillBonusTypes.Smithing, new List<SkillName>(new SkillName[]{ SkillName.Blacksmith }), 3, FeatureList.Citizenship.SkillChanceBonus, "Smithing", "Smithing Bonus"));
            Bonuses.Add("Warrior",  new CommonwealthSkillBonus(SkillBonusTypes.Warrior, new List<SkillName>(new SkillName[]{ SkillName.Fencing, SkillName.Macing, SkillName.Swords, SkillName.Parry, SkillName.Wrestling }), 2, FeatureList.Citizenship.SkillChanceBonus, "Warrior", "Warrior Bonus"));
            Bonuses.Add("Carpentry", new CommonwealthSkillBonus(SkillBonusTypes.Carpentry, new List<SkillName>(new SkillName[]{ SkillName.Carpentry}), 2, FeatureList.Citizenship.SkillChanceBonus, "Carpentry", "Carpentry Bonus"));
            Bonuses.Add("Tailoring", new CommonwealthSkillBonus(SkillBonusTypes.Tailoring, new List<SkillName>(new SkillName[] { SkillName.Tailoring }), 2, FeatureList.Citizenship.SkillChanceBonus, "Tailoring", "Tailoring Bonus"));
            Bonuses.Add("Tinkering", new CommonwealthSkillBonus(SkillBonusTypes.Tinkering, new List<SkillName>(new SkillName[] { SkillName.Tinkering }), 1, FeatureList.Citizenship.SkillChanceBonus, "Tinkering", "Tinkering Bonus"));
            Bonuses.Add("Mage", new CommonwealthSkillBonus(SkillBonusTypes.Mage, new List<SkillName>(new SkillName[] { SkillName.Magery, SkillName.MagicResist, SkillName.Meditation, SkillName.EvalInt }), 3, FeatureList.Citizenship.SkillChanceBonus, "Mage", "Mage Bonus"));
            Bonuses.Add("Thieving", new CommonwealthSkillBonus(SkillBonusTypes.Thieving, new List<SkillName>(new SkillName[]{SkillName.Snooping, SkillName.Stealing }), 2, FeatureList.Citizenship.SkillChanceBonus, "Thieving", "Thieving Bonus"));
            Bonuses.Add("Bard", new CommonwealthSkillBonus(SkillBonusTypes.Bard, new List<SkillName>(new SkillName[]{ SkillName.Peacemaking, SkillName.Provocation }), 2, FeatureList.Citizenship.SkillChanceBonus, "Bard", "Bard Bonus"));
            Bonuses.Add("Poisoning", new CommonwealthSkillBonus(SkillBonusTypes.Poisoning, new List<SkillName>(new SkillName[]{ SkillName.Poisoning }), 1, FeatureList.Citizenship.SkillChanceBonus, "Poisoning", "Poisoning Bonus"));
            Bonuses.Add("Scribe", new CommonwealthSkillBonus(SkillBonusTypes.Scribe, new List<SkillName>(new SkillName[] { SkillName.Inscribe }), 2, FeatureList.Citizenship.SkillChanceBonus, "Scribe", "Inscription Bonus"));
            Bonuses.Add("Tamer", new CommonwealthSkillBonus(SkillBonusTypes.Tamer, new List<SkillName>(new SkillName[] { SkillName.AnimalTaming, SkillName.AnimalLore }), 2, FeatureList.Citizenship.SkillChanceBonus, "Tamer", "Taming Bonus"));
        }
     
    }
}
