using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
    public class BlackmarketVendor : BaseVendor
    {
        private List<SBInfo> m_SBInfos = new List<SBInfo>();
        protected override List<SBInfo> SBInfos { get { return m_SBInfos; } }

        [Constructable]
        public BlackmarketVendor() : base("")
        {
            Name = "Shamus the fence";
            SetSkill(SkillName.Stealing, 60.0, 83.0);
            SetSkill(SkillName.Hiding, 45.0, 68.0);
            Direction = Direction.East; 
        }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBlackmarket());
        }

        // Override to allow criminals and murderers to interact
        public override bool CheckVendorAccess(Mobile from)
        {
            // Allow all players, regardless of karma or criminal status
            return true;
        }

        public override void InitOutfit()
        {
            AddItem(new Tunic(1109));
            AddItem(new Doublet(1109));
            AddItem(new Skirt(1109));
            AddItem(new Boots(1109));
            AddItem(new HalfApron(1109));
            AddItem(new Hood(1109));
            AddItem(new GoldRing());
            AddItem(new GoldBracelet());
            HairHue = 1109;
            Female = false;
            PackGold(20, 50); // Minimal gold for a shady banker
        }

        // Override to control in-game name display (exclude title)
        public BlackmarketVendor(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}