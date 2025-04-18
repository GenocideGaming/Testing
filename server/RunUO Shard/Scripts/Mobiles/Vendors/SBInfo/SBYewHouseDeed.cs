using System;
using System.Collections.Generic;
using Server.Multis.Deeds;

namespace Server.Mobiles
{
    public class SBYewHouseDeed : SBInfo
    {
        private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();
        public SBYewHouseDeed()
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
                Add(new GenericBuyInfo("deed to a Yew small cabin", typeof(SmallYewCabinDeed), 250000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a Yew log home", typeof(YewLogHomeDeedE), 350000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a Yew residence", typeof(YewResidenceDeedE), 1000000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a Yew veranda", typeof(YewVerandaDeedE), 2350000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a Yew two story residence", typeof(YewTwoStoryResidenceDeedE), 2350000, 20, 0x14F0, 0));
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {
                Add(typeof(SmallYewCabinDeed), 200000);
                Add(typeof(YewLogHomeDeedE), 280000);
                Add(typeof(YewResidenceDeedE), 800000);
                Add(typeof(YewVerandaDeedE), 1880000);
                Add(typeof(YewTwoStoryResidenceDeedE), 1880000);
            }
        }
    }
}
