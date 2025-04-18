using System;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "a dire wolf corpse" )]
	[TypeAlias( "Server.Mobiles.Direwolf" )]
	public class EncagedDireWolf : BaseCreature
	{
		[Constructable]
		public EncagedDireWolf() : base( AIType.AI_Generic,FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "an encaged dire wolf";
			Body = 23;
			BaseSoundID = 0xE5;

			SetStr( 96, 120 );
			SetDex( 81, 105 );
			SetInt( 36, 60 );

			SetHits( 90, 120 );
			SetMana( 0 );

			SetDamage( 6, 12 );

            VirtualArmor = 10;
            
            SetSkill(SkillName.Tactics, 100.0, 100.0);

			SetSkill( SkillName.MagicResist, 20.0, 25.0 );			
			SetSkill( SkillName.Wrestling, 50.0, 55.0 );

			Fame = 2500;
			Karma = -2500;		

			Tamable = false;
		}

		public override int Meat{ get{ return 1; } }
		public override int Hides{ get{ return 7; } }
		public override HideType HideType{ get{ return HideType.Spined; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Meat; } }
		public override PackInstinct PackInstinct{ get{ return PackInstinct.Canine; } }

        public EncagedDireWolf(Serial serial)
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