using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a mongbat corpse" )]
	public class GreaterMongbat : BaseCreature
	{
		[Constructable]
		public GreaterMongbat() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a greater mongbat";
			Body = 39;
			BaseSoundID = 422;

			SetStr( 56, 80 );
			SetDex( 61, 80 );
			SetInt( 26, 50 );

            SetHits(60, 80);
            SetMana(0);

            SetDamage(5, 10);

            VirtualArmor = 10;

            
            
            

            SetSkill(SkillName.Tactics, 100.0, 100.0);

            SetSkill(SkillName.MagicResist, 5.0, 10.0);
            SetSkill(SkillName.Wrestling, 25.0, 30.0);

			Fame = 450;
			Karma = -450;

			VirtualArmor = 10;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 71.1;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Poor );
		}

		public override int Meat{ get{ return 1; } }
		public override int Hides{ get{ return 6; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Meat; } }

		public GreaterMongbat( Serial serial ) : base( serial )
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
