using System;
using System.Collections.Generic;
using Server.Multis.Deeds;

namespace Server.Mobiles
{
    public class SBWindHouseDeed : SBInfo
    {
        private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();
        public SBWindHouseDeed()
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
                Add(new GenericBuyInfo("deed to a Wind small tower east", typeof(WindSmallTowerDeedE), 150000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a Wind small tower", typeof(WindSmallTowerDeed), 250000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a Wind mansion", typeof(WindMansionDeed), 12000000, 20, 0x14F0, 0));

            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {
                Add(typeof(WindSmallTowerDeedE), 120000);
                Add(typeof(WindSmallTowerDeed), 200000);
                Add(typeof(WindMansionDeed), 9600000);
            }
        }
    }
}
