using System;
using Server;
using Server.Mobiles;
using Server.Engines.Quests;
using Server.Items;

namespace Server.Engines.Quests
{
    public class SupplyRecoveryQuest : QuestSystem
    {
        private static readonly Type[] m_TypeReferenceTable = new Type[]
        {
            typeof(CollectSupplyCratesObjective),
            typeof(SupplyRecoveryConversation)
        };

        public override Type[] TypeReferenceTable
        {
            get { return m_TypeReferenceTable; }
        }

        public override object Name
        {
            get { return "Recover the Lost Supplies"; }
        }

        public override object OfferMessage
        {
            get { return "Bandits and zombies have scattered our supply crates across Britain. Recover 5 lost supply crates and bring them to me. Beware, they may be guarded!"; }
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

        public SupplyRecoveryQuest(PlayerMobile from) : base(from)
        {
        }

        public SupplyRecoveryQuest()
        {
        }

        public override void Accept()
        {
            base.Accept();
            From.SendMessage("Search Britain for 5 lost supply crates and return them to Master Blacksmith Torren.");
            AddObjective(new CollectSupplyCratesObjective(this));
        }

        public override void Complete()
        {
            From.SendMessage("You’ve recovered the lost supply crates!");
            Container cont = From.Backpack ?? From.BankBox;
            if (cont != null)
            {
                cont.AddItem(new Gold(1000));
                cont.AddItem(new Bandage(10));
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

    public class CollectSupplyCratesObjective : QuestObjective
    {
        private SupplyRecoveryQuest m_Quest;

        public override object Message
        {
            get { return "Collect 5 lost supply crates from the ruins of Britain and return them to Master Blacksmith Torren."; }
        }

        public CollectSupplyCratesObjective()
        {
            m_Quest = null;
        }

        public CollectSupplyCratesObjective(SupplyRecoveryQuest quest)
        {
            m_Quest = quest;
            System = quest;
        }

        public override void BaseDeserialize(GenericReader reader)
        {
            base.BaseDeserialize(reader);
            int version = reader.ReadInt();
        }

        public override void BaseSerialize(GenericWriter writer)
        {
            base.BaseSerialize(writer);
            writer.Write((int)0); // version
        }

        public override void CheckProgress()
        {
            if (System == null || System.From == null)
                return;

            PlayerMobile pm = System.From as PlayerMobile;
            if (pm == null)
                return;

            int crateCount = 0;
            foreach (Item item in pm.Backpack.Items)
            {
                if (item is LostSupplyCrate)
                    crateCount += item.Amount;
            }

            if (CurProgress != crateCount)
            {
                CurProgress = crateCount;
                System.ShowQuestLogUpdated(); // Refresh gump
            }

            if (CurProgress >= 5)
                Complete();
        }

        public override void Complete()
        {
            base.Complete();
            if (m_Quest != null)
                m_Quest.Complete();
        }

        public override void RenderProgress(BaseQuestGump gump)
        {
            if (CurProgress < 5)
            {
                int displayCrateCount = Math.Min(CurProgress, 5);
                string progress = String.Format("Crates: {0}/5", displayCrateCount);
                gump.AddHtmlObject(70, 260, 270, 100, progress, BaseQuestGump.Blue, false, false);
            }
            else
            {
                gump.AddHtmlObject(70, 260, 270, 100, 1049077, BaseQuestGump.Blue, false, false); // Completed
            }
        }
    }

    public class SupplyRecoveryConversation : QuestConversation
    {
        public override object Message
        {
            get { return "Those supply crates are vital for our survival. Find 5 of them in Britain’s ruins and bring them back. Watch out for ambushes!"; }
        }

        public SupplyRecoveryConversation()
        {
        }

        public override void OnRead()
        {
            System.From.SendMessage("Hurry and find those supply crates!");
        }
    }
}