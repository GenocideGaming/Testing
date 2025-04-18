using System;
using Server;
using Server.Misc;

namespace Server.Mobiles
{
	[CorpseName( "a pixie corpse" )]
	public class Pixie : BaseCreature
	{
		public override bool InitialInnocent{ get{ return true; } }

		[Constructable]
		public Pixie() : base( AIType.AI_Generic, FightMode.Evil, 10, 1, 0.2, 0.4 )
		{
			Name = NameList.RandomName( "pixie" );
			Body = 128;
			BaseSoundID = 0x467;

			SetStr( 21, 30 );
			SetDex( 301, 400 );
			SetInt( 201, 250 );

			SetHits( 30, 40 );

			SetDamage( 3, 5 );

            VirtualArmor = 5;

            
            
            

            SetSkill( SkillName.Tactics, 100.0, 100.0);

			SetSkill( SkillName.EvalInt, 90.1, 100.0 );
			SetSkill( SkillName.Magery, 90.1, 100.0 );
			SetSkill( SkillName.Meditation, 90.1, 100.0 );
			SetSkill( SkillName.MagicResist, 100.5, 150.0 );			
			SetSkill( SkillName.Wrestling, 100.0, 100.0);

			Fame = 7000;
			Karma = 7000;
			
			if ( 0.02 > Utility.RandomDouble() )
				PackStatue();
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.LowScrolls );
			AddLoot( LootPack.Gems, 2 );
		}

		public override HideType HideType{ get{ return HideType.Spined; } }
		public override int Hides{ get{ return 5; } }
		public override int Meat{ get{ return 1; } }

		public Pixie( Serial serial ) : base( serial )
		{
		}

		public override OppositionGroup OppositionGroup
		{
			get{ return OppositionGroup.FeyAndUndead; }
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