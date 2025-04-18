using System;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "an alligator corpse" )]
	public class Alligator : BaseCreature
	{
		[Constructable]
		public Alligator() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "an alligator";
			Body = 0xCA;
			BaseSoundID = 660;

			SetStr( 76, 100 );
			SetDex( 6, 25 );
			SetInt( 11, 20 );

            SetHits( 80, 100 );
			SetStam( 46, 65 );
			SetMana( 0 );

			SetDamage( 6, 12 );

            VirtualArmor = 20;

            SetSkill(SkillName.Tactics, 100.0, 100.0);

			SetSkill( SkillName.MagicResist, 25.0, 40.0 );			
			SetSkill( SkillName.Wrestling, 50.0, 55.0 );

			Fame = 600;
			Karma = -600;			

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 70.0;
		}

		public override int Meat{ get{ return 1; } }
		public override int Hides{ get{ return 12; } }
		public override HideType HideType{ get{ return HideType.Spined; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Meat | FoodType.Fish; } }

		public Alligator(Serial serial) : base(serial)
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

			if ( BaseSoundID == 0x5A )
				BaseSoundID = 660;
		}
	}
}