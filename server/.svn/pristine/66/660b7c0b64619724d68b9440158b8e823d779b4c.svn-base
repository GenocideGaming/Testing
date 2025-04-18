using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("an air elemental corpse")]
    public class SummonedAirElemental : BaseCreature
    {
        public override double DispelDifficulty { get { return 117.5; } }
        public override double DispelFocus { get { return 45.0; } }

        [Constructable]
        public SummonedAirElemental()
            : base(AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an air elemental";
            Body = 13;
            Hue = 0x4001;
            BaseSoundID = 655;

            SetStr(126, 155);
            SetDex(166, 185);
            SetInt(101, 125);

            SetHits(175, 200);

            SetDamage(5, 10);

            VirtualArmor = 5;

            SetSkill(SkillName.Tactics, 100.0, 100.0);

            SetSkill(SkillName.EvalInt, 60.0, 65.0);
            SetSkill(SkillName.Magery, 60.0, 65.0);
            SetSkill(SkillName.MagicResist, 75.0, 75.0);
            SetSkill(SkillName.Wrestling, 90.0, 95.0);

            Fame = 4500;
            Karma = -4500;

            ControlSlots = 2;
        }

        public override Poison PoisonImmune { get { return Poison.Greater; } }

        public SummonedAirElemental(Serial serial)
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

            if (BaseSoundID == 263)
                BaseSoundID = 655;
        }
    }
}