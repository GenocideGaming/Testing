using System;
using System.Collections;
using Server;
using Server.Prompts;
using Server.Mobiles;
using Server.ContextMenus;
using Server.Gumps;
using Server.Items;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{	
	public class CoinBox : Item 
	{
		[Constructable]
		public CoinBox( /* Mobile m */ ) : base( 0xE80 )
		{
			Movable = true;
			Weight = 0;
			Name = "Coin Box";
            LootType = LootType.Blessed;
			
			//m_Owner = m;			
		}		

		public override void OnDoubleClick( Mobile from )
		{
			/* if ( m_Owner == null )
			{
				Mobile mobile = (Mobile)from;
				PlayerMobile pm = (PlayerMobile)from;

				Owner = pm;
			}
			if ( from != m_Owner )
			{
				from.SendMessage( "You try to pry the box open but fail." ); 
			}
			else */ 
			if ( !IsChildOf( from.Backpack ) ) // Make sure its in their pack
			{
				 from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			}
			else if ( from is PlayerMobile )
			{
				from.SendGump( new CoinBoxGump( (PlayerMobile)from, this ) );
			}
		}

		

		

		public CoinBox( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			
			writer.Write((int)0); // version
			
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}

namespace Server.Gumps
{
	
	public class CoinBoxGump : Gump
	{
	
		private PlayerMobile m_From;
		private CoinBox m_Box;	
      	   
		public CoinBoxGump( PlayerMobile from, Item item ): base( 0, 0 )
		{
			m_From = from;			
			if (!(item is CoinBox))
				return;
			CoinBox box = item as CoinBox;
			m_Box = box;

			m_From.CloseGump( typeof( CoinBoxGump ) );
			
			Closable=true;
			Disposable=false;
			Dragable=true;
			Resizable=false;

			AddPage(0);
			AddImage(112, 135, 75);

			AddLabel(135, 187, 67, @"Platinum:");
			AddLabel(208, 187, 1160, m_From.PlatinumCoins.ToString("#,0"));

			AddLabel(135, 205, 67, @"Bloodcoin:");
			AddLabel(208, 205, 1160, m_From.BloodCoins.ToString("#,0"));

			AddLabel(135, 223, 67, @"Bank Gold:");
			AddLabel(208, 223, 1160, Banker.GetBalance((Mobile)from).ToString("#,0"));

			AddLabel(135, 241, 67, @"Bank Silver:");
			AddLabel(208, 241, 1160, Banker.GetSilverBalance((Mobile)from).ToString("#,0"));
		}
		
		public override void OnResponse( NetState sender, RelayInfo info )
		{
		}
	}
}
