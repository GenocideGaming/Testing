using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Engines.Quests;

namespace Server.Mobiles
{
    public class GloktaTheScavenger : BaseQuester
    {
        [Constructable]
        public GloktaTheScavenger() : base()
        {
            SetStr(80);
            SetDex(60);
            SetInt(50);
            Body = 0x190; // Male
            Name = "Glokta the Scavenger";
            Blessed = true;
            Hue = Utility.RandomSkinHue();
            Direction = Direction.South;
            AddItem(new Robe(1109)); // Robe
            //AddItem(new LeatherBelt());
            AddItem(Rehued(new LeatherGloves(), 1109));
            AddItem(new ScavengerHood(1109)); // SkullCap
            AddItem(new Boots(1109)); // Boots
            AddItem(new BlackStaff()); // Black staff
            SpeechHue = 1153;
        }

        public override void InitBody()
        {
            // Set in constructor
        }

        public override void InitOutfit()
        {
            // Set in constructor
        }
        public Item Rehued(Item item, int hue)
        {
            item.Hue = hue;
            return item;
        }

        public override void OnTalk(PlayerMobile player, bool contextMenu)
        {
            QuestSystem qs = player.Quest;

            if (qs == null && QuestSystem.CanOfferQuest(player, typeof(ScavengersHaulQuest)))
            {
                ScavengersHaulQuest quest = new ScavengersHaulQuest(player);
                quest.SendOffer();
            }
            else if (qs != null && qs.GetType() == typeof(ScavengersHaulQuest))
            {
                ScavengersHaulQuest scavQuest = (ScavengersHaulQuest)qs;
                if (contextMenu)
                {
                    SayTo(player, "Got all them items yet? Shamus and Ephram are countin’ on us!");
                }
                else
                {
                    object obj = scavQuest.FindObjective(typeof(CollectScavengerItemsObjective));
                    if (obj != null)
                    {
                        CollectScavengerItemsObjective objective = (CollectScavengerItemsObjective)obj;
                        if (objective.Completed)
                        {
                            bool hasAllItems = CheckRequiredItems(player);
                            if (hasAllItems)
                            {
                                ConsumeRequiredItems(player);
                                SayTo(player, "Well done! These’ll keep us goin’ down here. Take this for your trouble.");
                                scavQuest.Complete();
                            }
                            else
                            {
                                SayTo(player, "You ain’t got all the items yet. Need a rope, lockpick, bottle, bandage, torch, shovel, sewing kit, tinker’s tools, 20 arrows, and 10 raw fish steaks!");
                            }
                        }
                        else
                        {
                            scavQuest.ShowQuestLog();
                        }
                    }
                    else
                    {
                        scavQuest.ShowQuestLog();
                    }
                }
            }
            else
            {
                SayTo(player, "Come back when you’re ready to scavenge again, eh?");
            }
        }

        private bool CheckRequiredItems(PlayerMobile player)
        {
            Container backpack = player.Backpack;
            if (backpack == null)
                return false;

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

            foreach (Item item in backpack.Items)
            {
                switch (item.GetType().Name)
                {
                    case "Rope":
                        hasRope = true;
                        break;
                    case "Lockpick":
                        hasLockpick = true;
                        break;
                    case "Bottle":
                        hasBottle = true;
                        break;
                    case "Bandage":
                        hasBandage = true;
                        break;
                    case "Torch":
                        hasTorch = true;
                        break;
                    case "Shovel":
                        hasShovel = true;
                        break;
                    case "SewingKit":
                        hasSewingKit = true;
                        break;
                    case "TinkerTools":
                        hasTinkerTools = true;
                        break;
                    case "Arrow":
                        arrowCount += item.Amount;
                        break;
                    case "RawFishSteak":
                        fishSteakCount += item.Amount;
                        break;
                }
            }

            return hasRope && hasLockpick && hasBottle && hasBandage && hasTorch &&
                   hasShovel && hasSewingKit && hasTinkerTools &&
                   arrowCount >= 20 && fishSteakCount >= 10;
        }

        private void ConsumeRequiredItems(PlayerMobile player)
        {
            Container backpack = player.Backpack;
            if (backpack == null)
                return;

            bool consumedRope = false;
            bool consumedLockpick = false;
            bool consumedBottle = false;
            bool consumedBandage = false;
            bool consumedTorch = false;
            bool consumedShovel = false;
            bool consumedSewingKit = false;
            bool consumedTinkerTools = false;
            int arrowsToConsume = 20;
            int fishSteaksToConsume = 10;

            Item[] items = backpack.Items.ToArray();
            for (int i = 0; i < items.Length; i++)
            {
                Item item = items[i];
                switch (item.GetType().Name)
                {
                    case "Rope":
                        if (!consumedRope)
                        {
                            item.Delete();
                            consumedRope = true;
                        }
                        break;
                    case "Lockpick":
                        if (!consumedLockpick)
                        {
                            item.Delete();
                            consumedLockpick = true;
                        }
                        break;
                    case "Bottle":
                        if (!consumedBottle)
                        {
                            item.Delete();
                            consumedBottle = true;
                        }
                        break;
                    case "Bandage":
                        if (!consumedBandage)
                        {
                            item.Delete();
                            consumedBandage = true;
                        }
                        break;
                    case "Torch":
                        if (!consumedTorch)
                        {
                            item.Delete();
                            consumedTorch = true;
                        }
                        break;
                    case "Shovel":
                        if (!consumedShovel)
                        {
                            item.Delete();
                            consumedShovel = true;
                        }
                        break;
                    case "SewingKit":
                        if (!consumedSewingKit)
                        {
                            item.Delete();
                            consumedSewingKit = true;
                        }
                        break;
                    case "TinkerTools":
                        if (!consumedTinkerTools)
                        {
                            item.Delete();
                            consumedTinkerTools = true;
                        }
                        break;
                    case "Arrow":
                        if (arrowsToConsume > 0)
                        {
                            int amount = Math.Min(arrowsToConsume, item.Amount);
                            item.Consume(amount);
                            arrowsToConsume -= amount;
                        }
                        break;
                    case "RawFishSteak":
                        if (fishSteaksToConsume > 0)
                        {
                            int amount = Math.Min(fishSteaksToConsume, item.Amount);
                            item.Consume(amount);
                            fishSteaksToConsume -= amount;
                        }
                        break;
                }
            }
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

        public GloktaTheScavenger(Serial serial) : base(serial)
        {
        }
    }
}