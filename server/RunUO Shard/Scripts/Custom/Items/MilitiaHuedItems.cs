using System;
using System.Collections.Generic;
using Server;
using Server.Multis;
using Server.Targeting;
using Server.ContextMenus;
using Server.Gumps;

namespace Server.Items
{
    public static class MilitiaDyeHue
    {
        public static int Forsaken { get { return 2149; } }
        public static int Paladin { get { return 2141; } }
        public static int Urukhai { get { return 1507; } } //2137
    }

    public static class MilitiaVeteranClothHue
    {
        public static int Forsaken { get { return 2150; } }
        public static int Paladin { get { return 2142; } }
        public static int Urukhai { get { return 1507; } } //2138
    }

    public class ForsakenDyeTub : DyeTub
    {
        [Constructable]
        public ForsakenDyeTub()
        {
            Name = "Forsaken dye tub";
            Hue = DyedHue = MilitiaDyeHue.Forsaken;
            Redyable = false;

        }

        public ForsakenDyeTub(Serial serial)
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
    }

    public class PaladinOrderDyeTub: DyeTub
    {
        [Constructable]
        public PaladinOrderDyeTub()
        {
            Name = "Paladin Order dye tub";
            Hue = DyedHue = MilitiaDyeHue.Paladin;
            Redyable = false;
        }

        public PaladinOrderDyeTub(Serial serial) : base(serial) { }

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

    public class UrukhaiDyeTub : DyeTub
    {
        [Constructable]
        public UrukhaiDyeTub()
        {
            Name = "Uruk-hai dye tub";
            Hue = DyedHue = MilitiaDyeHue.Urukhai;
            Redyable = false;
        }

        public UrukhaiDyeTub(Serial serial) : base(serial) { }

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
    [FlipableAttribute(0x1765, 0x1767)]
    public class MilitiaCloth : Item
    {
        public override double DefaultWeight
        {
            get
            {
                return 1.0;
            }
        }

        public override int Hue
        {
            get
            {
                return base.Hue;
            }
            set
            {
            }
        }

        public MilitiaCloth(int hue) : this(hue, 1)
        {
            
        }

        public MilitiaCloth(int hue, int amount)
            : base(0x1767)
        {
            Name = "Militia cloth";
            base.Hue = hue;
            Stackable = true;
            Amount = amount;
        }

        public MilitiaCloth(Serial serial) : base(serial) { }

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


    public class UrukhaiCloth : MilitiaCloth
    {

        [Constructable]
        public UrukhaiCloth()
            : base(MilitiaVeteranClothHue.Urukhai)
        {

        }

        [Constructable]
        public UrukhaiCloth(int amount)
            : base(MilitiaVeteranClothHue.Urukhai, amount)
        {
            Name = "Uruk-hai cloth";
        }

        public UrukhaiCloth(Serial serial) : base(serial) { }

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

    public class PaladinOrderCloth : MilitiaCloth
    {

        [Constructable]
        public PaladinOrderCloth()
            : base(MilitiaVeteranClothHue.Paladin)
        {

        }

        [Constructable]
        public PaladinOrderCloth(int amount)
            : base(MilitiaVeteranClothHue.Paladin, amount)
        {
            Name = "Paladin Order cloth";
        }

        public PaladinOrderCloth(Serial serial) : base(serial) { }

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

    public class ForsakenCloth : MilitiaCloth
    {

        [Constructable]
        public ForsakenCloth()
            : base(MilitiaVeteranClothHue.Forsaken)
        {

        }

        [Constructable]
        public ForsakenCloth(int amount)
            : base(MilitiaVeteranClothHue.Forsaken, amount)
        {
            Name = "Forsaken cloth";
        }

        public ForsakenCloth(Serial serial) : base(serial) { }

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