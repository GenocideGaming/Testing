using System;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a gargoyle corpse" )]
	public class GargoyleDestroyer : BaseCreature
	{
		[Constructable]
		public GargoyleDestroyer() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Gargoyle Destroyer";
			Body = 0x2F3;
			BaseSoundID = 0x174;

            SpellCastAnimation = 12;
            SpellCastFrameCount = 7;

			SetStr( 400, 405 );
			SetDex( 40, 45 );
			SetInt( 800, 1000 );

			SetHits( 700, 800 );
            SetMana( 800, 1000 );

			SetDamage( 15, 25 );

            VirtualArmor = 25;
            
            SetSkill(SkillName.Tactics, 100.0, 100.0);

            SetSkill(SkillName.EvalInt, 75.0, 80.0);
            SetSkill(SkillName.Magery, 75.0, 80.0);
            SetSkill(SkillName.Meditation, 100.0, 100.0);

            SetSkill(SkillName.MagicResist, 75.0, 80.0);

            SetSkill(SkillName.Wrestling, 90.0, 95.0);

			Fame = 10000;
			Karma = -10000;
		}

		public override void GenerateLoot()
		{
            base.PackAddGoldTier(7);
            base.PackAddArtifactChanceTier(7);
            base.PackAddMapChanceTier(7);

            base.PackAddScrollTier(5);
            base.PackAddReagentTier(3);
            base.PackAddGemTier(3);
		}
		
		public override int Meat{ get{ return 1; } }		

		public GargoyleDestroyer( Serial serial ) : base( serial )
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