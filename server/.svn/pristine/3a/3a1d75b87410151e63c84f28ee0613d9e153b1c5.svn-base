using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "an imp corpse" )]
	public class Imp : BaseCreature
	{
		[Constructable]
		public Imp() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "an imp";
			Body = 74;
			BaseSoundID = 422;

            SpellCastAnimation = 16;
            SpellCastFrameCount = 3;

			SetStr( 30, 35 );
			SetDex( 40, 45 );
			SetInt( 400, 500 );

			SetHits( 50, 75 );
            SetMana( 400, 500 );

			SetDamage( 2, 4 );

            VirtualArmor = 5;

            SetSkill(SkillName.Tactics, 100.0, 100.0);

            SetSkill( SkillName.Meditation, 100.0, 100.0);
			SetSkill( SkillName.EvalInt, 50.0, 55.0 );
			SetSkill( SkillName.Magery, 50.0, 55.0 );

			SetSkill( SkillName.MagicResist, 50.0, 55.0 );

			SetSkill( SkillName.Wrestling, 35.0, 40.0 );

			Fame = 2500;
			Karma = -2500;
		}		 
		
		public override void GenerateLoot()
		{
            base.PackAddGoldTier(3);
            base.PackAddArtifactChanceTier(3);
            base.PackAddMapChanceTier(3);

            base.PackAddScrollTier(1);
            base.PackAddReagentTier(1);
		}

		public override int Meat{ get{ return 1; } }
		public override int Hides{ get{ return 6; } }
		public override HideType HideType{ get{ return HideType.Spined; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Meat; } }
		public override PackInstinct PackInstinct{ get{ return PackInstinct.Daemon; } }

		public Imp( Serial serial ) : base( serial )
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