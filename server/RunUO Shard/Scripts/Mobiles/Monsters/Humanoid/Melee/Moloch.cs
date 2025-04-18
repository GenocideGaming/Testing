using System;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a moloch corpse" )]
	public class Moloch : BaseCreature
	{
		public override WeaponAbility GetWeaponAbility()
		{
			return WeaponAbility.ConcussionBlow;
		}

		[Constructable]
		public Moloch() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a moloch";
			Body = 0x311;
			BaseSoundID = 0x300;

			SetStr( 331, 360 );
			SetDex( 66, 85 );
			SetInt( 41, 65 );

			SetHits( 350, 400 );

			SetDamage( 12, 18 );

            VirtualArmor = 10;

            
            
            

            SetSkill(SkillName.Tactics, 100.0, 100.0);

			SetSkill( SkillName.MagicResist, 60.0, 65.0 );
			SetSkill( SkillName.Wrestling, 65.0, 70.0 );

			Fame = 7500;
			Karma = -7500;			
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich );
		}

		public override Poison PoisonImmune{ get{ return Poison.Regular; } }

		public Moloch( Serial serial ) : base( serial )
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