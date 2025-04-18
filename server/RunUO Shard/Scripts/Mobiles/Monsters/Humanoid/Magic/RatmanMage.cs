using System;
using System.Collections;
using Server.Misc;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a glowing ratman corpse" )]
	public class RatmanMage : BaseCreature
	{
		public override InhumanSpeech SpeechType{ get{ return InhumanSpeech.Ratman; } }

		[Constructable]
		public RatmanMage() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = NameList.RandomName( "ratman" );
			Body = 0x8F;
			BaseSoundID = 437;

            SpellCastAnimation = 11;
            SpellCastFrameCount = 5;

            SetStr(100, 105);
            SetDex(45, 50);
            SetInt(500, 600);

            SetHits(100, 125);
            SetMana(500, 600);

            SetDamage(3, 7);

            VirtualArmor = 5;

            SetSkill(SkillName.Tactics, 100.0, 100.0);

            SetSkill(SkillName.EvalInt, 55.0, 60.0);
            SetSkill(SkillName.Magery, 55.0, 60.0);
            SetSkill(SkillName.Meditation, 100.0, 100.0);

            SetSkill(SkillName.MagicResist, 55.0, 60.0);

            SetSkill(SkillName.Wrestling, 50.0, 55.0);

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
        }

		public override bool CanRummageCorpses{ get{ return true; } }
		public override int Meat{ get{ return 1; } }
		public override int Hides{ get{ return 8; } }
		public override HideType HideType{ get{ return HideType.Spined; } }

		public RatmanMage( Serial serial ) : base( serial )
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

			if ( Body == 42 )
			{
				Body = 0x8F;
				Hue = 0;
			}
		}
	}
}
