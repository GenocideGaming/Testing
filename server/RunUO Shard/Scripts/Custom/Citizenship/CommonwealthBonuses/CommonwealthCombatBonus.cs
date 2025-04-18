using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Scripts.Custom.Citizenship.CommonwealthBonuses
{
    public class CommonwealthCombatBonus : CommonwealthBonus
    {

        private CombatBonusTypes mCombatType;
        private float mCombatBonus;
        private bool mActive;

        public CombatBonusTypes CombatType { get { return mCombatType; } set { mCombatType = value; } }
        public float CombatBonus { get { return mCombatBonus; } set { mCombatBonus = value; } }
        public bool Active { get { return mActive; } set { mActive = value; } }

        public CommonwealthCombatBonus(CombatBonusTypes combatType, float combatBonus, int pointCost, string dictKey, string displayName)
            : base(BonusTypes.Combat, pointCost, dictKey, displayName)
        {
            mCombatType = combatType;
            mCombatBonus = combatBonus;
            mActive = true;
        }

        public CommonwealthCombatBonus(GenericReader reader) : base(reader)
        {
            int version = reader.ReadInt();
            mCombatType = (CombatBonusTypes)reader.ReadInt();
            mCombatBonus = reader.ReadFloat();
            mActive = reader.ReadBool();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); //version
            writer.Write((int)mCombatType);
            writer.Write((float)mCombatBonus);
            writer.Write((bool)mActive);
        }
        public override string ToString()
        {
            return base.ToString() + " CombaType " + mCombatType.ToString();
        }
        public override CommonwealthBonus GetShallowCopy()
        {
            return (CommonwealthCombatBonus) this.MemberwiseClone();
        }
    }

    public enum CombatBonusTypes
    {
        MagicResist,
        MagicDamage,
        CombatDamage //weapon damage
    }

    public class CommonwealthCombatBonuses
    {
        public Dictionary<string, CommonwealthCombatBonus> Bonuses;

        public CommonwealthCombatBonuses()
        {
            Bonuses = new Dictionary<string, CommonwealthCombatBonus>();
            Bonuses.Add("MagicResist", new CommonwealthCombatBonus(CombatBonusTypes.MagicResist, FeatureList.Citizenship.MagicDamageReductionBonus, 2, "MagicResist", "Magic Resist Bonus"));
            Bonuses.Add("MagicDamage", new CommonwealthCombatBonus(CombatBonusTypes.MagicDamage, FeatureList.Citizenship.MagicDamageBonus, 2, "MagicDamage", "Magic Damage Bonus"));
            Bonuses.Add("CombatDamage", new CommonwealthCombatBonus(CombatBonusTypes.CombatDamage, FeatureList.Citizenship.WeaponDamageBonus, 2, "CombatDamage", "Combat Damage Bonus"));
        }
    }
}
