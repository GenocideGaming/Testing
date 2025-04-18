using System;
using System.Collections.Generic;
using System.Text;
using Server.Items;

namespace Server.Mobiles
{
    public class Pirate : BaseCreature
    {
        	public override bool ClickTitle{ get{ return false; } }

		[Constructable]
		public Pirate() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			SpeechHue = Utility.RandomDyedHue();
			Title = Utility.Random(2) == 0 ? "the pirate" : "the swashbuckler";
			Hue = Utility.RandomSkinHue();

			if ( this.Female = Utility.RandomBool() )
			{
				Body = 0x191;
				Name = NameList.RandomName( "female" );
                AddItem(new ShortPants(Utility.RandomNeutralHue()));
			}

			else
			{
				Body = 0x190;
				Name = NameList.RandomName( "male" );
				AddItem( new ShortPants( Utility.RandomNeutralHue() ) );
			}

            switch (Utility.Random(2))
            {
                case 0: AddItem(new ThighBoots(Utility.RandomNeutralHue())); break;                 
            }

            switch (Utility.Random(4))
            {
                case 0: AddItem(new FancyShirt(Utility.RandomDyedHue())); break;
                case 1: AddItem(new Shirt(Utility.RandomDyedHue())); break;
                case 2: AddItem(new BodySash()); break;
            }

            switch (Utility.Random(4))
            {
                case 0: AddItem(new TricorneHat()); break;
                case 1: AddItem(new SkullCap()); break;
                case 2: AddItem(new Bandana()); break;
            }

			SetStr( 125, 130 );
			SetDex( 50, 55 );
			SetInt( 30, 35 );

            SetHits(125, 150);

            SetDamage(10, 14); //Uses Weapon

            VirtualArmor = 5;

            SetSkill(SkillName.Tactics, 100.0, 100.0); //Uses Weapon

            SetSkill(SkillName.MagicResist, 35.0, 40.0);
            
            SetSkill(SkillName.Swords, 65.0, 70.0);
            SetSkill(SkillName.Archery, 65.0, 70.0);
            SetSkill(SkillName.Macing, 65.0, 70.0);
            SetSkill(SkillName.Fencing, 65.0, 70.0);
            SetSkill(SkillName.Wrestling, 65.0, 70.0);      

			Fame = 1000;
			Karma = -1000;
	            
			switch ( Utility.Random( 6 ))
			{
				case 0: AddItem( new Scimitar() ); break;
				case 1: AddItem( new Cutlass() ); break;
				case 2: AddItem( new Broadsword() ); break;
				case 3: AddItem( new Maul() ); break;
				case 4: AddItem( new Club() ); break;
                case 5:
                {
                    AddItem( new Crossbow());
                    PackItem(new CrossbowBolts(25));

                    break;
                };                    
			}

			Utility.AssignRandomHair( this );
		}		 
		
		public override void GenerateLoot()
		{
            base.PackAddGoldTier(3);
            base.PackAddArtifactChanceTier(3);
            base.PackAddMapChanceTier(3);

            PackItem(new PewterMug());
            PackItem(new Dagger());
            PackItem(new BeverageBottle(BeverageType.Ale));

			if (Utility.Random(500) == 7)
				PackItem( new DecoBottlesOfLiquor() );			
		}

		public override bool AlwaysMurderer{ get{ return true; } }

		public Pirate( Serial serial ) : base( serial )
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
