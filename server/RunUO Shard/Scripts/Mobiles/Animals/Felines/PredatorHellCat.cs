using System;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a hell cat corpse" )]
	[TypeAlias( "Server.Mobiles.Preditorhellcat" )]
	public class PredatorHellCat : BaseCreature
	{
		[Constructable]
		public PredatorHellCat() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a hell cat";
			Body = 214; //127;
			BaseSoundID = 0xBA;
            Hue = Utility.RandomList(0x647, 0x650, 0x659, 0x662, 0x66B, 0x674);

            SetStr( 100, 105 );
			SetDex( 80, 85 );
			SetInt( 15, 20 );

			SetHits( 175, 200 );

			SetDamage( 12, 16 );

            VirtualArmor = 10;

            SetSkill(SkillName.Tactics, 100.0, 100.0);

			SetSkill( SkillName.MagicResist, 45.0, 50.0 );	
		
			SetSkill( SkillName.Wrestling, 75.0, 80.0 );

			Fame = 2500;
			Karma = -2500;	
		}

		public override void GenerateLoot()
		{
            base.PackAddGoldTier(3);
            base.PackAddArtifactChanceTier(3);
            base.PackAddMapChanceTier(3);
            
            PackItem(new SulfurousAsh(15));
		}

		public override bool HasBreath{ get{ return true; } } // fire breath enabled

		public override int Hides{ get{ return 10; } }
		public override HideType HideType{ get{ return HideType.Spined; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Meat; } }
		public override PackInstinct PackInstinct{ get{ return PackInstinct.Feline; } }

		public PredatorHellCat(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}