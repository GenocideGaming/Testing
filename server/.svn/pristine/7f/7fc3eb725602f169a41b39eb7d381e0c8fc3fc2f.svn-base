using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Server.Items;
using Server.Factions;
using Server.Mobiles;
using Server.Scripts.Custom.VorshunWarsEquipment;
using Server.Custom;

namespace Server.Regions
{
    public class WorldWarsRegion : BaseRegion
    {
        public WorldWarsRegion(XmlElement xml, Map map, Region parent)
            : base(xml, map, parent)
        {


        }
        public override void OnEnter(Mobile m)
        {
            PlayerMobile player = m as PlayerMobile;
            if (player != null && !player.Alive)
            {
                WorldWarsController controller = WorldWarsController.Instance;
                if (controller == null)
                    return;
                Point3D goLocation = WorldWarGate.GetStagingAreaGoLocation(m);
                m.MoveToWorld(goLocation, Map.Felucca);
            }
            base.OnEnter(m);
        }
        public override void OnDeath(Mobile m)
        {
            base.OnDeath(m);

            WorldWarsController controller = WorldWarsController.Instance;
            if (controller == null)
                return;
            Point3D goLocation = WorldWarGate.GetStagingAreaGoLocation(m);
            m.MoveToWorld(goLocation, Map.Felucca);
        }

        public override void OnExit(Mobile m, Region newRegion)
        {
            PlayerMobile pm = m as PlayerMobile;
            if (pm == null) { return; }

            if (PlayerIsLoggingOut(newRegion))
            {
                base.OnExit(pm, newRegion);
                return;
            }

            if (newRegion is StagingAreaRegion)
            {
                base.OnExit(pm, newRegion);
                return;
            }

            if (pm.Holding != null)
                RemoveHeldItem(pm);

            if (pm.Backpack != null)
            {
                foreach (Item item in pm.Backpack.Items)
                    item.Movable = false;
            }

            if (VorshunStorage.ContainsStuffBelongingTo(pm))
                VorshunStorage.RestoreOriginalItems(pm);

            base.OnExit(m, newRegion);
        }

        private void DropHeldItem(Mobile m)
        {
            m.Holding.Delete();
            m.Holding.ClearBounce();
            m.Holding = null;
        }

        private void RemoveHeldItem(Mobile m)
        {
            m.AddToBackpack(m.Holding);

            m.Holding.ClearBounce();
            m.Holding = null;
        }

        private bool PlayerIsLoggingOut(Region newRegion)
        {
            return (newRegion.Map == Map.Internal);
        }
       
    }
}
