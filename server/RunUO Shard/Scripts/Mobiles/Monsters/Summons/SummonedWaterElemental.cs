using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a water elemental corpse")]
    public class SummonedWaterElemental : BaseCreature
    {
        public override double DispelDifficulty { get { return 117.5; } }
        public override double DispelFocus { get { return 45.0; } }

        [Constructable]
        public SummonedWaterElemental()
            : base(AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a water elemental";
            Body = 16;
            BaseSoundID = 278;

            SetStr(126, 155);
            SetDex(66, 85);
            SetInt(101, 125);

            SetHits(150, 175);

            SetDamage(4, 8);

            VirtualArmor = 5;

            SetSkill(SkillName.Tactics, 100.0, 100.0);

            SetSkill(SkillName.EvalInt, 65.0, 70.0);

            SetSkill(SkillName.Magery, 65.0, 70.0);

            SetSkill(SkillName.MagicResist, 195.0, 200.0);

            SetSkill(SkillName.Wrestling, 65.0, 70.0);

            Fame = 4500;
            Karma = -4500;

            ControlSlots = 2;

            CanSwim = true;
        }

        public SummonedWaterElemental(Serial serial)
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