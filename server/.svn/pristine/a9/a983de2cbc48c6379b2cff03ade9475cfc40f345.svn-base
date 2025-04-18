using System;
using Server;
using Server.Misc;
using Server.Items;

namespace Server.Mobiles 
{ 
	[CorpseName( "an evil mage corpse" )] 
	public class EvilMage : BaseCreature 
	{ 
		[Constructable] 
		public EvilMage() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 ) 
		{ 
			Name = NameList.RandomName( "evil mage" );
			Title = "the evil mage";
			
			if ( this.Female = Utility.RandomBool() )
			{
				Body = 0x191;
				Name = NameList.RandomName( "female" );
				AddItem( new Skirt( Utility.RandomNeutralHue() ) );
			}

			else
			{
				Body = 0x190;
				Name = NameList.RandomName( "male" );
				AddItem( new ShortPants( Utility.RandomNeutralHue() ) );
			}

            SetStr(95, 100);
            SetDex(40, 45);
            SetInt(500, 600);

            SetHits(175, 200);
            SetMana(500, 600);

            SetDamage(4, 8);

            SetSkill(SkillName.Tactics, 100.0, 100.0);

            SetSkill(SkillName.EvalInt, 75.0, 80.0);
            SetSkill(SkillName.Magery, 75.0, 80.0);
            SetSkill(SkillName.Meditation, 100.0, 100.0);

            SetSkill(SkillName.MagicResist, 75.0, 80.0);

            SetSkill(SkillName.Wrestling, 60.0, 65.0);

            Fame = 2500;
            Karma = -2500;
			
			AddItem( new Robe( Utility.RandomNeutralHue() ) );
			AddItem( new Sandals() );
		}		

		public override void GenerateLoot()
		{
            base.PackAddGoldTier(4);
            base.PackAddArtifactChanceTier(4);
            base.PackAddMapChanceTier(4);

            base.PackAddScrollTier(2);
            base.PackAddReagentTier(2);
		}

		public override bool CanRummageCorpses{ get{ return true; } }
		public override bool AlwaysMurderer{ get{ return true; } }
		public override int Meat{ get{ return 1; } }		

		public EvilMage( Serial serial ) : base( serial )
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