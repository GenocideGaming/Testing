using System;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Targeting;
using Server.Items;
using Server.Network;
using Server.Factions;
using Server.Spells.Seventh;
using Server.Spells.Fifth;
using Server.Spells;
using Server.Regions;
using Server.Scripts.Custom.Citizenship;
using Server.Scripts;
using Server.Engines.XmlSpawner2;

namespace Server.SkillHandlers
{
	public class Stealing
	{
		public static void Initialize()
		{
			SkillInfo.Table[33].Callback = new SkillUseCallback( OnUse );
		}

		public static readonly bool ClassicMode = true;
		public static readonly bool SuspendOnMurder = false;

		public static bool IsInnocentTo( Mobile from, Mobile to )
		{
			return ( Notoriety.Compute( from, (Mobile)to ) == Notoriety.Innocent );
		}

		private class StealingTarget : Target
		{
			private Mobile m_Thief;

			public StealingTarget( Mobile thief ) : base ( 1, false, TargetFlags.None )
			{
				m_Thief = thief;

				AllowNonlocal = true;
			}

			private Item TryStealItem( Item toSteal, ref bool caught )
			{
                double skill = m_Thief.Skills[SkillName.Stealing].Value;
                double stealChance = skill / 100;
                double maxWeightAllowedOnSuccess = 0;

                bool stealAttemptSuccess = false;

                //SkillGainCheck
                m_Thief.CheckTargetSkill(SkillName.Stealing, toSteal, 0, 100);

                //If Success Chance isMore Than Maximum Allowed
                if (stealChance > FeatureList.Stealing.StealingMaxSuccessChance)
                {
                    stealChance = FeatureList.Stealing.StealingMaxSuccessChance;
                }

                //Base Stealing attempt (Against Weight 1)
                if (stealChance >= Utility.RandomDouble())
                {
                    stealAttemptSuccess = true;
                }

                //Determine Maximum Weight Allowed For Successful Steal
                if (stealAttemptSuccess)
                {
                    //(Skill / 10) + (-2.5 to 2.5) stones
                    maxWeightAllowedOnSuccess = (skill / 10) + (((double)Utility.RandomMinMax(-25, 25)) / 10);                    
                }

				Item stolen = null;

				object root = toSteal.RootParent;

				StealableArtifactsSpawner.StealableInstance si = null;

                //Artifact Spawner
				if ( toSteal.Parent == null || !toSteal.Movable )
					si = StealableArtifactsSpawner.GetStealableInstance( toSteal );

                //Both Hands Not Free
				if ( !IsEmptyHanded( m_Thief ) )
				{
					m_Thief.SendLocalizedMessage( 1005584 ); // Both hands must be free to steal.
				}

                //Non-Player That isnt a PackHorse or PackLlama
                else if (root is BaseCreature && !(root is PackHorse || root is PackLlama))
                {
                    m_Thief.SendLocalizedMessage( 502710 ); // You can't steal that!
                }

                //NPC Vendor
				else if ( root is BaseVendor && ((BaseVendor)root).IsInvulnerable )
				{
					m_Thief.SendLocalizedMessage( 1005598 ); // You can't steal from shopkeepers.
				}

                //Player Vendor
				else if ( root is PlayerVendor )
				{
					m_Thief.SendLocalizedMessage( 502709 ); // You can't steal from vendors.
				}

                //Target Can't Be Seen
				else if ( !m_Thief.CanSee( toSteal ) )
				{
					m_Thief.SendLocalizedMessage( 500237 ); // Target can not be seen.
				}

                //Null Backpack
				else if ( m_Thief.Backpack == null)
				{
					m_Thief.SendLocalizedMessage( 1048147 ); // Your backpack can't hold anything else.
				}

                //Item Not Movable, unless marked as stealable (Stealable rares)
				else if ( si == null && ( toSteal.Parent == null || !toSteal.Movable ) && !ItemFlags.GetStealable(toSteal) )
				{
					m_Thief.SendLocalizedMessage( 502710 ); // You can't steal that!
				}

                //Newbified or Blessed, unless marked as stealable (Stealable rares)
				else if ( toSteal.LootType == LootType.Newbied || toSteal.CheckBlessed( root ) && !ItemFlags.GetStealable(toSteal) )
				{
                    m_Thief.SendMessage("That item is blessed");
					//m_Thief.SendLocalizedMessage( 502710 ); // You can't steal that!
				}
                
                //Too Far Away
				else if ( !m_Thief.InRange( toSteal.GetWorldLocation(), 1 ) )
				{
					m_Thief.SendLocalizedMessage( 502703 ); // You must be standing next to an item to steal it.
				}

                //Artifact Spawner
				else if ( si != null && m_Thief.Skills[SkillName.Stealing].Value < 100.0 )
				{
					m_Thief.SendLocalizedMessage( 1060025, "", 0x66D ); // You're not skilled enough to attempt the theft of this item.
				}

                //Item is Equipped
				else if ( toSteal.Parent is Mobile )
				{
					m_Thief.SendLocalizedMessage( 1005585 ); // You cannot steal items which are equiped.
				}

                //Target is Self
				else if ( root == m_Thief )
				{
					m_Thief.SendLocalizedMessage( 502704 ); // You catch yourself red-handed.
				}

                //Staff Member
				else if ( root is Mobile && ((Mobile)root).AccessLevel > AccessLevel.Player )
				{
					m_Thief.SendLocalizedMessage( 502710 ); // You can't steal that!
				}

                //Cant Harm Target
				else if ( root is Mobile && !m_Thief.CanBeHarmful( (Mobile)root ) )
				{
				}

                //Corpse
                else if (root is Corpse)
                {
                    m_Thief.SendLocalizedMessage(502710); // You can't steal that!
                }
               
                //Valid Stealing Attempt
                else
                {
                    int itemCount = toSteal.Amount;
                    double itemWeight = toSteal.Weight;
                    double totalWeight = (double)itemCount * itemWeight;

                    //Stackable Items
                    if (toSteal.Stackable && toSteal.Amount > 1)
                    {
                        double stackStealMaxWeight = maxWeightAllowedOnSuccess / FeatureList.Stealing.StackWeightStealDivider;
                        int amountStealable = (int)(Math.Floor(stackStealMaxWeight / itemWeight));

                        //Success
                        if (amountStealable > 0)
                        {
                            //If Able to Steal More than Exists
                            if (amountStealable > itemCount)
                            {
                                amountStealable = itemCount;
                            }

                            int stolenItemAmount = toSteal.Amount - amountStealable;                          

                            //Create New Item Stack and Delete Stolen Amount From Target's Stack

                            stolen = Mobile.LiftItemDupe(toSteal, stolenItemAmount);

                            //Delete Item if at 0 Amount After Dupe and Reduction
                            if (toSteal.Amount == 0)
                            {
                                toSteal.Delete();
                            }

                            if (stolen == null)
                            {
                                stolen = toSteal;
                            }
                        }

                        //Can't Steal Any
                        else
                        {
                            m_Thief.SendLocalizedMessage(502723); // You fail to steal the item.
                        }
                    }

                    //Single Item
                    else
                    {
                        //Success
                        if (maxWeightAllowedOnSuccess >= toSteal.TotalWeight && maxWeightAllowedOnSuccess >= toSteal.Weight)
                        {
                            stolen = toSteal;
                        }

                        else
                        {
                            m_Thief.SendLocalizedMessage(502723); // You fail to steal the item.
                        }
                    }

                    //Region Handling
                    Region region = Region.Find(m_Thief.Location, m_Thief.Map);
                    CommonwealthRegion township = region as CommonwealthRegion;

                    int caughtRollMax = 150;

                    if (township != null)
                    {
                        if ((township.MyCommonwealth.State.GuardSettings & (int)GuardTypes.WatchfulGuards) != 0)
                        {
                            caughtRollMax = FeatureList.Citizenship.StealingMaxRollUnderWatchfulGuards;
                        }
                    }

                    caught = (m_Thief.Skills[SkillName.Stealing].Value < Utility.Random(caughtRollMax));

                    PlayerMobile thief = m_Thief as PlayerMobile;

                    //Stealable rares handling
                    if(stolen != null)
                    {
                        ItemFlags.SetTaken(stolen, true); //set the taken flag to trigger release from any controlling spawner
                        ItemFlags.SetStealable(stolen, false); //clear the stealable flag so the item cannot be stolen if locked down
                        stolen.Movable = true;
                        //These next operations i could not find in our code, which I assume is an error (pan)
                        if(si != null)
                        {
                            toSteal.Movable = true;
                            si.Item = null;
                        }
                    }

                    //Stat Tracking
                    if (thief != null)
                    {
                        thief.StealAttemptsThisSession++;
                    }

                    if (thief != null && stolen != null)
                    {
                        thief.SuccessfulStealsThisSession++;
                    }
                } 

				return stolen;
			}

			protected override void OnTarget( Mobile from, object target )
			{
				Item stolen = null;
				object root = null;
				bool caught = false;   

				if ( target is Item )
				{
                    root = ((Item)target).RootParent;
                    if (root != null && root is Mobile && ((Mobile)root).Hidden)
                    {
                        m_Thief.SendMessage("You can't see it well enough to steal it!");
                        return;
                    }
                    from.RevealingAction();
                    
					stolen = TryStealItem( (Item)target, ref caught );
				} 

				else if ( target is PlayerMobile )
				{
                    if (((Mobile)target).Hidden)
                    {
                        m_Thief.SendMessage("You can't see them well enough to steal from them!");
                        return;
                    }
                    from.RevealingAction();
                    Container pack = ((Mobile)target).Backpack;

					if ( pack != null && pack.Items.Count > 0 )
					{
						int randomIndex = Utility.Random( pack.Items.Count );

						root = target;
						stolen = TryStealItem( pack.Items[randomIndex], ref caught );
					}
				} 

                //Invalid Item
				else 
				{
					m_Thief.SendLocalizedMessage( 502710 ); // You can't steal that!
				}

                //Item is Valid
				if ( stolen != null )
				{
                    //Check If Can Be Put in Pack 
                    if (m_Thief.Backpack.CheckHold( m_Thief, stolen, false, true))
                    {
                        from.AddToBackpack( stolen );
                        StolenItem.Add( stolen, m_Thief, root as Mobile );

                        m_Thief.SendLocalizedMessage(502724); // You succesfully steal the item.
                    }

                    //Can't Hold Item in Pack (Exceeds Weight or Item Limit)
                    else
                    {
                        //Drop to Ground
                        stolen.MoveToWorld(from.Location, from.Map);
                        from.SendMessage("Unable to find room in your pack, you drop the item to the ground");
                    } 
				}

                //Caught During Check
				if ( caught )
				{
					if ( root == null )
					{
						m_Thief.CriminalAction( false, true );
					}

					else if ( root is Corpse && ((Corpse)root).IsCriminalAction( m_Thief ) )
					{
						m_Thief.CriminalAction( false, true );
					}

					else if ( root is Mobile )
					{
						Mobile mobRoot = (Mobile)root;

						if ( IsInnocentTo( m_Thief, mobRoot ) )
							m_Thief.CriminalAction( false, true );

						string message = String.Format( "You notice {0} trying to steal from {1}.", m_Thief.Name, mobRoot.Name );

						foreach ( NetState ns in m_Thief.GetClientsInRange( 8 ) )
						{
							if ( ns.Mobile != m_Thief )
								ns.Mobile.SendMessage( message );
						}
					}
				}

				else if ( root is Corpse && ((Corpse)root).IsCriminalAction( m_Thief ) )
				{
					m_Thief.CriminalAction( false, true );
				}

				if ( root is Mobile && ((Mobile)root).Player && m_Thief is PlayerMobile && IsInnocentTo( m_Thief, (Mobile)root ) )
				{
					PlayerMobile pm = (PlayerMobile)m_Thief;

					pm.PermaFlags.Add( (Mobile)root );
					pm.Delta( MobileDelta.Noto );
				}
			}
		}

		public static bool IsEmptyHanded( Mobile from )
		{
			if ( from.FindItemOnLayer( Layer.OneHanded ) != null )
				return false;

			if ( from.FindItemOnLayer( Layer.TwoHanded ) != null )
				return false;

			return true;
		}

		public static TimeSpan OnUse( Mobile m )
		{
			if ( !IsEmptyHanded( m ) )
			{
				m.SendLocalizedMessage( 1005584 ); // Both hands must be free to steal.
			}
			else
			{
				m.Target = new Stealing.StealingTarget( m );
				m.RevealingAction();

				m.SendLocalizedMessage( 502698 ); // Which item do you want to steal?
			}

			return TimeSpan.FromSeconds( 10.0 );
		}
	}

    public class StolenItem
    {
        public static readonly TimeSpan StealTime = TimeSpan.FromSeconds(30); // was 2 minutes, now 30 seconds

        private Item m_Stolen;
        private Mobile m_Thief;
        private Mobile m_Victim;
        private DateTime m_Expires;

        public Item Stolen { get { return m_Stolen; } }
        public Mobile Thief { get { return m_Thief; } }
        public Mobile Victim { get { return m_Victim; } }
        public DateTime Expires { get { return m_Expires; } }

        public bool IsExpired { get { return (DateTime.Now >= m_Expires); } }

        public StolenItem(Item stolen, Mobile thief, Mobile victim)
        {
            m_Stolen = stolen;
            m_Thief = thief;
            m_Victim = victim;

            m_Expires = DateTime.Now + StealTime;
        }

        private static ArrayList m_Queue = new ArrayList();

        public static void Add(Item item, Mobile thief, Mobile victim)
        {
            Clean();

            m_Queue.Add(new StolenItem(item, thief, victim));
        }

        public static void Remove(Item item)
        {
            StolenItem itemToRemove = null;
            foreach (StolenItem stolenItem in m_Queue)
            {
                if (stolenItem.m_Stolen == item)
                {
                    itemToRemove = stolenItem;
                    break;
                }
            }
            if (itemToRemove != null)
            {
                m_Queue.Remove(itemToRemove);
            }
        }

        public static bool IsStolen(Item item)
        {
            Mobile victim = null;

            return IsStolen(item, ref victim);
        }

        public static bool IsStolen(Item item, ref Mobile victim)
        {
            Clean();

            foreach (StolenItem si in m_Queue)
            {
                if (si.m_Stolen == item && !si.IsExpired)
                {
                    victim = si.m_Victim;
                    return true;
                }
            }

            return false;
        }

        public static void ReturnOnDeath(Mobile killed, Container corpse)
        {
            Clean();

            foreach (StolenItem si in m_Queue)
            {
                if (si.m_Stolen.RootParent == corpse && si.m_Victim != null && !si.IsExpired)
                {
                    if (si.m_Victim.AddToBackpack(si.m_Stolen))
                        si.m_Victim.SendLocalizedMessage(1010464); // the item that was stolen is returned to you.
                    else
                        si.m_Victim.SendLocalizedMessage(1010463); // the item that was stolen from you falls to the ground.

                    si.m_Expires = DateTime.Now; // such a hack
                }
            }
        }

        public static void Clean()
        {
            while (m_Queue.Count > 0)
            {
                StolenItem si = (StolenItem)m_Queue[0];

                if (si.IsExpired)
                {
                    m_Queue.RemoveAt(0);
                }
                else
                    break;
            }
        }
    }
}