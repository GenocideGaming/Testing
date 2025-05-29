using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Engines.Quests;
using System.Linq;

namespace Server.Mobiles
{
    public class HighPriestessLiora : BaseQuester
    {
        [Constructable]
        public HighPriestessLiora() : base()
        {
            SetStr(60);
            SetDex(50);
            SetInt(80);
            Body = 0x191; // Female
            Name = "High Priestess Liora";
            Blessed = true;
            Hue = Utility.RandomSkinHue();
            HairItemID = 8252; // Short hair
            Direction = Direction.South;

            int[] clothingHues = new int[]
            {
                1102, 1107, 1112, 1122, 1132, 1142, 1150, 1160
            };

            Robe robe = new Robe();
            robe.Hue = clothingHues[Utility.Random(clothingHues.Length)];
            AddItem(robe);

            BodySash sash = new BodySash();
            sash.Hue = clothingHues[Utility.Random(clothingHues.Length)];
            AddItem(sash);

            Sandals sandals = new Sandals();
            sandals.Hue = clothingHues[Utility.Random(clothingHues.Length)];
            AddItem(sandals);

            AddItem(new QuarterStaff());
            SpeechHue = 1153;
        }

        public override void InitBody()
        {
        }

        public override void InitOutfit()
        {
        }

        public override void OnTalk(PlayerMobile player, bool contextMenu)
        {
            QuestSystem qs = player.Quest;

            if (qs == null && QuestSystem.CanOfferQuest(player, typeof(PurgeUndeadQuest)))
            {
                new PurgeUndeadQuest(player).SendOffer();
            }
            else if (qs is PurgeUndeadQuest)
            {
                PurgeUndeadQuest purgeQuest = qs as PurgeUndeadQuest;

                // Force check progress to update objective status
                QuestObjective obj = purgeQuest.FindObjective(typeof(KillUndeadObjective));
                KillUndeadObjective objective = obj as KillUndeadObjective;
                if (objective != null)
                {
                    objective.CheckProgress();
                    Console.WriteLine("PurgeUndeadQuest: Checked progress for {0}, Completed = {1}", player.Name, objective.Completed);
                }
                else
                {
                    Console.WriteLine("PurgeUndeadQuest: Objective not found for {0}", player.Name);
                }

                // Check objective status
                KillUndeadObjective killObjective = obj as KillUndeadObjective;
                if (killObjective != null && killObjective.Completed)
                {
                    SayTo(player, "Blessed be! You have purged the undead menace. Accept these rewards for your valor.");
                    purgeQuest.Complete();
                }
                else
                {
                    SayTo(player, "The undead still haunt our lands. Slay 5 each of Zombie, Skeleton, Bogle, Lich, and Wraith.");
                    if (!contextMenu)
                        qs.ShowQuestLog();
                }
            }
            else
            {
                SayTo(player, "Return when you're ready to purge the undead.");
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

        public HighPriestessLiora(Serial serial) : base(serial)
        {
        }
    }
}