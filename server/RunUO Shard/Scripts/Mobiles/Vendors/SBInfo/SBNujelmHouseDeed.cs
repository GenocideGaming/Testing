using System;
using System.Collections.Generic;
using Server.Multis.Deeds;

namespace Server.Mobiles
{
    public class SBNujelmHouseDeed : SBInfo
    {
        private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();
        public SBNujelmHouseDeed()
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
                Add(new GenericBuyInfo("deed to a Nujelm small house", typeof(NujelmSmallHouseDeedE), 90000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a Nujelm two story balcony", typeof(NujelmTwoStoryBalconyDeed), 150000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a Nujelm villa", typeof(NujelmVillaDeedE), 1000000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a Nujelm courtyard manor", typeof(NujelmCourtyardManorDeed), 8000000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a Nujelm two story ranch house", typeof(NujelmTwoStoryRanchHouseDeed), 9000000, 20, 0x14F0, 0));
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {
                Add(typeof(NujelmSmallHouseDeedE), 72000);
                Add(typeof(NujelmTwoStoryBalconyDeed), 120000);
                Add(typeof(NujelmVillaDeedE), 800000);
                Add(typeof(NujelmCourtyardManorDeed), 6400000);
                Add(typeof(NujelmTwoStoryRanchHouseDeed), 7200000);
            }
        }
    }
}
