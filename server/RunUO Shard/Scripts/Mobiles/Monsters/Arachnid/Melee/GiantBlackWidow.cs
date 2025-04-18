using System;
using Server.Items;
using Server.Targeting;
using System.Collections;

namespace Server.Mobiles
{
	[CorpseName( "a giant black widow spider corpse" )] // stupid corpse name
	public class GiantBlackWidow : BaseCreature
	{
		[Constructable]
		public GiantBlackWidow() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a giant black widow";
			Body =  0x9D;
			BaseSoundID = 0x388; // TODO: validate

			SetStr( 95, 100 );
			SetDex( 75, 80 );
			SetInt( 10, 15 );

			SetHits( 125, 150 );

			SetDamage( 5, 10 );

            VirtualArmor = 10;

            SetSkill(SkillName.Tactics, 100.0, 100.0);
			
			SetSkill( SkillName.MagicResist, 45.0, 50.0 );

			SetSkill( SkillName.Wrestling, 65.0, 70.0 );

			Fame = 3500;
			Karma = -3500;

            Tamable = true;
            ControlSlots = 3;
            MinTameSkill = 95.0;
		}		 
		
		public override void GenerateLoot()
		{
            base.PackAddGoldTier(4);
            base.PackAddArtifactChanceTier(4);
            base.PackAddMapChanceTier(4);

            base.PackAddPoisonPotionTier(7);

            PackItem(new SpidersSilk(15));

            if (0.35 > Utility.RandomDouble())
                PackItem(new DrowGland());
		}

        public override Poison PoisonImmune { get { return Poison.Lethal; } }
        public override Poison HitPoison { get { return Poison.Lethal; } }

		public override FoodType FavoriteFood{ get{ return FoodType.Meat; } }        

		public GiantBlackWidow( Serial serial ) : base( serial )
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