using Server.Items;
using Server.Mobiles;
using Server;
using System.Collections.Generic;

public class SBOutpostAnimalTrainer : SBInfo
{
    private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
    private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

    public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
    public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }

    public class InternalBuyInfo : List<GenericBuyInfo>
    {
        public InternalBuyInfo()
        {
            // Standard SBAnimalTrainer items
            Add(new AnimalBuyInfo(1, typeof(Horse), 550, 10, 204, 0));
            Add(new AnimalBuyInfo(1, typeof(PackHorse), 631, 10, 291, 0));
            // Additional items
            Add(new GenericBuyInfo(typeof(Bandage), 5, 100, 0xE21, 0));
        }
    }

    public class InternalSellInfo : GenericSellInfo
    {
        public InternalSellInfo()
        {
            Add(typeof(Bandage), 2);
        }
    }
}