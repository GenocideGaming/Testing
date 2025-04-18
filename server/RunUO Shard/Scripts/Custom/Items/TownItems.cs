using System;
using System.Collections.Generic;
using System.Text;
using Server.Factions;
using Server.Items;

namespace Server.Scripts.Custom.Citizenship
{
    public class TownBanner : Item
    {
        public TownBanner(int itemID) : base(itemID)
        {
        }

        public TownBanner(Serial serial) : base(serial)
        {

        }
        public override void  Serialize(GenericWriter writer)
        {
 	        base.Serialize(writer);
             writer.Write((int)0);
        }
        public override void  Deserialize(GenericReader reader)
        {
 	        base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class MilitiaRestrictedHat : BaseHat 
    {
        private Faction mMilitia;
        
        public MilitiaRestrictedHat(int itemID, Faction militia)
            : base(itemID)
        {
            mMilitia = militia;
        }

        public MilitiaRestrictedHat(Serial serial)
            : base(serial)
        {

        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);

            Faction.WriteReference(writer, mMilitia);
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            mMilitia = Faction.ReadReference(reader);
        }

        public override bool CanEquip(Mobile from)
        {
            Faction playerMilitia = Faction.Find(from);
            if (playerMilitia != mMilitia)
            {
                from.SendMessage("You must be a member of the " + mMilitia.Definition.TownshipName + " faction to wear this item.");
                return false;
            }
            return base.CanEquip(from);
        }
    }

    [Flipable(0x42CD, 0x42CE)] 
    public class PaladinBanner : TownBanner 
    {
        [Constructable]
        public PaladinBanner() : base(0x42CD)
        {
            //Hue = 0x4DF;
            Name = "paladin banner";
        }
        public PaladinBanner(Serial serial) : base(serial)
        {

        }
        public override void  Serialize(GenericWriter writer)
        {
 	        base.Serialize(writer);
             writer.Write((int)0);
        }
        public override void  Deserialize(GenericReader reader)
        {
 	        base.Deserialize(reader);
            int version = reader.ReadInt();
        }

    }

    [Flipable(0x42CB, 0x42CC)]
    public class ForsakenBanner : TownBanner
    {
        [Constructable]
        public ForsakenBanner() : base(0x42CB)  //0x15BC
        {
            //Hue = 0x4B9;
            Name = "forsaken banner";
        }
        public ForsakenBanner(Serial serial)
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
    [Flipable(0x0455, 0x03E9)]
    public class UrukhaiBanner : TownBanner
    {
        [Constructable]
        public UrukhaiBanner() : base(0x0455)  //0x15CC
        {
            //Hue = 0x58A;
            Name = "uruk-hi banner";
        }
        public UrukhaiBanner(Serial serial)
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

    public class PaladinHood1 : MilitiaRestrictedHat
    {
        public override int LabelNumber
        {
            get
            {
                return 1063522;
            }
        }
        [Constructable]
        public PaladinHood1()
            : base(0x3FFB, Faction.Find("Paladin"))
        {
            Hue = 0x4E6;
        }
        public PaladinHood1(Serial serial)
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

    public class PaladinHood2 : MilitiaRestrictedHat
    {
        public override int LabelNumber
        {
            get
            {
                return 1063522;
            }
        }
        [Constructable]
        public PaladinHood2()
            : base(0x3FFC, Faction.Find("Paladin"))
        {
            Hue = MilitiaDyeHue.Paladin;
            Name = "Paladin Order hood";
        }
        public PaladinHood2(Serial serial)
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

    public class ForsakenHood1 : MilitiaRestrictedHat
    {
        public override int LabelNumber
        {
            get
            {
                return 1063522;
            }
        }
        [Constructable]
        public ForsakenHood1()
            : base(0x3FFB, Faction.Find("Forsaken"))
        {
            Hue = 0x4B9;
        }
        public ForsakenHood1(Serial serial)
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

    public class ForsakenHood2 : MilitiaRestrictedHat
    {
        public override int LabelNumber
        {
            get
            {
                return 1063522;
            }
        }
        [Constructable]
        public ForsakenHood2()
            : base(0x3FFC, Faction.Find("Forsaken"))
        {
            Hue = MilitiaDyeHue.Forsaken;
            Name = "Forsaken Faction hood";
        }
        public ForsakenHood2(Serial serial)
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

    public class UrukhaiHood1 : MilitiaRestrictedHat
    {
        public override int LabelNumber
        {
            get
            {
                return 1063522;
            }
        }
        [Constructable]
        public UrukhaiHood1()
            : base(0x3FFB, Faction.Find("Uruk-hai"))
        {
            Hue = 0x5E3;
        }
        public UrukhaiHood1(Serial serial)
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

    public class UrukhaiHood2 : MilitiaRestrictedHat
    {
        public override int LabelNumber
        {
            get
            {
                return 1063522;
            }
        }
        [Constructable]
        public UrukhaiHood2()
            : base(0x3FFC, Faction.Find("Uruk-hai"))
        {
            Hue = MilitiaDyeHue.Urukhai;
            Name = "Uruk-hai hood";
        }
        public UrukhaiHood2(Serial serial)
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
    public class CitizenshipShield : BaseShield
    {

        public override int InitMinHits { get { return 45; } }
        public override int InitMaxHits { get { return 60; } }

        public override int OldDexBonus { get { return -5; } }
        public override int ArmorBase { get { return 10; } }

        public virtual ICommonwealth Township { get { return null; } }

        [Constructable]
        public CitizenshipShield(int itemID)
            : base(itemID)
        {
            Weight = 7.0;
        }

        public CitizenshipShield(Serial serial)
            : base(serial)
        {
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);//version
        }

        public override bool OnEquip(Mobile from)
        {
            if (Commonwealth.Find(from) != Township)
                return false;

            return base.OnEquip(from);
        }
    }


    public class PaladinShield : CitizenshipShield
    {
        public override int LabelNumber
        {
            get
            {
                return 1063577;
            }
        }
        public override int InitMinHits { get { return 45; } }
        public override int InitMaxHits { get { return 60; } }

        public override int OldDexBonus { get { return -5; } }
        public override int ArmorBase { get { return 10; } }

        public override ICommonwealth Township { get { return Commonwealth.Find("Paladin"); } }

        [Constructable]
        public PaladinShield()
            : base(0x3B52)
        {
            Weight = 7.0;
        }

        public PaladinShield(Serial serial)
            : base(serial)
        {
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);//version
        }

        public override bool OnEquip(Mobile from)
        {
            if (Commonwealth.Find(from) != Township)
                return false;

            return base.OnEquip(from);
        }
    }

    public class ForsakenShield : CitizenshipShield
    {
        public override int LabelNumber
        {
            get
            {
                return 1063579;
            }
        }
        public override int InitMinHits { get { return 45; } }
        public override int InitMaxHits { get { return 60; } }

        public override int OldDexBonus { get { return -5; } }
        public override int ArmorBase { get { return 10; } }

        public override ICommonwealth Township { get { return Commonwealth.Find("Forsaken"); } }

        [Constructable]
        public ForsakenShield()
            : base(0x3B54)
        {
            Weight = 7.0;
        }

        public ForsakenShield(Serial serial)
            : base(serial)
        {
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);//version
        }

        public override bool OnEquip(Mobile from)
        {
            if (Commonwealth.Find(from) != Township)
                return false;

            return base.OnEquip(from);
        }
    }
    public class UrukhaiShield : CitizenshipShield
    {
        public override int LabelNumber
        {
            get
            {
                return 1063577;
            }
        }
        public override int InitMinHits { get { return 45; } }
        public override int InitMaxHits { get { return 60; } }

        public override int OldDexBonus { get { return -5; } }
        public override int ArmorBase { get { return 10; } }

        public override ICommonwealth Township { get { return Commonwealth.Find("Uruk-hai"); } }

        [Constructable]
        public UrukhaiShield()
            : base(0x3B52)
        {
            Weight = 7.0;
        }

        public UrukhaiShield(Serial serial)
            : base(serial)
        {
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);//version
        }

        public override bool OnEquip(Mobile from)
        {
            if (Commonwealth.Find(from) != Township)
                return false;

            return base.OnEquip(from);
        }
    }
    public class PaladinTownshipHood : BaseHat
    {
        public override int LabelNumber
        {
            get
            {
                return 1063522;
            }
        }
        [Constructable]
        public PaladinTownshipHood()
            : base(0x3FFC)
        {
            Hue = CitizenDyeHue.Trinsic;
            Name = "Paladin Order hood";
        }
        public PaladinTownshipHood(Serial serial)
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

    public class ForsakenTownshipHood : BaseHat
    {
        public override int LabelNumber
        {
            get
            {
                return 1063522;
            }
        }
        [Constructable]
        public ForsakenTownshipHood()
            : base(0x3FFC)
        {
            Hue = CitizenDyeHue.Yew;
            Name = "Forsaken township hood";
        }
        public ForsakenTownshipHood(Serial serial)
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
    public class UrukhaiTownshipHood : BaseHat
    {
        public override int LabelNumber
        {
            get
            {
                return 1063522;
            }
        }
        [Constructable]
        public UrukhaiTownshipHood()
            : base(0x3FFC)
        {
            Hue = CitizenDyeHue.Trinsic;
            Name = "Uruk-hai hood";
        }
        public UrukhaiTownshipHood(Serial serial)
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
}

