using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a slimey corpse" )]
	public class Slime : BaseCreature
	{
		[Constructable]
		public Slime() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a slime";
			Body = 51;
			BaseSoundID = 456;

			Hue = Utility.RandomSlimeHue();

			SetStr( 22, 34 );
			SetDex( 16, 21 );
			SetInt( 16, 20 );

			SetHits( 15, 25 );

			SetDamage( 2, 4 );

            VirtualArmor = 5;

            SetSkill(SkillName.Tactics, 100.0, 100.0);
			
			SetSkill( SkillName.MagicResist, 15.0, 20.0 );

			SetSkill( SkillName.Wrestling, 10.0, 15.0 );

			Fame = 300;
			Karma = -300;	
		}

        public override void GenerateLoot()
        {
            base.PackAddGoldTier(1);
            base.PackAddArtifactChanceTier(1);
            base.PackAddMapChanceTier(1);
        }

		public override Poison PoisonImmune{ get{ return Poison.Lesser; } }		
		public override FoodType FavoriteFood{ get{ return FoodType.Meat | FoodType.Fish | FoodType.FruitsAndVegies | FoodType.GrainsAndHay | FoodType.Eggs; } }

		public Slime( Serial serial ) : base( serial )
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
