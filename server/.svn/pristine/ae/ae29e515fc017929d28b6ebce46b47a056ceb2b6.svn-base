using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a druid's corpse")]
    public class BearformDruid : BaseCreature
    {
        [Constructable]
        public BearformDruid()
            : base(AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an old druid";
            Body = 212;
            BaseSoundID = 0xA3;

            SetStr(700, 750);
            SetDex(140, 145);
            SetInt(1800, 2000);

            SetHits(1700, 1900);
            SetMana(1000, 1200);

            SetDamage(25, 40);

            VirtualArmor = 20;

            SetSkill(SkillName.Tactics, 100.0, 100.0);

            SetSkill(SkillName.EvalInt, 90.0, 95.0);
            SetSkill(SkillName.Meditation, 100.0, 100.0);
            SetSkill(SkillName.Magery, 90.0, 95.0);

            SetSkill(SkillName.MagicResist, 90.0, 95.0);

            SetSkill(SkillName.Wrestling, 90.0, 95.0);

            Fame = 24000;
            Karma = -24000;
        }

        public override void GenerateLoot()
        {
            base.PackAddGoldTier(11);
            base.PackAddArtifactChanceTier(11);
            base.PackAddMapChanceTier(11);


            base.PackAddArtifactChanceTier(8);

            if (0.1 > Utility.RandomDouble())
                PackItem(new BearMask ());
        }

        public override bool CanRummageCorpses { get { return true; } }

        public override int Meat { get { return 1; } }

        public BearformDruid(Serial serial)
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