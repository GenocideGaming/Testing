using System;
using Server.Items;

namespace Server.Mobiles
{
    public class UndeadCaptain : BaseCreature
    {
        [Constructable]
        public UndeadCaptain()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.SpeechHue = Utility.RandomDyedHue();
            this.Title = " the Undead Captain";
            this.Hue = 768;

            this.Body = 0x190;
            this.Name = NameList.RandomName("male");

            SetStr(200, 205);
            SetDex(45, 50);
            SetInt(80, 100);

            SetHits(175, 200);
            SetMana(800, 100);

            SetDamage(8, 14);

            VirtualArmor = 25;

            this.SetSkill(SkillName.MagicResist, 70.1, 85.0);
            this.SetSkill(SkillName.Swords, 60.1, 85.0);
            this.SetSkill(SkillName.Tactics, 75.1, 90.0);
            this.SetSkill(SkillName.Wrestling, 60.1, 85.0);

            this.Fame = 5000;
            this.Karma = -5000;

            this.AddItem(new FancyShirt());
            this.AddItem(new LongPants());
            this.AddItem(new ThighBoots());
            this.AddItem(new TricorneHat());
            this.AddItem(new BodySash());
            this.AddItem(new Cutlass());

            LeatherGloves gloves = new LeatherGloves();
            this.AddItem(gloves);

            this.FacialHairItemID = 0x203E; // Long Beard
            this.FacialHairHue = 0x455;

            Utility.AssignRandomHair(this);
            this.HairHue = 0x455;
        }
        public override void GenerateLoot()
        {
            base.PackAddGoldTier(5);
            base.PackAddArtifactChanceTier(5);
            base.PackAddMapChanceTier(5);

        }

        public UndeadCaptain(Serial serial)
            : base(serial)
        {
        }

        public override bool ClickTitle
        {
            get
            {
                return true;
            }
        }

        public override bool AlwaysMurderer
        {
            get
            {
                return true;
            }
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
