using System;
using Server;
using Server.Misc;
using Server.Items;
using Server.Factions;

namespace Server.Mobiles
{
	[CorpseName( "a wisp corpse" )]
	public class DarkWisp : BaseCreature
	{
		public override InhumanSpeech SpeechType{ get{ return InhumanSpeech.Wisp; } }

		public override Ethics.Ethic EthicAllegiance { get { return Ethics.Ethic.Evil; } }

		public override TimeSpan ReacquireDelay { get { return TimeSpan.FromSeconds( 1.0 ); } }

		[Constructable]
		public DarkWisp() : base( AIType.AI_Generic, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			Name = "a wisp";
			Body = 165;
			BaseSoundID = 466;

			SetStr( 196, 225 );
			SetDex( 196, 225 );
			SetInt( 196, 225 );

            SetHits(200, 225);

            SetDamage(8, 16);

            VirtualArmor = 5;

            
            
            

            SetSkill(SkillName.Tactics, 100.0, 100.0);

            SetSkill(SkillName.EvalInt, 80, 85.0);
            SetSkill(SkillName.Magery, 80, 85.0);
            SetSkill(SkillName.MagicResist, 80, 85.0);
            SetSkill(SkillName.Wrestling, 85, 90.0);

			Fame = 4000;
			Karma = -4000;		

			AddItem( new LightSource() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich );
			AddLoot( LootPack.Average );
		}

		public override OppositionGroup OppositionGroup
		{
			get{ return OppositionGroup.FeyAndUndead; }
		}

		public DarkWisp( Serial serial )
			: base( serial )
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