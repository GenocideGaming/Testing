using System;
using System.Collections.Generic;
using Server.Multis.Deeds;

namespace Server.Mobiles
{
    public class SBSkaraHouseDeed : SBInfo
    {
        private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();
        public SBSkaraHouseDeed()
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
                Add(new GenericBuyInfo("deed to a skara small shoppe", typeof(SkaraSmallShoppeDeed), 250000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a skara veranda", typeof(SkaraVerandaEDeed), 350000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a skara simple house", typeof(SkaraSimpleHouseDeed), 450000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a skara ranch house", typeof(SkaraRanchHouseEDeed), 850000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a skara adobe", typeof(SkaraAdobeDeed), 1000000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a skara courtyard shoppe", typeof(SkaraCourtyardShoppeDeed), 1500000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a skara courtyard house", typeof(SkaraCourtyardHouseDeed), 1500000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a skara manor", typeof(SkaraManorDeed), 1650000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a skara two story ranch house", typeof(SkaraTwoStoryRanchHouseDeed), 8000000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a skara villa", typeof(SkaraVillaEDeed), 8000000, 20, 0x14F0, 0));

            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {
                Add(typeof(SkaraSmallShoppeDeed), 200000);
                Add(typeof(SkaraVerandaEDeed), 280000);
                Add(typeof(SkaraSimpleHouseDeed), 360000);
                Add(typeof(SkaraRanchHouseEDeed), 680000);
                Add(typeof(SkaraAdobeDeed), 800000);
                Add(typeof(SkaraCourtyardShoppeDeed), 1200000);
                Add(typeof(SkaraCourtyardHouseDeed), 1200000);
                Add(typeof(SkaraManorDeed), 1320000);
                Add(typeof(SkaraTwoStoryRanchHouseDeed), 6400000);
                Add(typeof(SkaraVillaEDeed), 6400000);
            }
        }
    }
}
