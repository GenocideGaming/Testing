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
    public class WorldWarGate : Moongate
    {
        public static List<WorldWarGate> CurrentEventGates = new List<WorldWarGate>();
        public const int EventGateHue = 38;
        private string m_Commonwealth = null;

        //It goes Lillano, Pedran, Arbor, Calor, Vermell
        private static readonly Point3D[] m_TownMoongateLocations = new Point3D[] { new Point3D(5398, 3910, 0), new Point3D(5528, 3910, 0), new Point3D(5648, 3910, 0), new Point3D(5848, 3910, 0), new Point3D(5998, 3910, 0) };
        private static readonly Point3D[] m_StagingAreaTeleportLocations = new Point3D[] { new Point3D(4253, 3920, 13), new Point3D(4378, 3920, 13), new Point3D(4503, 3920, 13), new Point3D(4628, 3920, 13), new Point3D(4753, 3920, 13) };
        public static readonly Point3D DefaultSendHomeLocation = new Point3D(1354, 1382, 0);

        [Constructable]
        public WorldWarGate(bool bDispellable)
            : base(bDispellable)
        {
        }

        public WorldWarGate(Serial serial)
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

        public static void OpenGates()
        {
            for (int i = 0; i < m_TownMoongateLocations.Length; i++)
            {
                WorldWarGate newGate = new WorldWarGate(false);
                // hacky way to do it, but it works
                if (i == 0) newGate.m_Commonwealth = "Lillano";
                else if (i == 1) newGate.m_Commonwealth = "Pedran";
                else if (i == 2) newGate.m_Commonwealth = "Arbor";
                else if (i == 3) newGate.m_Commonwealth = "Calor";
                else if (i == 4) newGate.m_Commonwealth = "Vermell";
                
                newGate.Target = m_StagingAreaTeleportLocations[i];
                newGate.TargetMap = Map.Felucca;
                newGate.MoveToWorld(m_TownMoongateLocations[i], Map.Felucca);
                newGate.Name = "Vorshun World Wars";
                newGate.Hue = EventGateHue;
                CurrentEventGates.Add(newGate);
            }
        }

        public override void CheckGate(Mobile m, int range)
        {
            PlayerMobile pm = m as PlayerMobile;
            if (m_Commonwealth == null || (pm != null && pm.Citizenship != m_Commonwealth))
            {
                m.SendMessage("You are not a citizen of " + m_Commonwealth + "!");
                return;
            }
            base.CheckGate(m, range);
        }

        public override void UseGate(Mobile m)
        {
            ClientFlags flags = m.NetState == null ? ClientFlags.None : m.NetState.Flags;

            if (m.Spell != null)
            {
                m.SendLocalizedMessage(1049616); // You are too busy to do that at the moment.
            }
            else if (TargetMap != null && TargetMap != Map.Internal)
            {
                if (WorldWarsController.Instance != null && WorldWarsController.Instance.PetsAllowed)
                {
                    BaseCreature.TeleportPetsButNotEscortables(m, Target); // Will not take escortables.
                }

                m.MoveToWorld(Target, TargetMap);

                if (m.AccessLevel == AccessLevel.Player || !m.Hidden)
                    m.PlaySound(0x1FE);

                OnGateUsed(m);
            }
            else
            {
                m.SendMessage("This moongate does not seem to go anywhere.");
            }
        }

        public static void RemoveGates()
        {
            while (CurrentEventGates.Count != 0)
            {
                CurrentEventGates[0].Delete();
                CurrentEventGates.RemoveAt(0);
            }
            Server.Commands.CommandHandlers.BroadcastMessage(AccessLevel.Player, 0x30, "The Galven gate authority has closed the world war portals.");
        }

        public static Point3D GetStagingAreaGoLocation(Mobile m)
        {
            PlayerMobile pm = m as PlayerMobile;
            if (pm == null || pm.CitizenshipPlayerState == null || pm.CitizenshipPlayerState.Commonwealth == null)
            {
                return m.Location; // shouldn't ever happen
            }
            ICommonwealth commonwealth = pm.CitizenshipPlayerState.Commonwealth;
            Point3D goLocation = new Point3D();
            if (commonwealth != null && commonwealth.Definition != null)
            {
                switch (commonwealth.Definition.TownName.ToString())
                {
                    case "Lillano":
                        goLocation = m_StagingAreaTeleportLocations[0];
                        break;
                    case "Pedran":
                        goLocation = m_StagingAreaTeleportLocations[1];
                        break;
                    case "Arbor":
                        goLocation = m_StagingAreaTeleportLocations[2];
                        break;
                    case "Calor":
                        goLocation = m_StagingAreaTeleportLocations[3];
                        break;
                    case "Vermell":
                        goLocation = m_StagingAreaTeleportLocations[4];
                        break;
                }
            }
            else
            {
                goLocation = DefaultSendHomeLocation;
            }

            return goLocation;
        }
    }
}
