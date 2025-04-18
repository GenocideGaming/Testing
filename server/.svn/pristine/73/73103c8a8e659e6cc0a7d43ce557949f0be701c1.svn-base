using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "an ore elemental corpse" )]
	public class CopperElemental : BaseCreature
	{
		[Constructable]
		public CopperElemental() : this( 2 )
		{
		}

		[Constructable]
		public CopperElemental( int oreAmount ) : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a copper elemental";
			Body = 109;
			BaseSoundID = 268;

			SetStr( 226, 255 );
			SetDex( 126, 145 );
			SetInt( 71, 92 );

            SetHits(250, 275);

            SetDamage(11, 18);

            VirtualArmor = 45;

            
            
            

            SetSkill(SkillName.Tactics, 100.0, 100.0);

            SetSkill(SkillName.MagicResist, 75.0, 100.0);
            SetSkill(SkillName.Wrestling, 70.0, 75.0);

			Fame = 4800;
			Karma = -4800;			

			Item ore = new CopperOre( oreAmount );
			ore.ItemID = 0x19B9;
			PackItem( ore );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average );
			AddLoot( LootPack.Gems, 2 );
		}

		public override bool BleedImmune{ get{ return true; } }
		public override bool AutoDispel{ get{ return true; } }
		

		public override void CheckReflect( Mobile caster, ref bool reflect )
		{
			reflect = true; // Every spell is reflected back to the caster
		}

		public CopperElemental( Serial serial ) : base( serial )
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