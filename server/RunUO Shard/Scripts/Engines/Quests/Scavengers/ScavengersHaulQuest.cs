using System;
using Server;
using Server.Mobiles;
using Server.Engines.Quests;
using Server.Items;

namespace Server.Engines.Quests
{
    public class ScavengersHaulQuest : QuestSystem
    {
        private static readonly Type[] m_TypeReferenceTable = new Type[]
        {
            typeof(CollectScavengerItemsObjective),
            typeof(ScavengerConversation)
        };

        public override Type[] TypeReferenceTable
        {
            get { return m_TypeReferenceTable; }
        }

        public override object Name
        {
            get { return "Scavenger’s Haul"; }
        }

        public override object OfferMessage
        {
            get { return "The Blackmarket’s runnin’ dry, and we need supplies to survive this mess. Scour Britain for a rope, lockpick, bottle, bandage, torch, shovel, sewing kit, tinker’s tools, 20 arrows, and 10 raw fish steaks. Bring ‘em to me, and I’ll make it worth your while."; }
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

        public ScavengersHaulQuest(PlayerMobile from) : base(from)
        {
        }

        public ScavengersHaulQuest()
        {
        }

        public override void Accept()
        {
            base.Accept();
            From.SendMessage("Search Britain for a rope, lockpick, bottle, bandage, torch, shovel, sewing kit, tinker’s tools, 20 arrows, and 10 raw fish steaks. Return to Glokta in the Sewer Blackmarket.");
        }

        public override void Complete()
        {
            From.SendMessage("You’ve completed the scavenger hunt! Here’s your reward.");
            Container cont = From.Backpack;
            if (cont == null)
                cont = From.BankBox;
            if (cont != null)
            {
                cont.TryDropItem(From, new Gold(2000), false);
                cont.TryDropItem(From, new Bandage(20), false);
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

    public class CollectScavengerItemsObjective : QuestObjective
    {
        public override object Message
        {
            get { return "Collect a rope, lockpick, bottle, bandage, torch, shovel, sewing kit, tinker’s tools, 20 arrows, and 10 raw fish steaks. Return to Glokta the Scavenger."; }
        }

        public CollectScavengerItemsObjective(ScavengersHaulQuest quest)
        {
            System = quest;
        }

        public override void CheckProgress()
        {
            PlayerMobile pm = System.From as PlayerMobile;
            if (pm == null || pm.Backpack == null)
                return;

            bool hasRope = false;
            bool hasLockpick = false;
            bool hasBottle = false;
            bool hasBandage = false;
            bool hasTorch = false;
            bool hasShovel = false;
            bool hasSewingKit = false;
            bool hasTinkerTools = false;
            int arrowCount = 0;
            int fishSteakCount = 0;

            foreach (Item item in pm.Backpack.Items)
            {
                if (item is Rope)
                    hasRope = true;
                else if (item is Lockpick)
                    hasLockpick = true;
                else if (item is Bottle)
                    hasBottle = true;
                else if (item is Bandage)
                    hasBandage = true;
                else if (item is Torch)
                    hasTorch = true;
                else if (item is Shovel)
                    hasShovel = true;
                else if (item is SewingKit)
                    hasSewingKit = true;
                else if (item is TinkerTools)
                    hasTinkerTools = true;
                else if (item is Arrow)
                    arrowCount += item.Amount;
                else if (item is RawFishSteak)
                    fishSteakCount += item.Amount;
            }

            if (hasRope && hasLockpick && hasBottle && hasBandage && hasTorch &&
                hasShovel && hasSewingKit && hasTinkerTools &&
                arrowCount >= 20 && fishSteakCount >= 10)
            {
                Complete();
            }
        }

        public override void Complete()
        {
            base.Complete();
        }
    }

    public class ScavengerConversation : QuestConversation
    {
        public override object Message
        {
            get { return "Hurry up and find those items! We need that rope, lockpick, bottle, bandage, torch, shovel, sewing kit, tinker’s tools, arrows, and fish steaks to keep the Blackmarket runnin’!"; }
        }

        public ScavengerConversation()
        {
        }

        public override void OnRead()
        {
            System.From.SendMessage("Get scavengin’ for Glokta!");
        }
    }
}