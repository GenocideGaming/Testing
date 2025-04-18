using System;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "a pig corpse" )]
	public class Pig : BaseCreature
	{
		[Constructable]
		public Pig() : base( AIType.AI_Generic, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			Name = "a pig";
			Body = 0xCB;
			BaseSoundID = 0xC4;

			SetStr( 20 );
			SetDex( 20 );
			SetInt( 5 );

            SetHits(35, 55);
            SetMana(0);

			SetDamage( 3, 5 );

            VirtualArmor = 5;
            
            SetSkill(SkillName.Tactics, 100.0, 100.0);

			SetSkill( SkillName.MagicResist, 5.0 );
            SetSkill(SkillName.Wrestling, 30.0, 35.0);

			Fame = 150;
			Karma = 0;			

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 20.0;
		}

		public override int Meat{ get{ return 1; } }
		public override FoodType FavoriteFood{ get{ return FoodType.FruitsAndVegies | FoodType.GrainsAndHay; } }

		public Pig(Serial serial) : base(serial)
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