using System;
using Server.Items;
using Server.Targeting;
using System.Collections;

namespace Server.Mobiles
{
	[CorpseName( "a terathan drone corpse" )]
	public class TerathanDrone : BaseCreature
	{
		[Constructable]
		public TerathanDrone() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a terathan drone";
			Body = 71;
			BaseSoundID = 594;

			SetStr( 80, 85 );
			SetDex( 50, 55 );
			SetInt( 20, 25 );

			SetHits( 100, 125 );
			SetMana( 0 );

			SetDamage( 5, 10 );

            VirtualArmor = 10;

            SetSkill( SkillName.Tactics, 100.0, 100.0);

			SetSkill( SkillName.MagicResist, 25.0, 30.0 );

			SetSkill( SkillName.Wrestling, 60.0, 65.0 );

			Fame = 2000;
			Karma = -2000;
		}

        public override Poison PoisonImmune { get { return Poison.Regular; } }
        public override Poison HitPoison { get { return Poison.Lesser; } }         
		
		public override void GenerateLoot()
		{
            base.PackAddGoldTier(3);
            base.PackAddArtifactChanceTier(3);
            base.PackAddMapChanceTier(3);

            base.PackAddPoisonPotionTier(3);            
		}

		public override int Meat{ get{ return 4; } }

		public override OppositionGroup OppositionGroup
		{
			get{ return OppositionGroup.TerathansAndOphidians; }
		}

		public TerathanDrone( Serial serial ) : base( serial )
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

			if ( BaseSoundID == 589 )
				BaseSoundID = 594;
		}
	}
}