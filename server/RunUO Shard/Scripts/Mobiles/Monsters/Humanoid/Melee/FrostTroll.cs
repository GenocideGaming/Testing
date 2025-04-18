using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a frost troll corpse" )]
	public class FrostTroll : BaseCreature
	{
		[Constructable]
		public FrostTroll() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a frost troll";
			Body = 55;
			Hue = 1154;
			BaseSoundID = 461;

			SetStr( 227, 265 );
			SetDex( 66, 85 );
			SetInt( 46, 70 );

            SetHits(200, 225);

            SetDamage(8, 16);

            VirtualArmor = 15;

            
            
            

            SetSkill(SkillName.Tactics, 100.0, 100.0);

            SetSkill(SkillName.MagicResist, 20.0, 25.0);
            SetSkill(SkillName.Wrestling, 60.0, 65.0);

			Fame = 4000;
			Karma = -4000;            			

			AddItem( new DoubleAxe() ); 
		}

		 
		public override void GenerateLoot()
		{
			
		}

		public override int Meat{ get{ return 2; } }
		

		public FrostTroll( Serial serial ) : base( serial )
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