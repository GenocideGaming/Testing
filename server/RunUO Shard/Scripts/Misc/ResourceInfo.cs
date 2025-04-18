using System;
using System.Collections;

namespace Server.Items
{
	public enum CraftResource
	{
		None = 0,
		Iron = 1,
		DullCopper,
		ShadowIron,
		Copper,
		Bronze,
		Gold,
		Agapite,
		Verite,
		Valorite,

		RegularLeather = 101,
		SpinedLeather,
		HornedLeather,
		BarbedLeather,

		RedScales = 201,
		YellowScales,
		BlackScales,
		GreenScales,
		WhiteScales,
		BlueScales,

		RegularWood = 301,
		OakWood,
		AshWood,
		YewWood,


        ArtifactOfMight = 401,
        ArtifactOfForce,
        ArtifactOfPower,
        ArtifactOfVanquishing,
        ArtifactOfSurpassingAccuracy,
        ArtifactOfEminentAccuracy,
        ArtifactOfExceedingAccuracy,
        ArtifactOfSupremeAccuracy,
        ArtifactOfGuarding,
        ArtifactOfHardening,
        ArtifactOfFortification,
        ArtifactofInvulnerability,
        ArtifactOfUndeadSlayer,
        ArtifactOfElementalSlayer,
        ArtifactOfArachnidSlayer,
        ArtifactOfRepondSlayer,
        ArtifactOfReptileSlayer

        
	}

	public enum CraftResourceType
	{
		None,
        Artifact,
		Metal,
		Leather,
		Scales,
		Wood
       
	}

	public class CraftAttributeInfo
	{
		private int m_WeaponFireDamage;
		private int m_WeaponColdDamage;
		private int m_WeaponPoisonDamage;
		private int m_WeaponEnergyDamage;
		private int m_WeaponChaosDamage;
		private int m_WeaponDirectDamage;
		private int m_WeaponDurability;
		private int m_WeaponLuck;
		private int m_WeaponGoldIncrease;
		private int m_WeaponLowerRequirements;

		private int m_ArmorPhysicalResist;
		private int m_ArmorFireResist;
		private int m_ArmorColdResist;
		private int m_ArmorPoisonResist;
		private int m_ArmorEnergyResist;
		private int m_ArmorDurability;
		private int m_ArmorLuck;
		private int m_ArmorGoldIncrease;
		private int m_ArmorLowerRequirements;

		private int m_RunicMinAttributes;
		private int m_RunicMaxAttributes;
		private int m_RunicMinIntensity;
		private int m_RunicMaxIntensity;

        private WeaponDurabilityLevel mWeaponDurabilityLevel;
        private WeaponAccuracyLevel mWeaponAccuracyLevel;
        private WeaponDamageLevel mWeaponDamageLevel;

        private ArmorDurabilityLevel mArmorDurabilityLevel;
        private ArmorProtectionLevel mArmorProtectionLevel;

        private SlayerName mSlayer;

		public int WeaponFireDamage{ get{ return m_WeaponFireDamage; } set{ m_WeaponFireDamage = value; } }
		public int WeaponColdDamage{ get{ return m_WeaponColdDamage; } set{ m_WeaponColdDamage = value; } }
		public int WeaponPoisonDamage{ get{ return m_WeaponPoisonDamage; } set{ m_WeaponPoisonDamage = value; } }
		public int WeaponEnergyDamage{ get{ return m_WeaponEnergyDamage; } set{ m_WeaponEnergyDamage = value; } }
		public int WeaponChaosDamage{ get{ return m_WeaponChaosDamage; } set{ m_WeaponChaosDamage = value; } }
		public int WeaponDirectDamage{ get{ return m_WeaponDirectDamage; } set{ m_WeaponDirectDamage = value; } }
		public int WeaponDurability{ get{ return m_WeaponDurability; } set{ m_WeaponDurability = value; } }
		public int WeaponLuck{ get{ return m_WeaponLuck; } set{ m_WeaponLuck = value; } }
		public int WeaponGoldIncrease{ get{ return m_WeaponGoldIncrease; } set{ m_WeaponGoldIncrease = value; } }
		public int WeaponLowerRequirements{ get{ return m_WeaponLowerRequirements; } set{ m_WeaponLowerRequirements = value; } }

		public int ArmorPhysicalResist{ get{ return m_ArmorPhysicalResist; } set{ m_ArmorPhysicalResist = value; } }
		public int ArmorFireResist{ get{ return m_ArmorFireResist; } set{ m_ArmorFireResist = value; } }
		public int ArmorColdResist{ get{ return m_ArmorColdResist; } set{ m_ArmorColdResist = value; } }
		public int ArmorPoisonResist{ get{ return m_ArmorPoisonResist; } set{ m_ArmorPoisonResist = value; } }
		public int ArmorEnergyResist{ get{ return m_ArmorEnergyResist; } set{ m_ArmorEnergyResist = value; } }
		public int ArmorDurability{ get{ return m_ArmorDurability; } set{ m_ArmorDurability = value; } }
		public int ArmorLuck{ get{ return m_ArmorLuck; } set{ m_ArmorLuck = value; } }
		public int ArmorGoldIncrease{ get{ return m_ArmorGoldIncrease; } set{ m_ArmorGoldIncrease = value; } }
		public int ArmorLowerRequirements{ get{ return m_ArmorLowerRequirements; } set{ m_ArmorLowerRequirements = value; } }

		public int RunicMinAttributes{ get{ return m_RunicMinAttributes; } set{ m_RunicMinAttributes = value; } }
		public int RunicMaxAttributes{ get{ return m_RunicMaxAttributes; } set{ m_RunicMaxAttributes = value; } }
		public int RunicMinIntensity{ get{ return m_RunicMinIntensity; } set{ m_RunicMinIntensity = value; } }
		public int RunicMaxIntensity{ get{ return m_RunicMaxIntensity; } set{ m_RunicMaxIntensity = value; } }

        public WeaponDurabilityLevel WeaponDurabilityLevel { get { return mWeaponDurabilityLevel; } set { mWeaponDurabilityLevel = value; } }
        public WeaponAccuracyLevel WeaponAcurracyLevel { get { return mWeaponAccuracyLevel; } set { mWeaponAccuracyLevel = value; } }
        public WeaponDamageLevel WeaponDamageLevel { get { return mWeaponDamageLevel; } set { mWeaponDamageLevel = value; } }

        public ArmorDurabilityLevel ArmorDurabilityLevel { get { return mArmorDurabilityLevel; } set { mArmorDurabilityLevel = value; } }
        public ArmorProtectionLevel ArmorProtectionLevel { get { return mArmorProtectionLevel; } set { mArmorProtectionLevel = value; } }

        public SlayerName Slayer { get { return mSlayer; } set { mSlayer = value; } }
		public CraftAttributeInfo()
		{
		}

		public static readonly CraftAttributeInfo Blank;
		public static readonly CraftAttributeInfo DullCopper, ShadowIron, Copper, Bronze, Golden, Agapite, Verite, Valorite;
		public static readonly CraftAttributeInfo Spined, Horned, Barbed;
		public static readonly CraftAttributeInfo OakWood, AshWood, YewWood;
        public static readonly CraftAttributeInfo ArtifactOfRuin, ArtifactOfMight, ArtifactOfForce, ArtifaceOfPower, ArtifactOfVanquishing, 
            ArtifactOfAccuracy, ArtifactOfSurpassingAccuracy, ArtifactOfEminentAccuracy, ArtifactOfExceedingAccuracy, ArtifactOfSupremeAccuracy, ArtifactOfDefense,
            ArtifactOfGuarding, ArtifactOfHardening, ArtifactOfFortification, ArtifactOfInvulnerability, ArtifactOfUndeadSlayer, ArtifactOfElementalSlayer,
            ArtifactOfArachnidSlayer, ArtifactOfRepondSlayer, ArtifactOfReptileSlayer; 

		static CraftAttributeInfo()
		{
			Blank = new CraftAttributeInfo();

			CraftAttributeInfo dullCopper = DullCopper = new CraftAttributeInfo();

			dullCopper.ArmorDurability = 5;
			dullCopper.WeaponDurability = 5;

			CraftAttributeInfo shadowIron = ShadowIron = new CraftAttributeInfo();

			shadowIron.ArmorDurability = 5;
			shadowIron.WeaponDurability = 5;

			CraftAttributeInfo copper = Copper = new CraftAttributeInfo();

            copper.ArmorDurability = 10;
            copper.WeaponDurability = 10;

			CraftAttributeInfo bronze = Bronze = new CraftAttributeInfo();

            bronze.ArmorDurability = 10;
            bronze.WeaponDurability = 10;

			CraftAttributeInfo golden = Golden = new CraftAttributeInfo();

            golden.ArmorDurability = 15;
            golden.WeaponDurability = 15;

			CraftAttributeInfo agapite = Agapite = new CraftAttributeInfo();

            agapite.ArmorDurability = 15;
            agapite.WeaponDurability = 15;
		
			CraftAttributeInfo verite = Verite = new CraftAttributeInfo();

            verite.ArmorDurability = 20;
            verite.WeaponDurability = 20;

			CraftAttributeInfo valorite = Valorite = new CraftAttributeInfo();

			valorite.ArmorDurability = 25;
            valorite.WeaponDurability = 25;

            //artifacts
            
            ArtifactOfRuin = new CraftAttributeInfo();
            ArtifactOfRuin.WeaponDamageLevel = WeaponDamageLevel.Ruin;

            ArtifactOfMight = new CraftAttributeInfo();
            ArtifactOfMight.WeaponDamageLevel = WeaponDamageLevel.Might;

            ArtifactOfForce = new CraftAttributeInfo();
            ArtifactOfForce.WeaponDamageLevel = WeaponDamageLevel.Force;

            ArtifaceOfPower = new CraftAttributeInfo();
            ArtifaceOfPower.WeaponDamageLevel = WeaponDamageLevel.Power;

            ArtifactOfVanquishing = new CraftAttributeInfo();
            ArtifactOfVanquishing.WeaponDamageLevel = WeaponDamageLevel.Vanq;

            ArtifactOfAccuracy = new CraftAttributeInfo();
            ArtifactOfAccuracy.WeaponAcurracyLevel = WeaponAccuracyLevel.Accurate;

            ArtifactOfSurpassingAccuracy = new CraftAttributeInfo();
            ArtifactOfSurpassingAccuracy.WeaponAcurracyLevel = WeaponAccuracyLevel.Surpassingly;

            ArtifactOfEminentAccuracy = new CraftAttributeInfo();
            ArtifactOfEminentAccuracy.WeaponAcurracyLevel = WeaponAccuracyLevel.Eminently;

            ArtifactOfExceedingAccuracy = new CraftAttributeInfo();
            ArtifactOfExceedingAccuracy.WeaponAcurracyLevel = WeaponAccuracyLevel.Exceedingly;

            ArtifactOfSupremeAccuracy = new CraftAttributeInfo();
            ArtifactOfSupremeAccuracy.WeaponAcurracyLevel = WeaponAccuracyLevel.Supremely;

            ArtifactOfDefense = new CraftAttributeInfo();
            ArtifactOfDefense.ArmorProtectionLevel = ArmorProtectionLevel.Defense;

            ArtifactOfGuarding = new CraftAttributeInfo();
            ArtifactOfGuarding.ArmorProtectionLevel = ArmorProtectionLevel.Guarding;

            ArtifactOfHardening = new CraftAttributeInfo();
            ArtifactOfHardening.ArmorProtectionLevel = ArmorProtectionLevel.Hardening;

            ArtifactOfFortification = new CraftAttributeInfo();
            ArtifactOfFortification.ArmorProtectionLevel = ArmorProtectionLevel.Fortification;

            ArtifactOfInvulnerability = new CraftAttributeInfo();
            ArtifactOfInvulnerability.ArmorProtectionLevel = ArmorProtectionLevel.Invulnerability;
            
            ArtifactOfUndeadSlayer = new CraftAttributeInfo();
            ArtifactOfUndeadSlayer.Slayer = SlayerName.Silver;

            ArtifactOfElementalSlayer = new CraftAttributeInfo();
            ArtifactOfElementalSlayer.Slayer = SlayerName.ElementalBan;

            ArtifactOfArachnidSlayer = new CraftAttributeInfo();
            ArtifactOfArachnidSlayer.Slayer = SlayerName.ArachnidDoom;

            ArtifactOfRepondSlayer = new CraftAttributeInfo();
            ArtifactOfRepondSlayer.Slayer = SlayerName.Repond;

            ArtifactOfReptileSlayer = new CraftAttributeInfo();
            ArtifactOfReptileSlayer.Slayer = SlayerName.ReptilianDeath;

			CraftAttributeInfo spined = Spined = new CraftAttributeInfo();
            Spined.ArmorDurabilityLevel = ArmorDurabilityLevel.Substantial;

			CraftAttributeInfo horned = Horned = new CraftAttributeInfo();
            Horned.ArmorDurabilityLevel = ArmorDurabilityLevel.Massive;

			CraftAttributeInfo barbed = Barbed = new CraftAttributeInfo();
            Barbed.ArmorDurabilityLevel = ArmorDurabilityLevel.Fortified;

			OakWood = new CraftAttributeInfo();
            OakWood.ArmorDurabilityLevel = ArmorDurabilityLevel.Durable;
            OakWood.WeaponDurabilityLevel = WeaponDurabilityLevel.Durable;

			AshWood = new CraftAttributeInfo();
            AshWood.ArmorDurabilityLevel = ArmorDurabilityLevel.Substantial;
            AshWood.WeaponDurabilityLevel = WeaponDurabilityLevel.Substantial;

			YewWood = new CraftAttributeInfo();
            YewWood.ArmorDurabilityLevel = ArmorDurabilityLevel.Massive;
            YewWood.WeaponDurabilityLevel = WeaponDurabilityLevel.Massive;
	
		}
	}
	public class CraftResourceInfo
	{
		private int m_Hue;
		private int m_Number;
		private string m_Name;
		private CraftAttributeInfo m_AttributeInfo;
		private CraftResource m_Resource;
		private Type[] m_ResourceTypes;

		public int Hue{ get{ return m_Hue; } }
		public int Number{ get{ return m_Number; } }
		public string Name{ get{ return m_Name; } }
		public CraftAttributeInfo AttributeInfo{ get{ return m_AttributeInfo; } }
		public CraftResource Resource{ get{ return m_Resource; } }
		public Type[] ResourceTypes{ get{ return m_ResourceTypes; } }

		public CraftResourceInfo( int hue, int number, string name, CraftAttributeInfo attributeInfo, CraftResource resource, params Type[] resourceTypes )
		{
			m_Hue = hue;
			m_Number = number;
			m_Name = name;
			m_AttributeInfo = attributeInfo;
			m_Resource = resource;
			m_ResourceTypes = resourceTypes;

			for ( int i = 0; i < resourceTypes.Length; ++i )
				CraftResources.RegisterType( resourceTypes[i], resource );
		}
	}

	public class CraftResources
	{
		private static CraftResourceInfo[] m_MetalInfo = new CraftResourceInfo[]
			{
				new CraftResourceInfo( 0x000, 1053109, "Iron",			CraftAttributeInfo.Blank,		CraftResource.Iron,				typeof( IronIngot ),		typeof( IronOre ),			typeof( Granite ) ),
				new CraftResourceInfo( 0x973, 1053108, "Dull Copper",	CraftAttributeInfo.DullCopper,	CraftResource.DullCopper,		typeof( DullCopperIngot ),	typeof( DullCopperOre ),	typeof( DullCopperGranite ) ),
				new CraftResourceInfo( 0x966, 1053107, "Shadow Iron",	CraftAttributeInfo.ShadowIron,	CraftResource.ShadowIron,		typeof( ShadowIronIngot ),	typeof( ShadowIronOre ),	typeof( ShadowIronGranite ) ),
				new CraftResourceInfo( 0x96D, 1053106, "Copper",		CraftAttributeInfo.Copper,		CraftResource.Copper,			typeof( CopperIngot ),		typeof( CopperOre ),		typeof( CopperGranite ) ),
				new CraftResourceInfo( 0x972, 1053105, "Bronze",		CraftAttributeInfo.Bronze,		CraftResource.Bronze,			typeof( BronzeIngot ),		typeof( BronzeOre ),		typeof( BronzeGranite ) ),
				new CraftResourceInfo( 0x8A5, 1053104, "Gold",			CraftAttributeInfo.Golden,		CraftResource.Gold,				typeof( GoldIngot ),		typeof( GoldOre ),			typeof( GoldGranite ) ),
				new CraftResourceInfo( 0x979, 1053103, "Agapite",		CraftAttributeInfo.Agapite,		CraftResource.Agapite,			typeof( AgapiteIngot ),		typeof( AgapiteOre ),		typeof( AgapiteGranite ) ),
				new CraftResourceInfo( 0x89F, 1053102, "Verite",		CraftAttributeInfo.Verite,		CraftResource.Verite,			typeof( VeriteIngot ),		typeof( VeriteOre ),		typeof( VeriteGranite ) ),
				new CraftResourceInfo( 0x8AB, 1053101, "Valorite",		CraftAttributeInfo.Valorite,	CraftResource.Valorite,			typeof( ValoriteIngot ),	typeof( ValoriteOre ),		typeof( ValoriteGranite ) ),
			};

		private static CraftResourceInfo[] m_LeatherInfo = new CraftResourceInfo[]
			{
				new CraftResourceInfo( 0x000, 1049353, "Normal",		CraftAttributeInfo.Blank,		CraftResource.RegularLeather,	typeof( Leather ),			typeof( Hides ) ),
				new CraftResourceInfo( 0x904, 1049354, "Spined",		CraftAttributeInfo.Spined,		CraftResource.SpinedLeather,	typeof( SpinedLeather ),	typeof( SpinedHides ) ),
				new CraftResourceInfo( 0x903, 1049355, "Horned",		CraftAttributeInfo.Horned,		CraftResource.HornedLeather,	typeof( HornedLeather ),	typeof( HornedHides ) ),
				new CraftResourceInfo( 0x900, 1049356, "Barbed",		CraftAttributeInfo.Barbed,		CraftResource.BarbedLeather,	typeof( BarbedLeather ),	typeof( BarbedHides ) )
			};

		private static CraftResourceInfo[] m_WoodInfo = new CraftResourceInfo[]
			{
				new CraftResourceInfo( 0x000, 1011542, "Normal",		CraftAttributeInfo.Blank,		CraftResource.RegularWood,	typeof( Log ),			typeof( Board ) ),
				new CraftResourceInfo( 0x7DA, 1072533, "Oak",			CraftAttributeInfo.OakWood,		CraftResource.OakWood,		typeof( OakLog ),		typeof( OakBoard ) ),
				new CraftResourceInfo( 0x4A7, 1072534, "Ash",			CraftAttributeInfo.AshWood,		CraftResource.AshWood,		typeof( AshLog ),		typeof( AshBoard ) ),
				new CraftResourceInfo( 0x4A8, 1072535, "Yew",			CraftAttributeInfo.YewWood,		CraftResource.YewWood,		typeof( YewLog ),		typeof( YewBoard ) ),
			};
        private static CraftResourceInfo[] mArtifactInfo = new CraftResourceInfo[]
        {
            new CraftResourceInfo(0x000, 1063491, "Artifact of Might", CraftAttributeInfo.ArtifactOfMight, CraftResource.ArtifactOfMight, typeof(ArtifactOfMight), typeof(ArtifactOfMight)),
            new CraftResourceInfo(0x000, 1063492, "Artifact of Force", CraftAttributeInfo.ArtifactOfForce, CraftResource.ArtifactOfForce, typeof(ArtifactOfForce), typeof(ArtifactOfForce)),
            new CraftResourceInfo(0x000, 1063493, "Artifact of Power", CraftAttributeInfo.ArtifaceOfPower, CraftResource.ArtifactOfPower, typeof(ArtifactOfPower), typeof(ArtifactOfPower)),
            new CraftResourceInfo(0x000, 1063494, "Artifact of Vanquishing", CraftAttributeInfo.ArtifactOfVanquishing, CraftResource.ArtifactOfVanquishing, typeof(ArtifactOfVanquishing), typeof(ArtifactOfVanquishing)),
            new CraftResourceInfo(0x000, 1063501, "Artifact of Surpassing Accuracy", CraftAttributeInfo.ArtifactOfSurpassingAccuracy, CraftResource.ArtifactOfSurpassingAccuracy, typeof(ArtifactOfSurpassingAccuracy), typeof(ArtifactOfSurpassingAccuracy)),
            new CraftResourceInfo(0x000, 1063502, "Artifact of Eminent Accuracy", CraftAttributeInfo.ArtifactOfEminentAccuracy, CraftResource.ArtifactOfEminentAccuracy, typeof(ArtifactOfEminentAccuracy), typeof(ArtifactOfEminentAccuracy)),
            new CraftResourceInfo(0x000, 1063503, "Artifact of Exceeding Accuracy", CraftAttributeInfo.ArtifactOfExceedingAccuracy, CraftResource.ArtifactOfExceedingAccuracy, typeof(ArtifactOfExceedingAccuracy), typeof(ArtifactOfExceedingAccuracy)),
            new CraftResourceInfo(0x000, 1063504, "Artifact of Supreme Accuracy", CraftAttributeInfo.ArtifactOfSupremeAccuracy, CraftResource.ArtifactOfSupremeAccuracy, typeof(ArtifactOfSupremeAccuracy), typeof(ArtifactOfSupremeAccuracy)),
            new CraftResourceInfo(0x000, 1063506, "Artifact of Guarding", CraftAttributeInfo.ArtifactOfGuarding, CraftResource.ArtifactOfGuarding, typeof(ArtifactOfGuarding), typeof(ArtifactOfGuarding)),
            new CraftResourceInfo(0x000, 1063507, "Artifact of Hardening", CraftAttributeInfo.ArtifactOfHardening, CraftResource.ArtifactOfHardening, typeof(ArtifactOfHardening), typeof(ArtifactOfHardening)),
            new CraftResourceInfo(0x000, 1063508, "Artifact of Fortification", CraftAttributeInfo.ArtifactOfFortification, CraftResource.ArtifactOfFortification, typeof(ArtifactOfFortification), typeof(ArtifactOfFortification)),
            new CraftResourceInfo(0x000, 1063509, "Artifact of Invulnerability", CraftAttributeInfo.ArtifactOfInvulnerability, CraftResource.ArtifactofInvulnerability, typeof(ArtifactOfInvulnerability), typeof(ArtifactOfInvulnerability)),
            new CraftResourceInfo(0x000, 1063510, "Artifact of Silver", CraftAttributeInfo.ArtifactOfUndeadSlayer, CraftResource.ArtifactOfUndeadSlayer, typeof(ArtifactOfSilver), typeof(ArtifactOfSilver)),
            new CraftResourceInfo(0x000, 1063511, "Artifact of Elemental Slaying", CraftAttributeInfo.ArtifactOfElementalSlayer, CraftResource.ArtifactOfElementalSlayer, typeof(ArtifactOfElementalSlaying), typeof(ArtifactOfElementalSlaying)),
            new CraftResourceInfo(0x000, 1063512, "Artifact of Arachnid Slaying", CraftAttributeInfo.ArtifactOfArachnidSlayer, CraftResource.ArtifactOfArachnidSlayer, typeof(ArtifactOfArachnidSlaying), typeof(ArtifactOfArachnidSlaying)),
            new CraftResourceInfo(0x000, 1063513, "Artifact of Repond Slaying", CraftAttributeInfo.ArtifactOfRepondSlayer, CraftResource.ArtifactOfRepondSlayer, typeof(ArtifactOfRepondSlaying), typeof(ArtifactOfRepondSlaying)),
            new CraftResourceInfo(0x000, 1063514, "Artifact of Reptile Slaying", CraftAttributeInfo.ArtifactOfReptileSlayer, CraftResource.ArtifactOfReptileSlayer, typeof(ArtifactOfReptileSlaying), typeof(ArtifactOfReptileSlaying))
        };
		/// <summary>
		/// Returns true if '<paramref name="resource"/>' is None, Iron, RegularLeather or RegularWood. False if otherwise.
		/// </summary>
		public static bool IsStandard( CraftResource resource )
		{
			return ( resource == CraftResource.None || resource == CraftResource.Iron || resource == CraftResource.RegularLeather || resource == CraftResource.RegularWood );
		}

		private static Hashtable m_TypeTable;

		/// <summary>
		/// Registers that '<paramref name="resourceType"/>' uses '<paramref name="resource"/>' so that it can later be queried by <see cref="CraftResources.GetFromType"/>
		/// </summary>
		public static void RegisterType( Type resourceType, CraftResource resource )
		{
			if ( m_TypeTable == null )
				m_TypeTable = new Hashtable();

			m_TypeTable[resourceType] = resource;
		}

		/// <summary>
		/// Returns the <see cref="CraftResource"/> value for which '<paramref name="resourceType"/>' uses -or- CraftResource.None if an unregistered type was specified.
		/// </summary>
		public static CraftResource GetFromType( Type resourceType )
		{
			if ( m_TypeTable == null )
				return CraftResource.None;

			object obj = m_TypeTable[resourceType];

			if ( !(obj is CraftResource) )
				return CraftResource.None;

			return (CraftResource)obj;
		}

		/// <summary>
		/// Returns a <see cref="CraftResourceInfo"/> instance describing '<paramref name="resource"/>' -or- null if an invalid resource was specified.
		/// </summary>
		public static CraftResourceInfo GetInfo( CraftResource resource )
		{
			CraftResourceInfo[] list = null;

			switch ( GetType( resource ) )
			{
				case CraftResourceType.Metal: list = m_MetalInfo; break;
				case CraftResourceType.Leather: list = m_LeatherInfo; break;
				case CraftResourceType.Wood: list = m_WoodInfo; break;
                case CraftResourceType.Artifact: list = mArtifactInfo; break;
			}

			if ( list != null )
			{
				int index = GetIndex( resource );

				if ( index >= 0 && index < list.Length )
					return list[index];
			}

			return null;
		}

		/// <summary>
		/// Returns a <see cref="CraftResourceType"/> value indiciating the type of '<paramref name="resource"/>'.
		/// </summary>
		public static CraftResourceType GetType( CraftResource resource )
		{
			if ( resource >= CraftResource.Iron && resource <= CraftResource.Valorite )
				return CraftResourceType.Metal;

			if ( resource >= CraftResource.RegularLeather && resource <= CraftResource.BarbedLeather )
				return CraftResourceType.Leather;

			if ( resource >= CraftResource.RedScales && resource <= CraftResource.BlueScales )
				return CraftResourceType.Scales;

			if ( resource >= CraftResource.RegularWood && resource <= CraftResource.YewWood )
				return CraftResourceType.Wood;

            if (resource >= CraftResource.ArtifactOfMight && resource <= CraftResource.ArtifactOfReptileSlayer)
                return CraftResourceType.Artifact;

			return CraftResourceType.None;
		}

		/// <summary>
		/// Returns the first <see cref="CraftResource"/> in the series of resources for which '<paramref name="resource"/>' belongs.
		/// </summary>
		public static CraftResource GetStart( CraftResource resource )
		{
			switch ( GetType( resource ) )
			{
				case CraftResourceType.Metal: return CraftResource.Iron;
				case CraftResourceType.Leather: return CraftResource.RegularLeather;
				case CraftResourceType.Scales: return CraftResource.RedScales;
				case CraftResourceType.Wood: return CraftResource.RegularWood;
                case CraftResourceType.Artifact: return CraftResource.ArtifactOfMight;
			}

			return CraftResource.None;
		}

		/// <summary>
		/// Returns the index of '<paramref name="resource"/>' in the seriest of resources for which it belongs.
		/// </summary>
		public static int GetIndex( CraftResource resource )
		{
			CraftResource start = GetStart( resource );

			if ( start == CraftResource.None )
				return 0;

			return (int)(resource - start);
		}

		/// <summary>
		/// Returns the <see cref="CraftResourceInfo.Number"/> property of '<paramref name="resource"/>' -or- 0 if an invalid resource was specified.
		/// </summary>
		public static int GetLocalizationNumber( CraftResource resource )
		{
			CraftResourceInfo info = GetInfo( resource );

			return ( info == null ? 0 : info.Number );
		}

		/// <summary>
		/// Returns the <see cref="CraftResourceInfo.Hue"/> property of '<paramref name="resource"/>' -or- 0 if an invalid resource was specified.
		/// </summary>
		public static int GetHue( CraftResource resource )
		{
			CraftResourceInfo info = GetInfo( resource );

			return ( info == null ? 0 : info.Hue );
		}

		/// <summary>
		/// Returns the <see cref="CraftResourceInfo.Name"/> property of '<paramref name="resource"/>' -or- an empty string if the resource specified was invalid.
		/// </summary>
		public static string GetName( CraftResource resource )
		{
			CraftResourceInfo info = GetInfo( resource );

			return ( info == null ? String.Empty : info.Name );
		}

		/// <summary>
		/// Returns the <see cref="CraftResource"/> value which represents '<paramref name="info"/>' -or- CraftResource.None if unable to convert.
		/// </summary>
		public static CraftResource GetFromOreInfo( OreInfo info )
		{
			if ( info.Name.IndexOf( "Spined" ) >= 0 )
				return CraftResource.SpinedLeather;
			else if ( info.Name.IndexOf( "Horned" ) >= 0 )
				return CraftResource.HornedLeather;
			else if ( info.Name.IndexOf( "Barbed" ) >= 0 )
				return CraftResource.BarbedLeather;
			else if ( info.Name.IndexOf( "Leather" ) >= 0 )
				return CraftResource.RegularLeather;

			if ( info.Level == 0 )
				return CraftResource.Iron;
			else if ( info.Level == 1 )
				return CraftResource.DullCopper;
			else if ( info.Level == 2 )
				return CraftResource.ShadowIron;
			else if ( info.Level == 3 )
				return CraftResource.Copper;
			else if ( info.Level == 4 )
				return CraftResource.Bronze;
			else if ( info.Level == 5 )
				return CraftResource.Gold;
			else if ( info.Level == 6 )
				return CraftResource.Agapite;
			else if ( info.Level == 7 )
				return CraftResource.Verite;
			else if ( info.Level == 8 )
				return CraftResource.Valorite;

			return CraftResource.None;
		}

		/// <summary>
		/// Returns the <see cref="CraftResource"/> value which represents '<paramref name="info"/>', using '<paramref name="material"/>' to help resolve leather OreInfo instances.
		/// </summary>
		public static CraftResource GetFromOreInfo( OreInfo info, ArmorMaterialType material )
		{
			if ( material == ArmorMaterialType.Studded || material == ArmorMaterialType.Leather || material == ArmorMaterialType.Spined ||
				material == ArmorMaterialType.Horned || material == ArmorMaterialType.Barbed )
			{
				if ( info.Level == 0 )
					return CraftResource.RegularLeather;
				else if ( info.Level == 1 )
					return CraftResource.SpinedLeather;
				else if ( info.Level == 2 )
					return CraftResource.HornedLeather;
				else if ( info.Level == 3 )
					return CraftResource.BarbedLeather;

				return CraftResource.None;
			}

			return GetFromOreInfo( info );
		}
	}

	// NOTE: This class is only for compatability with very old RunUO versions.
	// No changes to it should be required for custom resources.
	public class OreInfo
	{
		public static readonly OreInfo Iron			= new OreInfo( 0, 0x000, "Iron" );
		public static readonly OreInfo DullCopper	= new OreInfo( 1, 0x973, "Dull Copper" );
		public static readonly OreInfo ShadowIron	= new OreInfo( 2, 0x966, "Shadow Iron" );
		public static readonly OreInfo Copper		= new OreInfo( 3, 0x96D, "Copper" );
		public static readonly OreInfo Bronze		= new OreInfo( 4, 0x972, "Bronze" );
		public static readonly OreInfo Gold			= new OreInfo( 5, 0x8A5, "Gold" );
		public static readonly OreInfo Agapite		= new OreInfo( 6, 0x979, "Agapite" );
		public static readonly OreInfo Verite		= new OreInfo( 7, 0x89F, "Verite" );
		public static readonly OreInfo Valorite		= new OreInfo( 8, 0x8AB, "Valorite" );

		private int m_Level;
		private int m_Hue;
		private string m_Name;

		public OreInfo( int level, int hue, string name )
		{
			m_Level = level;
			m_Hue = hue;
			m_Name = name;
		}

		public int Level
		{
			get
			{
				return m_Level;
			}
		}

		public int Hue
		{
			get
			{
				return m_Hue;
			}
		}

		public string Name
		{
			get
			{
				return m_Name;
			}
		}
	}
}