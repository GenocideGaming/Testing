using System;
using System.Collections;
using Server.Engines.CannedEvil;
using Server.Items;
using System.Collections.Generic;
using Server.Network;
using System.Linq;
using Server.Multis;

namespace Server.Mobiles
{
    [CorpseName("Lord Blackthorn corpse")]
    public class LordBlackthorn : BaseCreature
    {
        private DateTime m_NextAbilityTime;

        [Constructable]
        public LordBlackthorn()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Lord Blackthorn";
            Body = 0x190;
            BaseSoundID = 0x488;

            SetStr(550, 600);
            SetDex(68, 100);
            SetInt(488, 620);

            SetHits(5000);

            SetDamage(29, 35);

            SetSkill(SkillName.EvalInt, 80.1, 100.0);
            SetSkill(SkillName.Magery, 80.1, 100.0);
            SetSkill(SkillName.MagicResist, 100.3, 130.0);
            SetSkill(SkillName.Tactics, 97.6, 100.0);
            SetSkill(SkillName.Wrestling, 97.6, 100.0);
            SetSkill(SkillName.Necromancy, 80.1, 100.0);
            SetSkill(SkillName.SpiritSpeak, 80.1, 100.0);

            //Added to fix list. Commented out during project merger so it wouldn't effect the build. 7/9/22
            //SetWearable(new LordBlackthorneSuit());
            //SetWearable(new Crown());

            Fame = 2250;
            Karma = -2250;

            VirtualArmor = 50;
        }

        public LordBlackthorn(Serial serial)
            : base(serial)
        {

        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
        }

        public override bool AutoDispel { get { return !Controlled; } }
        public override bool BleedImmune { get { return true; } }
        public override bool ReacquireOnMovement { get { return !Controlled; } }
        public override double BonusPetDamageScalar { get { return (Core.SE) ? 3.0 : 1.0; } }
        public override OppositionGroup OppositionGroup { get { return OppositionGroup.FeyAndUndead; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } }
        public override bool AlwaysMurderer { get { return true; } }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (DateTime.UtcNow > m_NextAbilityTime && 20.0 > Utility.RandomDouble())
            {
                /*
                switch (Utility.Random(1))
                {
                    case 0: Lightning(); break;
                    case 1: BlastRadius(); break;
                }
                */
                m_NextAbilityTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
        }
        //Added to fix list. Commented out during project merger so it wouldn't effect the build. 7/9/22
        /*
        #region Lightning
        private void Lightning()
        {
            int count = 0;

            IPooledEnumerable eable = GetMobilesInRange(BlastRange);
            foreach (Mobile m in eable)
            {
                if (m is ExodusJuggernaut || m is ExodusZealot || m is ShadowFiend || m is ShadowDweller)
                    continue;

                if (m.IsPlayer() && GetDistanceToSqrt(m) <= BlastRange && CanBeHarmful(m))
                {
                    DoHarmful(m);

                    Effects.SendBoltEffect(m, false, 0);
                    Effects.PlaySound(m, m.Map, 0x51D);

                    double damage = m.Hits * 0.6;

                    if (damage < 100.0)
                        damage = 100.0;
                    else if (damage > 200.0)
                        damage = 200.0;

                    AOS.Damage(m, this, (int)damage, 0, 0, 0, 0, 100);

                    count++;

                    if (count >= 6)
                        break;
                }
            }

            eable.Free();
        }
        #endregion

        #region Blast Radius
        private static readonly int BlastRange = 16;

        private static readonly double[] BlastChance = new double[]
            {
                0.0, 0.0, 0.05, 0.95, 0.95, 0.95, 0.05, 0.95, 0.95,
                0.95, 0.05, 0.95, 0.95, 0.95, 0.05, 0.95, 0.95
            };

        private void BlastRadius()
        {
            // TODO: Based on OSI taken videos, not accurate, but an aproximation

            Point3D loc = Location;

            for (int x = -BlastRange; x <= BlastRange; x++)
            {
                for (int y = -BlastRange; y <= BlastRange; y++)
                {
                    Point3D p = new Point3D(loc.X + x, loc.Y + y, loc.Z);
                    int dist = (int)Math.Round(Utility.GetDistanceToSqrt(loc, p));

                    if (dist <= BlastRange && BlastChance[dist] > Utility.RandomDouble())
                    {
                        Timer.DelayCall(TimeSpan.FromSeconds(0.1 * dist), new TimerCallback(
                            delegate
                            {
                                int hue = Utility.RandomList(90, 95);

                                Effects.SendPacket(loc, Map, new HuedEffect(EffectType.FixedXYZ, Serial.Zero, Serial.Zero, 0x3709, p, p, 20, 30, true, false, hue, 4));
                            }
                        ));
                    }
                }
            }

            PlaySound(0x64C);

            IPooledEnumerable eable = GetMobilesInRange(BlastRange);
            foreach (Mobile m in eable)
            {
                if (this != m && GetDistanceToSqrt(m) <= BlastRange && CanBeHarmful(m))
                {
                    DoHarmful(m);
                }
            }

            eable.Free();
        }
        #endregion
        */
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
