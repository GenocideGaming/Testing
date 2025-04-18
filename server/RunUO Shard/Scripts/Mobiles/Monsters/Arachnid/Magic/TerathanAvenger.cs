using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a terathan avenger corpse" )]
	public class TerathanAvenger : BaseCreature
	{
		[Constructable]
		public TerathanAvenger() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a terathan avenger";
			Body = 152;
			BaseSoundID = 0x24D;

            SpellCastAnimation = 11;
            SpellCastFrameCount = 5;

			SetStr( 250, 255 );
			SetDex( 80, 85 );
			SetInt( 400, 500 );

			SetHits( 300, 350 );
			SetMana( 400, 500 );

			SetDamage( 14, 22 );

            VirtualArmor = 10;

            SetSkill(SkillName.Tactics, 100.0, 100.0);

			SetSkill( SkillName.EvalInt, 50.0, 55.0 );
            SetSkill(SkillName.Meditation, 100.0, 100.0);
			SetSkill( SkillName.Magery, 50.0, 55.0 );

			SetSkill( SkillName.MagicResist, 45.0, 50.0 );

			SetSkill( SkillName.Wrestling, 75.0, 80.0 );

			Fame = 15000;
			Karma = -15000;			
		}
		
		public override void GenerateLoot()
		{
            base.PackAddGoldTier(6);
            base.PackAddArtifactChanceTier(6);
            base.PackAddMapChanceTier(6);

            base.PackAddScrollTier(5);
            base.PackAddPoisonPotionTier(5);
		}

		public override Poison PoisonImmune{ get{ return Poison.Deadly; } }
		public override Poison HitPoison{ get{ return Poison.Greater; } }
		
		public override int Meat{ get{ return 2; } }

		public override OppositionGroup OppositionGroup
		{
			get{ return OppositionGroup.TerathansAndOphidians; }
		}

		public TerathanAvenger( Serial serial ) : base( serial )
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

			if ( BaseSoundID == 263 )
				BaseSoundID = 0x24D;
		}
	}
}
