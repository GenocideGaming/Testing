using System;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "a mountain goat corpse" )]
	public class MountainGoat : BaseCreature
	{
		[Constructable]
		public MountainGoat() : base( AIType.AI_Generic, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			Name = "a mountain goat";
            Body = 0xD1;
            BaseSoundID = 0x99;

			SetStr( 22, 64 );
			SetDex( 56, 75 );
			SetInt( 16, 30 );

            SetHits(40, 55);
			SetMana( 0 );

			SetDamage( 3, 6 );

            VirtualArmor = 5;

            SetSkill(SkillName.Tactics, 100.0, 100.0);

			SetSkill( SkillName.MagicResist, 25.1, 30.0 );			
			SetSkill( SkillName.Wrestling, 35.0, 40.0 );

			Fame = 300;
			Karma = 0;			

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 25.0;
		}

		public override int Meat{ get{ return 2; } }
		public override int Hides{ get{ return 12; } }
		public override FoodType FavoriteFood{ get{ return FoodType.GrainsAndHay | FoodType.FruitsAndVegies; } }

		public MountainGoat(Serial serial) : base(serial)
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