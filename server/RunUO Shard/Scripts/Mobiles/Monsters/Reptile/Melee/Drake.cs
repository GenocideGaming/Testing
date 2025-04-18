using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a drake corpse" )]
	public class Drake : BaseCreature
	{
		[Constructable]
		public Drake () : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a drake";
			Body = Utility.RandomList( 60, 61 );
			BaseSoundID = 362;            

            SetStr(450, 500);
            SetDex(70, 75);
            SetInt(25, 30);

            SetHits(500, 600);

            SetDamage(15, 22);

            VirtualArmor = 20;

            SetSkill( SkillName.Tactics, 100.0, 100.0);

            SetSkill( SkillName.MagicResist, 45.0, 50.0);

            SetSkill( SkillName.Wrestling, 70.0, 75.0);

			Fame = 5500;
			Karma = -5500;			

			Tamable = true;
			ControlSlots = 2;
			MinTameSkill = 85.0;
		}
		
		public override void GenerateLoot()
		{
            base.PackAddGoldTier(6);
            base.PackAddArtifactChanceTier(6);
            base.PackAddMapChanceTier(6);

            base.PackAddGemTier(1);

            PackItem(new Bone());
            PackItem(new Bone()); 
		}

		public override bool ReacquireOnMovement{ get{ return true; } }
		public override bool HasBreath{ get{ return true; } } // fire breath enabled
		
		public override int Meat{ get{ return 10; } }
		public override int Hides{ get{ return 20; } }
		public override HideType HideType{ get{ return HideType.Horned; } }
		public override int Scales{ get{ return 2; } }
		public override ScaleType ScaleType{ get{ return ( Body == 60 ? ScaleType.Yellow : ScaleType.Red ); } }
		public override FoodType FavoriteFood{ get{ return FoodType.Meat | FoodType.Fish; } }

		public Drake( Serial serial ) : base( serial )
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