using System;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "a timber wolf corpse" )]
	[TypeAlias( "Server.Mobiles.Timberwolf" )]
	public class TimberWolf : BaseCreature
	{
		[Constructable]
		public TimberWolf() : base( AIType.AI_Generic, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			Name = "a timber wolf";
			Body = 225;
			BaseSoundID = 0xE5;

			SetStr( 56, 80 );
			SetDex( 56, 75 );
			SetInt( 11, 25 );

			SetHits( 80, 100 );
			SetMana( 0 );

			SetDamage( 5, 10 );

            VirtualArmor = 10;

            SetSkill(SkillName.Tactics, 100.0, 100.0);

			SetSkill( SkillName.MagicResist, 27.6, 45.0 );			
			SetSkill( SkillName.Wrestling, 45.0, 50.0 );

			Fame = 450;
			Karma = 0;			

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 50.0;
		}

		public override int Meat{ get{ return 1; } }
		public override int Hides{ get{ return 5; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Meat; } }
		public override PackInstinct PackInstinct{ get{ return PackInstinct.Canine; } }

		public TimberWolf(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}