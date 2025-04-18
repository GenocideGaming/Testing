using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a balron corpse" )]
	public class Balron : BaseCreature
	{
		[Constructable]
		public Balron () : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = NameList.RandomName( "balron" );
			Body = 40;
			BaseSoundID = 357;

            SpellCastAnimation = 12;
            SpellCastFrameCount = 7;

			SetStr( 500, 550 );
			SetDex( 110, 115 );
			SetInt( 1800, 2000 );

			SetHits( 1000, 1200 );
            SetMana( 1000, 1200 );

			SetDamage( 25, 40 );

            VirtualArmor = 10;

            SetSkill(SkillName.Tactics, 100.0, 100.0);
			
			SetSkill( SkillName.EvalInt, 90.0, 95.0 );
            SetSkill( SkillName.Meditation, 100.0, 100.0);
            SetSkill( SkillName.Magery, 90.0, 95.0);

            SetSkill( SkillName.MagicResist, 90.0, 95.0);

			SetSkill( SkillName.Wrestling, 90.0, 95.0 );

			Fame = 24000;
			Karma = -24000;
		}
		
		public override void GenerateLoot()
		{
            base.PackAddGoldTier(11);
            base.PackAddArtifactChanceTier(11);
            base.PackAddMapChanceTier(11);

            base.PackAddScrollTier(8);
            base.PackAddReagentTier(4);

            base.PackAddArtifactChanceTier(8);

			PackItem( new Longsword() );			
		}

		public override bool CanRummageCorpses{ get{ return true; } }		
		
		public override int Meat{ get{ return 1; } }

		public Balron( Serial serial ) : base( serial )
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