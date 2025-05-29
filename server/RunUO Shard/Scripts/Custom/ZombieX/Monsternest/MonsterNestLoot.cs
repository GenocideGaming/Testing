using System;
using System.Collections.Generic;
using Server.Mobiles;

namespace Server.Items
{
    public class MonsterNestLoot : Item
    {
        private int m_LootLevel;
        private string m_name;
        public override string DefaultName { get { return m_name + " (double click to loot)"; } }

        [Constructable]
        public MonsterNestLoot(int itemid, int hue, int lootlevel, string name) : base(itemid)
        {
            m_name = name;
            Hue = hue;
            Movable = false;
            m_LootLevel = lootlevel;
        }

        public override void OnDoubleClick(Mobile from)
        {
            List<Mobile> list = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(Location, 20);

            foreach (Mobile m in eable)
            {
                if (m is PlayerMobile)
                    list.Add(m);
            }
            eable.Free();

            foreach (Mobile m in list)
                AddLoot(m);
            list.Clear();
            Delete();
        }

        public void AddLoot(Mobile m)
        {
            int chance = Utility.Random(5, 20) * m_LootLevel;

            if (chance < 10)
            {
                m.AddToBackpack(new Gold(Utility.Random(500, 1000)));
            }
            else if (chance < 20)
            {
                m.AddToBackpack(new Gold(Utility.Random(1000, 2000)));
            }
            else if (chance < 30)
            {
                m.AddToBackpack(new BankCheck(Utility.Random(2000, 3000)));
            }
            else if (chance < 40)
            {
                m.AddToBackpack(new BankCheck(Utility.Random(3000, 4000)));
            }
            else if (chance < 55)
            {
                m.AddToBackpack(new BankCheck(Utility.Random(4000, 5000)));
            }
            else if (chance < 65)
            {
                m.AddToBackpack(new BankCheck(Utility.Random(5000, 6000)));
            }
            else if (chance < 70)
            {
                m.AddToBackpack(new BankCheck(Utility.Random(6000, 7000)));
            }
            else if (chance < 85)
            {
                m.AddToBackpack(new BankCheck(Utility.Random(7000, 8000)));
            }
            else if (chance < 95)
            {
                m.AddToBackpack(new BankCheck(Utility.Random(8000, 9000)));
            }
            else
            {
                m.AddToBackpack(new BankCheck(Utility.Random(7000, 10000)));
            }
        }

        public MonsterNestLoot(Serial serial) : base(serial)
        { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0);
            writer.Write(m_LootLevel);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();

            m_LootLevel = reader.ReadInt();
        }

        private class RegenTimer : Timer
        {
            private readonly MonsterNest nest;

            public RegenTimer(MonsterNest n) : base(TimeSpan.FromSeconds(30.0))
            {
                nest = n;
            }

            protected override void OnTick()
            {
                if (nest != null && !nest.Deleted)
                {
                    if (nest.Hits < 0)
                    {
                        nest.Destroy();
                        return;
                    }
                    nest.Hits += nest.HitsMax / 10;
                    if (nest.Hits > nest.HitsMax)
                        nest.Hits = nest.HitsMax;
                    new RegenTimer(nest).Start();
                }
            }
        }

        private class SpawnTimer : Timer
        {
            private readonly MonsterNest nest;

            public SpawnTimer(MonsterNest n) : base(n.RespawnTime)
            {
                nest = n;
            }

            protected override void OnTick()
            {
                if (nest != null && !nest.Deleted)
                {
                    nest.DoSpawn();
                    new SpawnTimer(nest).Start();
                }
            }
        }
    }
}
