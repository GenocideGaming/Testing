using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "an ice elemental corpse" )]
	public class IceElemental : BaseCreature
	{
		[Constructable]
		public IceElemental () : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "an ice elemental";
			Body = 161;
			BaseSoundID = 268;

			SetStr( 156, 185 );
			SetDex( 96, 115 );
			SetInt( 171, 192 );

			SetHits( 200, 250 );
            SetMana(650, 900);

			SetDamage( 7, 14 );

            VirtualArmor = 15;

            SetSkill(SkillName.Tactics, 100.0, 100.0);

			SetSkill( SkillName.EvalInt, 50.0, 60.0 );
			SetSkill( SkillName.Magery, 50.0, 60.0 );			
            SetSkill(SkillName.Meditation, 100.0, 100.0);

            SetSkill(SkillName.MagicResist, 30.1, 80.0);

            SetSkill(SkillName.Wrestling, 70.0, 75.0);

			Fame = 4000;
			Karma = -4000;
		}
		
		public override void GenerateLoot()
		{			
		}
		
		public override bool BleedImmune{ get{ return true; } }

		public IceElemental( Serial serial ) : base( serial )
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