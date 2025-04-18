using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Scripts.Custom.Citizenship.CommonwealthBonuses
{
    public class CommonwealthResourceBonus : CommonwealthBonus
    {
        private ResourceBonusTypes mResourceType;
        private int mHarvestBonus;

        public ResourceBonusTypes ResourceType { get { return mResourceType; } set { mResourceType = value; } }
        public int HarvestBonus
        {
            get { return mHarvestBonus; } 
            set { mHarvestBonus = value; } 
        }

        public CommonwealthResourceBonus(ResourceBonusTypes resourceType, int harvestBonus, int pointCost, string dictKey, string displayName)
            : base(BonusTypes.Resource, pointCost, dictKey, displayName)
        {
            mResourceType = resourceType;
            mHarvestBonus = harvestBonus;
        }

        public CommonwealthResourceBonus(GenericReader reader)
            : base(reader)
        {
            int version = reader.ReadInt();
            mResourceType = (ResourceBonusTypes)reader.ReadInt();
            mHarvestBonus = reader.ReadInt();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); //version
            writer.Write((int)mResourceType);
            writer.Write((int)mHarvestBonus);
        }
        public override string ToString()
        {
            return base.ToString() + " ResourceType:" + mResourceType.ToString();
        }
        public override CommonwealthBonus GetShallowCopy()
        {
            return (CommonwealthResourceBonus) this.MemberwiseClone();
        }
    }
    public enum ResourceBonusTypes
    {
        Hunting, //shearing, skinning and defeathering
        Adventurer, // increased gold drop
        Fame, //increased fame
        Mining, //increased ore
        Lumberjack, //increased wood
        Fishing,
        Silver

    }

    public class CommonwealthResourceBonuses
    {
        public Dictionary<string, CommonwealthResourceBonus> Bonuses;

        public CommonwealthResourceBonuses()
        {
            Bonuses = new Dictionary<string, CommonwealthResourceBonus>();
            Bonuses.Add("Hunting", new CommonwealthResourceBonus(ResourceBonusTypes.Hunting, FeatureList.Citizenship.HuntingResourceBonus, 1, "Hunting", "Hunting Bonus"));
            Bonuses.Add("Adventurer", new CommonwealthResourceBonus(ResourceBonusTypes.Adventurer, FeatureList.Citizenship.AdventurerResourceBonus, 2, "Adventurer", "Adventurer Bonus"));
            Bonuses.Add("Fame", new CommonwealthResourceBonus(ResourceBonusTypes.Fame, FeatureList.Citizenship.FameResourceBonus, 1, "Fame", "Fame Bonus"));
            Bonuses.Add("Mining", new CommonwealthResourceBonus(ResourceBonusTypes.Mining, FeatureList.Citizenship.MiningResourceBonus, 1, "Mining", "Mining Bonus"));
            Bonuses.Add("Lumberjack", new CommonwealthResourceBonus(ResourceBonusTypes.Lumberjack, FeatureList.Citizenship.LumberjackResourceBonus, 1, "Lumberjack", "Lumberjacking Bonus"));
            Bonuses.Add("Fishing", new CommonwealthResourceBonus(ResourceBonusTypes.Fishing, FeatureList.Citizenship.FishingResourceBonus, 1, "Fishing", "Fishing Bonus"));
            Bonuses.Add("Silver", new CommonwealthResourceBonus(ResourceBonusTypes.Silver, FeatureList.Citizenship.SilverResourceBonus, 2, "Silver", "Silver Bonus"));
        }
    }
}
