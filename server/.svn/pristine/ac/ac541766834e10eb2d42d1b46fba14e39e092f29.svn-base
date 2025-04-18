using System;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "an inhuman corpse" )]
	public class Cursed : BaseCreature
	{
		public override bool ClickTitle{ get{ return false; } }
		public override bool ShowFameTitle{ get{ return false; } }

		[Constructable]
		public Cursed() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Title = "the Cursed";

			Hue = Utility.RandomMinMax( 0x8596, 0x8599 );
			Body = 0x190;
			Name = NameList.RandomName( "male" );
			BaseSoundID = 471;

			AddItem( new ShortPants( Utility.RandomNeutralHue() ) );
			AddItem( new Shirt( Utility.RandomNeutralHue() ) );

			BaseWeapon weapon = Loot.RandomWeapon();
			weapon.Movable = false;
			AddItem( weapon );

			SetStr( 91, 100 );
			SetDex( 86, 95 );
			SetInt( 61, 70 );

			SetHits( 175, 200 );

            SetDamage(1, 2); //Uses Weapon

            VirtualArmor = 0;

            
            
            

            SetSkill(SkillName.Tactics, 50.0, 50.0); //Uses Weapon
            
			SetSkill( SkillName.MagicResist, 53.5, 62.5 );			
			SetSkill( SkillName.Poisoning, 60.0, 82.5 );

            SetSkill(SkillName.Swords, 55.0, 60.0);
            SetSkill(SkillName.Fencing, 55.0, 60.0);
            SetSkill(SkillName.Macing, 55.0, 60.0);

			Fame = 1000;
			Karma = -2000;
		}

		public override int GetAttackSound()
		{
			return -1;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Meager );
			//AddLoot( LootPack.Miscellaneous );
		}

		public override bool AlwaysMurderer{ get{ return true; } }

		public Cursed( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}