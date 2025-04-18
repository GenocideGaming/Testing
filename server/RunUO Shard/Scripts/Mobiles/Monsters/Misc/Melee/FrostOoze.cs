using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a frost ooze corpse" )]
	public class FrostOoze : BaseCreature
	{
		[Constructable]
		public FrostOoze() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a frost ooze";
			Body = 94;
			BaseSoundID = 456;

			SetStr( 18, 30 );
			SetDex( 16, 21 );
			SetInt( 16, 20 );

            SetHits(20, 30);

            SetDamage(3, 5);

            VirtualArmor = 5;

            
            
            

            SetSkill(SkillName.Tactics, 100.0, 100.0);

            SetSkill(SkillName.Poisoning, 30.0, 50.0);
            SetSkill(SkillName.MagicResist, 15.0, 20.0);
            SetSkill(SkillName.Wrestling, 15.0, 20.0);

			Fame = 450;
			Karma = -450;			
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Gems, Utility.RandomMinMax( 1, 2 ) );
		}

		public FrostOoze( Serial serial ) : base( serial )
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