using System;
using System.Collections.Generic;
using Server.Multis.Deeds;

namespace Server.Mobiles
{
    public class SBMaginciaHouseDeed : SBInfo
    {
        private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();
        public SBMaginciaHouseDeed()
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
                Add(new GenericBuyInfo("deed to a Magincia wagon", typeof(MaginciaWagonDeed), 250000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a Magincia wagon east", typeof(MaginciaWagonDeedE), 250000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a Magincia half shoppe", typeof(MaginciaHalfShoppeEDeed), 150000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a Magincia shoppe", typeof(MaginciaShoppeEDeed), 150000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a Magincia outpost", typeof(MaginciaOutpostEDeed), 550000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a Magincia two story house", typeof(MaginciaTwoStoryHouseEDeed), 550000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a Magincia residence", typeof(MaginciaResidenceEDeed), 750000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a Magincia villa", typeof(MaginciaVillaDeed), 1000000, 20, 0x14F0, 0));
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {
                Add(typeof(MaginciaWagonDeed), 200000);
                Add(typeof(MaginciaWagonDeedE), 200000);
                Add(typeof(MaginciaHalfShoppeEDeed), 120000);
                Add(typeof(MaginciaShoppeEDeed), 120000);
                Add(typeof(MaginciaOutpostEDeed), 440000);
                Add(typeof(MaginciaTwoStoryHouseEDeed), 440000);
                Add(typeof(MaginciaResidenceEDeed), 600000);
                Add(typeof(MaginciaVillaDeed), 800000);
            }
        }
    }
}
