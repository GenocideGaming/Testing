using System;
using System.IO;
using System.Reflection;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server
{
	public class Loot
	{
		#region List definitions

		private static Type[] m_WeaponTypes = new Type[]
		   {
				typeof( Axe ),					typeof( BattleAxe ),			typeof( DoubleAxe ),
				typeof( ExecutionersAxe ),		typeof( Hatchet ),				typeof( LargeBattleAxe ),
				typeof( TwoHandedAxe ),			typeof( WarAxe ),				typeof( Club ),
				typeof( Mace ),					typeof( Maul ),					typeof( WarHammer ),
				typeof( WarMace ),				typeof( Bardiche ),				typeof( Halberd ),
				typeof( Spear ),				typeof( ShortSpear ),			typeof( Pitchfork ),
				typeof( WarFork ),				typeof( BlackStaff ),			typeof( GnarledStaff ),
				typeof( QuarterStaff ),			typeof( Broadsword ),			typeof( Cutlass ),
				typeof( Katana ),				typeof( Kryss ),				typeof( Longsword ),
				typeof( Scimitar ),				typeof( VikingSword ),			typeof( Pickaxe ),
				typeof( HammerPick ),			typeof( ButcherKnife ),			typeof( Cleaver ),
				typeof( Dagger ),				typeof( SkinningKnife ),		typeof( ShepherdsCrook )
			};

		public static Type[] WeaponTypes{ get{ return m_WeaponTypes; } }


		private static Type[] m_RangedWeaponTypes = new Type[]
			{
				typeof( Bow ),					typeof( Crossbow ),				typeof( HeavyCrossbow )
			};

		public static Type[] RangedWeaponTypes{ get{ return m_RangedWeaponTypes; } }

		private static Type[] m_ArmorTypes = new Type[]
			{
				typeof( BoneArms ),				typeof( BoneChest ),			typeof( BoneGloves ),
				typeof( BoneLegs ),				typeof( BoneHelm ),				typeof( ChainChest ),
				typeof( ChainLegs ),			typeof( ChainCoif ),			typeof( Bascinet ),
				typeof( CloseHelm ),			typeof( Helmet ),				typeof( NorseHelm ),
				typeof( OrcHelm ),				typeof( FemaleLeatherChest ),	typeof( LeatherArms ),
				typeof( LeatherBustierArms ),	typeof( LeatherChest ),			typeof( LeatherGloves ),
				typeof( LeatherGorget ),		typeof( LeatherLegs ),			typeof( LeatherShorts ),
				typeof( LeatherSkirt ),			typeof( LeatherCap ),			typeof( FemalePlateChest ),
				typeof( PlateArms ),			typeof( PlateChest ),			typeof( PlateGloves ),
				typeof( PlateGorget ),			typeof( PlateHelm ),			typeof( PlateLegs ),
				typeof( RingmailArms ),			typeof( RingmailChest ),		typeof( RingmailGloves ),
				typeof( RingmailLegs ),			typeof( FemaleStuddedChest ),	typeof( StuddedArms ),
				typeof( StuddedBustierArms ),	typeof( StuddedChest ),			typeof( StuddedGloves ),
				typeof( StuddedGorget ),		typeof( StuddedLegs )
			};

		public static Type[] ArmorTypes{ get{ return m_ArmorTypes; } }

		private static Type[] m_AosShieldTypes = new Type[]
			{
				typeof( ChaosShield ),			typeof( OrderShield )
			};

		public static Type[] AosShieldTypes{ get{ return m_AosShieldTypes; } }

		private static Type[] m_ShieldTypes = new Type[]
			{
				typeof( BronzeShield ),			typeof( Buckler ),				typeof( HeaterShield ),
				typeof( MetalShield ),			typeof( MetalKiteShield ),		typeof( WoodenKiteShield ),
				typeof( WoodenShield )
			};

		public static Type[] ShieldTypes{ get{ return m_ShieldTypes; } }

		private static Type[] m_GemTypes = new Type[]
			{
				typeof( Amber ),				typeof( Amethyst ),				typeof( Citrine ),
				typeof( Diamond ),				typeof( Emerald ),				typeof( Ruby ),
				typeof( Sapphire ),				typeof( StarSapphire ),			typeof( Tourmaline )
			};

		public static Type[] GemTypes{ get{ return m_GemTypes; } }

		private static Type[] m_JewelryTypes = new Type[]
			{
				typeof( GoldRing ),				typeof( GoldBracelet ),
				typeof( SilverRing ),			typeof( SilverBracelet )
			};

		public static Type[] JewelryTypes{ get{ return m_JewelryTypes; } }

		private static Type[] m_RegTypes = new Type[]
			{
				typeof( BlackPearl ),			typeof( Bloodmoss ),			typeof( Garlic ),
				typeof( Ginseng ),				typeof( MandrakeRoot ),			typeof( Nightshade ),
				typeof( SulfurousAsh ),			typeof( SpidersSilk )
			};

		public static Type[] RegTypes{ get{ return m_RegTypes; } }
		
		private static Type[] m_NecroRegTypes = new Type[]
			{
				typeof( BatWing ),			typeof( DaemonBlood ),			typeof( DaemonBone ),
				typeof( DeadWood ),				typeof( GraveDust ),			typeof( NoxCrystal ),
				typeof( PigIron )
			};

		public static Type[] NecroRegTypes{ get{ return m_RegTypes; } }

		private static Type[] m_PotionTypes = new Type[]
			{
				typeof( AgilityPotion ),		typeof( StrengthPotion ),		typeof( RefreshPotion ),
				typeof( CurePotion ),		typeof( HealPotion )
			};

        public static Type[] PotionTypes { get { return m_PotionTypes; } }


        private static Type[] m_GreaterPotionTypes = new Type[]
			{
				typeof( GreaterAgilityPotion ),		typeof( GreaterStrengthPotion ),		typeof( TotalRefreshPotion ),
				typeof( GreaterCurePotion ),		typeof( GreaterHealPotion )
			};

        public static Type[] GreaterPotionTypes { get { return m_GreaterPotionTypes; } }


		private static Type[] m_InstrumentTypes = new Type[]
			{
				typeof( Drums ),				typeof( Harp ),					typeof( LapHarp ),
				typeof( Lute ),					typeof( Tambourine ),			typeof( TambourineTassel )
			};

		public static Type[] InstrumentTypes{ get{ return m_InstrumentTypes; } }

		private static Type[] m_StatueTypes = new Type[]
		{
			typeof( StatueSouth ),			typeof( StatueSouth2 ),			typeof( StatueNorth ),
			typeof( StatueWest ),			typeof( StatueEast ),			typeof( StatueEast2 ),
			typeof( StatueSouthEast ),		typeof( BustSouth ),			typeof( BustEast )
		};

		public static Type[] StatueTypes{ get{ return m_StatueTypes; } }

		private static Type[] m_RegularScrollTypes = new Type[]
			{
				typeof( ReactiveArmorScroll ),	typeof( ClumsyScroll ),			typeof( CreateFoodScroll ),		typeof( FeeblemindScroll ),
				typeof( HealScroll ),			typeof( MagicArrowScroll ),		typeof( NightSightScroll ),		typeof( WeakenScroll ),
				typeof( AgilityScroll ),		typeof( CunningScroll ),		typeof( CureScroll ),			typeof( HarmScroll ),
				typeof( MagicTrapScroll ),		typeof( MagicUnTrapScroll ),	typeof( ProtectionScroll ),		typeof( StrengthScroll ),
				typeof( BlessScroll ),			typeof( FireballScroll ),		typeof( MagicLockScroll ),		typeof( PoisonScroll ),
				typeof( TelekinisisScroll ),	typeof( TeleportScroll ),		typeof( UnlockScroll ),			typeof( WallOfStoneScroll ),
				typeof( ArchCureScroll ),		typeof( ArchProtectionScroll ),	typeof( CurseScroll ),			typeof( FireFieldScroll ),
				typeof( GreaterHealScroll ),	typeof( LightningScroll ),		typeof( ManaDrainScroll ),		typeof( RecallScroll ),
				typeof( BladeSpiritsScroll ),	typeof( DispelFieldScroll ),	typeof( IncognitoScroll ),		typeof( MagicReflectScroll ),
				typeof( MindBlastScroll ),		typeof( ParalyzeScroll ),		typeof( PoisonFieldScroll ),	typeof( SummonCreatureScroll ),
				typeof( DispelScroll ),			typeof( EnergyBoltScroll ),		typeof( ExplosionScroll ),		typeof( InvisibilityScroll ),
				typeof( MarkScroll ),			typeof( MassCurseScroll ),		typeof( ParalyzeFieldScroll ),	typeof( RevealScroll ),
				typeof( ChainLightningScroll ), typeof( EnergyFieldScroll ),	typeof( FlamestrikeScroll ),	typeof( GateTravelScroll ),
				typeof( ManaVampireScroll ),	typeof( MassDispelScroll ),		typeof( MeteorSwarmScroll ),	typeof( PolymorphScroll ),
				typeof( EarthquakeScroll ),		typeof( EnergyVortexScroll ),	typeof( ResurrectionScroll ),	typeof( SummonAirElementalScroll ),
				typeof( SummonDaemonScroll ),	typeof( SummonEarthElementalScroll ),	typeof( SummonFireElementalScroll ),	typeof( SummonWaterElementalScroll )
			};

	
		public static Type[] RegularScrollTypes{ get{ return m_RegularScrollTypes; } }

        private static Type[] m_LowScrollTypes = new Type[]
        {
                typeof( ReactiveArmorScroll ),	typeof( ClumsyScroll ),			typeof( CreateFoodScroll ),		typeof( FeeblemindScroll ),
				typeof( HealScroll ),			typeof( MagicArrowScroll ),		typeof( NightSightScroll ),		typeof( WeakenScroll ),
				typeof( AgilityScroll ),		typeof( CunningScroll ),		typeof( CureScroll ),			typeof( HarmScroll ),
				typeof( MagicTrapScroll ),		typeof( MagicUnTrapScroll ),	typeof( ProtectionScroll ),		typeof( StrengthScroll ),
				typeof( BlessScroll ),			typeof( FireballScroll ),		typeof( MagicLockScroll ),		typeof( PoisonScroll ),
				typeof( TelekinisisScroll ),	typeof( TeleportScroll ),		typeof( UnlockScroll ),			typeof( WallOfStoneScroll ),
        };

        public static Type[] LowScrollTypes { get { return m_LowScrollTypes; } }

        private static Type[] m_MedScrollTypes = new Type[] 
        {
            	typeof( ArchCureScroll ),		typeof( ArchProtectionScroll ),	typeof( CurseScroll ),			typeof( FireFieldScroll ),
				typeof( GreaterHealScroll ),	typeof( LightningScroll ),		typeof( ManaDrainScroll ),		typeof( RecallScroll ),
				typeof( BladeSpiritsScroll ),	typeof( DispelFieldScroll ),	typeof( IncognitoScroll ),		typeof( MagicReflectScroll ),
				typeof( MindBlastScroll ),		typeof( ParalyzeScroll ),		typeof( PoisonFieldScroll ),	typeof( SummonCreatureScroll ),
				typeof( DispelScroll ),			typeof( EnergyBoltScroll ),		typeof( ExplosionScroll ),		typeof( InvisibilityScroll ),
				typeof( MarkScroll ),			typeof( MassCurseScroll ),		typeof( ParalyzeFieldScroll ),	typeof( RevealScroll ),
        };
        public static Type[] MedScrollTypes { get { return m_MedScrollTypes; } }

        private static Type[] m_HighScrollTypes = new Type[]
        {
                typeof( ChainLightningScroll ), typeof( EnergyFieldScroll ),	typeof( FlamestrikeScroll ),	typeof( GateTravelScroll ),
				typeof( ManaVampireScroll ),	typeof( MassDispelScroll ),		typeof( MeteorSwarmScroll ),	typeof( PolymorphScroll ),
				typeof( EarthquakeScroll ),		typeof( EnergyVortexScroll ),	typeof( ResurrectionScroll ),	typeof( SummonAirElementalScroll ),
				typeof( SummonDaemonScroll ),	typeof( SummonEarthElementalScroll ),	typeof( SummonFireElementalScroll ),	typeof( SummonWaterElementalScroll )
        };

        public static Type[] HighScrollTypes { get { return m_HighScrollTypes; } }

		private static Type[] m_WandTypes = new Type[]
			{
				typeof( ClumsyWand ),               typeof( FeebleWand ),           typeof( FireballWand ),
				typeof( GreaterHealWand ),          typeof( HarmWand ),             typeof( HealWand ),
				typeof( IDWand ),                   typeof( LightningWand ),        typeof( MagicArrowWand ),
				typeof( ManaDrainWand ),            typeof( WeaknessWand )          
                
			};
		public static Type[] WandTypes{ get{ return m_WandTypes; } }

		private static Type[] m_ClothingTypes = new Type[]
			{
				typeof( Cloak ),				
				typeof( Bonnet ),               typeof( Cap ),		            typeof( FeatheredHat ),
				typeof( FloppyHat ),            typeof( JesterHat ),			typeof( Surcoat ),
				typeof( SkullCap ),             typeof( StrawHat ),	            typeof( TallStrawHat ),
				typeof( TricorneHat ),			typeof( WideBrimHat ),          typeof( WizardsHat ),
				typeof( BodySash ),             typeof( Doublet ),              typeof( Boots ),
				typeof( FullApron ),            typeof( JesterSuit ),           typeof( Sandals ),
				typeof( Tunic ),				typeof( Shoes ),				typeof( Shirt ),
				typeof( Kilt ),                 typeof( Skirt ),				typeof( FancyShirt ),
				typeof( FancyDress ),			typeof( ThighBoots ),			typeof( LongPants ),
				typeof( PlainDress ),           typeof( Robe ),					typeof( ShortPants ),
				typeof( HalfApron )
			};
		public static Type[] ClothingTypes{ get{ return m_ClothingTypes; } }


		private static Type[] m_HatTypes = new Type[]
			{
				typeof( SkullCap ),			typeof( Bandana ),		typeof( FloppyHat ),
				typeof( Cap ),				typeof( WideBrimHat ),	typeof( StrawHat ),
				typeof( TallStrawHat ),		typeof( WizardsHat ),	typeof( Bonnet ),
				typeof( FeatheredHat ),		typeof( TricorneHat ),	typeof( JesterHat )
			};

		public static Type[] HatTypes{ get{ return m_HatTypes; } }

        //There are 3 types of artifacts in 4 tiers
        private static Type[] mArtifactTypes = new Type[]
        {
            typeof(ArtifactOfMight), typeof(ArtifactOfForce), typeof(ArtifactOfPower), typeof(ArtifactOfVanquishing), 
            typeof(ArtifactOfSurpassingAccuracy), typeof(ArtifactOfEminentAccuracy), typeof(ArtifactOfExceedingAccuracy), typeof(ArtifactOfSupremeAccuracy),
            typeof(ArtifactOfGuarding), typeof(ArtifactOfHardening), typeof(ArtifactOfFortification), typeof(ArtifactOfInvulnerability),
        };

        public static Type[] ArtifactTypes { get { return mArtifactTypes; } }

        private static Type[] mSlayerArtifactTypes = new Type[]
        {
            typeof(ArtifactOfSilver), typeof(ArtifactOfElementalSlaying), typeof(ArtifactOfArachnidSlaying), 
            typeof(ArtifactOfRepondSlaying), typeof(ArtifactOfReptileSlaying)
        };

        public static Type[] SlayerArtifactTypes { get { return mSlayerArtifactTypes; } }

        private static Type[] mLeatherDyeTubTypes1 = new Type[]
        {
            typeof(LeatherDyeTubBronze), 
            typeof(LeatherDyeTubCopper), typeof(LeatherDyeTubDullCopper),
            typeof(LeatherDyeTubGolden), typeof(LeatherDyeTubYellows)
        };

        public static Type[] LeatherDyeTubTypes1 { get { return mLeatherDyeTubTypes1; } }

        private static Type[] mLeatherDyeTubTypes2 = new Type[]
        {
            typeof(LeatherDyeTubAgapite),
            typeof(LeatherDyeTubShadowIron), typeof(LeatherDyeTubValorite), typeof(LeatherDyeTubVerite)
        };

        public static Type[] LeatherDyeTubTypes2 { get { return mLeatherDyeTubTypes2; } }

        private static Type[] mLeatherDyeTubTypes3 = new Type[]
        {
            typeof(LeatherDyeTubBlues), 
            typeof(LeatherDyeTubGreens), typeof(LeatherDyeTubReds)
        };

        public static Type[] LeatherDyeTubTypes3 { get { return mLeatherDyeTubTypes3; } }
        
        private static Type[] mBodyPartTypes = new Type[]
        {
            typeof(LeftLeg), typeof(RightLeg), typeof(LeftArm), 
            typeof(RightArm), typeof(Head), typeof(Bone), typeof(RibCage),
            typeof(BonePile)
        };
        
        public static Type[] BodyPartTypes { get { return mBodyPartTypes; } }
        
		#endregion

		#region Accessors

		public static BaseWand RandomWand()
		{
			return Construct( m_WandTypes ) as BaseWand;
		}

		public static BaseClothing RandomClothing()
		{
			return Construct( m_ClothingTypes ) as BaseClothing;
		}

		public static BaseWeapon RandomRangedWeapon()
		{
			return Construct( m_RangedWeaponTypes ) as BaseWeapon;
		}

		public static BaseWeapon RandomWeapon()
		{
			return Construct( m_WeaponTypes ) as BaseWeapon;
		}
		public static Item RandomWeaponOrJewelry()
		{

			return Construct( m_WeaponTypes, m_JewelryTypes );
		}

		public static BaseJewel RandomJewelry()
		{
			return Construct( m_JewelryTypes ) as BaseJewel;
		}

		public static BaseArmor RandomArmor()
		{
			return Construct( m_ArmorTypes ) as BaseArmor;
		}

		public static BaseHat RandomHat()
		{
			return Construct( m_HatTypes ) as BaseHat;
		}

		public static Item RandomArmorOrHat()
		{
			return Construct( m_ArmorTypes, m_HatTypes );
		}

		public static BaseShield RandomShield()
		{
			return Construct( m_ShieldTypes ) as BaseShield;
		}

		public static BaseArmor RandomArmorOrShield()
		{
			return Construct( m_ArmorTypes, m_ShieldTypes ) as BaseArmor;
		}

		public static Item RandomArmorOrShieldOrJewelry( bool inTokuno )
		{
			return Construct( m_ArmorTypes, m_HatTypes, m_ShieldTypes, m_JewelryTypes );
		}

		public static Item RandomArmorOrShieldOrWeapon()
		{
			return Construct( m_WeaponTypes, m_RangedWeaponTypes, m_ArmorTypes, m_HatTypes, m_ShieldTypes );
		}

		public static Item RandomArmorOrShieldOrWeaponOrJewelry()
		{
			return Construct( m_WeaponTypes, m_RangedWeaponTypes, m_ArmorTypes, m_HatTypes, m_ShieldTypes, m_JewelryTypes );
		}

		public static Item RandomGem()
		{
			return Construct( m_GemTypes );
		}

		public static Item RandomReagent()
		{
			return Construct( m_RegTypes );
		}
		
		public static Item RandomNecroReagent()
		{
			return Construct( m_NecroRegTypes );
		}

		public static Item RandomPossibleReagent()
		{
			return Construct( m_RegTypes );
		}

		public static Item RandomPotion()
		{
			return Construct( m_PotionTypes );
		}

        public static Item RandomGreaterPotion()
        {
            return Construct(m_GreaterPotionTypes);
        }

		public static BaseInstrument RandomInstrument()
		{
			return Construct( m_InstrumentTypes ) as BaseInstrument;
		}

		public static Item RandomStatue()
		{
			return Construct( m_StatueTypes );
		}

		public static SpellScroll RandomScroll( int minIndex, int maxIndex, SpellbookType type )
		{	
			return Construct( m_RegularScrollTypes, Utility.RandomMinMax( minIndex, maxIndex ) ) as SpellScroll;
		}


        public static Item RandomArtifact(int artifactLevel)
        {
            switch (artifactLevel)
            {
                case 1:
                    return Construct(mArtifactTypes, Utility.RandomList(0, 4 ,8));
                case 2:
                    return Construct(mArtifactTypes, Utility.RandomList(1, 5, 9));
                case 3:
                    return Construct(mArtifactTypes, Utility.RandomList(2, 6, 10));
                case 4:
                    return Construct(mArtifactTypes, Utility.RandomList(3, 7, 11));
            }

            return Construct(mArtifactTypes, Utility.RandomMinMax(0, 11));
        }

        public static Item RandomArtifact(int minIndex, int maxIndex)
        {
            return Construct(mArtifactTypes, Utility.RandomMinMax(minIndex, maxIndex));
        }

        public static Item RandomSlayerArtifact()
        {
            return Construct(mSlayerArtifactTypes, Utility.RandomMinMax(0, mSlayerArtifactTypes.Length));
        }

        public static Item RandomLeatherDyeTubTypes1()
        {
            return Construct(mLeatherDyeTubTypes1, Utility.RandomMinMax(0, mLeatherDyeTubTypes1.Length));
        }

        public static Item RandomLeatherDyeTubTypes2()
        {
            return Construct(mLeatherDyeTubTypes2, Utility.RandomMinMax(0, mLeatherDyeTubTypes2.Length));
        }

        public static Item RandomLeatherDyeTubTypes3()
        {
            return Construct(mLeatherDyeTubTypes3, Utility.RandomMinMax(0, mLeatherDyeTubTypes3.Length));
        }

        public static Item RandomLeatherDyeTubTypesAll()
        {
            return Construct(mLeatherDyeTubTypes1, mLeatherDyeTubTypes2, mLeatherDyeTubTypes3 );
        }

        public static Item RandomBodyPart()
        {
            return Construct(mBodyPartTypes, Utility.RandomMinMax(0, mBodyPartTypes.Length));
        }
		
		public static Item RandomHighScroll()
		{
			return Construct(m_HighScrollTypes, Utility.RandomMinMax(0, m_HighScrollTypes.Length));
		}
		#endregion

		#region Construction methods
		public static Item Construct( Type type )
		{
			try
			{
				return Activator.CreateInstance( type ) as Item;
			}
			catch
			{
				return null;
			}
		}

		public static Item Construct( Type[] types )
		{
			if ( types.Length > 0 )
				return Construct( types, Utility.Random( types.Length ) );

			return null;
		}

		public static Item Construct( Type[] types, int index )
		{
			if ( index >= 0 && index < types.Length )
				return Construct( types[index] );

			return null;
		}

		public static Item Construct( params Type[][] types )
		{
			int totalLength = 0;

			for ( int i = 0; i < types.Length; ++i )
				totalLength += types[i].Length;

			if ( totalLength > 0 )
			{
				int index = Utility.Random( totalLength );

				for ( int i = 0; i < types.Length; ++i )
				{
					if ( index >= 0 && index < types[i].Length )
						return Construct( types[i][index] );

					index -= types[i].Length;
				}
			}

			return null;
		}
		#endregion
	}
	

}