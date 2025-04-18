using System;
using System.Collections.Generic;
using Server.Multis.Deeds;

namespace Server.Mobiles
{
    public class SBBlackrockHouseDeed : SBInfo
    {
        private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();
        public SBBlackrockHouseDeed()
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
                Add(new GenericBuyInfo("deed to a blackrock small tower", typeof(BlackrockSmallTowerDeedE), 150000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a blackrock scout tower", typeof(BlackrockScoutTowerDeed), 150000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a blackrock tower", typeof(BlackrockTowerDeedE), 250000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a blackrock outpost", typeof(BlackrockOutpostDeed), 550000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a blackrock keep", typeof(BlackrockKeepDeed), 1500000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a blackrock fort", typeof(BlackrockFortDeed), 9000000, 20, 0x14F0, 0));

            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {
                Add(typeof(BlackrockSmallTowerDeedE), 120000);
                Add(typeof(BlackrockScoutTowerDeed), 120000);
                Add(typeof(BlackrockTowerDeedE), 200000);
                Add(typeof(BlackrockOutpostDeed), 440000);
                Add(typeof(BlackrockKeepDeed), 1200000);
                Add(typeof(BlackrockFortDeed), 7200000);

            }
        }
    }
}
