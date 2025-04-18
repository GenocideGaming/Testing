using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Gumps;
using Server.Prompts;
using Server.Targeting;
using Server.Misc;
using Server.Multis;
using Server.ContextMenus;

namespace Server.Items
{
	public enum TrapType
	{
		None,
		MagicTrap,
		ExplosionTrap,
		DartTrap,
		PoisonTrap
	}

	public abstract class TrapableContainer : BaseContainer, ITelekinesisable
	{
		private TrapType m_TrapType;
		private int m_TrapPower;
		private int m_TrapLevel;

		[CommandProperty( AccessLevel.GameMaster )]
		public TrapType TrapType
		{
			get
			{
				return m_TrapType;
			}
			set
			{
				m_TrapType = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int TrapPower
		{
			get
			{
				return m_TrapPower;
			}
			set
			{
				m_TrapPower = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int TrapLevel
		{
			get
			{
				return m_TrapLevel;
			}
			set
			{
				m_TrapLevel = value;
			}
		}

		public virtual bool TrapOnOpen{ get{ return true; } }

		public TrapableContainer( int itemID ) : base( itemID )
		{
		}

		public TrapableContainer( Serial serial ) : base( serial )
		{
		}

		private void SendMessageTo( Mobile to, int number, int hue )
		{
			if ( Deleted || !to.CanSee( this ) )
				return;

			to.Send( new Network.MessageLocalized( Serial, ItemID, Network.MessageType.Regular, hue, 3, number, "", "" ) );
		}

		private void SendMessageTo( Mobile to, string text, int hue )
		{
			if ( Deleted || !to.CanSee( this ) )
				return;

			to.Send( new Network.UnicodeMessage( Serial, ItemID, Network.MessageType.Regular, hue, 3, "ENU", "", text ) );
		}

		public virtual bool ExecuteTrap( Mobile from )
		{
			if ( m_TrapType != TrapType.None )
			{
				Point3D loc = this.GetWorldLocation();
				Map facet = this.Map;

				if ( from.AccessLevel >= AccessLevel.GameMaster )
				{
					SendMessageTo( from, "That is trapped, but you open it with your godly powers.", 0x3B2 );
					return false;
				}

				switch ( m_TrapType )
				{
					case TrapType.ExplosionTrap:
					{
						SendMessageTo( from, 502999, 0x3B2 ); // You set off a trap!

						if ( from.InRange( loc, 3 ) )
						{
							int damage;

                            if (m_TrapLevel > 0)
                                damage = Utility.RandomMinMax(25, 50) * m_TrapLevel;
                            else
                                damage = m_TrapPower;

							AOS.Damage( from, damage, 0, 100, 0, 0, 0 );

							// Your skin blisters from the heat!
							from.LocalOverheadMessage( Network.MessageType.Regular, 0x2A, 503000 );
						}

						Effects.SendLocationEffect( loc, facet, 0x36BD, 15, 10 );
						Effects.PlaySound( loc, facet, 0x307 );

                        double trapRemovalChance = .85 - (m_TrapLevel * .15);
                                                
                        //Trap Removed
                        if (trapRemovalChance >= Utility.RandomDouble())
                        {
                            m_TrapType = TrapType.None;
                            m_TrapPower = 0;
                            m_TrapLevel = 0;

                            from.SendMessage("You are fairly certain the the chest is no longer trapped...");                             
                        }

                        //Trap Still Active  
                        else
                        {
                            from.SendMessage("You have reason to believe the chest may still contain traps...");  
                        }

						break;
					}

					case TrapType.MagicTrap:
					{
						if ( from.InRange( loc, 1 ) )
							from.Damage( m_TrapPower );							

						Effects.PlaySound( loc, Map, 0x307 );

						Effects.SendLocationEffect( new Point3D( loc.X - 1, loc.Y, loc.Z ), Map, 0x36BD, 15 );
						Effects.SendLocationEffect( new Point3D( loc.X + 1, loc.Y, loc.Z ), Map, 0x36BD, 15 );

						Effects.SendLocationEffect( new Point3D( loc.X, loc.Y - 1, loc.Z ), Map, 0x36BD, 15 );
						Effects.SendLocationEffect( new Point3D( loc.X, loc.Y + 1, loc.Z ), Map, 0x36BD, 15 );

						Effects.SendLocationEffect( new Point3D( loc.X + 1, loc.Y + 1, loc.Z + 11 ), Map, 0x36BD, 15 );

                        m_TrapType = TrapType.None;
                        m_TrapPower = 0;
                        m_TrapLevel = 0;

						break;
					}

					case TrapType.DartTrap:
					{
						SendMessageTo( from, 502999, 0x3B2 ); // You set off a trap!

						if ( from.InRange( loc, 3 ) )
						{
							int damage;

							if ( m_TrapLevel > 0 )
								damage = Utility.RandomMinMax( 5, 15 ) * m_TrapLevel;
							else
								damage = m_TrapPower;

							AOS.Damage( from, damage, 100, 0, 0, 0, 0 );

							// A dart imbeds itself in your flesh!
							from.LocalOverheadMessage( Network.MessageType.Regular, 0x62, 502998 );
						}

						Effects.PlaySound( loc, facet, 0x223 );

                        m_TrapType = TrapType.None;
                        m_TrapPower = 0;
                        m_TrapLevel = 0;

						break;
					}

					case TrapType.PoisonTrap:
					{
						SendMessageTo( from, 502999, 0x3B2 ); // You set off a trap!

						if ( from.InRange( loc, 3 ) )
						{
							Poison poison;

							if ( m_TrapLevel > 0 )
							{
								poison = Poison.GetPoison( Math.Max( 0, Math.Min( 4, m_TrapLevel - 1 ) ) );
							}
							else
							{
								AOS.Damage( from, m_TrapPower, 0, 0, 0, 100, 0 );
								poison = Poison.Greater;
							}

							from.ApplyPoison( from, poison );

							// You are enveloped in a noxious green cloud!
							from.LocalOverheadMessage( Network.MessageType.Regular, 0x44, 503004 );
						}

						Effects.SendLocationEffect( loc, facet, 0x113A, 10, 20 );
						Effects.PlaySound( loc, facet, 0x231 );

                        m_TrapType = TrapType.None;
                        m_TrapPower = 0;
                        m_TrapLevel = 0;

						break;
					}
				}			

				return true;
			}

			return false;
		}

		public virtual void OnTelekinesis( Mobile from )
		{
			Effects.SendLocationParticles( EffectItem.Create( Location, Map, EffectItem.DefaultDuration ), 0x376A, 9, 32, 5022 );
			Effects.PlaySound( Location, Map, 0x1F5 );

			if( this.TrapOnOpen )
			{
				ExecuteTrap( from );
			}
		}

		public override void Open( Mobile from )
		{
            bool foundTopOfContainer = false;
            bool itemOnVendor = false;
       
            Container currentContainer = this;

            while (!foundTopOfContainer)
            {               
                //Check Parent Container              
                Container parentContainer = currentContainer.Parent as Container;             

                //Parent is Valid Container
                if (parentContainer != null)
                {                   
                    currentContainer = parentContainer;                    
                }               
                   
                //No Valid Parent: End of Container Depth
                else
                {                   
                    foundTopOfContainer = true;

                    //Double Check for Non-Null
                    if (currentContainer != null)
                    {
                        //Container is VendorBackpack
                        if (currentContainer is VendorBackpack)
                        {
                            itemOnVendor = true;
                        }                        
                    }

                    break;
                }                                
            }

            //Trapped Item on Vendor: Prevent Open
            if (itemOnVendor && this.TrapType != TrapType.None)
            {
                from.SendMessage("You feel a sense of danger surrounding that object...");

                //Staff can Open Without Issue
                if (from.AccessLevel > AccessLevel.Player)
                {
                    base.Open(from);
                }
            }

            //Normal Behavior
            else
            {
                //Open Container
                if (!this.TrapOnOpen || !ExecuteTrap(from))
                {
                    base.Open(from);
                }
            }
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 2 ); // version

			writer.Write( (int) m_TrapLevel );

			writer.Write( (int) m_TrapPower );
			writer.Write( (int) m_TrapType );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 2:
				{
					m_TrapLevel = reader.ReadInt();
					goto case 1;
				}
				case 1:
				{
					m_TrapPower = reader.ReadInt();
					goto case 0;
				}
				case 0:
				{
					m_TrapType = (TrapType)reader.ReadInt();
					break;
				}
			}
		}
	}
}