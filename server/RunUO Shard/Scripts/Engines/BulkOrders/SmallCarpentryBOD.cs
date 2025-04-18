using System;
using System.Collections.Generic;
using Server;
using Server.Engines.Craft;
using Server.Items;

namespace Server.Engines.BulkOrders
{
    [TypeAlias("Scripts.Engines.BulkOrders.SmallCarpentryBOD")]
    public class SmallCarpentryBOD : SmallBOD
    {
        public static double[] m_CarpentryMaterialChances = new double[]
            {
            0.513718750, // None
            0.292968750, // Oak
            0.117187500, // Ash
            0.046875000, // Yew
            0.018750000, // Heartwood
            0.007500000, // Bloodwood
            0.003000000 // Frostwood
			};

        public override int ComputeFame()
        {
            return CarpentryRewardCalculator.Instance.ComputeFame(this);
        }

        public override int ComputeGold()
        {
            return CarpentryRewardCalculator.Instance.ComputeGold(this);
        }

        public override List<Item> ComputeRewards(bool full)
        {
            List<Item> list = new List<Item>();

            RewardGroup rewardGroup = CarpentryRewardCalculator.Instance.LookupRewards(CarpentryRewardCalculator.Instance.ComputePoints(this));

            if (rewardGroup != null)
            {
                if (full)
                {
                    for (int i = 0; i < rewardGroup.Items.Length; ++i)
                    {
                        Item item = rewardGroup.Items[i].Construct();

                        if (item != null)
                            list.Add(item);
                    }
                }
                else
                {
                    RewardItem rewardItem = rewardGroup.AcquireItem();

                    if (rewardItem != null)
                    {
                        Item item = rewardItem.Construct();

                        if (item != null)
                            list.Add(item);
                    }
                }
            }

            return list;
        }

        public static SmallCarpentryBOD CreateRandomFor(Mobile m)
        {
            SmallBulkEntry[] entries;
            bool useMaterials = Utility.RandomBool();

            entries = SmallBulkEntry.CarpentrySmalls;

            if (entries.Length > 0)
            {
                double theirSkill = m.Skills[SkillName.Carpentry].Base;
                int amountMax;

                if (theirSkill >= 70.1)
                    amountMax = Utility.RandomList(10, 15, 20, 20);
                else if (theirSkill >= 50.1)
                    amountMax = Utility.RandomList(10, 15, 15, 20);
                else
                    amountMax = Utility.RandomList(10, 10, 15, 20);

                BulkMaterialType material = BulkMaterialType.None;

                if (useMaterials && theirSkill >= 70.1)
                {
                    for (int i = 0; i < 20; ++i)
                    {
                        BulkMaterialType check = GetRandomMaterial(BulkMaterialType.OakWood, m_CarpentryMaterialChances);
                        double skillReq = 0.0;

                        switch (check)
                        {
                            case BulkMaterialType.OakWood: skillReq = 80.0; break;   //65
                            case BulkMaterialType.AshWood: skillReq = 85.0; break;   //70
                            case BulkMaterialType.YewWood: skillReq = 90.0; break;   //75
                            //case BulkMaterialType.Heartwood: skillReq = 90.0; break; //80
                            //case BulkMaterialType.Bloodwood: skillReq = 95.0; break; //85
                            //case BulkMaterialType.Frostwood: skillReq = 100.0; break; //90
                        }

                        if (theirSkill >= skillReq)
                        {
                            material = check;
                            break;
                        }
                    }
                }

                double excChance = 0.0;

                if (theirSkill >= 70.1)
                    excChance = (theirSkill + 80.0) / 200.0;

                bool reqExceptional = (excChance > Utility.RandomDouble());

                CraftSystem system = DefCarpentry.CraftSystem;

                List<SmallBulkEntry> validEntries = new List<SmallBulkEntry>();

                for (int i = 0; i < entries.Length; ++i)
                {
                    CraftItem item = system.CraftItems.SearchFor(entries[i].Type);

                    if (item != null)
                    {
                        bool allRequiredSkills = true;
                        double chance = item.GetSuccessChance(m, null, system, false, ref allRequiredSkills);

                        if (allRequiredSkills && chance >= 0.0)
                        {
                            if (reqExceptional)
                                chance = item.GetExceptionalChance(system, chance, m);

                            if (chance > 0.0)
                                validEntries.Add(entries[i]);
                        }
                    }
                }

                if (validEntries.Count > 0)
                {
                    SmallBulkEntry entry = validEntries[Utility.Random(validEntries.Count)];
                    return new SmallCarpentryBOD(entry, material, amountMax, reqExceptional);
                }
            }

            return null;
        }

        private SmallCarpentryBOD(SmallBulkEntry entry, BulkMaterialType material, int amountMax, bool reqExceptional)
        {
            this.Hue = 1512;
            this.AmountMax = amountMax;
            this.Type = entry.Type;
            this.Number = entry.Number;
            this.Graphic = entry.Graphic;
            this.RequireExceptional = reqExceptional;
            this.Material = material;
        }

        [Constructable]
        public SmallCarpentryBOD()
        {
            SmallBulkEntry[] entries;
            bool useMaterials = Utility.RandomBool();

            entries = SmallBulkEntry.CarpentrySmalls;

            if (entries.Length > 0)
            {
                int hue = 1512;
                int amountMax = Utility.RandomList(10, 15, 20);

                BulkMaterialType material = BulkMaterialType.None;

                if (useMaterials)
                    material = GetRandomMaterial(BulkMaterialType.OakWood, m_CarpentryMaterialChances);

                bool reqExceptional = Utility.RandomBool() || (material == BulkMaterialType.None);

                SmallBulkEntry entry = entries[Utility.Random(entries.Length)];

                this.Hue = hue;
                this.AmountMax = amountMax;
                this.Type = entry.Type;
                this.Number = entry.Number;
                this.Graphic = entry.Graphic;
                this.RequireExceptional = reqExceptional;
                this.Material = material;
            }
        }

        public SmallCarpentryBOD(int amountCur, int amountMax, Type type, int number, int graphic, bool reqExceptional, BulkMaterialType mat)
        {
            this.Hue = 1512;
            this.AmountMax = amountMax;
            this.AmountCur = amountCur;
            this.Type = type;
            this.Number = number;
            this.Graphic = graphic;
            this.RequireExceptional = reqExceptional;
            this.Material = mat;
        }

        public SmallCarpentryBOD(Serial serial) : base(serial)
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
    }
}