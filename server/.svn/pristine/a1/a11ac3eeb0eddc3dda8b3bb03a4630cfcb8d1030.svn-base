using System;
using Server.Items;
using Server.Factions;
using Server.Mobiles;
using Server.Multis;
using Server.Targeting;
using Server.Regions;
using Server.Scripts;

namespace Server.SkillHandlers
{
	public class DetectHidden
	{
		public static void Initialize()
		{
			SkillInfo.Table[(int)SkillName.DetectHidden].Callback = new SkillUseCallback( OnUse );
		}

		public static TimeSpan OnUse( Mobile src )
		{
            src.RevealingAction();
			src.SendLocalizedMessage( 500819 ); //Where will you search?
			src.Target = new InternalTarget();
            
			return TimeSpan.FromSeconds( 6.0 );
		}

		private class InternalTarget : Target
		{
			public InternalTarget() : base( 12, true, TargetFlags.None )
			{
			}

			protected override void OnTarget( Mobile detecter, object target )
			{
				bool foundAnyone = false;

				Point3D p;

                if (target is Mobile)
                {
                    p = ((Mobile)target).Location;
                }

                else if (target is Item)
                {
                    p = ((Item)target).Location;
                }

                else if (target is IPoint3D)
                {
                    p = new Point3D((IPoint3D)target);
                }

                else
                    p = detecter.Location;

                int detectRange = (int)(Math.Floor(detecter.Skills[SkillName.DetectHidden].Value / 25));
                int houseDetectRange = 0;

                //Skill Gain Check
                detecter.CheckSkill(SkillName.DetectHidden, 0.0, 100.0);

                BaseHouse house = BaseHouse.FindHouseAt(p, detecter.Map, 16);

				bool inHouse = ( house != null && house.IsFriend( detecter ) );

				if ( inHouse )
                    houseDetectRange = 22;               
                               
                //Normal Detect Hidden
                if (detectRange > 0)
                {
                    int effectiveRange = detectRange - 1;

                    IPooledEnumerable inRange = detecter.Map.GetMobilesInRange(p, effectiveRange);

                    foreach (Mobile mobile in inRange)
                    {
                        if (mobile.Hidden && detecter != mobile)
                        {
                            if (detecter.AccessLevel >= mobile.AccessLevel)
                            {
                                mobile.RevealingAction();
                                mobile.SendLocalizedMessage(500814); // You have been revealed!

                                foundAnyone = true;
                            }
                        }
                    }

                    inRange.Free();
                }

                //House Detect Hidden
                if (houseDetectRange > 0)
                {
                    IPooledEnumerable inHouseRange = detecter.Map.GetMobilesInRange(p, houseDetectRange);

                    foreach (Mobile mobile in inHouseRange)
                    {
                        if (mobile.Hidden && detecter != mobile)
                        {
                            //Mobile and Target Are Both Inside The House
                            if (detecter.AccessLevel >= mobile.AccessLevel && (inHouse && house.IsInside(mobile)))
                            {
                                mobile.RevealingAction();
                                mobile.SendLocalizedMessage(500814); // You have been revealed!

                                foundAnyone = true;
                            }
                        }
                    }

                    inHouseRange.Free();
                }

                //Faction Detect Hidden
                if (Faction.Find(detecter) != null)
				{
                    IPooledEnumerable itemsInRange = detecter.Map.GetItemsInRange(p, detectRange);

					foreach ( Item item in itemsInRange )
					{
						if ( item is BaseFactionTrap )
						{
							BaseFactionTrap trap = (BaseFactionTrap) item;
                                
                            if (detecter.CheckTargetSkill(SkillName.DetectHidden, trap, 80.0, 100.0))
							{
                                detecter.SendLocalizedMessage(1042712, true, " " + (trap.Faction == null ? "" : trap.Faction.Definition.FriendlyName)); // You reveal a trap placed by a faction:

								trap.Visible = true;
								trap.BeginConceal();

								foundAnyone = true;
							}
						}
					}

					itemsInRange.Free();
				}				

                //Didn't Reveal Anyone
				if ( !foundAnyone )
				{
                    detecter.SendLocalizedMessage(500817); // You can see nothing hidden there.
				}
			}
		}
	}
}
