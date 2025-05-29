using System;
using Server;
using Server.Mobiles;
using Server.Items;
using System.Collections.Generic;
using Server.Engines.Quests;

namespace Server.Mobiles
{
    public class SurvivorEscortable : BaseEscortable
    {
        [Constructable]
        public SurvivorEscortable()
        {
            Name = "a survivor";
            Body = Utility.RandomBool() ? 0x190 : 0x191; // Male or female
            Hue = Utility.RandomSkinHue();

            // Clothing
            AddItem(new ShortPants(Utility.RandomNeutralHue()));
            AddItem(new Shirt(Utility.RandomNeutralHue()));
            AddItem(new Boots());

            // Random weapon
            Type[] weaponTypes = new Type[] { typeof(Club), typeof(Mace), typeof(Dagger), typeof(QuarterStaff) };
            AddItem((Item)Activator.CreateInstance(weaponTypes[Utility.Random(weaponTypes.Length)]));

            // Random hair
            Utility.AssignRandomHair(this);

            // Random headgear: Bandana, Skullcap, or none (33.3% each)
            switch (Utility.Random(3))
            {
                case 0: AddItem(new Bandana(Utility.RandomNeutralHue())); break;
                case 1: AddItem(new SkullCap(Utility.RandomNeutralHue())); break;
                case 2: break; // No headgear
            }

            // Stats and skills
            SetStr(30, 50);
            SetDex(20, 40);
            SetInt(20, 30);
            SetSkill(SkillName.Wrestling, 20.0, 30.0);
            Fame = 100;
            Karma = 1000;
            SpeechHue = 1153;
            SetControlMaster(null); // Ensure no control master
            Destination = "Britain Dockhouse"; // Set destination
        }

        public override void InitBody()
        {
            // Set in constructor
        }

        public override void InitOutfit()
        {
            // Set in constructor
        }

        public override bool Escortable
        {
            get { return true; }
        }

        public override void OnThink()
        {
            base.OnThink();
            // Panic dialogue when zombies are nearby
            Mobile escorter = GetEscorter();
            if (escorter != null)
            {
                foreach (Mobile m in GetMobilesInRange(5))
                {
                    if (m is Zombiex || m.GetType().Name == "LesserZombie")
                    {
                        SayTo(escorter, "Oh no, they’re coming! Help!");
                        break;
                    }
                }
            }
        }

        public override bool AcceptEscorter(Mobile m)
        {
            //Console.WriteLine("SurvivorEscortable: Attempting to accept escorter {0}.", m?.Name ?? "null");

            // Validate mobile
            if (m == null)
            {
                Console.WriteLine("SurvivorEscortable: Failed - mobile is null.");
                return false;
            }

            if (!m.Alive)
            {
                Console.WriteLine("SurvivorEscortable: Failed - mobile is not alive.");
                return false;
            }

            // Check if survivor is already escorted
            if (GetEscorter() != null)
            {
                Console.WriteLine("SurvivorEscortable: Failed - survivor already has an escorter.");
                m.SendMessage("This survivor is already being escorted.");
                return false;
            }

            // Check if mobile is a player
            if (!(m is PlayerMobile))
            {
                Console.WriteLine("SurvivorEscortable: Failed - mobile {0} is not a PlayerMobile.", m.Name);
                return false;
            }

            PlayerMobile player = (PlayerMobile)m;

            // Check quest or GameMaster status
            if (!(player.Quest is SurvivorRescueQuest) && player.AccessLevel < AccessLevel.GameMaster)
            {
                Console.WriteLine("SurvivorEscortable: Player {0} does not have SurvivorRescueQuest.", m.Name);
                m.SendMessage("You must have the survivor rescue quest to escort this survivor.");
                return false;
            }

            // Attempt to accept escort
            if (!base.AcceptEscorter(m))
            {
                Console.WriteLine("SurvivorEscortable: base.AcceptEscorter failed for {0}.", m.Name);
                m.SendMessage("Unable to escort this survivor at this time.");
                return false;
            }

            // Spawn zombies when escort begins
            SpawnZombies();
            Console.WriteLine("SurvivorEscortable: Escort accepted by {0}.", m.Name);
            return true;
        }

        private void SpawnZombies()
        {
            int zombieCount = Utility.RandomMinMax(2, 4);
            for (int i = 0; i < zombieCount; i++)
            {
                Mobile zombie = Utility.RandomBool() ? new Zombiex() : (Mobile)Activator.CreateInstance(Type.GetType("Server.Mobiles.LesserZombie"));
                Point3D loc = new Point3D(Location.X + Utility.RandomMinMax(-5, 5), Location.Y + Utility.RandomMinMax(-5, 5), Location.Z);
                zombie.MoveToWorld(loc, Map);
            }
        }

        public override bool CheckAtDestination()
        {
            EscortDestinationInfo dest = GetDestination();
            Mobile escorter = GetEscorter();

            if (dest == null || escorter == null)
                return false;

            if (dest.Contains(Location))
            {
                // Send thank you message
                SayTo(escorter, "Thank you… I thought I was done for!");

                // Notify player of rewards
                escorter.SendMessage("You’ve saved a survivor! You receive 1000 gold and some supplies.");

                // Add rewards to backpack or bank box
                Container cont = escorter.Backpack ?? escorter.BankBox;
                if (cont != null)
                {
                    cont.TryDropItem(escorter, new Gold(1000), false);
                    cont.TryDropItem(escorter, new Bandage(10), false);
                }

                /* Update escort counter
                if (escorter is PlayerMobile pm)
                {
                    pm.EscortsTakenThisSession++;
                    CheckSurvivorMilestone(pm);
                }
                */
                // Clean up escort
                StopFollow();
                SetControlMaster(null);
                EscortTable.Remove(escorter);
                SurvivorSpawnManager.Instance.RemoveSurvivor(this);
                Timer.DelayCall(TimeSpan.FromSeconds(5.0), () => Delete()); // Delete after 5 seconds
                return true;
            }
            return false;
        }

        private void CheckSurvivorMilestone(PlayerMobile pm)
        {
            int rescues = pm.EscortsTakenThisSession;
            if (rescues == 1)
                pm.SendMessage("You’ve rescued your first survivor this session!");
            else if (rescues == 5)
                pm.SendMessage("Milestone: You’ve rescued 5 survivors this session! Keep it up!");
            else if (rescues == 20)
                pm.SendMessage("Heroic Milestone: You’ve rescued 20 survivors this session! You’re a true savior!");
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.Alive && from.InRange(this, 2))
            {
                Console.WriteLine("SurvivorEscortable: Double-click by {0}.", from.Name);
                AcceptEscorter(from);
            }
        }

        public SurvivorEscortable(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}