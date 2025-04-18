using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a flesh golem corpse")]
    public class FleshGolem : BaseCreature
    {
        [Constructable]
        public FleshGolem()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a flesh golem";
            Body = 155;
            BaseSoundID = 684;

            SetStr(176, 200);
            SetDex(51, 75);
            SetInt(46, 70);

            SetHits(106, 120);

            SetDamage(18, 22);

            SetSkill(SkillName.MagicResist, 50.1, 75.0);
            SetSkill(SkillName.Tactics, 55.1, 80.0);
            SetSkill(SkillName.Wrestling, 60.1, 70.0);

            Fame = 4000;
            Karma = -4000;

            VirtualArmor = 30;

            switch (Utility.Random(3))
            {
                case 0: PackItem(new Bandage(1)); break;
                case 1: PackItem(new Bandage(2)); break;
                case 2: PackItem(new Bandage(3)); break;

            }

        }
        public override void GenerateLoot()
        {
            base.PackAddGoldTier(4);
            base.PackAddArtifactChanceTier(4);
            base.PackAddMapChanceTier(4);
            AddLoot(LootPack.LowPotions);
        }

        public FleshGolem(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
