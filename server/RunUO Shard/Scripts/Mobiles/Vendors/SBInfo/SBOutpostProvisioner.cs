using Server.Items;
using Server.Mobiles;
using Server;
using System.Collections.Generic;

public class SBOutpostProvisioner : SBInfo
{
    private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
    private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

    public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
    public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }

    public class InternalBuyInfo : List<GenericBuyInfo>
    {
        public InternalBuyInfo()
        {
            // Standard SBProvisioner items
            Add(new GenericBuyInfo(typeof(Arrow), 3, 500, 0xF3F, 0));
            Add(new GenericBuyInfo(typeof(Backpack), 15, 20, 0x9B2, 0));
            // Quest resources
            Add(new GenericBuyInfo(typeof(FishSteak), 3, 500, 0x97B, 0));
            Add(new GenericBuyInfo(typeof(Cotton), 3, 500, 0xF95, 0));
            Add(new GenericBuyInfo(typeof(Log), 3, 500, 0x1BDD, 0));
            Add(new GenericBuyInfo(typeof(IronOre), 3, 500, 0x19B7, 0));
            Add(new GenericBuyInfo(typeof(WheatSheaf), 3, 500, 0xF36, 0));
        }
    }

    public class InternalSellInfo : GenericSellInfo
    {
        public InternalSellInfo()
        {
            // Standard SBProvisioner items
            Add(typeof(Arrow), 1);
            Add(typeof(Backpack), 7);
            // Quest resources
            Add(typeof(FishSteak), 1);
            Add(typeof(Cotton), 1);
            Add(typeof(Log), 1);
            Add(typeof(IronOre), 1);
            Add(typeof(WheatSheaf), 1);
        }
    }
}