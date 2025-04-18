using System;
using Server;

namespace Server.Items
{
	public class SeaChart : RelPorMapItem
	{
		[Constructable]
		public SeaChart() : base(-1)
		{
			SetDisplay( 0, 0);
		}

		public override void CraftInit( Mobile from )
		{
			double skillValue = from.Skills[SkillName.Cartography].Value;
			int dist = 64 + (int)(skillValue * 10);

			if ( dist < 200 )
				dist = 200;

			int size = 24 + (int)(skillValue * 3.3);

			if ( size < 200 )
				size = 200;
			else if ( size > 400 )
				size = 400;

			SetDisplay( from.X, from.Y);
		}

		public override int LabelNumber{ get{ return 1015232; } } // sea chart

		public SeaChart( Serial serial ) : base( serial )
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