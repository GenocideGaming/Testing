using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a giant scorpion corpse" )]
	public class GiantScorpion : BaseCreature
	{
		[Constructable]
		public GiantScorpion() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a giant scorpion";
			Body = 48;
			BaseSoundID = 397;

			SetStr( 100, 105 );
			SetDex( 50, 55 );
			SetInt( 15, 20 );

			SetHits( 175, 225 );			

			SetDamage( 18, 24 );

            VirtualArmor = 75;
            
            SetSkill( SkillName.Tactics, 100.0, 100.0);
			
			SetSkill( SkillName.MagicResist, 30.0, 50.0 );

			SetSkill( SkillName.Wrestling, 55.0, 60.0 );

			Fame = 2000;
			Karma = -2000;			

			Tamable = true;
			ControlSlots = 2;
			MinTameSkill = 70.0;
		}
		
		public override void GenerateLoot()
		{
            base.PackAddGoldTier(3);
            base.PackAddArtifactChanceTier(3);
            base.PackAddMapChanceTier(3);

            base.PackAddPoisonPotionTier(5);
		}

        public override Poison PoisonImmune { get { return Poison.Deadly; } }
        public override Poison HitPoison { get { return Poison.Greater; } }
		
		public override int Meat{ get{ return 1; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Meat; } }
		public override PackInstinct PackInstinct{ get{ return PackInstinct.Arachnid; } }		

		public GiantScorpion( Serial serial ) : base( serial )
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