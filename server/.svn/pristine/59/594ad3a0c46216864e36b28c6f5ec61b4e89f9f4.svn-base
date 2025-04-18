using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a mummy corpse" )]
	public class Mummy : BaseCreature
	{
		[Constructable]
		public Mummy() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.4, 0.8 )
		{
			Name = "a mummy";
			Body = 154;
			BaseSoundID = 471;

			SetStr( 300, 305 );
			SetDex( 35, 40 );
			SetInt( 20, 25 );

			SetHits( 700, 800 );

			SetDamage( 15, 25 );

            VirtualArmor = 5;

            SetSkill( SkillName.Tactics, 100.0, 100.0);

			SetSkill( SkillName.MagicResist, 45.0, 50.0 );

			SetSkill( SkillName.Wrestling, 25.0, 30.0 );

			Fame = 4000;
			Karma = -4000;
		}
		
		public override void GenerateLoot()
		{
            base.PackAddGoldTier(5);
            base.PackAddArtifactChanceTier(5);
            base.PackAddMapChanceTier(5);

            base.PackAddBandageTier(5);

            if (0.4 > Utility.RandomDouble())
                PackItem(new BoneDust());
		}

        public override Poison PoisonImmune { get { return Poison.Lethal; } }
		public override bool BleedImmune{ get{ return true; } }	

		public Mummy( Serial serial ) : base( serial )
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