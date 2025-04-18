using System;
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server.Misc;

namespace Server.Mobiles
{
	[CorpseName( "a lizardman corpse" )]
	public class Lizardman : BaseCreature
	{
		public override InhumanSpeech SpeechType{ get{ return InhumanSpeech.Lizardman; } }

		[Constructable]
		public Lizardman() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = NameList.RandomName( "lizardman" );
			Body = Utility.RandomList( 35, 36 );
			BaseSoundID = 417;

            SetStr(100, 105);
            SetDex(45, 50);
            SetInt(20, 25);

            SetHits(100, 125);

            SetDamage(6, 12);

            VirtualArmor = 5;

            SetSkill(SkillName.Tactics, 100.0, 100.0);

            SetSkill(SkillName.MagicResist, 25.0, 30.0);

            SetSkill(SkillName.Wrestling, 55.0, 60.0);

			Fame = 1500;
			Karma = -1500;			
		}		 
		
		public override void GenerateLoot()
		{
            base.PackAddGoldTier(2);
            base.PackAddArtifactChanceTier(2);
            base.PackAddMapChanceTier(2);

            base.PackAddGemTier(1);

			PackItem( new ShortSpear() );			
		}

		public override bool CanRummageCorpses{ get{ return true; } }
		public override int Meat{ get{ return 1; } }
		public override int Hides{ get{ return 12; } }
		public override HideType HideType{ get{ return HideType.Spined; } }

		public Lizardman( Serial serial ) : base( serial )
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