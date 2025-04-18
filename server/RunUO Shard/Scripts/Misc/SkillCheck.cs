using System;
using Server;
using Server.Mobiles;
using Server.Factions;
using Server.Scripts;
using Server.Regions;
using Server.Items;

namespace Server.Misc
{
    public class SkillCheck
    {
        private static readonly bool AntiMacroCode = false; //!Core.ML;		//Change this to false to disable anti-macro code

        public static TimeSpan AntiMacroExpire = TimeSpan.FromMinutes(5.0); //How long do we remember targets/locations?
        public const int Allowance = 3;	//How many times may we use the same location/target for gain
        private const int LocationSize = 5; //The size of eeach location, make this smaller so players dont have to move as far
        private static bool[] UseAntiMacro = new bool[]
		{
			// true if this skill uses the anti-macro code, false if it does not
			false,// Alchemy = 0,
			true,// Anatomy = 1,
			true,// AnimalLore = 2,
			true,// ItemID = 3,
			true,// ArmsLore = 4,
			false,// Parry = 5,
			true,// Begging = 6,
			false,// Blacksmith = 7,
			false,// Fletching = 8,
			true,// Peacemaking = 9,
			true,// Camping = 10,
			false,// Carpentry = 11,
			false,// Cartography = 12,
			false,// Cooking = 13,
			true,// DetectHidden = 14,
			true,// Discordance = 15,
			true,// EvalInt = 16,
			true,// Healing = 17,
			true,// Fishing = 18,
			true,// Forensics = 19,
			true,// Herding = 20,
			true,// Hiding = 21,
			true,// Provocation = 22,
			false,// Inscribe = 23,
			true,// Lockpicking = 24,
			true,// Magery = 25,
			true,// MagicResist = 26,
			false,// Tactics = 27,
			true,// Snooping = 28,
			true,// Musicianship = 29,
			true,// Poisoning = 30,
			false,// Archery = 31,
			true,// SpiritSpeak = 32,
			true,// Stealing = 33,
			false,// Tailoring = 34,
			true,// AnimalTaming = 35,
			true,// TasteID = 36,
			false,// Tinkering = 37,
			true,// Tracking = 38,
			true,// Veterinary = 39,
			false,// Swords = 40,
			false,// Macing = 41,
			false,// Fencing = 42,
			false,// Wrestling = 43,
			true,// Lumberjacking = 44,
			true,// Mining = 45,
			true,// Meditation = 46,
			true,// Stealth = 47,
			true,// RemoveTrap = 48,
			true,// Necromancy = 49,
			false,// Focus = 50,
			true,// Chivalry = 51
			true,// Bushido = 52
			true,//Ninjitsu = 53
			true // Spellweaving
		};

        public static void Initialize()
        {
            // Took the follow 4 lines of code out to replace with XMLSpawner skill triggering for spawns.  
            // Mobile.SkillCheckLocationHandler = new SkillCheckLocationHandler(Mobile_SkillCheckLocation);
            // Mobile.SkillCheckDirectLocationHandler = new SkillCheckDirectLocationHandler(Mobile_SkillCheckDirectLocation);

            // Mobile.SkillCheckTargetHandler = new SkillCheckTargetHandler(Mobile_SkillCheckTarget);
            // Mobile.SkillCheckDirectTargetHandler = new SkillCheckDirectTargetHandler(Mobile_SkillCheckDirectTarget);
         
            // Begin mod to enable XmlSpawner skill triggering
            Mobile.SkillCheckLocationHandler = new SkillCheckLocationHandler(XmlSpawnerSkillCheck.Mobile_SkillCheckLocation);
            Mobile.SkillCheckDirectLocationHandler = new SkillCheckDirectLocationHandler(XmlSpawnerSkillCheck.Mobile_SkillCheckDirectLocation);
            Mobile.SkillCheckTargetHandler = new SkillCheckTargetHandler(XmlSpawnerSkillCheck.Mobile_SkillCheckTarget);
            Mobile.SkillCheckDirectTargetHandler = new SkillCheckDirectTargetHandler(XmlSpawnerSkillCheck.Mobile_SkillCheckDirectTarget);
            // End mod to enable XmlSpawner skill triggering
            
        

        
        
        
        
        
        }

        public static bool CheckSkill(Mobile from, Skill skill, object amObj, double chance)
        {
            double SkillGainChance = 0.0;

            double ExpectedTime = 0.25;
            double SkillPointsInRange = 50.0;
            double SkillInterval = 1.0;
            double ConstantModifier = 1.0;
            double LocationFactor = 1.0;
            double CitizenshipFactor = 1.0;

            bool skillSuccess = false;
            bool skillGain = false;

            //Check for Skill Use Success 
            if (chance >= Utility.RandomDouble())
            {
                skillSuccess = true;
            }

            //Double Check This
            if (from.Skills.Cap == 0)
                return false;

            //Begin Skill Gain Check

            //Determine Skill Range
            int CurrentRange = (int)(Math.Floor((double)skill.Base / (SkillPointsInRange / 10)));

            if (CurrentRange >= FeatureList.SkillChanges.SkillRanges.GetLength(0))
            {
                CurrentRange = FeatureList.SkillChanges.SkillRanges.GetLength(0) - 1;
            }

            //Gets Expected Time Needed To Complete Full Skill Range            
            ExpectedTime = FeatureList.SkillChanges.SkillRanges[CurrentRange, 1];

            //Convert Expected Time into Seconds
            ExpectedTime *= 3600;

            //Get Skill Interval from FeatureList Table for Skill
            SkillInterval = FeatureList.SkillChanges.SkillIntervals[skill.SkillName];

            //SkillCheck For Reactive Skills
            if (SkillInterval == 0.0)
            {
                //Use Estimates For Frequency (Temporary Solution)
                if (skill.SkillName == SkillName.Parry)
                {
                    SkillInterval = 1.5;
                }

                if (skill.SkillName == SkillName.MagicResist)
                {
                    SkillInterval = 2;
                }
            }

            //SkillCheck For Speed-Based Skills
            if (SkillInterval == -1.0)
            {
                if (skill.SkillName == SkillName.Healing)
                {
                    //Don't Know If Healing Self or Other, so Average in Between, With Bonus for Dex                    
                    SkillInterval = 10 - (from.Dex / 25);
                }

                else //Combat Skill
                {
                    BaseWeapon weapon = from.Weapon as BaseWeapon;

                    SkillInterval = weapon.Speed / (((double)from.Stam + 100) / 100);
                }
            }

            //Constant Modifier
            ConstantModifier = FeatureList.SkillChanges.SkillConstantMod[skill.SkillName];

            //Location Factors
            if (from.Region.IsPartOf(typeof(GuardedRegion)))
            {
                LocationFactor = FeatureList.SkillChanges.TownFactor;
            }

            else if (from.Region.IsPartOf(typeof(HouseRegion)))
            {
                LocationFactor = FeatureList.SkillChanges.HouseFactor;
            }

            else if (from.Region.IsPartOf(typeof(DungeonRegion)))
            {
                LocationFactor = FeatureList.SkillChanges.DungeonFactor;
            }

            else //Wilderness
            {
                LocationFactor = FeatureList.SkillChanges.WildernessFactor;
            }

            //Citizenship Factor
            PlayerMobile pm = from as PlayerMobile;

            if (pm != null)
            {
                CitizenshipFactor = pm.GetSkillGainChanceBonus(skill);

                //Temporary
                if (CitizenshipFactor == 0)
                {
                    CitizenshipFactor = 1.0;
                }
            }

            //Determine Estimated Number of Skill Usages Needed for 1 Skill Point Gain
            SkillGainChance = 1 / (ExpectedTime / SkillPointsInRange / SkillInterval / LocationFactor / ConstantModifier / CitizenshipFactor);

            //Determine SkillPoint Gain Amount
            int pointGain = 1;

            //Gain Multiple SkillPoints for Each additional 100% chance to gain
            if (SkillGainChance > 1)
            {
                pointGain = (int)Math.Floor(SkillGainChance);
            }

            //Check For SkillGain

            // No skill gain for players who are attacking doors
            if (from.Combatant != null && from.Combatant is IDamageableItemMilitiaRestricted)
                SkillGainChance = 0;

            skillGain = (SkillGainChance >= Utility.RandomDouble());

            //Skill Gain For Players That Are Alive
            if (skillGain && from.Player && from.Alive)
                Gain(from, skill, pointGain);

            return skillSuccess;
        }

        public static bool Mobile_SkillCheckLocation(Mobile from, SkillName skillName, double minSkill, double maxSkill)
        {
            Skill skill = from.Skills[skillName];

            if (skill == null)
                return false;

            double value = skill.Value;

            if (value < minSkill)
                return false; // Too difficult
            else if (value >= maxSkill)
                return true; // No challenge

            double chance = (value - minSkill) / (maxSkill - minSkill);

            Point2D loc = new Point2D(from.Location.X / LocationSize, from.Location.Y / LocationSize);

            return CheckSkill(from, skill, loc, chance);
        }

        public static bool Mobile_SkillCheckDirectLocation(Mobile from, SkillName skillName, double chance)
        {
            Skill skill = from.Skills[skillName];

            if (skill == null)
                return false;

            if (chance < 0.0)
                return false; // Too difficult
            else if (chance >= 1.0)
                return true; // No challenge

            Point2D loc = new Point2D(from.Location.X / LocationSize, from.Location.Y / LocationSize);

            return CheckSkill(from, skill, loc, chance);
        }

        public static bool Mobile_SkillCheckTarget(Mobile from, SkillName skillName, object target, double minSkill, double maxSkill)
        {
            Skill skill = from.Skills[skillName];

            if (skill == null)
                return false;

            double value = skill.Value;

            if (value < minSkill)
                return false; // Too difficult
            else if (value >= maxSkill)
                return true; // No challenge

            double chance = (value - minSkill) / (maxSkill - minSkill);

            return CheckSkill(from, skill, target, chance);
        }

        public static bool Mobile_SkillCheckDirectTarget(Mobile from, SkillName skillName, object target, double chance)
        {
            Skill skill = from.Skills[skillName];

            if (skill == null)
                return false;

            if (chance < 0.0)
                return false; // Too difficult
            else if (chance >= 1.0)
                return true; // No challenge

            return CheckSkill(from, skill, target, chance);
        }

        private static bool AllowGain(Mobile from, Skill skill, object obj)
        {
            if (AntiMacroCode && from is PlayerMobile && UseAntiMacro[skill.Info.SkillID])
                return ((PlayerMobile)from).AntiMacroCheck(skill, obj);
            else
                return true;
        }

        public enum Stat { Str, Dex, Int }

        public static void Gain(Mobile from, Skill skill, int amount)
        {
            if (from is PlayerMobile && Faction.InSkillLoss(from))
                return;

            if (from.Region.IsPartOf(typeof(Regions.Jail)))
                return;

            if (from is BaseCreature && ((BaseCreature)from).IsDeadPet)
                return;

            if (skill.SkillName == SkillName.Focus && from is BaseCreature)
                return;

            if (skill.Base < skill.Cap && skill.Lock == SkillLock.Up)
            {
                int toGain = amount;

                Skills skills = from.Skills;

                if (from.Player && (skills.Total / skills.Cap) >= Utility.RandomDouble()) //( skills.Total >= skills.Cap )
                {
                    for (int i = 0; i < skills.Length; ++i)
                    {
                        Skill toLower = skills[i];

                        if (toLower != skill && toLower.Lock == SkillLock.Down && toLower.BaseFixedPoint >= toGain)
                        {
                            toLower.BaseFixedPoint -= toGain;
                            break;
                        }
                    }
                }

                if (!from.Player || (skills.Total + toGain) <= skills.Cap)
                {
                    skill.BaseFixedPoint += toGain;
                }
            }

            SkillInfo info = skill.Info;

            if (from.StrLock == StatLockType.Up && (info.StrGain / FeatureList.SkillChanges.StatGainDivisionFactor) > Utility.RandomDouble())
            {
                GainStat(from, Stat.Str);
            }

            else if (from.DexLock == StatLockType.Up && (info.DexGain / FeatureList.SkillChanges.StatGainDivisionFactor) > Utility.RandomDouble())
            {
                GainStat(from, Stat.Dex);
            }

            else if (from.IntLock == StatLockType.Up && (info.IntGain / FeatureList.SkillChanges.StatGainDivisionFactor) > Utility.RandomDouble())
            {
                GainStat(from, Stat.Int);
            }          
        }

        private static TimeSpan m_StatGainDelay = TimeSpan.FromMinutes(0.5);
        private static TimeSpan m_PetStatGainDelay = TimeSpan.FromMinutes(5.0);

        public static void GainStat(Mobile from, Stat stat)
        {
            switch (stat)
            {
                case Stat.Str:
                    {
                        if (from is BaseCreature && ((BaseCreature)from).Controlled)
                        {
                            if ((from.LastStrGain + m_PetStatGainDelay) >= DateTime.Now)
                                return;
                        }

                        else if ((from.LastStrGain + m_StatGainDelay) >= DateTime.Now)
                            return;

                        from.LastStrGain = DateTime.Now;

                break;
                }

                case Stat.Dex:
                    {
                        if (from is BaseCreature && ((BaseCreature)from).Controlled)
                        {
                            if ((from.LastDexGain + m_PetStatGainDelay) >= DateTime.Now)
                                return;
                        }

                        else if ((from.LastDexGain + m_StatGainDelay) >= DateTime.Now)
                            return;

                        from.LastDexGain = DateTime.Now;
                break;
                }

                case Stat.Int:
                    {
                        if (from is BaseCreature && ((BaseCreature)from).Controlled)
                        {
                            if ((from.LastIntGain + m_PetStatGainDelay) >= DateTime.Now)
                                return;
                        }

                        else if ((from.LastIntGain + m_StatGainDelay) >= DateTime.Now)
                            return;

                        from.LastIntGain = DateTime.Now;
                        break;
                    }
            }

            //Current Only for Players
            if (from.Player)
            {
                IncreaseStat(from, stat);
            }
        }        

        public static void IncreaseStat(Mobile from, Stat stat)
        {
            switch (stat)
            {
                case Stat.Str:
                {
                    //Want Str To Go Up and Can Go Up
                    if (from.StrLock == StatLockType.Up && from.RawStr < 100)
                    {
                        //Under Stat Cap
                        if (from.RawStatTotal < from.StatCap)
                        {
                            ++from.RawStr;
                        }

                        //At Stat Cap
                        else
                        {
                            //Lower Dex
                            if (CanLower(from, Stat.Dex))
                            {
                                ++from.RawStr;
                                --from.RawDex;
                            }

                            //Lower Int
                            else if (CanLower(from, Stat.Int))
                            {
                                ++from.RawStr;
                                --from.RawInt;
                            }
                        }
                    }

                    break;
                }

                case Stat.Dex:
                {
                    //Want Dex To Go Up and Can Go Up
                    if (from.DexLock == StatLockType.Up && from.RawDex < 100)
                    {
                        //Under Stat Cap
                        if (from.RawStatTotal < from.StatCap)
                        {
                            ++from.RawDex;
                        }

                        //At Stat Cap
                        else
                        {
                            //Lower Str
                            if (CanLower(from, Stat.Str))
                            {
                                ++from.RawDex;
                                --from.RawStr;
                            }

                            //Lower Int
                            else if (CanLower(from, Stat.Int))
                            {
                                ++from.RawDex;
                                --from.RawInt;
                            }
                        }
                    }

                    break;
                }

                case Stat.Int:
                {
                    //Want Int To Go Up and Can Go Up
                    if (from.IntLock == StatLockType.Up && from.RawInt < 100)
                    {
                        //Under Stat Cap
                        if (from.RawStatTotal < from.StatCap)
                        {
                            ++from.RawInt;
                        }

                        //At Stat Cap
                        else
                        {
                            //Lower Str
                            if (CanLower(from, Stat.Str))
                            {
                                ++from.RawInt;
                                --from.RawStr;
                            }

                            //Lower Dex
                            else if (CanLower(from, Stat.Dex))
                            {
                                ++from.RawInt;
                                --from.RawDex;
                            }
                        }
                    }

                    break;                   
                }
            }
        }

        public static bool CanLower(Mobile from, Stat stat)
        {
            switch (stat)
            {
                case Stat.Str: return (from.StrLock == StatLockType.Down && from.RawStr > 10);
                case Stat.Dex: return (from.DexLock == StatLockType.Down && from.RawDex > 10);
                case Stat.Int: return (from.IntLock == StatLockType.Down && from.RawInt > 10);
            }

            return false;
        }

        public static bool IsCraftingSkill(Skill skill)
        {
            if (skill.SkillName == SkillName.Alchemy || skill.SkillName == SkillName.Blacksmith || skill.SkillName == SkillName.Carpentry ||
                skill.SkillName == SkillName.Fletching || skill.SkillName == SkillName.Inscribe || skill.SkillName == SkillName.Poisoning ||
                skill.SkillName == SkillName.Tinkering || skill.SkillName == SkillName.Tailoring || skill.SkillName == SkillName.Cooking)
                return true;
            else
                return false;
        }
    }


}