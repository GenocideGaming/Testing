using System;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Factions;
using Server.Scripts;

namespace Server
{
    public class SpeedInfo
    {
        // Should we use the new method of speeds?

        private static bool Enabled = FeatureList.BaseCreatureAI.EnableSpeedTables;

        private double m_ActiveSpeed;
        private double m_PassiveSpeed;
        private Type[] m_Types;

        public double ActiveSpeed
        {
            get { return m_ActiveSpeed; }
            set { m_ActiveSpeed = value; }
        }

        public double PassiveSpeed
        {
            get { return m_PassiveSpeed; }
            set { m_PassiveSpeed = value; }
        }

        public Type[] Types
        {
            get { return m_Types; }
            set { m_Types = value; }
        }

        public SpeedInfo(double activeSpeed, double passiveSpeed, Type[] types)
        {
            m_ActiveSpeed = activeSpeed;
            m_PassiveSpeed = passiveSpeed;
            m_Types = types;
        }

        public static bool Contains(object obj)
        {
            if (!Enabled)
                return false;

            if (m_Table == null)
                LoadTable();

            SpeedInfo sp = (SpeedInfo)m_Table[obj.GetType()];

            return (sp != null);
        }

        public static bool GetSpeeds(object obj, ref double activeSpeed, ref double passiveSpeed)
        {
            if (!Enabled)
                return false;

            if (m_Table == null)
                LoadTable();

            SpeedInfo sp = (SpeedInfo)m_Table[obj.GetType()];

            if (sp == null)
                return false;

            activeSpeed = sp.ActiveSpeed;
            passiveSpeed = sp.PassiveSpeed;

            return true;
        }

        private static void LoadTable()
        {
            m_Table = new Hashtable();

            for (int i = 0; i < m_Speeds.Length; ++i)
            {
                SpeedInfo info = m_Speeds[i];
                Type[] types = info.Types;

                for (int j = 0; j < types.Length; ++j)
                    m_Table[types[j]] = info;
            }
        }

        private static Hashtable m_Table;

        private static SpeedInfo[] m_Speeds = new SpeedInfo[]
			{
				
                /* Very Slow */
				new SpeedInfo( 0.8, 1.0, new Type[]
				{
					typeof( Zombie ),
                    typeof( Mummy ),
                    typeof( RottingCorpse ),
                    typeof( FailedExperiment )
				} ),

                /* Slow */
				new SpeedInfo( 0.6, 0.8, new Type[]
				{
					typeof( AntLion ),		
                    typeof( ArcticOgreLord ),
                    typeof( BogThing ),
					typeof( Bogle ),			
                    typeof( BoneKnight ),		
                    typeof( EarthElemental ),
					typeof( Ettin ),			
                    typeof( FrostOoze ),		
                    typeof( FrostTroll ),
					typeof( GazerLarva ),		
                    typeof( Ghoul ),		
                    typeof( Golem ),
					typeof( HeadlessOne ),		
                    typeof( Jwilson ),			
                    typeof( Juggernaut ),
					typeof( Ogre ),				
                    typeof( OgreLord ),			
                    typeof( PlagueBeast ),
					typeof( Quagmire ),			
                    typeof( Rat ),				
                    typeof( Beetle ),
					typeof( Sewerrat ),			
                    typeof( Skeleton ),			
                    typeof( Slime ),
					typeof( Walrus ),			
                    typeof( RestlessSoul ),
					typeof( Serado ),          
                    typeof( ValoriteElemental ),
                    typeof( VeriteElemental ), 
                    typeof( Titan ),			
                    typeof( Troll ),
                    typeof( AgapiteElemental ),
                    typeof( BronzeElemental ),
                    typeof( CopperElemental ),
                    typeof( DullCopperElemental ),
                    typeof( GoldenElemental ),
                    typeof( IceElemental ),
                    typeof( PlagueSpawn ),
                    typeof( ShadowIronElemental ),
                    typeof( SnowElemental ),
                    typeof( AngryGuest ),
                    typeof( RestlessStudent ),
                    typeof( SpectralStudent ),
                    typeof( FrozenElemental ),
                    typeof( Puddle ),
                    typeof( EvilMageRobed ),
                    typeof( DustElemental ),
                    typeof( EvilMageLordRobed ),
                    typeof( CrazedPeasant ),
                    typeof( CrazedFarmer ),
				} ),

                /* Medium */
				new SpeedInfo( 0.4, 0.6, new Type[]
				{
                    typeof( EnergyVortex ),
                    typeof( BladeSpirits ),
					typeof( ToxicElemental ),                    
					typeof( AncientLich ),	                    			
                    typeof( BlackSolenInfiltratorQueen ),
                    typeof( BlackSolenInfiltratorWarrior ),
					typeof( BlackSolenQueen ),	
                    typeof( BlackSolenWarrior ), 
                    typeof( BlackSolenWorker ),
					typeof( BloodElemental ),	
                    typeof( Boar ),				
                    typeof( Bogling ),
					typeof( BoneMagi ),	
                    typeof( Bull ),				
                    typeof( BullFrog ),									
                    typeof( Centaur ),			
                    typeof( ChaosDaemon ),
					typeof( Chicken ),			
                    typeof( GolemController ),
                    typeof( Cow ),
					typeof( Cyclops ),			
                    typeof( Daemon ),			
                    typeof( DeepSeaSerpent ),		
	                typeof( CrazedAlchemist),
                    typeof( ElderGazer ),
					typeof( EvilMage ),			
                    typeof( EvilMageLord ),		
                    typeof( Executioner ),								
                    typeof( FireElemental ),	
                    typeof( FireGargoyle ),					
					typeof( FrostSpider ),		
                    typeof( Gargoyle ),			
                    typeof( Gazer ),
					typeof( IceSerpent ),		
                    typeof( GiantRat ),			
                    typeof( GiantSerpent ),
                    typeof( EncagedGiantSerpent ),
					typeof( GiantSpider ),		
                    typeof( GiantToad ),
		            typeof( EncagedGiantToad ),	
                    typeof( Goat ),	
                    typeof( Guardian ),
					typeof( Harpy ),			
                    typeof( Harrower ),
                    typeof( HordeMinion ),	
                    typeof( IceFiend ),
					typeof( IceSnake ),			
                    typeof( Imp ),				
                    typeof( JackRabbit ),
					typeof( Kirin ),			
                    typeof( Kraken ),			
                    typeof( PredatorHellCat ),
					typeof( LavaLizard ),		
                    typeof( LavaSerpent ),		
                    typeof( LavaSnake ),
					typeof( Lizardman ),		
                    typeof( Llama ),
			        typeof( EncagedLlama ),
                    typeof( Mongbat ),
					typeof( StrongMongbat ),	
                    typeof( MountainGoat ),		
                    typeof( Orc ),
					typeof( OrcBomber ),		
                    typeof( OrcBrute ),		
                    typeof( OrcCaptain ),
					typeof( OrcishLord ),		
                    typeof( OrcishMage ),
                    typeof( AncientVampire ),
                    typeof( InfestedRat ),
                    typeof( LockmoorAlchemist ),
                    typeof( PeculiarOrc ),
                    typeof( LockmoorAlchemist ),
                    typeof( LockmoorArcher ),
                    typeof( LockmoorButcher ),
                    typeof( LockmoorGuard ),
                    typeof( LockmoorMage ),
                    typeof( LockmoorNoble ),
                    typeof( LockmoorRoyalty ),
                    typeof( OrcArcher ),
                    typeof( Torturer  ),
                    typeof( Vampire ),
                    typeof( VampireOutcast ),
                    typeof( Pig ),
					typeof( Ratman ),			
                    typeof( RatmanArcher ),		
                    typeof( RatmanMage ),
					typeof( RedSolenInfiltratorQueen ), 
                    typeof( RedSolenInfiltratorWarrior ), 
                    typeof( RedSolenQueen ),
					typeof( RedSolenWarrior ),	
                    typeof( RedSolenWorker ),
                    typeof( Scorpion ),	
		            typeof( EncagedScorpion ),		
                    typeof( SeaSerpent ),
					typeof( SerpentineDragon ),
                    typeof( Shade ),
					typeof( ShadowWisp ),		
                    typeof( ShadowWyrm ),		
                    typeof( Sheep ),	
					typeof( BlackSheep ),	
                    typeof( SkeletalDragon ),	
                    typeof( SkeletalMage ),
					typeof( Snake ),
					typeof( SpectralArmour ),	
                    typeof( Spectre ),
					typeof( StoneGargoyle ),	
                    typeof( StoneHarpy ),		
                    typeof( SwampDragon ),
					typeof( ScaledSwampDragon ),
                    typeof( SwampTentacle ),	
                    typeof( TerathanAvenger ),
					typeof( TerathanDrone ),	
                    typeof( TerathanMatriarch ), 
                    typeof( TerathanWarrior ),
					typeof( WaterElemental ),
                    typeof( WhippingVine ),
					typeof( Wraith ),			
					typeof( KhaldunZealot ),	
                    typeof( KhaldunSummoner ),	
					typeof( LichLord ),			
                    typeof( SkeletalKnight ),	
                    typeof( SummonedDaemon ),
					typeof( SummonedEarthElemental ),
                    typeof( SummonedWaterElemental ),
                    typeof( SummonedFireElemental ),
					typeof( MeerWarrior ),	
                    typeof( MeerEternal ),		
                    typeof( MeerMage ),
					typeof( MeerCaptain ),	
                    typeof( JukaLord ),			
                    typeof( JukaMage ),
					typeof( JukaWarrior ),	
                    typeof( Cursed ),			
                    typeof( GrimmochDrummel ),
					typeof( LysanderGathenwale ),
                    typeof( MorgBergen ),	
                    typeof( ShadowFiend ),
					typeof( SpectralArmour ),
                    typeof( TavaraSewel ),	
                    typeof( ArcaneDaemon ),
					typeof( Doppleganger ),	
                    typeof( EnslavedGargoyle ),
                    typeof( ExodusMinion ),
					typeof( ExodusOverseer ),
                    typeof( GargoyleDestroyer ),
                    typeof( GargoyleEnforcer ),
					typeof( Moloch ),
                    typeof( Gaman ),			
			        typeof( Lich ),
                    typeof( OphidianArchmage ),
					typeof( OphidianMage ),		
                    typeof( OphidianWarrior ),	
                    typeof( OphidianMatriarch ),
					typeof( OphidianKnight ),
	                typeof( OphidianQueen ),
					typeof( OphidianDestroyer ),
                    typeof( PoisonElemental ),	
                    typeof( SandVortex ),	
                    typeof( Leviathan ),
		            typeof( DreadSpider ),		
                    typeof( Efreet ),
                    typeof( LordOaks ),			
                    typeof( Silvani ),
	                typeof( SilverSerpent ),                    
                    typeof( EvilButcher ),
                    typeof( HallMonitor ),
                    typeof( Librarian ),
                    typeof( StorageImp ),
                    typeof( GargoyleAugurer ),
                    typeof( GargoyleConjurer ),
                    typeof( GargoyleElder ),
                    typeof( EliteGargoyleElder ),
                    typeof( GargoyleWarrior ),
                    typeof( GargoyleYoung ),
                    typeof( SandVortex ),
                    typeof( ElderWaterElemental ),
                    typeof( SteamElemental ),
                    typeof( SanguinConscript ),
                    typeof( SanguinDefender ),   
                    typeof( SanguinKnight ),
                    typeof( SanguinMage ),
                    typeof( SanguinProtector ),  
                    typeof( SanguinWizard ),
                    typeof( SanguinMedic ),
                    typeof( SanguinMender ),
                    typeof( SanguinScout ),
                    typeof( SanguinAssassin ),
                    typeof( EvilAlchemist ),
                    typeof( Savage ),
                    typeof( Brigand ), 
                    typeof( Pirate ), 
                    typeof( SavageShaman ),
                    typeof( CrazedAdventurer ),
                    typeof( CrazedLumberjack ),
                    typeof( CrazedMiner ),
                    typeof( WaterNymph ),
                    typeof( WaterSiren ),
                    typeof( WaterNymphWarrior ),
                    typeof( WaterSirenWarrior ),
                    typeof( GargoyleRector ),
                    typeof( GargoyleYoungWarrior ),
                    typeof( GargoyleAncient ),
                    typeof( BridgeEttin ),
                    typeof( BridgeLockmoorArcher  ),
                    typeof( BridgeLockmoorGuard  ),
                    typeof( BridgeMage ),
                    typeof( BridgeOrcArcher ),
                    typeof( BridgeOrcCaptain  ),
                    typeof( BridgeOrcishMage  ),
                    typeof( EliteGargoyleAncient ),
                    typeof( BearDruid ),
                    typeof( BearformDruid ),
                    typeof( DeerDruid ),
                    typeof( DeerformDruid ),
                    typeof( RandomChild),
				} ),  
            
                /* Fast */
				new SpeedInfo( 0.3, 0.4, new Type[]
				{                     
                    typeof( SummonedAirElemental ),
                    typeof( GargoyleTaskmaster ),
                    typeof( AirElemental ),	
                    typeof( FuriousHallMonitor ),
                    typeof( BlackBear ),
                    typeof( Alligator ),
                    typeof( EncagedAlligator ),
                    typeof( BrownBear ),
                    typeof( Gorilla ),
                    typeof( EncagedGorilla ),
                    typeof( WhiteWyrm ),
		            typeof( GiantBlackWidow ),
                    typeof( AncientWyrm ),		
                    typeof( Balron ),	
                    typeof( Dragon ),	
                    typeof( Wyvern ),
                    typeof( Drake ),
		            typeof( BabyWaterWyrm ),
		            typeof( WaterWyrm ), 
                    typeof( SavageRider ),
				} ),	

				/* Very Fast */
				new SpeedInfo( 0.21, 0.40, new Type[]
				{                   
                    typeof( Cougar ),
                    typeof( Cat ),
                    typeof( DireWolf ),	
		            typeof( EncagedDireWolf ),	
                    typeof( Dog ),
                    typeof( Dolphin ),
	
                    typeof( DesertOstard ), 
                    typeof( FireSteed ),		
                    typeof( ForestOstard ),		
                    typeof( FrenziedOstard ),
                    typeof( GreatHart ),
					typeof( GreyWolf ),	
                    typeof( HellHound ),                    
                    typeof( BloodthirstyVampire ),
					typeof( Hind ),	
                    typeof( Horse ),					
                    typeof( PackHorse ),
					typeof( PackLlama ),
                    typeof( Panther ),                    	
                    typeof( Rabbit ),
                    typeof( EncagedRabbit ),
                    typeof( RidableLlama ),
					typeof( Ridgeback ),
                    typeof( SilverSteed ),
                    typeof( Bird ),
                    typeof( Crane ),
                    typeof( SavageRidgeback ),
                    typeof( WhiteWolf ),
                    typeof( TBWarHorse ),
                    typeof( CoMWarHorse ),
                    typeof( MinaxWarHorse ),
                    typeof( SLWarHorse ),
                    typeof( Unicorn ),
                    typeof( TimberWolf ),
                    typeof( EncagedTimberWolf ),
                    typeof( SnowLeopard ),
                    typeof( HellCat ),
                    typeof( SkeletalMount ),
                    typeof( EtherealWarrior ),
					typeof( Nightmare ),
                    typeof( Wisp ),
					typeof( LesserHiryu ),	
                    typeof( GrizzlyBear ),
                    typeof( EncagedGrizzlyBear ),
                    typeof( PolarBear ),
                    typeof( EncagedPolarBear ),
                    typeof( Hiryu ),                    
                    typeof( FactionKnight ),    
                    typeof( FactionHenchman ),	
                    typeof( FactionBerserker ),
                    typeof( FactionMercenary ),	
                    typeof( FactionSorceress ),	
                    typeof( FactionWizard ),
                    typeof( WanderingHealer )                   
				} ),

				/* Ultra */
				new SpeedInfo( 0.175, 0.4, new Type[]
				{
					typeof( Barracoon ),		
                    typeof( Mephitis ),			
                    typeof( Neira ),
					typeof( Rikktor ),			
                    typeof( Semidar ),
					typeof( Pixie ),
					typeof( VorpalBunny ),  	
                    typeof( KhaldunRevenant )					
				} ),
			
                /* Guard */
				new SpeedInfo( 0.25, 0.8, new Type[]
				{
					typeof( DungeonGuardMelee ), 
  				    typeof( DungeonGuardRanged ) 
				} )	
			};
    }
}