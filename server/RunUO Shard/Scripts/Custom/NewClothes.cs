using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Items
{
    public class HoodHelm : BaseArmor
    {
        public override int BasePhysicalResistance { get { return 2; } }
        public override int BaseFireResistance { get { return 4; } }
        public override int BaseColdResistance { get { return 3; } }
        public override int BasePoisonResistance { get { return 3; } }
        public override int BaseEnergyResistance { get { return 3; } }

        public override int InitMinHits { get { return 30; } }
        public override int InitMaxHits { get { return 40; } }

        public override int AosStrReq { get { return 20; } }
        public override int OldStrReq { get { return 15; } }

        public override int ArmorBase { get { return 13; } }

        public override ArmorMaterialType MaterialType { get { return ArmorMaterialType.Leather; } }
        public override CraftResource DefaultResource { get { return CraftResource.RegularLeather; } }

        public override ArmorMeditationAllowance DefMedAllowance { get { return ArmorMeditationAllowance.All; } }

        [Constructable]
        public HoodHelm()
            : base(0x3FFC)
        {
            Weight = 2.0;
        }

        public HoodHelm(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (Weight == 1.0)
                Weight = 2.0;
        }
    }

    public class LeatherBelt : BaseWaist
    {
        public override int LabelNumber
        {
            get
            {
                return 1063569;
            }
        }
        [Constructable]
        public LeatherBelt()
            : base(0x3FF9)
        {
        }

        public LeatherBelt(Serial serial)
            : base(serial)
        {
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }

        public override bool Dye(Mobile from, DyeTub sender)
        {
            return false;
        }
    }

    public class LeatherDress : BaseOuterTorso
    {
        [Constructable]
        public LeatherDress()
            : base(0x3FFA)
        {
        }

        public LeatherDress(Serial serial)
            : base(serial)
        {
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
