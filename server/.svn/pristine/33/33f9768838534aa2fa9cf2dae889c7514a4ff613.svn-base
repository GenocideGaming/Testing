using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a headless corpse" )]
	public class HeadlessOne : BaseCreature
	{
		[Constructable]
		public HeadlessOne() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a headless one";
			Body = 31;
			Hue = Utility.RandomSkinHue() & 0x7FFF;
			BaseSoundID = 0x39D;

			SetStr( 26, 50 );
			SetDex( 36, 55 );
			SetInt( 15, 20 );

			SetHits( 50, 60 );

			SetDamage( 3, 6 );

            VirtualArmor = 5;

            SetSkill(SkillName.Tactics, 100.0, 100.0);

			SetSkill( SkillName.MagicResist, 10.0, 15.0 );

			SetSkill( SkillName.Wrestling, 10.0, 15.0 );

			Fame = 450;
			Karma = -450;			
		}        

        public override void GenerateLoot()
        {
            base.PackAddGoldTier(1);
            base.PackAddArtifactChanceTier(1);
            base.PackAddMapChanceTier(1);
        }

		public override bool CanRummageCorpses{ get{ return true; } }
		public override int Meat{ get{ return 1; } }

		public HeadlessOne( Serial serial ) : base( serial )
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