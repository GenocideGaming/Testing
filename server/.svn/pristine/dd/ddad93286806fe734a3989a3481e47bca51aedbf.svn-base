using System;
using Server;
using Server.Items;
using Server.Scripts.Custom.Items;

namespace Server.Mobiles
{
    [CorpseName("a druid's corpse")]
    public class DeerformDruid : BaseCreature
    {
        [Constructable]
        public DeerformDruid()
            : base(AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an old druid";   // elite ancient gargoyle
            Body = 0xEA;


            SetStr(600, 605);
            SetDex(90, 100);
            SetInt(1500, 1750);

            SetHits(1800, 1900);
            SetMana(1300, 1550);

            SetDamage(45, 65);

            VirtualArmor = 25;

            SetSkill(SkillName.Tactics, 150.0, 150.0);

            SetSkill(SkillName.EvalInt, 120.0, 135.0);
            SetSkill(SkillName.Meditation, 150.0, 150.0);
            SetSkill(SkillName.Magery, 130.0, 135.0);

            SetSkill(SkillName.MagicResist, 90.0, 95.0);

            SetSkill(SkillName.Wrestling, 120.0, 125.0);

            Fame = 500;
            Karma = -500;
        }

        public override int GetAttackSound()
        {
            return 0x82;
        }

        public override int GetHurtSound()
        {
            return 0x83;
        }

        public override int GetDeathSound()
        {
            return 0x84;
        } 

        public override void GenerateLoot()
        {

            base.PackAddGoldTier(11);
            base.PackAddGoldTier(5);
            base.PackAddArtifactChanceTier(9);
            base.PackAddMapChanceTier(10);

            base.PackAddScrollTier(8);
            base.PackAddReagentTier(6);
            base.PackAddReagentTier(6);
            base.PackAddGemTier(5);

            if (0.1 > Utility.RandomDouble())
                PackItem(new DeerMask ());
            
        }

        public override bool CanRummageCorpses { get { return true; } }

        public override int Meat { get { return 2; } }

        public DeerformDruid(Serial serial)
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