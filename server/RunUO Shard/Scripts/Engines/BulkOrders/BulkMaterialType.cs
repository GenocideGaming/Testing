using System;
using Server;
using Server.Engines.Craft;
using Server.Items;

namespace Server.Engines.BulkOrders
{
    public enum BulkMaterialType
    {
        None,
        DullCopper,
        ShadowIron,
        Copper,
        Bronze,
        Gold,
        Agapite,
        Verite,
        Valorite,
        Spined,
        Horned,
        Barbed,
        OakWood,
        AshWood,
        YewWood

    }

    public enum BulkGenericType
    {
        Iron,
        Cloth,
        Leather,
        Wood
    }

    public class BGTClassifier
    {
        public static BulkGenericType Classify(BODType deedType, Type itemType)
        {
            if (deedType == BODType.Tailor)
            {
                if (itemType == null || itemType.IsSubclassOf(typeof(BaseArmor)) || itemType.IsSubclassOf(typeof(BaseShoes)))
                    return BulkGenericType.Leather;

                return BulkGenericType.Cloth;
            }
            else if (deedType == BODType.Carpentry)
            {
                return BulkGenericType.Wood;
            }
            return BulkGenericType.Iron;
        }
    }
}
