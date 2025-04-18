using System;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "a ridgeback corpse" )]
	public class Ridgeback : BaseMount
	{
		[Constructable]
		public Ridgeback() : this( "a ridgeback" )
		{
		}

		[Constructable]
		public Ridgeback( string name ) : base( name, 187, 0x3EBA, AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			BaseSoundID = 0x3F3;

			SetStr( 58, 100 );
			SetDex( 56, 75 );
			SetInt( 16, 30 );

            SetHits(120, 140);
			SetMana( 0 );

			SetDamage( 5, 10 );

            VirtualArmor = 20;

            
            
            

            SetSkill(SkillName.Tactics, 100.0, 100.0);

			SetSkill( SkillName.MagicResist, 25.3, 40.0 );
            SetSkill(SkillName.Wrestling, 35.0, 40.0);

			Fame = 300;
			Karma = 0;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 83.1;
		}

		public override double GetControlChance( Mobile m, bool useBaseSkill )
		{
			return 1.0;
		}

		public override int Meat{ get{ return 1; } }
		public override int Hides{ get{ return 12; } }
		public override HideType HideType{ get{ return HideType.Spined; } }
		public override FoodType FavoriteFood{ get{ return FoodType.FruitsAndVegies | FoodType.GrainsAndHay; } }

		public Ridgeback( Serial serial ) : base( serial )
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