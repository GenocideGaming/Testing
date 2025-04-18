using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "an ore elemental corpse" )]
	public class ShadowIronElemental : BaseCreature
	{
		[Constructable]
		public ShadowIronElemental() : this( 2 )
		{
		}

		[Constructable]
		public ShadowIronElemental( int oreAmount ) : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a shadow iron elemental";
			Body = 111;
			BaseSoundID = 268;

			SetStr( 226, 255 );
			SetDex( 126, 145 );
			SetInt( 71, 92 );

            SetHits(225, 250);

            SetDamage(10, 16);

            VirtualArmor = 40;

            
            
            

            SetSkill(SkillName.Tactics, 100.0, 100.0);

            SetSkill(SkillName.MagicResist, 75.0, 100.0);
            SetSkill(SkillName.Wrestling, 70.0, 75.0);

			Fame = 4500;
			Karma = -4500;

			Item ore = new ShadowIronOre( oreAmount );
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
		
		public override Poison PoisonImmune{ get{ return Poison.Deadly; } }
		public override bool BreathImmune{ get{ return true; } }

		public override void AlterMeleeDamageFrom( Mobile from, ref int damage )
		{
			if ( from is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)from;

				if ( bc.Controlled || bc.BardTarget == this )
					damage = 0; // Immune to pets and provoked creatures
			}
		}

		public override void AlterDamageScalarFrom( Mobile caster, ref double scalar )
		{
			scalar = 0.0; // Immune to magic
		}
		
		public override void AlterSpellDamageFrom( Mobile from, ref int damage )
		{
			damage = 0;
		}

		public ShadowIronElemental( Serial serial ) : base( serial )
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