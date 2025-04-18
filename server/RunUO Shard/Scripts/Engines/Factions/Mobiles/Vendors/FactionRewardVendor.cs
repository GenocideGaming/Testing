using System;
using System.Collections.Generic;
using System.Text;
using Server.Mobiles;
using Server.Items;
using Server.Regions;
using Server.Scripts.Custom.Citizenship;
using Server.Gumps;
using Server.ContextMenus;

namespace Server.Factions
{
    public class FactionRewardVendor : BaseCreature
    {

        private List<SBInfo> m_SBInfos = new List<SBInfo>();
		protected List<SBInfo> SBInfos{ get { return m_SBInfos; } }
 
        public void InitSBInfo()
        {
            MilitiaStronghold stronghold = this.Region as MilitiaStronghold;
        
            if (stronghold != null)
            {
                switch (stronghold.Owner.Definition.TownName.String)
                {
                    case "Trinsic":
                        SBInfos.Add(new SBPaladinFactionReward());
                        break;
                    case "Blackrock":
                        SBInfos.Add(new SBUrukhaiFactionReward());
                        break;
                    case "Yew":
                        SBInfos.Add(new SBForsakenFactionReward());
                        break;
                }
            }
        }
        [Constructable]
        public FactionRewardVendor() : base(AIType.AI_Vendor, FightMode.None, 15, 0, 1.0, 1.0)
        {
            InitBody();
            Timer.DelayCall(TimeSpan.FromSeconds(10.0), new TimerCallback(InitSBInfo));
            Title = "Faction Reward Merchant";

        }

        public FactionRewardVendor(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            InitSBInfo();
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            if (e.Speech.Contains("buy"))
            {
                MilitiaStronghold stronghold = Region as MilitiaStronghold;
                if (stronghold == null)
                    return;

                Faction militia = stronghold.Owner.Militia;
                Faction playersMilitia = Faction.Find(e.Mobile);

                if (playersMilitia != militia && e.Mobile.AccessLevel == AccessLevel.Player)
                {
                    SayTo(e.Mobile, "I will not deal with thee.");
                    return;
                }

                e.Handled = true;
                VendorBuy(e.Mobile);

            }
            base.OnSpeech(e);
        }
        public virtual void VendorBuy(Mobile from)
        {
            if (SBInfos.Count > 0)
            {
                from.CloseGump(typeof(BaseRewardGump));
                from.SendGump(new BaseRewardGump(SBInfos[0], 0, from, RewardCurrency.Silver, "Faction Reward Vendor"));
                SayTo(from, "Have a look at my wares.");
            }
        }
        public virtual void InitBody()
        {
            InitStats(100, 100, 25);

            SpeechHue = Utility.RandomDyedHue();
            Hue = Utility.RandomSkinHue();
            Direction = Direction.South;
            NameHue = 0x35;
            Body = 0x191;

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                //Name = NameList.RandomName("female");
                AddItem(new Doublet(Utility.RandomBlueHue()));
                AddItem(new FancyShirt());
                AddItem(new Skirt());
                AddItem(new Hood());
                AddItem(new HalfApron());
                AddItem(new LeatherGloves());
                AddItem(new Sandals(1));
                AddItem(new FullSpellbook());
                AddItem(new Lantern());

            }
            else
            {
                Body = 0x190;
                //Name = NameList.RandomName("male");
                AddItem(new Doublet(Utility.RandomBlueHue()));
                AddItem(new FancyShirt());
                AddItem(new Skirt());
                AddItem(new Hood());
                AddItem(new HalfApron());
                AddItem(new LeatherGloves());
                AddItem(new Sandals(1));
                AddItem(new FullSpellbook());
                AddItem(new Lantern());
            }
        }

        public override bool CanBeDamaged()
        {
            return false;
        }

        public override void AddCustomContextEntries(Mobile from, List<ContextMenus.ContextMenuEntry> list)
        {
            if (from.Alive)
            {
                list.Add(new FactionRewardVendorBuyEntry(from, this));
            }
            base.AddCustomContextEntries(from, list);
        }
    }
    public class FactionRewardVendorBuyEntry : ContextMenuEntry
    {
        private FactionRewardVendor mVendor;

        public FactionRewardVendorBuyEntry(Mobile from, FactionRewardVendor rewardVendor) : base(6103, 8)
        {
            mVendor = rewardVendor;
            Enabled = (Faction.Find(from) != null || from.AccessLevel >= AccessLevel.GameMaster) ? true : false;
        }

        public override void OnClick()
        {
            MilitiaStronghold stronghold = mVendor.Region as MilitiaStronghold;
            if (stronghold == null)
                return;

            Faction militia = stronghold.Owner.Militia;
            Faction playersMilitia = Faction.Find(Owner.From);

            if (playersMilitia != militia && Owner.From.AccessLevel < AccessLevel.GameMaster)
            {
               mVendor.SayTo(Owner.From, "I will not deal with thee.");
                return;
            }

            mVendor.VendorBuy(Owner.From);
        }
    }
    public class SBFactionReward : SBInfo
    {
        private List<GenericBuyInfo> mBuyInfo = new InternalBuyInfo();
        private IShopSellInfo mSellInfo = new InternalSellInfo();
        private Commonwealth mTownship;

        public SBFactionReward(Commonwealth township)
		{
            mTownship = township;
		}

		public override IShopSellInfo SellInfo { get { return mSellInfo; } }
		public override List<GenericBuyInfo> BuyInfo { get { return mBuyInfo; } }

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
               //Add(new GenericBuyInfo(typeof(FactionReward), 3, 20, 0x????, 0));
                
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {
            }
        }
    }
    public class SBPaladinFactionReward : SBInfo
    {
        private List<GenericBuyInfo> mBuyInfo = new InternalBuyInfo();
        private IShopSellInfo mSellInfo = new InternalSellInfo();
 
        public SBPaladinFactionReward()
        {
            
        }

        public override IShopSellInfo SellInfo { get { return mSellInfo; } }
        public override List<GenericBuyInfo> BuyInfo { get { return mBuyInfo; } }

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                //Add(new GenericBuyInfo(typeof(FactionReward), 3, 20, 0x????, 0));
                Add(new GenericBuyInfo("Paladin Dye Tub", typeof(PaladinOrderDyeTub), 50, 20, 0xFAB, 0));
                Add(new GenericBuyInfo("Paladin Alternate Dye Tub", typeof(LeatherDyeTubDullCopper), 200, 20, 0xFAB, 0));
                Add(new GenericBuyInfo("Paladin Order Armament Dye Tub", typeof(PaladinOrderArmamentDyeTub), 500, 20, 0xFAB, 0));
                Add(new GenericBuyInfo("Oxidizing Powder", typeof(OxidizingPowder), 100, 20, 0x0F8F, 0xD7));
                Add(new GenericBuyInfo("Paladin Order Cloth", typeof(PaladinOrderCloth), 500, 20, 0x1767, 0));
                Add(new GenericBuyInfo("Paladin Alternate Cloth", typeof(PaladinOrderCloth), 500, 20, 0x1767, 0));
                Add(new GenericBuyInfo("Paladin Order Hood", typeof(PaladinHood2), 500, 20, 0x3FFB, 0x4E6));
                Add(new GenericBuyInfo("Paladin Order Helm", typeof(PaladinHood2), 500, 20, 0x3FFB, 0x4E6));
                Add(new GenericBuyInfo("Paladin Order Banner", typeof(PaladinBanner), 2500, 20, 0x15ED, 0));
                Add(new GenericBuyInfo("Armor Display", typeof(ArmorSet1), 2000, 20, 0x4711, 0));
                Add(new GenericBuyInfo("Armor Display", typeof(ArmorSet2), 2000, 20, 0x4712, 0));
                Add(new GenericBuyInfo("Armor Display", typeof(ArmorSet3), 2000, 20, 0x4714, 0));
                Add(new GenericBuyInfo("Armor Display", typeof(ArmorSet4), 2000, 20, 0x4716, 0));
                Add(new GenericBuyInfo("Armor Display", typeof(ArmorSet5), 2000, 20, 0x4717, 0));
                Add(new GenericBuyInfo("Armor Display", typeof(ArmorSet6), 2000, 20, 0x4722, 0));
                Add(new GenericBuyInfo("Armor Display Case", typeof(ArmorDisplayCase1), 2000, 20, 0x4769, 0));
                Add(new GenericBuyInfo("Armor Display Case", typeof(ArmorDisplayCase2), 2000, 20, 0x476A, 0));
                Add(new GenericBuyInfo("Armor Display Case", typeof(ArmorDisplayCase3), 2000, 20, 0x476B, 0));
                Add(new GenericBuyInfo("Swords Wall Display", typeof(DecorativeSwordNorth), 2000, 20, 0x1565, 0));
                Add(new GenericBuyInfo("Axe Wall Display", typeof(DecorativeAxeNorth), 2000, 20, 0x1561, 0));
                Add(new GenericBuyInfo("Large Wall Display", typeof(DecorativeDAxeNorth), 2000, 20, 0x1569, 0));
                Add(new GenericBuyInfo("Bow Wall Display", typeof(DecorativeBowNorth), 2000, 20, 0x155C, 0));
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {
            }
        }
    }

    public class SBUrukhaiFactionReward : SBInfo
    {
        private List<GenericBuyInfo> mBuyInfo = new InternalBuyInfo();
        private IShopSellInfo mSellInfo = new InternalSellInfo();

        public SBUrukhaiFactionReward()
        {

        }

        public override IShopSellInfo SellInfo { get { return mSellInfo; } }
        public override List<GenericBuyInfo> BuyInfo { get { return mBuyInfo; } }

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                //Add(new GenericBuyInfo(typeof(FactionReward), 3, 20, 0x????, 0));
                Add(new GenericBuyInfo("Uruk-hai Dye Tub", typeof(UrukhaiDyeTub), 50, 20, 0xFAB, 0x5E3));
                Add(new GenericBuyInfo("Dull Copper Leather Dye Tub", typeof(LeatherDyeTubDullCopper), 200, 20, 0xFAB, 0));
                Add(new GenericBuyInfo("Bronze Dye Tub", typeof(LeatherDyeTubBronze), 200, 20, 0xFAB, 0));
                Add(new GenericBuyInfo("Golden Leather Dye Tub", typeof(LeatherDyeTubGolden), 200, 20, 0xFAB, 0));
                Add(new GenericBuyInfo("Copper Leather Dye Tub", typeof(LeatherDyeTubCopper), 200, 20, 0xFAB, 0));
                Add(new GenericBuyInfo("Uruk-hai Banner", typeof(UrukhaiBanner), 2500, 20, 0x0455, 0));
                Add(new GenericBuyInfo("Uruk-hai Militia Hood", typeof(UrukhaiHood2), 500, 20, 0x3FFC, 0x5E3));
                Add(new GenericBuyInfo("Uruk-hai Militia Cloth", typeof(UrukhaiCloth), 500, 20, 0x1767, 0x5E3));
                Add(new GenericBuyInfo("Uruk-hai Armament Dye Tub", typeof(UrukhaiArmamentDyeTub), 500, 20, 0xFAB, 0x5E3));
                Add(new GenericBuyInfo("Oxidizing Powder", typeof(OxidizingPowder), 100, 20, 0x0F8F, 0xD7));
                Add(new GenericBuyInfo("Armor Display", typeof(ArmorSet1), 2000, 20, 0x4711, 0));
                Add(new GenericBuyInfo("Armor Display", typeof(ArmorSet2), 2000, 20, 0x4712, 0));
                Add(new GenericBuyInfo("Armor Display", typeof(ArmorSet3), 2000, 20, 0x4714, 0));
                Add(new GenericBuyInfo("Armor Display", typeof(ArmorSet4), 2000, 20, 0x4716, 0));
                Add(new GenericBuyInfo("Armor Display", typeof(ArmorSet5), 2000, 20, 0x4717, 0));
                Add(new GenericBuyInfo("Armor Display", typeof(ArmorSet6), 2000, 20, 0x4722, 0));
                Add(new GenericBuyInfo("Armor Display Case", typeof(ArmorDisplayCase1), 2000, 20, 0x4769, 0));
                Add(new GenericBuyInfo("Armor Display Case", typeof(ArmorDisplayCase2), 2000, 20, 0x476A, 0));
                Add(new GenericBuyInfo("Armor Display Case", typeof(ArmorDisplayCase3), 2000, 20, 0x476B, 0));
                Add(new GenericBuyInfo("Swords Wall Display", typeof(DecorativeSwordNorth), 2000, 20, 0x1565, 0));
                Add(new GenericBuyInfo("Axe Wall Display", typeof(DecorativeAxeNorth), 2000, 20, 0x1561, 0));
                Add(new GenericBuyInfo("Large Wall Display", typeof(DecorativeDAxeNorth), 2000, 20, 0x1569, 0));
                Add(new GenericBuyInfo("Bow Wall Display", typeof(DecorativeBowNorth), 2000, 20, 0x155C, 0));
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {
            }
        }
    }

    public class SBForsakenFactionReward : SBInfo
    {
        private List<GenericBuyInfo> mBuyInfo = new InternalBuyInfo();
        private IShopSellInfo mSellInfo = new InternalSellInfo();

        public SBForsakenFactionReward()
        {

        }

        public override IShopSellInfo SellInfo { get { return mSellInfo; } }
        public override List<GenericBuyInfo> BuyInfo { get { return mBuyInfo; } }

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                //Add(new GenericBuyInfo(typeof(FactionReward), 3, 20, 0x????, 0));
                Add(new GenericBuyInfo("Forsaken Dye Tub", typeof(ForsakenDyeTub), 50, 20, 0xFAB, 0));
                Add(new GenericBuyInfo("Forsaken Alternate Dye Tub", typeof(LeatherDyeTubDullCopper), 200, 20, 0xFAB, 0));
                Add(new GenericBuyInfo("Bronze Dye Tub", typeof(LeatherDyeTubBronze), 200, 20, 0xFAB, 0));
                Add(new GenericBuyInfo("Golden Leather Dye Tub", typeof(LeatherDyeTubGolden), 200, 20, 0xFAB, 0));
                Add(new GenericBuyInfo("Copper Leather Dye Tub", typeof(LeatherDyeTubCopper), 200, 20, 0xFAB, 0));
                Add(new GenericBuyInfo("Forsaken Banner", typeof(ForsakenBanner), 2500, 20, 0x42CB, 0));  //0x15BD
                Add(new GenericBuyInfo("Forsaken Militia Hood", typeof(ForsakenHood2), 500, 20, 0x3FFC, 0x4B9));
                Add(new GenericBuyInfo("Forsaken Militia Cloth", typeof(ForsakenCloth), 500, 20, 0x1767, 0));
                Add(new GenericBuyInfo("Forsaken Armament Dye Tub", typeof(ForsakenArmamentDyeTub), 500, 20, 0xFAB, 0));
                Add(new GenericBuyInfo("Oxidizing Powder", typeof(OxidizingPowder), 100, 20, 0x0F8F, 0xD7));
                Add(new GenericBuyInfo("Armor Display", typeof(ArmorSet1), 2000, 20, 0x4711, 0));
                Add(new GenericBuyInfo("Armor Display", typeof(ArmorSet2), 2000, 20, 0x4712, 0));
                Add(new GenericBuyInfo("Armor Display", typeof(ArmorSet3), 2000, 20, 0x4714, 0));
                Add(new GenericBuyInfo("Armor Display", typeof(ArmorSet4), 2000, 20, 0x4716, 0));
                Add(new GenericBuyInfo("Armor Display", typeof(ArmorSet5), 2000, 20, 0x4717, 0));
                Add(new GenericBuyInfo("Armor Display", typeof(ArmorSet6), 2000, 20, 0x4722, 0));
                Add(new GenericBuyInfo("Armor Display Case", typeof(ArmorDisplayCase1), 2000, 20, 0x4769, 0));
                Add(new GenericBuyInfo("Armor Display Case", typeof(ArmorDisplayCase2), 2000, 20, 0x476A, 0));
                Add(new GenericBuyInfo("Armor Display Case", typeof(ArmorDisplayCase3), 2000, 20, 0x476B, 0));
                Add(new GenericBuyInfo("Swords Wall Display", typeof(DecorativeSwordNorth), 2000, 20, 0x1565, 0));
                Add(new GenericBuyInfo("Axe Wall Display", typeof(DecorativeAxeNorth), 2000, 20, 0x1561, 0));
                Add(new GenericBuyInfo("Large Wall Display", typeof(DecorativeDAxeNorth), 2000, 20, 0x1569, 0));
                Add(new GenericBuyInfo("Bow Wall Display", typeof(DecorativeBowNorth), 2000, 20, 0x155C, 0));
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {
            }
        }
    }

}
