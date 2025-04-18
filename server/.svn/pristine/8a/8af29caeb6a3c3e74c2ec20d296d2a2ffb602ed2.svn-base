using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a centaur corpse" )]
	public class Centaur : BaseCreature
	{
		[Constructable]
		public Centaur() : base( AIType.AI_Generic, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			Name = NameList.RandomName( "centaur" );
			Body = 101;
			BaseSoundID = 679;

			SetStr( 202, 300 );
			SetDex( 104, 260 );
			SetInt( 91, 100 );

			SetHits( 250, 300 );

			SetDamage( 5, 10 );

            VirtualArmor = 10;

            
            
            

            SetSkill(SkillName.Tactics, 100.0, 100.0);
			
			SetSkill( SkillName.Archery, 80.0, 85.0 );
			SetSkill( SkillName.MagicResist, 40.0, 45.0 );			
			SetSkill( SkillName.Wrestling, 80.0, 85.0 );

			Fame = 6500;
			Karma = 0;
			
			AddItem( new Bow() );
			PackItem( new Arrow( Utility.RandomMinMax( 80, 90 ) ) ); // OSI it is different: in a sub backpack, this is probably just a limitation of their engine
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich );
			AddLoot( LootPack.Average );
			AddLoot( LootPack.Gems );
		}

		public override OppositionGroup OppositionGroup
		{
			get{ return OppositionGroup.FeyAndUndead; }
		}

		public override int Meat{ get{ return 1; } }
		public override int Hides{ get{ return 8; } }
		public override HideType HideType{ get{ return HideType.Spined; } }

		public Centaur( Serial serial ) : base( serial )
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

			if ( BaseSoundID == 678 )
				BaseSoundID = 679;
		}
	}
}