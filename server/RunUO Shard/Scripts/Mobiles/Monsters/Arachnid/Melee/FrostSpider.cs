using System;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a frost spider corpse" )]
	public class FrostSpider : BaseCreature
	{
		[Constructable]
		public FrostSpider() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a frost spider";
			Body = 20;
			BaseSoundID = 0x388;

			SetStr( 76, 100 );
			SetDex( 126, 145 );
			SetInt( 36, 60 );

			SetHits( 100, 125 );
			SetMana( 0 );

			SetDamage( 6, 12 );

            VirtualArmor = 10;

            
            
            

            SetSkill(SkillName.Tactics, 100.0, 100.0);

			SetSkill( SkillName.MagicResist, 25.1, 40.0 );
			SetSkill( SkillName.Wrestling, 55.0, 60.0 );

			Fame = 775;
			Karma = -775;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 74.7;

			PackItem( new SpidersSilk( 7 ) );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Meager );
			AddLoot( LootPack.Poor );
		}

		public override FoodType FavoriteFood{ get{ return FoodType.Meat; } }
		public override PackInstinct PackInstinct{ get{ return PackInstinct.Arachnid; } }

		public FrostSpider( Serial serial ) : base( serial )
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

			if ( BaseSoundID == 387 )
				BaseSoundID = 0x388;
		}
	}
}