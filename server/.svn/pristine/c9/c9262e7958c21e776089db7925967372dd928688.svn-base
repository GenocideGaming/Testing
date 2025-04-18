using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Mobiles;

namespace Server.Items
{
    class HeadRegistryPersistance : Item
    {
        private static HeadRegistryPersistance m_Instance;
        public static HeadRegistryPersistance Instance { get { return m_Instance; } }
        public override string DefaultName { get { return "Severed Head Registry - Internal"; } }

        public HeadRegistryPersistance() : base(1)
        {
            Movable = false;

            if (m_Instance == null || m_Instance.Deleted)
                m_Instance = this;
            else
                base.Delete();
        }

        public HeadRegistryPersistance(Serial serial) : base(serial)
        {
            m_Instance = this;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version

            int playerCount = HeadRegistry.Heads.Count;
            writer.Write(playerCount);

            foreach (KeyValuePair<Serial, List<Serial>> entry in HeadRegistry.Heads)
            {
                writer.Write(entry.Key);

                int headCount = entry.Value.Count;
                writer.Write(headCount);

                foreach (Serial headSerial in entry.Value)
                {
                    writer.Write(headSerial);
                }
            }
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 0:
                    {
                        int playerCount = reader.ReadInt();

                        for (int i =0; i < playerCount; i++)
                        {
                            Serial headOwner = (Serial)reader.ReadInt();

                            int headCount = reader.ReadInt();

                            for (int j = 0; j < headCount; j++)
                            {
                                Serial headSerial = (Serial)reader.ReadInt();

                                if (headOwner != null)
                                    HeadRegistry.Add(headOwner, headSerial);
                            }        
                        }

                        break; 
                    }
            }
        }

        public override void Delete()
        {
        }
    }

    static class HeadRegistry 
    {
        public static void Initialize()
        {
            new HeadRegistryPersistance();
        }

        private static Dictionary<Serial, List<Serial>> m_Heads = new Dictionary<Serial, List<Serial>>();

        [CommandProperty(AccessLevel.GameMaster)]
        public static Dictionary<Serial, List<Serial>> Heads
        {
            get { return m_Heads; }
        }

        public static List<Serial> EveryHeadOf(Serial playerSerial)
        {
            if (!m_Heads.ContainsKey(playerSerial))
                return new List<Serial>();

            return m_Heads[playerSerial];
        }

        public static List<Serial> EveryHeadOf(PlayerMobile player)
        {
            if (player == null)
                return new List<Serial>();

            return EveryHeadOf(player.Serial);
        }

        public static void RemoveAllHeadsOf(PlayerMobile player)
        {
            if (player == null)
                return;

            RemoveAllHeadsOf(player.Serial);
        }

        public static void RemoveAllHeadsOf(Serial playerSerial)
        {
            if (!m_Heads.ContainsKey(playerSerial))
                return;

            m_Heads.Remove(playerSerial);
        }

        public static void Add(Serial headOwner, Serial headSerial)
        {
            if (m_Heads.ContainsKey(headOwner))
                m_Heads[headOwner].Add(headSerial);
            else
            {
                m_Heads.Add(headOwner, new List<Serial>());
                m_Heads[headOwner].Add(headSerial);
            }
        }
    }
}
