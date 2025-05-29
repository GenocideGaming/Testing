using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class OutpostTraderunQuest : QuestSystem
    {
        private enum ResourceType
        {
            Fishsteak, Cotton, Logs, IronOre, Wheat
        }

        private ResourceType m_Resource;
        private Mobile m_StartTamer;
        private Mobile m_DestinationTamer;
        private OutpostPackhorse m_Packhorse;
        private DateTime m_LastOffered;

        public override object Name { get { return "Outpost Trade Run"; } }
        public override object OfferMessage { get { return "We need 500 units of a resource delivered to another outpost. Fill the commodity deed and deliver it to the local tamer."; } }
        public override int Picture { get { return 0x15C9; } }
        public override bool IsTutorial { get { return false; } }
        public override TimeSpan RestartDelay { get { return TimeSpan.FromHours(6); } }
        public override Type[] TypeReferenceTable { get { return new Type[] { typeof(OutpostTraderunQuest) }; } }

        public OutpostTraderunQuest(PlayerMobile owner, Mobile offerer) : base(owner)
        {
            m_Resource = (ResourceType)Utility.Random(5);
            m_StartTamer = GetTamerForProvisioner(offerer);
            m_DestinationTamer = GetDestinationTamer(offerer);
            m_LastOffered = DateTime.UtcNow;
        }

        public OutpostTraderunQuest()
        {
        }

        public bool CanOffer()
        {
            return DateTime.UtcNow >= m_LastOffered.AddHours(6);
        }

        public void OnAccept()
        {
            CommodityDeed deed = new CommodityDeed();
            Item commodity = Activator.CreateInstance(GetItemType(m_Resource)) as Item;
            commodity.Amount = 500;
            if (deed.SetCommodity(commodity))
            {
                deed.Hue = 0;
                deed.Movable = true;
                From.AddToBackpack(deed);
                From.SendMessage("Fill the commodity deed with 500 {0} and deliver it to {1}.", m_Resource.ToString(), m_StartTamer.Name);
                Accept();
            }
            else
            {
                commodity.Delete();
                From.SendMessage("Failed to create the commodity deed.");
            }
        }

        private Type GetItemType(ResourceType resource)
        {
            if (resource == ResourceType.Fishsteak) return typeof(FishSteak);
            if (resource == ResourceType.Cotton) return typeof(Cotton);
            if (resource == ResourceType.Logs) return typeof(Log);
            if (resource == ResourceType.IronOre) return typeof(IronOre);
            if (resource == ResourceType.Wheat) return typeof(WheatSheaf);
            return typeof(FishSteak);
        }

        private Mobile GetTamerForProvisioner(Mobile provisioner)
        {
            if (provisioner is TaliaTheProvisioner)
                return World.Mobiles.Values.FirstOrDefault(m => m is RorikTheAnimalTamer && m.Map == Map.Felucca);
            if (provisioner is GideonTheProvisioner)
                return World.Mobiles.Values.FirstOrDefault(m => m is LioraTheAnimalTamer && m.Map == Map.Felucca);
            if (provisioner is SeleneTheProvisioner)
                return World.Mobiles.Values.FirstOrDefault(m => m is VarnTheAnimalTamer && m.Map == Map.Felucca);
            return null;
        }

        private Mobile GetDestinationTamer(Mobile provisioner)
        {
            if (provisioner is GideonTheProvisioner)
            {
                return Utility.RandomBool()
                    ? World.Mobiles.Values.FirstOrDefault(m => m is RorikTheAnimalTamer && m.Map == Map.Felucca)
                    : World.Mobiles.Values.FirstOrDefault(m => m is VarnTheAnimalTamer && m.Map == Map.Felucca);
            }
            return World.Mobiles.Values.FirstOrDefault(m => m is LioraTheAnimalTamer && m.Map == Map.Felucca);
        }

        public bool ProcessCommodityDeed(Mobile tamer, CommodityDeed deed)
        {
            if (tamer == m_StartTamer && IsValidDeed(deed, m_Resource))
            {
                deed.Movable = false;
                CreatePackhorse();
                m_Packhorse.Backpack.AddItem(deed);
                From.SendMessage("Escort the packhorse to {0} at the destination outpost.", m_DestinationTamer.Name);
                return true;
            }
            return false;
        }

        public bool CheckComplete(Mobile tamer, CommodityDeed deed)
        {
            if (tamer == m_DestinationTamer && IsValidDeed(deed, m_Resource))
            {
                deed.Delete();
                CompleteQuest();
                return true;
            }
            return false;
        }

        private bool IsValidDeed(CommodityDeed deed, ResourceType resource)
        {
            return deed != null && deed.Commodity != null && deed.Commodity.GetType() == GetItemType(resource) && deed.Commodity.Amount >= 500;
        }

        private void CreatePackhorse()
        {
            m_Packhorse = new OutpostPackhorse(From, this)
            {
                Location = From.Location,
                Map = From.Map
            };
            m_Packhorse.SetControlMaster(From);
        }

        private void CompleteQuest()
        {
            From.AddToBackpack(new Gold(2000));
            Item reward;
            int choice = Utility.Random(3);
            if (choice == 0)
                reward = new Bandage(10);
            else if (choice == 1)
                reward = new SmithHammer(5);
            else
            {
                reward = new GoldBracelet();
            }
            From.AddToBackpack(reward);
            From.SendMessage("You have completed the quest and received your reward!");
            if (m_Packhorse != null)
                m_Packhorse.Delete();
            Complete();
        }

        public void OnPackhorseDeath(OutpostPackhorse horse)
        {
            From.SendMessage("The packhorse was slain! The quest has failed.");
            Cancel();
        }

        public override void BaseSerialize(GenericWriter writer)
        {
            base.BaseSerialize(writer);
            writer.Write((int)0); // version
            writer.Write((int)m_Resource);
            writer.Write(m_StartTamer);
            writer.Write(m_DestinationTamer);
            writer.Write(m_Packhorse);
            writer.Write(m_LastOffered);
        }

        public override void BaseDeserialize(GenericReader reader)
        {
            base.BaseDeserialize(reader);
            int version = reader.ReadInt();
            m_Resource = (ResourceType)reader.ReadInt();
            m_StartTamer = reader.ReadMobile();
            m_DestinationTamer = reader.ReadMobile();
            m_Packhorse = reader.ReadMobile() as OutpostPackhorse;
            m_LastOffered = reader.ReadDateTime();
        }
    }
}