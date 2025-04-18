using System;
using Server.Items;
using Server.Factions;
using Server.Misc;

namespace Server.Mobiles
{
    public class Warlord : BaseCreature
    {
        [Constructable]
        public Warlord() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a warlord";
            Body = 0xA3;

            SetStr(151, 175);
            SetDex(61, 85);
            SetInt(151, 175);

            SetHits(142, 149);
            SetDamage(25, 32);

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

            VirtualArmor = 50;

            Fame = 750;
            Karma = -750;

        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
        }

        public Warlord(Serial serial)
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
