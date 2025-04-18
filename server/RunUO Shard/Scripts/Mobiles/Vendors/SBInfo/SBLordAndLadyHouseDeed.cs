using System;
using System.Collections.Generic;
using Server.Multis.Deeds;

namespace Server.Mobiles
{
    public class SBLordAndLadyHouseDeed : SBInfo
    {
        private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();
        public SBLordAndLadyHouseDeed()
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
                Add(new GenericBuyInfo("deed to a lord of skara compound", typeof(SkaraLordCompoundDeed), 24000000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a lord of trinsic fortress", typeof(TrinsicLordFortressDeed), 24000000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a lord of yew farmstead", typeof(YewLordFarmsteadDeed), 24000000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a lord of wind compound", typeof(WindCompoundDeed), 24000000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a lord of nujelm compound", typeof(NujelmLordCompoundDeed), 24000000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a lord of magincia compound", typeof(MaginciaLordCompoundDeed), 24000000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a grand keep", typeof(GrandKeepDeed), 24000000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a fortress", typeof(FortressDeed), 26000000, 20, 0x14F0, 0));
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {
                Add(typeof(SkaraLordCompoundDeed), 19200000);
                Add(typeof(TrinsicLordFortressDeed), 19200000);
                Add(typeof(YewLordFarmsteadDeed), 19200000);
                Add(typeof(WindCompoundDeed), 19200000);
                Add(typeof(NujelmLordCompoundDeed), 19200000);
                Add(typeof(MaginciaLordCompoundDeed), 19200000);
                Add(typeof(GrandKeepDeed), 19200000);
                Add(typeof(FortressDeed), 20800000);
            }
        }
    }
}
