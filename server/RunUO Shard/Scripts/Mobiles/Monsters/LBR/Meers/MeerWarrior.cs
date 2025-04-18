using System;
using System.Collections;
using Server;
using Server.Misc;
using Server.Items;
using Server.Spells;

namespace Server.Mobiles
{
	[CorpseName( "a meer corpse" )]
	public class MeerWarrior : BaseCreature
	{
		[Constructable]
		public MeerWarrior() : base( AIType.AI_Generic, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			Name = "a meer warrior";
			Body = 771;

			SetStr( 86, 100 );
			SetDex( 186, 200 );
			SetInt( 86, 100 );

			SetHits( 100, 125 );

			SetDamage( 8, 16 );

            VirtualArmor = 15;

            
            
            

            SetSkill(SkillName.Tactics, 100.0, 100.0);

			SetSkill( SkillName.MagicResist, 50.0, 55.0 );
			SetSkill( SkillName.Wrestling, 75.0, 80.0 );			

			Fame = 2000;
			Karma = 5000;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Meager );
		}

		public override bool BardImmune{ get{ return !Core.AOS; } }
		public override bool CanRummageCorpses{ get{ return true; } }

		public override bool InitialInnocent{ get{ return true; } }

		public override void OnDamage( int amount, Mobile from, bool willKill )
		{
			if ( from != null && !willKill && amount > 3 && from != null && !InRange( from, 7 ) )
			{
				this.MovingEffect( from, 0xF51, 10, 0, false, false );
				SpellHelper.Damage( TimeSpan.FromSeconds( 1.0 ), from, this, Utility.RandomMinMax( 30, 40 ) - (Core.AOS ? 0 : 10), true );
			}

			base.OnDamage( amount, from, willKill );
		}

		public override int GetHurtSound()
		{
			return 0x156;
		}

		public override int GetDeathSound()
		{
			return 0x15C;
		}

		public MeerWarrior( Serial serial ) : base( serial )
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