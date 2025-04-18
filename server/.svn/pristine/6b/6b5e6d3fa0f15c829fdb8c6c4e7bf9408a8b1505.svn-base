using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a liche's corpse" )]
	public class Lich : BaseCreature
	{
		[Constructable]
		public Lich() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a lich";
			Body = 24;
			BaseSoundID = 0x3E9;

            SpellCastAnimation = 12;
            SpellCastFrameCount = 7;

			SetStr( 200, 205 );
			SetDex( 45, 50 );
			SetInt( 800, 1000 );

			SetHits( 175, 200 );
            SetMana( 800, 1000 );

			SetDamage( 8, 12 );

            VirtualArmor = 5;        

            SetSkill( SkillName.Tactics, 100.0, 100.0);
			
            SetSkill( SkillName.EvalInt, 80.0, 85.0);
			SetSkill( SkillName.Magery, 80.0, 85.0 );
            SetSkill( SkillName.Meditation, 100.0, 100.0);

			SetSkill( SkillName.MagicResist, 80.0, 85.0 );

            SetSkill( SkillName.Wrestling, 60.0, 65.0);

			Fame = 8000;
			Karma = -8000;			
		}		 
		
		public override void GenerateLoot()
		{
            base.PackAddGoldTier(5);
            base.PackAddArtifactChanceTier(5);
            base.PackAddMapChanceTier(5);

            base.PackAddScrollTier(3);
            base.PackAddReagentTier(3);

            PackItem( new BonePile());
			PackItem( new GnarledStaff() );

            if (0.75 > Utility.RandomDouble())
                PackItem(new BoneDust());
		}

        public override Poison PoisonImmune { get { return Poison.Lethal; } }

		public override OppositionGroup OppositionGroup
		{
			get{ return OppositionGroup.FeyAndUndead; }
		}

		public override bool CanRummageCorpses{ get{ return true; } }
		public override bool BleedImmune{ get{ return true; } }
				
		public Lich( Serial serial ) : base( serial )
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