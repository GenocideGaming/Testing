using System;
using System.Collections.Generic;
using Server.Multis.Deeds;

namespace Server.Mobiles
{
    public class SBMinocHouseDeed : SBInfo
    {
        private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();
        public SBMinocHouseDeed()
        {
        }

        public override IShopSellInfo SellInfo
        {
            get
            {
                return m_SellInfo;
            }
        }
        public override List<GenericBuyInfo> BuyInfo
        {
            get
            {
                return m_BuyInfo;
            }
        }

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                Add(new GenericBuyInfo("deed to a Minoc small house", typeof(MinocSmallHouseDeed), 90000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a Minoc villa", typeof(MinocVillaEDeed), 450000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a Minoc loft", typeof(MinocLoftEDeed), 750000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a Minoc large villa", typeof(MinocLargeVillaDeed), 8000000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a Minoc bungalo", typeof(MinocBungaloDeed), 9000000, 20, 0x14F0, 0));

            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {
                Add(typeof(MinocSmallHouseDeed), 72000);
                Add(typeof(MinocVillaEDeed), 360000);
                Add(typeof(MinocLoftEDeed), 600000);
                Add(typeof(MinocLargeVillaDeed), 6400000);
                Add(typeof(MinocBungaloDeed), 7200000);
            }
        }
    }
}
