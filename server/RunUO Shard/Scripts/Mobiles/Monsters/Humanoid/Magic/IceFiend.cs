using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "an ice fiend corpse" )]
	public class IceFiend : BaseCreature
	{
		[Constructable]
		public IceFiend () : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "an ice fiend";
			Body = 43;
			BaseSoundID = 357;

			SetStr( 376, 405 );
			SetDex( 176, 195 );
			SetInt( 201, 225 );

			SetHits( 350, 400 );

			SetDamage( 10, 16 );

            VirtualArmor = 20;

            
            
            

            SetSkill(SkillName.Tactics, 100.0, 100.0);

			SetSkill( SkillName.EvalInt, 60.0, 65.0 );
			SetSkill( SkillName.Magery, 60.0, 65.0 );
			SetSkill( SkillName.MagicResist, 60.0, 65.0 );
			SetSkill( SkillName.Wrestling, 70.1, 75.0 );

			Fame = 18000;
			Karma = -18000;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich );
			AddLoot( LootPack.Average );
			AddLoot( LootPack.MedScrolls, 2 );
		}

		
		public override int Meat{ get{ return 1; } }

		public IceFiend( Serial serial ) : base( serial )
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