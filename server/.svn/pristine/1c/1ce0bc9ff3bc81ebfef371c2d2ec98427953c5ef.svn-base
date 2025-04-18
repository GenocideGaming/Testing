using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a harpy corpse" )]
	public class Harpy : BaseCreature
	{
		[Constructable]
		public Harpy() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a harpy";
			Body = 30;
			BaseSoundID = 402;

			SetStr( 80, 85 );
			SetDex( 55, 60 );
			SetInt( 15, 20 );

			SetHits( 100, 125 );

			SetDamage( 6, 12 );

            VirtualArmor = 5;

            SetSkill( SkillName.Tactics, 100.0, 100.0);

			SetSkill( SkillName.MagicResist, 25.0, 30.0 );

			SetSkill( SkillName.Wrestling, 45.0, 50.0 );

			Fame = 2500;
			Karma = -2500;			
		}

        public override void GenerateLoot()
        {
            base.PackAddGoldTier(2);
            base.PackAddArtifactChanceTier(2);
            base.PackAddMapChanceTier(2);

            base.PackAddGemTier(1);
        }

		public override int GetAttackSound()
		{
			return 916;
		}

		public override int GetAngerSound()
		{
			return 916;
		}

		public override int GetDeathSound()
		{
			return 917;
		}

		public override int GetHurtSound()
		{
			return 919;
		}

		public override int GetIdleSound()
		{
			return 918;
		}

		public override bool CanRummageCorpses{ get{ return true; } }
		public override int Meat{ get{ return 4; } }
		public override MeatType MeatType{ get{ return MeatType.Bird; } }
		public override int Feathers{ get{ return 50; } }

		public Harpy( Serial serial ) : base( serial )
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