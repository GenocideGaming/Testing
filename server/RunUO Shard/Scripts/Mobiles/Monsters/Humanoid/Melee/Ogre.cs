using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "an ogre corpse" )]
	public class Ogre : BaseCreature
	{
		[Constructable]
		public Ogre () : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "an ogre";
			Body = 1;
			BaseSoundID = 427;

            SetStr(150, 155);
            SetDex(45, 50);
            SetInt(20, 25);

            SetHits(175, 200);

            SetDamage(9, 18);

            VirtualArmor = 5;

            SetSkill(SkillName.Tactics, 100.0, 100.0);

            SetSkill(SkillName.MagicResist, 25.0, 30.0);

            SetSkill(SkillName.Wrestling, 50.0, 55.0);

			Fame = 3000;
			Karma = -3000;			
		}
		
		public override void GenerateLoot()
		{
            base.PackAddGoldTier(3);
            base.PackAddArtifactChanceTier(3);
            base.PackAddMapChanceTier(3);

            base.PackAddGemTier(1);
			
			PackItem( new Club() );
		}

		public override bool CanRummageCorpses{ get{ return true; } }
		
		public override int Meat{ get{ return 2; } }

		public Ogre( Serial serial ) : base( serial )
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