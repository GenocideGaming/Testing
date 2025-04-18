using System; 
using Server;
using Server.Items;

namespace Server.Mobiles 
{ 
	[CorpseName( "an evil mage lord corpse" )] 
	public class EvilMageLord : BaseCreature 
	{ 
		[Constructable] 
		public EvilMageLord() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 ) 
		{ 
			Name = NameList.RandomName( "evil mage lord" );
			
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

            SetHits(250, 275);
            SetMana(500, 600);

            SetDamage(4, 8);

            SetSkill(SkillName.Tactics, 100.0, 100.0);

            SetSkill(SkillName.EvalInt, 85.0, 90.0);
            SetSkill(SkillName.Magery, 85.0, 90.0);
            SetSkill(SkillName.Meditation, 100.0, 100.0);

            SetSkill(SkillName.MagicResist, 85.0, 90.0);

            SetSkill(SkillName.Wrestling, 70.0, 75.0);

            Fame = 10500;
            Karma = -10500;

            VirtualArmor = 0;
				
	        this.AddItem( new Robe( Utility.RandomMetalHue() ) ); 
			AddItem( new WizardsHat( Utility.RandomMetalHue() ) ); 

			if ( Utility.RandomBool() )
				AddItem( new Shoes() );
			else
				AddItem( new Sandals() );
		}

		public override void GenerateLoot()
		{
            base.PackAddGoldTier(6);
            base.PackAddArtifactChanceTier(6);
            base.PackAddMapChanceTier(6);

            base.PackAddScrollTier(4);
            base.PackAddReagentTier(3);

            if (Utility.Random(100) == 7)
                PackItem(new DecoCrystalBall());	
		}

		public override bool CanRummageCorpses{ get{ return true; } }
		public override bool AlwaysMurderer{ get{ return true; } }
		public override int Meat{ get{ return 1; } }		

		public EvilMageLord( Serial serial ) : base( serial ) 
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