using System;
using System.Collections;
using Server.Items;
using Server.ContextMenus;
using Server.Misc;
using Server.Network;

namespace Server.Mobiles
{
	public class Brigand : BaseCreature
	{
		public override bool ClickTitle{ get{ return false; } }

		[Constructable]
		public Brigand() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			SpeechHue = Utility.RandomDyedHue();
			Title = "the brigand";
			Hue = Utility.RandomSkinHue();

			if ( this.Female = Utility.RandomBool() )
			{
				Body = 0x191;
				Name = NameList.RandomName( "female" );				
			}

			else
			{
				Body = 0x190;
				Name = NameList.RandomName( "male" );				
			}            

            Utility.AssignRandomHair( this );

			SetStr( 95, 100 );
			SetDex( 50, 55 );
			SetInt( 35, 40 );

            SetHits( 150, 175 );

			SetDamage( 7, 14 );

            VirtualArmor = 105;

            SetSkill( SkillName.Tactics, 100.0, 100.0);

			SetSkill( SkillName.MagicResist, 25.0, 30.0 );

            SetSkill( SkillName.Parry, 35.0, 35.0);

            SetSkill( SkillName.Hiding, 80.0, 85.0);
            SetSkill( SkillName.Stealth, 50.0, 55.0);

            SetSkill( SkillName.Macing, 60.0, 65.0);
            SetSkill( SkillName.Fencing, 60.0, 65.0);
            SetSkill( SkillName.Swords, 60.0, 65.0);
			SetSkill( SkillName.Wrestling, 60.0, 65.0 );

			Fame = 1000;
			Karma = -1000;

            AddItem( new ShortPants(Utility.RandomNeutralHue()));
			AddItem( new Boots( Utility.RandomNeutralHue() ) );

			switch ( Utility.Random( 10 ))
			{
                case 0:
                {
                    AddItem(new Longsword());
                    AddItem(new WoodenKiteShield());

                    break;
                }
                case 1:
                {
                    AddItem(new ShortSpear()); 
                    break;
                }

                case 2:
                {
                    AddItem(new Broadsword());
                    AddItem(new WoodenShield());
                    break;
                }

                case 3:
                {
                    AddItem(new Axe()); 
                    break;
                }

                case 4:
                {
                    AddItem(new Club());
                    AddItem(new WoodenKiteShield());
                    break;
                }

                case 5:
                {
                    AddItem(new WarHammer());
                    break;
                }

                case 6:
                {
                    AddItem(new Pitchfork()); 
                    break;
                }

                case 7:
                {
                    AddItem(new Bow());
                    AddItem(new Arrow(20)); 
                    break;
                }

                case 8:
                {
                    AddItem(new Crossbow()); 
                    AddItem(new CrossbowBolts(20)); 
                    break;
                }

                case 9:
                {
                    AddItem(new WarFork());
                    break;
                }
			}

            switch (Utility.Random(3))
            {
                case 0: AddItem(new LeatherChest()); break;
                case 1: AddItem(new StuddedChest()); break;
                case 2: AddItem(new BoneChest()); break;
                case 3: AddItem(new RingmailChest()); break;               
            }

            switch (Utility.Random(3))
            {
                case 0: AddItem(new LeatherGloves()); break;
                case 1: AddItem(new StuddedGloves()); break;
                case 2: AddItem(new BoneGloves()); break;
                case 3: AddItem(new RingmailGloves()); break;                    
            }

            switch (Utility.Random(4))
            {
                case 0: AddItem(new Bandana()); break;
                case 1: AddItem(new LeatherCap()); break;
                case 2: AddItem(new StrawHat()); break;
                case 3: AddItem(new FeatheredHat()); break;                
            }	
		}

        public override void GenerateLoot()
        {
            base.PackAddGoldTier(3);
            base.PackAddArtifactChanceTier(3);
            base.PackAddMapChanceTier(3);
            
            base.PackAddRandomPotionTier(1);
            base.PackAddBandageTier(1);
        }

		public override bool AlwaysMurderer{ get{ return true; } }

		public Brigand( Serial serial ) : base( serial )
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