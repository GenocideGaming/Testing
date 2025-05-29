using System;
using System.Collections;
using System.Collections.Generic;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class LioraTheAnimalTamer : BaseVendor
    {
        private readonly List<SBInfo> m_SBInfos = new List<SBInfo>();
        protected override List<SBInfo> SBInfos { get { return m_SBInfos; } }

        [Constructable]
        public LioraTheAnimalTamer() : base("the Animal Tamer")
        {
            Name = "Liora";
            SetSkill(SkillName.AnimalTaming, 65.0, 88.0);
            SetSkill(SkillName.AnimalLore, 65.0, 88.0);
        }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBAnimalTrainer());
        }

        public override void InitOutfit()
        {
            AddItem(new FancyDress());
            AddItem(new Sandals());
            AddItem(new ShepherdsCrook());
            AddItem(new Backpack());
        }

        public LioraTheAnimalTamer(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
        }

        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            if (dropped is CommodityDeed deed && from is PlayerMobile pm && pm.Quest is OutpostTraderunQuest quest)
            {
                if (quest.CheckComplete(this, deed))
                    return true;
            }
            return base.OnDragDrop(from, dropped);
        }
    }
}