using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a gazer larva corpse" )]
	public class GazerLarva : BaseCreature
	{
		[Constructable]
		public GazerLarva () : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a gazer larva";
			Body = 778;
			BaseSoundID = 377;

			SetStr( 76, 100 );
			SetDex( 51, 75 );
			SetInt( 56, 80 );

			SetHits( 75, 100 );

			SetDamage( 5, 10 );

            VirtualArmor = 5;

            
            
            

            SetSkill(SkillName.Tactics, 100.0, 100.0);

            SetSkill(SkillName.MagicResist, 50.0, 55.0);
            SetSkill(SkillName.Wrestling, 45.0, 50.0);

			Fame = 900;
			Karma = -900;			

			PackItem( new Nightshade( Utility.RandomMinMax( 2, 3 ) ) );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Poor );
		}

		public override int Meat{ get{ return 1; } }

		public GazerLarva( Serial serial ) : base( serial )
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