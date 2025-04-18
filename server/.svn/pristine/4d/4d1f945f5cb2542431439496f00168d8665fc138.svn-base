using System;
using Server;
using Server.Items;
using Server.Scripts;
using EDI = Server.Mobiles.EscortDestinationInfo;

namespace Server.Mobiles
{
	public class SeekerOfAdventure : BaseEscortable
	{

        int TownHome = (Utility.RandomMinMax(1, 5));
        
        private string Town;

        public override string PickRandomDestination()
        {
            return Town;
        }
        
        public override string TakeMe()
        {
            return "Don't let me end up like the others. Please take me home!"; 
        }

        public override string LeadOn()
        {
            return "I'll follow you! Keep me safe and out of danger!";
        }

        public override string ThankYou()
        {
            return "Thank you for saving me! Here is your reward.";
        }


        public override int GoldAmount()
        {
            return 1100;  // how much gold these guys give
        }
        



        [Constructable]

        public SeekerOfAdventure()
		{
            Name = "";

            if (TownHome == 1)
            {
                Town = "Calor";
                Name = "Calori Citizen";
                int TownColor = 2137;
                Item Cloak = new Cloak();
                Cloak.Movable = false;
                Cloak.Hue = TownColor;
                AddItem(Cloak);

            }
            else if (TownHome == 2)
            {
                Town = "Vermell";
                Name = "Vermell Citizen";
                int TownColor = 2149;
                Item Cloak = new Cloak();
                Cloak.Movable = false;
                Cloak.Hue = TownColor;
                AddItem(Cloak);

            }
            else if (TownHome == 3)
            {
                Town = "Arbor";
                Name = "Arbor Citizen";
                int TownColor = 2133;
                Item Cloak = new Cloak();
                Cloak.Movable = false;
                Cloak.Hue = TownColor;
                AddItem(Cloak);

            }
            else if (TownHome == 4)
            {
                Town = "Pedran";
                Name = "Pedran Citizen";
                int TownColor = 2145;
                Item Cloak = new Cloak();
                Cloak.Movable = false;
                Cloak.Hue = TownColor;
                AddItem(Cloak);

            }
            else if (TownHome == 5)
            {
                Town = "Lillano";
                Name = "Lillano Citizen";
                int TownColor = 2141;
                Item Cloak = new Cloak();
                Cloak.Movable = false;
                Cloak.Hue = TownColor;
                AddItem(Cloak);
            }


            
            
		}

        

		public override bool ClickTitle{ get{ return false; } } // Do not display 'the seeker of adventure' when single-clicking

        

		private static int GetRandomHue()
		{
			switch ( Utility.Random( 6 ) )
			{
				default:
				case 0: return 0;
				case 1: return Utility.RandomBlueHue();
				case 2: return Utility.RandomGreenHue();
				case 3: return Utility.RandomRedHue();
				case 4: return Utility.RandomYellowHue();
				case 5: return Utility.RandomNeutralHue();
			}
		}

		public override void InitOutfit()
		{
			if ( Female )
				AddItem( new FancyDress( GetRandomHue() ) );
			else
				AddItem( new FancyShirt( GetRandomHue() ) );

			int lowHue = GetRandomHue();

			AddItem( new ShortPants( lowHue ) );

			if ( Female )
				AddItem( new ThighBoots( lowHue ) );
			else
				AddItem( new Boots( lowHue ) );

			if ( !Female )
				AddItem( new BodySash( lowHue ) );


			AddItem( new Longsword() );

			Utility.AssignRandomHair( this );

		}
        
		public SeekerOfAdventure( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}