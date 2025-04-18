using System;
using System.Collections.Generic;
using System.Text;
using Server.Commands;
using Server.Gumps;
using Server.Items;
using Server.Factions;
using Server.Network;

namespace Server.Scripts.Custom.WorldWars
{
    public class WorldWarsGump : Gump
    {
        Mobile caller;

        public static void Initialize()
        {
            CommandSystem.Register("wwmap", AccessLevel.Player, new CommandEventHandler(wwgump_OnCommand));
        }

        [Usage("[wwgump")]
        [Description("Opens a map of vorshun, showing who owns which flag.")]
        public static void wwgump_OnCommand(CommandEventArgs e)
        {
            if (e.Mobile.HasGump(typeof(WorldWarsGump)))
                e.Mobile.CloseGump(typeof(WorldWarsGump));
            e.Mobile.SendGump(new WorldWarsGump(e.Mobile));
        }

        public WorldWarsGump(Mobile from)
            : this()
        {
            caller = from;
        }
        //These positions correspond to the flag order in WorldWarsController
        public static Point2D[] FlagPositions = new Point2D[]{new Point2D(45, 77), new Point2D(211, 23), new Point2D(352, 53), new Point2D(317, 243), 
                                                                new Point2D(194, 217), new Point2D(298, 162), new Point2D(95, 142), new Point2D(215, 131)};

        public static Point2D[] LabelPositions = new Point2D[] { new Point2D(39, 111), new Point2D(206, 53), new Point2D(338, 88), new Point2D(310, 281),
                                                                new Point2D(189, 249), new Point2D(293, 198), new Point2D(87, 186), new Point2D(207, 165) };

        public WorldWarsGump()
            : base(50, 50)
        {
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;

            AddPage(0);
            AddBackground(0, 0, 420, 350, 9250);
            AddBackground(12, 11, 395, 313, 9350);
            AddImage(19, 17, 6002);

            if (WorldWarsController.Instance == null)
                return;
             AddLabel(15, 321, 0, @"Time left: " + WorldWarsController.Instance.GameTimeLeft.Minutes + " minutes"); 
            WorldWarsFlag[] flags = WorldWarsController.Instance.GetFlags();
		   if(flags == null)
			return;
            for (int i = 0; i < flags.Length; i++)
            {
                int hue = flags[i] != null ? (flags[i].Militia != null ? flags[i].Militia.Definition.HuePrimary : 0) : 0;
                string label = flags[i] != null ? (flags[i].Militia != null ? flags[i].Militia.Definition.TownshipName : "None") : "None";
                AddImage(FlagPositions[i].X, FlagPositions[i].Y, 6001, hue);
                AddLabel(LabelPositions[i].X, LabelPositions[i].Y, 0x9C2, label);
            }

           
        }



        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;
        }
    }
}
