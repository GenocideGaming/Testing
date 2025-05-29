using System;
using Server;
using Server.Mobiles;
using Server.Engines.Quests;
using Server.Items;

namespace Server.Engines.Quests
{
    public class SurvivorRescueQuest : QuestSystem
    {
        private static readonly Type[] m_TypeReferenceTable = new Type[]
        {
            typeof(RescueSurvivorObjective),
            typeof(SurvivorRescueConversation)
        };

        public override Type[] TypeReferenceTable
        {
            get { return m_TypeReferenceTable; }
        }

        public override object Name
        {
            get { return "Rescue from the Ruins"; }
        }

        public override object OfferMessage
        {
            get { return "Survivors are trapped in the ruins of Britain, surrounded by zombies. Find one and escort them to the Britain Dockhouse."; }
        }

        public override TimeSpan RestartDelay
        {
            get { return TimeSpan.FromMinutes(30); }
        }

        public override bool IsTutorial
        {
            get { return false; }
        }

        public override int Picture
        {
            get { return 0x15C9; } // Quest scroll icon
        }

        public SurvivorRescueQuest(PlayerMobile from) : base(from)
        {
        }

        public SurvivorRescueQuest()
        {
        }

        public override void Accept()
        {
            base.Accept();
            // No survivor assigned; player must find one
            From.SendMessage("Search the ruins of Britain for a survivor to escort to the Britain Dockhouse.");
            AddObjective(new RescueSurvivorObjective(this));
        }

        public override void Complete()
        {
            From.SendMessage("You’ve completed the survivor rescue quest!");
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

    public class RescueSurvivorObjective : QuestObjective
    {
        public override object Message
        {
            get { return "Find a survivor in the ruins of Britain and escort them to the Britain Dockhouse."; }
        }

        public RescueSurvivorObjective(SurvivorRescueQuest quest)
        {
            System = quest;
        }

        public override void CheckProgress()
        {
            PlayerMobile pm = System.From as PlayerMobile;
            if (pm == null)
                return;

            // Check for any SurvivorEscortable being escorted by the player
            foreach (Mobile m in pm.GetMobilesInRange(30))
            {
                SurvivorEscortable survivor = m as SurvivorEscortable;
                if (survivor != null && survivor.GetEscorter() == pm)
                {
                    if (survivor.CheckAtDestination())
                    {
                        Complete();
                    }
                    break;
                }
            }
        }

        public override void Complete()
        {
            base.Complete();
            System.Complete();
        }
    }

    public class SurvivorRescueConversation : QuestConversation
    {
        public override object Message
        {
            get { return "Hurry! Survivors are hiding in the ruins of Britain. Find one and get them to the Britain Dockhouse safely."; }
        }

        public SurvivorRescueConversation()
        {
        }

        public override void OnRead()
        {
            System.From.SendMessage("Find a survivor quickly!");
        }
    }
}