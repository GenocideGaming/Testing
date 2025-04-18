using System;
using System.Collections;
using System.Collections.Generic;
using Server.Accounting;
using Server.Commands;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Items;

namespace Server.Custom
{
    public class EventGate : Moongate
    {
        public EventGate PartnerGate = null;
        
        [Constructable]
		public EventGate(bool bDispellable) : base( bDispellable )
		{
		}

        public EventGate(Serial serial)
            : base(serial)
		{
		}

        public override void Serialize(GenericWriter writer)
        {
            // do not save event gates!
        }

        public override void Deserialize(GenericReader reader)
        {
            // do not load event gates!
        }

        public override void OnDelete()
        {
            if (PartnerGate != null)
            {
                PartnerGate.PartnerGate = null;
                PartnerGate.Delete();
            }
            base.OnDelete();
        }
    }

    
    public class EventGates
    {
        public static Point3D ReturnLocation = new Point3D(1246, 1445, 0); // galven outside
        public const int EventGateHue = 38;
        public const int SpecialGateHue = 20;

        public static Point3D[] TownGateLocations = new Point3D[] {
            new Point3D(546,1354,0), // lillano
            new Point3D(999,1894,0), // calor
            new Point3D(1331,1376,0), // galven
            new Point3D(1775,1064,0), // arbor
            new Point3D(1685,1665,0), // vermell
            new Point3D(843,858,0), // pedran
            new Point3D(1196,865,2), // albus
            new Point3D(838,1770,0), // bowen
            new Point3D(1370,1934,0) // roache
        };

        public static List<EventGate> CurrentEventGates = new List<EventGate>();
        
        public static void Initialize()
        {
            CommandSystem.Register("eventgates", AccessLevel.GameMaster, new CommandEventHandler(EventGates_Command));
        }

        public static void EventGates_Command(CommandEventArgs e)
        {
            string[] args = e.Arguments;
            if (args != null)
            {
                string error_msg = "You must specify open/close OR a specific location!  E.g. '[eventgates open' or '[eventgates close' OR [eventgates 50 100 0 for x=50 y=100 z=0";
                if (args.Length > 0)
                {
                    if (args[0] == "open")
                    {
                        while (CurrentEventGates.Count != 0) // delete current gates
                        {
                            CurrentEventGates[0].Delete();
                            CurrentEventGates.RemoveAt(0);
                        }
                        Point3D targetlocation = e.Mobile.Location;
                        Server.Commands.CommandHandlers.BroadcastMessage(AccessLevel.Player, 0x30, "The Galven gate authority has opened the town portals near each bank--Enter at your own risk!");
                        foreach (Point3D townPortalLocation in TownGateLocations)
                        {
                            EventGate newGate = new EventGate(false);
                            newGate.Map = Map.Felucca;
                            newGate.Location = townPortalLocation;
                            newGate.TargetMap = Map.Felucca;
                            newGate.Target = e.Mobile.Location;
                            newGate.Hue = EventGateHue;
                            CurrentEventGates.Add(newGate);
                        }
                        EventGate returnGate = new EventGate(false);
                        returnGate.Map = Map.Felucca;
                        returnGate.Location = e.Mobile.Location;
                        returnGate.TargetMap = Map.Felucca;
                        returnGate.Target = ReturnLocation;
                        returnGate.Hue = EventGateHue;
                        CurrentEventGates.Add(returnGate);
                    }
                    else if (args[0] == "close")
                    {
                        while (CurrentEventGates.Count != 0)
                        {
                            CurrentEventGates[0].Delete();
                            CurrentEventGates.RemoveAt(0);
                        }
                        Server.Commands.CommandHandlers.BroadcastMessage(AccessLevel.Player, 0x30, "The Galven gate authority has closed the town portals.");
                    }
                    else if (args.Length > 2) // expected x y z
                    {
                        try
                        {
                            int x = int.Parse(args[0]);
                            int y = int.Parse(args[1]);
                            int z = int.Parse(args[2]);
                            EventGate returnGate = new EventGate(false);
                            returnGate.Map = Map.Felucca;
                            returnGate.Location = e.Mobile.Location;
                            returnGate.TargetMap = Map.Felucca;
                            returnGate.Target = new Point3D(x,y,z);
                            returnGate.Hue = SpecialGateHue;
                            EventGate toGate = new EventGate(false);
                            toGate.Map = Map.Felucca;
                            toGate.Location = new Point3D(x,y,z);
                            toGate.TargetMap = Map.Felucca;
                            toGate.Target = e.Mobile.Location;
                            toGate.Hue = SpecialGateHue;
                            returnGate.PartnerGate = toGate;
                            toGate.PartnerGate = returnGate;
                            string ToGateName = "";
                            for (int i = 3; i < args.Length; i++) 
                            {
                                ToGateName += args[i] + " ";
                            }
                            if (ToGateName != "")
                            {
                                toGate.Name = ToGateName;
                            }
                        }
                        catch
                        {
                            e.Mobile.SendMessage(error_msg);
                        }
                    }
                    else
                    {
                        e.Mobile.SendMessage(error_msg);
                    }
                }
                else
                {
                    e.Mobile.SendMessage(error_msg);
                }
            }
        }
    }
}
