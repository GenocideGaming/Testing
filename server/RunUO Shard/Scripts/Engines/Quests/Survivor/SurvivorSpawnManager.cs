using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;

namespace Server.Mobiles
{
    public class SurvivorSpawnManager
    {
        private static SurvivorSpawnManager m_Instance;
        public static SurvivorSpawnManager Instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = new SurvivorSpawnManager();
                return m_Instance;
            }
        }

        private readonly List<SurvivorEscortable> m_Survivors = new List<SurvivorEscortable>();
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

        private SurvivorSpawnManager()
        {
            StartSpawnTimer();
        }

        private void StartSpawnTimer()
        {
            if (m_SpawnTimer != null)
                m_SpawnTimer.Stop();

            TimeSpan delay = TimeSpan.FromSeconds(Utility.RandomMinMax((int)m_SpawnIntervalMin.TotalSeconds, (int)m_SpawnIntervalMax.TotalSeconds));
            m_SpawnTimer = Timer.DelayCall(delay, SpawnSurvivors);
            m_SpawnTimer.Start();
        }

        private void SpawnSurvivors()
        {
            // Remove deleted survivors
            m_Survivors.RemoveAll(s => s == null || s.Deleted);

            // Spawn up to 3 survivors
            while (m_Survivors.Count < 3)
            {
                // Pick a random, unoccupied spawn location
                List<Point3D> availableLocations = new List<Point3D>(m_SpawnLocations);
                foreach (SurvivorEscortable survivor in m_Survivors)
                {
                    if (survivor != null && !survivor.Deleted)
                        availableLocations.RemoveAll(loc => loc == survivor.Location);
                }

                if (availableLocations.Count == 0)
                {
                    Console.WriteLine("SurvivorSpawnManager: No available spawn locations.");
                    break;
                }

                Point3D spawnLoc = availableLocations[Utility.Random(availableLocations.Count)];
                SurvivorEscortable newSurvivor = new SurvivorEscortable();
                newSurvivor.MoveToWorld(spawnLoc, Map.Felucca);
                newSurvivor.Destination = "Britain Dockhouse";
                newSurvivor.SetControlMaster(null); // Ensure no control master
                m_Survivors.Add(newSurvivor);
                Console.WriteLine("SurvivorSpawnManager: Spawned survivor at {0}.", spawnLoc);

                // Schedule despawn
                Timer.DelayCall(m_DespawnTime, () =>
                {
                    if (newSurvivor != null && !newSurvivor.Deleted && newSurvivor.GetEscorter() == null)
                    {
                        m_Survivors.Remove(newSurvivor);
                        newSurvivor.Delete();
                        Console.WriteLine("SurvivorSpawnManager: Despawned survivor at {0}.", spawnLoc);
                    }
                });
            }

            StartSpawnTimer();
        }

        public void RemoveSurvivor(SurvivorEscortable survivor)
        {
            if (survivor != null)
            {
                m_Survivors.Remove(survivor);
                Console.WriteLine("SurvivorSpawnManager: Removed survivor.");
            }
        }

        public SurvivorEscortable FindAvailableSurvivor()
        {
            foreach (SurvivorEscortable survivor in m_Survivors)
            {
                if (survivor != null && !survivor.Deleted && survivor.GetEscorter() == null)
                    return survivor;
            }
            return null;
        }
    }
}