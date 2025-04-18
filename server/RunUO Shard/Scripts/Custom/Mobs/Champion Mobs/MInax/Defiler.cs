using System;
using Server.Items;
using Server.Factions;
using Server.Misc;

namespace Server.Mobiles
{
    public class Defiler : BaseCreature
    {
        [Constructable]
        public Defiler() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a defiler";
            Body = 198;

            SetStr(91, 115);
            SetDex(61, 85);
            SetInt(81, 95);

            SetHits(112, 119);
            SetDamage(10, 14);

            SetSkill(SkillName.Healing, 80.0, 90.0);
            SetSkill(SkillName.Anatomy, 80.0, 90.0);
            SetSkill(SkillName.MagicResist, 80.0, 90.0);
            SetSkill(SkillName.Swords, 80.0, 90.0);
            SetSkill(SkillName.Tactics, 80.0, 90.0);
            SetSkill(SkillName.Wrestling, 80.0, 90.0);

            SetResistance(ResistanceType.Physical, 10, 30);
            SetResistance(ResistanceType.Fire, 10, 30);
            SetResistance(ResistanceType.Cold, 10, 30);
            SetResistance(ResistanceType.Energy, 10, 30);
            SetResistance(ResistanceType.Poison, 10, 30);

            VirtualArmor = 20;

            Fame = 150;
            Karma = -150;

        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
        }

        public Defiler(Serial serial)
            : base(serial)
        {
        }

        public override bool ClickTitle{get{return true;}}

        public override bool AlwaysAttackable{get{return true;}}

        public override bool ShowFameTitle{get{return false;}}

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
