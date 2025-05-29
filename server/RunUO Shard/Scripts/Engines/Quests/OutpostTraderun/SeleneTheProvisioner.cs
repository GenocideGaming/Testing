using System;
using System.Collections;
using System.Collections.Generic;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class SeleneTheProvisioner : BaseVendor
    {
        private readonly List<SBInfo> m_SBInfos = new List<SBInfo>();
        protected override List<SBInfo> SBInfos { get { return m_SBInfos; } }

        [Constructable]
        public SeleneTheProvisioner() : base("the Provisioner")
        {
            Name = "Selene";
            SetSkill(SkillName.Camping, 45.0, 68.0);
            SetSkill(SkillName.Tactics, 45.0, 68.0);
        }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBProvisioner());
        }

        public override void InitOutfit()
        {
            AddItem(new HalfApron());
            AddItem(new FancyShirt());
            AddItem(new Skirt());
            AddItem(new Boots());
            AddItem(new Backpack());
        }

        public SeleneTheProvisioner(Serial serial) : base(serial)
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

        public override void OnDoubleClick(Mobile from)
        {
            if (from.InRange(this, 3) && from is PlayerMobile pm)
            {
                bool inRestartPeriod;
                if (!QuestSystem.CanOfferQuest(pm, typeof(OutpostTraderunQuest), out inRestartPeriod))
                    SayToMarketplace(from);
                else
                {
                    OutpostTraderunQuest quest = new OutpostTraderunQuest(pm, this);
                    quest.SendOffer();
                }
            }
        }

        private void SayToMarketplace(Mobile m)
        {
            SayTo(m, "Come back later, our supplies are still being organized!");
        }
    }
}