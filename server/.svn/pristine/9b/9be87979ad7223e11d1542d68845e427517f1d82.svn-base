using System; 
using System.Collections.Generic; 
using Server.Items; 

namespace Server.Mobiles 
{ 
	public class SBHairStylist : SBInfo 
	{ 
		private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo(); 
		private IShopSellInfo m_SellInfo = new InternalSellInfo(); 

		public SBHairStylist() 
		{ 
		} 

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } } 
		public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } } 

		public class InternalBuyInfo : List<GenericBuyInfo> 
		{ 
			public InternalBuyInfo() 
			{ 
				Add( new GenericBuyInfo( "1041060", typeof( HairDye ), 60, 20, 0xEFF, 0 ) ); 
			} 
		} 

		public class InternalSellInfo : GenericSellInfo 
		{ 
			public InternalSellInfo() 
			{ 
				Add( typeof( HairDye ), 30 ); 
			} 
		} 
	} 
}