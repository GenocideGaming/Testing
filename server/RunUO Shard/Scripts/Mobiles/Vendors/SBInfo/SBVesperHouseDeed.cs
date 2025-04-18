using System;
using System.Collections.Generic;
using Server.Multis.Deeds;

namespace Server.Mobiles
{
    public class SBVesperHouseDeed : SBInfo
    {
        private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();
        public SBVesperHouseDeed()
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
                Add(new GenericBuyInfo("deed to a Vesper shoppe", typeof(VesperShopDeed), 450000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a Vesper ranch house", typeof(VesperSlateRanchHouseDeed), 550000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a Vesper cabin", typeof(VesperCabinDeed), 550000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a Vesper overlook", typeof(VesperOverlookEDeed), 1000000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a Vesper two story cabin", typeof(VesperTwoStoryCabinDeed), 1250000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a Vesper compound", typeof(VesperCompoundDeed), 12000000, 20, 0x14F0, 0));


            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {
                Add(typeof(VesperShopDeed), 360000);
                Add(typeof(VesperSlateRanchHouseDeed), 440000);
                Add(typeof(VesperCabinDeed), 440000);
                Add(typeof(VesperOverlookEDeed), 800000);
                Add(typeof(VesperTwoStoryCabinDeed), 1000000);
                Add(typeof(VesperCompoundDeed), 9600000);
            }
        }
    }
}
