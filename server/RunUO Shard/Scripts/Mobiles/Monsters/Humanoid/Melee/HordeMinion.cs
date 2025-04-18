using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a horde minion corpse" )]
	public class HordeMinion : BaseCreature
	{
		[Constructable]
		public HordeMinion () : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a horde minion";
			Body = 776;
			BaseSoundID = 357;

			SetStr( 16, 40 );
			SetDex( 31, 60 );
			SetInt( 11, 25 );

			SetHits( 30, 40 );

			SetDamage( 4, 8 );

            VirtualArmor = 5;

            
            
            

            SetSkill(SkillName.Tactics, 100.0, 100.0);

            SetSkill(SkillName.MagicResist, 20.0, 25.0);
			SetSkill( SkillName.Wrestling, 25.0, 30.0 );

			Fame = 500;
			Karma = -500;			

			AddItem( new LightSource() );

			PackItem( new Bone( 3 ) );
			// TODO: Body parts
		}

		public override int GetIdleSound()
		{
			return 338;
		}

		public override int GetAngerSound()
		{
			return 338;
		}

		public override int GetDeathSound()
		{
			return 338;
		}

		public override int GetAttackSound()
		{
			return 406;
		}

		public override int GetHurtSound()
		{
			return 194;
		}

		public HordeMinion( Serial serial ) : base( serial )
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