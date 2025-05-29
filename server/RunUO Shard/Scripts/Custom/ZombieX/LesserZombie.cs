using System;
using System.Collections.Generic;
using Server.Items;
using Server.Factions;
using Server.Scripts.Engines.Factions.Instances.Factions;

namespace Server.Mobiles
{
    [CorpseName("a lesser infected corpse")]
    public class LesserZombie : BaseUndead
    {
        public override bool CanRegenHits { get { return true; } }

        public override Faction FactionAllegiance { get { return Forsaken.Instance; } }
        public override Ethics.Ethic EthicAllegiance { get { return Ethics.Ethic.Evil; } }

        public override bool IsScaredOfScaryThings { get { return false; } }
        public override bool IsScaryToPets { get { return true; } }
        private RotTimer m_Timer;

        [Constructable]
        public LesserZombie() : base(AIType.AI_Melee, FightMode.Closest, 8, 1, 0.2, 0.4)
        {
            Name = "A Lesser Contagious Zombie";
            Body = 155;
            BaseSoundID = 471;

            SetStr(150); // 50% of Zombiex's 300
            SetDex(12);  // ~50% of Zombiex's 25
            SetInt(5);   // 50% of Zombiex's 10

            SetHits(250); // 50% of Zombiex's 500
            SetStam(75);  // 50% of Zombiex's 150
            SetMana(0);

            SetDamage(5, 7); // 50% of Zombiex's 10-15

            SetDamageType(ResistanceType.Physical, 25);
            SetDamageType(ResistanceType.Cold, 25);
            SetDamageType(ResistanceType.Poison, 50);

            SetResistance(ResistanceType.Physical, 25, 40); // ~50% of Zombiex's 50-80
            SetResistance(ResistanceType.Fire, 10, 15);    // ~50% of Zombiex's 20-30
            SetResistance(ResistanceType.Cold, 50, 50);    // Reduced from 99-100
            SetResistance(ResistanceType.Poison, 50, 50);  // Reduced from 99-100
            SetResistance(ResistanceType.Energy, 20, 30);  // ~50% of Zombiex's 40-60

            SetSkill(SkillName.Poisoning, 60.0);    // 50% of Zombiex's 120.0
            SetSkill(SkillName.MagicResist, 50.0);  // 50% of Zombiex's 100.0
            SetSkill(SkillName.Tactics, 50.0);      // 50% of Zombiex's 100.0
            SetSkill(SkillName.Wrestling, 45.0, 50.0); // ~50% of Zombiex's 90.1-100.0

            PassiveSpeed = .4;
            ActiveSpeed = .8;

            Fame = 3000;  // 50% of Zombiex's 6000
            Karma = -3000; // 50% of Zombiex's -6000

            VirtualArmor = 20; // 50% of Zombiex's 40

            m_Timer = new RotTimer(this);
            m_Timer.Start();
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Poor);
        }

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (!m.Hidden && InRange(m, 8) && InLOS(m))
            {
                if (IsEnemy(m) && m.AccessLevel < AccessLevel.Counselor)
                {
                    Combatant = m;
                    Warmode = true;
                }
            }
            // LesserZombies don't call other zombies to simplify behavior
        }

        public override void OnGotMeleeAttack(Mobile attacker)
        {
            if (HitsMaxSeed < 5)
            {
                HitsMaxSeed = 5;
            }
            if (Hits < HitsMax / 6)
            {
                CantWalk = true;
            }
            if (attacker is PlayerMobile)
            {
                switch (Utility.Random(2))
                {
                    case 0:
                        Str -= 1;
                        break;
                }
            }
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (defender is BaseCreature)
            {
                switch (Utility.Random(4))
                {
                    case 0:
                        Str -= 1;
                        break;
                }
            }
        }

        public Item CloneItem(Item item)
        {
            if ((item.Layer != Layer.Bank) && (item.Layer != Layer.Backpack))
            {
                Item newItem = new Item(item.ItemID);
                newItem.Hue = item.Hue;
                newItem.Layer = item.Layer;

                return newItem;
            }

            return null;
        }

        private class RotTimer : Timer
        {
            private Mobile m_Mobile;

            public RotTimer(Mobile m) : base(TimeSpan.FromMinutes(5.0))
            {
                m_Mobile = m;
            }

            protected override void OnTick()
            {
                RotTimer rotTimer = new RotTimer(m_Mobile);
                if (m_Mobile.Str < 2)
                {
                    m_Mobile.Kill();
                    Stop();
                }
                else
                {
                    if (m_Mobile.Hits > m_Mobile.HitsMax / 2)
                    {
                        m_Mobile.Hits -= m_Mobile.HitsMax / 100;
                    }

                    if (m_Mobile.Hits <= m_Mobile.HitsMax / 2)
                    {
                        m_Mobile.Hits -= 1;
                    }

                    if (m_Mobile.Hits < m_Mobile.HitsMax / 4)
                    {
                        m_Mobile.CantWalk = true;
                    }

                    if (m_Mobile.Str > 50)
                    {
                        m_Mobile.Str -= m_Mobile.Str / 30;
                    }

                    if (m_Mobile.Str <= 50)
                    {
                        m_Mobile.Str -= 1;
                    }
                    rotTimer.Start();
                }
            }
        }

        public override bool AlwaysMurderer { get { return true; } }

        public override bool BleedImmune { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } }
        public override Poison HitPoison { get { return Poison.Lethal; } }
        public override bool CanRummageCorpses { get { return true; } }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
            if (Utility.RandomDouble() < 0.5) // Lower chance than Zombiex's 0.99
                c.DropItem(new ZombieKillerPotion());
            if (Utility.RandomDouble() < 0.5) // Lower chance than Zombiex's 0.99
                c.DropItem(new ArtifactOfSilver());
        }

        public LesserZombie(Serial serial) : base(serial)
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

            m_Timer = new RotTimer(this);
            m_Timer.Start();
        }
    }
}