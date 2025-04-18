using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a phoenix corpse" )]
	public class Phoenix : BaseCreature
	{
		[Constructable]
		public Phoenix() : base( AIType.AI_Generic, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			Name = "a phoenix";
			Body = 5;
			Hue = 0x674;
			BaseSoundID = 0x8F;

			SetStr( 504, 700 );
			SetDex( 202, 300 );
			SetInt( 504, 700 );

			SetHits( 700, 800 );

            SetDamage(15, 25);

            VirtualArmor = 5;

            SetSkill(SkillName.Tactics, 100.0, 100.0);

			SetSkill( SkillName.EvalInt, 90.0, 95.0 );
			SetSkill( SkillName.Magery, 90.0, 95 );
			SetSkill( SkillName.Meditation, 100.0, 100.0 );

			SetSkill( SkillName.MagicResist, 86.0, 135.0 );	
		
			SetSkill( SkillName.Wrestling, 85.0, 90.0 );

			Fame = 15000;
			Karma = 0;		
		}

		public override void GenerateLoot()
		{
		}

		public override int Meat{ get{ return 1; } }
		public override MeatType MeatType{ get{ return MeatType.Bird; } }
		public override int Feathers{ get{ return 36; } }

		public Phoenix( Serial serial ) : base( serial )
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