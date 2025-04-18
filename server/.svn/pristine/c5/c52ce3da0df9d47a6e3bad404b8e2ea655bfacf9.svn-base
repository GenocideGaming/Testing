using System;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "a giant toad corpse" )]
	[TypeAlias( "Server.Mobiles.Gianttoad" )]
	public class GiantToad : BaseCreature
	{
		[Constructable]
		public GiantToad() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a giant toad";
			Body = 80;
			BaseSoundID = 0x26B;

			SetStr( 76, 100 );
			SetDex( 6, 25 );
			SetInt( 11, 20 );

            SetHits(25, 35);
			SetMana( 0 );

			SetDamage( 3, 5 );

            VirtualArmor = 5;
            
            SetSkill(SkillName.Tactics, 100.0, 100.0);

			SetSkill( SkillName.MagicResist, 25.1, 40.0 );			
			SetSkill( SkillName.Wrestling, 25.0, 30.0 );

			Fame = 750;
			Karma = -750;			

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 35.0;
		}

		public override void GenerateLoot()
		{			
		}

		public override int Hides{ get{ return 12; } }
		public override HideType HideType{ get{ return HideType.Spined; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Fish | FoodType.Meat; } }

		public GiantToad(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 1);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
			if ( version < 1 )
			{
					AI = AIType.AI_Melee;
					FightMode = FightMode.Closest;
			}
		}
	}
}