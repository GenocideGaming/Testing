using System;
using System.Collections;
using System.Collections.Generic;
using Server.Accounting;
using Server.Commands;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Items;
using Server.Scripts.Custom.Citizenship;

namespace Server.Custom
{
    public class WorldWarPlank : Teleporter
    {
        public static List<WorldWarPlank> CurrentEventTeleporters = new List<WorldWarPlank>();

        //It goes Lillano, Pedran, Arbor, Calor, Vermell
        private static readonly Point3D[] m_TeleporterLocations = new Point3D[] { new Point3D(4254, 3917, 11), new Point3D(4379, 3917, 11), new Point3D(4504, 3917, 11), new Point3D(4629, 3917, 11), new Point3D(4754, 3917, 11) };
        private static readonly Point3D[] m_VorshunTeleportLocations = new Point3D[] { new Point3D(527, 1861, 2), new Point3D(612, 1823, 2), new Point3D(686, 1846, 2), new Point3D(666, 1943, 2), new Point3D(605, 1929, 2) };

        [Constructable]
        public WorldWarPlank()
            : base()
        {
        }

        public WorldWarPlank(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            // do not save worldwar gates!
        }

        public override void Deserialize(GenericReader reader)
        {
            // do not load worldwar gates!
        }

        public static void OpenTeleporters()
        {
            int randomOffset = Utility.RandomMinMax(0,4); // so they don't start in the same starting point each time
            for (int i = 0; i < m_TeleporterLocations.Length; i++)
            {
                WorldWarPlank newTeleporter = new WorldWarPlank();
                newTeleporter.MoveToWorld(m_TeleporterLocations[i], Map.Felucca);
                newTeleporter.MapDest = Map.Felucca;
                newTeleporter.PointDest = m_VorshunTeleportLocations[(i + randomOffset) % 5];
                CurrentEventTeleporters.Add(newTeleporter);
                //Effects.SendBoltEffect((IEntity)newTeleporter);
                Point3D secondTeleporterLocation = new Point3D(m_TeleporterLocations[i].X + 1, m_TeleporterLocations[i].Y, m_TeleporterLocations[i].Z);
                newTeleporter = new WorldWarPlank();
                newTeleporter.MoveToWorld(secondTeleporterLocation, Map.Felucca);
                newTeleporter.MapDest = Map.Felucca;
                newTeleporter.PointDest = m_VorshunTeleportLocations[(i + randomOffset) % 5];
                CurrentEventTeleporters.Add(newTeleporter);
                //Effects.SendBoltEffect((IEntity)newTeleporter);
                Effects.SendLocationEffect(m_TeleporterLocations[i], Map.Felucca, 14000, 50);
                Effects.SendLocationEffect(secondTeleporterLocation, Map.Felucca, 14000, 50);
                Effects.PlaySound(((IEntity)newTeleporter).Location, ((IEntity)newTeleporter).Map, 519);
            }
        }

        public static void RemoveTeleporters()
        {
            while (CurrentEventTeleporters.Count != 0)
            {
                CurrentEventTeleporters[0].Delete();
                CurrentEventTeleporters.RemoveAt(0);
            }
        }
    }
}
