using System;

namespace Server.Items
{
	public class Tambourine : BaseInstrument
	{
		[Constructable]
		public Tambourine() : base( 0xE9D, 0x52, 0x53 )
		{
			Weight = 1.0;
		}

		public Tambourine( Serial serial ) : base( serial )
		{
		}
        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Tambourine");

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

			if ( Weight == 2.0 )
				Weight = 1.0;
		}
	}
}