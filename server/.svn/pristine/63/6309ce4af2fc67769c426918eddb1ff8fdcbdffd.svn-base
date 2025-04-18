using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a reapers corpse" )]
	public class Reaper : BaseCreature
	{
		[Constructable]
		public Reaper() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a reaper";
			Body = 47;
			BaseSoundID = 442;
            
            Hue = Utility.RandomNeutralHue();            

            SpellCastAnimation = 11;
            SpellCastFrameCount = 5;

			SetStr( 145, 150 );
			SetDex( 35, 40 );
			SetInt( 500, 600 );

			SetHits( 275, 300 );			
            SetMana( 500, 600);

			SetDamage( 8, 16 );

            VirtualArmor = 15;

            SetSkill( SkillName.Tactics, 100.0, 100.0);

            SetSkill( SkillName.Meditation, 100.0, 100.0);
			SetSkill( SkillName.EvalInt, 65.0, 70.0 );
			SetSkill( SkillName.Magery, 65.0, 70.0 );

			SetSkill( SkillName.MagicResist, 65.0, 70.0 );	
		
			SetSkill( SkillName.Wrestling, 55.0, 60.0 );

			Fame = 3500;
			Karma = -3500;	
		}

        public override void GenerateLoot()
        {
            base.PackAddGoldTier(3);
            base.PackAddArtifactChanceTier(3);
            base.PackAddMapChanceTier(3);

            base.PackAddScrollTier(2);

            base.PackAddBoardTier(5);

            PackItem(new MandrakeRoot(15));
        }        	
		
		public override bool DisallowAllMoves{ get{ return true; } }

		public Reaper( Serial serial ) : base( serial )
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