using System;
using System.Collections.Generic;
using System.Text;
using Server;
using Server.Mobiles;

namespace Server.Scripts.Custom.BountySystem
{
    class BountyRegistryPersistance : Item
    {
        private static BountyRegistryPersistance m_Instance;
        public static BountyRegistryPersistance Instance { get { return m_Instance; } }
        public override string DefaultName { get { return "Bounty Registry - Internal"; } }

        public BountyRegistryPersistance()
            : base(1)
        {
            Movable = false;

            if (m_Instance == null || m_Instance.Deleted)
                m_Instance = this;
            else
                base.Delete();
        }

        public BountyRegistryPersistance(Serial serial)
            : base(serial)
        {
            m_Instance = this;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version

            try
            {
                BountyRegistry.PruneOlderThan(FeatureList.MurderSystem.TimeToStayOnBountyBoard);
            }
            catch (Exception e)
            {
                Console.WriteLine("Something went wrong while trying to prune the bounty board. Full details:");
                Console.WriteLine(e.Message);
            }

            int bountyCount = BountyRegistry.Postings.Length;

            writer.Write(bountyCount);

            foreach (BountyPosting posting in BountyRegistry.Postings)
            {
                writer.Write(posting.Killer);
                writer.Write(posting.Bounty);
                writer.Write(posting.PostedAt);
            }
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            BountyRegistry.Clear();

            switch (version)
            {
                case 0:
                    {
                        int playerCount = reader.ReadInt();

                        for (int i = 0; i < playerCount; i++)
                        {
                            PlayerMobile killer = (PlayerMobile)reader.ReadMobile();
                            int bounty = reader.ReadInt();
                            DateTime postedAt = reader.ReadDateTime();

                            if (killer != null)
                                BountyRegistry.Add(killer, bounty, postedAt);
                        }

                        break;
                    }
            }
        }
    }

    class BountyPosting: IComparable
    {
        public static void Initialize()
        {
            new BountyRegistryPersistance();
        }

        PlayerMobile m_killer;
        int m_Bounty;
        DateTime m_postedAt;

        public PlayerMobile Killer { get { return m_killer; } }
        public int Bounty { get { return m_Bounty; } set { m_Bounty = value; } }
        public DateTime PostedAt { get { return m_postedAt; } set { m_postedAt = value; } }
        public string LastActive
        {
            get
            {
                TimeSpan timeSince = DateTime.Now - m_postedAt;

                if (timeSince.TotalDays < 1)
                    return "Today";
                else if (timeSince.TotalDays < 2)
                    return "1 day ago";
                else
                    return Math.Floor(timeSince.TotalDays).ToString() + " days ago";
            }
        }

        public BountyPosting(PlayerMobile killer, int bounty, DateTime postedAt)
        {
            m_killer = killer;
            m_Bounty = bounty;
            m_postedAt = postedAt;
        }

        public BountyPosting(PlayerMobile killer, int bounty) : this(killer, bounty, DateTime.Now) { }

        public int CompareTo(object obj)
        {
            BountyPosting otherPost = obj as BountyPosting;

            if (this.Bounty < otherPost.Bounty)
                return 1;
            else if (this.Bounty > otherPost.Bounty)
                return -1;
            else
                return 0;
        }
    }

    static class BountyRegistry
    {
        static Dictionary<PlayerMobile, BountyPosting> m_Postings = new Dictionary<PlayerMobile, BountyPosting>();

        public static BountyPosting[] Postings
        {
            get
            {
                BountyPosting[] sortedList = new BountyPosting[m_Postings.Count];
                m_Postings.Values.CopyTo(sortedList, 0);

                Array.Sort(sortedList);
                return sortedList;
            }
        }

        public static void Add(PlayerMobile killer, int bounty, DateTime postedAt)
        {
            if (m_Postings.ContainsKey(killer))
            {
                m_Postings[killer].Bounty = bounty;
                m_Postings[killer].PostedAt = postedAt;
            }
            else
            {
                m_Postings.Add(killer, new BountyPosting(killer, bounty, postedAt));
            }
        }

        public static void ClearBounty(PlayerMobile killer)
        {
            if (m_Postings.ContainsKey(killer))
            {
                m_Postings.Remove(killer);
            }
        }

        public static void Clear()
        {
            m_Postings.Clear();
        }

        public static void PruneOlderThan(TimeSpan validity)
        {
            List<PlayerMobile> toBeRemoved = new List<PlayerMobile>();
            foreach (KeyValuePair<PlayerMobile, BountyPosting> post in m_Postings)
            {
                if (post.Value.PostedAt < (DateTime.Now - validity))
                    toBeRemoved.Add(post.Key);
            }

            foreach(PlayerMobile removeMe in toBeRemoved)
                m_Postings.Remove(removeMe);
        }

        public static bool Contains(PlayerMobile player)
        {
            if (m_Postings.ContainsKey(player))
                return true;
            else
                return false;
        }

        public static int FindBountyByName(string playerName)
        {
            PlayerMobile foundPlayer = null;
            BountyPosting foundBountyPosting = null;
            int index = -1;
            int currentIndex = 0;

            // Iterate through the dictionary
            foreach (var kvp in m_Postings)
            {
                if (kvp.Key.Name.Equals(playerName, StringComparison.OrdinalIgnoreCase))
                {
                    foundPlayer = kvp.Key;
                    foundBountyPosting = kvp.Value;
                    index = currentIndex;
                    break;
                }
                currentIndex++;
            }

            if (foundPlayer != null)
            {
                return foundBountyPosting.Bounty;
            }
            
            return -1;
        }
    }
}
