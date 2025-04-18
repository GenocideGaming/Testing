using System;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "a polar bear corpse" )]
	[TypeAlias( "Server.Mobiles.Polarbear" )]
	public class EncagedPolarBear : BaseCreature
	{
		[Constructable]
		public EncagedPolarBear() : base( AIType.AI_Generic, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			Name = "an encaged polar bear";
			Body = 213;
			BaseSoundID = 0xA3;

			SetStr( 116, 140 );
			SetDex( 81, 105 );
			SetInt( 26, 50 );

			SetHits( 130, 150 );
			SetMana( 0 );

			SetDamage( 8, 16 );

            VirtualArmor = 10;

            SetSkill(SkillName.Tactics, 100.0, 100.0);

            SetSkill(SkillName.MagicResist, 45.1, 60.0);		
			SetSkill( SkillName.Wrestling, 55.0, 60.0 );

			Fame = 1500;
			Karma = 0;			

			Tamable = false;
		}

		public override int Meat{ get{ return 2; } }
		public override int Hides{ get{ return 16; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Fish | FoodType.FruitsAndVegies | FoodType.Meat; } }
		public override PackInstinct PackInstinct{ get{ return PackInstinct.Bear; } }

        public EncagedPolarBear(Serial serial)
            : base(serial)
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