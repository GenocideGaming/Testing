using System;
using System.Collections.Generic;
using Server;
using Server.Multis;
using Server.Targeting;
using Server.ContextMenus;
using Server.Gumps;

namespace Server.Items
{
    public static class CitizenDyeHue
    {
        public static int Yew { get { return 2148; } }
        public static int Trinsic { get { return 1254; } }
        public static int Blackrock { get { return 1507; } }

        public static int Find(string townName)
        {
            switch (townName)
            {
                case "Yew": { return Yew; }
                case "Trinsic": { return Trinsic; }
                case "Blackrock": { return Blackrock; }
                default: { return 0; }
            }
        }
    }

	public class YewDyeTub : DyeTub
	{
		[Constructable]
		public YewDyeTub()
		{
            Name = "Yew dye tub";
			Hue = DyedHue = CitizenDyeHue.Yew;
			Redyable = false;
		}

        public YewDyeTub(Serial serial) : base(serial)
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

    public class TrinsicDyeTub : DyeTub
    {
        [Constructable]
        public TrinsicDyeTub()
        {
            Name = "Trinsic dye tub";
            Hue = DyedHue = CitizenDyeHue.Trinsic;
            Redyable = false;
        }

        public TrinsicDyeTub(Serial serial) : base(serial) { }

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
    public class BlackrockDyeTub : DyeTub
    {
        [Constructable]
        public BlackrockDyeTub()
        {
            Name = "Blackrock dye tub";
            Hue = DyedHue = CitizenDyeHue.Trinsic;
            Redyable = false;
        }

        public BlackrockDyeTub(Serial serial) : base(serial) { }

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