using System;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "a goat corpse" )]
	public class Goat : BaseCreature
	{
		[Constructable]
		public Goat() : base( AIType.AI_Generic, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			Name = "a goat";
			Body = 0xD1;
			BaseSoundID = 0x99;

			SetStr( 19 );
			SetDex( 15 );
			SetInt( 5 );

            SetHits(35, 50);
			SetMana( 0 );

			SetDamage( 3, 6 );

            VirtualArmor = 5;

            SetSkill(SkillName.Tactics, 100.0, 100.0);

            SetSkill(SkillName.MagicResist, 9.0);
            SetSkill(SkillName.Wrestling, 30.0, 35.0);

			Fame = 150;
			Karma = 0;			

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 25.0;
		}

		public override int Meat{ get{ return 2; } }
		public override int Hides{ get{ return 8; } }
		public override FoodType FavoriteFood{ get{ return FoodType.GrainsAndHay | FoodType.FruitsAndVegies; } }

		public Goat(Serial serial) : base(serial)
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