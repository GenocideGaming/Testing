using System;
using System.Collections;
using Server.Misc;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a blade spirit corpse")]
    public class BladeSpirits : BaseCreature
    {
        public override bool DeleteCorpseOnDeath { get { return Core.AOS; } }
        public override bool IsHouseSummonable { get { return true; } }
        public override bool AlwaysMurderer { get { return true; } }

        public override double DispelDifficulty { get { return 0.0; } }
        public override double DispelFocus { get { return 20.0; } }

        public override double GetFightModeRanking(Mobile m, FightMode acqType, bool bPlayerOnly)
        {
            return (m.Str + m.Skills[SkillName.Tactics].Value) / Math.Max(GetDistanceToSqrt(m), 1.0);
        }

        [Constructable]
        public BladeSpirits()
            : base(AIType.AI_Generic, FightMode.Closest, 10, 1, 0.3, 0.6)
        {
            Name = "a blade spirit";
            Body = 574;

            SetStr(150);
            SetDex(150);
            SetInt(100);

            SetHits(175, 200);
            SetStam(250);
            SetMana(0);

            SetDamage(6, 12);

            VirtualArmor = 5;

            SetSkill(SkillName.Tactics, 100.0, 100.0);

            SetSkill(SkillName.MagicResist, 50.0, 50.0);
            SetSkill(SkillName.Wrestling, 80.0, 85.0);

            Fame = 0;
            Karma = 0;

            ControlSlots = 2;
            Kills = 5;
        }

        public override bool ReturnsHome { get { return false; } }
        public override bool BleedImmune { get { return true; } }

        public override int GetAngerSound()
        {
            return 0x23A;
        }

        public override int GetAttackSound()
        {
            return 0x3B8;
        }

        public override int GetHurtSound()
        {
            return 0x23A;
        }

        public override void OnThink()
        {
            base.OnThink();
        }

        public BladeSpirits(Serial serial)
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
    }
}