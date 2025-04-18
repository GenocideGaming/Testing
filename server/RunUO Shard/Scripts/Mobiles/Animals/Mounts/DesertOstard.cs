using System;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "an ostard corpse" )]
	public class DesertOstard : BaseMount
	{
		[Constructable]
		public DesertOstard() : this( "a desert ostard" )
		{
		}

		[Constructable]
		public DesertOstard( string name ) : base( name, 0xD2, 0x3EA3, AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			BaseSoundID = 0x270;

			SetStr( 94, 170 );
			SetDex( 56, 75 );
			SetInt( 6, 10 );

            SetHits(100, 120);
            SetMana(0);

            SetDamage(5, 10);

            VirtualArmor = 10;

            
            
            

            SetSkill(SkillName.Tactics, 100.0, 100.0);

            SetSkill(SkillName.MagicResist, 27.1, 32.0);
            SetSkill(SkillName.Wrestling, 40.0, 45.0);

			Fame = 450;
			Karma = 0;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 29.1;
		}

		public override int Meat{ get{ return 3; } }
		public override FoodType FavoriteFood{ get{ return FoodType.FruitsAndVegies | FoodType.GrainsAndHay; } }
		public override PackInstinct PackInstinct{ get{ return PackInstinct.Ostard; } }

		public DesertOstard( Serial serial ) : base( serial )
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