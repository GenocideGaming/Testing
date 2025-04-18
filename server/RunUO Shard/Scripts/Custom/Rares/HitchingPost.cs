using System;

namespace Server.Items
{
    public class HitchingPost : Item
	{
		[Constructable]
        public HitchingPost()
            : base(Utility.RandomList(0x14E7, 0x14E8))
		{
			Movable = true;
			Stackable = false;
		}

        public HitchingPost(Serial serial)
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
