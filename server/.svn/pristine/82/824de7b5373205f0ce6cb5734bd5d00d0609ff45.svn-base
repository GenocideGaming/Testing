using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "an earth elemental corpse" )]
	public class EarthElemental : BaseCreature
	{
		public override double DispelDifficulty{ get{ return 117.5; } }
		public override double DispelFocus{ get{ return 45.0; } }

		[Constructable]
		public EarthElemental() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "an earth elemental";
			Body = 14;
			BaseSoundID = 268;

			SetStr( 225, 230 );
			SetDex( 50, 55 );
			SetInt( 10, 15 );

			SetHits( 175, 200 );

			SetDamage( 10, 18 );

            VirtualArmor = 30;

            SetSkill(SkillName.Tactics, 100.0, 100.0);

			SetSkill( SkillName.MagicResist, 95.0, 100.0 );

			SetSkill( SkillName.Wrestling, 70.0, 75.0 );

			Fame = 3500;
			Karma = -3500;
			
			ControlSlots = 2;
		}		 
		
		public override void GenerateLoot()
		{
            base.PackAddGoldTier(4);
            base.PackAddArtifactChanceTier(4);
            base.PackAddMapChanceTier(4);

            base.PackAddGemTier(1);
            base.PackAddIngotTier(2);

            PackItem(new IronOre());
		}

		public override bool BleedImmune{ get{ return true; } }
		

		public EarthElemental( Serial serial ) : base( serial )
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