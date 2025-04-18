using System;
using Server;
using Server.Misc;
using Server.Items;
using Server.Factions;

namespace Server.Mobiles
{
	[CorpseName( "a wisp corpse" )]
	public class Wisp : BaseCreature
	{
		public override InhumanSpeech SpeechType{ get{ return InhumanSpeech.Wisp; } }

		public override Ethics.Ethic EthicAllegiance { get { return Ethics.Ethic.Hero; } }

		public override TimeSpan ReacquireDelay { get { return TimeSpan.FromSeconds( 1.0 ); } }

		[Constructable]
		public Wisp() : base( AIType.AI_Generic, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			Name = "a wisp";
			Body = 58;
			BaseSoundID = 466;

            SpellCastAnimation = 3;
            SpellCastFrameCount = 4;

			SetStr( 125, 130 );
			SetDex( 85, 90 );
			SetInt( 600, 800 );

			SetHits( 200, 225 );
            SetMana( 600, 800);

			SetDamage( 8, 16 );

            VirtualArmor = 5;
            
            SetSkill(SkillName.Tactics, 100.0, 100.0);

            SetSkill(SkillName.Meditation, 100, 100.0);
            SetSkill(SkillName.EvalInt, 80, 85.0);            
            SetSkill(SkillName.Magery, 80, 85.0);

            SetSkill(SkillName.MagicResist, 80, 85.0);

            SetSkill( SkillName.Wrestling, 75, 80.0 );

			Fame = 4000;
			Karma = 0;			

			AddItem( new LightSource() );
		}        

        public override void GenerateLoot()
        {
            base.PackAddGoldTier(5);
            base.PackAddArtifactChanceTier(5);
            base.PackAddMapChanceTier(5);

            PackItem(new BlackPearl(10));
        }

		public override OppositionGroup OppositionGroup
		{
			get{ return OppositionGroup.FeyAndUndead; }
		}

		public Wisp( Serial serial ) : base( serial )
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