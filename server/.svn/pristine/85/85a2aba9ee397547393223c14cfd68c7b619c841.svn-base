using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("an earth elemental corpse")]
    public class SummonedEarthElemental : BaseCreature
    {
        public override double DispelDifficulty { get { return 117.5; } }
        public override double DispelFocus { get { return 45.0; } }

        [Constructable]
        public SummonedEarthElemental()
            : base(AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an earth elemental";
            Body = 14;
            BaseSoundID = 268;

            SetStr(126, 155);
            SetDex(66, 85);
            SetInt(71, 92);

            SetHits(225, 250);

            SetDamage(12, 16);

            VirtualArmor = 30;

            SetSkill(SkillName.Tactics, 100.0, 100.0);

            SetSkill(SkillName.MagicResist, 50.0, 50.0);
            SetSkill(SkillName.Wrestling, 75.0, 80.0);

            Fame = 3500;
            Karma = -3500;

            ControlSlots = 2;
        }

        public SummonedEarthElemental(Serial serial)
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