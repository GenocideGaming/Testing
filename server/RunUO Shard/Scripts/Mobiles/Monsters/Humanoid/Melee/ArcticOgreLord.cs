using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a frozen ogre lord's corpse" )]
	[TypeAlias( "Server.Mobiles.ArticOgreLord" )]
	public class ArcticOgreLord : BaseCreature
	{
		[Constructable]
		public ArcticOgreLord() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "an arctic ogre lord";
            Body = Utility.RandomList(161, 162);
            Hue = 1154;
            BaseSoundID = 427;

            SetStr(767, 945);
            SetDex(66, 75);
            SetInt(46, 70);

            SetHits(500, 600);

            SetDamage(16, 22);

            VirtualArmor = 5;

            
            
            

            SetSkill(SkillName.Tactics, 100.0, 100.0);

            SetSkill(SkillName.MagicResist, 15.0, 20.0);
            SetSkill(SkillName.Wrestling, 50.0, 55.0);

			Fame = 15000;
			Karma = -15000;			

			
		}

		 
		
		public override void GenerateLoot()
		{
			PackItem( new Club() );
			
			if ( Utility.Random(1000) == 7 )
			    PackItem( new SnowPile() );
			
			
		}

		public override Poison PoisonImmune{ get{ return Poison.Regular; } }
		

		public ArcticOgreLord( Serial serial ) : base( serial )
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