using System;
using System.Xml;
using Server.Items;
using Server.Mobiles;
using Server.Scripts.Custom.VorshunWarsEquipment;
using Server.Custom;
using Server.Gumps;

namespace Server.Regions
{
    class VorshunRegion : DungeonRegion  // new region for events. Switch between this and WorldWarsRegion in Regions.xml for Vorshun depending on event
    {
        public VorshunRegion(XmlElement element, Map map, Region parent) : base(element, map, parent) { }

        public override void OnEnter(Mobile m)
        {
            if (WorldWarsController.Instance != null && WorldWarsController.Instance.Active)
            {
                PlayerMobile player = m as PlayerMobile;
                if (player != null)
                {
                    if (!player.Alive)
                    {
                        WorldWarsController controller = WorldWarsController.Instance;
                        if (controller == null)
                            return;
                        Point3D goLocation = WorldWarGate.GetStagingAreaGoLocation(m);
                        m.MoveToWorld(goLocation, Map.Felucca);
                    }
                }
            }
            else
            {
                RoacheCriminalTimer criminalTimer = new RoacheCriminalTimer(m);
                criminalTimer.Start();
            }

            base.OnEnter(m);
        }

        public override void OnDeath(Mobile m)
        {
            base.OnDeath(m);
            int sound = m.GetDeathSound();
            if (sound >= 0)
                Effects.PlaySound(m, m.Map, sound);

            if (WorldWarsController.Instance != null && WorldWarsController.Instance.Active)
            {
                WorldWarsController controller = WorldWarsController.Instance;
                if (controller == null)
                    return;
                Point3D goLocation = WorldWarGate.GetStagingAreaGoLocation(m);
                m.MoveToWorld(goLocation, Map.Felucca);
            }
        }

        public override void OnExit(Mobile m, Region newRegion)
        {
            if (WorldWarsController.Instance != null && WorldWarsController.Instance.Active)
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
                pm.CloseGump(typeof(ChooseEquipmentGump));
                if (VorshunStorage.ContainsStuffBelongingTo(pm))
                {
                    if (pm.Backpack != null)
                    {
                        foreach (Item item in pm.Backpack.Items)
                            item.Movable = false;
                    }
                    VorshunStorage.RestoreOriginalItems(pm);
                }
            }

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
            if (newRegion == null) return true;
            return (newRegion.Map == Map.Internal);
        }

        private class RoacheCriminalTimer : Timer
        {
            private Mobile player;

            public RoacheCriminalTimer(Mobile m)
                : base(TimeSpan.Zero, TimeSpan.FromSeconds(1.0))
            {
                player = m;
            }

            protected override void OnTick()
            {
                if (player.Region.IsPartOf("Vorshun"))
                    player.Criminal = true;
                else
                {
                    Timer.DelayCall(TimeSpan.FromSeconds(10.0), new TimerCallback(MakeUnCriminal));
                    this.Stop();
                }
            }

            private void MakeUnCriminal()
            {
                player.Criminal = false;
            }
        }
    }
}
