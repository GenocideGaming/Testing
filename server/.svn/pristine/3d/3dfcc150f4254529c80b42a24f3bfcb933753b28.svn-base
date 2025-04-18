using System;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "a dog corpse" )]
	public class Dog : BaseCreature
	{
		[Constructable]
		public Dog() : base( AIType.AI_Generic, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			Name = "a dog";
			Body = 0xD9;
			Hue = Utility.RandomAnimalHue();
			BaseSoundID = 0x85;

			SetStr( 27, 37 );
			SetDex( 28, 43 );
			SetInt( 29, 37 );

            SetHits(30, 40);
            SetMana(0);

            SetDamage(3, 6);

            VirtualArmor = 10;

            SetSkill(SkillName.Tactics, 100.0, 100.0);

            SetSkill(SkillName.MagicResist, 20.1, 35.0);
            SetSkill(SkillName.Wrestling, 35.0, 40.0);

			Fame = 0;
			Karma = 300;			

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 15.0;
		}

        public override OppositionGroup OppositionGroup
        {
            get { return OppositionGroup.DogsCatsAndRats; }
        }

		public override int Meat{ get{ return 1; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Meat; } }
		public override PackInstinct PackInstinct{ get{ return PackInstinct.Canine; } }

		public Dog(Serial serial) : base(serial)
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