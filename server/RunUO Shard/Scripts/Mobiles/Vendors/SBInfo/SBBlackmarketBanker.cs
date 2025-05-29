using System;
using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles
{
    public class SBBlackmarketBanker : SBInfo
    {
        private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SBBlackmarketBanker()
        {
        }

        public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
        public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                // No items for sale 
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {
                Add(typeof(Amber), 25);
                Add(typeof(Amethyst), 50);
                Add(typeof(Citrine), 25);
                Add(typeof(Diamond), 100);
                Add(typeof(Emerald), 50);
                Add(typeof(Ruby), 37);
                Add(typeof(Sapphire), 50);
                Add(typeof(StarSapphire), 62);
                Add(typeof(Tourmaline), 47);
                Add(typeof(GoldRing), 13);
                Add(typeof(SilverRing), 10);
                Add(typeof(Necklace), 13);
                Add(typeof(GoldNecklace), 13);
                Add(typeof(GoldBeadNecklace), 13);
                Add(typeof(SilverNecklace), 10);
                Add(typeof(SilverBeadNecklace), 10);
                Add(typeof(Beads), 13);
                Add(typeof(GoldBracelet), 13);
                Add(typeof(SilverBracelet), 10);
                Add(typeof(GoldEarrings), 13);
                Add(typeof(SilverEarrings), 10);
            }
        }
    }
}