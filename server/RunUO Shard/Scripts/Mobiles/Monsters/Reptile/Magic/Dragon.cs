using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a dragon corpse" )]
	public class Dragon : BaseCreature
	{
		[Constructable]
		public Dragon () : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a dragon";
			Body = Utility.RandomList( 12, 59 );
			BaseSoundID = 362;

            SpellCastAnimation = 11;
            SpellCastFrameCount = 5;

			SetStr( 500, 505 );
			SetDex( 70, 75 );
			SetInt( 500, 600 );

			SetHits( 800, 900 );
            SetMana( 500, 600);

			SetDamage( 20, 30 );

            VirtualArmor = 20;

            SetSkill( SkillName.Tactics, 100.0, 100.0);

            SetSkill( SkillName.Meditation, 100.0, 100.0);
			SetSkill( SkillName.EvalInt, 30.0, 35.0 );
			SetSkill( SkillName.Magery, 30.0, 35.0 );

			SetSkill( SkillName.MagicResist, 45.0, 50.0 );

			SetSkill( SkillName.Wrestling, 75.0, 80.0 );

			Fame = 15000;
			Karma = -15000;			

			Tamable = true;
			ControlSlots = 3;
            MinTameSkill = 97.0;			
		}		 
		
		public override void GenerateLoot()
		{
            base.PackAddGoldTier(8);
            base.PackAddArtifactChanceTier(8);
            base.PackAddMapChanceTier(8);

            base.PackAddScrollTier(6);
            base.PackAddReagentTier(4);

            base.PackAddGemTier(2);

            PackItem(new BonePile());
            PackItem(new BonePile());
		}

		public override bool ReacquireOnMovement{ get{ return !Controlled; } }
		public override bool HasBreath{ get{ return true; } } // fire breath enabled
		public override bool AutoDispel{ get{ return !Controlled; } }
		
		public override int Meat{ get{ return 19; } }
		public override int Hides{ get{ return 20; } }
		public override HideType HideType{ get{ return HideType.Barbed; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Meat; } }		

		public Dragon( Serial serial ) : base( serial )
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