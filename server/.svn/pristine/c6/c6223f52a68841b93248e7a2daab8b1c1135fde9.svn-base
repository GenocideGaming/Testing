using System;
using Server.Items;
using Server.Network;
using Server.Targeting;
using Server.Engines.Craft;
using Server.Mobiles;
using Server.Scripts;

namespace Server.Items
{
    public abstract class BaseOre : Item, ICommodity
    {
        private CraftResource m_Resource;

        [CommandProperty(AccessLevel.GameMaster)]
        public CraftResource Resource
        {
            get { return m_Resource; }
            set { m_Resource = value; InvalidateProperties(); }
        }

        int ICommodity.DescriptionNumber { get { return LabelNumber; } }
        bool ICommodity.IsDeedable { get { return true; } }

        public abstract BaseIngot GetIngot();

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version

            writer.Write((int)m_Resource);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 1:
                    {
                        m_Resource = (CraftResource)reader.ReadInt();
                        break;
                    }
                case 0:
                    {
                        OreInfo info;

                        switch (reader.ReadInt())
                        {
                            case 0: info = OreInfo.Iron; break;
                            case 1: info = OreInfo.DullCopper; break;
                            case 2: info = OreInfo.ShadowIron; break;
                            case 3: info = OreInfo.Copper; break;
                            case 4: info = OreInfo.Bronze; break;
                            case 5: info = OreInfo.Gold; break;
                            case 6: info = OreInfo.Agapite; break;
                            case 7: info = OreInfo.Verite; break;
                            case 8: info = OreInfo.Valorite; break;
                            default: info = null; break;
                        }

                        m_Resource = CraftResources.GetFromOreInfo(info);
                        break;
                    }
            }
        }

        public BaseOre(CraftResource resource)
            : this(resource, 1)
        {
        }

        public BaseOre(CraftResource resource, int amount)
            : base(Utility.Random(4))
        {
            {
                double random = Utility.RandomDouble();
                if (0.12 >= random)
                    ItemID = 0x19B7;
                else if (0.18 >= random)
                    ItemID = 0x19B8;
                else if (0.25 >= random)
                    ItemID = 0x19BA;
                else
                    ItemID = 0x19B9;
            }

            Stackable = true;
            Amount = amount;
            Weight = FeatureList.Harvesting.OreWeight;
            Hue = CraftResources.GetHue(resource);

            m_Resource = resource;
        }

        public BaseOre(Serial serial)
            : base(serial)
        {
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            if (Amount > 1)
                list.Add(1050039, "{0}\t#{1}", Amount, 1026583); // ~1_NUMBER~ ~2_ITEMNAME~
            else
                list.Add(1026583); // ore
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            if (!CraftResources.IsStandard(m_Resource))
            {
                int num = CraftResources.GetLocalizationNumber(m_Resource);

                if (num > 0)
                    list.Add(num);
                else
                    list.Add(CraftResources.GetName(m_Resource));
            }
        }

        public override int LabelNumber
        {
            get
            {
                if (m_Resource >= CraftResource.DullCopper && m_Resource <= CraftResource.Valorite)
                    return 1042845 + (int)(m_Resource - CraftResource.DullCopper);

                return 1042853; // iron ore;
            }
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!Movable)
                return;

            /*
            if (RootParent is BaseCreature)
            {
                from.SendLocalizedMessage(500447); // That is not accessible
                return;
            }
            */
            else if (from.InRange(this.GetWorldLocation(), 2))
            {
                from.SendLocalizedMessage(501971); // Select the forge on which to smelt the ore, or another pile of ore with which to combine it.
                from.Target = new InternalTarget(this);
            }
            else
            {
                from.SendLocalizedMessage(501976); // The ore is too far away.
            }
        }

        private class InternalTarget : Target
        {
            private BaseOre m_Ore;

            public InternalTarget(BaseOre ore)
                : base(2, false, TargetFlags.None)
            {
                m_Ore = ore;
            }

            private bool IsForge(object obj)
            {
                if (Core.ML && obj is Mobile && ((Mobile)obj).IsDeadBondedPet)
                    return false;

                if (obj.GetType().IsDefined(typeof(ForgeAttribute), false))
                    return true;

                int itemID = 0;

                if (obj is Item)
                    itemID = ((Item)obj).ItemID;
                else if (obj is StaticTarget)
                    itemID = ((StaticTarget)obj).ItemID;

                return (itemID == 4017 || (itemID >= 6522 && itemID <= 6569));
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (m_Ore.Deleted)
                    return;

                if (!from.InRange(m_Ore.GetWorldLocation(), 2))
                {
                    from.SendLocalizedMessage(501976); // The ore is too far away.
                    return;
                }

                #region Combine Ore
                if (targeted is BaseOre)
                {
                    BaseOre ore = (BaseOre)targeted;
                    if (!ore.Movable)
                        return;
                    else if (m_Ore == ore)
                    {
                        from.SendLocalizedMessage(501972); // Select another pile or ore with which to combine this.
                        from.Target = new InternalTarget(ore);
                        return;
                    }
                    else if (ore.Resource != m_Ore.Resource)
                    {
                        from.SendLocalizedMessage(501979); // You cannot combine ores of different metals.
                        return;
                    }

                    int worth = ore.Amount;
                    if (ore.ItemID == 0x19B9)
                        worth *= 8;
                    else if (ore.ItemID == 0x19B7)
                        worth *= 2;
                    else
                        worth *= 4;
                    int sourceWorth = m_Ore.Amount;
                    if (m_Ore.ItemID == 0x19B9)
                        sourceWorth *= 8;
                    else if (m_Ore.ItemID == 0x19B7)
                        sourceWorth *= 2;
                    else
                        sourceWorth *= 4;
                    worth += sourceWorth;

                    int plusWeight = 0;
                    int newID = ore.ItemID;
                    if (ore.DefaultWeight != m_Ore.DefaultWeight)
                    {
                        if (ore.ItemID == 0x19B7 || m_Ore.ItemID == 0x19B7)
                        {
                            newID = 0x19B7;
                        }
                        else if (ore.ItemID == 0x19B9)
                        {
                            newID = m_Ore.ItemID;
                            plusWeight = ore.Amount * 2;
                        }
                        else
                        {
                            plusWeight = m_Ore.Amount * 2;
                        }
                    }

                    if ((ore.ItemID == 0x19B9 && worth > 120000) || ((ore.ItemID == 0x19B8 || ore.ItemID == 0x19BA) && worth > 60000) || (ore.ItemID == 0x19B7 && worth > 30000))
                    {
                        from.SendLocalizedMessage(1062844); // There is too much ore to combine.
                        return;
                    }
                    else if (ore.RootParent is Mobile && (plusWeight + ((Mobile)ore.RootParent).Backpack.TotalWeight) > ((Mobile)ore.RootParent).Backpack.MaxWeight)
                    {
                        from.SendLocalizedMessage(501978); // The weight is too great to combine in a container.
                        return;
                    }

                    ore.ItemID = newID;
                    if (ore.ItemID == 0x19B9)
                    {
                        ore.Amount = worth / 8;
                        m_Ore.Delete();
                    }
                    else if (ore.ItemID == 0x19B7)
                    {
                        ore.Amount = worth / 2;
                        m_Ore.Delete();
                    }
                    else
                    {
                        ore.Amount = worth / 4;
                        m_Ore.Delete();
                    }
                    return;
                }
                #endregion

                if (IsForge(targeted))
                {
                    double difficulty = 25; //Default
                    double miningSkill = from.Skills[SkillName.Mining].Value;

                    switch (m_Ore.Resource)
                    {
                        case CraftResource.Iron: difficulty = 25.0; break; //100% at 50
                        case CraftResource.DullCopper: difficulty = 50.0; break; //100% at 75
                        case CraftResource.ShadowIron: difficulty = 55.0; break; //100% at 80
                        case CraftResource.Copper: difficulty = 60.0; break; //100% at 85
                        case CraftResource.Bronze: difficulty = 65.0; break; //100% at 90
                        case CraftResource.Gold: difficulty = 70.0; break; //100% at 95
                        case CraftResource.Agapite: difficulty = 75.0; break; //100% at 100
                        case CraftResource.Verite: difficulty = 80.0; break; //80% at 100, 100% at 105
                        case CraftResource.Valorite: difficulty = 85.0; break; //60% at 100, 100% at 110
                    }

                    double chance = ((miningSkill - difficulty) * (100 / FeatureList.Harvesting.MiningSmeltingDifficultyDivider)) / 100;

                    //Check If Possible
                    if (difficulty > miningSkill)
                    {
                        from.SendLocalizedMessage(501986); // You have no idea how to smelt this strange ore!
                        return;
                    }

                    //Not Enough Ore
                    if (m_Ore.Amount <= 1 && m_Ore.ItemID == 0x19B7)
                    {
                        from.SendLocalizedMessage(501987); // There is not enough metal-bearing ore in this pile to make an ingot.
                        return;
                    }

                    //Successful Smelting Attempt
                    if (chance >= Utility.RandomDouble())
                    {
                        if (m_Ore.Amount <= 0)
                        {
                            from.SendLocalizedMessage(501987); // There is not enough metal-bearing ore in this pile to make an ingot.
                        }

                        else
                        {
                            int amount = m_Ore.Amount;
                            if (m_Ore.Amount > 30000)
                                amount = 30000;

                            BaseIngot ingot = m_Ore.GetIngot();

                            if (m_Ore.ItemID == 0x19B7)
                            {
                                if (m_Ore.Amount % 2 == 0)
                                {
                                    amount /= 2;
                                    m_Ore.Delete();
                                }
                                else
                                {
                                    amount /= 2;
                                    m_Ore.Amount = 1;
                                }
                            }

                            else if (m_Ore.ItemID == 0x19B9)
                            {
                                amount *= 2;
                                m_Ore.Delete();
                            }

                            else
                            {
                                amount /= 1;
                                m_Ore.Delete();
                            }

                            ingot.Amount = amount;
                            from.AddToBackpack(ingot);
                            //from.PlaySound( 0x57 );


                            from.SendLocalizedMessage(501988); // You smelt the ore removing the impurities and put the metal in your backpack.
                        }
                    }

                    //Not Enough Ore for Success
                    else if (m_Ore.Amount < 2 && m_Ore.ItemID == 0x19B9)
                    {
                        from.SendLocalizedMessage(501990); // You burn away the impurities but are left with less useable metal.
                        m_Ore.ItemID = 0x19B8;
                    }

                    //Not Enough Ore for Success
                    else if (m_Ore.Amount < 2 && m_Ore.ItemID == 0x19B8 || m_Ore.ItemID == 0x19BA)
                    {
                        from.SendLocalizedMessage(501990); // You burn away the impurities but are left with less useable metal.
                        m_Ore.ItemID = 0x19B7;
                    }

                    //Fail
                    else
                    {
                        from.SendLocalizedMessage(501990); // You burn away the impurities but are left with less useable metal.
                        m_Ore.Amount /= 2;
                    }

                    //SkillCheck For Smelting
                    from.CheckTargetSkill(SkillName.Mining, targeted, 0, 100);
                }
            }
        }
    }

    public class IronOre : BaseOre
    {
        [Constructable]
        public IronOre()
            : this(1)
        {
        }

        [Constructable]
        public IronOre(int amount)
            : base(CraftResource.Iron, amount)
        {
        }

        public IronOre(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }



        public override BaseIngot GetIngot()
        {
            return new IronIngot();
        }
    }

    public class DullCopperOre : BaseOre
    {
        [Constructable]
        public DullCopperOre()
            : this(1)
        {
        }

        [Constructable]
        public DullCopperOre(int amount)
            : base(CraftResource.DullCopper, amount)
        {
        }

        public DullCopperOre(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }



        public override BaseIngot GetIngot()
        {
            return new DullCopperIngot();
        }
    }

    public class ShadowIronOre : BaseOre
    {
        [Constructable]
        public ShadowIronOre()
            : this(1)
        {
        }

        [Constructable]
        public ShadowIronOre(int amount)
            : base(CraftResource.ShadowIron, amount)
        {
        }

        public ShadowIronOre(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }



        public override BaseIngot GetIngot()
        {
            return new ShadowIronIngot();
        }
    }

    public class CopperOre : BaseOre
    {
        [Constructable]
        public CopperOre()
            : this(1)
        {
        }

        [Constructable]
        public CopperOre(int amount)
            : base(CraftResource.Copper, amount)
        {
        }

        public CopperOre(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }



        public override BaseIngot GetIngot()
        {
            return new CopperIngot();
        }
    }

    public class BronzeOre : BaseOre
    {
        [Constructable]
        public BronzeOre()
            : this(1)
        {
        }

        [Constructable]
        public BronzeOre(int amount)
            : base(CraftResource.Bronze, amount)
        {
        }

        public BronzeOre(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }



        public override BaseIngot GetIngot()
        {
            return new BronzeIngot();
        }
    }

    public class GoldOre : BaseOre
    {
        [Constructable]
        public GoldOre()
            : this(1)
        {
        }

        [Constructable]
        public GoldOre(int amount)
            : base(CraftResource.Gold, amount)
        {
        }

        public GoldOre(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }



        public override BaseIngot GetIngot()
        {
            return new GoldIngot();
        }
    }

    public class AgapiteOre : BaseOre
    {
        [Constructable]
        public AgapiteOre()
            : this(1)
        {
        }

        [Constructable]
        public AgapiteOre(int amount)
            : base(CraftResource.Agapite, amount)
        {
        }

        public AgapiteOre(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }



        public override BaseIngot GetIngot()
        {
            return new AgapiteIngot();
        }
    }

    public class VeriteOre : BaseOre
    {
        [Constructable]
        public VeriteOre()
            : this(1)
        {
        }

        [Constructable]
        public VeriteOre(int amount)
            : base(CraftResource.Verite, amount)
        {
        }

        public VeriteOre(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }



        public override BaseIngot GetIngot()
        {
            return new VeriteIngot();
        }
    }

    public class ValoriteOre : BaseOre
    {
        [Constructable]
        public ValoriteOre()
            : this(1)
        {
        }

        [Constructable]
        public ValoriteOre(int amount)
            : base(CraftResource.Valorite, amount)
        {
        }

        public ValoriteOre(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }



        public override BaseIngot GetIngot()
        {
            return new ValoriteIngot();
        }
    }
}