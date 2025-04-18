using System;
using Server.Items;
using Server.Targeting;
using System.Collections;

namespace Server.Mobiles
{
	[CorpseName( "a terathan warrior corpse" )]
	public class TerathanWarrior : BaseCreature
	{
		[Constructable]
		public TerathanWarrior() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a terathan warrior";
			Body = 70;
			BaseSoundID = 589;

			SetStr( 145, 150 );
			SetDex( 75, 80 );
			SetInt( 20, 25 );

			SetHits( 225, 250 );
			SetMana( 0 );

			SetDamage( 10, 18 );

            VirtualArmor = 10;

            SetSkill(SkillName.Tactics, 100.0, 100.0);
			
			SetSkill( SkillName.MagicResist, 45.0, 50.0 );

            SetSkill(SkillName.Wrestling, 70.0, 75.0);

			Fame = 4000;
			Karma = -4000;
        }		 
		
		public override void GenerateLoot()
		{
            base.PackAddGoldTier(4);
            base.PackAddArtifactChanceTier(4);
            base.PackAddMapChanceTier(4);

            base.PackAddPoisonPotionTier(4); 
		}

        public override Poison PoisonImmune { get { return Poison.Greater; } }
        public override Poison HitPoison { get { return Poison.Regular; } }
		
		public override int Meat{ get{ return 4; } }

		public override OppositionGroup OppositionGroup
		{
			get{ return OppositionGroup.TerathansAndOphidians; }
		}

		public TerathanWarrior( Serial serial ) : base( serial )
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