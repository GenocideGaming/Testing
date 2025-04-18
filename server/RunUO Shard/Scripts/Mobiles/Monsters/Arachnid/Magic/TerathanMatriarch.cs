using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a terathan matriarch corpse" )]
	public class TerathanMatriarch : BaseCreature
	{
		[Constructable]
		public TerathanMatriarch() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a terathan matriarch";
			Body = 72;
			BaseSoundID = 599;

            SpellCastAnimation = 14;
            SpellCastFrameCount = 5;

			SetStr( 400, 405 );
			SetDex( 70, 75 );
			SetInt( 1000, 1200 );

			SetHits( 600, 700 );
            SetMana( 1000, 1200 );

			SetDamage( 14, 22 );

            VirtualArmor = 10;

            SetSkill( SkillName.Tactics, 100.0, 100.0);

			SetSkill( SkillName.EvalInt, 85.0, 90.0 );
			SetSkill( SkillName.Magery, 85.0, 90.0 );					
            SetSkill( SkillName.Meditation, 100.0, 100.0);

            SetSkill( SkillName.MagicResist, 85.0, 90.0);	

            SetSkill( SkillName.Wrestling, 70.0, 75.0);

			Fame = 10000;
			Karma = -10000;
		}		 
		
		public override void GenerateLoot()
		{
            base.PackAddGoldTier(9);
            base.PackAddArtifactChanceTier(9);
            base.PackAddMapChanceTier(9);

            base.PackAddScrollTier(7);
            base.PackAddReagentTier(4);
            base.PackAddPoisonPotionTier(6);
		}

        public override Poison PoisonImmune { get { return Poison.Deadly; } }
        public override Poison HitPoison { get { return Poison.Greater; } }

		public override OppositionGroup OppositionGroup
		{
			get{ return OppositionGroup.TerathansAndOphidians; }
		}

		public TerathanMatriarch( Serial serial ) : base( serial )
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