using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "an eagle corpse" )]
	public class Eagle : BaseCreature
	{
		[Constructable]
		public Eagle() : base( AIType.AI_Generic, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			Name = "an eagle";
			Body = 5;
			BaseSoundID = 0x2EE;

			SetStr( 31, 47 );
			SetDex( 36, 60 );
			SetInt( 8, 20 );

			SetHits( 20, 30 );
			SetMana( 0 );

			SetDamage( 4, 8 );

            VirtualArmor = 5;

            SetSkill(SkillName.Tactics, 100.0, 100.0);

			SetSkill( SkillName.MagicResist, 15.3, 30.0 );			
			SetSkill( SkillName.Wrestling, 55.0, 60.0 );

			Fame = 300;
			Karma = 0;			

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 40.0;
		}

		public override int Meat{ get{ return 1; } }
		public override MeatType MeatType{ get{ return MeatType.Bird; } }
		public override int Feathers{ get{ return 36; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Meat | FoodType.Fish; } }

		public Eagle(Serial serial) : base(serial)
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