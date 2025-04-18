using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "an ore elemental corpse" )]
	public class DullCopperElemental : BaseCreature
	{
		[Constructable]
		public DullCopperElemental() : this( 2 )
		{
		}

		[Constructable]
		public DullCopperElemental( int oreAmount ) : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a dull copper elemental";
			Body = 110;
			BaseSoundID = 268;

			SetStr( 226, 255 );
			SetDex( 126, 145 );
			SetInt( 71, 92 );

            SetHits(200, 225);

            SetDamage(9, 14);

            VirtualArmor = 35;

            
            
            

            SetSkill(SkillName.Tactics, 100.0, 100.0);

            SetSkill(SkillName.MagicResist, 75.0, 100.0);
            SetSkill(SkillName.Wrestling, 70.0, 75.0);

			Fame = 3500;
			Karma = -3500;			

			Item ore = new DullCopperOre( oreAmount );
			ore.ItemID = 0x19B9;
			PackItem( ore );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average );
			AddLoot( LootPack.Gems, 2 );
		}

		public override bool AutoDispel{ get{ return true; } }
		public override bool BleedImmune{ get{ return true; } }
		

		public DullCopperElemental( Serial serial ) : base( serial )
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