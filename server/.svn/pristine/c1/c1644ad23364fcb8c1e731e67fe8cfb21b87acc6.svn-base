using System;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "a gorilla corpse" )]
	public class EncagedGorilla : BaseCreature
	{
		[Constructable]
		public EncagedGorilla() : base( AIType.AI_Generic, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			Name = "an encaged gorilla";
			Body = 0x1D;
			BaseSoundID = 0x9E;

			SetStr( 53, 95 );
			SetDex( 36, 55 );
			SetInt( 36, 60 );

			SetHits( 100, 120 );
			SetMana( 0 );

			SetDamage( 5, 10 );

            VirtualArmor = 5;

            SetSkill(SkillName.Tactics, 100.0, 100.0);

			SetSkill( SkillName.MagicResist, 45.1, 60.0 );			
			SetSkill( SkillName.Wrestling, 55, 60.0 );

			Fame = 450;
			Karma = 0;			

			Tamable = false;
		}

		public override int Meat{ get{ return 1; } }
		public override int Hides{ get{ return 6; } }
		public override FoodType FavoriteFood{ get{ return FoodType.FruitsAndVegies | FoodType.GrainsAndHay; } }

        public EncagedGorilla(Serial serial)
            : base(serial)
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