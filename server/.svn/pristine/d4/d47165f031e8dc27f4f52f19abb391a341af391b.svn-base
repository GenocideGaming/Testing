using System;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "a panther corpse" )]
	public class Panther : BaseCreature
	{
		[Constructable]
		public Panther() : base( AIType.AI_Generic, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			Name = "a panther";
			Body = 0xD6;
			Hue = 0x901;
			BaseSoundID = 0x462;

			SetStr( 61, 85 );
			SetDex( 86, 105 );
			SetInt( 26, 50 );

			SetHits( 65, 80 );
			SetMana( 0 );

			SetDamage( 5, 10 );

            SetSkill(SkillName.Tactics, 100.0, 100.0);

			SetSkill( SkillName.MagicResist, 15.1, 30.0 );			
			SetSkill( SkillName.Wrestling, 45.0, 50.0 );

			Fame = 450;
			Karma = 0;

			VirtualArmor = 10;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 50.0;
		}

		public override int Meat{ get{ return 1; } }
		public override int Hides{ get{ return 10; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Meat | FoodType.Fish; } }
		public override PackInstinct PackInstinct{ get{ return PackInstinct.Feline; } }

		public Panther(Serial serial) : base(serial)
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