using System;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "a bull frog corpse" )]
	[TypeAlias( "Server.Mobiles.Bullfrog" )]
	public class BullFrog : BaseCreature
	{
		[Constructable]
		public BullFrog() : base( AIType.AI_Generic, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			Name = "a bull frog";
			Body = 81;
			Hue = Utility.RandomList( 0x5AC,0x5A3,0x59A,0x591,0x588,0x57F );
			BaseSoundID = 0x266;

			SetStr( 46, 70 );
			SetDex( 6, 25 );
			SetInt( 11, 20 );

            SetHits(10, 15);
			SetMana( 0 );

			SetDamage( 1, 2);

            VirtualArmor = 5;

            SetSkill(SkillName.Tactics, 100.0, 100.0);

			SetSkill( SkillName.MagicResist, 25.1, 40.0 );			
			SetSkill( SkillName.Wrestling, 20.0, 25.0 );

			Fame = 350;
			Karma = 0;			

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 35.0;
		}

		public override void GenerateLoot()
		{			
		}

		public override int Meat{ get{ return 1; } }
		public override int Hides{ get{ return 4; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Fish | FoodType.Meat; } }

		public BullFrog(Serial serial) : base(serial)
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