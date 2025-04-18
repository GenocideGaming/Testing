using System;
using Server.Items;
using Server.Factions;
using Server.Misc;

namespace Server.Mobiles
{
    public class DreadKnight : BaseCreature
    {
        [Constructable]
        public DreadKnight() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Dread Knight";
            Body = 197;

            SetStr(116, 125);
            SetDex(61, 85);
            SetInt(81, 95);

            SetHits(222, 225);
            SetDamage(15, 20);

            SetSkill(SkillName.Healing, 80.0, 90.0);
            SetSkill(SkillName.Anatomy, 80.0, 90.0);
            SetSkill(SkillName.MagicResist, 80.0, 90.0);
            SetSkill(SkillName.Swords, 80.0, 90.0);
            SetSkill(SkillName.Tactics, 80.0, 90.0);
            SetSkill(SkillName.Wrestling, 80.0, 90.0);

            VirtualArmor = 25;

            Fame = 250;
            Karma = -250;

        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich, 2);
        }

        public DreadKnight(Serial serial)
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
