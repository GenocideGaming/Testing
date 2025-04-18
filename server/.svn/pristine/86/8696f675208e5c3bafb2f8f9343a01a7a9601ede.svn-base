using System;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x1c04, 0x1c05 )]
	public class FemalePlateChest : BaseArmor
	{
        public override int InitMinHits { get { return 50; } }
        public override int InitMaxHits { get { return 65; } }

        public override int OldStrReq { get { return 40; } }

        public override int ArmorBase { get { return 11; } }
        public override int OldDexBonus { get { return -7; } }

		public override ArmorMaterialType MaterialType{ get{ return ArmorMaterialType.Plate; } }
        public override ArmorMeditationAllowance DefMedAllowance { get { return ArmorMeditationAllowance.None; } }

		[Constructable]
		public FemalePlateChest() : base( 0x1C04 )
		{
			Weight = 4.0;
		}

		public FemalePlateChest( Serial serial ) : base( serial )
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
				Weight = 4.0;
		}
	}
}