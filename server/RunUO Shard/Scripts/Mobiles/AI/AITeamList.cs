using System;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Factions;
using Server.Scripts;
using System.Collections.Generic;
using System.IO;

namespace Server
{
    public class AITeamList
    {
        public const ulong CustomTeamFlags = 0x7FEL; // "and" this a mobile's teamflags to determine whether there are any custom teams (this is based on the teamflags,Team1 = 0x2, Team10 = 0x400, so the all custom team bit flags including those are 11111111110, which is 0x7FE in hexadecimal

        public enum NotoTypeEnum : int
        {
            ALLIES_GREEN = 0,
            ALLIES_NORMAL_IGNORE_MILITIA = 1,
            ALLIES_NORMAL_ALLOW_HEAL_MILITIA = 2,
            ALLIES_NORMAL_NO_HEAL_MILITIA = 3
        }
        public static bool TeamHarm = true;

        public static NotoTypeEnum NotoType = NotoTypeEnum.ALLIES_NORMAL_IGNORE_MILITIA; 
                            //0 = all allies are green, all enemies are orange
                            //1 = all allies are blue/grey/red but enemy militia are not orange, all enemies are orange
                            //2 = all allies standard noto, all enemies orange, can heal allies who are in enemy militia
                            //3 = all allies standard noto, all enemies orange, can't heal allies who are in enemy militia
        
        
        public static void LogWarning(Type npctype)
        {
            using (StreamWriter writer = new StreamWriter("AITeamListWarnings.txt", true))
            {
                string message = "The type " + npctype + " is not defined in the AITeamList.TeamFlagDict, therefore it is assigned Player AND Monster flags (they won't attack either).  Please go define it in AITeamList.TeamFlagDict!" + DateTime.Now;
                writer.WriteLine(message);
            }
        }
        
        private Type[] m_Types;

        public Type[] Types
        {
            get { return m_Types; }
            set { m_Types = value; }
        }

        public AITeamList(Type[] types)
        {
            m_Types = types;
        }

        public static bool CheckSameTeam(Mobile from, Mobile to)
        {
            return (from.TeamFlags & to.TeamFlags) > 1L; // 1L is player team
        }

        public static bool CheckEnemyTeam(Mobile from, Mobile to)
        {
            return from.TeamFlags > 1L && to.TeamFlags > 1L && !CheckSameTeam(from, to); // 1L is player team
        }

        public static bool CheckTeamOld(object objFrom, object objTarget) // old, non-TeamFlags method
        {
            Type fromType = objFrom.GetType();
            Type targetType = objTarget.GetType();

            //Both the Same Type: Automatically Teammates
            if (fromType == targetType)
                return true;

            bool foundFrom = false;
            bool foundTarget = false;

            //For Each Team in List: Except Last Team (Which is Independents)
            for (int i = 0; i < (m_Teams.Length - 1); ++i)
            {
                AITeamList info = m_Teams[i];
                Type[] types = info.Types;

                foundFrom = false;
                foundTarget = false;

                //Check If From is In Current Team
                for (int j = 0; j < types.Length; ++j)
                {
                    if (fromType == types[j])
                    {
                        foundFrom = true;
                        break;
                    }
                }

                //Check If Target Is Also in Current Team
                if (foundFrom)
                {
                    for (int j = 0; j < types.Length; ++j)
                    {
                        if (targetType == types[j])
                        {
                            foundTarget = true;
                            break;
                        }
                    }
                }

                //Both From and Target in Same List (They are Teammates)
                if (foundFrom && foundTarget)
                    return true;
            }

            return false;
        }

        public enum TeamFlags : ulong
        {
            None = 0L,
            Player =                0x0000000000000001L,
            Monster =               0x8000000000000000L,
            
            Team1 =                 0x0000000000000002L,
            Team2 =                 0x0000000000000004L,
            Team3 =                 0x0000000000000008L,
            Team4 =                 0x0000000000000010L,
            Team5 =                 0x0000000000000020L,
            Team6 =                 0x0000000000000040L,
            Team7 =                 0x0000000000000080L,
            Team8 =                 0x0000000000000100L,
            Team9 =                 0x0000000000000200L,
            Team10 =                0x0000000000000400L,
            Beetles =               0x0000000000000800L,
            Daemons =               0x0000000000001000L,
            Dragons =               0x0000000000002000L,
            Gargoyles =             0x0000000000004000L,
            Gazers =                0x0000000000008000L,
            OgresTrollsEttins =     0x0000000000010000L,
            Orcs =                  0x0000000000020000L,
            Rats =                  0x0000000000040000L,
            Slimes =                0x0000000000080000L,
            Edgewich =              0x0000000000100000L,
            Elementals =            0x0000000000200000L,
            ElementalAir =          0x0000000000400000L,
            ElementalEarth =        0x0000000000800000L,
            ElementalFire =         0x0000000001000000L,
            ElementalWater =        0x0000000002000000L,
            ElementalMisc =         0x0000000004000000L,
            EvilMage =              0x0000000008000000L,
            Faction =               0x0000000010000000L,
            Frogs =                 0x0000000020000000L,
            Giants =                0x0000000040000000L,
            Golem =                 0x0000000080000000L,
            Guard =                 0x0000000100000000L,
            Harpy =                 0x0000000200000000L,
            Hiryu =                 0x0000000400000000L,
            Imps =                  0x0000000800000000L,
            Juka =                  0x0000001000000000L,
            Khaldun =               0x0000002000000000L,
            Lockmoor =              0x0000004000000000L,
            Meer =                  0x0000008000000000L,
            Ophidians =             0x0000010000000000L,
            Plants =                0x0000020000000000L,
            Pulma =                 0x0000040000000000L,
            Ratman =                0x0000080000000000L,
            Sanguin =               0x0000100000000000L,
            Savage =                0x0000200000000000L,
            SeaMonster =            0x0000400000000000L,
            Snakes =                0x0000800000000000L,
            SolenBlack =            0x0001000000000000L,
            SolenRed =              0x0002000000000000L,
            Spiders =               0x0004000000000000L,
            SummonedDrones =        0x0008000000000000L,
            Terathans =             0x0010000000000000L,
            Undead =                0x0020000000000000L,
            Wisp =                  0x0040000000000000L,
            KinOrcs =               0x0080000000000000L,
            KinDrow =               0x0100000000000000L,
            KinUndead =             0x0200000000000000L,
            KinSavage =             0x0400000000000000L
        }
        
        public static Dictionary<Type, ulong> TeamFlagDict = new Dictionary<Type, ulong>()
        {
	     //Undead
            { typeof( Zombie ), (ulong)TeamFlags.Undead + (ulong)TeamFlags.KinUndead + (ulong)TeamFlags.Monster},
            { typeof( Mummy ), (ulong)TeamFlags.Undead + (ulong)TeamFlags.Monster },
            { typeof( RottingCorpse ), (ulong)TeamFlags.Undead + (ulong)TeamFlags.Monster},
            { typeof( BoneKnight ), (ulong)TeamFlags.Undead + (ulong)TeamFlags.Monster},
	        { typeof( Ghoul ), (ulong)TeamFlags.Undead + (ulong)TeamFlags.KinUndead + (ulong)TeamFlags.Monster},
            { typeof( Skeleton ), (ulong)TeamFlags.Undead + (ulong)TeamFlags.KinUndead + (ulong)TeamFlags.Monster},
            { typeof( RestlessSoul ), (ulong)TeamFlags.Undead + (ulong)TeamFlags.Monster},
            { typeof( AncientLich ), (ulong)TeamFlags.Undead + (ulong)TeamFlags.Monster},
            { typeof( BoneMagi ), (ulong)TeamFlags.Undead + (ulong)TeamFlags.Monster},
            { typeof( SkeletalDragon ), (ulong)TeamFlags.Undead + (ulong)TeamFlags.Dragons + (ulong)TeamFlags.Monster},
            { typeof( SkeletalMage ), (ulong)TeamFlags.Undead + (ulong)TeamFlags.Monster},
            { typeof( Shade ), (ulong)TeamFlags.Undead + (ulong)TeamFlags.Monster},
            { typeof( Wraith ), (ulong)TeamFlags.Undead + (ulong)TeamFlags.KinUndead + (ulong)TeamFlags.Monster},
			{ typeof( LichLord ), (ulong)TeamFlags.Undead + (ulong)TeamFlags.Monster},			
            { typeof( SkeletalKnight), (ulong)TeamFlags.Undead + (ulong)TeamFlags.Monster},
	        { typeof( Cursed ), (ulong)TeamFlags.Undead + (ulong)TeamFlags.Monster},	
            { typeof( SpectralArmour), (ulong)TeamFlags.Undead + (ulong)TeamFlags.Monster},
            { typeof( Lich ), (ulong)TeamFlags.Undead + (ulong)TeamFlags.Monster},
            { typeof( AngryGuest ), (ulong)TeamFlags.Undead + (ulong)TeamFlags.Edgewich + (ulong)TeamFlags.Monster},
            { typeof( RestlessStudent ), (ulong)TeamFlags.Undead + (ulong)TeamFlags.Edgewich + (ulong)TeamFlags.Monster},
            { typeof( SpectralStudent ), (ulong)TeamFlags.Undead + (ulong)TeamFlags.Edgewich + (ulong)TeamFlags.Monster},
            { typeof( SkeletalMount ), (ulong)TeamFlags.Undead + (ulong)TeamFlags.Monster},
            { typeof( Bogle ), (ulong)TeamFlags.Undead + (ulong)TeamFlags.KinUndead + (ulong)TeamFlags.Monster},
            { typeof( Spectre ),  (ulong)TeamFlags.Undead + (ulong)TeamFlags.Monster },

        //OgreEttinTroll
            { typeof( Ogre ), (ulong)TeamFlags.OgresTrollsEttins + (ulong)TeamFlags.Monster},				
            { typeof( OgreLord ), (ulong)TeamFlags.OgresTrollsEttins + (ulong)TeamFlags.Monster},	
            { typeof( ArcticOgreLord ), (ulong)TeamFlags.OgresTrollsEttins + (ulong)TeamFlags.Monster},
            { typeof( Ettin ), (ulong)TeamFlags.OgresTrollsEttins + (ulong)TeamFlags.Monster},
	        { typeof( FrostTroll ), (ulong)TeamFlags.OgresTrollsEttins + (ulong)TeamFlags.Monster},
            { typeof( Troll ), (ulong)TeamFlags.OgresTrollsEttins + (ulong)TeamFlags.Monster},
        //Orc
            { typeof( Orc ), (ulong)TeamFlags.Orcs + (ulong)TeamFlags.KinOrcs + (ulong)TeamFlags.Monster},
			{ typeof( OrcBomber ), (ulong)TeamFlags.Orcs + (ulong)TeamFlags.KinOrcs + (ulong)TeamFlags.Monster},		
            { typeof( OrcBrute ), (ulong)TeamFlags.Orcs + (ulong)TeamFlags.KinOrcs + (ulong)TeamFlags.Monster},		
            { typeof( OrcCaptain ), (ulong)TeamFlags.Orcs + (ulong)TeamFlags.KinOrcs + (ulong)TeamFlags.Monster},
			{ typeof( OrcishLord ), (ulong)TeamFlags.Orcs + (ulong)TeamFlags.KinOrcs + (ulong)TeamFlags.Monster},		
            { typeof( OrcishMage ), (ulong)TeamFlags.Orcs + (ulong)TeamFlags.KinOrcs + (ulong)TeamFlags.Monster},
            { typeof( OrcArcher ), (ulong)TeamFlags.Orcs + (ulong)TeamFlags.KinOrcs + (ulong)TeamFlags.Monster},
            { typeof( BridgeEttin ), (ulong)TeamFlags.Orcs + (ulong)TeamFlags.KinOrcs + (ulong)TeamFlags.Monster},
            { typeof( BridgeOrcArcher ), (ulong)TeamFlags.Orcs + (ulong)TeamFlags.KinOrcs + (ulong)TeamFlags.Monster},
            { typeof( BridgeOrcCaptain ), (ulong)TeamFlags.Orcs + (ulong)TeamFlags.KinOrcs + (ulong)TeamFlags.Monster},
            { typeof( BridgeOrcishMage ), (ulong)TeamFlags.Orcs + (ulong)TeamFlags.KinOrcs + (ulong)TeamFlags.Monster},
            { typeof( PeculiarOrc ), (ulong)TeamFlags.Orcs + (ulong)TeamFlags.KinOrcs + (ulong)TeamFlags.Monster},
            
        //Gazers
            { typeof( GazerLarva ), (ulong)TeamFlags.Gazers + (ulong)TeamFlags.Monster},
            { typeof( Gazer ), (ulong)TeamFlags.Gazers + (ulong)TeamFlags.Monster},
            { typeof( ElderGazer ), (ulong)TeamFlags.Gazers + (ulong)TeamFlags.Monster},               
        //Rats
            { typeof( Rat ), (ulong)TeamFlags.Player + (ulong)TeamFlags.Rats + (ulong)TeamFlags.Monster},
	        { typeof( Sewerrat ), (ulong)TeamFlags.Rats + (ulong)TeamFlags.Monster},
	        { typeof( GiantRat ), (ulong)TeamFlags.Rats + (ulong)TeamFlags.Monster},		

        //Slimes
            { typeof( FrostOoze ), (ulong)TeamFlags.Slimes + (ulong)TeamFlags.Monster},
            { typeof( Jwilson ), (ulong)TeamFlags.Slimes + (ulong)TeamFlags.Monster},	
            { typeof( Slime ), (ulong)TeamFlags.Slimes + (ulong)TeamFlags.Monster},
            { typeof( Puddle ), (ulong)TeamFlags.Slimes + (ulong)TeamFlags.Pulma + (ulong)TeamFlags.Monster},

        //Misc Elementals		
            { typeof( IceElemental ), (ulong)TeamFlags.ElementalMisc + (ulong)TeamFlags.Monster},
            { typeof( SnowElemental ), (ulong)TeamFlags.ElementalMisc + (ulong)TeamFlags.Monster},
            { typeof( FrozenElemental ), (ulong)TeamFlags.ElementalMisc + (ulong)TeamFlags.Pulma + (ulong)TeamFlags.Monster},

        //Golem
            { typeof( GolemController ), (ulong)TeamFlags.Golem + (ulong)TeamFlags.Monster},
            { typeof( Golem ), (ulong)TeamFlags.Golem + (ulong)TeamFlags.Monster},

        //Earth Elementals 
            { typeof( DustElemental ), (ulong)TeamFlags.ElementalEarth + (ulong)TeamFlags.Monster},  
            { typeof( EarthElemental ), (ulong)TeamFlags.ElementalEarth + (ulong)TeamFlags.Monster},               
            { typeof( ValoriteElemental ), (ulong)TeamFlags.ElementalEarth + (ulong)TeamFlags.Monster},
            { typeof( VeriteElemental ), (ulong)TeamFlags.ElementalEarth + (ulong)TeamFlags.Monster}, 
            { typeof( AgapiteElemental ), (ulong)TeamFlags.ElementalEarth + (ulong)TeamFlags.Monster},
            { typeof( BronzeElemental ), (ulong)TeamFlags.ElementalEarth + (ulong)TeamFlags.Monster},
            { typeof( CopperElemental ), (ulong)TeamFlags.ElementalEarth + (ulong)TeamFlags.Monster},
            { typeof( DullCopperElemental ), (ulong)TeamFlags.ElementalEarth + (ulong)TeamFlags.Monster},
            { typeof( GoldenElemental ), (ulong)TeamFlags.ElementalEarth + (ulong)TeamFlags.Monster},
            { typeof( ShadowIronElemental ), (ulong)TeamFlags.ElementalEarth + (ulong)TeamFlags.Monster},
            
        //Evil Mage
            { typeof( EvilMageRobed ), (ulong)TeamFlags.EvilMage + (ulong)TeamFlags.Monster},
            { typeof( EvilMage ), (ulong)TeamFlags.EvilMage + (ulong)TeamFlags.Monster},			
            { typeof( EvilMageLord ), (ulong)TeamFlags.EvilMage + (ulong)TeamFlags.Monster},	
            { typeof( EvilMageLordRobed ), (ulong)TeamFlags.EvilMage + (ulong)TeamFlags.Monster},

        //Black Solen
            { typeof( BlackSolenInfiltratorQueen ), (ulong)TeamFlags.SolenBlack + (ulong)TeamFlags.Monster},
            { typeof( BlackSolenInfiltratorWarrior ), (ulong)TeamFlags.SolenBlack + (ulong)TeamFlags.Monster},
			{ typeof( BlackSolenQueen ), (ulong)TeamFlags.SolenBlack + (ulong)TeamFlags.Monster},	
            { typeof( BlackSolenWarrior ), (ulong)TeamFlags.SolenBlack + (ulong)TeamFlags.Monster}, 
            { typeof( BlackSolenWorker ), (ulong)TeamFlags.SolenBlack + (ulong)TeamFlags.Monster},

        //Red Solen
            { typeof( RedSolenInfiltratorQueen ), (ulong)TeamFlags.SolenRed + (ulong)TeamFlags.Monster}, 
            { typeof( RedSolenInfiltratorWarrior ), (ulong)TeamFlags.SolenRed + (ulong)TeamFlags.Monster}, 
            { typeof( RedSolenQueen ), (ulong)TeamFlags.SolenRed + (ulong)TeamFlags.Monster},
			{ typeof( RedSolenWarrior ), (ulong)TeamFlags.SolenRed + (ulong)TeamFlags.Monster},	
            { typeof( RedSolenWorker ), (ulong)TeamFlags.SolenRed + (ulong)TeamFlags.Monster},

        //Plants
            { typeof( BogThing ), (ulong)TeamFlags.Plants + (ulong)TeamFlags.Monster},				
            { typeof( Bogling ), (ulong)TeamFlags.Plants + (ulong)TeamFlags.Monster},
            { typeof( CapturedHordeMinion ), (ulong)TeamFlags.Plants + (ulong)TeamFlags.Monster},
            { typeof( PlagueBeast ), (ulong)TeamFlags.Plants + (ulong)TeamFlags.Monster},
            { typeof( Quagmire ), (ulong)TeamFlags.Plants + (ulong)TeamFlags.Monster},	
            { typeof( PlagueSpawn ), (ulong)TeamFlags.Plants + (ulong)TeamFlags.Monster},
            { typeof( SwampTentacle ), (ulong)TeamFlags.Plants + (ulong)TeamFlags.Monster},
	        { typeof( WhippingVine ), (ulong)TeamFlags.Plants + (ulong)TeamFlags.Monster},
            { typeof( Corpser ), (ulong)TeamFlags.Plants + (ulong)TeamFlags.Monster},
	        { typeof( Reaper ), (ulong)TeamFlags.Plants + (ulong)TeamFlags.Monster},

        //Beetles
            { typeof( AntLion ), (ulong)TeamFlags.Beetles + (ulong)TeamFlags.Monster}, 
            { typeof( Beetle ), (ulong)TeamFlags.Beetles + (ulong)TeamFlags.Monster},  

        //SummonedDrones
            { typeof( EnergyVortex ), (ulong)TeamFlags.SummonedDrones},
            { typeof( BladeSpirits ), (ulong)TeamFlags.SummonedDrones},

        //SummonedElementals/deamon
            { typeof( SummonedAirElemental), (ulong)TeamFlags.Player },
            { typeof( SummonedDaemon ), (ulong)TeamFlags.Player },
            { typeof( SummonedEarthElemental ), (ulong)TeamFlags.Player },
            { typeof( SummonedFireElemental ), (ulong)TeamFlags.Player },
            { typeof( SummonedWaterElemental ), (ulong)TeamFlags.Player },

        //Spiders
            { typeof( FrostSpider ), (ulong)TeamFlags.Spiders + (ulong)TeamFlags.Monster},	
            { typeof( GiantSpider ), (ulong)TeamFlags.Spiders + (ulong)TeamFlags.KinDrow + (ulong)TeamFlags.Monster},
		    { typeof( GiantBlackWidow ), (ulong)TeamFlags.Spiders + (ulong)TeamFlags.Monster},                    	
		    { typeof( DreadSpider ), (ulong)TeamFlags.Spiders + (ulong)TeamFlags.Monster},	
              
        //Gargoyles
            { typeof( FireGargoyle ), (ulong)TeamFlags.Gargoyles + (ulong)TeamFlags.Monster},
            { typeof( Gargoyle ), (ulong)TeamFlags.Gargoyles + (ulong)TeamFlags.Monster},
	        { typeof( EnslavedGargoyle ), (ulong)TeamFlags.Gargoyles + (ulong)TeamFlags.Monster},
            { typeof( StoneGargoyle ), (ulong)TeamFlags.Gargoyles + (ulong)TeamFlags.Monster},
            { typeof( GargoyleDestroyer ), (ulong)TeamFlags.Gargoyles + (ulong)TeamFlags.Monster},
            { typeof( GargoyleEnforcer ), (ulong)TeamFlags.Gargoyles + (ulong)TeamFlags.Monster},
            { typeof( GargoyleAugurer ), (ulong)TeamFlags.Gargoyles + (ulong)TeamFlags.Monster},
            { typeof( GargoyleConjurer ), (ulong)TeamFlags.Gargoyles + (ulong)TeamFlags.Monster},
            { typeof( GargoyleElder ), (ulong)TeamFlags.Gargoyles + (ulong)TeamFlags.Monster},
            { typeof( EliteGargoyleElder ), (ulong)TeamFlags.Gargoyles + (ulong)TeamFlags.Monster},
            { typeof( GargoyleWarrior ), (ulong)TeamFlags.Gargoyles + (ulong)TeamFlags.Monster},
            { typeof( GargoyleYoung ), (ulong)TeamFlags.Gargoyles + (ulong)TeamFlags.Monster},
            { typeof( GargoyleTaskmaster ), (ulong)TeamFlags.Gargoyles + (ulong)TeamFlags.Monster},
            { typeof( GargoyleRector ), (ulong)TeamFlags.Gargoyles + (ulong)TeamFlags.Monster},
            { typeof( GargoyleYoungWarrior ), (ulong)TeamFlags.Gargoyles + (ulong)TeamFlags.Monster},
            { typeof( GargoyleAncient ), (ulong)TeamFlags.Gargoyles + (ulong)TeamFlags.Monster},
            { typeof( EliteGargoyleAncient ), (ulong)TeamFlags.Gargoyles + (ulong)TeamFlags.Monster},
            
        //Snakes
            { typeof( IceSerpent ), (ulong)TeamFlags.Snakes + (ulong)TeamFlags.Monster},
	        { typeof( GiantSerpent ), (ulong)TeamFlags.Snakes + (ulong)TeamFlags.Monster},
            { typeof( IceSnake ), (ulong)TeamFlags.Snakes + (ulong)TeamFlags.Monster},
	        { typeof( LavaSerpent ), (ulong)TeamFlags.Snakes + (ulong)TeamFlags.Monster},
            { typeof( LavaSnake ), (ulong)TeamFlags.Snakes + (ulong)TeamFlags.Monster},
            { typeof( Snake ), (ulong)TeamFlags.Player + (ulong)TeamFlags.Snakes + (ulong)TeamFlags.Monster},
            { typeof( SilverSerpent ), (ulong)TeamFlags.Snakes + (ulong)TeamFlags.Monster}, 

        //Frogs
            { typeof( BullFrog ), (ulong)TeamFlags.Frogs + (ulong)TeamFlags.Monster },
            { typeof( GiantToad ), (ulong)TeamFlags.Frogs + (ulong)TeamFlags.Monster},

        //Ratman
            { typeof( Ratman ), (ulong)TeamFlags.Ratman + (ulong)TeamFlags.Monster},			
            { typeof( RatmanArcher ), (ulong)TeamFlags.Ratman + (ulong)TeamFlags.Monster},		
            { typeof( RatmanMage ), (ulong)TeamFlags.Ratman + (ulong)TeamFlags.Monster},

        //Giants
            { typeof( Titan ), (ulong)TeamFlags.Giants + (ulong)TeamFlags.Monster}, 
	        { typeof( Cyclops ), (ulong)TeamFlags.Giants + (ulong)TeamFlags.Monster}, 	

        //Lockmoor
            { typeof( AncientVampire ), (ulong)TeamFlags.Lockmoor + (ulong)TeamFlags.Monster},
            { typeof( InfestedRat ), (ulong)TeamFlags.Lockmoor + (ulong)TeamFlags.Monster},
            { typeof( LockmoorAlchemist ), (ulong)TeamFlags.Lockmoor + (ulong)TeamFlags.Monster},
            { typeof( LockmoorArcher ), (ulong)TeamFlags.Lockmoor + (ulong)TeamFlags.Monster},
            { typeof( LockmoorButcher ), (ulong)TeamFlags.Lockmoor + (ulong)TeamFlags.Monster},
            { typeof( LockmoorGuard ), (ulong)TeamFlags.Lockmoor + (ulong)TeamFlags.Monster},
            { typeof( LockmoorMage ), (ulong)TeamFlags.Lockmoor + (ulong)TeamFlags.Monster},
            { typeof( LockmoorNoble ), (ulong)TeamFlags.Lockmoor + (ulong)TeamFlags.Monster},
            { typeof( LockmoorRoyalty ), (ulong)TeamFlags.Lockmoor + (ulong)TeamFlags.Monster},
            { typeof( BloodthirstyVampire ), (ulong)TeamFlags.Lockmoor + (ulong)TeamFlags.Monster},
            { typeof( Torturer  ), (ulong)TeamFlags.Lockmoor + (ulong)TeamFlags.Monster},
            { typeof( Vampire ), (ulong)TeamFlags.Lockmoor + (ulong)TeamFlags.Monster},
            { typeof( BridgeLockmoorArcher ), (ulong)TeamFlags.Lockmoor + (ulong)TeamFlags.Monster},
            { typeof( BridgeLockmoorGuard ), (ulong)TeamFlags.Lockmoor + (ulong)TeamFlags.Monster},
            { typeof( BridgeMage ), (ulong)TeamFlags.Lockmoor + (ulong)TeamFlags.Monster},
            { typeof( VampireOutcast ), (ulong)TeamFlags.Lockmoor + (ulong)TeamFlags.Monster},


        //Daemons
            { typeof( ChaosDaemon ), (ulong)TeamFlags.Daemons + (ulong)TeamFlags.Monster},
            { typeof( Daemon ), (ulong)TeamFlags.Daemons + (ulong)TeamFlags.Monster},
            { typeof( IceFiend ), (ulong)TeamFlags.Daemons + (ulong)TeamFlags.Monster},
	        { typeof( Balron ), (ulong)TeamFlags.Daemons + (ulong)TeamFlags.Monster},
            { typeof( ArcaneDaemon ), (ulong)TeamFlags.Daemons + (ulong)TeamFlags.Monster},
            { typeof( Moloch ), (ulong)TeamFlags.Daemons + (ulong)TeamFlags.Monster},                

        //Dragons
            { typeof( Dragon ), (ulong)TeamFlags.Dragons + (ulong)TeamFlags.Monster},			
            { typeof( Drake ), (ulong)TeamFlags.Dragons + (ulong)TeamFlags.Monster},
            { typeof( ShadowWyrm ), (ulong)TeamFlags.Dragons + (ulong)TeamFlags.Monster},
	        { typeof( SerpentineDragon ), (ulong)TeamFlags.Dragons + (ulong)TeamFlags.Monster},   
            //{ typeof( SkeletalDragon ), (ulong)TeamFlags.Dragons + (ulong)TeamFlags.Monster},	
            { typeof( SwampDragon ), (ulong)TeamFlags.Dragons + (ulong)TeamFlags.Monster},
			{ typeof( ScaledSwampDragon ), (ulong)TeamFlags.Dragons + (ulong)TeamFlags.Monster},								
            { typeof( Wyvern ), (ulong)TeamFlags.Dragons + (ulong)TeamFlags.Monster},
            { typeof( WhiteWyrm ), (ulong)TeamFlags.Dragons + (ulong)TeamFlags.Monster},
            { typeof( AncientWyrm ), (ulong)TeamFlags.Dragons + (ulong)TeamFlags.Monster},
            { typeof( BabyWaterWyrm ), (ulong)TeamFlags.Dragons + (ulong)TeamFlags.Pulma + (ulong)TeamFlags.Monster},
			{ typeof( WaterWyrm ), (ulong)TeamFlags.Dragons + (ulong)TeamFlags.Pulma + (ulong)TeamFlags.Monster},

        //Terathans
            { typeof( TerathanAvenger ), (ulong)TeamFlags.Terathans + (ulong)TeamFlags.Monster},
			{ typeof( TerathanDrone ), (ulong)TeamFlags.Terathans + (ulong)TeamFlags.Monster},	
            { typeof( TerathanMatriarch ), (ulong)TeamFlags.Terathans + (ulong)TeamFlags.Monster}, 
            { typeof( TerathanWarrior ), (ulong)TeamFlags.Terathans + (ulong)TeamFlags.Monster},

        //Ophidians
            { typeof( OphidianArchmage ), (ulong)TeamFlags.Ophidians + (ulong)TeamFlags.Monster},
			{ typeof( OphidianMage ), (ulong)TeamFlags.Ophidians + (ulong)TeamFlags.Monster},		
            { typeof( OphidianWarrior ), (ulong)TeamFlags.Ophidians + (ulong)TeamFlags.Monster},	
            { typeof( OphidianMatriarch ), (ulong)TeamFlags.Ophidians + (ulong)TeamFlags.Monster},
			{ typeof( OphidianKnight ), (ulong)TeamFlags.Ophidians + (ulong)TeamFlags.Monster},
	        { typeof( OphidianQueen ), (ulong)TeamFlags.Ophidians + (ulong)TeamFlags.Monster},
			{ typeof( OphidianDestroyer ), (ulong)TeamFlags.Ophidians + (ulong)TeamFlags.Monster},

        //Imps & Minions
            { typeof( Imp ), (ulong)TeamFlags.Imps + (ulong)TeamFlags.Monster},	
            { typeof( HordeMinion ), (ulong)TeamFlags.Imps + (ulong)TeamFlags.Monster},
            { typeof( Mongbat ), (ulong)TeamFlags.Imps + (ulong)TeamFlags.Monster},
			{ typeof( StrongMongbat ), (ulong)TeamFlags.Imps + (ulong)TeamFlags.Monster},	

        //Air Elemental
            { typeof( AirElemental ), (ulong)TeamFlags.ElementalAir + (ulong)TeamFlags.Monster},	

        //Fire Elemental
            { typeof( FireElemental ), (ulong)TeamFlags.ElementalFire + (ulong)TeamFlags.Monster},

        //Water Elemental
            { typeof( WaterElemental ), (ulong)TeamFlags.ElementalWater + (ulong)TeamFlags.Monster},
            { typeof( ElderWaterElemental ), (ulong)TeamFlags.ElementalWater + (ulong)TeamFlags.Pulma + (ulong)TeamFlags.Monster},
            
        //Sanguin
            { typeof( SanguinConscript ), (ulong)TeamFlags.Sanguin + (ulong)TeamFlags.Monster},
            { typeof( SanguinDefender ), (ulong)TeamFlags.Sanguin + (ulong)TeamFlags.Monster},   
            { typeof( SanguinKnight ), (ulong)TeamFlags.Sanguin + (ulong)TeamFlags.Monster},
            { typeof( SanguinMage ), (ulong)TeamFlags.Sanguin + (ulong)TeamFlags.Monster},
            { typeof( SanguinProtector ), (ulong)TeamFlags.Sanguin + (ulong)TeamFlags.Monster},  
            { typeof( SanguinWizard ), (ulong)TeamFlags.Sanguin + (ulong)TeamFlags.Monster},
            { typeof( SanguinScout ), (ulong)TeamFlags.Sanguin + (ulong)TeamFlags.Monster},
            { typeof( SanguinAssassin ), (ulong)TeamFlags.Sanguin + (ulong)TeamFlags.Monster},
            { typeof( SanguinMedic ), (ulong)TeamFlags.Sanguin + (ulong)TeamFlags.Monster},
            { typeof( SanguinMender ), (ulong)TeamFlags.Sanguin + (ulong)TeamFlags.Monster},
            { typeof( SanguinHealer ), (ulong)TeamFlags.Sanguin + (ulong)TeamFlags.Monster},

        //Sea Monsters
            { typeof( DeepSeaSerpent ), (ulong)TeamFlags.SeaMonster + (ulong)TeamFlags.Monster},  
            { typeof( Kraken ), (ulong)TeamFlags.SeaMonster + (ulong)TeamFlags.Monster},	
            { typeof( SeaSerpent ), (ulong)TeamFlags.SeaMonster + (ulong)TeamFlags.Monster},
            { typeof( Leviathan ), (ulong)TeamFlags.SeaMonster + (ulong)TeamFlags.Monster},

        //Juka
            { typeof( JukaLord ), (ulong)TeamFlags.Juka + (ulong)TeamFlags.Monster},			
            { typeof( JukaMage ), (ulong)TeamFlags.Juka + (ulong)TeamFlags.Monster},
			{ typeof( JukaWarrior ), (ulong)TeamFlags.Juka + (ulong)TeamFlags.Monster},	

        //Meer
            { typeof( MeerWarrior ), (ulong)TeamFlags.Meer + (ulong)TeamFlags.Monster},	
            { typeof( MeerEternal ), (ulong)TeamFlags.Meer + (ulong)TeamFlags.Monster},		
            { typeof( MeerMage ), (ulong)TeamFlags.Meer + (ulong)TeamFlags.Monster},
			{ typeof( MeerCaptain ), (ulong)TeamFlags.Meer + (ulong)TeamFlags.Monster},	

        //Savage
            { typeof( SavageShaman ), (ulong)TeamFlags.Savage + (ulong)TeamFlags.KinSavage + (ulong)TeamFlags.Monster},
            { typeof( Savage ), (ulong)TeamFlags.Savage + (ulong)TeamFlags.KinSavage + (ulong)TeamFlags.Monster},
            { typeof( SavageRider ), (ulong)TeamFlags.Savage + (ulong)TeamFlags.KinSavage + (ulong)TeamFlags.Monster},

        //Harpy
            { typeof( Harpy ), (ulong)TeamFlags.Harpy + (ulong)TeamFlags.Monster},	
            { typeof( StoneHarpy ), (ulong)TeamFlags.Harpy + (ulong)TeamFlags.Monster},

        //Wisp
            { typeof( ShadowWisp ), (ulong)TeamFlags.Wisp + (ulong)TeamFlags.Monster},
            { typeof( Wisp ), (ulong)TeamFlags.Player + (ulong)TeamFlags.Wisp + (ulong)TeamFlags.Monster},

        //Khaldun
            { typeof( KhaldunZealot ), (ulong)TeamFlags.Khaldun + (ulong)TeamFlags.Monster},	
            { typeof( KhaldunSummoner ), (ulong)TeamFlags.Khaldun + (ulong)TeamFlags.Monster},
            { typeof( KhaldunRevenant ), (ulong)TeamFlags.Khaldun + (ulong)TeamFlags.Monster},

        //Hiryu
            { typeof( LesserHiryu ), (ulong)TeamFlags.Hiryu + (ulong)TeamFlags.Monster},		
            { typeof( Hiryu ), (ulong)TeamFlags.Hiryu + (ulong)TeamFlags.Monster},
            
        //Edgewich
            //{ typeof( AngryGuest ), (ulong)TeamFlags.Edgewich + (ulong)TeamFlags.Monster},
            { typeof( EvilAlchemist ), (ulong)TeamFlags.Edgewich + (ulong)TeamFlags.Monster},
            { typeof( EvilButcher ), (ulong)TeamFlags.Edgewich + (ulong)TeamFlags.Monster},
            { typeof( FailedExperiment ), (ulong)TeamFlags.Edgewich + (ulong)TeamFlags.Monster},
            { typeof( FuriousHallMonitor ), (ulong)TeamFlags.Edgewich + (ulong)TeamFlags.Monster},
            { typeof( HallMonitor ), (ulong)TeamFlags.Edgewich + (ulong)TeamFlags.Monster},
            { typeof( Librarian ), (ulong)TeamFlags.Edgewich + (ulong)TeamFlags.Monster},
            //{ typeof( RestlessStudent ), (ulong)TeamFlags.Edgewich + (ulong)TeamFlags.Monster},
            //{ typeof( SpectralStudent ), (ulong)TeamFlags.Edgewich + (ulong)TeamFlags.Monster},
            { typeof( StorageImp ), (ulong)TeamFlags.Edgewich + (ulong)TeamFlags.Monster},

        //Lapison
            { typeof( CrazedAdventurer ), (ulong)TeamFlags.Monster},
            { typeof( CrazedLumberjack ), (ulong)TeamFlags.Monster},
            { typeof( CrazedMiner ), (ulong)TeamFlags.Monster},
            { typeof( CrazedAlchemist ), (ulong)TeamFlags.Monster},
            { typeof( CrazedFarmer  ), (ulong)TeamFlags.Monster},
            { typeof( CrazedPeasant ), (ulong)TeamFlags.Monster},

        //Pulma
            { typeof( SteamElemental ), (ulong)TeamFlags.Pulma + (ulong)TeamFlags.Monster},
            { typeof( WaterNymph ), (ulong)TeamFlags.Pulma + (ulong)TeamFlags.Monster},
            { typeof( WaterSiren ), (ulong)TeamFlags.Pulma + (ulong)TeamFlags.Monster},
            { typeof( WaterNymphWarrior ), (ulong)TeamFlags.Pulma + (ulong)TeamFlags.Monster},
            { typeof( WaterSirenWarrior ), (ulong)TeamFlags.Pulma + (ulong)TeamFlags.Monster},

        //Guard
            { typeof( DungeonGuardMelee ), (ulong)TeamFlags.Player + (ulong)TeamFlags.Guard + (ulong)TeamFlags.Monster},
            { typeof( DungeonGuardRanged ), (ulong)TeamFlags.Player + (ulong)TeamFlags.Guard + (ulong)TeamFlags.Monster},
            { typeof( ArcherGuard ), (ulong)TeamFlags.Player + (ulong)TeamFlags.Guard + (ulong)TeamFlags.Monster},
            { typeof( WarriorGuard ), (ulong)TeamFlags.Player + (ulong)TeamFlags.Guard + (ulong)TeamFlags.Monster},
            { typeof( BaseGuard ), (ulong)TeamFlags.Player + (ulong)TeamFlags.Guard + (ulong)TeamFlags.Monster},

            //INDEPENDENTS: NO TEAM - Only Here For Reference
            { typeof( RandomChild ), (ulong)TeamFlags.Monster },
            { typeof( ToxicElemental ), (ulong)TeamFlags.Monster },
			{ typeof( BloodElemental ), (ulong)TeamFlags.Monster },
            { typeof( Juggernaut ), (ulong)TeamFlags.Monster },
            { typeof( HeadlessOne ), (ulong)TeamFlags.Monster },
            { typeof( Serado ), (ulong)TeamFlags.Monster }, 
			{ typeof( Walrus ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },					                
            { typeof( Boar ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },	
            { typeof( Bull ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },				
            { typeof( Centaur ), (ulong)TeamFlags.Monster },
			{ typeof( Chicken ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( Cow ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },  
            { typeof( Executioner ), (ulong)TeamFlags.Monster },	
            { typeof( Goat ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },	
            { typeof( Guardian ), (ulong)TeamFlags.Monster },							
            { typeof( Harrower ), (ulong)TeamFlags.Monster },
            { typeof( JackRabbit ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( Llama ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },	
            { typeof( MountainGoat ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },	
            { typeof( Pig ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },					
            { typeof( Scorpion ), (ulong)TeamFlags.Monster },	
            { typeof( Sheep ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( BlackSheep ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( Kirin ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },	
            { typeof( PredatorHellCat ), (ulong)TeamFlags.Monster },
			{ typeof( LavaLizard ), (ulong)TeamFlags.Monster },
			{ typeof( Lizardman ), (ulong)TeamFlags.Monster },	
            { typeof( ShadowFiend ), (ulong)TeamFlags.Monster },					                   
            //{ typeof( Cursed ), (ulong)TeamFlags.Monster },			
            { typeof( GrimmochDrummel ), (ulong)TeamFlags.Monster },
			{ typeof( LysanderGathenwale ), (ulong)TeamFlags.Monster },
            { typeof( MorgBergen ), (ulong)TeamFlags.Monster },
            { typeof( TavaraSewel ), (ulong)TeamFlags.Monster },	
			{ typeof( Doppleganger ), (ulong)TeamFlags.Monster },	                    
            { typeof( ExodusMinion ), (ulong)TeamFlags.Monster },
			{ typeof( ExodusOverseer ), (ulong)TeamFlags.Monster },					
            { typeof( Gaman ), (ulong)TeamFlags.Monster },
            { typeof( PoisonElemental ), (ulong)TeamFlags.Monster },	
            { typeof( SandVortex ), (ulong)TeamFlags.Monster },
            { typeof( Efreet ), (ulong)TeamFlags.Monster },
            { typeof( LordOaks ), (ulong)TeamFlags.Monster },			
            { typeof( Silvani ), (ulong)TeamFlags.Monster },
            { typeof( Brigand ), (ulong)TeamFlags.Monster }, 
            { typeof( Pirate ), (ulong)TeamFlags.Monster }, 
            { typeof( BlackBear ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( Alligator ), (ulong)TeamFlags.Monster },
            { typeof( BrownBear ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( Gorilla ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( GrizzlyBear ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( PolarBear ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( Cougar ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( Cat ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( DireWolf ), (ulong)TeamFlags.Monster },			
            { typeof( Dog ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( Dolphin ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( DesertOstard ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player }, 
            { typeof( FireSteed ), (ulong)TeamFlags.Monster },		
            { typeof( ForestOstard ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },		
            { typeof( FrenziedOstard ), (ulong)TeamFlags.Monster },
            { typeof( GreatHart ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
			{ typeof( GreyWolf ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },	
            { typeof( HellHound ), (ulong)TeamFlags.Monster },
			{ typeof( Hind ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },	
            { typeof( Horse ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },					
            { typeof( PackHorse ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
			{ typeof( PackLlama ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( Panther ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },                    	
            { typeof( Rabbit ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( RidableLlama ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
			{ typeof( Ridgeback ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( SilverSteed ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( Bird ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( Parrot ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( Eagle ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( Phoenix ), (ulong)TeamFlags.Monster },
            { typeof( Crane ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( SavageRidgeback ), (ulong)TeamFlags.Monster },
            { typeof( WhiteWolf ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( TBWarHorse ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( CoMWarHorse ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( MinaxWarHorse ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( SLWarHorse ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( Unicorn ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( TimberWolf ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( SnowLeopard ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( HellCat ), (ulong)TeamFlags.Monster },                    
            { typeof( EtherealWarrior ), (ulong)TeamFlags.Monster },
			{ typeof( Nightmare ), (ulong)TeamFlags.Monster },
            { typeof( Barracoon ), (ulong)TeamFlags.Monster },		
            { typeof( Mephitis ), (ulong)TeamFlags.Monster },			
            { typeof( Neira ), (ulong)TeamFlags.Monster },
			{ typeof( Rikktor ), (ulong)TeamFlags.Monster },			
            { typeof( Semidar ), (ulong)TeamFlags.Monster },
			{ typeof( Pixie ), (ulong)TeamFlags.Monster },
			{ typeof( VorpalBunny ), (ulong)TeamFlags.Monster },
	        { typeof( WanderingHealer ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( BearDruid ), (ulong)TeamFlags.Monster },
            { typeof( BearformDruid ), (ulong)TeamFlags.Monster },
            { typeof( DeerDruid ), (ulong)TeamFlags.Monster },
            { typeof( DeerformDruid ), (ulong)TeamFlags.Monster },

            // "good" NPCs
            { typeof( PlayerVendor ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( TownHero ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( BaseFactionGuard ), (ulong)TeamFlags.Faction + (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( FactionBerserker ), (ulong)TeamFlags.Faction + (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( FactionDeathKnight ), (ulong)TeamFlags.Faction + (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( FactionHenchman ), (ulong)TeamFlags.Faction + (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( FactionKnight ), (ulong)TeamFlags.Faction + (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( FactionMercenary ), (ulong)TeamFlags.Faction + (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( FactionSorceress ), (ulong)TeamFlags.Faction + (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( FactionWizard ), (ulong)TeamFlags.Faction + (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( BaseFactionVendor ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( FactionBoardVendor ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( FactionBottleVendor ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( FactionHorseVendor ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( FactionOreVendor ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( FactionReagentVendor ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( FactionRewardVendor ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( BountyHunter ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( TownshipRewardVendor ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },

            { typeof( EvilHealer ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( EvilWanderingHealer ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( FortuneTeller ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( BaseHealer ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( Healer ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( PricedHealer ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },

            // vendors / npcs
            { typeof( Actor ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( Artist ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( Banker ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( BaseEscortable ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( TalkingBaseEscortable ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( TalkingBaseVendor ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( TalkingBaseCreature ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( BrideGroom ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( EscortableMage ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( Gypsy ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( HarborMaster ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( Merchant ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( Messenger ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( Minter ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( Ninja ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( Noble ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( Peasant), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( Sculptor ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( SeekerOfAdventure ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( TownCrier ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( BaseVendor ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( Alchemist ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( AnimalTrainer ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( Architect ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( Armorer), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( Baker ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( Bard ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( Barkeeper), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( Beekeeper), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( Blacksmith), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( Bowyer ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( Butcher ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( Carpenter ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( Cobbler ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( Cook ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( CustomHairstylist ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( Farmer ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( Fisherman ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( Furtrader ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( Glassblower ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( GolemCrafter ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( GypsyAnimalTrainer ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( GypsyBanker ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( GypsyMaiden ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( HairStylist ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( Herbalist ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( HolyMage ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( InnKeeper ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( IronWorker ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( Jeweler ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( KeeperOfChivalry ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( LeatherWorker ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( Mage ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( Mapmaker ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( Miller ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( Miner ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( Monk ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( Provisioner ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( Rancher ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( Ranger ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( RealEstateBroker ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( Scribe ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( Shipwright ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( StoneCrafter ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( Tailor ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( Tanner ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( TavernKeeper ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( Thief ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( Tinker ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( Vagabond ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( VarietyDealer ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( Veterinarian ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( Waiter ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( Weaponsmith ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( Weaver ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( XmlQuestNPC ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( TalkingJeweler ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( TalkingDrake ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },

            { typeof( ThiefGuildmaster ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( PlayerBarkeeper ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( TropicalBird ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( EncagedGiantToad ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            
            
            // ZOO
            { typeof( EncagedAlligator ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( EncagedDireWolf ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( EncagedGiantSerpent ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( EncagedGorilla ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( EncagedGrizzlyBear ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( EncagedLlama ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( EncagedPolarBear ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( EncagedRabbit ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( EncagedScorpion ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player },
            { typeof( EncagedTimberWolf ), (ulong)TeamFlags.Monster + (ulong)TeamFlags.Player }            
        };

        public static void SetDefaultTeam(Mobile m) {
            if (AITeamList.TeamFlagDict.ContainsKey(m.GetType()))
            {
                m.TeamFlags = (ulong)AITeamList.TeamFlagDict[m.GetType()];
            }
            else
            {
                AITeamList.LogWarning(m.GetType());
                m.TeamFlags = (ulong)AITeamList.TeamFlags.Player + (ulong)AITeamList.TeamFlags.Monster;
            }
        }

        private static AITeamList[] m_Teams = new AITeamList[]
		{  
            //Undead
			new AITeamList(new Type[]
			{
				typeof( Zombie ),
                typeof( Mummy ),
                typeof( RottingCorpse ),
                typeof( BoneKnight ),
	            typeof( Ghoul ),
                typeof( Skeleton ),
                typeof( RestlessSoul ),
                typeof( AncientLich ),	
                typeof( BoneMagi ),	
                typeof( SkeletalDragon ),	
                typeof( SkeletalMage ),
                typeof( Shade ),                
                typeof( Wraith ),		
				typeof( LichLord ),			
                typeof( SkeletalKnight),
	            typeof( Cursed ),	
                typeof( SpectralArmour),
                typeof( Lich ),
                typeof( AngryGuest ),
                typeof( RestlessSoul ),
                typeof( RestlessStudent ),
                typeof( SpectralStudent ),
                typeof( SkeletalMount ),
                typeof( Bogle )
			} ),

            //Ogres, Trolls, Ettins
			new AITeamList(new Type[]
			{
                typeof( Ogre ),				
                typeof( OgreLord ),	
                typeof( ArcticOgreLord ),
                typeof( Ettin ),
	            typeof( FrostTroll ),
                typeof( Troll ),
            } ),

            //Orcs
			new AITeamList(new Type[]
			{
                typeof( Orc ),
			    typeof( OrcBomber ),		
                typeof( OrcBrute ),		
                typeof( OrcCaptain ),
			    typeof( OrcishLord ),		
                typeof( OrcishMage ),
                typeof( OrcArcher ),
                typeof( BridgeEttin ),
                typeof( BridgeOrcArcher ),
                typeof( BridgeOrcCaptain ),
                typeof( BridgeOrcishMage ),
                typeof( PeculiarOrc )
            } ),

            //Gazers
			new AITeamList(new Type[]
			{
               typeof( GazerLarva ),
               typeof( Gazer ),
               typeof( ElderGazer ),               
            } ),

            //Rats
			new AITeamList(new Type[]
			{
               typeof( Rat ),
	           typeof( Sewerrat ),
	           typeof( GiantRat ),		
            } ),

            //Slimes
			new AITeamList(new Type[]
			{
               typeof( FrostOoze ),
               typeof( Jwilson ),	
               typeof( Slime ),
               typeof( Puddle ),
            } ),

            //Misc Elementals
			new AITeamList(new Type[]
			{
                typeof( IceElemental ),
                typeof( SnowElemental ),
                typeof( FrozenElemental ),
            } ),

            //Golem
            new AITeamList(new Type[]
			{
               typeof( GolemController ),
               typeof( Golem )               
            } ),

            //Earth Elementals
			new AITeamList(new Type[]
			{       
               typeof( DustElemental ),  
               typeof( EarthElemental ),               
               typeof( ValoriteElemental ),
               typeof( VeriteElemental ), 
               typeof( AgapiteElemental ),
               typeof( BronzeElemental ),
               typeof( CopperElemental ),
               typeof( DullCopperElemental ),
               typeof( GoldenElemental ),
               typeof( ShadowIronElemental ),
            } ),

            //Evil Mage
			new AITeamList(new Type[]
			{
                typeof( EvilMageRobed ),
                typeof( EvilMage ),			
                typeof( EvilMageLord ),	
                typeof( EvilMageLordRobed ),
            } ),

            //Black Solen
			new AITeamList(new Type[]
			{
                typeof( BlackSolenInfiltratorQueen ),
                typeof( BlackSolenInfiltratorWarrior ),
				typeof( BlackSolenQueen ),	
                typeof( BlackSolenWarrior ), 
                typeof( BlackSolenWorker ),
            } ),

            //Red Solen
			new AITeamList(new Type[]
			{
                typeof( RedSolenInfiltratorQueen ), 
                typeof( RedSolenInfiltratorWarrior ), 
                typeof( RedSolenQueen ),
				typeof( RedSolenWarrior ),	
                typeof( RedSolenWorker ),
            } ),

            //Plants
			new AITeamList(new Type[]
			{
                typeof( BogThing ),				
                typeof( PlagueBeast ),
                typeof( Quagmire ),	
                typeof( PlagueSpawn ),
                typeof( SwampTentacle ),
	            typeof( WhippingVine ),
                typeof( Corpser ),
	            typeof( Reaper )
            } ),

            //Beetles
            new AITeamList(new Type[]
			{
                typeof( AntLion ), 
                typeof( Beetle ),  
            } ),

            //SummonedDrones
            new AITeamList(new Type[]
			{
                typeof( EnergyVortex ),
                typeof( BladeSpirits )
            } ),

            //Spiders
            new AITeamList(new Type[]
			{
                typeof( FrostSpider ),	
                typeof( GiantSpider ),
		        typeof( GiantBlackWidow ),                    	
		        typeof( DreadSpider ),	
            } ),

            //Gargoyles
            new AITeamList(new Type[]
			{
               	typeof( FireGargoyle ),
                typeof( Gargoyle ),
	            typeof( EnslavedGargoyle ),
                typeof( StoneGargoyle ),
                typeof( GargoyleDestroyer ),
                typeof( GargoyleEnforcer ),
                typeof( GargoyleAugurer ),
                typeof( GargoyleConjurer ),
                typeof( GargoyleElder ),
                typeof( EliteGargoyleElder ),
                typeof( GargoyleWarrior ),
                typeof( GargoyleYoung ),
                typeof( GargoyleTaskmaster ),
                typeof( GargoyleRector ),
                typeof( GargoyleYoungWarrior ),
                typeof( GargoyleAncient ),
                typeof( EliteGargoyleAncient ),
            } ),

            //Snakes
            new AITeamList(new Type[]
			{
               	typeof( IceSerpent ),
	            typeof( GiantSerpent ),
                typeof( IceSnake ),
	            typeof( LavaSerpent ),
                typeof( LavaSnake ),
                typeof( Snake ),
                typeof( SilverSerpent ), 
            } ),

            //Frogs
            new AITeamList(new Type[]
			{
               typeof( BullFrog ),
               typeof( GiantToad ),
            } ),

            //Ratman
            new AITeamList(new Type[]
			{
                typeof( Ratman ),			
                typeof( RatmanArcher ),		
                typeof( RatmanMage ),
            } ),

            //Giants
            new AITeamList(new Type[]
			{
                typeof( Titan ), 
	            typeof( Cyclops ), 	
            } ),

            //Lockmoor
            new AITeamList(new Type[]
			{
                typeof( AncientVampire ),
                typeof( InfestedRat ),
                typeof( LockmoorAlchemist ),
                typeof( LockmoorArcher ),
                typeof( LockmoorButcher ),
                typeof( LockmoorGuard ),
                typeof( LockmoorMage ),
                typeof( LockmoorNoble ),
                typeof( LockmoorRoyalty ),
                typeof( BloodthirstyVampire ),
                typeof( Torturer  ),
                typeof( Vampire ),
                typeof( BridgeLockmoorArcher ),
                typeof( BridgeLockmoorGuard ),
                typeof( BridgeMage ),
                typeof( VampireOutcast )
            } ),

            //Daemons
            new AITeamList(new Type[]
			{
                typeof( ChaosDaemon ),
                typeof( Daemon ),
                typeof( IceFiend ),
	            typeof( Balron ),
                typeof( ArcaneDaemon ),
                typeof( Moloch ),                
            } ),

            //Dragons
            new AITeamList(new Type[]
			{
                typeof( Dragon ),			
                typeof( Drake ),
                typeof( ShadowWyrm ),
	            typeof( SerpentineDragon ),   
                typeof( SkeletalDragon ),	
                typeof( SwampDragon ),
				typeof( ScaledSwampDragon ),								
                typeof( Wyvern ),
                typeof( WhiteWyrm ),
                typeof( AncientWyrm ),
                typeof( BabyWaterWyrm ),
				typeof( WaterWyrm )	            
            } ),

            //Terathans
            new AITeamList(new Type[]
			{
                typeof( TerathanAvenger ),
				typeof( TerathanDrone ),	
                typeof( TerathanMatriarch ), 
                typeof( TerathanWarrior ),
            } ),

            //Ophidians
            new AITeamList(new Type[]
			{
                typeof( OphidianArchmage ),
				typeof( OphidianMage ),		
                typeof( OphidianWarrior ),	
                typeof( OphidianMatriarch ),
				typeof( OphidianKnight ),
	            typeof( OphidianQueen ),
				typeof( OphidianDestroyer ),
            } ),

            //Imps & Minions
            new AITeamList(new Type[]
			{
                typeof( Imp ),	
                typeof( HordeMinion ),
                typeof( Mongbat ),
				typeof( StrongMongbat ),	
            } ),

            //Air Elemental
            new AITeamList(new Type[]
			{
                typeof( AirElemental ),	
            } ),

            //Fire Elemental
            new AITeamList(new Type[]
			{
                typeof( FireElemental ),     
            } ),

            //Water Elemental
            new AITeamList(new Type[]
			{
                typeof( WaterElemental ),
                typeof( ElderWaterElemental ),
            } ),

            //Sanguin
            new AITeamList(new Type[]
			{
                typeof( SanguinConscript ),
                typeof( SanguinDefender ),   
                typeof( SanguinKnight ),
                typeof( SanguinMage ),
                typeof( SanguinProtector ),  
                typeof( SanguinWizard ),
                typeof( SanguinScout ),
                typeof( SanguinAssassin ),
                typeof( SanguinMedic ),
                typeof( SanguinMender ),
                typeof( SanguinHealer )
            } ),

            //Sea Monsters
            new AITeamList(new Type[]
			{
                typeof( DeepSeaSerpent ),  
                typeof( Kraken ),	
                typeof( SeaSerpent ),
                typeof( Leviathan ),
            } ),

            //Juka
            new AITeamList(new Type[]
			{
                typeof( JukaLord ),			
                typeof( JukaMage ),
				typeof( JukaWarrior ),	
            } ),

            //Meer
            new AITeamList(new Type[]
			{
                typeof( MeerWarrior ),	
                typeof( MeerEternal ),		
                typeof( MeerMage ),
				typeof( MeerCaptain ),	
            } ),

            //Savage
            new AITeamList(new Type[]
			{
              	typeof( SavageShaman ),
                typeof( Savage ),
                typeof( SavageRider ),
            } ),

            //Harpy
            new AITeamList(new Type[]
			{
              	typeof( Harpy ),	
                typeof( StoneHarpy ),
            } ),

            //Wisp
            new AITeamList(new Type[]
			{
              	typeof( ShadowWisp ),
                typeof( Wisp )
            } ),

            //Khaldun
            new AITeamList(new Type[]
			{
              	typeof( KhaldunZealot ),	
                typeof( KhaldunSummoner ),
                typeof( KhaldunRevenant )
            } ),

            //Hiryu
            new AITeamList(new Type[]
			{
              	typeof( LesserHiryu ),		
                typeof( Hiryu )
            } ),  
            
            //Faction
            new AITeamList(new Type[]
			{
              	typeof( FactionKnight ),    
                typeof( FactionHenchman ),	
                typeof( FactionBerserker ),
                typeof( FactionMercenary ),	
                typeof( FactionSorceress ),	
                typeof( FactionWizard )       
            } ),

            //Edgewich
            new AITeamList(new Type[]
			{
                typeof( AngryGuest ),
                typeof( EvilAlchemist ),
                typeof( EvilButcher ),
                typeof( FailedExperiment ),
                typeof( FuriousHallMonitor ),
                typeof( HallMonitor ),
                typeof( Librarian ),
                typeof( RestlessStudent ),
                typeof( SpectralStudent ),
                typeof( StorageImp ),   
            } ),

            //Pulma
            new AITeamList(new Type[]
			{
                typeof( BabyWaterWyrm ),
                typeof( ElderWaterElemental ),
                typeof( FrozenElemental ),
                typeof( SteamElemental ),
                typeof( Puddle ),
                typeof( WaterWyrm ),
                typeof( WaterNymph ),
                typeof( WaterSiren ),
                typeof( WaterNymphWarrior ),
                typeof( WaterSirenWarrior ),
            } ),

            //Guard
            new AITeamList(new Type[]
			{
                typeof( DungeonGuardMelee ),
                typeof( DungeonGuardRanged ),
            } ),

            //INDEPENDENTS: NO TEAM - This Must be the Last Table in List
            //Table Not Looked At: Only Here For Reference
            new AITeamList(new Type[]
			{
              	typeof( ToxicElemental ),
				typeof( BloodElemental ),
                typeof( Juggernaut ),
                typeof( HeadlessOne ),
                typeof( Serado ), 
				typeof( Walrus ),					                
                typeof( Boar ),	
                typeof( Bull ),				
                typeof( Centaur ),
				typeof( Chicken ),
                typeof( Cow ),  
                typeof( Executioner ),	
                typeof( Goat ),	
                typeof( Guardian ),							
                typeof( Harrower ),
                typeof( JackRabbit ),
                typeof( Llama ),	
                typeof( MountainGoat ),	
                typeof( Pig ),					
                typeof( Scorpion ),	
                typeof( Sheep ),
                typeof( BlackSheep ),
                typeof( Kirin ),	
                typeof( PredatorHellCat ),
				typeof( LavaLizard ),
				typeof( Lizardman ),	
                typeof( ShadowFiend ),					                   
                typeof( Cursed ),			
                typeof( GrimmochDrummel ),
				typeof( LysanderGathenwale ),
                typeof( MorgBergen ),
                typeof( TavaraSewel ),	
				typeof( Doppleganger ),	                    
                typeof( ExodusMinion ),
				typeof( ExodusOverseer ),					
                typeof( Gaman ),
                typeof( PoisonElemental ),	
                typeof( SandVortex ),
                typeof( Efreet ),
                typeof( LordOaks ),			
                typeof( Silvani ),
                typeof( SteamElemental ),
                typeof( Brigand ), 
                typeof( Pirate ), 
                typeof( BlackBear ),
                typeof( Alligator ),
                typeof( BrownBear ),
                typeof( Gorilla ),
                typeof( GrizzlyBear ),
                typeof( PolarBear ),
                typeof( Cougar ),
                typeof( Cat ),
                typeof( DireWolf ),			
                typeof( Dog ),
                typeof( Dolphin ),
                typeof( DesertOstard ), 
                typeof( FireSteed ),		
                typeof( ForestOstard ),		
                typeof( FrenziedOstard ),
                typeof( GreatHart ),
				typeof( GreyWolf ),	
                typeof( HellHound ),
				typeof( Hind ),	
                typeof( Horse ),					
                typeof( PackHorse ),
				typeof( PackLlama ),
                typeof( Panther ),                    	
                typeof( Rabbit ),
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
                typeof( SnowLeopard ),
                typeof( HellCat ),                    
                typeof( EtherealWarrior ),
				typeof( Nightmare ),
                typeof( Barracoon ),		
                typeof( Mephitis ),			
                typeof( Neira ),
				typeof( Rikktor ),			
                typeof( Semidar ),
				typeof( Pixie ),
				typeof( VorpalBunny ),
	            typeof( WanderingHealer ),
                typeof( RandomChild), 
            
            } )		
		};
    }

    public class AIKinTeamList
    {
        public Type[] Types { get; set; }
        public int HueMod { get; set; }
        public string Name { get; set; }

        public AIKinTeamList(Type[] types, int hue, string name)
        {
            Types = types;
            HueMod = hue;
            Name = name;
        }
        public static bool CheckKinTeam(object objFrom, object objTarget)
        {
            PlayerMobile playerTarget = objTarget as PlayerMobile;
            if (playerTarget == null)
                return false;

            int playerHueMod = playerTarget.HueMod;

            Type fromType = objFrom.GetType();
            bool validKin = false;
            for (int i = 0; i < m_KinTeams.Length; i++)
            {
                AIKinTeamList typeList = m_KinTeams[i];
                Type[] types = typeList.Types;

                for (int j = 0; j < types.Length; j++)
                {
                    if (fromType == types[j] && playerHueMod == typeList.HueMod)
                    {
                        validKin = true;
                        break;
                    }

                }

                if (validKin)
                    return true;
            }
            return false;
        }

        //When a user is effected by kin paints we check against the following teams
        public static AIKinTeamList[] m_KinTeams = new AIKinTeamList[]
        {
            //orcs
            new AIKinTeamList(new Type[]
            {
                typeof(Orc),
                typeof(OrcCaptain),
                typeof(OrcBrute ),
                typeof(BridgeOrcArcher ),
                typeof(BridgeOrcCaptain ),
                typeof(BridgeOrcishMage ),
                typeof(PeculiarOrc ),
                typeof(OrcArcher ),
                typeof(OrcishMage),
                typeof(OrcishLord)
            }, 1451, "Orc"),

            //Drow
            new AIKinTeamList(new Type[]
            {
                typeof(GiantSpider)
            }, 1108, "Drow"),

            //Undead
            new AIKinTeamList(new Type[]
            {
                typeof(Skeleton),
                typeof(Zombie),
                typeof(Ghoul),
                typeof(Bogle),
                typeof(Wraith)
            }, 1882, "Undead"),

            //Savage
            new AIKinTeamList(new Type[]
            {
                typeof(Savage),
                typeof(SavageRider),
                typeof(SavageShaman)
            }, 0, "Savage"),
        };
    }
}