using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "an acid elemental corpse" )]
	public class ToxicElemental : BaseCreature
	{
		[Constructable]
		public ToxicElemental () : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "an acid elemental";
			Body = 0x9E;
			BaseSoundID = 278;

            SpellCastAnimation = 3;
            SpellCastFrameCount = 4;

            SetStr(245, 250);
            SetDex(40, 45);
            SetInt(600, 800);

            SetHits(325, 350);
            SetMana(600, 800);

            SetDamage(8, 14);

            VirtualArmor = 5;

            SetSkill(SkillName.Tactics, 100.0, 100.0);
			
			SetSkill( SkillName.EvalInt, 60.0, 65.0 );
            SetSkill(SkillName.Meditation, 100.0, 100.0);
			SetSkill( SkillName.Magery, 60.0, 65.0 );

			SetSkill( SkillName.MagicResist, 195.0, 200.0 );

			SetSkill( SkillName.Wrestling, 50.0, 55.0 );

			Fame = 10000;
			Karma = -10000;

            CanSwim = true;
		}

		public override void GenerateLoot()
		{
            base.PackAddGoldTier(6);
            base.PackAddArtifactChanceTier(6);
            base.PackAddMapChanceTier(6);

            base.PackAddScrollTier(5);

            base.PackAddPoisonPotionTier(7);

            PackItem(new Nightshade(20));
		}

		public override bool BleedImmune{ get{ return true; } }

        public override Poison PoisonImmune { get { return Poison.Lethal; } }
		public override Poison HitPoison{ get{ return Poison.Deadly; } }
		public override double HitPoisonChance{ get{ return 0.667; } }		

		public ToxicElemental( Serial serial ) : base( serial )
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

			if ( BaseSoundID == 263 )
				BaseSoundID = 278;

			if ( Body == 13 )
				Body = 0x9E;

			if ( Hue == 0x4001 )
				Hue = 0;
		}
	}
}