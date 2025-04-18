using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a scorpion corpse" )]
	public class Scorpion : BaseCreature
	{
		[Constructable]
		public Scorpion() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a scorpion";
			Body = 48;
			BaseSoundID = 397;

			SetStr( 100, 105 );
			SetDex( 50, 55 );
			SetInt( 15, 20 );

			SetHits( 75, 100 );			

			SetDamage( 5, 10 );

            VirtualArmor = 20;
            
            SetSkill( SkillName.Tactics, 100.0, 100.0);
			
			SetSkill( SkillName.MagicResist, 20.0, 25.0 );

			SetSkill( SkillName.Wrestling, 50.0, 55.0 );

			Fame = 2000;
			Karma = -2000;			

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 70.0;
		}
		
		public override void GenerateLoot()
		{
            base.PackAddGoldTier(3);
            base.PackAddArtifactChanceTier(3);
            base.PackAddMapChanceTier(3);

            base.PackAddPoisonPotionTier(6);
		}

        public override Poison PoisonImmune { get { return Poison.Deadly; } }
        public override Poison HitPoison { get { return Poison.Greater; } }
		
		public override int Meat{ get{ return 1; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Meat; } }
		public override PackInstinct PackInstinct{ get{ return PackInstinct.Arachnid; } }		

		public Scorpion( Serial serial ) : base( serial )
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