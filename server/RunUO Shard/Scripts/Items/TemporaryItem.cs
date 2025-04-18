using System;
using System.Collections.Generic;
using Server.Scripts.Custom.Citizenship;

namespace Server.Items
{
    public class TemporaryItem : Item
    {
        private static DecayTimer DecayTimerInstance;

        public static void Initialize()
        {
            DecayTimerInstance = new DecayTimer();
            DecayTimerInstance.Start();
        }
        
        [CommandProperty(AccessLevel.GameMaster)]
        public bool Refresh { get { return false; } set { LastRefreshed = DateTime.Now; } }

        private DateTime m_LastRefreshed;
        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime LastRefreshed { get { return m_LastRefreshed; } set { m_LastRefreshed = value; } }

        private TimeSpan m_Duration;
        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Duration { get { return m_Duration; } set { m_Duration = value; AddOrUpdateItem(this); } }

        private string m_Town;
        [CommandProperty(AccessLevel.GameMaster)]
        public string Town { get { return m_Town; } set { m_Town = value; } }

        private static List<TemporaryItem> TemporaryItems = new List<TemporaryItem>();

        private void AddOrUpdateItem(TemporaryItem newItem)
        {
            int index = -1;
            int newIndex = -1;
            TemporaryItems.Remove(newItem);
            
            foreach (TemporaryItem item in TemporaryItems)
            {
                index++;
                // add in a "earliest decay first" order
                if (item.LastRefreshed + item.Duration > newItem.LastRefreshed + newItem.Duration)
                {
                    newIndex = index;
                    break;
                }
            }
            if (newIndex != -1)
            {
                TemporaryItems.Insert(newIndex, newItem);
                return;
            }
            // add to the end of the list if you get here
            TemporaryItems.Add(newItem);
        }

        [Constructable]
        public TemporaryItem()
            : base(157)
        {
            Weight = 1;
            m_LastRefreshed = DateTime.Now;
            m_Duration = TimeSpan.FromHours(24);
            AddOrUpdateItem(this);
        }

        public TemporaryItem(Serial serial)
            : base(serial)
        {

        }

        public override void OnDelete()
        {
            TemporaryItems.Remove(this);
            base.OnDelete();
        }

        public override bool OnEquip(Mobile from)
        {
            if (m_Town == null) return true;
            
            ICommonwealth town = Commonwealth.Find(from);
            if (town != null && town.Definition != null && town.Definition.TownName == m_Town)
            {
                return true;
            }
            else
            {
                from.SendMessage("That is only usable by citizens of " + m_Town + "!");
                return false;
            }            
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
            writer.Write((DateTime)m_LastRefreshed);
            writer.Write((TimeSpan)m_Duration);
            writer.Write((string)m_Town);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            m_LastRefreshed = reader.ReadDateTime();
            m_Duration = reader.ReadTimeSpan();
            m_Town = reader.ReadString();
            AddOrUpdateItem(this);
        }

        private class DecayTimer : Timer
        {
            public DecayTimer()
                : base(TimeSpan.FromSeconds(0.0), TimeSpan.FromSeconds(1))
            {
            }

            protected override void OnTick()
            {
                base.OnTick();
                while (TemporaryItems.Count > 0)
                {
                    TemporaryItem item = TemporaryItems[0];
                    if (item.LastRefreshed + item.Duration < DateTime.Now)
                    {
                        TemporaryItems.RemoveAt(0);
                        item.Delete();
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
    }


}