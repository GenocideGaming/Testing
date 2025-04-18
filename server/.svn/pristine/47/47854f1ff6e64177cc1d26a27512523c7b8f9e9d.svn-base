using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a gargoyle corpse" )]
	public class Gargoyle : BaseCreature
	{
		[Constructable]
		public Gargoyle() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a gargoyle";
			Body = 4;
			BaseSoundID = 372;

            SpellCastAnimation = 12;
            SpellCastFrameCount = 7;

			SetStr( 175, 180 );
			SetDex( 45, 50 );
			SetInt( 400, 500 );

			SetHits( 175, 200 );
            SetMana( 400, 500);

			SetDamage( 6, 12 );

            VirtualArmor = 25;

            SetSkill( SkillName.Tactics, 100.0, 100.0);

            SetSkill( SkillName.Meditation, 100.0, 100.0);
			SetSkill( SkillName.EvalInt, 60.0, 65.0 );
			SetSkill( SkillName.Magery, 60.0, 65.0 );

			SetSkill( SkillName.MagicResist, 60.0, 65.0 );

			SetSkill( SkillName.Wrestling, 60.0, 65.0 );

			Fame = 3500;
			Karma = -3500;
		}		 
		
		public override void GenerateLoot()
		{
            base.PackAddGoldTier(5);
            base.PackAddArtifactChanceTier(5);
            base.PackAddMapChanceTier(5);

            base.PackAddScrollTier(2);
            base.PackAddReagentTier(2);
            base.PackAddGemTier(1);
		}
		
		public override int Meat{ get{ return 1; } }

		public Gargoyle( Serial serial ) : base( serial )
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