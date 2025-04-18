using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "a fire steed corpse" )]
	public class FireSteed : BaseMount
	{
		[Constructable]
		public FireSteed() : this( "a fire steed" )
		{
		}

		[Constructable]
		public FireSteed( string name ) : base( name, 0xBE, 0x3E9E, AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			BaseSoundID = 0xA8;

			SetStr( 376, 400 );
			SetDex( 91, 120 );
			SetInt( 291, 300 );

			SetHits( 300, 350 );

            SetDamage(10, 18);

            VirtualArmor = 5;

            
            
            

            SetSkill(SkillName.Tactics, 100.0, 100.0);

            SetSkill(SkillName.MagicResist, 80.0);
            SetSkill(SkillName.Wrestling, 55.0, 60.0);

			Fame = 20000;
			Karma = -20000;

			Tamable = true;
			ControlSlots = 2;
			MinTameSkill = 106.0;
		}

		public override void GenerateLoot()
		{
			PackItem( new SulfurousAsh( Utility.RandomMinMax( 151, 300 ) ) );
			PackItem( new Ruby( Utility.RandomMinMax( 16, 30 ) ) );
		}

		public override bool HasBreath{ get{ return true; } } // fire breath enabled
		public override FoodType FavoriteFood{ get{ return FoodType.Meat; } }
		public override PackInstinct PackInstinct{ get{ return PackInstinct.Daemon | PackInstinct.Equine; } }

		public FireSteed( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if ( BaseSoundID <= 0 )
				BaseSoundID = 0xA8;

			if( version < 1 )
			{
				for ( int i = 0; i < Skills.Length; ++i )
				{
					Skills[i].Cap = Math.Max( 100.0, Skills[i].Cap * 0.9 );

					if ( Skills[i].Base > Skills[i].Cap )
					{
						Skills[i].Base = Skills[i].Cap;
					}
				}
			}
		}
	}
}