using System;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x1c0c, 0x1c0d )]
	public class StuddedBustierArms : BaseArmor
	{
        public override int InitMinHits { get { return 35; } }
        public override int InitMaxHits { get { return 45; } }

        public override int OldStrReq { get { return 15; } }

        public override int ArmorBase { get { return 3; } }
        public override int OldDexBonus { get { return -1; } }

		public override ArmorMaterialType MaterialType{ get{ return ArmorMaterialType.Studded; } }
        public override ArmorMeditationAllowance DefMedAllowance { get { return ArmorMeditationAllowance.ThreeQuarter; } }

		public override CraftResource DefaultResource{ get{ return CraftResource.RegularLeather; } }		

		public override bool AllowMaleWearer{ get{ return false; } }

		[Constructable]
		public StuddedBustierArms() : base( 0x1C0C )
		{
			Weight = 1.0;
		}

		public StuddedBustierArms( Serial serial ) : base( serial )
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
		}
	}
}