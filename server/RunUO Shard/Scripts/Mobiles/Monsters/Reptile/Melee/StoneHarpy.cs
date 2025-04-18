using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a stone harpy corpse" )]
	public class StoneHarpy : BaseCreature
	{
		[Constructable]
		public StoneHarpy() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a stone harpy";
			Body = 73;
			BaseSoundID = 402;
            Hue = 2131;

			SetStr( 200, 205 );
			SetDex( 45, 50 );
			SetInt( 15, 20 );

            SetHits( 275, 300);
			SetMana( 0 );

			SetDamage( 9, 18 );

            VirtualArmor = 25;

            SetSkill( SkillName.Tactics, 100.0, 100.0);

			SetSkill( SkillName.MagicResist, 30.0, 35.0 );

			SetSkill( SkillName.Wrestling, 65.0, 70.0 );

			Fame = 4500;
			Karma = -4500;			
		}        

        public override void GenerateLoot()
        {
            base.PackAddGoldTier(4);
            base.PackAddArtifactChanceTier(4);
            base.PackAddMapChanceTier(4);

            base.PackAddGemTier(3);
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

		public override int Meat{ get{ return 1; } }
		public override int Feathers{ get{ return 50; } }

		public StoneHarpy( Serial serial ) : base( serial )
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