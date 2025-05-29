using System;
using System.Collections;
using System.Collections.Generic;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class OutpostPackhorse : PackHorse
    {
        private Mobile m_Owner;
        private OutpostTraderunQuest m_Quest;

        [Constructable]
        public OutpostPackhorse(Mobile owner, OutpostTraderunQuest quest) : base()
        {
            Name = "Outpost Supply Packhorse";
            m_Owner = owner;
            m_Quest = quest;
            ControlSlots = 0;
            Tamable = false;
            SetStr(100);
            SetDex(50);
            SetInt(25);
            SetHits(200);
            SetDamage(1, 4);
            AI = AIType.AI_Animal;
            FightMode = FightMode.Aggressor;
        }

        public OutpostPackhorse(Serial serial) : base(serial)
        {
        }

        public override void GenerateLoot()
        {
            // No standard loot
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
            PackGold(50, 100); // Minor loot
            if (m_Quest != null)
                m_Quest.OnPackhorseDeath(this);
        }

        public override bool CheckItemUse(Mobile from, Item item)
        {
            if (item is CommodityDeed && from == m_Owner)
            {
                from.SendMessage("The commodity deed is locked to the packhorse until the quest is completed.");
                return false;
            }
            return base.CheckItemUse(from, item);
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from == m_Owner && InRange(from, 3))
                Backpack.DisplayTo(from);
        }

        public override bool CanBeControlledBy(Mobile m)
        {
            return m == m_Owner;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
            writer.Write(m_Owner);
            QuestSerializer.Serialize(m_Quest, writer);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
            m_Owner = reader.ReadMobile();
            m_Quest = QuestSerializer.DeserializeQuest(reader) as OutpostTraderunQuest;
        }
    }
}