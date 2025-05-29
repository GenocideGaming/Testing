using System;
using System.Collections.Generic;
using Server;
using Server.Items;

namespace Server.Items
{
    public class SupplyCrateSpawnManager
    {
        private static SupplyCrateSpawnManager m_Instance;
        public static SupplyCrateSpawnManager Instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = new SupplyCrateSpawnManager();
                return m_Instance;
            }
        }

        private readonly List<LostSupplyCrate> m_Crates = new List<LostSupplyCrate>();
        private readonly Point3D[] m_SpawnLocations = new Point3D[]
        {
            new Point3D(1320, 1604, 50), // Britain Castle Kitchen
            new Point3D(1619, 1584, -20), // East Brit Tavern Basement
            new Point3D(1619, 1764, 100), // East Brit Tower
            new Point3D(1543, 1741, 35), // East Brit Orphans
            new Point3D(1456, 1644, 20), // Britain Center
            new Point3D(1675, 1547, 51), // East Brit House
            new Point3D(1721, 1611, 2), // East Brit Beach
            new Point3D(1503, 1604, 10), // West Brit Inn
            new Point3D(1453, 1577, 30), // West Brit Theator
            new Point3D(1336, 1680, 30) // Britain Castle Garrison
        };
        private readonly TimeSpan m_SpawnIntervalMin = TimeSpan.FromMinutes(5);
        private readonly TimeSpan m_SpawnIntervalMax = TimeSpan.FromMinutes(15);
        private readonly TimeSpan m_DespawnTime = TimeSpan.FromMinutes(30);
        private Timer m_SpawnTimer;

        private SupplyCrateSpawnManager()
        {
            StartSpawnTimer();
        }

        private void StartSpawnTimer()
        {
            if (m_SpawnTimer != null)
                m_SpawnTimer.Stop();

            TimeSpan delay = TimeSpan.FromSeconds(Utility.RandomMinMax((int)m_SpawnIntervalMin.TotalSeconds, (int)m_SpawnIntervalMax.TotalSeconds));
            m_SpawnTimer = Timer.DelayCall(delay, SpawnCrates);
            m_SpawnTimer.Start();
        }

        private void SpawnCrates()
        {
            // Remove deleted crates
            m_Crates.RemoveAll(c => c == null || c.Deleted);

            // Spawn up to 5 crates
            while (m_Crates.Count < 10)
            {
                // Pick a random, unoccupied spawn location
                List<Point3D> availableLocations = new List<Point3D>(m_SpawnLocations);
                foreach (LostSupplyCrate crate in m_Crates)
                {
                    if (crate != null && !crate.Deleted)
                        availableLocations.RemoveAll(loc => loc == crate.Location);
                }

                if (availableLocations.Count == 0)
                {
                    Console.WriteLine("SupplyCrateSpawnManager: No available spawn locations.");
                    break;
                }

                Point3D spawnLoc = availableLocations[Utility.Random(availableLocations.Count)];
                LostSupplyCrate newCrate = new LostSupplyCrate();
                newCrate.MoveToWorld(spawnLoc, Map.Felucca);
                m_Crates.Add(newCrate);
                Console.WriteLine("SupplyCrateSpawnManager: Spawned crate at {0}.", spawnLoc);

                // Schedule despawn
                Timer.DelayCall(m_DespawnTime, () =>
                {
                    if (newCrate != null && !newCrate.Deleted && newCrate.Parent == null)
                    {
                        m_Crates.Remove(newCrate);
                        newCrate.Delete();
                        Console.WriteLine("SupplyCrateSpawnManager: Despawned crate at {0}.", spawnLoc);
                    }
                });
            }

            StartSpawnTimer();
        }

        public void RemoveCrate(LostSupplyCrate crate)
        {
            if (crate != null)
            {
                m_Crates.Remove(crate);
                Console.WriteLine("SupplyCrateSpawnManager: Removed crate.");
            }
        }

        public SupplyCrateSpawnManager(Serial serial)
        {
        }

        public void Serialize(GenericWriter writer)
        {
            writer.Write((int)0); // version
        }

        public void Deserialize(GenericReader reader)
        {
            int version = reader.ReadInt();
        }
    }
}