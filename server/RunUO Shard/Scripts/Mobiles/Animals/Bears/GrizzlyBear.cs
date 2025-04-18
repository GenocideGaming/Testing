using System;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "a grizzly bear corpse" )]
	[TypeAlias( "Server.Mobiles.Grizzlybear" )]
	public class GrizzlyBear : BaseCreature
	{
		[Constructable]
		public GrizzlyBear() : base( AIType.AI_Generic, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			Name = "a grizzly bear";
			Body = 212;
			BaseSoundID = 0xA3;

			SetStr( 126, 155 );
			SetDex( 81, 105 );
			SetInt( 16, 40 );

			SetHits( 120, 140 );
			SetMana( 0 );

			SetDamage( 7, 14 );           

            VirtualArmor = 10;

            SetSkill(SkillName.Tactics, 100.0, 100.0);

			SetSkill( SkillName.MagicResist, 25.1, 40.0 );			
			SetSkill( SkillName.Wrestling, 50.0, 55.0 );

			Fame = 1000;
			Karma = 0;			

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 65.0;
		}

		public override int Meat{ get{ return 2; } }
		public override int Hides{ get{ return 16; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Fish | FoodType.FruitsAndVegies | FoodType.Meat; } }
		public override PackInstinct PackInstinct{ get{ return PackInstinct.Bear; } }

		public GrizzlyBear( Serial serial ) : base( serial )
		{
		}

		public override void Serialize(GenericWriter writer)
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