using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "an ant lion corpse" )]
	public class AntLion : BaseCreature
	{
		[Constructable]
		public AntLion() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "an ant lion";
			Body = 787;
			BaseSoundID = 1006;

			SetStr( 296, 320 );
			SetDex( 81, 105 );
			SetInt( 36, 60 );

			SetHits( 250, 300 );

			SetDamage( 15, 30 );

            VirtualArmor = 25;

            
            
            

            SetSkill(SkillName.Tactics, 100.0, 100.0);

			SetSkill( SkillName.MagicResist, 70.0 );			
			SetSkill( SkillName.Wrestling, 80.0 );

			Fame = 4500;
			Karma = -4500;

			PackItem( new Bone( 3 ) );
			PackItem( new FertileDirt( Utility.RandomMinMax( 1, 5 ) ) );

			if ( Core.ML && Utility.RandomDouble() < .33 )
				PackItem( Engines.Plants.Seed.RandomPeculiarSeed(2) );

			Item orepile = null; /* no trust, no love :( */

			switch (Utility.Random(4))
			{
				case 0: orepile = new DullCopperOre(); break;
				case 1: orepile = new ShadowIronOre(); break;
				case 2: orepile = new CopperOre(); break;
				default: orepile = new BronzeOre(); break;
			}
			orepile.Amount = Utility.RandomMinMax(1, 10);
			orepile.ItemID = 0x19B9;
			PackItem(orepile);

			// TODO: skeleton
		}

		public override int GetAngerSound()
		{
			return 0x5A;
		}

		public override int GetIdleSound()
		{
			return 0x5A;
		}

		public override int GetAttackSound()
		{
			return 0x164;
		}

		public override int GetHurtSound()
		{
			return 0x187;
		}

		public override int GetDeathSound()
		{
			return 0x1BA;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average, 2 );
		}


		public AntLion( Serial serial ) : base( serial )
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