using System;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a swamp tentacle corpse" )]
	public class SwampTentacle : BaseCreature
	{
		[Constructable]
		public SwampTentacle() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a swamp tentacle";
			Body = 66;
			BaseSoundID = 352;

			SetStr( 96, 120 );
			SetDex( 66, 85 );
			SetInt( 16, 30 );

			SetHits( 250, 300 );
			SetMana( 0 );

			SetDamage( 7, 14 );

            VirtualArmor = 5;

            
            
            

            SetSkill(SkillName.Tactics, 100.0, 100.0);

			SetSkill( SkillName.MagicResist, 20.0, 25.0 );			
			SetSkill( SkillName.Wrestling, 55.0, 60.0 );

			Fame = 3000;
			Karma = -3000;			

			PackReg( 3 );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average );
		}

		public override Poison PoisonImmune{ get{ return Poison.Greater; } }

		public SwampTentacle( Serial serial ) : base( serial )
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