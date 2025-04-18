using System;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "a jack rabbit corpse" )]
	[TypeAlias( "Server.Mobiles.Jackrabbit" )]
	public class JackRabbit : BaseCreature
	{
		[Constructable]
		public JackRabbit() : base( AIType.AI_Generic, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			Name = "a jack rabbit";
			Body = 0xCD;
			Hue = 0x1BB;

			SetStr( 15 );
			SetDex( 25 );
			SetInt( 5 );

            SetHits(5, 10);
            SetMana(0);

			SetDamage( 1, 2 );

            VirtualArmor = 5;

            SetSkill(SkillName.Tactics, 100.0, 100.0);

            SetSkill(SkillName.MagicResist, 25.1, 30.0);
            SetSkill(SkillName.Wrestling, 20.0, 25.0);

			Fame = 150;
			Karma = 0;			

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 5.0;
		}

		public override int Meat{ get{ return 1; } }
		public override int Hides{ get{ return 1; } }
		public override FoodType FavoriteFood{ get{ return FoodType.FruitsAndVegies; } }

		public JackRabbit(Serial serial) : base(serial)
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