using System;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "a snake corpse" )]
	public class Snake : BaseCreature
	{
		[Constructable]
		public Snake() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a snake";
			Body = 52;
			Hue = Utility.RandomSnakeHue();
			BaseSoundID = 0xDB;

			SetStr( 22, 34 );
			SetDex( 16, 25 );
			SetInt( 6, 10 );

			SetHits( 20, 30 );
			SetMana( 0 );

			SetDamage( 3, 5 );

            VirtualArmor = 20;

            SetSkill(SkillName.Tactics, 100.0, 100.0);

			SetSkill( SkillName.MagicResist, 25.0, 30.0 );	
		
			SetSkill( SkillName.Wrestling, 25.0, 30.0 );

			Fame = 300;
			Karma = -300;		

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 40.0;
		}
		
		public override void GenerateLoot()
		{			
		}

		public override Poison PoisonImmune{ get{ return Poison.Regular; } }
		public override Poison HitPoison{ get{ return Poison.Lesser; } }

		public override bool DeathAdderCharmable{ get{ return true; } }

		public override int Meat{ get{ return 1; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Eggs; } }

		public Snake(Serial serial) : base(serial)
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