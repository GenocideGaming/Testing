using System;

namespace Server.Items
{
    public class Grasses : Item
	{
		[Constructable]
        public Grasses()
            : base(Utility.RandomList(0x0C93, 0x0C94, 0x0C97, 0x0C98, 0x0CA7, 0x0CAC, 0x0CAD, 0x0CAE, 0x0CAF, 0x0CB0, 0x0CB1, 0x0CB2, 0x0CB3, 0x0CB4, 0x0CB5, 0x0CB6, 0x0CB7, 0x0CB8, 0x0CB9, 0x0CBA, 0x0CBB, 0x0CBC, 0x0CBD, 0x0CC3, 0x0CC4, 0x0CC5, 0x0CC6, 0x0CC7, 0x1782, 0x1783, 0x1784, 0x1785, 0x1786, 0x1787, 0x1788, 0x1789))
		{
			Movable = true;
			Stackable = false;
		}

        public Grasses(Serial serial)
            : base(serial)
		{            
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}
