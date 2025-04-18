using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a titans corpse" )]
	public class Titan : BaseCreature
	{
		[Constructable]
		public Titan() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a titan";
			Body = 75;
			BaseSoundID = 609;

            SpellCastAnimation = 18;
            SpellCastFrameCount = 5;

			SetStr( 600, 605 );
			SetDex( 45, 50 );
			SetInt( 600, 800 );

			SetHits( 900, 1000 );
            SetMana( 600, 800);

			SetDamage( 40, 60 );

            VirtualArmor = 5;

            SetSkill( SkillName.Tactics, 100.0, 100.0);

            SetSkill( SkillName.Meditation, 100.0, 100.0);
			SetSkill( SkillName.EvalInt, 55.0, 60.0 );
			SetSkill( SkillName.Magery, 55.0, 60.0 );

			SetSkill( SkillName.MagicResist, 60.0, 65.0 );

			SetSkill( SkillName.Wrestling, 60.0, 65.0 );

			Fame = 11500;
			Karma = -11500;
		}		 
		
		public override void GenerateLoot()
		{
            base.PackAddGoldTier(8);
            base.PackAddArtifactChanceTier(8);
            base.PackAddMapChanceTier(8);

            base.PackAddScrollTier(4);
            base.PackAddReagentTier(2);

            base.PackAddGemTier(4);
		}

		public override int Meat{ get{ return 4; } }
		public override Poison PoisonImmune{ get{ return Poison.Regular; } }
		

		public Titan( Serial serial ) : base( serial )
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