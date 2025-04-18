using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Targeting;

using Server.Scripts.Custom.Citizenship;
using Server.Factions;
using Server.Scripts.Engines.Factions.Instances.Factions;

namespace Server.Targets
{
	public class AIControlMobileTarget : Target
	{
		private List<BaseAI> m_List;
		private OrderType m_Order;
		private BaseCreature m_Mobile;

		public OrderType Order 
		{
			get 
			{
				return m_Order;
			}
		}

		public AIControlMobileTarget( BaseAI ai, OrderType order, BaseCreature mobile = null ) : base( -1, false, ( order == OrderType.Attack ? TargetFlags.Harmful : TargetFlags.None ) )
		{
			m_List = new List<BaseAI>();
			m_Order = order;

			if(mobile != null)
				m_Mobile = mobile;

			AddAI( ai );
		}

		public void AddAI( BaseAI ai )
		{
			if ( !m_List.Contains( ai ) )
				m_List.Add( ai );
		}

		protected override void OnTarget( Mobile from, object o )
		{
			if ( o is Mobile ) 
			{
				Mobile m = (Mobile)o;

				if(m_Mobile.Controlled && m_Mobile.ControlMaster != null)
                {
                    PlayerMobile pm = (PlayerMobile)m_Mobile.ControlMaster;

                    // Prevent players from farming their own Champions or Champion spawns
                    Faction playerFaction = Faction.Find(pm, true);
					string cannotAttackMsg = "You cannot attack a member of your own champion spawn!";

                    Type[] forsakenTypes = new Type[] { typeof(ForsakenMinion), typeof(ForsakenDK), typeof(ForsakenNecromancer), typeof(Chandrian), typeof(Haliax) };
                    Type[] urukhaiTypes = new Type[] { typeof(UrukGrunt), typeof(UrukRaider), typeof(UrukShaman), typeof(UrukScout), typeof(Gojira) };
                    Type[] paladinTypes = new Type[] { typeof(POSquire), typeof(POKnight), typeof(POPaladin), typeof(Amyr), typeof(DupreThePaladin) };

                    if( (playerFaction == Forsaken.Instance || pm.Citizenship == "Yew") && Array.Exists(forsakenTypes, element => element == m.GetType()) ) // forsaken , yew
                    {
						pm.SendMessage(cannotAttackMsg);
                        return;
                    }
                    else if( (playerFaction == Urukhai.Instance || pm.Citizenship == "Blackrock") && Array.Exists(urukhaiTypes, element => element == m.GetType()) ) // urukhai , blackrock
                    {
						pm.SendMessage(cannotAttackMsg);
                        return;
                    }
                    else if( (playerFaction == PaladinOrder.Instance || pm.Citizenship == "Trinsic") && Array.Exists(paladinTypes, element => element == m.GetType()) ) // palading , trinsic
                    {
						pm.SendMessage(cannotAttackMsg);
                        return;
                    }
                }

				for ( int i = 0; i < m_List.Count; ++i )
					m_List[i].EndPickTarget( from, m, m_Order );
			}
		}
	}
}