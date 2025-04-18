using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Commands;
using Server.Scripts.Custom.VorshunWarsEquipment;
using Server.Mobiles;
using Server.Scripts.Custom.Citizenship;
using Server.Items;

namespace Server.Scripts.Custom.VorshunWarsEquipment
{
    public class Set
    {
        string name = "INVALID NAME";
        public string Name { get { return name; } }

        Dictionary<Layer, int> contents;
        public Dictionary<Layer, int> Equipment { get { return contents; } }

        public Set(string name, Dictionary<Layer, int> contents)
        {
            this.name = name;
            this.contents = contents;
        }

        public Set(Set toClone)
        {
            this.name = toClone.name;
            this.contents = new Dictionary<Layer, int>(toClone.contents);
        }
    }

    static class EquipmentSetRegistry
    {
        public static void Initialize() { new EquipmentSetPersistance(); }

        static Dictionary<Serial, Dictionary<int, Set>> m_Sets = new Dictionary<Serial, Dictionary<int, Set>>();

        public static Dictionary<Serial, Dictionary<int, Set>> Storage
        {
            get { return m_Sets; } 
        }

        public static void StoreSet(Mobile owner, string name, Dictionary<Layer, int> equipment, int slot)
        {
            StoreSet(owner, new Set(name, equipment), slot);
        }

        public static void StoreSet(Mobile owner, Set set, int slot)
        {
            if (!m_Sets.ContainsKey(owner.Serial))
                m_Sets.Add(owner.Serial, new Dictionary<int, Set>());

            Set toStore = new Set(set);

            if (m_Sets[owner.Serial].ContainsKey(slot))
                m_Sets[owner.Serial][slot] = toStore;
            else
                m_Sets[owner.Serial].Add(slot, toStore);
        }

        public static bool Contains(Mobile owner, int slot)
        {
            if (!m_Sets.ContainsKey(owner.Serial))
                return false;

            if (!m_Sets[owner.Serial].ContainsKey(slot))
                return false;

            return true;
        }

        public static string GetName(Mobile owner, int slot)
        {
            if (!m_Sets.ContainsKey(owner.Serial))
                return "No Equipment Stored";

            if (!m_Sets[owner.Serial].ContainsKey(slot))
                return "No Equipment Stored";

            return m_Sets[owner.Serial][slot].Name;
        }

        public static Set Retrieve(Mobile owner, int slot)
        {
            CheckValidity(owner, slot);

            return new Set(m_Sets[owner.Serial][slot]);     // Clone the set rather than returning by ref
        }

        private static void CheckValidity(Mobile owner, int slot)
        {
            if (!m_Sets.ContainsKey(owner.Serial))
                throw new ArgumentException("Tried to retrieve armor for an owner who isn't stored!");

            if (!m_Sets[owner.Serial].ContainsKey(slot))
                throw new ArgumentException("Tried to retrieve armor for a slot that does not exist!");
        }        
    }

    class EquipmentSetPersistance : Item
    {
        private static EquipmentSetPersistance m_Instance;
        public static EquipmentSetPersistance Instance { get { return m_Instance; } }
        public override string DefaultName { get { return "Equipment Sets    - Internal"; } }

        public EquipmentSetPersistance()
            : base(1)
        {
            Movable = false;

            if (m_Instance == null || m_Instance.Deleted)
                m_Instance = this;
            else
                base.Delete();
        }

        public EquipmentSetPersistance(Serial serial)
            : base(serial)
        {
            m_Instance = this;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
            
            int playerCount = EquipmentSetRegistry.Storage.Count;
            writer.Write(playerCount);

            foreach (KeyValuePair<Serial, Dictionary<int, Set>> playerEntry in EquipmentSetRegistry.Storage)
            {
                writer.Write(playerEntry.Key);

                int setCount = playerEntry.Value.Count;
                writer.Write(setCount);

                foreach (KeyValuePair<int, Set> equipmentSet in playerEntry.Value)
                {
                    writer.Write(equipmentSet.Key);             // slot
                    writer.Write(equipmentSet.Value.Name);      // name
    
                    int itemCount = equipmentSet.Value.Equipment.Count;
                    writer.Write(itemCount);

                    foreach (KeyValuePair<Layer, int> equipmentItem in equipmentSet.Value.Equipment)
                    {
                        writer.Write((int)equipmentItem.Key);   // layer
                        writer.Write(equipmentItem.Value);      // itemid
                    }
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

                        for (int i = 0; i < playerCount; i++)
                        {
                            Serial playerSerial = (Serial)reader.ReadInt();
                            Mobile player = World.FindMobile(playerSerial);

                            int setCount = reader.ReadInt();
                            for (int j = 0; j < setCount; j++)
                            {
                                int slot = reader.ReadInt();
                                string setName = reader.ReadString();

                                Dictionary<Layer, int> equipmentForSet = new Dictionary<Server.Layer, int>();

                                int itemcount = reader.ReadInt();

                                for (int k = 0; k < itemcount; k++)
                                {
                                    Layer itemLayer = (Layer)reader.ReadInt();
                                    int itemID = reader.ReadInt();

                                    equipmentForSet.Add(itemLayer, itemID);
                                }

                                Set thisSet = new Set(setName, equipmentForSet);

                                if (player != null)
                                    EquipmentSetRegistry.StoreSet(player, thisSet, slot);
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
}