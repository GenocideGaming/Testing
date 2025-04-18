using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Scripts.Custom.Citizenship.CommonwealthBonuses
{
    public class CommonwealthTownBonus : CommonwealthBonus
    {
        private TownBonusTypes mBonusType;

        public TownBonusTypes BonusType { get { return mBonusType; } set { mBonusType = value; } }

        public CommonwealthTownBonus(TownBonusTypes bonusType, int pointCost, string dictKey, string displayName) : base(BonusTypes.Town, pointCost, dictKey, displayName)
        {
            mBonusType = bonusType;
        }

        public CommonwealthTownBonus(GenericReader reader) : base(reader)
        {
            int version = reader.ReadInt();
            mBonusType = (TownBonusTypes)reader.ReadInt();
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
            return (CommonwealthTownBonus) this.MemberwiseClone();
        }
    }

    public enum TownBonusTypes
    {
        MinistersGuards = 1,
        DiscplinedGuards = 2,
        WatchfulGuards = 4,
        DiligentMerchants = 8
    }

    public class CommonwealthTownBonuses
    {
        public Dictionary<string, CommonwealthTownBonus> Bonuses;

        public CommonwealthTownBonuses()
        {
            Bonuses = new Dictionary<string, CommonwealthTownBonus>();
            Bonuses.Add("MinistersGuards", new CommonwealthTownBonus(TownBonusTypes.MinistersGuards, 1, "MinistersGuards", "Minister's Guards"));
            Bonuses.Add("DisciplinedGuards", new CommonwealthTownBonus(TownBonusTypes.DiscplinedGuards, 1, "DisciplinedGuards", "Disciplined Guards"));
            Bonuses.Add("WatchfulGuards", new CommonwealthTownBonus(TownBonusTypes.WatchfulGuards, 2, "WatchfulGuards", "Watchful Guards"));
            Bonuses.Add("DiligentMerchants", new CommonwealthTownBonus(TownBonusTypes.DiligentMerchants, 2, "DiligentMerchants", "Diligent Merchants"));
        }
    }
}
