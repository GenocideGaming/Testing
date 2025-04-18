using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a corpser corpse" )]
	public class Corpser : BaseCreature
	{
		[Constructable]
		public Corpser() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a corpser";
			Body = 8;
			BaseSoundID = 684;

			SetStr( 100, 105 );
			SetDex( 60, 65 );
			SetInt( 20, 25 );

			SetHits( 175, 200 );			

			SetDamage( 5, 10 );

            VirtualArmor = 5;

            SetSkill( SkillName.Tactics, 100.0, 100.0);

			SetSkill( SkillName.MagicResist, 25.0, 30.0 );			
			SetSkill( SkillName.Wrestling, 45.0, 50.0 );

			Fame = 1000;
			Karma = -1000;	
		}

        public override void GenerateLoot()
        {
            base.PackAddGoldTier(1);
            base.PackAddArtifactChanceTier(1);
            base.PackAddMapChanceTier(1);

            base.PackAddBoardTier(4);

            PackItem(new MandrakeRoot(5));
        }

		public override bool DisallowAllMoves{ get{ return true; } }

		public Corpser( Serial serial ) : base( serial )
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

			if ( BaseSoundID == 352 )
				BaseSoundID = 684;
		}
	}
}
