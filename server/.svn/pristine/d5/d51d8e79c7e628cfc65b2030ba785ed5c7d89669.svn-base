using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a krakens corpse" )]
	public class Kraken : BaseCreature
	{
		[Constructable]
		public Kraken() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a kraken";
			Body = 77;
			BaseSoundID = 353;

			SetStr( 756, 780 );
			SetDex( 226, 245 );
			SetInt( 26, 40 );

			SetHits( 600, 800 );
			SetMana( 0 );

			SetDamage( 12, 24 );

            VirtualArmor = 20;

            
            
            

            SetSkill(SkillName.Tactics, 100.0, 100.0);

			SetSkill( SkillName.MagicResist, 35.0, 40.0 );
			SetSkill( SkillName.Wrestling, 55.0, 60.0 );

			Fame = 11000;
			Karma = -11000;			

			CanSwim = true;
			CantWalk = true;

			Rope rope = new Rope();
			rope.ItemID = 0x14F8;
			PackItem( rope );
			
			if( Utility.RandomDouble() < .05 )
				PackItem( new MessageInABottle() );
				
			PackItem( new SpecialFishingNet() ); //Confirm?
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich );
		}

		

		public Kraken( Serial serial ) : base( serial )
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
