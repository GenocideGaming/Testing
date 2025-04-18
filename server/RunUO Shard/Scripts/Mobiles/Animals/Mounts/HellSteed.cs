using System;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "a hellsteed corpse" )]
	public class HellSteed : BaseMount
	{
		[Constructable] 
		public HellSteed() : this( "a hellsteed" )
		{
		}

		[Constructable]
		public HellSteed( string name ) : base( name, 793, 0x3EBB, AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			SetStr( 201, 210 );
			SetDex( 101, 110 );
			SetInt( 101, 115 );

            SetHits(300, 350);

			SetDamage( 10, 18 );

            VirtualArmor = 5;

            
            
            

            SetSkill(SkillName.Tactics, 100.0, 100.0);

			SetSkill( SkillName.MagicResist, 90.1, 110.0 );			
			SetSkill( SkillName.Wrestling, 55.0, 60.0 );

			Fame = 0;
			Karma = 0;
		}

		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		// TODO: "This creature can breath chaos."

		public HellSteed( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}