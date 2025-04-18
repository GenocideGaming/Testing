using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "an ancient liche's corpse" )]
	public class AncientLich : BaseCreature
	{
		[Constructable]
		public AncientLich() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = NameList.RandomName( "ancient lich" );
			Body = 78;
			BaseSoundID = 412;

            SpellCastAnimation = 12;
            SpellCastFrameCount = 7;

			SetStr( 400, 405 );
			SetDex( 95, 100 );
			SetInt( 4000, 5000 );

			SetHits( 1000, 1200 );
            SetMana( 4000, 5000);

			SetDamage( 16, 24 );

            VirtualArmor = 5;

            SetSkill(SkillName.Tactics, 100.0, 100.0);

			SetSkill( SkillName.EvalInt, 120.0, 125.0 );
			SetSkill( SkillName.Magery, 120.0, 125.0 );
			SetSkill( SkillName.Meditation, 100.0, 100.0 );			

			SetSkill( SkillName.MagicResist, 120, 125 );

			SetSkill( SkillName.Wrestling, 70.0, 75.0 );

			Fame = 23000;
			Karma = -23000;				
		}

        public override void GenerateLoot()
        {
            base.PackAddGoldTier(11);
            base.PackAddArtifactChanceTier(11);
            base.PackAddMapChanceTier(11);

            base.PackAddScrollTier(8);
            base.PackAddScrollTier(7);

            base.PackAddReagentTier(5);
            base.PackAddReagentTier(5);

            PackItem(new BonePile());
            PackItem(new GnarledStaff());

            if (0.99 > Utility.RandomDouble())
                PackItem(new BoneDust());
        }

        public override bool Unprovokable { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } }
        public override bool BleedImmune { get { return true; } }
       
		public override OppositionGroup OppositionGroup
		{
			get{ return OppositionGroup.FeyAndUndead; }
		}

		public override int GetIdleSound()
		{
			return 0x19D;
		}

		public override int GetAngerSound()
		{
			return 0x175;
		}

		public override int GetDeathSound()
		{
			return 0x108;
		}

		public override int GetAttackSound()
		{
			return 0xE2;
		}

		public override int GetHurtSound()
		{
			return 0x28B;
		}	

		public AncientLich( Serial serial ) : base( serial )
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