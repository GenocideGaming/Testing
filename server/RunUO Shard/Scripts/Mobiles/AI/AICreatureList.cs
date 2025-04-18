using System;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Factions;
using Server.Scripts;

namespace Server
{
    public class AICreatureList
    {
        private AIGroup m_AIGroup;
        private AISubgroup m_AISubgroup;
        private Type[] m_Types;

        public AIGroup Group
        {
            get { return m_AIGroup; }
            set { m_AIGroup = value; }
        }

        public AISubgroup Subgroup
        {
            get { return m_AISubgroup; }
            set { m_AISubgroup = value; }
        }

        public Type[] Types
        {
            get { return m_Types; }
            set { m_Types = value; }
        }

        public AICreatureList(AIGroup group, AISubgroup subgroup, Type[] types)
        {
            m_AIGroup = group;
            m_AISubgroup = subgroup;
            m_Types = types;
        }

        public static bool Contains(object obj)
        {
            if (m_Table == null)
                LoadTable();

            AICreatureList sp = (AICreatureList)m_Table[obj.GetType()];

            return (sp != null);
        }

        public static bool GetAI(object obj)
        {
            if (m_Table == null)
                LoadTable();

            AICreatureList sp = (AICreatureList)m_Table[obj.GetType()];

            if (sp == null)
                return false;

            BaseCreature creature = obj as BaseCreature;

            if (creature != null)
            {
                creature.Group = sp.Group;
                creature.Subgroup = sp.Subgroup;
            }

            return true;
        }

        private static void LoadTable()
        {
            m_Table = new Hashtable();

            for (int i = 0; i < m_AIList.Length; ++i)
            {
                AICreatureList info = m_AIList[i];
                Type[] types = info.Types;

                for (int j = 0; j < types.Length; ++j)
                    m_Table[types[j]] = info;
            }
        }

        private static Hashtable m_Table;

        private static AICreatureList[] m_AIList = new AICreatureList[]
		{            
			new AICreatureList( AIGroup.MonsterGeneric, AISubgroup.None, new Type[]
			{	
                typeof( GargoyleTaskmaster),
			    typeof( AntLion ),		
                typeof( ArcticOgreLord ),
                typeof( BogThing ),				
	            typeof( Ettin ),			
                typeof( FrostOoze ),		
                typeof( FrostTroll ),
                typeof( Golem ),
				typeof( HeadlessOne ),		
                typeof( Jwilson ),			
                typeof( Juggernaut ),
			    typeof( Ogre ),		
		        typeof( BridgeEttin ),
                typeof( BridgeLockmoorArcher ),
                typeof( BridgeLockmoorGuard ),
                typeof( BridgeOrcArcher ),
                typeof( BridgeOrcCaptain ),
                typeof( OgreLord ),			
                typeof( PlagueBeast ),
			    typeof( Quagmire ),	
                typeof( Slime ),
                typeof( EarthElemental ),
                typeof( ValoriteElemental ),
                typeof( VeriteElemental ), 
                typeof( Troll ),
                typeof( AgapiteElemental ),
                typeof( BronzeElemental ),
                typeof( CopperElemental ),
                typeof( DullCopperElemental ),
                typeof( GoldenElemental ),                
                typeof( PlagueSpawn ),
                typeof( ShadowIronElemental ),
                typeof( SnowElemental ),
                typeof( BlackSolenInfiltratorQueen ),
                typeof( BlackSolenInfiltratorWarrior ),
				typeof( BlackSolenQueen ),	
                typeof( BlackSolenWarrior ), 
                typeof( BlackSolenWorker ),
                typeof( Bogling ),
                typeof( Centaur ),	
                typeof( ChaosDaemon ),
                typeof( Cyclops ),	
                typeof( DeepSeaSerpent ),                
	            typeof( FrostSpider ),	
                typeof( IceSerpent ),
	            typeof( GiantSerpent ),
                typeof( EncagedGiantSerpent ),
				typeof( GiantSpider ),
                typeof( Harpy ),
                typeof( PeculiarOrc ),
                typeof( HordeMinion ),
                typeof( IceSnake ),	
                typeof( Kraken ),
		        typeof( Lizardman ),
                typeof( Mongbat ),
				typeof( StrongMongbat ),
                typeof( Orc ),
                typeof( OrcArcher ),
                typeof( OrcBomber ),		
                typeof( OrcBrute ),		
                typeof( OrcCaptain ),
				typeof( OrcishLord ),
                typeof( Ratman ),			
                typeof( RatmanArcher ),	
                typeof( LockmoorArcher ),
                typeof( LockmoorButcher ),
                typeof( LockmoorGuard ),
                typeof( Torturer ),
                typeof( RedSolenInfiltratorQueen ), 
                typeof( RedSolenInfiltratorWarrior ), 
                typeof( RedSolenQueen ),
				typeof( RedSolenWarrior ),	
                typeof( RedSolenWorker ),
                typeof( StoneGargoyle ),	
                typeof( StoneHarpy ),
                typeof( SwampTentacle ),
                typeof( TerathanDrone ),
                typeof( TerathanWarrior ),
                typeof( WhippingVine ),
                typeof( MeerWarrior ),
                typeof( InfestedRat ),
                typeof( MeerCaptain ),	
                typeof( JukaLord ),	
                typeof( JukaWarrior ),
                typeof( LockmoorNoble ),
                typeof( LockmoorRoyalty ),
	            typeof( EnslavedGargoyle ),
                typeof( ExodusMinion ),
                typeof( ExodusOverseer ),
                typeof( Moloch ),
                typeof( OphidianWarrior ),	
                typeof( OphidianKnight ),	
                typeof( OphidianDestroyer ),
                typeof( SandVortex ),	
                typeof( Leviathan ),
                typeof( GazerLarva ),
                typeof( GargoyleYoungWarrior ),
                typeof( SandVortex ),
                typeof( BearDruid ),
                typeof( BearformDruid ),
                typeof( Puddle )
			} ),

            new AICreatureList(AIGroup.MonsterGeneric,AISubgroup.MeleeMage1, new Type[]
			{	
				 typeof( GargoyleYoung ),
                 typeof( Bogle ),
                 typeof( Titan ),
			} )	,

			new AICreatureList(AIGroup.MonsterGeneric,AISubgroup.Mage1, new Type[]
			{					
			    typeof( ShadowWisp ),                
			} )	,
	
            new AICreatureList(AIGroup.MonsterGeneric,AISubgroup.MeleeMage2, new Type[]
			{	               	
		        typeof( ToxicElemental ), 
                typeof( IceElemental ),  
                typeof( Gargoyle ),	
                typeof( GargoyleRector ),
                typeof( IceFiend ),               
                typeof( TerathanAvenger ),
                typeof( FrozenElemental ),
                typeof( GargoyleWarrior ),
            } )	,

		    new AICreatureList(AIGroup.MonsterGeneric,AISubgroup.Mage2, new Type[]
			{	                
                 typeof( Imp ),
                 typeof( Gazer ),
				 typeof( HallMonitor ),
                 typeof( Reaper ),
			} )	,
	
            new AICreatureList(AIGroup.MonsterGeneric,AISubgroup.MeleeMage3, new Type[]
			{	
                typeof( FireGargoyle ),
			    typeof( AirElemental ),                
                typeof( WaterElemental ),
                typeof( BloodElemental ),
                typeof( Efreet ),	
                typeof( GargoyleDestroyer ),
                typeof( GargoyleEnforcer ),                            
                typeof( PoisonElemental ),                
                typeof( SteamElemental ),  
                typeof( FireElemental ),
                typeof( Daemon ),
            } )	,

            new AICreatureList(AIGroup.MonsterGeneric,AISubgroup.Mage3, new Type[]
			{	
                typeof( OphidianMage ),
                typeof( StorageImp ),                
				typeof( FuriousHallMonitor ),   
                typeof( VampireOutcast ),
			} )	,
	
            new AICreatureList(AIGroup.MonsterGeneric,AISubgroup.MeleeMage4, new Type[]
			{
                typeof( Librarian ),               			
                typeof( OphidianMatriarch ),
                typeof( TerathanMatriarch ),
                typeof( OphidianQueen ),                
                typeof( ElderWaterElemental ), 
                typeof( ArcaneDaemon ),
                typeof( Balron ),
                typeof( GargoyleAncient ),
                typeof( BloodthirstyVampire ),
            } )	,

		    new AICreatureList(AIGroup.MonsterGeneric,AISubgroup.Mage4, new Type[]
			{		
               typeof( ElderGazer ),
		       typeof( OphidianArchmage ),
               typeof( GargoyleAugurer ),
               typeof( GargoyleConjurer ),
			} )	,
	
            new AICreatureList(AIGroup.MonsterGeneric,AISubgroup.MeleeMage5, new Type[]
			{
				typeof( Harrower ),		
		        typeof( GargoyleElder ),  
                typeof( EliteGargoyleElder ),  
              	typeof( EliteGargoyleAncient ),
                typeof( DeerformDruid ),
			} )	,

            new AICreatureList(AIGroup.MonsterGeneric,AISubgroup.Mage5, new Type[]
			{
				typeof( AncientLich ), 
                typeof( AncientVampire )
			} )	,

            new AICreatureList(AIGroup.MonsterGeneric,AISubgroup.GroupHealerMage2, new Type[]
			{		
		       typeof( OrcishMage ),
               typeof( RatmanMage ),
               typeof( BridgeOrcishMage ),
               typeof( BridgeMage ),
			} )	,

            new AICreatureList(AIGroup.MonsterGeneric,AISubgroup.GroupHealerMelee, new Type[]
			{		
		        typeof( DustElemental ),	
			} )	,	

            new AICreatureList( AIGroup.EvilHuman, AISubgroup.None, new Type[]
			{	
			    typeof( Executioner ), 
                typeof( Savage ),
                typeof( SavageRider ),
                typeof( KhaldunZealot ),
                typeof( KhaldunRevenant ),
                typeof( Doppleganger ),
                typeof( SanguinConscript ),               
                typeof( SanguinProtector ),
                typeof( EvilButcher ),
                typeof( Pirate ),
                typeof( WaterSirenWarrior ),                
			} ),

            new AICreatureList( AIGroup.EvilHuman, AISubgroup.MeleePotion, new Type[]
			{
                typeof( CrazedMiner ),
                typeof( CrazedLumberjack ),
                typeof( CrazedAdventurer ),
                typeof( WaterNymphWarrior ),
                typeof( CrazedPeasant ),
                typeof( CrazedFarmer ),
                typeof( CrazedAlchemist),
            } ),

            new AICreatureList(AIGroup.EvilHuman,AISubgroup.AntiArmor, new Type[]
			{
                typeof( SanguinDefender ),
                typeof( SanguinKnight ),      
                
			} )	,

            new AICreatureList(AIGroup.EvilHuman,AISubgroup.GroupHealerMelee, new Type[]
			{
                typeof( SanguinMedic ),             
			} )	,             

            new AICreatureList(AIGroup.EvilHuman,AISubgroup.Scout, new Type[]
			{	
                typeof( SanguinScout ), 
			} )	,

            new AICreatureList(AIGroup.EvilHuman,AISubgroup.Thief, new Type[]
			{	                
                typeof( Brigand ) ,
                typeof( RandomChild),
			} )	,
            
            new AICreatureList(AIGroup.EvilHuman,AISubgroup.Assassin, new Type[]
			{     
                 typeof( SanguinAssassin )               
			} )	,
            
            new AICreatureList(AIGroup.EvilHuman,AISubgroup.MeleeMage2, new Type[]
			{                               
			} )	,

            new AICreatureList(AIGroup.EvilHuman,AISubgroup.Mage3, new Type[]
			{	
				typeof( EvilMage ),	
                typeof( LockmoorMage ),
				typeof( GolemController ),
                typeof( SanguinMage ),
                typeof( SanguinWizard ),
                typeof( EvilMageRobed ), 
                typeof( WaterNymph )
			} )	,	

		    new AICreatureList(AIGroup.EvilHuman,AISubgroup.Mage4, new Type[]
			{		
				typeof( EvilMageLord ),	
                typeof( EvilMageLordRobed ),
	            typeof( KhaldunSummoner ),
                typeof( EvilAlchemist ),
                typeof( LockmoorAlchemist ),
                typeof( WaterSiren )
			} )	,

            new AICreatureList(AIGroup.EvilHuman,AISubgroup.MeleeMage5, new Type[]
			{		
				typeof( GrimmochDrummel ),
				typeof( LysanderGathenwale ),
                typeof( MorgBergen ),	
                typeof( ShadowFiend ),					
                typeof( TavaraSewel ),	
			} )	,

            new AICreatureList(AIGroup.EvilHuman,AISubgroup.GroupHealerMage2, new Type[]
			{
                typeof( SanguinHealer ),                           
			} )	,

            new AICreatureList(AIGroup.EvilHuman,AISubgroup.GroupHealerMage3, new Type[]
			{
                typeof( SanguinMender ),               
			} )	,

            new AICreatureList(AIGroup.EvilHuman,AISubgroup.GroupHealerMeleeMage2, new Type[]
			{
                typeof( SavageShaman ),                      
			} )	,
            
            new AICreatureList( AIGroup.NeutralHuman, AISubgroup.None, new Type[]
			{		
		        typeof( Guardian ),                
			} ),
	
            new AICreatureList( AIGroup.NeutralHuman, AISubgroup.MeleeMage3, new Type[]
			{
                typeof( MeerEternal ),
            } ),

            new AICreatureList( AIGroup.NeutralHuman, AISubgroup.Mage3, new Type[]
			{		
                typeof( MeerMage ),                    		
                typeof( JukaMage ),                
			} ),  
            
            new AICreatureList( AIGroup.NeutralHuman, AISubgroup.WanderingHealer, new Type[]
			{		
                typeof( WanderingHealer ), 
			} ),

            new AICreatureList( AIGroup.GoodHuman, AISubgroup.DungeonGuardMelee, new Type[]
			{		
                typeof( DungeonGuardMelee ), 
			} ),

            new AICreatureList( AIGroup.GoodHuman, AISubgroup.DungeonGuardRanged, new Type[]
			{		
                typeof( DungeonGuardRanged ), 
			} ),

            new AICreatureList(AIGroup.FactionHuman,AISubgroup.None, new Type[]
			{	
				typeof( FactionKnight ),    
                typeof( FactionHenchman ),	
                typeof( FactionBerserker ),
                typeof( FactionMercenary ),	                			
			} )	,
            
            new AICreatureList( AIGroup.FactionHuman, AISubgroup.Mage4, new Type[]
			{		
		        typeof( FactionSorceress ),	
                typeof( FactionWizard )			            
			} ),
	
            new AICreatureList( AIGroup.Undead, AISubgroup.None, new Type[]
			{	
			    typeof( Zombie ),
                typeof( Mummy ),
                typeof( RottingCorpse ),
                typeof( BoneKnight ),
                typeof( Ghoul ),
                typeof( Skeleton ),
                typeof( RestlessSoul ),
                typeof( SpectralArmour ),                
                typeof( SkeletalKnight ),
                typeof( Cursed ),                
                typeof( FailedExperiment )
			} ),
			
			new AICreatureList(AIGroup.Undead,AISubgroup.Mage1, new Type[]
			{		
				typeof( Shade ),
		        typeof( Spectre ),
                typeof( Wraith ),	
			} )	,
	
            new AICreatureList(AIGroup.Undead,AISubgroup.MeleeMage2, new Type[]
			{
                typeof( SkeletalDragon ),
                typeof( RestlessStudent ),
                typeof( SpectralStudent ),
            } )	,

		    new AICreatureList(AIGroup.Undead,AISubgroup.Mage2, new Type[]
			{		
				typeof( SkeletalMage ),
			    typeof( BoneMagi ),
			} )	,
	
            new AICreatureList(AIGroup.Undead,AISubgroup.MeleeMage3, new Type[]
			{	
                typeof( EtherealWarrior ),	
            } )	,

            new AICreatureList(AIGroup.Undead,AISubgroup.Mage3, new Type[]
			{	
		        typeof( Lich ),
                typeof( AngryGuest ),
			} )	,	

		    new AICreatureList(AIGroup.Undead,AISubgroup.Mage4, new Type[]
			{	
				typeof( LichLord ),
	            typeof( DeerDruid ),
	            typeof( Vampire )
			} )	,	

            new AICreatureList(AIGroup.Undead,AISubgroup.Mage5, new Type[]
			{	
				typeof( AncientLich )			    
			} )	,

            new AICreatureList( AIGroup.EvilAnimal, AISubgroup.None, new Type[]
			{	
			    typeof( PredatorHellCat ), 
                typeof( LavaLizard ),
                typeof( LavaSerpent ),
	            typeof( LavaSnake ),
                typeof( SeaSerpent ),
                typeof( GiantBlackWidow ),
                typeof( SilverSerpent ),
                typeof( FrenziedOstard ),
                typeof( Alligator ),
                typeof( EncagedAlligator ),
                typeof( ScaledSwampDragon ),
                typeof( SwampDragon ),
                typeof( Gaman ),
                typeof( HellHound ),
                typeof( HellCat ), 
                typeof( Wyvern ),
                typeof( SkeletalMount ),
                typeof( FireSteed ),
		        typeof( Nightmare ),
                typeof( Hiryu ), 
                typeof( LesserHiryu ),
		        typeof( VorpalBunny ), 
                typeof( BabyWaterWyrm ),
                typeof( Scorpion ),
                typeof( EncagedScorpion ),
                typeof( Drake ),
			} ),


            new AICreatureList( AIGroup.EvilAnimal, AISubgroup.MeleeMage1, new Type[]
			{
                typeof( Dragon ),
                typeof( WaterWyrm )
            } ),

            new AICreatureList( AIGroup.EvilAnimal, AISubgroup.MeleeMage2, new Type[]
			{	
			    typeof( ShadowWyrm ), 
			} ),

            new AICreatureList( AIGroup.EvilAnimal, AISubgroup.MeleeMage3, new Type[]
			{			
	            typeof( WhiteWyrm ),
                typeof( DreadSpider ),
	            typeof( AncientWyrm ),    
			} ),

            new AICreatureList( AIGroup.EvilAnimal, AISubgroup.MeleeMage4, new Type[]
			{   
			} ),

            new AICreatureList( AIGroup.EvilAnimal, AISubgroup.MeleeMage5, new Type[]
			{	
			    typeof( SerpentineDragon ), 
			} ),

            new AICreatureList( AIGroup.Animal, AISubgroup.SuperPredator, new Type[]
			{
				
			} ),

            new AICreatureList( AIGroup.Animal, AISubgroup.Predator, new Type[]
			{	
			     typeof( Cougar ), 
                 typeof( SnowLeopard ),
                 typeof( Panther ), 
                 typeof( DireWolf ),
                 typeof( EncagedDireWolf ),
	             typeof( GreyWolf ),
		         typeof( WhiteWolf ),
                 typeof( TimberWolf ),
                 typeof( EncagedTimberWolf ),
			} ),

            new AICreatureList( AIGroup.Animal, AISubgroup.Prey, new Type[]
			{	
                 typeof( Boar ),	
			     typeof( Rat ),
                 typeof( Sewerrat ),
                 typeof( Chicken ),	
                 typeof( GiantRat ),
                 typeof( Goat ),
	             typeof( Cow ),
                 typeof( JackRabbit ),
                 typeof( Llama ),
                 typeof( EncagedLlama ),
                 typeof( MountainGoat ),
		         typeof( Pig ),
                 typeof( Sheep ),	
                 typeof( Cat ),
                 typeof( Dog ),
                 typeof( Hind ),	
                 typeof( Horse ),
                 typeof( GreatHart ),
                 typeof( Rabbit ),
                 typeof( EncagedRabbit ),
                 typeof( Bird ),
                 typeof( Crane ),
			} ),  
 
            new AICreatureList( AIGroup.Animal, AISubgroup.None, new Type[]
			{	
			     typeof( GrizzlyBear ),	
                 typeof( EncagedGrizzlyBear ),	
                 typeof( PolarBear ),
                 typeof( EncagedPolarBear ),
                 typeof( Walrus ), 
                 typeof( Beetle ),
                 typeof( Bull ),
		         typeof( BullFrog ),                 
                 typeof( GiantToad ),  
                 typeof( EncagedGiantToad ),   
	             typeof( Snake ),
                 typeof( Dolphin ),
                 typeof( BlackBear ),
                 typeof( BrownBear ),
                 typeof( Gorilla ),  
                 typeof( EncagedGorilla ),  
                 typeof( PackHorse ),
				 typeof( PackLlama ),
                 typeof( PackHorse ),				 
                 typeof( RidableLlama ),
                 typeof( Ridgeback ),
                 typeof( SilverSteed ),
                 typeof( Unicorn ),
                 typeof( TBWarHorse ),
                 typeof( CoMWarHorse ),
                 typeof( MinaxWarHorse ),
                 typeof( SLWarHorse ),
                 typeof( DesertOstard ),
                 typeof( ForestOstard ),
	             typeof( SavageRidgeback ), 
                 typeof( BlackSheep ), 
			} ),  

            new AICreatureList( AIGroup.Animal, AISubgroup.MeleeMage3, new Type[]
			{	              
                typeof( Kirin ),  
            } ), 

            new AICreatureList( AIGroup.Summoned, AISubgroup.None, new Type[]
			{
                typeof( SummonedEarthElemental ),
            }),

            new AICreatureList( AIGroup.Summoned, AISubgroup.MeleeMage2, new Type[]
			{
                typeof( SummonedDaemon ),
            }),
                        
            new AICreatureList( AIGroup.Summoned, AISubgroup.MeleeMage3, new Type[]
			{
                typeof( SummonedAirElemental ),               
                typeof( SummonedWaterElemental ),
                typeof( SummonedFireElemental ),                
            }),

            new AICreatureList( AIGroup.Summoned, AISubgroup.Berserk, new Type[]
			{
                typeof( BladeSpirits ),	
                typeof( EnergyVortex )
            }),

            new AICreatureList( AIGroup.Boss, AISubgroup.None, new Type[]
			{
                typeof( Serado ),	
			    typeof( Barracoon ),	
                typeof( Mephitis ),
		        typeof( Rikktor ),	
            }),

            new AICreatureList( AIGroup.Boss, AISubgroup.MeleeMage5, new Type[]
			{
                typeof( LordOaks ),			
                typeof( Silvani ),
                typeof( Neira ),
                typeof( Semidar ),
            }),

            new AICreatureList( AIGroup.None, AISubgroup.Mage3, new Type[]
			{	
                typeof( Wisp ),               
            } ),  

             new AICreatureList( AIGroup.None, AISubgroup.Mage4, new Type[]
			{	
                typeof( Pixie ),                 
            } ) 
		};
    }
}