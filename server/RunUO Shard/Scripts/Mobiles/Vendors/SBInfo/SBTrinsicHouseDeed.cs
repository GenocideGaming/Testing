using System;
using System.Collections.Generic;
using Server.Multis.Deeds;

namespace Server.Mobiles
{
    public class SBTrinsicHouseDeed : SBInfo
    {
        private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();
        public SBTrinsicHouseDeed()
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
                Add(new GenericBuyInfo("deed to a Trinsic shoppe", typeof(TrinsicShopDeed), 350000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a Trinsic villa", typeof(TrinsicVillaDeed), 650000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a Trinsic patio residence", typeof(TrinsicPatioResidenceDeedE), 750000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a Trinsic patio villa", typeof(TrinsicPatioVillaDeed), 750000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a Trinsic fortified villa", typeof(TrinsicFortifiedVillaDeedE), 8000000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a Trinsic courtyard house", typeof(TrinsicCourtyardHouseDeed), 8000000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a Trinsic manor", typeof(TrinsicManorDeed), 9000000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a Trinsic keep", typeof(TrinsicKeepDeed), 12000000, 20, 0x14F0, 0));
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {
                Add(typeof(TrinsicShopDeed), 280000);
                Add(typeof(TrinsicVillaDeed), 520000);
                Add(typeof(TrinsicPatioResidenceDeedE), 600000);
                Add(typeof(TrinsicPatioVillaDeed), 600000);
                Add(typeof(TrinsicFortifiedVillaDeedE), 6400000);
                Add(typeof(TrinsicCourtyardHouseDeed), 6400000);
                Add(typeof(TrinsicManorDeed), 7200000);
                Add(typeof(TrinsicKeepDeed), 9600000);
            }
        }
    }
}
