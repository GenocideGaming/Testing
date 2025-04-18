using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.Targeting;

namespace Server.Items
{
    public class ArmamentDyeStrippingTub : Item
    {
        private int mUsesRemaining;

        [CommandProperty(AccessLevel.GameMaster, AccessLevel.GameMaster)]
        public int UsesRemaining { get { return mUsesRemaining; } set { mUsesRemaining = value; } }

        [Constructable]
        public ArmamentDyeStrippingTub()
            : base(0xFAB)
        {
            Weight = 10.0;
            UsesRemaining = 10;
            Hue = 0;
            Name = "armament dye stripping tub";

        }

        public ArmamentDyeStrippingTub(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);

            writer.Write((int)mUsesRemaining);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            mUsesRemaining = reader.ReadInt();
        }

        public override void OnDoubleClick(Mobile from)
        {
           
            if (from.InRange(this.GetWorldLocation(), 1))
            {
                from.SendMessage("Select the armor or weapon to strip the dye from.");
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

       private class InternalTarget : Target
        {
           private ArmamentDyeStrippingTub mTub;

           public InternalTarget(ArmamentDyeStrippingTub tub)
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
                        from.SendMessage("You may not strip dye from armor that is locked down.");
                    else if (toDye.Parent is Mobile)
                        from.SendMessage("You may not strip dye from armor that is being worn.");
                    else
                    {
                        toDye.Hue = 0;

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
                        from.SendMessage("You may not strip dye from weapons that are locked down.");
                    else if (toDye.Parent is Mobile)
                        from.SendMessage("You may not strip dye from weapons that are being worn.");
                    else
                    {
                        toDye.Hue = 0;

                        mTub.UsesRemaining--;
                        if (mTub.UsesRemaining <= 0)
                            mTub.Delete();

                        from.PlaySound(0x23E);
                    }
                }
            }
        }
    }
}
