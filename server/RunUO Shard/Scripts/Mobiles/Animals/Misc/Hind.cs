using System;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "a deer corpse" )]
	public class Hind : BaseCreature
	{
		[Constructable]
		public Hind() : base( AIType.AI_Generic, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			Name = "a hind";
			Body = 0xED;

			SetStr( 21, 51 );
			SetDex( 47, 77 );
			SetInt( 17, 47 );

            SetHits(40, 50);
			SetMana( 0 );

            SetDamage(3, 5);

            VirtualArmor = 10;
            
            SetSkill(SkillName.Tactics, 100.0, 100.0);

			SetSkill( SkillName.MagicResist, 15.0 );
            SetSkill(SkillName.Wrestling, 30.0, 35.0);

			Fame = 300;
			Karma = 0;			

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 35.0;
		}

		public override int Meat{ get{ return 5; } }
		public override int Hides{ get{ return 8; } }
		public override FoodType FavoriteFood{ get{ return FoodType.FruitsAndVegies | FoodType.GrainsAndHay; } }

		public Hind(Serial serial) : base(serial)
		{
		}

		public override int GetAttackSound() 
		{ 
			return 0x82; 
		} 

		public override int GetHurtSound() 
		{ 
			return 0x83; 
		} 

		public override int GetDeathSound() 
		{ 
			return 0x84; 
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