using System;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a doppleganger corpse" )]
	public class Doppleganger : BaseCreature
	{
		[Constructable]
		public Doppleganger() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a doppleganger";
			Body = 0x309;
			BaseSoundID = 0x451;

			SetStr( 81, 110 );
			SetDex( 56, 75 );
			SetInt( 81, 105 );

			SetHits( 250, 300 );

            SetDamage(5, 10); //Uses Weapon

            VirtualArmor = 0;

            
            
            

            SetSkill(SkillName.Tactics, 100.0, 100.0); //Uses Weapon

			SetSkill( SkillName.MagicResist, 45.0, 50.0 );
			SetSkill( SkillName.Wrestling, 75.0, 80.0 );

			Fame = 1000;
			Karma = -1000;			
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average );
		}

		public override int Hides{ get{ return 6; } }
		public override int Meat{ get{ return 1; } }

		public Doppleganger( Serial serial ) : base( serial )
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