using System;
using Server;

namespace Server.Items
{
	public class NorseHelm : BaseArmor
	{
        public override int InitMinHits { get { return 50; } }
        public override int InitMaxHits { get { return 65; } }

        public override int OldStrReq { get { return 40; } }

        public override int ArmorBase { get { return 6; } }
        public override int OldDexBonus { get { return -3; } }

		public override ArmorMaterialType MaterialType{ get{ return ArmorMaterialType.Plate; } }
        public override ArmorMeditationAllowance DefMedAllowance { get { return ArmorMeditationAllowance.None; } }

		[Constructable]
		public NorseHelm() : base( 0x140E )
		{
			Weight = 5.0;
		}

		public NorseHelm( Serial serial ) : base( serial )
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

			if ( Weight == 1.0 )
				Weight = 5.0;
		}
	}
}