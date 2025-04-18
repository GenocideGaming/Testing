using System;
using Server;
using Server.Items;
using System.Collections;

namespace Server.Mobiles
{
    [CorpseName("an energy vortex corpse")]
    public class EnergyVortex : BaseCreature
    {
        public override bool DeleteCorpseOnDeath { get { return Summoned; } }
        public override bool AlwaysMurderer { get { return true; } } // Or Llama vortices will appear gray.

        public override double DispelDifficulty { get { return 80.0; } }
        public override double DispelFocus { get { return 20.0; } }

        public override double GetFightModeRanking(Mobile m, FightMode acqType, bool bPlayerOnly)
        {
            return (m.Int + m.Skills[SkillName.Magery].Value) / Math.Max(GetDistanceToSqrt(m), 1.0);
        }

        [Constructable]
        public EnergyVortex()
            : base(AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an energy vortex";

            if (Core.SE && 0.002 > Utility.RandomDouble()) // Per OSI FoF, it's a 1/500 chance.
            {
                // Llama vortex!
                Body = 0xDC;
                Hue = 0x76;
            }
            else
            {
                Body = 164;
            }

            SetStr(200);
            SetDex(200);
            SetInt(100);

            SetHits(200, 225);
            SetStam(250);
            SetMana(0);

            SetDamage(14, 20);

            VirtualArmor = 5;

            SetSkill(SkillName.Tactics, 100.0, 100.0);

            SetSkill(SkillName.MagicResist, 50.0, 55.0);
            SetSkill(SkillName.Wrestling, 85.0, 90.0);

            Fame = 0;
            Karma = 0;

            ControlSlots = 2;

            Kills = 5;
        }

        public override bool ReturnsHome { get { return false; } }
        public override bool BleedImmune { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } }

        public override int GetAngerSound()
        {
            return 0x15;
        }

        public override int GetAttackSound()
        {
            return 0x28;
        }

        public override void OnThink()
        {
            base.OnThink();
        }


        public EnergyVortex(Serial serial)
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

            if (BaseSoundID == 263)
                BaseSoundID = 0;
        }
    }
}