using System;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "an enslaved gargoyle corpse" )]
	public class EnslavedGargoyle : BaseCreature
	{
		[Constructable]
		public EnslavedGargoyle() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "an enslaved gargoyle";
			Body = 0x2F1;
			BaseSoundID = 0x174;

			SetStr( 302, 360 );
			SetDex( 76, 95 );
			SetInt( 81, 105 );

            SetHits(225, 250);

            SetDamage(5, 10);

            VirtualArmor = 25;

            
            
            

            SetSkill(SkillName.Tactics, 100.0, 100.0);

            SetSkill(SkillName.MagicResist, 45.0, 50.0);
            SetSkill(SkillName.Wrestling, 60.0, 65.0);

			Fame = 3500;
			Karma = 0;		
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich );
			AddLoot( LootPack.Gems );
		}

		public override int Meat{ get{ return 1; } }
		

		public EnslavedGargoyle( Serial serial ) : base( serial )
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