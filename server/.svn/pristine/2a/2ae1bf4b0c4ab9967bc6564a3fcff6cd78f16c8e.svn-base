using System;
using System.Collections.Generic;
using System.Text;
using Server.Items;
using Server.Factions;
using Server.Network;

namespace Server.Gumps
{
    class WorldWarsResultsGump : Gump
    {
        public WorldWarsResultsGump(Dictionary<Faction, int> scores) : base( 100, 100 )
        {
            this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;

            if (scores == null)
                return;

			AddPage(0);
			AddBackground(0, 0, 217, 359, 9250);
			AddLabel(26, 16, 0, @"Thank you for participating ");
			AddLabel(49, 35, 0, @"in the World Wars!");

            int y1 = 95;
            int y2 = 115;
            int i = 0;
            foreach (Faction militia in scores.Keys)
            {
                AddLabel(19, y1 + i * 50, 0, militia.Definition.TownshipName);
                AddLabel(19, y2 + i * 50, 0, "Points: "+scores[militia].ToString());
                i++;
            }
            
        }

        

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

        }
    }
}
