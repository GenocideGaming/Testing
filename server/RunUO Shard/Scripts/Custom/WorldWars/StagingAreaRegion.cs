using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Server.Factions;
using Server.Mobiles;
using Server.Scripts.Custom.VorshunWarsEquipment;
using Server.Scripts.Custom.Citizenship;
using Server.Items;
using Server.Gumps;

namespace Server.Regions
{
    public class StagingAreaRegion : DungeonRegion
    {
        public StagingAreaRegion(XmlElement xml, Map map, Region parent)
            : base(xml, map, parent)
        {
        }
        public override void OnEnter(Mobile m)
        {
            if (Commonwealth.Find(m) == null && m.AccessLevel == AccessLevel.Player && m is PlayerMobile)
            {
                Timer.DelayCall(TimeSpan.FromSeconds(1.0), new TimerCallback(((PlayerMobile)m).ReturnToGalven));
                m.SendMessage("You must be a citizen of a town to participate in Vorshun Wars.");
                return;
            }
            
            PlayerMobile pm = m as PlayerMobile;
            if (pm == null) { return; }
            pm.TeamFlags = pm.TeamFlagsCitizenship;
            ICommonwealth township = Commonwealth.Find(pm);
            if (township == null) { return; }

            if (pm.Holding != null)
                RemoveHeldItem(pm);

            if (pm.Alive == false)
            {
                pm.Resurrect();
            }

            if (!VorshunStorage.ContainsStuffBelongingTo(pm))
                VorshunStorage.Store(pm);
        }

        public override bool OnResurrect(Mobile m)
        {
            PlayerMobile pm = m as PlayerMobile;
            if (pm == null) { return base.OnResurrect(m); }

            VorshunArmory.Restock(m);

            return base.OnResurrect(m);
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

            if (newRegion is VorshunRegion 
                && WorldWarsController.Instance != null 
                && WorldWarsController.Instance.Active)
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
    }
}
