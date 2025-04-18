using System;
using Server;
using Server.Misc;
using Server.Items;
using System.Collections.Generic;

namespace Server.Mobiles
{
	[CorpseName( "a glowing orc corpse" )]
	public class OrcishMage : BaseCreature
	{
		public override InhumanSpeech SpeechType{ get{ return InhumanSpeech.Orc; } }

		[Constructable]
		public OrcishMage () : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "an orcish mage";
			Body = 140;
			BaseSoundID = 0x45A;

            SpellCastAnimation = 11;
            SpellCastFrameCount = 5;

			SetStr( 100, 105 );
			SetDex( 45, 50 );
			SetInt( 500, 600 );

			SetHits( 125, 150 );
            SetMana( 500, 600 );

			SetDamage( 4, 8 );

            VirtualArmor = 5;

            SetSkill( SkillName.Tactics, 100.0, 100.0);

			SetSkill( SkillName.EvalInt, 55.0, 60.0 );
			SetSkill( SkillName.Magery, 55.0, 60.0 );			
            SetSkill( SkillName.Meditation, 100.0, 100.0);

            SetSkill( SkillName.MagicResist, 55.0, 60.0);

			SetSkill( SkillName.Wrestling, 55.0, 60.0 );

			Fame = 3000;
			Karma = -3000;
 		}

		public override void GenerateLoot()
		{
            base.PackAddGoldTier(4);
            base.PackAddArtifactChanceTier(4);
            base.PackAddMapChanceTier(4);

            base.PackAddScrollTier(2);
            base.PackAddReagentTier(2);

            if (0.05 > Utility.RandomDouble())
                PackItem(new OrcishKinMask());

            if (0.2 > Utility.RandomDouble())
                PackItem(new OrcishHeart());
		}

		public override bool CanRummageCorpses{ get{ return true; } }
		
		public override int Meat{ get{ return 1; } }

		public override OppositionGroup OppositionGroup
		{
			get{ return OppositionGroup.SavagesAndOrcs; }
		}

        public override bool IsEnemy(Mobile m)  
		{
			if ( m.Player && m.FindItemOnLayer( Layer.Helm ) is OrcishKinMask )
				return false;

			return base.IsEnemy( m );
		}

		public override void AggressiveAction( Mobile aggressor, bool criminal )
		{
            base.AggressiveAction(aggressor, criminal);

            if (aggressor.HueMod == 1451)
            {
                AOS.Damage(aggressor, 50, 0, 100, 0, 0, 0);
                aggressor.BodyMod = 0;
                aggressor.HueMod = -1;
                aggressor.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);
                aggressor.PlaySound(0x307);
                aggressor.SendMessage("Your skin is scorched as the orcish paint burns away!"); // Your skin is scorched as the tribal paint burns away!
            }
		}

		public OrcishMage( Serial serial ) : base( serial )
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
