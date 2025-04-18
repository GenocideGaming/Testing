using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a gargoyle corpse" )]
	public class StoneGargoyle : BaseCreature
	{
		[Constructable]
		public StoneGargoyle() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a stone gargoyle";
			Body = 67;
			BaseSoundID = 0x174;

			SetStr( 246, 275 );
			SetDex( 76, 95 );
			SetInt( 81, 105 );

			SetHits( 150, 175 );

			SetDamage( 8, 16 );

            VirtualArmor = 30;

            
            
            

            SetSkill(SkillName.Tactics, 100.0, 100.0);

			SetSkill( SkillName.MagicResist, 55.0, 60.0 );
			SetSkill( SkillName.Wrestling, 60.0, 65.0 );

			Fame = 4000;
			Karma = -4000;			

			PackItem( new IronIngot( 12 ) );

			if ( 0.05 > Utility.RandomDouble() )
				PackItem( new GargoylesPickaxe() );
		}

        

        public override void GenerateLoot()
        {
            
        }

		

		public StoneGargoyle( Serial serial ) : base( serial )
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