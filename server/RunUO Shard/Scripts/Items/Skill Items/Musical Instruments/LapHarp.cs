using System;

namespace Server.Items
{
	public class LapHarp : BaseInstrument
	{
		[Constructable]
		public LapHarp() : base( 0xEB2, 0x45, 0x46 )
		{
			Weight = 10.0;
		}

		public LapHarp( Serial serial ) : base( serial )
		{
		}
        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Lap Harp");

            if (this.Crafter != null)
            {
                LabelTo(from, "Crafted by {0}", this.Crafter.Name);
            }
            // If Quality is Exceptional
            if (this.Quality == InstrumentQuality.Exceptional)
            {
                if (this.Slayer != SlayerName.None && this.Slayer2 != SlayerName.None)
                {
                    // Display Quality, Slayer, and Slayer2 on the same line
                    LabelTo(from, "[{0}/{1}/{2}]", this.Quality, this.Slayer, this.Slayer2);
                }
                else if (this.Slayer != SlayerName.None)
                {
                    // Display Quality and Slayer
                    LabelTo(from, "[{0}/{1}]", this.Quality, this.Slayer);
                }
                else
                {
                    // Display only Quality
                    LabelTo(from, "{0}", this.Quality);
                }
            }
            else
            {
                // If Quality is not Exceptional and Slayer exists
                if (this.Slayer != SlayerName.None && this.Slayer2 != SlayerName.None)
                {
                    // Display Slayer and Slayer2 on the same line
                    LabelTo(from, "[{0}/{1}]", this.Slayer, this.Slayer2);
                }
                else if (this.Slayer != SlayerName.None)
                {
                    // Display only Slayer
                    LabelTo(from, "{0}", this.Slayer);
                }
            }
            LabelTo(from, "Durability : {0}", this.UsesRemaining);
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

			if ( Weight == 3.0 )
				Weight = 10.0;
		}
	}
}