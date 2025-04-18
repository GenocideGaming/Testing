using System;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x13da, 0x13e1 )]
	public class StuddedLegs : BaseArmor
	{
        public override int InitMinHits { get { return 35; } }
        public override int InitMaxHits { get { return 45; } }

        public override int OldStrReq { get { return 15; } }

        public override int ArmorBase { get { return 4; } }
        public override int OldDexBonus { get { return -1; } }

		public override ArmorMaterialType MaterialType{ get{ return ArmorMaterialType.Studded; } }
        public override ArmorMeditationAllowance DefMedAllowance { get { return ArmorMeditationAllowance.ThreeQuarter; } }

		public override CraftResource DefaultResource{ get{ return CraftResource.RegularLeather; } }
        
        [Constructable]
		public StuddedLegs() : base( 0x13DA )
		{
			Weight = 5.0;
		}

		public StuddedLegs( Serial serial ) : base( serial )
		{
		}
		
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}
		
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();

			if ( Weight == 3.0 )
				Weight = 5.0;
		}
	}
}