using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a rotting corpse" )]
	public class RottingCorpse : BaseCreature
	{
		[Constructable]
		public RottingCorpse() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a rotting corpse";
			Body = 155;
			BaseSoundID = 471;

			SetStr( 100, 105 );
			SetDex( 15, 20 );
			SetInt( 151, 200 );

			SetHits( 300, 400 );
			SetStam( 150 );
			SetMana( 0 );

			SetDamage( 4, 8 );

            VirtualArmor = 5;

            SetSkill( SkillName.Tactics, 100.0, 100.0);
			
            SetSkill( SkillName.MagicResist, 25.0, 30.0 );
			SetSkill( SkillName.Wrestling, 5.0, 10.0 );

			Fame = 6000;
			Karma = -6000;			
		}

		public override void GenerateLoot()
		{
            base.PackAddGoldTier(5);
            base.PackAddArtifactChanceTier(5);
            base.PackAddMapChanceTier(5);

            base.PackAddBandageTier(4);

            if (0.5 > Utility.RandomDouble())
                PackItem(new BoneDust());
		}

		public override bool BleedImmune{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override Poison HitPoison{ get{ return Poison.Deadly; } }		

		public RottingCorpse( Serial serial ) : base( serial )
		{
		}

		public override OppositionGroup OppositionGroup
		{
			get{ return OppositionGroup.FeyAndUndead; }
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