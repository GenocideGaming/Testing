using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Scripts.Custom.Citizenship.CommonwealthBonuses
{
    public class CommonwealthBonus
    {
        protected BonusTypes mType;
        protected int mPointCost;
        protected string mDictKey;
        protected string mDisplayName;

        public BonusTypes Type { get { return mType; } set { mType = value; } }
        public int PointCost { get { return mPointCost; } set { mPointCost = value; } }
        public string DictKey { get { return mDictKey; } set { mDictKey = value; } }
        public string DisplayName { get { return mDisplayName; } set { mDisplayName = value; } }

        public CommonwealthBonus(BonusTypes type, int pointCost, string dictKey, string displayName)
        {
            mType = type;
            mPointCost = pointCost;
            mDictKey = dictKey;
            mDisplayName = displayName;
        }
        public CommonwealthBonus(GenericReader reader)
        {
            int version = reader.ReadInt(); //version
            switch(version)
            {
                case 1:
                    mDisplayName = reader.ReadString();
                    goto case 0;
                case 0:
                    int type = reader.ReadInt();
                    mType = (BonusTypes)type;
                    mPointCost = reader.ReadInt();
                    mDictKey = reader.ReadString();
                    break;
            }
        }
        public virtual void Serialize(GenericWriter writer)
        {
            writer.Write((int)1); //version

            writer.Write((string)mDisplayName);

            writer.Write((int)mType);
            writer.Write((int)mPointCost);
            writer.Write((string)mDictKey);
        }
        public override string ToString()
        {
            return "Type:"+mType.ToString();
        }
        public virtual CommonwealthBonus GetShallowCopy()
        {
            return (CommonwealthBonus) this.MemberwiseClone();
        }
    }
    public enum BonusTypes
    {
        Town,
        Resource,
        Skill,
        Combat,
        Hero,
    }
}
