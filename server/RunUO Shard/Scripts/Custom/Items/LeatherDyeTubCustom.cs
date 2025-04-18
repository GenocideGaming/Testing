using System;

namespace Server.Items
{
    public class LeatherDyeTubCustom : DyeTub
    {
        public override bool AllowDyables { get { return false; } }
        public override bool AllowLeather { get { return true; } }
        public override int TargetMessage { get { return 1042416; } } // Select the leather item to dye.
        public override int FailMessage { get { return 1042418; } } // You can only dye leather with this tub.
        public override int LabelNumber { get { return 1041284; } } // Leather Dye Tub
        //public override CustomHuePicker CustomHuePicker { get { return CustomHuePicker.LeatherDyeTub; } }

        private int mUsesRemaining;
        private int mDyeHue;

        public int UsesRemaining { get { return mUsesRemaining; } set { mUsesRemaining = value; } }

        public int DyeHue { get { return mDyeHue; } set { mDyeHue = value; } }

        

       // [Constructable]
        public LeatherDyeTubCustom()
        {
            
            LootType = LootType.Regular;
            Weight = 10.0;
            UsesRemaining = 10;
            
        }

        public override void OnSingleClick(Mobile from)
        {
            DisplayDurabilityTo(from);
            base.OnSingleClick(from);
        }

        public override void OnDoubleClick(Mobile from)
        {
            UsesRemaining--;
            if (UsesRemaining <= 0)
                Delete();

            else
            from.PlaySound(0x23E);

            base.OnDoubleClick(from);
        }

        public LeatherDyeTubCustom(Serial serial)
            : base(serial)
        {
        }

        public virtual void DisplayDurabilityTo(Mobile m)
        {
            LabelTo(m, "Charges : {0}", mUsesRemaining);
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            list.Add(1060584, mUsesRemaining.ToString());
            
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version

            writer.Write((int)mUsesRemaining);

           
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            mUsesRemaining = reader.ReadInt();

            
        }
    }



    public class LeatherDyeTubDullCopper : LeatherDyeTubCustom
    {
        [Constructable]
        public LeatherDyeTubDullCopper()
        {
            Name = "Dull Copper Leather Dye Tub";
        }
        
    public override CustomHuePicker CustomHuePicker { get { return CustomHuePicker.LeatherDyeTubDullCopper; } }



    public LeatherDyeTubDullCopper(Serial serial)
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

    public class LeatherDyeTubShadowIron : LeatherDyeTubCustom
    {
        [Constructable]
        public LeatherDyeTubShadowIron()
        {
            Name = "Shadow Iron Leather Dye Tub";
        }

        public override CustomHuePicker CustomHuePicker { get { return CustomHuePicker.LeatherDyeTubShadowIron; } }



        public LeatherDyeTubShadowIron(Serial serial)
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

    public class LeatherDyeTubCopper : LeatherDyeTubCustom
    {
        [Constructable]
        public LeatherDyeTubCopper()
        {
            Name = "Copper Leather Dye Tub";
        }

        public override CustomHuePicker CustomHuePicker { get { return CustomHuePicker.LeatherDyeTubCopper; } }



        public LeatherDyeTubCopper(Serial serial)
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

    public class LeatherDyeTubBronze : LeatherDyeTubCustom
    {
        [Constructable]
        public LeatherDyeTubBronze()
        {
            Name = "Bronze Leather Dye Tub";
        }

        public override CustomHuePicker CustomHuePicker { get { return CustomHuePicker.LeatherDyeTubBronze; } }



        public LeatherDyeTubBronze(Serial serial)
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

    public class LeatherDyeTubGolden : LeatherDyeTubCustom
    {
        [Constructable]
        public LeatherDyeTubGolden()
        {
            Name = "Golden Leather Dye Tub";
        }

        public override CustomHuePicker CustomHuePicker { get { return CustomHuePicker.LeatherDyeTubGolden; } }



        public LeatherDyeTubGolden(Serial serial)
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

    public class LeatherDyeTubAgapite : LeatherDyeTubCustom
    {
        [Constructable]
        public LeatherDyeTubAgapite()
        {
            Name = "Agapite Leather Dye Tub";
        }

        public override CustomHuePicker CustomHuePicker { get { return CustomHuePicker.LeatherDyeTubAgapite; } }



        public LeatherDyeTubAgapite(Serial serial)
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

    public class LeatherDyeTubVerite : LeatherDyeTubCustom
    {
        [Constructable]
        public LeatherDyeTubVerite()
        {
            Name = "Verite Leather Dye Tub";
        }

        public override CustomHuePicker CustomHuePicker { get { return CustomHuePicker.LeatherDyeTubVerite; } }



        public LeatherDyeTubVerite(Serial serial)
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

    public class LeatherDyeTubValorite : LeatherDyeTubCustom
    {
        [Constructable]
        public LeatherDyeTubValorite()
        {
            Name = "Valorite Leather Dye Tub";
        }

        public override CustomHuePicker CustomHuePicker { get { return CustomHuePicker.LeatherDyeTubValorite; } }



        public LeatherDyeTubValorite(Serial serial)
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

    public class LeatherDyeTubReds : LeatherDyeTubCustom
    {
        [Constructable]
        public LeatherDyeTubReds()
        {
            Name = "Red Leather Dye Tub";
        }

        public override CustomHuePicker CustomHuePicker { get { return CustomHuePicker.LeatherDyeTubReds; } }



        public LeatherDyeTubReds(Serial serial)
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

    public class LeatherDyeTubBlues : LeatherDyeTubCustom
    {
        [Constructable]
        public LeatherDyeTubBlues()
        {
            Name = "Blue Leather Dye Tub";
        }

        public override CustomHuePicker CustomHuePicker { get { return CustomHuePicker.LeatherDyeTubBlues; } }



        public LeatherDyeTubBlues(Serial serial)
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

    public class LeatherDyeTubGreens : LeatherDyeTubCustom
    {
        [Constructable]
        public LeatherDyeTubGreens()
        {
            Name = "Green Leather Dye Tub";
        }

        public override CustomHuePicker CustomHuePicker { get { return CustomHuePicker.LeatherDyeTubGreens; } }



        public LeatherDyeTubGreens(Serial serial)
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

    public class LeatherDyeTubYellows : LeatherDyeTubCustom
    {
        [Constructable]
        public LeatherDyeTubYellows()
        {
            Name = "Yellow Leather Dye Tub";
        }

        public override CustomHuePicker CustomHuePicker { get { return CustomHuePicker.LeatherDyeTubYellows; } }



        public LeatherDyeTubYellows(Serial serial)
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