using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a troll corpse" )]
	public class Troll : BaseCreature
	{
		[Constructable]
		public Troll () : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a troll";
			Body = Utility.RandomList( 53, 54 );
			BaseSoundID = 461;

            SetStr(150, 155);
            SetDex(45, 50);
            SetInt(20, 25);

            SetHits(175, 200);

            SetDamage(10, 20);

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
        }

		public override bool CanRummageCorpses{ get{ return true; } }
		
		public override int Meat{ get{ return 2; } }

		public Troll( Serial serial ) : base( serial )
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