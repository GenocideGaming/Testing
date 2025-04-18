using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a ghostly corpse" )]
	public class Bogle : BaseCreature
	{
		[Constructable]
		public Bogle() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a bogle";
			Body = 153;
			BaseSoundID = 0x482;

            SpellCastAnimation = 6;
            SpellCastFrameCount = 4;

			SetStr( 100, 150 );
			SetDex( 35, 40 );
			SetInt( 200, 300 );

			SetHits( 100, 125 );
            SetMana( 200, 300 ); 

			SetDamage( 4, 8 );

            VirtualArmor = 5;

            SetSkill( SkillName.Tactics, 100.0, 100.0);

            SetSkill( SkillName.Meditation, 100.0, 100.0);
			SetSkill( SkillName.EvalInt, 40.0, 45.0 );
			SetSkill( SkillName.Magery, 40.0, 45.0 );

			SetSkill( SkillName.MagicResist, 40.0, 45.0 );

			SetSkill( SkillName.Wrestling, 45.0, 50.0 );

			Fame = 4000;
			Karma = -4000;
		}		 
		
		public override void GenerateLoot()
		{
            base.PackAddGoldTier(4);
            base.PackAddArtifactChanceTier(4);
            base.PackAddMapChanceTier(4);

            base.PackAddScrollTier(1);
            base.PackAddReagentTier(2);

            PackItem(new Bone());
            PackItem(new BonePile());

            if (0.2 > Utility.RandomDouble())
                PackItem(new BoneDust());
		}

		public override bool BleedImmune{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }

		public Bogle( Serial serial ) : base( serial )
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

        public override void AggressiveAction(Mobile aggressor, bool criminal)
        {
            base.AggressiveAction(aggressor, criminal);

            if (aggressor.HueMod == 1882)
            {
                AOS.Damage(aggressor, 50, 0, 100, 0, 0, 0);
                aggressor.BodyMod = 0;
                aggressor.HueMod = -1;
                aggressor.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);
                aggressor.PlaySound(0x307);
                aggressor.SendMessage("Your skin is scorched as the undead paint burns away!"); // Your skin is scorched as the tribal paint burns away!
            }
        }
	}
}