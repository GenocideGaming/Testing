using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using System.IO;

namespace Server.Scripts.Custom.VorshunWarsEquipment
{
    static class VorshunStorage
    {
        public static void Initialize() { new VorshunStoragePersistance(); }

        static Dictionary<Mobile, List<Item>> m_Storage = new Dictionary<Mobile, List<Item>>();
        static public Dictionary<Mobile, List<Item>> Storage { get { return m_Storage; } }


        public static void Store(Mobile owner)
        {
            if (m_Storage.ContainsKey(owner))
                return;

            new DefaultItemsTimer(owner).Start();
        }

        public static List<Item> Retrieve(Mobile owner)
        {
            if (!m_Storage.ContainsKey(owner))
                return null;

            return m_Storage[owner];
        }

        public static bool ContainsStuffBelongingTo(Mobile owner)
        {
            return m_Storage.ContainsKey(owner);
        }

        public static void Add(Mobile owner, List<Item> stuff)
        {
            m_Storage.Add(owner, stuff);
        }

        internal static void Clear(Mobile m)
        {
            m_Storage.Remove(m);
        }

        public static void CleanUp()
        {
            if (WorldWarsController.Instance == null) return;
            foreach (KeyValuePair<Mobile, List<Item>> entry in m_Storage)
            {
                PlayerMobile player = entry.Key as PlayerMobile;
                if (player == null || player.Deleted) continue;

                player.MoveToWorld(WorldWarsController.Instance.GetRemoveToLocation(player), Map.Felucca);
                VorshunStorage.RestoreOriginalItems(player);
            }
        }

        public static void LogStorage(Mobile owner, Item item)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter("VorshunStorageLog.txt", true))
                {
                    string message = owner.Name + "\t" + item + "\t" + item.Name + "\t" + item.Hue;
                    writer.WriteLine(message);
                }
            }
            catch { }
        }

        internal static void RestoreOriginalItems(Mobile m)
        {
            PlayerMobile pm = m as PlayerMobile;
            if (pm != null)
                pm.TeamFlags = (ulong)AITeamList.TeamFlags.Player;
            new RestoreStuffTimer(m).Start();
        }
    }

    class DefaultItemsTimer : Timer
    {
        private Mobile m_Owner;

        public DefaultItemsTimer(Mobile owner)
            : base(TimeSpan.FromSeconds(0.25))
        {
            m_Owner = owner;
        }

        protected override void OnTick()
        {
            List<Item> toRemove = new List<Item>(); // take it off their body
            for (int i = 0; i < m_Owner.Items.Count; i++)
            {
                if (!(m_Owner.Items[i] is Backpack || m_Owner.Items[i] is BankBox))
                {
                    toRemove.Add(m_Owner.Items[i]);
                }
            }
            foreach (Item item in toRemove)
            {
                m_Owner.RemoveItem(item);
                m_Owner.Backpack.AddItem(item);
                VorshunStorage.LogStorage(m_Owner, item);
            }
            m_Owner.Backpack.InStorage = true;

            List<Item> toStore = new List<Item>();
            toStore.Add(m_Owner.Backpack);
            VorshunStorage.Storage.Add(m_Owner, toStore);

            m_Owner.RemoveItem(m_Owner.Backpack);

            VorshunArmory.GiveStarterBackpack(m_Owner);
            VorshunArmory.GiveDefaultClothing(m_Owner);
        }
    }

    class RestoreStuffTimer : Timer
    {
        private Mobile m_Owner;

        public RestoreStuffTimer(Mobile owner)
            : base(TimeSpan.FromSeconds(0.25))
        {
            m_Owner = owner;
        }

        protected override void OnTick()
        {
            if (!VorshunStorage.ContainsStuffBelongingTo(m_Owner))
            {
                return;
            }

            if (!m_Owner.Alive)
            {
                m_Owner.PlaySound(0x214);
                m_Owner.FixedEffect(0x376A, 10, 16);
                m_Owner.Resurrect();
            }

            List<Item> removeMe = new List<Item>(m_Owner.Items);
            foreach (Item item in removeMe)
            {
                if (item != m_Owner.BankBox)
                {
                    m_Owner.RemoveItem(item);
                }
            }

            List<Item> stuff = VorshunStorage.Retrieve(m_Owner);

            foreach (Item item in stuff)
            {
                m_Owner.AddItem(item);
                item.InStorage = false;
            }

            VorshunStorage.Clear(m_Owner);
        }
    }

    class VorshunStoragePersistance : Item
    {
        private static VorshunStoragePersistance m_Instance;
        public static VorshunStoragePersistance Instance { get { return m_Instance; } }
        public override string DefaultName { get { return "Vorshun Storage - Internal"; } }

        public VorshunStoragePersistance()
            : base(1)
        {
            Movable = false;

            if (m_Instance == null || m_Instance.Deleted)
                m_Instance = this;
            else
                base.Delete();
        }

        public VorshunStoragePersistance(Serial serial)
            : base(serial)
        {
            m_Instance = this;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version

            int playerCount = VorshunStorage.Storage.Count;
            writer.Write(playerCount);

            foreach (KeyValuePair<Mobile, List<Item>> entry in VorshunStorage.Storage)
            {
                writer.Write(entry.Key.Serial);
                writer.Write(entry.Value, true);
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

                        for (int i = 0; i < playerCount; i++)
                        {
                            Serial serial = (Serial)reader.ReadInt();
                            List<Item> contents = reader.ReadStrongItemList();

                            Mobile owner = World.FindMobile(serial);
                            if (owner == null) { continue; }

                            VorshunStorage.Add(owner, contents);
                        }

                        break;
                    }
            }
        }

        public override void Delete()
        {
        }
    }

}
