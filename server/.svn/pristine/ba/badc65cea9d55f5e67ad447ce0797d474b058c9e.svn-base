using System;
using Server.Items;
using Server.Targeting;
using System.Collections;

namespace Server.Mobiles
{
	[CorpseName( "a giant spider corpse" )]
	public class GiantSpider : BaseCreature
	{
		[Constructable]
		public GiantSpider() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a giant spider";
			Body = 28;
			BaseSoundID = 0x388;

			SetStr( 75, 80 );
			SetDex( 60, 65 );
			SetInt( 10, 15 );

			SetHits( 75, 100 );
			SetMana( 0 );

			SetDamage( 4, 8 );

            VirtualArmor = 10;

            SetSkill(SkillName.Tactics, 100.0, 100.0);

			SetSkill( SkillName.MagicResist, 25.0, 30.0 );

			SetSkill( SkillName.Wrestling, 50.0, 55.0 );

			Fame = 600;
			Karma = -600;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 60.0;
		}		 
		
		public override void GenerateLoot()
		{
            base.PackAddGoldTier(2);
            base.PackAddArtifactChanceTier(2);
            base.PackAddMapChanceTier(2);

            base.PackAddPoisonPotionTier(3);

            PackItem(new SpidersSilk(10));

            if (0.1 > Utility.RandomDouble())
                PackItem(new DrowGland());
		}

        public override Poison PoisonImmune { get { return Poison.Greater; } }
        public override Poison HitPoison { get { return Poison.Regular; } }

		public override FoodType FavoriteFood{ get{ return FoodType.Meat; } }
		public override PackInstinct PackInstinct{ get{ return PackInstinct.Arachnid; } }		

		public GiantSpider( Serial serial ) : base( serial )
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


        public override void AggressiveAction(Mobile aggressor, bool criminal)
        {
            base.AggressiveAction(aggressor, criminal);

            if (aggressor.HueMod == 1108)
            {
                AOS.Damage(aggressor, 50, 0, 100, 0, 0, 0);
                aggressor.BodyMod = 0;
                aggressor.HueMod = -1;
                aggressor.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);
                aggressor.PlaySound(0x307);
                aggressor.SendMessage("Your skin is scorched as the drow paint burns away!"); // Your skin is scorched as the tribal paint burns away!
            }
        }
	}
}