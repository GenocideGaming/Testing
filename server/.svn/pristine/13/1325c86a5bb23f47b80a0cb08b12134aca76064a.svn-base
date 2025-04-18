using System;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "a llama corpse" )]
	public class Llama : BaseCreature
	{
		[Constructable]
		public Llama() : base( AIType.AI_Generic, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			Name = "a llama";
			Body = 0xDC;
			BaseSoundID = 0x3F3;

			SetStr( 21, 49 );
			SetDex( 36, 55 );
			SetInt( 16, 30 );

			SetHits( 80, 100 );
			SetMana( 0 );

			SetDamage( 4, 6 );

            VirtualArmor = 5;

            SetSkill(SkillName.Tactics, 100.0, 100.0);

			SetSkill( SkillName.MagicResist, 15.1, 20.0 );			
			SetSkill( SkillName.Wrestling, 30.0, 35.0 );

			Fame = 300;
			Karma = 0;			

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 30.0;
		}

		public override int Meat{ get{ return 1; } }
		public override int Hides{ get{ return 12; } }
		public override FoodType FavoriteFood{ get{ return FoodType.FruitsAndVegies | FoodType.GrainsAndHay; } }

		public Llama(Serial serial) : base(serial)
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