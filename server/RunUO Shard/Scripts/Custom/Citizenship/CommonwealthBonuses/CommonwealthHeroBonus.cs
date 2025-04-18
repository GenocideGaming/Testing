using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Scripts.Custom.Citizenship.CommonwealthBonuses
{
    public class CommonwealthHeroBonus : CommonwealthBonus
    {
        private HeroBonusTypes mBonusType;

        public HeroBonusTypes BonusType { get { return mBonusType; } set { mBonusType = value; } }

        public CommonwealthHeroBonus(HeroBonusTypes bonusType, int pointCost, string dictKey, string displayName)
            : base(BonusTypes.Hero, pointCost, dictKey, displayName)
        {
            mBonusType = bonusType;
        }

        public CommonwealthHeroBonus(GenericReader reader)
            : base(reader)
        {
            int version = reader.ReadInt();
            mBonusType = (HeroBonusTypes)reader.ReadInt();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write((int)mBonusType);
        }

        public override string ToString()
        {
            return mBonusType.ToString();
        }

        public override CommonwealthBonus GetShallowCopy()
        {
            return (CommonwealthHeroBonus) this.MemberwiseClone();
        }
    }
    public enum HeroBonusTypes
    {
        Defiance = 1,
        SiegeTools = 2,
        Intimidation = 4,
        StrongDoors = 8
    }

    public class CommonwealthHeroBonuses
    {
        public Dictionary<string, CommonwealthHeroBonus> Bonuses;

        public CommonwealthHeroBonuses()
        {
            Bonuses = new Dictionary<string, CommonwealthHeroBonus>();
            Bonuses.Add("Defiance", new CommonwealthHeroBonus(HeroBonusTypes.Defiance, 2, "Defiance", "Defiance Bonus"));
            Bonuses.Add("SiegeTools", new CommonwealthHeroBonus(HeroBonusTypes.SiegeTools, 2, "SiegeTools", "Siege Tools Bonus"));
            Bonuses.Add("Intimidation", new CommonwealthHeroBonus(HeroBonusTypes.Intimidation, 2, "Intimidation", "Intimidation Bonus"));
            Bonuses.Add("StrongDoors", new CommonwealthHeroBonus(HeroBonusTypes.StrongDoors, 2, "StrongDoors", "Strong Doors Bonus"));
        }
    }
}
