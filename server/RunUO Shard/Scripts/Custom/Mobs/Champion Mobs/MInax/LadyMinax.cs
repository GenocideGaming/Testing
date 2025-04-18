using System;
using Server.Engines.CannedEvil;
using Server.Items;
using Server.Spells.Fifth;
using Server.Spells.Seventh;

namespace Server.Mobiles
{
    public class LadyMinaxChampion : BaseCreature
    {
        [Constructable]
        public LadyMinaxChampion()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Minax";
            Body = 0x191;
            Hue = 0x83EC;

            SetStr(550, 600);
            SetDex(150);
            SetInt(750);

            SetHits(10000);
            SetStam(150);
            SetMana(750);

            SetDamage(29, 38);


            SetSkill(SkillName.MagicResist, 100.0);
            SetSkill(SkillName.Magery, 118.3, 120.2);
            SetSkill(SkillName.Meditation, 118.3, 120.2);
            SetSkill(SkillName.EvalInt, 118.3, 120.2);
            SetSkill(SkillName.Necromancy, 118.3, 120.2);
            SetSkill(SkillName.Wrestling, 118.4, 122.7);

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 70;
            //Added to fix list. Commented out during project merger so it wouldn't effect the build. 7/9/22
            /*
            SetWearable(new MinaxsArmor());
            SetWearable(new LeatherGloves(), 1645);
            SetWearable(new WizardsHat(1645));
            SetWearable(new Sandals(1645));
            SetWearable(new Cloak(1645));
            */


            HairItemID = 0x203C; // Long Hair
            HairHue = 1109;

        }

        public LadyMinaxChampion(Serial serial)
            : base(serial)
        {
        }

        public override bool AlwaysMurderer
        {
            get
            {
                return true;
            }
        }
        public override bool AutoDispel
        {
            get
            {
                return true;
            }
        }
        public override double AutoDispelChance
        {
            get
            {
                return 1.0;
            }
        }
        public override bool BardImmune
        {
            get
            {
                return true;
            }
        }
        public override bool Unprovokable
        {
            get
            {
                return true;
            }
        }
        public override bool Uncalmable
        {
            get
            {
                return true;
            }
        }
        public override Poison PoisonImmune
        {
            get
            {
                return Poison.Deadly;
            }
        }
        public override bool ShowFameTitle
        {
            get
            {
                return false;
            }
        }
        public override bool ClickTitle
        {
            get
            {
                return false;
            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 3);
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            c.DropItem(new PowerScroll(SkillName.Tinkering, 105));

            new BucsDenGate(true, Corpse.Location, Corpse.Map);
        }

        public void Polymorph(Mobile m)
        {
            if (!m.CanBeginAction(typeof(PolymorphSpell)) || !m.CanBeginAction(typeof(IncognitoSpell)) || m.IsBodyMod)
                return;

            IMount mount = m.Mount;

            if (mount != null)
                mount.Rider = null;

            if (m.Mounted)
                return;

            if (m.BeginAction(typeof(PolymorphSpell)))
            {
                Item disarm = m.FindItemOnLayer(Layer.OneHanded);

                if (disarm != null && disarm.Movable)
                    m.AddToBackpack(disarm);

                disarm = m.FindItemOnLayer(Layer.TwoHanded);

                if (disarm != null && disarm.Movable)
                    m.AddToBackpack(disarm);

                m.BodyMod = 129;
                m.HueMod = 0;

                new ExpirePolymorphTimer(m).Start();
            }
        }

        private class ExpirePolymorphTimer : Timer
        {
            private Mobile m_Owner;

            public ExpirePolymorphTimer(Mobile owner) : base(TimeSpan.FromMinutes(3.0))
            {
                m_Owner = owner;

                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (!m_Owner.CanBeginAction(typeof(PolymorphSpell)))
                {
                    m_Owner.BodyMod = 0;
                    m_Owner.HueMod = -1;
                    m_Owner.EndAction(typeof(PolymorphSpell));
                }
            }
        }

        public void SpawnRatmen(Mobile target)
        {
            Map map = this.Map;

            if (map == null)
                return;

            int rats = 0;

            foreach (Mobile m in this.GetMobilesInRange(10))
            {
                if (m is Ratman || m is RatmanArcher || m is RatmanMage)
                    ++rats;
            }

            if (rats < 16)
            {
                PlaySound(0x3D);

                int newRats = Utility.RandomMinMax(3, 6);

                for (int i = 0; i < newRats; ++i)
                {
                    BaseCreature rat;

                    switch (Utility.Random(5))
                    {
                        default:
                        case 0:
                            rat = new Defiler();
                            break;
                        case 1:
                            rat = new Executioner();
                            break;
                    }

                    rat.Team = this.Team;

                    bool validLocation = false;
                    Point3D loc = this.Location;

                    for (int j = 0; !validLocation && j < 10; ++j)
                    {
                        int x = X + Utility.Random(3) - 1;
                        int y = Y + Utility.Random(3) - 1;
                        int z = map.GetAverageZ(x, y);

                        if (validLocation = map.CanFit(x, y, this.Z, 16, false, false))
                            loc = new Point3D(x, y, Z);
                        else if (validLocation = map.CanFit(x, y, z, 16, false, false))
                            loc = new Point3D(x, y, z);
                    }

                    rat.MoveToWorld(loc, map);
                    rat.Combatant = target;
                }
            }
        }

        public void DoSpecialAbility(Mobile target)
        {
            if (target == null || target.Deleted) //sanity
                return;

            if (0.2 >= Utility.RandomDouble()) // 20% chance to more ratmen
                SpawnRatmen(target);

            if (Hits < 2000 && !IsBodyMod) // Baracoon is low on life, polymorph into a ratman
                Polymorph(this);
        }

        public override void OnGotMeleeAttack(Mobile attacker)
        {
            base.OnGotMeleeAttack(attacker);

            DoSpecialAbility(attacker);
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            DoSpecialAbility(defender);
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
