using System;
using System.IO;
using System.Reflection;
using Server;
using Server.Items;
using Server.Mobiles;
using System.Collections.Generic;


namespace Server.Mobiles
{
	public class LootBag
	{		
        public static int[,] GoldTiers = new int[,] {
                                         { 10, 25 },     //Tier 1
                                         { 25, 50 },     //Tier 2
                                         { 50, 75 },     //Tier 3
                                         { 75, 100 },    //Tier 4
                                         { 100, 150 },   //Tier 5
                                         { 150, 250 },   //Tier 6
                                         { 250, 400 },   //Tier 7
                                         { 400, 600 },   //Tier 8
                                         { 600, 800 },   //Tier 9
                                         { 900, 1000 },  //Tier 10
                                         { 1000, 1250 }, //Tier 11
                                         { 1250, 1500 }, //Tier 12
                                         };

        public static double[,,] ArtifactTiers = new double[,,]
                                         {
                                            {                       //{Chance, Min Artifact Level, Max Artifact Level}
                                                {.021, 1, 1},       //Tier 1  (1/300) **Multiplied by 4
                                                {.021, 1, 1},       //Tier 2  (1/200)
                                                {.021, 1, 2},      //Tier 3  (1/200)
                                                {.034, 1, 3},        //Tier 4  (1/165)
                                                {.034, 1, 3 },       //Tier 5  (1/140)
                                                {.040, 2, 3},      //Tier 6  (1/125)
                                                {.040, 2, 4 },        //Tier 7  (1/100)
                                                {.045, 2, 4},       //Tier 8  (1/85)
                                                {.045, 2, 4},      //Tier 9  (1/75)
                                                {.048, 3, 4},        //Tier 10  (1/70)
                                                {.050, 3, 4},       //Tier 11  (1/65)
                                                {.060, 3, 4},         //Tier 12  (1/50)
                                            }
                                        };

        public static int[,] LeatherDyeTubTiers = new int[,]
                                         
                                            {                       //{Chance (divide number by 100), mDyeTubGroup}
                                                {3, 0},       //Tier 1  (1/40) Dull Copper, Copper, Bronze, Golden, Yellow   0
                                                {5, 0},       //Tier 2  (1/20) Dull Copper, Copper, Bronze, Golden, Yellow  1
                                                {3, 1},      //Tier 3  (1/40) Agapite, Verite, Valorie, Shadow  2
                                                {5,  1},        //Tier 4  (1/20) Agapite, Verite, Valorie, Shadow  3
                                                {3, 2},       //Tier 5  (1/40) Green, Red, Blue  4
                                                {5,  2},      //Tier 6  (1/20) Green, Red, Blue  5
                                                {3, 3},        //Tier 7  (1/40) ALL HUES  6
                                                {5,  3},       //Tier 8  (1/20) ALL HUES  7
                                            };

        public static double[,] MapTiers = new double[,]
                                          {                //{Percent Chance, Map Level}
                                          {.005, 1},       //Tier 1 
                                          {.01, 1},        //Tier 2  
                                          {.005, 2},       //Tier 3 
                                          {.004, 2},     //Tier 4  
                                          {.004, 2 },       //Tier 5 
                                          {.007, 3},        //Tier 6  
                                          {.01, 3 },    //Tier 7  
                                          {.02, 3},        //Tier 8  
                                          {.015, 4},        //Tier 9  
                                          {.02, 4},        //Tier 10  
                                          {.025, 5},        //Tier 11  
                                          {.05, 6},         //Tier 12  
                                         };

        public static int[,] ReagentTiers = new int [,] {
                                         { 3, 5 },          //Tier 1
                                         { 5, 10 },         //Tier 2
                                         { 10, 15 },        //Tier 3
                                         { 15, 25 },        //Tier 4
                                         { 25, 50 },        //Tier 5
                                         { 50, 100 },       //Tier 6
                                         { 100, 150 },      //Tier 7
                                         { 150, 200 },      //Tier 8
                                         };

        public static int[,] IngotTiers = new int[,] {
                                         { 3, 5 },          //Tier 1
                                         { 5, 10 },         //Tier 2
                                         { 10, 15 },        //Tier 3
                                         { 15, 25 },        //Tier 4
                                         { 25, 50 },        //Tier 5
                                         { 50, 100 },       //Tier 6
                                         { 100, 150 },      //Tier 7
                                         { 150, 200 },      //Tier 8
                                         };

        public static int[,] BoardTiers = new int[,] {
                                         { 3, 5 },          //Tier 1
                                         { 5, 10 },         //Tier 2
                                         { 10, 15 },        //Tier 3
                                         { 15, 25 },        //Tier 4
                                         { 25, 50 },        //Tier 5
                                         { 50, 100 },       //Tier 6
                                         { 100, 150 },      //Tier 7
                                         { 150, 200 },      //Tier 8
                                         };

        public static int[,] BandageTiers = new int[,] {
                                         { 3, 5 },          //Tier 1
                                         { 5, 10 },         //Tier 2
                                         { 10, 15 },        //Tier 3
                                         { 15, 25 },        //Tier 4
                                         { 25, 50 },        //Tier 5
                                         { 50, 100 },       //Tier 6
                                         { 100, 150 },      //Tier 7
                                         { 150, 200 },      //Tier 8
                                         };

        public static int[,] ArrowTiers = new int[,] {
                                         { 5, 10 },     //Tier 1
                                         { 10, 25 },    //Tier 2
                                         { 25, 50 },    //Tier 3
                                         { 50, 75 },    //Tier 4
                                         { 75, 100 },   //Tier 5
                                         { 100, 150 },  //Tier 6
                                         { 150, 200 },  //Tier 7
                                         { 200, 250 },  //Tier 8
                                         };

        public static int[,] GemTiers = new int[,] {
                                         { 1, 2 },      //Tier 1
                                         { 3, 4 },      //Tier 2
                                         { 4, 6 },      //Tier 3
                                         { 6, 10 },     //Tier 4
                                         { 10, 15 },    //Tier 5
                                         { 15, 20 },    //Tier 6
                                         { 20, 25 },    //Tier 7
                                         { 25, 30 },    //Tier 8
                                         };

        public static int[,,] SpellScrollTiers = new int[,,]
                                         {
                                            {                  //{Min Level of Scroll, Max Level of Scroll, Number of Scrolls}
                                                { 1, 4, 1 },   //Tier 1
                                                { 5, 5, 1 },   //Tier 2
                                                { 5, 6, 1 },   //Tier 3
                                                { 6, 6, 1 },   //Tier 4
                                                { 6, 7, 1 },   //Tier 5
                                                { 7, 7, 1 },   //Tier 6
                                                { 7, 8, 1 },   //Tier 7
                                                { 8, 8, 1 },   //Tier 8
                                            }
                                         };

        public static int[, ,] PoisonPotionTiers = new int[,,] 
                                         {
                                            {                  //{Poison Level, Min Number of Potions, Max Number of Potions}
                                                { 0, 1, 1 },   //Tier 1
                                                { 0, 1, 2 },   //Tier 2
                                                { 1, 1, 1 },   //Tier 3
                                                { 1, 1, 2 },   //Tier 4
                                                { 2, 1, 1 },   //Tier 5
                                                { 2, 1, 2 },   //Tier 6
                                                { 2, 1, 1 },   //Tier 7
                                                { 2, 1, 2 },   //Tier 8
                                            }
                                         };

        public static int[, ,] RandomPotionTiers = new int[,,]
                                         {   
                                            {                  //{Potion Level, Min Number of Potions, Max Number of Potions}
                                                { 0, 1, 1 },   //Tier 1
                                                { 0, 1, 2 },   //Tier 2
                                                { 1, 1, 1 },   //Tier 3
                                                { 1, 1, 2 },   //Tier 4
                                            }
                                         };
        public LootBag()
        {
        }

        public static void Initialize()
        {
        }

        /*
        
        public static Type[] Orcs = new Type [] {  typeof(Orc), typeof(OrcishLord), typeof(OrcCaptain), typeof(OrcishMage) };
        public static Type[] Casters = new Type[] { typeof(OrcishMage), typeof(EvilMage), typeof(EvilMageLord), typeof(SavageShaman) };
        public static Type[] Brutes = new Type [] { typeof(Ettin), typeof(Ogre), typeof(Troll), typeof(ArcticOgreLord), typeof(FrostTroll), typeof(OgreLord) };
        public static Type[] Humans = new Type[] { typeof(EvilMage), typeof(EvilMageLord), typeof(Pirate) };
        public static Type[] Undead = new Type[] { typeof(AncientLich), typeof(Bogle), typeof(BoneKnight), typeof(BoneMagi), typeof(Ghoul), typeof(HellSteed), typeof(Lich), typeof(LichLord), typeof(Mummy),  typeof(RottingCorpse), typeof(Shade), typeof(SkeletalKnight), typeof(SkeletalMage), typeof( SkeletalMount), typeof(Skeleton), typeof(Spectre), typeof(Wraith), typeof(Zombie)};
        public static Type[] Savages = new Type[] { typeof(Savage), typeof(SavageRider), typeof(SavageShaman) };

        public static Item GetGold(int lootLevel)
        {
            int goldAmount;
			
            switch (lootLevel)
            {
                    case 1: goldAmount = Utility.RandomMinMax(tier1[0], tier1[1]); break;
                    case 2: goldAmount = Utility.RandomMinMax(tier2[0], tier2[1]); break;
                    case 3: goldAmount = Utility.RandomMinMax(tier3[0], tier3[1]); break;
                    case 4: goldAmount = Utility.RandomMinMax(tier4[0], tier4[1]); break;
                    case 5: goldAmount = Utility.RandomMinMax(tier5[0], tier5[1]); break;
                    case 6: goldAmount = Utility.RandomMinMax(tier6[0], tier6[1]); break;
                    case 7: goldAmount = Utility.RandomMinMax(tier7[0], tier7[1]); break;
                    case 8: goldAmount = Utility.RandomMinMax(tier8[0], tier8[1]); break;
                    default: goldAmount = 0; break;
            }
			
            Gold gold = new Gold(goldAmount);
            return gold;
        }
		
        public static List<Item> LootBagGenerate(BaseCreature mobile, int lootLevel)
        {
            List<Item> loots = new List<Item>();
			
            loots.AddRange(GenerateGems(lootLevel));
			
            loots.AddRange(GenerateArtifacts(lootLevel));

            if (mobile.Skills[ SkillName.Magery ].Value > 10 )
                loots.AddRange(GenerateCasterLoot(lootLevel));
			
            foreach (Type orc in Orcs)
                if (mobile.GetType() == orc)
                    loots.AddRange(GenerateOrcLoot());
			
            foreach (Type brute in Brutes)
                if (mobile.GetType() == brute)
                    loots.AddRange(GenerateBruteLoot());
			
            foreach (Type human in Humans)
                if (mobile.GetType() == human)
                    loots.AddRange(GenerateHumanLoot());
			
            foreach (Type savage in Savages)
                if (mobile.GetType() == savage)
                    loots.AddRange(GenerateSavageLoot());

            foreach (Type undead in Undead)
                if (mobile.GetType() == undead)
                    loots.AddRange(GenerateUndeadLoot());
			
            if (lootLevel > 0)
                loots.Add( GetGold(lootLevel) );
			
            return loots;
        }
		
        private static List<Item> GenerateGems(int LootLevel)
        {
            List<Item> loots = new List<Item>();
			
            while ( (LootLevel * 2) > Utility.Random(28) )
            {
                loots.Add( Loot.RandomGem() );
            }
			
            return loots;
        }
		
        /* 	Artifact Drop Rates
		 
            LootLevel 1: No artifacts
            LootLevel 2: No artifacts
            LootLevel 3: Tier 1 Artifacts, 1/60 Chance
            LootLevel 4: Tier 1-2 Artifacts, 1/40 Chance
            LootLevel 5: Tier 1-3 Artifacts, 1/35 Chance
            LootLevel 6: Tier 1-4 Artifacts, 1/30 Chance
            LootLevel 7: Tier 2-4 Artifacts, 1/25 Chance
			
            Artifact chance by type
            Slayer 40% 
            Damage 15% (0-3)
            Accuracy 15% (4-7)
            Armor 30% (8-11) 
        */

        /*
		private static List<Item> GenerateArtifacts(int lootLevel)
		{
			List<Item> loots = new List<Item>();
			int dropChance = 0;
			
			switch ( lootLevel )
			{
				case 0: dropChance = 0; break;
				case 1: dropChance = 0; break;
				case 2: dropChance = 0; break;
				case 3: dropChance = 100; break;
				case 4: dropChance = 75; break;
				case 5: dropChance = 60; break;
				case 6: dropChance = 40; break;
				case 7: dropChance = 30; break;
				default: dropChance = 0; break;
			}
			
			if ( Utility.Random(dropChance) == 7 )
			{
				int minArtifact = 0;
				int maxArtifact = 0;
				
				switch ( lootLevel )
				{
					case 3: minArtifact = 0; maxArtifact = 0; break;
					case 4: minArtifact = 0; maxArtifact = 1; break;
					case 5: minArtifact = 0; maxArtifact = 2; break;
					case 6: minArtifact = 0; maxArtifact = 3; break;
					case 7: minArtifact = 1; maxArtifact = 3; break;
					default: minArtifact = 0; break;
				}
				
				int typeRoll = Utility.Random(100);
				
				if (typeRoll <= 39)
					loots.Add( Loot.RandomSlayerArtifact() );
				else if (typeRoll <= 54)
					loots.Add( Loot.RandomArtifact(0 + minArtifact, 0 + maxArtifact) );
				else if (typeRoll <= 69)
					loots.Add( Loot.RandomArtifact(4 + minArtifact, 4 + maxArtifact) );
				else 
					loots.Add( Loot.RandomArtifact(8 + minArtifact, 8 + maxArtifact) );
			}
			
			return loots;
		}
		
		private static List<Item> GenerateOrcLoot()
		{
			List<Item> loots = new List<Item>();
			
			switch ( Utility.Random( 20 ) )
			{
				case 0: loots.Add( new Scimitar() ); break;
				case 1: loots.Add( new Katana() ); break;
				case 2: loots.Add( new WarMace() ); break;
				case 3: loots.Add( new WarHammer() ); break;
				case 4: loots.Add( new Kryss() ); break;
				case 5: loots.Add( new Pitchfork() ); break;
			}
			
			for (int i = 0; i < 2; i++)
			{
				switch ( Utility.Random( 25 ) )
				{
					case 0: loots.Add( new Ribs() ); break;
					case 1: loots.Add( new Shaft() ); break;
					case 2: loots.Add( new Candle() ); break;
					case 3: loots.Add( new Arrow() ); break;
					case 4: loots.Add( new Lockpick() ); break;
					case 5: loots.Add( new Shaft() ); break;
					case 6: loots.Add( new Ribs() ); break;
					case 7: loots.Add( new Bandage() ); break;
					case 8: loots.Add( new BeverageBottle( BeverageType.Wine ) ); break;
					case 9: loots.Add( new Jug( BeverageType.Cider ) ); break;
					case 10: loots.Add( new ThighBoots() ); break;
				}
			}
			
			if ( Utility.Random(50) == 7 )
				loots.Add( new OrcishKinMask() );
			
			return loots;
		}
		
		private static List<Item> GenerateCasterLoot(int lootLevel)
		{
			List<Item> loots = new List<Item>();
			
			if ( (Utility.Random(10) <= lootLevel) && (lootLevel > 0) )
				loots.Add( Loot.RandomScroll((lootLevel * lootLevel - 1), (lootLevel * 10 - 3), SpellbookType.Regular) );
			
			if ( Utility.RandomBool() && (lootLevel >= 5) )
				loots.Add ( Loot.RandomHighScroll() );
			
			Item reagent;
			for (int i = 0; i < Utility.Random(2); i++)
			{
				reagent = Loot.RandomReagent(); 
				reagent.Amount = Utility.RandomMinMax(lootLevel*2+3,lootLevel*2+8); 
				loots.Add(reagent);
			}	
			
			return loots;
		}
		
		private static List<Item> GenerateBruteLoot()
		{
			List<Item> loots = new List<Item>();
			
			for (int i = 0; i < 2; i++)
			{
				switch ( Utility.Random(10) )
				{
					case 0: loots.Add ( new RawRibs() ); break;
					case 1: loots.Add ( new Hides() ); break;
					case 2: loots.Add ( new BeverageBottle(BeverageType.Ale) ); break;
					case 3: loots.Add ( new RawChickenLeg() ); break;
					case 4: loots.Add ( Loot.RandomBodyPart() ); break;
				}
			}
			
			return loots;
		}
		
		private static List<Item> GenerateHumanLoot()
		{
			List<Item> loots = new List<Item>();
			
			if ( Utility.Random(1000) == 7 )
				loots.Add ( new PlayingCards() );
		
			return loots;
		}
		
		private static List<Item> GenerateSavageLoot()
		{
			List<Item> loots = new List<Item>();
			
			loots.Add ( new Bandage(Utility.RandomMinMax(1, 15)) );
			loots.Add ( new BoneArms() );
			loots.Add ( new BoneLegs() );
			
			if ( Utility.RandomBool() )
				loots.Add( new SavageMask() );
			
			if ( Utility.Random(10) == 7 )
				loots.Add( new TribalBerry() );
			
			return loots;
		}
		
		private static List<Item> GenerateUndeadLoot()
		{
			List<Item> loots = new List<Item>();
			
			if ( Utility.Random(5000) == 7 )
				loots.Add ( Loot.RandomNecroReagent() );
					
			for (int i = 0; i < 2; i++)
			{
				switch ( Utility.Random(8) )
				{
					case 0: loots.Add ( new Bone(Utility.Random(10)) ); break;
					case 1: loots.Add ( new Bone(Utility.Random(10)) ); break;
					case 2: loots.Add ( new Bandage() ); break;
					case 4: loots.Add ( Loot.RandomBodyPart() ); break;
					default: break;
				}
			}		
					
			return loots;
		}
        */
	}
}