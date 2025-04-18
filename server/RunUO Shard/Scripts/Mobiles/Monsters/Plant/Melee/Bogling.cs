using System;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a plant corpse" )]
	public class Bogling : BaseCreature
	{
		[Constructable]
		public Bogling() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a bogling";
			Body = 779;
			BaseSoundID = 422;

			SetStr( 96, 120 );
			SetDex( 91, 115 );
			SetInt( 21, 45 );

			SetHits( 125, 150 );

			SetDamage( 5, 10 );

            VirtualArmor = 5;

            
            
            

            SetSkill(SkillName.Tactics, 100.0, 100.0);

			SetSkill( SkillName.MagicResist, 25.0, 30.0 );			
			SetSkill( SkillName.Wrestling, 35.0, 40.0 );

			Fame = 450;
			Karma = -450;			

			PackItem( new Log( 4 ) );
			PackItem( new Engines.Plants.Seed() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Meager );
		}

		public override int Hides{ get{ return 6; } }
		public override int Meat{ get{ return 1; } }

		public Bogling( Serial serial ) : base( serial )
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