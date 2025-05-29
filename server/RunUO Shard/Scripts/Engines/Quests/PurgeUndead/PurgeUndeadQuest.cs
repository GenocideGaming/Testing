using System;
using Server;
using Server.Mobiles;
using Server.Engines.Quests;
using Server.Items;

namespace Server.Engines.Quests
{
    public class PurgeUndeadQuest : QuestSystem
    {
        private static readonly Type[] m_TypeReferenceTable = new Type[]
        {
            typeof(KillUndeadObjective),
            typeof(PurgeUndeadConversation)
        };

        public override Type[] TypeReferenceTable
        {
            get { return m_TypeReferenceTable; }
        }

        public override object Name
        {
            get { return "Purge the Undead"; }
        }

        public override object OfferMessage
        {
            get { return "The undead plague Britain’s lands. Slay 5 each of Zombie, Skeleton, Bogle, Lich, and Wraith to cleanse this evil. Return to me when the task is done."; }
        }

        public override TimeSpan RestartDelay
        {
            get { return TimeSpan.FromSeconds(1); } // Minimal delay, daily reset controls
        }

        public override bool IsTutorial
        {
            get { return false; }
        }

        public override int Picture
        {
            get { return 0x15C9; } // Quest scroll icon
        }

        public PurgeUndeadQuest(PlayerMobile from) : base(from)
        {
        }

        public PurgeUndeadQuest()
        {
        }

        public override void Accept()
        {
            base.Accept();
            From.SendMessage("Seek out and slay 5 each of Zombie, Skeleton, Bogle, Lich, and Wraith, then return to High Priestess Liora.");
            AddObjective(new KillUndeadObjective(this));
        }

        public override void Complete()
        {
            From.SendMessage("You have cleansed the undead scourge! High Priestess Liora rewards you for your bravery.");
            Container cont = From.Backpack ?? From.BankBox;
            if (cont != null)
            {
                cont.AddItem(new Gold(2000));
                cont.AddItem(new Bandage(20));
                // Random magical item (e.g., weapon)
                BaseWeapon weapon = new Longsword();
                weapon.Attributes.WeaponDamage = Utility.RandomMinMax(5, 10);
                cont.AddItem(weapon);
            }
            base.Complete();
        }

        public override void Cancel()
        {
            base.Cancel();
        }

        public override void BaseSerialize(GenericWriter writer)
        {
            base.BaseSerialize(writer);
            writer.Write((int)0); // version
        }

        public override void BaseDeserialize(GenericReader reader)
        {
            base.BaseDeserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class KillUndeadObjective : QuestObjective
    {
        private PurgeUndeadQuest m_Quest;
        private int m_ZombieKills;
        private int m_SkeletonKills;
        private int m_BogleKills;
        private int m_LichKills;
        private int m_WraithKills;

        public override object Message
        {
            get { return "Slay 5 each of Zombie, Skeleton, Bogle, Lich, and Wraith, then return to High Priestess Liora."; }
        }

        public KillUndeadObjective()
        {
            m_Quest = null;
            m_ZombieKills = 0;
            m_SkeletonKills = 0;
            m_BogleKills = 0;
            m_LichKills = 0;
            m_WraithKills = 0;
        }

        public KillUndeadObjective(PurgeUndeadQuest quest)
        {
            m_Quest = quest;
            System = quest;
            m_ZombieKills = 0;
            m_SkeletonKills = 0;
            m_BogleKills = 0;
            m_LichKills = 0;
            m_WraithKills = 0;
        }

        public override void BaseSerialize(GenericWriter writer)
        {
            base.BaseSerialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_ZombieKills);
            writer.Write(m_SkeletonKills);
            writer.Write(m_BogleKills);
            writer.Write(m_LichKills);
            writer.Write(m_WraithKills);
        }

        public override void BaseDeserialize(GenericReader reader)
        {
            base.BaseDeserialize(reader);
            int version = reader.ReadInt();
            m_ZombieKills = reader.ReadInt();
            m_SkeletonKills = reader.ReadInt();
            m_BogleKills = reader.ReadInt();
            m_LichKills = reader.ReadInt();
            m_WraithKills = reader.ReadInt();
            CheckCompletionStatus();
        }

        public override void OnKill(BaseCreature creature, Container corpse)
        {
            if (creature is Zombie)
                m_ZombieKills = Math.Min(m_ZombieKills + 1, 5);
            else if (creature is Skeleton)
                m_SkeletonKills = Math.Min(m_SkeletonKills + 1, 5);
            else if (creature is Bogle)
                m_BogleKills = Math.Min(m_BogleKills + 1, 5);
            else if (creature is Lich)
                m_LichKills = Math.Min(m_LichKills + 1, 5);
            else if (creature is Wraith)
                m_WraithKills = Math.Min(m_WraithKills + 1, 5);

            CheckCompletionStatus();
            if (System != null && System.From != null)
            {
                // Use capped values for display
                int displayZombieKills = Math.Min(m_ZombieKills, 5);
                int displaySkeletonKills = Math.Min(m_SkeletonKills, 5);
                int displayBogleKills = Math.Min(m_BogleKills, 5);
                int displayLichKills = Math.Min(m_LichKills, 5);
                int displayWraithKills = Math.Min(m_WraithKills, 5);

                System.From.SendMessage("You have slain a {0}. Progress: Zombie {1}/5, Skeleton {2}/5, Bogle {3}/5, Lich {4}/5, Wraith {5}/5",
                    creature.GetType().Name, displayZombieKills, displaySkeletonKills, displayBogleKills, displayLichKills, displayWraithKills);
            }
            Console.WriteLine("PurgeUndeadQuest: {0} killed {1}, Progress: Zombie {2}/5, Skeleton {3}/5, Bogle {4}/5, Lich {5}/5, Wraith {6}/5",
                System != null && System.From != null ? System.From.Name : "Unknown", creature.GetType().Name,
                Math.Min(m_ZombieKills, 5), Math.Min(m_SkeletonKills, 5), Math.Min(m_BogleKills, 5), Math.Min(m_LichKills, 5), Math.Min(m_WraithKills, 5));
        }

        public override void CheckProgress()
        {
            CheckCompletionStatus();
        }

        private void CheckCompletionStatus()
        {
            if (m_ZombieKills >= 5 && m_SkeletonKills >= 5 && m_BogleKills >= 5 && m_LichKills >= 5 && m_WraithKills >= 5)
            {
                Complete();
            }
        }

        public override void Complete()
        {
            base.Complete();
            if (m_Quest != null)
                m_Quest.Complete();
        }

        public override void RenderProgress(BaseQuestGump gump)
        {
            if (!Completed)
            {
                // Cap displayed counts at 5
                int displayZombieKills = Math.Min(m_ZombieKills, 5);
                int displaySkeletonKills = Math.Min(m_SkeletonKills, 5);
                int displayBogleKills = Math.Min(m_BogleKills, 5);
                int displayLichKills = Math.Min(m_LichKills, 5);
                int displayWraithKills = Math.Min(m_WraithKills, 5);

                // Display kill progress
                string progress = String.Format(
                    "Zombies: {0}/5\nSkeletons: {1}/5\nBogles: {2}/5\nLiches: {3}/5\nWraiths: {4}/5",
                    displayZombieKills, displaySkeletonKills, displayBogleKills, displayLichKills, displayWraithKills
                );
                gump.AddHtmlObject(70, 260, 270, 100, progress, BaseQuestGump.Blue, false, false);
            }
            else
            {
                gump.AddHtmlObject(70, 260, 270, 100, 1049077, BaseQuestGump.Blue, false, false); // Completed
            }
        }
    }

    public class PurgeUndeadConversation : QuestConversation
    {
        public override object Message
        {
            get { return "The undead are a blight upon our lands. Slay 5 each of Zombie, Skeleton, Bogle, Lich, and Wraith to restore peace. Return to me when your task is complete."; }
        }

        public PurgeUndeadConversation()
        {
        }

        public override void OnRead()
        {
            System.From.SendMessage("Hasten to slay the undead!");
        }
    }
}