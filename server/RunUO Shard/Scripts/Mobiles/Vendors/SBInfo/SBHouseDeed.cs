using System;
using System.Collections.Generic;
using Server.Multis.Deeds;

namespace Server.Mobiles
{
	public class SBHouseDeed: SBInfo
	{
		private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
		private IShopSellInfo m_SellInfo = new InternalSellInfo();

		public SBHouseDeed()
		{
		}

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : List<GenericBuyInfo>
		{
			public InternalBuyInfo()
			{
                Add(new GenericBuyInfo("deed to a stone-and-plaster house", typeof(StonePlasterHouseDeed), 90000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a field stone house", typeof(FieldStoneHouseDeed), 90000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a field stone house facing east", typeof(FieldStoneHouseDeedE), 90000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a small brick house", typeof(SmallBrickHouseDeed), 90000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a small brick house facing east", typeof(SmallBrickHouseDeedE), 90000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a wooden house", typeof(WoodHouseDeed), 90000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a wood-and-plaster house", typeof(WoodPlasterHouseDeed), 90000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a thatched-roof cottage", typeof(ThatchedRoofCottageDeed), 90000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a small stone workshop", typeof(StoneWorkshopDeed), 120000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a small marble workshop", typeof(MarbleWorkshopDeed), 120000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a small stone tower", typeof(SmallTowerDeed), 150000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a small stone tower facing east", typeof(SmallTowerDeedE), 150000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a sandstone house with patio", typeof(SandstonePatioDeed), 350000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a two story log cabin", typeof(LogCabinDeed), 450000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a two story villa", typeof(VillaDeed), 650000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a large house with patio", typeof(LargePatioDeed), 1000000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a marble house with patio", typeof(LargeMarbleDeed), 1250000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a two-story wood-and-plaster house", typeof(TwoStoryWoodPlasterHouseDeed), 1250000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a two-story stone-and-plaster house", typeof(TwoStoryStonePlasterHouseDeed), 1250000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a brick house", typeof(BrickHouseDeed), 850000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a tower", typeof(TowerDeed), 9000000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a tower facing east", typeof(TowerDeedE), 9000000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a stone keep", typeof(KeepDeed), 12000000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("deed to a castle", typeof(CastleDeed), 15000000, 20, 0x14F0, 0));
            }
        }

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				Add( typeof( StonePlasterHouseDeed ), 72000 );
				Add( typeof( FieldStoneHouseDeed ), 72000 );
                Add( typeof( FieldStoneHouseDeedE ), 72000);
                Add( typeof( SmallBrickHouseDeed ), 72000 );
                Add( typeof( SmallBrickHouseDeedE ), 72000);
                Add( typeof( WoodHouseDeed ), 72000 );
				Add( typeof( WoodPlasterHouseDeed ), 72000 );
				Add( typeof( ThatchedRoofCottageDeed ), 72000 );
				Add( typeof( BrickHouseDeed ), 680000 );
				Add( typeof( TwoStoryWoodPlasterHouseDeed ), 1000000 );
				Add( typeof( TwoStoryStonePlasterHouseDeed ), 1000000);
				Add( typeof( TowerDeed ), 7200000 );
                Add( typeof( TowerDeedE ), 7200000);
                Add( typeof( KeepDeed ), 9600000 );
				Add( typeof( CastleDeed ), 12000000 );
				Add( typeof( LargePatioDeed ), 800000 );
				Add( typeof( LargeMarbleDeed ), 1000000 );
				Add( typeof( SmallTowerDeed ), 120000 );
                Add( typeof( SmallTowerDeedE ), 120000);
                Add( typeof( LogCabinDeed ), 360000 );
				Add( typeof( SandstonePatioDeed ), 280000 );
				Add( typeof( VillaDeed ), 520000 );
				Add( typeof( StoneWorkshopDeed ), 96000 );
				Add( typeof( MarbleWorkshopDeed ), 96000 );
				Add( typeof( SmallBrickHouseDeed ), 72000 );
			}
		}
	}
}
