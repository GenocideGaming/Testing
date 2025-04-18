using System;
using System.Collections.Generic;
using System.Text;
using Server.Engines.Craft;
using Server.Scripts;
using Server.Scripts.Custom.Crafting;

namespace Server.Items
{
    //This is just a layer for checking if we can craft with a given tool
    public class ImbuingTool : BaseTool
    {
        public override CraftSystem CraftSystem
        {
            get { throw new NotImplementedException(); }
        }

      
        public ImbuingTool(int itemID)
            : base(FeatureList.ArtifactCrafting.ImbuingToolUses, itemID)
        {
        }

        public ImbuingTool(int uses, int itemID) : base(uses, itemID) {}
        public ImbuingTool(Serial serial) : base(serial) { }

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

        public override void OnDoubleClick(Mobile from)
        {
            //from.BeginTarget(0, false, Targeting.TargetFlags.None, new TargetCallback(ItemTargetted));
            //from.SendMessage(FeatureList.ArtifactCrafting.ImbuingToolDoubleClickMessage);
            from.SendGump(new ImbuingGump(from, this));
        }

        public virtual void ItemTargetted(Mobile from, object o)
        {
            // override for each specific tool
            
        }

        public static bool SuccessRoll(double skillValue, double itemRequiredSkill)
        {
            if (skillValue < itemRequiredSkill)
                return false;

            double successChance = GetSuccessChance(skillValue, itemRequiredSkill);

            if (successChance >= Utility.RandomDouble())
                return true;
            else
                return false;

        }

        public static double GetSuccessChance(double skillValue, double itemRequiredSkill)
        {
            return FeatureList.ArtifactCrafting.BaseSuccessChance + (skillValue - itemRequiredSkill) / (100.0 - itemRequiredSkill);
        }
    } 


    public class ImbuingHammer : ImbuingTool
    {
        public override int LabelNumber { get { return 1063515; } }

        public override CraftSystem CraftSystem
        {
            get { return DefBlacksmithy.CraftSystem; }
        }

        public override int  Hue
        {
	        get 
	        { 
		        return 0x482;
	        }
	        set 
	        { 
		        base.Hue = value;
	        }
        }

        [Constructable]
        public ImbuingHammer() : base(FeatureList.ArtifactCrafting.ImbuingToolUses, 0x13E4) { }

        [Constructable]
        public ImbuingHammer(int uses) : base(uses, 0x13E4) { }

        public ImbuingHammer(Serial serial) : base(serial) { }

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

    public class ImbuingSaw : ImbuingTool
    {
        public override int LabelNumber { get { return 1063516; } }

        public override CraftSystem CraftSystem
        {
            get { return DefCarpentry.CraftSystem; }
        }

        public override int  Hue
        {
	        get 
	        { 
		        return 0x482;
	        }
	        set 
	        { 
		        base.Hue = value;
	        }
        }

        [Constructable]
        public ImbuingSaw() : base(FeatureList.ArtifactCrafting.ImbuingToolUses, 0x1028) { }

        [Constructable]
        public ImbuingSaw(int uses) : base(uses, 0x1028) { }

        public ImbuingSaw(Serial serial) : base(serial) { }

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

    public class ImbuingSewingKit : ImbuingTool
    {
        public override int LabelNumber { get { return 1063517; } }

        public override CraftSystem CraftSystem
        {
            get { return DefTailoring.CraftSystem; }
        }

         public override int  Hue
        {
	        get 
	        { 
		        return 0x482;
	        }
	        set 
	        { 
		        base.Hue = value;
	        }
        }

        [Constructable]
        public ImbuingSewingKit() : base(FeatureList.ArtifactCrafting.ImbuingToolUses, 0xF9D) { }

        [Constructable]
        public ImbuingSewingKit(int uses) : base(uses, 0xF9D) { }

        public ImbuingSewingKit(Serial serial) : base(serial) { }

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

    public class ImbuingFletchingTools : ImbuingTool
    {
        public override int LabelNumber { get { return 1063518;  } }

        public override CraftSystem CraftSystem
        {
            get { return DefCarpentry.CraftSystem; }
        }

        public override int Hue
        {
            get
            {
                return 0x482;
            }
            set
            {
                base.Hue = value;
            }
        }

        [Constructable]
        public ImbuingFletchingTools() : base(FeatureList.ArtifactCrafting.ImbuingToolUses, 0x1022) { }

        [Constructable]
        public ImbuingFletchingTools(int uses) : base(uses, 0x1022) { }

        public ImbuingFletchingTools(Serial serial) : base(serial) { }

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
