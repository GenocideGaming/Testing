using System;
using System.Collections.Generic;
using System.Text;
using Server.Mobiles;
using Server.Network;
using Server.Factions;
using Server.Items;

namespace Server.Gumps
{
    public enum RewardCurrency
    {
        Gold = 1,
        Copper,
        Silver,
    }
    //Reward gumps contain paginated results, 4 returned per page with buy buttons
    class BaseRewardGump : Gump
    {
        private int mCurrentPage;
        private SBInfo mSellBuyInfo;
        private Mobile mCaller;
        private int mPages;
        private RewardCurrency mCurrency;
        private Type mCurrencyType;
        private string mTitle;
        private Point3D mOpenLocation;

        public BaseRewardGump(SBInfo sellBuyInfo, int currentPage, Mobile from, RewardCurrency currency, string title) : base(100, 100)
        {
            mSellBuyInfo = sellBuyInfo;
            mCurrentPage = currentPage;
            mCaller = from;
            mPages = sellBuyInfo.BuyInfo.Count / 4;
            mCurrency = currency;
            mTitle = title;
            mOpenLocation = mCaller.Location;

            switch (currency)
            {
                case RewardCurrency.Copper:
                    mCurrencyType = null; //todo add copper
                    break;
                case RewardCurrency.Gold:
                    mCurrencyType = typeof(Gold);
                    break;
                case RewardCurrency.Silver:
                    mCurrencyType = typeof(Silver);
                    break;
            }

            if (mCurrencyType == null)
                return;

            BuildGump(title);
        }

        private void BuildGump(string title)
        {
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;

            AddPage(0);
            AddBackground(0, 0, 370, 350, 9380);

            AddImage(28, 60, 50);
            AddImage(175, 60, 50);
            AddImage(28, 285, 50);
            AddImage(175, 285, 50);

            AddButton(322, 300, 2085, 2085, (int)Buttons.PageNext1, GumpButtonType.Reply, 0);
            AddLabel(115, 35, 0, title);
            AddButton(322, 40, 2084, 2084, (int)Buttons.PagePrevious1, GumpButtonType.Reply, 0);

            int xImageOffset = 45; int xImageSpacing = 140;
            int yImageOffset = 85; int yImageSpacing = 100;
            
            int xTextOffset = 90; int xTextSpacing = 140; 
            int yTextOffset = 75; int yTextSpacing = 110;
            int textAreaWidth = 90; int textAreaHeight = 55;

            int xPriceOffset = 95; int xPriceSpacing = 140;
            int yPriceOffset = 130; int yPriceSpacing = 110;

            int xButtonOffset = 110; int xButtonSpacing = 140;
            int yButtonOffset = 150; int yButtonSpacing = 110;


            int iStart = mCurrentPage * 4;
            int i = iStart;
            int buttonIndex = 3; //start after the next and back button
            for (int y = 0; y < 2; y++)
            {
                for (int x = 0; x < 2; x++)
                {
                    
                    if(i < mSellBuyInfo.BuyInfo.Count)
                    {
                        
                        AddItem(xImageOffset + x * xImageSpacing, yImageOffset + y * yImageSpacing, mSellBuyInfo.BuyInfo[i].ItemID, mSellBuyInfo.BuyInfo[i].Hue);
                        AddHtml(xTextOffset + x * xTextSpacing, yTextOffset + y * yTextSpacing, textAreaWidth, textAreaHeight, mSellBuyInfo.BuyInfo[i].Name, false, false);
                        AddLabel(xPriceOffset + x * xPriceSpacing, yPriceOffset + y * yPriceSpacing, 0, "Price " + mSellBuyInfo.BuyInfo[i].Price);
                        AddButton(xButtonOffset + x * xButtonSpacing, yButtonOffset + y * yButtonSpacing, 1153, 1154, buttonIndex, GumpButtonType.Reply, 0);
                    }
                    i++;
                    buttonIndex++;
                }
            }

            string currencyAvailableName = string.Empty;
           
            if (mCurrency == RewardCurrency.Gold)
            {
                currencyAvailableName = "Gold Available";
               
            }
            else if (mCurrency == RewardCurrency.Silver)
            {
                currencyAvailableName = "Silver Available";
               
            }
            else if (mCurrency == RewardCurrency.Copper)
            {
                currencyAvailableName = "Copper Available";
                //todo add copper
            }


            Item currency = mCaller.Backpack.FindItemByType(mCurrencyType);
            int currencyAvailable = 0;
            if(currency != null && currency.Amount > 0)
            {
                currencyAvailable = currency.Amount;
            }
           
            AddLabel(31, 301, 0, currencyAvailableName);
            AddLabel(156, 303, 0, @": "+ currencyAvailable);

            

        }


        public enum Buttons
        {
            PageNext1 = 1,
            PagePrevious1,
        }

        public enum PurchaseResult
        {
            Success,
            NotEnoughCurrency,
            InvalidItem,
            NoPurchase
        }
        public virtual PurchaseResult Purchase(int index)
        {
            int i = mCurrentPage * 4 + index;

            if (i > mSellBuyInfo.BuyInfo.Count)
                return PurchaseResult.InvalidItem;

            int price = mSellBuyInfo.BuyInfo[i].Price;

            Item[] currencyStacks;
            bool fromBank = false;
            
            currencyStacks = mCaller.Backpack.FindItemsByType(mCurrencyType);

            int currencyAmount = 0;
            foreach (Item silverStack in currencyStacks)
            {
                currencyAmount += silverStack.Amount;
            }

            if (price > 2500 && currencyAmount < price)
            {
                currencyStacks = mCaller.BankBox.FindItemsByType(mCurrencyType);
                fromBank = true;
            }

            currencyAmount = 0;
            foreach (Item silverStack in currencyStacks)
            {
                currencyAmount += silverStack.Amount;
            }

            if (currencyStacks == null || currencyAmount < price)
                return PurchaseResult.NotEnoughCurrency;

            if (fromBank)
                mCaller.BankBox.ConsumeTotal(mCurrencyType, price);
            else
                mCaller.Backpack.ConsumeTotal(mCurrencyType, price);

            Type type = mSellBuyInfo.BuyInfo[i].Type;
            Item purchasedItem = Construct(type);

            if (purchasedItem is MilitiaCloth)
                purchasedItem.Amount = 10;

            if (!mCaller.Backpack.TryDropItem(mCaller, purchasedItem, true))
                purchasedItem.MoveToWorld(mCaller.Location, Map.Felucca);

            PlayerMobile player = mCaller as PlayerMobile;
            if (player != null && mCurrency == RewardCurrency.Gold)
                player.GoldSpentThisSession += price;

            return PurchaseResult.Success;
            
        }

        public static Item Construct(Type type)
        {
            try
            {
                return Activator.CreateInstance(type) as Item;
            }
            catch
            {
                return null;
            }
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            if (from.GetDistanceToSqrt(mOpenLocation) > 10)
            {
                from.SendMessage("That is too far away.");
                from.CloseGump(typeof(BaseRewardGump));
                return;
            }

            PurchaseResult result = PurchaseResult.NoPurchase;
            switch (info.ButtonID)
            {
                case (int)Buttons.PageNext1:
                    {
                        if (mCurrentPage + 1 < mPages)
                            from.SendGump(new BaseRewardGump(mSellBuyInfo, mCurrentPage + 1, from, mCurrency, mTitle));
                        else
                            from.SendGump(new BaseRewardGump(mSellBuyInfo, mPages, from, mCurrency, mTitle));
                        break;
                    }
                case (int)Buttons.PagePrevious1:
                    {
                        if (mCurrentPage > 0)
                            from.SendGump(new BaseRewardGump(mSellBuyInfo, mCurrentPage - 1, from, mCurrency, mTitle));
                        else
                            from.SendGump(new BaseRewardGump(mSellBuyInfo, 0, from, mCurrency, mTitle));

                        break;
                    }
                case 3:
                    {
                        result = Purchase(0);
                        break;
                    }
                case 4:
                    {
                        result = Purchase(1);
                        break;
                    }
                case 5:
                    {
                        result = Purchase(2);
                        break;
                    }
                case 6:
                    {
                        result = Purchase(3);
                        break;
                    }

            }

            if (result == PurchaseResult.Success)
            {
                from.SendMessage("You have purchased a reward!");
            }
            else if (result == PurchaseResult.NotEnoughCurrency)
            {
                from.SendMessage("You do not have enough currency to complete that purchase.");
            }
            else if( result == PurchaseResult.InvalidItem)
            {
                from.SendMessage("Sorry but that is an invalid item, cannot complete purchase.");
            }
        }
    }
}
