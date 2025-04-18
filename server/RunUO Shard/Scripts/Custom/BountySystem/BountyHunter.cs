using System;
using Server.Items;
using Server;
using Server.Misc;
using Server.Scripts;
using Server.Scripts.Custom.BountySystem;
using Server.Scripts.Custom.WebService;

namespace Server.Mobiles
{
    class BountyHunter : BaseCreature
    {
        public override bool CanTeach { get { return false; } }

        [Constructable]
        public BountyHunter() : base (AIType.AI_Vendor, FightMode.None, 2, 1, 0.5, 2)
        {
            InitStats( 90, 75, 25 );

            // Todo: set skills

			SpeechHue = Utility.RandomDyedHue();
			Title = "the bounty hunter";
			Hue = Utility.RandomSkinHue();

			if( this.Female = Utility.RandomBool() )
			{
				this.Body = 0x191;
				this.Name = NameList.RandomName( "female" );

                AddItem(new FemalePlateChest());
                AddItem(new PlateArms());
                AddItem(new PlateLegs());
                AddItem(new Halberd());

                switch (Utility.Random(2))
                {
                    case 0: AddItem(new Doublet(Utility.RandomNondyedHue())); break;
                    case 1: AddItem(new BodySash(Utility.RandomNondyedHue())); break;
                }

                switch (Utility.Random(2))
                {
                    case 0: AddItem(new Skirt(Utility.RandomNondyedHue())); break;
                    case 1: AddItem(new Kilt(Utility.RandomNondyedHue())); break;
                }


            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");

                AddItem(new PlateChest());
                AddItem(new PlateArms());
                AddItem(new PlateLegs());
                AddItem(new Halberd());

                switch (Utility.Random(3))
                {
                    case 0: AddItem(new Doublet(Utility.RandomNondyedHue())); break;
                    case 1: AddItem(new Tunic(Utility.RandomNondyedHue())); break;
                    case 2: AddItem(new BodySash(Utility.RandomNondyedHue())); break;
                }
            }

            Utility.AssignRandomHair(this);

        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!e.Handled && from is PlayerMobile && from.InRange(this.Location, 2) && IsEnrolRequest(e.Speech))
            {
                PlayerMobile pm = (PlayerMobile)from;

                if (pm.BountyHunter)
                    Say(FeatureList.BountyHunters.AlreadyBountyHunterText);
                else
                    Say(FeatureList.BountyHunters.RegistrationOfferText);

                SetDirection(GetDirectionTo(pm.X, pm.Y));

                e.Handled = true;
            }

            base.OnSpeech(e);
        }

        // YEAH THAT'S RIGHT I SAID ENROL, RULE BRITANNIA MOTHERFUCKERS
        private bool IsEnrolRequest(string spokenText)
        {
            foreach (string candidate in FeatureList.BountyHunters.EnrolText)
            {
                if (Insensitive.Contains(spokenText, candidate))
                    return true;
            }

            return false;
        }

        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            PlayerMobile pm = (PlayerMobile)from;

            if (pm != null && dropped is SeveredHead)
            {
                SeveredHead head = (SeveredHead)dropped;
                
                if (head.Decayed)
                {
                    Say("The head is too decayed to identify. It's worthless to me.");
                    return false;
                }

                if(!pm.BountyHunter)
                {
                    Say("Good job, {0}. You should join the Bounty Hunters Guild and earn blood coin for your heads!", from.Name);
                }

                if (pm.BountyHunter && head.OwnerExists)
                {
                    head.From.PlayerMurdererStatus = PlayerMobile.MurdererStatus.Parole;
                    
                    head.From.SendMessage(0x8A, "{0} has turned your head over to the authorities!", from.Name);

                    if(BountyRegistry.Contains(head.From))
                    {
                        head.From.AutomatedBounty = 0;
                        head.From.PlayerBounty = 0;

                        int bounty = BountyRegistry.FindBountyByName(head.From.Name);

                        if(bounty > 0)
                        {    
                            Say("Well done, {0}. {1} blood coins have been added to your coin box for this bountied head", pm.Name, bounty);
                            pm.BloodCoins = (pm.BloodCoins + bounty);
                        }
                        
                        // This shouldn't happen, but adding it in to handle the error and so the player gets something
                        else if(bounty <= 0)
                        {
                            Say("Good job, {0}. I can't read the bounty amount but here's 5 blood coins for your work.", from.Name);
                            pm.BloodCoins = (pm.BloodCoins + 5);
                        }
                            

                        BountyRegistry.ClearBounty(head.From);
                    }
                }

                SeveredHead.DecayAllHeads(head.OwnerID);
                dropped.Delete();
                
                //Database
                pm.HeadsTurnedInThisSession++;
                return true;
                    
            }

            if (pm != null && dropped is BankCheck)
            {
                BankCheck check = dropped as BankCheck;

                // it's possible to be in statloss post-expiration because the save hasn't ticked yet
                // if this is the case, don't take the gold
                if (pm.PlayerMurdererStatus == PlayerMobile.MurdererStatus.Parole && pm.BountyParoleExpiration > pm.GameTime)
                {
                    ReduceParoleDurationByAmount(pm, check.Worth);

                    dropped.Delete();
                    return true;
                }
            }

            return base.OnDragDrop(from, dropped);
        }

        private void ReduceParoleDurationByAmount(PlayerMobile pm, int amount)
        {
            TimeSpan reductionAmount = TimeSpan.FromHours((double)amount / 
                (double)FeatureList.MurderSystem.BribeForRemovingOneHourOfParole); 

            if ((pm.BountyParoleExpiration - reductionAmount) < pm.GameTime)
            {
                Emote("*cough*");
                Say("There's been a terrible misunderstanding! This {0} is clearly innocent.", GenderOf(pm));

                // I think we should go ahead and change them back to a regular murderer at this point
                // instead of waiting on the server save tick to do so
                pm.PlayerMurdererStatus = PlayerMobile.MurdererStatus.None;

                pm.BountyParoleExpiration = TimeSpan.MinValue;
            }
            else
            {
                Emote("*cough*");
                Say("It seems there's a small mistake in your records, I'll see that it's corrected.");

                pm.BountyParoleExpiration -= reductionAmount;
            }
        }

        private TimeSpan CalculateParoleDuration(int bounty)
        {
            double numberOfKills = (double)bounty / (double)FeatureList.MurderSystem.AutomaticBountyPerKill;
            //double minutesOfStatloss = numberOfKills * FeatureList.MurderSystem.StatlossPenaltyDurationPerKill;
            double minutesOfParole = numberOfKills * FeatureList.MurderSystem.ParolePenaltyDurationPerKill;
            return TimeSpan.FromMinutes(minutesOfParole);
        }

        public override bool OnGoldGiven(Mobile from, Gold dropped)
        {
            PlayerMobile pm = from as PlayerMobile;
            if (pm == null) { return false; }

            /* if (pm.PlayerMurdererStatus == PlayerMobile.MurdererStatus.Outcast)
            {
                Random random = new Random();
                int randomNumber = random.Next(0, 2);
                String disgustedResponse = "";
                switch (randomNumber)
                {
                    case 0: disgustedResponse = "Back, you fiend!"; break;
                    case 1: disgustedResponse = "I won't accept this blood money!  Away with you!"; break;
                }
                Say(disgustedResponse);
            } */
            // it's possible to be in statloss post-expiration because the save hasn't ticked yet
            // if this is the case, don't take the gold
            //else 
            if (pm.PlayerMurdererStatus == PlayerMobile.MurdererStatus.Parole && pm.BountyParoleExpiration > pm.GameTime)
            {
                ReduceParoleDurationByAmount(pm, dropped.Amount);

                dropped.Delete();
                return true;
            }
            else if (dropped.Amount == FeatureList.BountyHunters.RegistrationCost)
            {
                if (pm.Kills >= 5)
                {
                    Say("Do you plan to hunt yourself?! I won't register you as a bounty hunter!");
                    return false;
                }
                if (!pm.BountyHunter)
                {
                    Say(FeatureList.BountyHunters.JoinText);
                    pm.BountyHunter = true;

                    pm.Delta(MobileDelta.Noto);
                    pm.InvalidateProperties();

                    //DatabaseController.UpdateCharacterBountyHunter(pm, true);

                    dropped.Delete();
                    return true;
                }
            }

            return false;
        }

        private string GenderOf(PlayerMobile pm)
        {
            if (pm.Female)
                return "woman";
            else
                return "man";
        }

        private FemaleStuddedChest GenerateFemaleChest()
        {
            FemaleStuddedChest chest = new FemaleStuddedChest();
            chest.Hue = 0x96C;
            return chest;
        }

        private StuddedLegs GenerateStuddedLegs()
        {
            StuddedLegs legs = new StuddedLegs();
            legs.Hue = 0x966;
            return legs;
        }

        #region Serialization
        public BountyHunter( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 ); // version 
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
        }
        #endregion
    }
}
