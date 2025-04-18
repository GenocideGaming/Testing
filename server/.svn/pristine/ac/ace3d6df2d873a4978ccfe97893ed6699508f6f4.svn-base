using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a white wyrm corpse" )]
	public class WhiteWyrm : BaseCreature
	{
		[Constructable]
		public WhiteWyrm () : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Body = (Utility.Random(1,2)==1) ? 180 :  49;
			Name = "a white wyrm";
			BaseSoundID = 362;

            SpellCastAnimation = 11;
            SpellCastFrameCount = 5;

			SetStr( 400, 405 );
			SetDex( 65, 70 );
			SetInt( 1400, 1600 );

            SetHits(800, 900);
            SetMana(1400, 1600);

            SetDamage(18, 28);

            VirtualArmor = 20;
            
            SetSkill(SkillName.Tactics, 100.0, 100.0);

            SetSkill(SkillName.EvalInt, 80.0, 85.0);
            SetSkill(SkillName.Meditation, 100, 100.0);
            SetSkill(SkillName.Magery, 80.0, 85.0);

            SetSkill(SkillName.MagicResist, 80.0, 85.0);

            SetSkill(SkillName.Wrestling, 70.0, 75.0);

			Fame = 18000;
			Karma = -18000;

			Tamable = true;
			ControlSlots = 4;
            MinTameSkill = 99.6;            
		}		
		 
		public override void GenerateLoot()
		{
            base.PackAddGoldTier(8);
            base.PackAddArtifactChanceTier(8);
            base.PackAddMapChanceTier(8);

            base.PackAddScrollTier(8);

            base.PackAddReagentTier(4);
            base.PackAddReagentTier(4);

            PackItem(new BonePile());
            PackItem(new BonePile());
		}

		public override bool ReacquireOnMovement{ get{ return true; } }
        public override bool HasBreath { get { return true; } } // fire breath enabled
		
		public override int Meat{ get{ return 19; } }
		public override int Hides{ get{ return 20; } }
		public override HideType HideType{ get{ return HideType.Barbed; } }
		public override int Scales{ get{ return 9; } }
		public override ScaleType ScaleType{ get{ return ScaleType.White; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Meat | FoodType.Gold; } }		

		public WhiteWyrm( Serial serial ) : base( serial )
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

			if ( Core.AOS && Body == 49 )
				Body = 180;
		}
	}
}