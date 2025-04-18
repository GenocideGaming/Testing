using System;
using Server.Targeting;

namespace Server.Items
{
    public class RunebookBlessTarget : Target // Create our targeting class (which we derive from the base target class)
    {
        private readonly RunebookBlessDeed m_Deed;
        public RunebookBlessTarget(RunebookBlessDeed deed)
            : base(1, false, TargetFlags.None)
        {
            this.m_Deed = deed;
        }

        protected override void OnTarget(Mobile from, object target) // Override the protected OnTarget() for our feature
        {
            if (this.m_Deed.Deleted || this.m_Deed.RootParent != from)
                return;

            if (target is Runebook)
            {
                Runebook item = (Runebook)target;

                if (item is IArcaneEquip)
                {
                    IArcaneEquip eq = (IArcaneEquip)item;
                    if (eq.IsArcane)
                    {
                        from.SendLocalizedMessage(1005010); // This bless deed is for Clothes only.
                        return;
                    }
                }

                if (item.LootType == LootType.Blessed || item.BlessedFor == from || (Mobile.InsuranceEnabled && item.Insured)) // Check if its already newbied (blessed)
                {
                    from.SendLocalizedMessage(1045113); // That item is already blessed
                }
                else
                {
                    item.LootType = LootType.Blessed;
                    from.SendLocalizedMessage(1010026); // You bless the item....

                    this.m_Deed.Delete(); // Delete the bless deed
                }
            }
            else
            {
                from.SendLocalizedMessage(500509); // You cannot bless that object
            }
        }
    }

    public class RunebookBlessDeed : Item // Create the item class which is derived from the base item class
    {
		public override int LabelNumber { get { return 1041534; } } // A Runebook bless deed
		
        [Constructable]
        public RunebookBlessDeed()
            : base(0x14F0)
        {
            Weight = 1.0;
            LootType = LootType.Blessed;
        }

        public RunebookBlessDeed(Serial serial)
            : base(serial)
        {
        }

        public override bool DisplayLootType
        {
            get
            {
                return Core.AOS;
            }
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            this.LootType = LootType.Blessed;

            int version = reader.ReadInt();
        }

        public override void OnDoubleClick(Mobile from) // Override double click of the deed to call our target
        {
            if (!this.IsChildOf(from.Backpack)) // Make sure its in their pack
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
            }
            else
            {
                from.SendLocalizedMessage(1005009); // What would you like to bless? (Clothes Only)
                from.Target = new RunebookBlessTarget(this); // Call our target
            }
        }
    }
}
