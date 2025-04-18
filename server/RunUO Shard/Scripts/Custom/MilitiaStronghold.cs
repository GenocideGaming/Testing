using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Server.Commands;
using Server.Items;
using Server.Mobiles;
using Server.Gumps;
using Server.Scripts.Custom.Citizenship;
using Server.Scripts.Custom;
using Server.Factions;
using Server.Ethics;

namespace Server.Regions
{
    public class MilitiaStronghold : BaseRegion
    {
        /* 
            If FlagCriminal_NonCitizens is marked true, then players will be marked criminal if they are not a citizen of the township associated with a faction

            If this is marked false, then it will only check if a player is not in a faction, if they are not then citizenship is not checked, and will be marked criminal

            Once marked criminal, the FactionRegionCheckTimer triggers every 5 seconds to check if the player is:
            1) Still in the stronghold region
            2) If they are now a faction member
            3) If FlagCriminal_NonCitizens is true, then it will check if they are a Citizen in an opposing stronghold region

            The time renews the Criminal flag, it does not remove that. That is handled under the normal Criminal removal timer.
        */
        private const bool FlagCriminal_NonCitizens = false;

        private int mRegionId;
        private const float mTakeOverTimeInSeconds = 10f;
        private ICommonwealth mOwner;

        public ICommonwealth Owner { get { return mOwner; } set { mOwner = value; } }

        public MilitiaStronghold(ICommonwealth owner) :
            base("Test Stronghold", Map.Felucca, 0, new Rectangle2D[] { new Rectangle2D(500, 500, 10, 10) })
        {
            mOwner = owner;
            mOwner.HeroHomeRegion = this;
        }

        public MilitiaStronghold(XmlElement xml, Map map, Region parent)
            : base(xml, map, parent)
        {
            XmlElement id = xml["heroinfo"];
            ReadInt32(id, "id", ref mRegionId);

            XmlElement townshipElement = xml["townshipinfo"];
            string townshipName = string.Empty;
            ReadString(townshipElement, "name", ref townshipName);
            mOwner = Commonwealth.Find(townshipName);
            mOwner.HeroHomeRegion = this;
        }

        public class FactionRegionCheckTimer : Timer
        {
            private Mobile m_Player;
            private Region m_Region;

            public FactionRegionCheckTimer(Mobile player, Region region) : base(TimeSpan.FromSeconds(5.0), TimeSpan.FromSeconds(5.0))
            {
                m_Player = player;
                m_Region = region;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Player != null && m_Region != null)
                {
                    // Check for non-faction members in a stronghold region, if they are still in it then their Criminal flag will be renewed
                    if (m_Player.Region == m_Region && Faction.Find(m_Player) == null)
                    {
                        PlayerMobile pm = (PlayerMobile)m_Player;

                        // If FlagCriminal_NonCitizens is marked true (see line 30), this will check if a non-faction player is in the faction stronghold associated with their Citizenship to prevent marking criminal
                        if(FlagCriminal_NonCitizens)
                        {    
                            if(m_Region.Name == "YewStronghold" && pm.Citizenship == "Yew")
                                Stop();

                            else if(m_Region.Name == "BlackrockStronghold" && pm.Citizenship == "Blackrock")
                                Stop();

                            else if(m_Region.Name == "TrinsicStronghold" && pm.Citizenship == "Trinsic")
                                Stop();
                            
                            else
                                m_Player.Criminal = true;
                        }
                        
                        m_Player.Criminal = true;
                    }
                    else
                    {
                        Stop();
                    }
                }
                else
                {
                    Stop();
                }
            }
        }

        public override void OnEnter(Mobile m)
        {
            if (m is PlayerMobile)
            {
                m.SendMessage(43, "Entering the faction stronghold of " + mOwner.Definition.TownName);
                
                if (m.AccessLevel > AccessLevel.Player)
                    return;

                // If a non-faction member enters a stronghold, they will be marked criminal a timer is started to re-check if they are still in the stronghold region
                if (Faction.Find(m) == null)
                {
                    PlayerMobile pm = (PlayerMobile)m;

                    // If FlagCriminal_NonCitizens is marked true (see line 30), this will check if a non-faction player is in the faction stronghold associated with their Citizenship to prevent marking criminal
                    if(FlagCriminal_NonCitizens)
                    {    
                        if(this.Name == "YewStronghold" && pm.Citizenship == "Yew")
                            return;

                        else if(this.Name == "BlackrockStronghold" && pm.Citizenship == "Blackrock")
                            return;

                        else if(this.Name == "TrinsicStronghold" && pm.Citizenship == "Trinsic")
                            return;
                    }

                    m.SendMessage(28, "You have trespassed on a faction stronghold and have been flagged as criminal");
                    m.Criminal = true;
                    Timer timer = new FactionRegionCheckTimer(m, this);
                    timer.Start();
                }
            }    
        }

        public override void OnExit(Mobile m)
        {
            if(m is PlayerMobile)
                m.SendMessage(43, "Exiting the faction stronghold of " + mOwner.Definition.TownName);
        }

        private void MakeUnCriminal(Mobile m)
        {
            m.SendMessage("You are no longer considered a criminal.");
            m.Criminal = false;
        }
    }
}

