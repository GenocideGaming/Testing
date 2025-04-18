using System;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "a rat corpse" )]
	public class Rat : BaseCreature
	{
		[Constructable]
		public Rat() : base( AIType.AI_Generic, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			Name = "a rat";
			Body = 238;
			BaseSoundID = 0xCC;

			SetStr( 9 );
			SetDex( 35 );
			SetInt( 5 );

            SetHits(5, 10);
            SetMana(0);

            SetDamage(1, 3);

            VirtualArmor = 5;

            SetSkill(SkillName.Tactics, 100.0, 100.0);

            SetSkill(SkillName.MagicResist, 25.1, 30.0);
            SetSkill(SkillName.Wrestling, 20.0, 25.0);

			Fame = 150;
			Karma = -150;			

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 10.0;
		}

        public override OppositionGroup OppositionGroup
        {
            get { return OppositionGroup.DogsCatsAndRats; }
        }

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Poor );
		}

		public override int Meat{ get{ return 1; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Meat | FoodType.Fish | FoodType.Eggs | FoodType.GrainsAndHay; } }

		public Rat(Serial serial) : base(serial)
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