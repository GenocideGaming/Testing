using System;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "a hare corpse" )]
	public class EncagedRabbit : BaseCreature
	{
		[Constructable]
		public EncagedRabbit() : base( AIType.AI_Generic, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			Name = "an encaged rabbit";
			Body = 205;

			if ( 0.5 >= Utility.RandomDouble() )
				Hue = Utility.RandomAnimalHue();

			SetStr( 6, 10 );
			SetDex( 26, 38 );
			SetInt( 6, 14 );

            SetHits(5, 10);
            SetMana(0);

            SetDamage(1, 2);

            VirtualArmor = 5;
            
            SetSkill(SkillName.Tactics, 100.0, 100.0);

            SetSkill(SkillName.MagicResist, 25.1, 30.0);
            SetSkill(SkillName.Wrestling, 20.0, 25.0);

			Fame = 150;
			Karma = 0;			

			Tamable = false;
		}

		public override int Meat{ get{ return 1; } }
		public override int Hides{ get{ return 1; } }
		public override FoodType FavoriteFood{ get{ return FoodType.FruitsAndVegies; } }

        public EncagedRabbit(Serial serial)
            : base(serial)
		{
		}

		public override int GetAttackSound() 
		{ 
			return 0xC9; 
		} 

		public override int GetHurtSound() 
		{ 
			return 0xCA; 
		} 

		public override int GetDeathSound() 
		{ 
			return 0xCB; 
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