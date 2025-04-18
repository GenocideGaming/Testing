using System;
using Server;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Factions
{
    public class TownStoneGump : FactionGump
    {
        private PlayerMobile m_From;
        private Faction m_Faction;
        private Town m_Town;

        public TownStoneGump(PlayerMobile from, Faction faction, Town town)
            : base(50, 50)
        {
            m_From = from;
            m_Faction = faction;
            m_Town = town;

            AddPage(0);

            AddBackground(0, 0, 320, 250, 5054);
            AddBackground(10, 10, 300, 230, 3000);

            AddHtmlText(25, 30, 250, 25, town.Definition.TownStoneHeader, false, false);

            AddHtmlLocalized(55, 210, 150, 25, 1011441, false, false); // EXIT
            AddButton(20, 210, 4005, 4007, 0, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (m_Town.Militia != m_Faction || !m_Faction.IsCommander(m_From))
            {
                m_From.SendLocalizedMessage(1010339); // You no longer control this city
                return;
            }
        }
    }
}