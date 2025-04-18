using System; 
using System.Collections; 
using Server.Items; 
using Server.ContextMenus; 
using Server.Misc; 
using Server.Network; 

namespace Server.Mobiles 
{ 
	public class Executioner : BaseCreature 
	{ 
		[Constructable] 
		public Executioner() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 ) 
		{ 
			SpeechHue = Utility.RandomDyedHue(); 
			Title = "the executioner"; 
			Hue = Utility.RandomSkinHue(); 

			if ( this.Female = Utility.RandomBool() ) 
			{ 
				this.Body = 0x191; 
				this.Name = NameList.RandomName( "female" ); 
				
                AddItem( new Skirt( Utility.RandomRedHue() ) ); 
			} 

			else 
			{ 
				this.Body = 0x190; 
				this.Name = NameList.RandomName( "male" ); 
				
                AddItem( new ShortPants( Utility.RandomRedHue() ) ); 
			}

            Item Hood = new Hood();
            Hood.Movable = false;
            Hood.Hue = 1;
            AddItem(Hood);

            Item surcoat = new Surcoat();
            surcoat.Movable = false;
            surcoat.Hue = 1;
            AddItem(surcoat);

            Item thighBoots = new ThighBoots();
            thighBoots.Movable = false;
            thighBoots.Hue = 1;
            AddItem(thighBoots);

            Item studdedGloves = new StuddedGloves();
            studdedGloves.Movable = false;
            studdedGloves.Hue = 1;
            AddItem(studdedGloves);

            Item executionersAxe = new ExecutionersAxe();
            executionersAxe.Movable = false;
            executionersAxe.Hue = 1775;
            AddItem(executionersAxe);

			SetStr( 400, 405 );
			SetDex( 80, 85 );
			SetInt( 35, 40 );

            SetHits( 375, 400 );

			SetDamage( 20, 30 );

            VirtualArmor = 0;

            SetSkill(SkillName.Tactics, 100.0, 100.0);

			SetSkill( SkillName.MagicResist, 45.0, 50.0 );

            SetSkill( SkillName.Swords, 80.0, 85.0);   

			Fame = 5000;
			Karma = -5000;						

			Utility.AssignRandomHair( this );
		}       

        public override void GenerateLoot()
        {
            base.PackAddGoldTier(5);
            base.PackAddArtifactChanceTier(5);
            base.PackAddMapChanceTier(5);

            base.PackAddRandomPotionTier(3);
        }

		public override bool AlwaysMurderer{ get{ return true; } }

		public Executioner( Serial serial ) : base( serial ) 
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