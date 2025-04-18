using System;
using System.Collections;
using Server.Misc;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a ratman archer corpse" )]
	public class RatmanArcher : BaseCreature
	{
		public override InhumanSpeech SpeechType{ get{ return InhumanSpeech.Ratman; } }

		[Constructable]
		public RatmanArcher() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = NameList.RandomName( "ratman" );
			Body = 0x8E;
			BaseSoundID = 437;

			SetStr( 146, 180 );
			SetDex( 101, 130 );
			SetInt( 116, 140 );

			SetHits( 75, 100 );

			SetDamage( 1, 2 );

            VirtualArmor = 10;

            
            
            

            SetSkill(SkillName.Tactics, 25.0,25.0); //Reduced Damage for Weapon
			
			SetSkill( SkillName.MagicResist, 15.0, 25.0 );
            SetSkill( SkillName.Archery, 50.0, 55.0);
			SetSkill( SkillName.Wrestling, 50.0, 55.0 );

			Fame = 6500;
			Karma = -6500;			

		}

		 
		
		public override void GenerateLoot()
		{
			AddItem( new Bow() );
			PackItem( new Arrow( Utility.RandomMinMax( 50, 75 ) ) );
			
		}
		
		public override bool CanRummageCorpses{ get{ return true; } }
		public override int Hides{ get{ return 8; } }
		public override HideType HideType{ get{ return HideType.Spined; } }

		public RatmanArcher( Serial serial ) : base( serial )
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
				Body = 0x8E;
				Hue = 0;
			}
		}
	}
}
