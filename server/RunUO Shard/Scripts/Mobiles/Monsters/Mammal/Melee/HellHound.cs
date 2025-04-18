using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a hell hound corpse" )]
	public class HellHound : BaseCreature
	{
		[Constructable]
		public HellHound() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a hell hound";
            Body = 225;
            Hue = 1157;
            BaseSoundID = 229;

			SetStr( 102, 150 );
			SetDex( 81, 105 );
			SetInt( 36, 60 );

			SetHits( 125, 150 );

			SetDamage( 8, 12 );

            VirtualArmor = 10;

            SetSkill(SkillName.Tactics, 100.0, 100.0);
            SetSkill(SkillName.Wrestling, 50.0, 55.0);

			Fame = 3400;
			Karma = -3400;			

			//Tamable = true;
			//ControlSlots = 1;
			//MinTameSkill = 85.5;

			PackItem( new SulfurousAsh( 5 ) );
		}

        

        public override void GenerateLoot()
        {
            
        }

		public override bool HasBreath{ get{ return true; } } // fire breath enabled
		public override int Meat{ get{ return 1; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Meat; } }
		public override PackInstinct PackInstinct{ get{ return PackInstinct.Canine; } }

		public HellHound( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}