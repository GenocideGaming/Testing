using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.Targeting;
using Server.Items;
using Server.Factions;
using Server.Network;
using Server.Scripts.Custom.Citizenship;

namespace Server.Items
{
    public static class ArmamentDyeHues
    {
        public const int Urukhai = 2139;
        public const int Paladin = 2143;
        public const int Forsaken = 2151;
    }
    public class ArmamentDyeTub : Item
    {
        private int mUsesRemaining;
        private int mDyeHue;

        [CommandProperty(AccessLevel.GameMaster,AccessLevel.GameMaster)]
        public int UsesRemaining { get { return mUsesRemaining; } set { mUsesRemaining = value; } }

        public int DyeHue { get { return mDyeHue; } set { mDyeHue = value; } }

        public ArmamentDyeTub(int hue)
            : base(0xFAB)
        {
            Weight = 10.0;
            UsesRemaining = 10;
            Hue = DyeHue = hue;

        }

        public ArmamentDyeTub(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);

            writer.Write((int)mUsesRemaining);
            writer.Write((int)mDyeHue);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            mUsesRemaining = reader.ReadInt();
            mDyeHue = reader.ReadInt();
        }

        public override void OnDoubleClick(Mobile from)
        {
            Faction playerMilitia = Faction.Find(from);
            Faction militia = GetMilitiaFromHue();

            if (playerMilitia != militia && from.AccessLevel == AccessLevel.Player)
            {
                from.SendMessage("You must be a member of {0} to use that dye tub.", militia.Definition.TownshipName);
            }

            if (from.InRange(this.GetWorldLocation(), 1))
            {
                from.SendMessage("Select the armor or weapon to dye.");
                from.Target = new InternalTarget(this);
            }
            else
            {
                from.SendLocalizedMessage(500446); //That is too far away
            }
        }
        public override void OnSingleClick(Mobile from)
        {
            DisplayDurabilityTo(from);
            base.OnSingleClick(from);
        }
        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            list.Add(1060584, mUsesRemaining.ToString());
        }

        public virtual void DisplayDurabilityTo(Mobile m)
        {
            LabelTo(m, "Charges : {0}", mUsesRemaining);
        }

        private Faction GetMilitiaFromHue()
        {
            switch (DyeHue)
            {
                case ArmamentDyeHues.Urukhai:
                    return Faction.Find("Uruk-hai");
                case ArmamentDyeHues.Paladin:
                    return Faction.Find("Paladin");
                case ArmamentDyeHues.Forsaken:
                    return Faction.Find("Forsaken");
            }

            return null;
        }
        private class InternalTarget : Target
        {
            private ArmamentDyeTub mTub;

            public InternalTarget(ArmamentDyeTub tub)
                : base(1, false, TargetFlags.None)
            {
                mTub = tub;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is BaseArmor)
                {
                    BaseArmor toDye = targeted as BaseArmor;
                    if (!from.InRange(mTub.GetWorldLocation(), 1) || !from.InRange(toDye.GetWorldLocation(), 1))
                        from.SendLocalizedMessage(500446); //That is too far away
                    else if (!toDye.Movable)
                        from.SendMessage("You may not dye armor that is locked down.");
                    else if (toDye.Parent is Mobile)
                        from.SendMessage("You may not dye armor that is being worn.");
                    else if (toDye is CitizenshipShield)
                        from.SendMessage("You may not dye citizenship shields.");
                    else
                    {
                        toDye.Hue = mTub.DyeHue;

                        mTub.UsesRemaining--;
                        if (mTub.UsesRemaining <= 0)
                            mTub.Delete();

                        from.PlaySound(0x23E);
                    }
                    
                }
                else if (targeted is BaseWeapon)
                {
                    BaseWeapon toDye = targeted as BaseWeapon;
                    if (!from.InRange(mTub.GetWorldLocation(), 1) || !from.InRange(toDye.GetWorldLocation(), 1))
                        from.SendLocalizedMessage(500446); //That is too far away
                    else if (!toDye.Movable)
                        from.SendMessage("You may not dye weapons that are locked down.");
                    else if (toDye.Parent is Mobile)
                        from.SendMessage("You may not dye weapons that are being worn.");
                    else
                    {
                        toDye.Hue = mTub.DyeHue;

                        mTub.UsesRemaining--;
                        if (mTub.UsesRemaining <= 0)
                            mTub.Delete();

                        from.PlaySound(0x23E);
                    }
                }
            }
        }
    }

    public class UrukhaiArmamentDyeTub : ArmamentDyeTub
    {
        [Constructable]
        public UrukhaiArmamentDyeTub()
            : base(ArmamentDyeHues.Urukhai)
        {
            Name = "Urukhai armament dye tub";
        }

        public UrukhaiArmamentDyeTub(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); //version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class ForsakenArmamentDyeTub : ArmamentDyeTub
    {
        [Constructable]
        public ForsakenArmamentDyeTub()
            : base(ArmamentDyeHues.Forsaken)
        {
            Name = "Forsaken armament dye tub";
        }

        public ForsakenArmamentDyeTub(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); //version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class PaladinOrderArmamentDyeTub : ArmamentDyeTub
    {
        [Constructable]
        public PaladinOrderArmamentDyeTub()
            : base(ArmamentDyeHues.Paladin)
        {
            Name = "Paladin Order armament dye tub";
        }

        public PaladinOrderArmamentDyeTub(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); //version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}
