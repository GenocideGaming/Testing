using System;
using System.Collections;
using Server.Items;
using Server.ContextMenus;
using Server.Misc;
using Server.Network;

namespace Server.Mobiles
{

    [CorpseName("a crazed farmer's corpse")]
    public class CrazedFarmer : BaseCreature
    {
        public override bool ShowFameTitle
        {
            get
            {
                return false;
            }
        }

        [Constructable]
        public CrazedFarmer()
            : base(AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            SpeechHue = Utility.RandomDyedHue();
            Name = "a crazed farmer";
            Hue = 2155;

            if (this.Female = Utility.RandomBool())
            {
                this.Body = 0x191;
            }
            else
            {
                this.Body = 0x190;
            }

            SetStr(76, 85);
            SetDex(66, 75);
            SetInt(25, 35);

            SetHits(55, 65);
            SetStam(40, 55);

            SetDamage(3, 7); //Uses Weapon

            VirtualArmor = 5;

            SetSkill(SkillName.Tactics, 56.0, 68.0); //Uses Weapon
            SetSkill(SkillName.MagicResist, 30.0, 35.0);
            SetSkill(SkillName.Parry, 35.0, 40.0);
            SetSkill(SkillName.Fencing, 56.0, 68.0);

            Fame = 1500;
            Karma = -1500;
            int lowHue = GetRandomHue();

            Item boots = new Boots(lowHue);
            boots.Movable = false;
            AddItem(boots);

            Item shirt = new FancyShirt(lowHue);
            shirt.Movable = false;
            AddItem(shirt);

            Item pants = new LongPants(lowHue);
            pants.Movable = false;
            AddItem(pants);

            Item hat = new TallStrawHat();
            hat.Movable = false;
            AddItem(hat);

            AddItem(new Pitchfork());

            Utility.AssignRandomHair(this, 2155);

            switch (Utility.Random(2))
            {
                case 0: PackItem(new Bandage(1)); break;
                case 1: PackItem(new Bandage(2)); break;
            }

            switch (Utility.Random(6))
            {
                case 0: PackItem(new Carrot(1)); break;
                case 1: PackItem(new Carrot(2)); break;
                case 2: PackItem(new Carrot(2)); PackItem(new Cabbage(1)); break;
                case 3: PackItem(new Cabbage(1)); break;
                case 4: PackItem(new Cabbage(2)); break;
                case 5: PackItem(new Cabbage(2)); PackItem(new Carrot(1)); break;
            }

        }

        public override void GenerateLoot()
        {
            base.PackAddGoldTier(2);
            base.PackAddArtifactChanceTier(2);
            base.PackAddMapChanceTier(2);
            base.PackAddRandomPotionTier(2);
        }

        public override bool AlwaysAttackable { get { return true; } }

        public CrazedFarmer(Serial serial)
            : base(serial)
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
        private static int GetRandomHue()
        {
            switch (Utility.Random(6))
            {
                default:
                case 0:
                    return 0;
                case 1:
                    return Utility.RandomBlueHue();
                case 2:
                    return Utility.RandomGreenHue();
                case 3:
                    return Utility.RandomRedHue();
                case 4:
                    return Utility.RandomYellowHue();
                case 5:
                    return Utility.RandomNeutralHue();
            }
        }

    }
}