using System;
using Server.Items;

namespace Server.Items
{
	public class PlateGorget : BaseArmor
	{
        public override int InitMinHits { get { return 50; } }
        public override int InitMaxHits { get { return 65; } }

        public override int OldStrReq { get { return 40; } }

        public override int ArmorBase { get { return 5; } }
        public override int OldDexBonus { get { return -2; } }

		public override ArmorMaterialType MaterialType{ get{ return ArmorMaterialType.Plate; } }
        public override ArmorMeditationAllowance DefMedAllowance { get { return ArmorMeditationAllowance.None; } }

		[Constructable]
		public PlateGorget() : base( 0x1413 )
		{
			Weight = 2.0;
		}

		public PlateGorget( Serial serial ) : base( serial )
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