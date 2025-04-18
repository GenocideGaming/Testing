using System;
using System.Collections;
using Server.Items;
using Server.ContextMenus;
using Server.Misc;
using Server.Network;

namespace Server.Mobiles
{

    [CorpseName("a crazed peasant's corpse")]
    public class CrazedPeasant : BaseCreature
    {
        public override bool ShowFameTitle
        {
            get
            {
                return false;
            }
        }

        [Constructable]
        public CrazedPeasant()
            : base(AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            SpeechHue = Utility.RandomDyedHue();
            Name = "a crazed peasant";
            Hue = 2155;

            if (this.Female = Utility.RandomBool())
            {
                this.Body = 0x191;
                int lowHue = GetRandomHue();
                this.AddItem(new PlainDress(lowHue));
                this.AddItem(new Boots(lowHue));
            }
            else
            {
                this.Body = 0x190;
                this.AddItem(new Shirt(GetRandomHue()));
                int lowHue = GetRandomHue();
                this.AddItem(new Shoes(lowHue));
                this.AddItem(new ShortPants(lowHue));
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
            SetSkill(SkillName.Swords, 56.0, 68.0);
            SetSkill(SkillName.Macing, 56.0, 68.0);


            Fame = 1500;
            Karma = -1500;


            AddItem(new Torch(true));

            switch (Utility.Random(3))
            {
                case 0: AddItem(new Dagger()); break;
                case 1: AddItem(new ButcherKnife()); break;
                case 2: AddItem(new Club()); break;
            }

            switch (Utility.Random(4))
            {
                case 0: PackItem(new Apple(1)); break;
                case 1: PackItem(new Apple(2)); break;
                case 2: PackItem(new Apple(1)); PackItem(new Cabbage(1)); break;
                case 3: PackItem(new ChickenLeg(1)); break;
            }


            Utility.AssignRandomHair(this, 2155);
        }

        public override void GenerateLoot()
        {
            base.PackAddGoldTier(2);
            base.PackAddArtifactChanceTier(2);
            base.PackAddMapChanceTier(2);
            base.PackAddRandomPotionTier(2);
        }

        public override bool AlwaysAttackable { get { return true; } }

        public CrazedPeasant(Serial serial)
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