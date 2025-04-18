using System;

namespace Server.Items
{
	public class FullSpellbook : Spellbook
	{
		[Constructable]
		public FullSpellbook()
			: base(UInt64.MaxValue)
		{ }

		public FullSpellbook(Serial serial)
			: base(serial)
		{ }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadEncodedInt();
        }
    }
}
