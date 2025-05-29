using System;
using Server;
using Server.Items;
using Server.Mobiles;
using System.Collections.Generic;

namespace Server.Mobiles
{
    public class BlackmarketBanker : Banker
    {
        private List<SBInfo> m_SBInfos = new List<SBInfo>();
        protected override List<SBInfo> SBInfos { get { return m_SBInfos; } }

        public override bool IsActiveVendor { get { return true; } } // Enable vendor functionality
        public override bool IsActiveBuyer { get { return true; } } // Allow players to sell items
        public override bool IsActiveSeller { get { return false; } } // Disable buying from banker
        public override NpcGuild NpcGuild { get { return NpcGuild.None; } }
        public override bool ClickTitle { get { return false; } }

        [Constructable]
        public BlackmarketBanker()
        {
            Name = "Ephram";
            Direction = Direction.East;
        }

        // Override to allow criminals and murderers to interact
        public override bool CheckVendorAccess(Mobile from)
        {
            // Allow all players, regardless of karma or criminal status
            return true;
        }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBlackmarketBanker()); // Add jewel sell list
        }

        public override void InitBody()
        {
            // Base body initialization without random name
            InitStats(100, 100, 25);
            SpeechHue = Utility.RandomDyedHue();
            Hue = Utility.RandomSkinHue();

            if (IsInvulnerable && !Core.AOS)
                NameHue = 0x35;

            Female = false;
            Body = 0x190;
            // Name is already set in constructor, so skip random name generation
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt());
            AddItem(new Doublet(1109));
            AddItem(new Skirt(1109));
            AddItem(new Boots(1109));
            AddItem(new HalfApron(1109));
            AddItem(new StrawHat(1109));
            AddItem(new GoldRing());
            AddItem(new GoldBracelet());
            Hue = 33770;
            HairItemID = 8265;
            HairHue = 1109;
            FacialHairItemID = 8254;
            FacialHairHue = 1109;

            PackGold(20, 50); // Minimal gold for a shady banker
        }

        public BlackmarketBanker(Serial serial) : base(serial)
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