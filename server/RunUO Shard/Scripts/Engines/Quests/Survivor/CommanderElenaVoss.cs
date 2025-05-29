using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Mobiles
{
    public class CommanderElenaVoss : BaseQuester
    {
        [Constructable]
        public CommanderElenaVoss() : base()
        {
            SetStr(100);
            SetDex(80);
            SetInt(60);
            Body = 0x191; // Female
            Name = "Commander Elena Voss";
            Blessed = true;
            Hue = Utility.RandomSkinHue();
            HairItemID = 8252; // Short hair
            Direction = Direction.South;
            AddItem(new ChainChest()); 
            AddItem(new BodySash(1209)); 
            AddItem(new Skirt(1209));
            AddItem(new LeatherGloves()); 
            AddItem(new PlateArms()); 
            AddItem(new PlateLegs()); 
            AddItem(new Torch(true)); // Lit torch
            AddItem(new Broadsword()); 
            AddItem(new Bandana(1209)); 
            AddItem(new FurCape(1209)); 
            SpeechHue = 1150;
        }

        public override void InitBody()
        {
            // Already set in constructor
        }

        public override void InitOutfit()
        {
            // Already set in constructor
        }

        public override void OnTalk(PlayerMobile player, bool contextMenu)
        {
            QuestSystem qs = player.Quest;

            if (qs == null && QuestSystem.CanOfferQuest(player, typeof(SurvivorRescueQuest)))
            {
                new SurvivorRescueQuest(player).SendOffer();
            }
            else if (qs is SurvivorRescueQuest)
            {
                if (contextMenu)
                    SayTo(player, "Have you found a survivor yet? They're in danger!");
                else
                    qs.ShowQuestLog();
            }
            else
            {
                SayTo(player, "Come back when you're ready to save more survivors.");
            }
        }
        public Item Rehued(Item item, int hue)
        {
            item.Hue = hue;
            return item;
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

        public CommanderElenaVoss(Serial serial) : base(serial)
        {
        }
    }
}