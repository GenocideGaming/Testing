using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.Regions;
using Server.Scripts.Custom.Citizenship;
using Server.Gumps;
using Server.ContextMenus;
using Server.Items;

namespace Server.Mobiles
{
    public class TownshipRewardVendor : BaseCreature
    {

        private List<SBInfo> m_SBInfos = new List<SBInfo>();
        protected List<SBInfo> SBInfos { get { return m_SBInfos; } }

        public void InitSBInfo()
        {
            CommonwealthRegion townRegion = this.Region as CommonwealthRegion;

            if (townRegion != null)
            {
                switch (townRegion.MyCommonwealth.Definition.TownName.String)
                {
                    case "Trinsic":
                        SBInfos.Add(new SBPaladinTownshipReward());
                        break;
                    case "Blackrock":
                        SBInfos.Add(new SBUrukhaiTownshipReward());
                        break;
                    case "Yew":
                        SBInfos.Add(new SBForsakenTownshipReward());
                        break;
                }
            }
        }
        [Constructable]
        public TownshipRewardVendor()
            : base(AIType.AI_Vendor, FightMode.None, 15, 0, 1.0, 1.0)
        {
            InitBody();
            Timer.DelayCall(TimeSpan.FromSeconds(10.0), new TimerCallback(InitSBInfo));
            Title = "the Township Reward Merchant";

        }

        public TownshipRewardVendor(Serial serial) : base(serial) { }

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
                CommonwealthRegion townRegion = Region as CommonwealthRegion;
                if (townRegion == null)
                    return;

                ICommonwealth township = townRegion.MyCommonwealth;
                ICommonwealth playersTownship = Commonwealth.Find(e.Mobile);

                if (playersTownship != township && e.Mobile.AccessLevel == AccessLevel.Player)
                {
                    SayTo(e.Mobile, "I will not deal with thee.");
                    return;
                }

                VendorBuy(e.Mobile);

                e.Handled = true;
            }
            base.OnSpeech(e);
        }
        public virtual void VendorBuy(Mobile from)
        {
            if (SBInfos.Count > 0)
            {
                from.CloseGump(typeof(BaseRewardGump));
                from.SendGump(new BaseRewardGump(SBInfos[0], 0, from, RewardCurrency.Gold, "Township Reward Vendor"));
                SayTo(from, "Have a look at my wares.");
            }
        }
        public virtual void InitBody()
        {
            InitStats(100, 100, 25);

            SpeechHue = Utility.RandomDyedHue();
            Hue = Utility.RandomSkinHue();

            NameHue = 0x35;

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
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
                list.Add(new TownshipRewardVendorBuyEntry(from, this));
            }
            base.AddCustomContextEntries(from, list);
        }
    }
    public class TownshipRewardVendorBuyEntry : ContextMenuEntry
    {
        private TownshipRewardVendor mVendor;

        public TownshipRewardVendorBuyEntry(Mobile from, TownshipRewardVendor rewardVendor)
            : base(6103, 8)
        {
            mVendor = rewardVendor;
            Enabled = (Commonwealth.Find(from) != null || from.AccessLevel >= AccessLevel.GameMaster) ? true : false;
        }

        public override void OnClick()
        {
            CommonwealthRegion townRegion = mVendor.Region as CommonwealthRegion;
            if (townRegion == null)
                return;

            ICommonwealth township = townRegion.MyCommonwealth;
            ICommonwealth playersTownship = Commonwealth.Find(Owner.From);

            if (playersTownship != township && Owner.From.AccessLevel < AccessLevel.GameMaster)
            {
                mVendor.SayTo(Owner.From, "I will not deal with thee.");
                return;
            }

            mVendor.VendorBuy(Owner.From);
        }
    }
    public class SBTownshipReward : SBInfo
    {
        private List<GenericBuyInfo> mBuyInfo = new InternalBuyInfo();
        private IShopSellInfo mSellInfo = new InternalSellInfo();
        private Commonwealth mTownship;

        public SBTownshipReward(Commonwealth township)
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
    public class SBPaladinTownshipReward : SBInfo
    {
        private List<GenericBuyInfo> mBuyInfo = new InternalBuyInfo();
        private IShopSellInfo mSellInfo = new InternalSellInfo();

        public SBPaladinTownshipReward()
        {

        }

        public override IShopSellInfo SellInfo { get { return mSellInfo; } }
        public override List<GenericBuyInfo> BuyInfo { get { return mBuyInfo; } }

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                //Add(new GenericBuyInfo(typeof(FactionReward), 3, 20, 0x????, 0));
                Add(new GenericBuyInfo("Paladin Shield", typeof(PaladinShield), 5000, 20, 0x3B52, 0));
                Add(new GenericBuyInfo("Name Change Deed", typeof(NameChangeDeed), 40000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("Trinsic Dye Tub", typeof(TrinsicDyeTub), 500, 20, 0xFAB, 0));
                Add(new GenericBuyInfo("Trinsic Township Hood", typeof(PaladinTownshipHood), 50000, 20, 0x3FFC, 0));
                Add(new GenericBuyInfo("Display Case", typeof(DisplayCase), 20000, 20, 0xB09, 0));
                Add(new GenericBuyInfo("Display Case Lid", typeof(DisplayCaseLid), 5000, 20, 0xB0B, 0));
                Add(new GenericBuyInfo("Clothing Bless Deed", typeof(ClothingBlessDeed), 50000, 20, 0x14F0, 0));
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {
            }
        }
    }

    public class SBUrukhaiTownshipReward : SBInfo
    {
        private List<GenericBuyInfo> mBuyInfo = new InternalBuyInfo();
        private IShopSellInfo mSellInfo = new InternalSellInfo();

        public SBUrukhaiTownshipReward()
        {

        }

        public override IShopSellInfo SellInfo { get { return mSellInfo; } }
        public override List<GenericBuyInfo> BuyInfo { get { return mBuyInfo; } }

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                //Add(new GenericBuyInfo(typeof(FactionReward), 3, 20, 0x????, 0));
                Add(new GenericBuyInfo("Uruk-hai Shield", typeof(UrukhaiShield), 5000, 20, 0x3B52, 0));
                Add(new GenericBuyInfo("Name Change Deed", typeof(NameChangeDeed), 40000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("Blackrock Dye Tub", typeof(BlackrockDyeTub), 500, 20, 0xFAB, 0));
                Add(new GenericBuyInfo("Uruk-hai Township Hood", typeof(UrukhaiTownshipHood), 50000, 20, 0x3FFC, 0));
                Add(new GenericBuyInfo("Display Case", typeof(DisplayCase), 20000, 20, 0xB09, 0));
                Add(new GenericBuyInfo("Display Case Lid", typeof(DisplayCaseLid), 5000, 20, 0xB0B, 0));
                Add(new GenericBuyInfo("Clothing Bless Deed", typeof(ClothingBlessDeed), 50000, 20, 0x14F0, 0));
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {
            }
        }
    }

    public class SBForsakenTownshipReward : SBInfo
    {
        private List<GenericBuyInfo> mBuyInfo = new InternalBuyInfo();
        private IShopSellInfo mSellInfo = new InternalSellInfo();

        public SBForsakenTownshipReward()
        {

        }

        public override IShopSellInfo SellInfo { get { return mSellInfo; } }
        public override List<GenericBuyInfo> BuyInfo { get { return mBuyInfo; } }

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                //Add(new GenericBuyInfo(typeof(FactionReward), 3, 20, 0x????, 0));
                Add(new GenericBuyInfo("Forsaken Shield", typeof(ForsakenShield), 5000, 20, 0x3B52, 0));
                Add(new GenericBuyInfo("Name Change Deed", typeof(NameChangeDeed), 40000, 20, 0x14F0, 0));
                Add(new GenericBuyInfo("Forsaken Dye Tub", typeof(ForsakenDyeTub), 500, 20, 0xFAB, 0));
                Add(new GenericBuyInfo("Forsaken Township Hood", typeof(ForsakenTownshipHood), 50000, 20, 0x3FFC, 0));
                Add(new GenericBuyInfo("Display Case", typeof(DisplayCase), 20000, 20, 0xB09, 0));
                Add(new GenericBuyInfo("Display Case Lid", typeof(DisplayCaseLid), 5000, 20, 0xB0B, 0));
                Add(new GenericBuyInfo("Clothing Bless Deed", typeof(ClothingBlessDeed), 50000, 20, 0x14F0, 0));
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
