using System;
using Server;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;
using System.Collections.Generic;
using Server.Scripts.Custom.Citizenship.CommonwealthBonuses;
using Server.Scripts;
using System.IO;

namespace Server.Factions
{
	public class FactionStoneGump : FactionGump
	{
		private PlayerMobile m_From;
		private Faction m_Faction;

		public override int ButtonTypes{ get{ return 4; } }

		public FactionStoneGump( PlayerMobile from, Faction faction ) : base( 20, 30 )
		{
            if (from == null || faction == null || faction.Definition == null) { return; }
            m_From = from;
			m_Faction = faction;

			AddPage( 0 );

			AddBackground( 0, 0, 550, 440, 5054 );
			AddBackground( 10, 10, 530, 420, 3000 );

			#region General
			AddPage( 1 );

			AddHtmlText( 20, 30, 510, 20, faction.Definition.Header, false, false );

			AddHtmlLocalized( 20, 60, 100, 20, 1011429, false, false ); // Led By : 
			AddHtml( 125, 60, 200, 20, faction.Commander != null ? faction.Commander.Name : "Nobody", false, false );

			AddHtmlLocalized( 20, 80, 100, 20, 1011457, false, false ); // Tithe rate : 
			if ( faction.SilverTax >= 0 && faction.SilverTax <= 100 && (faction.SilverTax % 10) == 0 )
				AddHtmlLocalized( 125, 80, 350, 20, 1011480 + (faction.SilverTax / 10), false, false );
			else
				AddHtml( 125, 80, 350, 20, faction.SilverTax + "%", false, false );

			AddHtmlLocalized( 20, 100, 100, 20, 1011458, false, false ); // Traps placed : 
			AddHtml( 125, 100, 50, 20, faction.Traps != null ? faction.Traps.Count.ToString() : "None", false, false );

			AddHtmlLocalized( 55, 150, 100, 20, 1011430, false, false ); // CITY STATUS
			AddButton( 20, 150, 4005, 4007, 0, GumpButtonType.Page, 2 );

			AddHtmlLocalized( 55, 175, 100, 20, 1011444, false, false ); // STATISTICS
			AddButton( 20, 175, 4005, 4007, 0, GumpButtonType.Page, 4 );

			PlayerState pl = PlayerState.Find( from );

			AddHtmlLocalized( 55, 200, 300, 20, 1011461, false, false ); // COMMANDER OPTIONS
			if ( faction.IsCommander( from ) )
				AddButton( 20, 200, 4005, 4007, 0, GumpButtonType.Page, 6 );
			else
				AddImage( 20, 200, 4020 );

			AddHtmlLocalized( 55, 225, 300, 20, 1011426, false, false ); // LEAVE THIS FACTION
			AddButton( 20, 225, 4005, 4007, ToButtonID( 0, 1 ), GumpButtonType.Reply, 0 );

			AddHtmlLocalized( 55, 250, 200, 20, 1011441, false, false ); // EXIT
			AddButton( 20, 250, 4005, 4007, 0, GumpButtonType.Reply, 0 );
			#endregion

            #region CityStatus
            AddPage(2);
            if (faction.OwningCommonwealth != null
                && faction.OwningCommonwealth.State != null
                && faction.OwningCommonwealth.State.Bonuses != null)
            {
                List<CommonwealthBonus> townshipBonuses = faction.OwningCommonwealth.State.Bonuses;
                AddLabel(40, 55, 0, "This township bestows the following bonuses:");
                for (int i = 0; i < townshipBonuses.Count; i++)
                {
                    CommonwealthBonus bonus = townshipBonuses[i];
                    AddLabel(40, 80 + (i * 20), 0, bonus.DictKey);
                }
            }
            #endregion

            #region Statistics
            AddPage( 4 );

			AddHtmlLocalized( 20, 30, 150, 20, 1011444, false, false ); // STATISTICS

			AddHtmlLocalized( 20, 100, 100, 20, 1011445, false, false ); // Name : 
			AddHtml( 120, 100, 150, 20, from.Name, false, false );

			AddHtmlLocalized( 20, 130, 100, 20, 1018064, false, false ); // score : 
			AddHtml( 120, 130, 100, 20, (pl != null ? pl.KillPoints : 0).ToString(), false, false );

			AddHtmlLocalized( 20, 160, 100, 20, 1011446, false, false ); // Rank : 
			AddHtml( 120, 160, 100, 20, ((pl != null && pl.Rank != null) ? pl.Rank.Rank : 0).ToString(), false, false );

			AddHtmlLocalized( 55, 250, 100, 20, 1011447, false, false ); // BACK
			AddButton( 20, 250, 4005, 4007, 0, GumpButtonType.Page, 1 );
			#endregion



			#region Commander Options
			if ( faction.IsCommander( from ) )
			{
				#region General
				AddPage( 6 );

				AddHtmlLocalized( 20, 30, 200, 20, 1011461, false, false ); // COMMANDER OPTIONS

				AddHtmlLocalized( 20, 70, 120, 20, 1011457, false, false ); // Tithe rate : 
				if ( faction.SilverTax >= 0 && faction.SilverTax <= 50 && (faction.SilverTax % 10) == 0 )
					AddHtmlLocalized( 140, 70, 250, 20, 1011480 + (faction.SilverTax / 10), false, false );
				else
					AddHtml( 140, 70, 250, 20, faction.SilverTax + "%", false, false );

				AddHtmlLocalized( 20, 100, 120, 20, 1011474, false, false ); // Silver available : 
				AddHtml( 140, 100, 50, 20, faction.Silver != null ? faction.Silver.ToString( "N0" ) : 0 + "", false, false ); // NOTE: Added 'N0' formatting

				AddHtmlLocalized( 55, 130, 200, 20, 1011478, false, false ); // CHANGE TITHE RATE
				AddButton( 20, 130, 4005, 4007, 0, GumpButtonType.Page, 8 );

                AddLabel(55, 160, 0, "Guard Options");
                AddButton(20, 160, 4005, 4007, 0, GumpButtonType.Page, 9);

				AddHtmlLocalized( 55, 310, 100, 20, 1011447, false, false ); // BACK
				AddButton( 20, 310, 4005, 4007, 0, GumpButtonType.Page, 1 );
				#endregion

				#region Change Tithe Rate
				AddPage( 8 );

				AddHtmlLocalized( 20, 30, 400, 20, 1011479, false, false ); // Select the % for the new tithe rate

				int y = 55;

				for ( int i = 0; i <= 5; ++i )
				{
					if ( i == 5 )
						y += 5;

					AddHtmlLocalized( 55, y, 300, 20, 1011480 + i, false, false );
					AddButton( 20, y, 4005, 4007, ToButtonID( 3, i ), GumpButtonType.Reply, 0 );

					y += 20;

					if ( i == 5 )
						y += 5;
				}

				AddHtmlLocalized( 55, 310, 300, 20, 1011447, false, false ); // BACK
				AddButton( 20, 310, 4005, 4007, 0, GumpButtonType.Page, 1 );
				#endregion

                #region GuardOptions
                AddPage(9);
                AddLabel(20, 30, 0, "Silver available: " + faction.Silver);
                if (Town.FromMilitia(faction) != null && Town.FromMilitia(faction).GuardLists != null)
                {
                    List<GuardList> guardLists = Town.FromMilitia(faction).GuardLists;
                    AddLabel(20, 60, 0, "Your hero is currently protected by the following guards:");
                    for (int i = 0; i < guardLists.Count; i++)
                    {
                        y = 90 + i * 30;
                        if (guardLists[i].Definition != null && guardLists[i].Definition.Header != null && guardLists[i].Guards != null)
                        {
                            AddLabel(20, y, 0, guardLists[i].Definition.Header.String + " : " + guardLists[i].Guards.Count + "/" + guardLists[i].Definition.Maximum);
                        }
                    }
                }
                else
                {
                    AddLabel(20, y, 0, "Town.FromMilitia Error");
                }

                if (faction.Definition != null && faction.Definition.Guards != null)
                {
                    GuardDefinition[] guardDefinitions = faction.Definition.Guards;

                    for (int i = 0; i < guardDefinitions.Length; i++)
                    {
                        y = 90 + (i * 30);
                        AddButton(200, y, 4005, 4007, ToButtonID(2, i), GumpButtonType.Reply, 0);
                        AddLabel(230, y, 0, guardDefinitions[i].Label.String + " Cost: " + guardDefinitions[i].Price);
                    }
                }
                else
                {
                    AddLabel(230, y, 0, "guardDefinitions Error");
                }
                AddHtmlLocalized(55, 360, 200, 25, 1011067, false, false); // Previous page
                AddButton(20, 360, 4005, 4007, 0, GumpButtonType.Page, 1);
                #endregion
            }
			#endregion
		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
            int type, index;
            try
            {
                if (!FromButtonID(info.ButtonID, out type, out index))
                    return;
                Console.WriteLine("Faction gump type " + type + " index " + index);
                switch (type)
                {
                    case 0: // general
                        {
                            switch (index)
                            {
                                case 1: // leave
                                    {
                                        m_From.SendGump(new LeaveFactionGump(m_From, m_Faction));
                                        break;
                                    }
                            }

                            break;
                        }
                    case 3: // change tithe
                        {
                            if (!m_Faction.IsCommander(m_From))
                                return;

                            if (index >= 0 && index <= 5)
                                if (m_Faction.CanChangeSilverTaxRate)
                                {
                                    m_Faction.SilverTax = index * 10;
                                    m_Faction.Broadcast("The silver tax rate has been set to " + m_Faction.SilverTax + "% by your commander.");
                                }
                                else
                                {
                                    DateTime nextSilverChangeTime = m_Faction.LastSilverTaxChange + TimeSpan.FromHours(FeatureList.Militias.SilverTaxChangeBlockTimeInHours);
                                    m_From.SendMessage("You must wait until " + nextSilverChangeTime + " before you can change the silver tax again");
                                }

                            break;
                        }
                    case 2: //guard options
                        {
                            Console.WriteLine("Faction gump clicked guard options index " + index);
                            if (!m_Faction.IsCommander(m_From))
                                return;
                            Town town = Town.FromMilitia(m_Faction);
                            if (index >= 0 && index < m_Faction.Definition.Guards.Length)
                            {
                                if (m_Faction.Silver - m_Faction.Definition.Guards[index].Price >= 0)
                                {
                                    GuardList guardList = town.FindGuardList(m_Faction.Definition.Guards[index].Type);

                                    BaseFactionGuard guard = guardList.Construct();

                                    if (guard != null)
                                    {
                                        guard.Faction = m_Faction;
                                        guard.Town = town;

                                        m_Faction.Silver -= guardList.Definition.Price;

                                        guard.MoveToWorld(m_From.Location, m_From.Map);
                                        guard.Home = guard.Location;
                                    }
                                }
                                else
                                {
                                    m_From.SendMessage("Your militia cannot afford that guard.");
                                    return;
                                }
                            }
                            break;
                        }
                }
            }
            catch (Exception e)
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter("ERROR-factionStoneGump.txt", true))
                    {
                        string message = e.Message + ":\n" + e.StackTrace + "\n" + sender.Mobile.Name + " pushed button " + info.ButtonID;
                        writer.WriteLine(message);
                    }
                    sender.Mobile.SendMessage("An error was encountered in processing your request. It has been logged and will be looked at soon by staff.");
                }
                catch {}
            }
		}
	}
}