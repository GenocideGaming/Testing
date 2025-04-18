using System;
using Server;
using Server.Gumps;

namespace Server.Items
{
    [Flipable(0x1E5E, 0x1E5F)]
    class BountyBoard : Item
    {
        [Constructable]
		public BountyBoard() : base( 0x1E5E )
		{
            Name = "bounty board";
            Movable = false;
		}

        public BountyBoard(Serial serial)
            : base(serial)
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

        public override void OnDoubleClick(Mobile from)
        {
            if (from.HasGump(typeof(BountyBoardGump)))
                from.CloseGump(typeof(BountyBoardGump));
            from.SendGump(new BountyBoardGump(from));

            base.OnDoubleClick(from);
        }
    }
}
