using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a mongbat corpse" )]
	public class Mongbat : BaseCreature
	{
		[Constructable]
		public Mongbat() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a mongbat";
			Body = 39;
			BaseSoundID = 422;

			SetStr( 25, 30 );
			SetDex( 25, 30 );
			SetInt( 15, 20 );

			SetHits( 15, 20 );
			SetMana( 0 );

			SetDamage( 2, 4 );

            VirtualArmor = 10;

            SetSkill( SkillName.Tactics, 100.0, 100.0);

			SetSkill( SkillName.MagicResist, 5.0, 10.0 );
			SetSkill( SkillName.Wrestling, 5.0, 10.0 );

			Fame = 150;
			Karma = -150;	
		}

        public override void GenerateLoot()
        {
            base.PackAddGoldTier(1);
            base.PackAddArtifactChanceTier(1);
            base.PackAddMapChanceTier(1);
        }

		public override int Meat{ get{ return 1; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Meat; } }

		public Mongbat( Serial serial ) : base( serial )
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