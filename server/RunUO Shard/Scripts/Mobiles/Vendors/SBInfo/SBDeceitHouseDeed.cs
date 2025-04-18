using System;
using System.Collections.Generic;
using Server.Multis.Deeds;

namespace Server.Mobiles
{
    public class SBDeceitHouseDeed : SBInfo
    {
        private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();
        public SBDeceitHouseDeed()
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
                Add(new GenericBuyInfo("deed to a Deceit small house", typeof(DeceitSmallHouseDeed), 90000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a Deceit watchtower", typeof(DeceitWatchtowerDeed), 450000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a Deceit lodge", typeof(DeceitLodgeDeed), 750000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a Deceit great lodge", typeof(DeceitGreatLodgeDeed), 1000000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a Deceit chalet", typeof(DeceitChaletDeed), 2000000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a Deceit longhouse", typeof(DeceitLonghouseDeedE), 9000000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a Deceit great hall", typeof(DeceitGreatHallDeed), 9000000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a Deceit farmstead", typeof(DeceitFarmsteadDeedE), 10000000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a Deceit grand lodge", typeof(DeceitGrandLodgeDeed), 12000000, 20, 0x14F0, 0));

            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {
                Add(typeof(DeceitSmallHouseDeed), 72000);
                Add(typeof(DeceitWatchtowerDeed), 360000);
                Add(typeof(DeceitLodgeDeed), 600000);
                Add(typeof(DeceitGreatLodgeDeed), 800000);
                Add(typeof(DeceitChaletDeed), 1600000);
                Add(typeof(DeceitLonghouseDeedE), 7200000);
                Add(typeof(DeceitGreatHallDeed), 7200000);
                Add(typeof(DeceitFarmsteadDeedE), 8000000);
                Add(typeof(DeceitGrandLodgeDeed), 9600000);
            }
        }
    }
}
