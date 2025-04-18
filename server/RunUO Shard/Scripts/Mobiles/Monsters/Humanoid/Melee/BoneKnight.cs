using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a skeletal corpse" )]
	public class BoneKnight : BaseCreature
	{
		[Constructable]
		public BoneKnight() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a bone knight";
			Body = 57;
			BaseSoundID = 451;

			SetStr( 196, 250 );
			SetDex( 76, 95 );
			SetInt( 36, 60 );

			SetHits( 275, 300 );

			SetDamage( 10, 18 );

            VirtualArmor = 15;

            SetSkill(SkillName.Tactics, 100.0, 100.0);

			SetSkill( SkillName.MagicResist, 20.0, 25.0 );

			SetSkill( SkillName.Wrestling, 70.0, 75.0 );

			Fame = 3000;
			Karma = -3000;  
		}
		
		public override void GenerateLoot()
		{
            base.PackAddGoldTier(4);
            base.PackAddArtifactChanceTier(4);
            base.PackAddMapChanceTier(4);

            base.PackAddBandageTier(2);

			switch ( Utility.Random( 6 ) )
			{
				case 0: PackItem( new PlateArms() ); break;
				case 1: PackItem( new PlateChest() ); break;
				case 2: PackItem( new PlateGloves() ); break;
				case 3: PackItem( new PlateGorget() ); break;
				case 4: PackItem( new PlateLegs() ); break;
				case 5: PackItem( new PlateHelm() ); break;
			}

			PackItem( new Scimitar() );
			PackItem( new WoodenShield() );			

            if (0.25 > Utility.RandomDouble())
                PackItem(new BoneDust());
		}

		public override bool BleedImmune{ get{ return true; } }

		public BoneKnight( Serial serial ) : base( serial )
		{
		}

		public override OppositionGroup OppositionGroup
		{
			get{ return OppositionGroup.FeyAndUndead; }
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