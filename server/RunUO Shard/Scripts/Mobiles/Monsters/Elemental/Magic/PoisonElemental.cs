using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a poison elementals corpse" )]
	public class PoisonElemental : BaseCreature
	{
		[Constructable]
		public PoisonElemental () : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a poison elemental";
			Body = 162;
			BaseSoundID = 263;

            SpellCastAnimation = 3;
            SpellCastFrameCount = 4;

			SetStr( 300, 305 );
			SetDex( 75, 80 );
			SetInt( 600, 800 );

			SetHits( 500, 600 );
            SetMana( 600, 800 );

			SetDamage( 5, 10 );

            VirtualArmor = 5;

            SetSkill(SkillName.Tactics, 100.0, 100.0);

			SetSkill( SkillName.EvalInt, 65.0, 70.0 );
			SetSkill( SkillName.Magery, 65.0, 70.0 );
            SetSkill( SkillName.Meditation, 100.0, 100.0);
			
			SetSkill( SkillName.MagicResist, 95.0, 100.0 );

			SetSkill( SkillName.Wrestling, 50.0, 55.0 );

			Fame = 12500;
			Karma = -12500;

            CanSwim = true;
		}		 
		
		public override void GenerateLoot()
		{
            base.PackAddGoldTier(7);
            base.PackAddArtifactChanceTier(7);
            base.PackAddMapChanceTier(7);

            base.PackAddScrollTier(6);

            base.PackAddPoisonPotionTier(8);

            PackItem(new Nightshade(30));
		}
		
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override Poison HitPoison{ get{ return Poison.Lethal; } }
		public override double HitPoisonChance{ get{ return 0.75; } }

        public override bool BleedImmune { get { return true; } }

		public PoisonElemental( Serial serial ) : base( serial )
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