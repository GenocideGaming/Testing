using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a charred corpse" )]
	public class FireGargoyle : BaseCreature
	{
		[Constructable]
		public FireGargoyle() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = NameList.RandomName( "fire gargoyle" );
			Body = 130;
			BaseSoundID = 0x174;

			SetStr( 351, 400 );
			SetDex( 126, 145 );
			SetInt( 226, 250 );

			SetHits( 250, 300 );

			SetDamage( 7, 14 );

            VirtualArmor = 15;

            SetSkill(SkillName.Tactics, 100.0, 100.0);

            SetSkill(SkillName.EvalInt, 65.0, 70.0);
            SetSkill(SkillName.Meditation, 100.0, 100.0);
            SetSkill(SkillName.Magery, 65.0, 70.0);

            SetSkill(SkillName.MagicResist, 60.0, 65.0);

            SetSkill(SkillName.Wrestling, 65.0, 70.0);

			Fame = 3500;
			Karma = -3500;
		}

		public override void GenerateLoot()
		{
			
		}

		public override bool HasBreath{ get{ return true; } } // fire breath enabled
		
		public override int Meat{ get{ return 1; } }

		public FireGargoyle( Serial serial ) : base( serial )
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