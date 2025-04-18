using System;
using Server.Network;
using Server.Targeting;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
	public interface ILockpickable : IPoint2D
	{
		int LockLevel{ get; set; }
		bool Locked{ get; set; }
		Mobile Picker{ get; set; }
		int MaxLockLevel{ get; set; }
		int RequiredSkill{ get; set; }

		void LockPick( Mobile from );
	}



	[FlipableAttribute( 0x14fc, 0x14fb )]
	public class Lockpick : Item, ICommodity
	{
		int ICommodity.DescriptionNumber { get { return LabelNumber; } }
		bool ICommodity.IsDeedable { get { return true; } }
	
		[Constructable]
		public Lockpick() : this( 1 )
		{
		}

		[Constructable]
		public Lockpick( int amount ) : base( 0x14FC )
		{
			Stackable = true;
			Amount = amount;
		}

		public Lockpick( Serial serial ) : base( serial )
		{
		}
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if ( version == 0 && Weight == 0.1 )
				Weight = -1;
		}

		public override void OnDoubleClick( Mobile from )
		{
			from.SendLocalizedMessage( 502068 ); // What do you want to pick?
			from.Target = new InternalTarget( this );
		}

		private class InternalTarget : Target
		{
			private Lockpick m_Item;

			public InternalTarget( Lockpick item ) : base( 1, false, TargetFlags.None )
			{
				m_Item = item;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( m_Item.Deleted )
					return;

				if ( targeted is ILockpickable )
				{
					Item item = (Item)targeted;
					from.Direction = from.GetDirectionTo( item );

					if ( ((ILockpickable)targeted).Locked )
					{
						from.PlaySound( 0x241 );

						new InternalTimer( from, (ILockpickable)targeted, m_Item ).Start();
					}
					else
					{
						// The door is not locked
						from.SendLocalizedMessage( 502069 ); // This does not appear to be locked
					}
				}
				else
				{
					from.SendLocalizedMessage( 501666 ); // You can't unlock that!
				}
			}

			private class InternalTimer : Timer
			{
				private Mobile m_From;
				private ILockpickable m_Item;
				private Lockpick m_Lockpick;
			
				public InternalTimer( Mobile from, ILockpickable item, Lockpick lockpick ) : base( TimeSpan.FromSeconds( 3.0 ) )
				{
					m_From = from;
					m_Item = item;
					m_Lockpick = lockpick;
					Priority = TimerPriority.TwoFiftyMS;
				}

				protected void BrokeLockPickTest()
				{
					// When failed, a 25% chance to break the lockpick
					if ( Utility.Random( 4 ) == 0 )
					{
						Item item = (Item)m_Item;

						// You broke the lockpick.
						item.SendLocalizedMessageTo( m_From, 502074 );

						m_From.PlaySound( 0x3A4 );
						m_Lockpick.Consume();
					}
				}
				
				protected override void OnTick()
				{
					Item item = (Item)m_Item;

                    double lockpickingSkill = m_From.Skills[SkillName.Lockpicking].Value;
                    double difficulty = (double)m_Item.RequiredSkill;
                    double chance = (lockpickingSkill - difficulty) * .04;
                    
					if ( !m_From.InRange( item.GetWorldLocation(), 1 ) )
						return;

					if ( m_Item.LockLevel == 0 || m_Item.LockLevel == -255 )
					{						
						item.SendLocalizedMessageTo( m_From, 502073 ); // This lock cannot be picked by normal means
						return;
					}

                    //Skill Gain Check (Can Gain on Any Locked Item)
                    if (m_From.CheckTargetSkill(SkillName.Lockpicking, m_Item, 0, 100))
                    {
                    }

                    //If Chest Is Beyond Skill
                    if (lockpickingSkill < difficulty)
					{
						item.SendLocalizedMessageTo( m_From, 502072 ); // You don't see how that lock can be manipulated.

						return;
					}                    

                    //If Successful
					if (chance >= Utility.RandomDouble() )
					{						
						item.SendLocalizedMessageTo( m_From, 502076 ); // The lock quickly yields to your skill.

						m_From.PlaySound( 0x4A );
						m_Item.LockPick( m_From );

                        PlayerMobile player = m_From as PlayerMobile;
                        if (player != null)
                            player.ChestsPickedThisSession++;
					}

					else
					{
						// The player failed to pick the lock
						BrokeLockPickTest();
						item.SendLocalizedMessageTo( m_From, 502075 ); // You are unable to pick the lock.
					}
				}
			}
		}
	}
}