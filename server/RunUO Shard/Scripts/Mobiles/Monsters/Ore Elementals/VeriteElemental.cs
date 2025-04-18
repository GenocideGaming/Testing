using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "an ore elemental corpse" )]
	public class VeriteElemental : BaseCreature
	{
		[Constructable]
		public VeriteElemental() : this( 2 )
		{
		}

		[Constructable]
		public VeriteElemental( int oreAmount ) : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a verite elemental";
			Body = 113;
			BaseSoundID = 268;

			SetStr( 226, 255 );
			SetDex( 126, 145 );
			SetInt( 71, 92 );

            SetHits(325, 350);

            SetDamage(14, 24);

            VirtualArmor = 60;

            
            
            

            SetSkill(SkillName.Tactics, 100.0, 100.0);

            SetSkill(SkillName.MagicResist, 75.0, 100.0);
            SetSkill(SkillName.Wrestling, 70.0, 75.0);

			Fame = 3500;
			Karma = -3500;			

			Item ore = new VeriteOre( oreAmount );
			ore.ItemID = 0x19B9;
			PackItem( ore );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich );
			AddLoot( LootPack.Gems, 2 );
		}

		public override bool AutoDispel{ get{ return true; } }
		public override bool BleedImmune{ get{ return true; } }
		

		public VeriteElemental( Serial serial ) : base( serial )
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