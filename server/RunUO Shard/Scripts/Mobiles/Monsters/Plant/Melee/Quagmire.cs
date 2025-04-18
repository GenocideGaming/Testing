using System;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a quagmire corpse" )]
	public class Quagmire : BaseCreature
	{
		[Constructable]
		public Quagmire() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.4, 0.8 )
		{
			Name = "a quagmire";
			Body = 789;
			BaseSoundID = 352;

			SetStr( 101, 130 );
			SetDex( 66, 85 );
			SetInt( 31, 55 );

			SetHits(300, 350);

			SetDamage( 8, 16 );

            VirtualArmor = 5;

            
            
            

            SetSkill(SkillName.Tactics, 100.0, 100.0);

			SetSkill( SkillName.MagicResist, 25.0, 30.0 );			
			SetSkill( SkillName.Wrestling, 65.0, 70.0 );

			Fame = 1500;
			Karma = -1500;			
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average );
		}

		public override int GetAngerSound()
		{
			return 353;
		}

		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override Poison HitPoison{ get{ return Poison.Lethal; } }
		public override double HitPoisonChance{ get{ return 0.1; } }

		public Quagmire( Serial serial ) : base( serial )
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

			if ( BaseSoundID == -1 )
				BaseSoundID = 352;
		}
	}
}