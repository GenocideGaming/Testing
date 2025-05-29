using System;
using System.Collections;
using Server.Mobiles;

namespace Server.Items
{
    public class MonsterNest : Item
    {
        private ArrayList m_Spawn;
        private Mobile m_Entity;

        [CommandProperty(AccessLevel.GameMaster)]
        public string NestSpawnType { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public int MaxCount { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan RespawnTime { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public int HitsMax { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Hits { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public int RangeHome { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public int LootLevel { get; set; }

        public override double DefaultWeight { get { return 0.1; } }
        public override string DefaultName { get { return "Monster Nest"; } }

        [Constructable]
        public MonsterNest() : this(4962)
        { }

        public MonsterNest(int itemID) : base(itemID)
        {
            NestSpawnType = null;
            m_Spawn = new ArrayList();
            Hue = 1818;
            HitsMax = 300;
            Hits = 300;
            RangeHome = 10;
            RespawnTime = TimeSpan.FromSeconds(15.0);
            new InternalTimer(this).Start();
            new RegenTimer(this).Start();
            Movable = false;
            m_Entity = new MonsterNestEntity(this);
            m_Entity.MoveToWorld(Location, Map);
        }

        public override void OnDelete()
        {
            base.OnDelete();

            if (m_Spawn != null && m_Spawn.Count > 0)
            {
                for (int i = 0; i < m_Spawn.Count; i++)
                {
                    Mobile m = (Mobile)m_Spawn[i];
                    m.Delete();
                }
            }
            if (m_Entity != null)
                m_Entity.Delete();
        }

        public void Damage(int damage)
        {
            Hits -= damage;
            //PublicOverheadMessage( MessageType.Regular, 0x22, false, damage.ToString() );
            if (Hits <= 0)
                Destroy();
        }

        public override void OnDoubleClick(Mobile from)
        {
            from.SendMessage(0, "You begin to attack the hideous nest!");
            if (m_Entity != null)
                from.Combatant = m_Entity;
        }

        public virtual void AddLoot()
        {
            MonsterNestLoot loot = new MonsterNestLoot(6585, Hue, LootLevel, "Monster Nest remains");
            loot.MoveToWorld(Location, Map);
        }

        public void Destroy()
        {
            AddLoot();
            if (m_Spawn != null && m_Spawn.Count > 0)
            {
                for (int i = 0; i < m_Spawn.Count; i++)
                {
                    Mobile m = (Mobile)m_Spawn[i];
                    m.Kill();
                }
            }
            if (m_Entity != null)
                m_Entity.Delete();
            Delete();
        }

        public int Count()
        {
            int c = 0;
            if (m_Spawn != null && m_Spawn.Count > 0)
            {
                for (int i = 0; i < m_Spawn.Count; i++)
                {
                    Mobile m = (Mobile)m_Spawn[i];
                    if (m.Alive)
                        c += 1;
                }
            }
            return c;
        }

        public void DoSpawn()
        {
            if (m_Entity != null)
                m_Entity.MoveToWorld(Location, Map);
            if (NestSpawnType != null && m_Spawn != null && Count() < MaxCount)
            {
                try
                {
                    Type type = SpawnerType.GetType(NestSpawnType);
                    BaseCreature m = Activator.CreateInstance(type) as BaseCreature;
                    if (m != null)
                    {
                        m_Spawn.Add(m);
                        m.OnBeforeSpawn(Location, Map);
                        m.MoveToWorld(Location, Map);
                        m.Home = Location;
                        m.RangeHome = RangeHome;
                    }
                }
                catch
                {
                    NestSpawnType = null;
                }
            }
        }

        public MonsterNest(Serial serial) : base(serial)
        { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0);
            writer.Write(NestSpawnType);
            writer.WriteMobileList(m_Spawn);
            writer.Write(MaxCount);
            writer.Write(RespawnTime);
            writer.Write(HitsMax);
            writer.Write(Hits);
            writer.Write(RangeHome);
            writer.Write(LootLevel);
            writer.Write(m_Entity);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();

            NestSpawnType = reader.ReadString();
            m_Spawn = reader.ReadMobileList();
            MaxCount = reader.ReadInt();
            RespawnTime = reader.ReadTimeSpan();
            HitsMax = reader.ReadInt();
            Hits = reader.ReadInt();
            RangeHome = reader.ReadInt();
            LootLevel = reader.ReadInt();
            m_Entity = reader.ReadMobile();
        }

        private class RegenTimer : Timer
        {
            private readonly MonsterNest nest;

            public RegenTimer(MonsterNest n) : base(TimeSpan.FromMinutes(1.0))
            {
                nest = n;
            }

            protected override void OnTick()
            {
                if (nest != null && !nest.Deleted)
                {
                    nest.Hits += nest.HitsMax / 10;
                    if (nest.Hits > nest.HitsMax)
                        nest.Hits = nest.HitsMax;
                    new RegenTimer(nest).Start();
                }
            }
        }

        private class InternalTimer : Timer
        {
            private readonly MonsterNest nest;

            public InternalTimer(MonsterNest n) : base(n.RespawnTime)
            {
                nest = n;
            }

            protected override void OnTick()
            {
                if (nest != null && !nest.Deleted)
                {
                    nest.DoSpawn();
                    new InternalTimer(nest).Start();
                }
            }
        }
    }

    public class MonsterNestEntity : BaseCreature
    {
        private MonsterNest m_MonsterNest;

        [Constructable]
        public MonsterNestEntity(MonsterNest nest) : base(AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            m_MonsterNest = nest;
            Name = nest.Name;
            Title = "";
            Body = 399;
            BaseSoundID = 0;
            Hue = 0;

            SetStr(0);
            SetDex(0);
            SetInt(0);

            SetHits(nest.HitsMax);

            SetDamage(0, 0);

            SetDamageType(ResistanceType.Physical, 0);

            SetResistance(ResistanceType.Physical, 0);
            SetResistance(ResistanceType.Fire, 0);
            SetResistance(ResistanceType.Cold, 0);
            SetResistance(ResistanceType.Poison, 0);
            SetResistance(ResistanceType.Energy, 0);

            Fame = 5000;
            Karma = -5000;

            VirtualArmor = 0;
            CantWalk = true;
        }

        public override void OnThink()
        {
            Frozen = true;
            Location = m_MonsterNest.Location;
            if (m_MonsterNest != null)
            {
                Hits = m_MonsterNest.Hits;
            }
        }

        public override void OnDamage(int amount, Mobile from, bool willkill)
        {
            if (m_MonsterNest != null)
            {
                m_MonsterNest.Damage(amount);
            }
            base.OnDamage(amount, from, willkill);
        }

        public override int GetAngerSound()
        {
            return 9999;
        }

        public override int GetIdleSound()
        {
            return 9999;
        }

        public override int GetAttackSound()
        {
            return 9999;
        }

        public override int GetHurtSound()
        {
            return 9999;
        }

        public override int GetDeathSound()
        {
            return 9999;
        }

        public override bool OnBeforeDeath()
        {
            return false;
        }

        public MonsterNestEntity(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
            writer.Write(m_MonsterNest);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            reader.ReadInt();
            m_MonsterNest = (MonsterNest)reader.ReadItem();
        }
    }
}
