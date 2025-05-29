using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Mobiles
{
    public class MasterBlacksmithTorren : BaseQuester
    {
        [Constructable]
        public MasterBlacksmithTorren() : base()
        {
            SetStr(90);
            SetDex(70);
            SetInt(50);
            Body = 0x190; // Male
            Name = "Master Blacksmith Torren";
            Blessed = true;
            Hue = Utility.RandomSkinHue();
            HairItemID = 8253; // Long hair
            Direction = Direction.South;
            AddItem(new Shirt());
            AddItem(new LongPants());
            AddItem(new LeatherGloves());
            AddItem(new SmithHammer());
            AddItem(new FullApron());
            AddItem(new Boots());
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

        public override void OnTalk(PlayerMobile player, bool contextMenu)
        {
            QuestSystem qs = player.Quest;

            if (qs == null && QuestSystem.CanOfferQuest(player, typeof(SupplyRecoveryQuest)))
            {
                new SupplyRecoveryQuest(player).SendOffer();
            }
            else if (qs is SupplyRecoveryQuest)
            {
                SupplyRecoveryQuest supplyQuest = qs as SupplyRecoveryQuest;

                // Force check progress to update objective status
                QuestObjective obj = supplyQuest.FindObjective(typeof(CollectSupplyCratesObjective));
                CollectSupplyCratesObjective objective = obj as CollectSupplyCratesObjective;
                if (objective != null)
                {
                    objective.CheckProgress();
                    Console.WriteLine("SupplyRecoveryQuest: Checked progress for {0}, CurProgress = {1}, Completed = {2}", player.Name, objective.CurProgress, objective.Completed);
                }
                else
                {
                    Console.WriteLine("SupplyRecoveryQuest: Objective not found for {0}", player.Name);
                }

                // Check crate count and objective status
                CollectSupplyCratesObjective collectObjective = obj as CollectSupplyCratesObjective;
                if (collectObjective != null)
                {
                    int crateCount = 0;
                    List<LostSupplyCrate> cratesToConsume = new List<LostSupplyCrate>();
                    foreach (Item item in player.Backpack.Items)
                    {
                        if (item is LostSupplyCrate)
                        {
                            LostSupplyCrate crate = item as LostSupplyCrate;
                            crateCount += crate.Amount;
                            cratesToConsume.Add(crate);
                            Console.WriteLine("SupplyRecoveryQuest: Found crate for {0}, Amount = {1}", player.Name, crate.Amount);
                        }
                    }

                    Console.WriteLine("SupplyRecoveryQuest: {0} has {1} crates, Objective Completed = {2}", player.Name, crateCount, collectObjective.Completed);

                    if (collectObjective.Completed && crateCount >= 5)
                    {
                        int toRemove = 5;
                        foreach (LostSupplyCrate crate in cratesToConsume)
                        {
                            if (toRemove <= 0)
                                break;

                            int amountToRemove = Math.Min(toRemove, crate.Amount);
                            crate.Consume(amountToRemove);
                            toRemove -= amountToRemove;
                        }

                        SayTo(player, "Excellent work! You've brought me the supply crates we needed.");
                        supplyQuest.Complete();
                    }
                    else
                    {
                        SayTo(player, "You need to collect 5 lost supply crates to complete the quest.");
                        if (!contextMenu)
                            qs.ShowQuestLog();
                    }
                }
                else
                {
                    SayTo(player, "You need to collect 5 lost supply crates. Keep searching Britain!");
                    if (!contextMenu)
                        qs.ShowQuestLog();
                }
            }
            else
            {
                SayTo(player, "Return when you're ready to recover more supplies.");
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

        public MasterBlacksmithTorren(Serial serial) : base(serial)
        {
        }
    }
}