using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a solen infiltrator corpse" )] // TODO: Corpse name?
	public class BlackSolenInfiltratorQueen : BaseCreature
	{
		[Constructable]
		public BlackSolenInfiltratorQueen() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a black solen infiltrator";
			Body = 807;
			BaseSoundID = 959;
			Hue = 0x453;

			SetStr( 326, 350 );
			SetDex( 141, 165 );
			SetInt( 96, 120 );

			SetHits( 300, 350 );

			SetDamage( 15, 20 );

            VirtualArmor = 10;

            
            
            

            SetSkill(SkillName.Tactics, 100.0, 100.0);

			SetSkill( SkillName.MagicResist, 90.0 );			
			SetSkill( SkillName.Wrestling, 85.0 );

			Fame = 6500;
			Karma = -6500;

			SolenHelper.PackPicnicBasket( this );

			PackItem( new ZoogiFungus( ( 0.05 > Utility.RandomDouble() ) ? 16 : 4 ) );
		}

		public override int GetAngerSound()
		{
			return 0x259;
		}

		public override int GetIdleSound()
		{
			return 0x259;
		}

		public override int GetAttackSound()
		{
			return 0x195;
		}

		public override int GetHurtSound()
		{
			return 0x250;
		}

		public override int GetDeathSound()
		{
			return 0x25B;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich );
		}

		public override bool IsEnemy( Mobile m )
		{
			if ( SolenHelper.CheckBlackFriendship( m ) )
				return false;
			else
				return base.IsEnemy( m );
		}

		public override void OnDamage( int amount, Mobile from, bool willKill )
		{
			SolenHelper.OnBlackDamage( from );

			base.OnDamage( amount, from, willKill );
		}

		public BlackSolenInfiltratorQueen( Serial serial ) : base( serial )
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