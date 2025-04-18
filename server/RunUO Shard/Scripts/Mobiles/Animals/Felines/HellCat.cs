using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "a hell cat corpse" )]
	[TypeAlias( "Server.Mobiles.Hellcat" )]
	public class HellCat : BaseCreature
	{
		[Constructable]
		public HellCat() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a hell cat";
			Body = 0xC9;
			Hue = Utility.RandomList( 0x647, 0x650, 0x659, 0x662, 0x66B, 0x674);
			BaseSoundID = 0x69;

			SetStr( 50, 55 );
			SetDex( 45, 50 );
			SetInt( 10, 15 );

			SetHits( 75, 100 );

			SetDamage( 8, 12 );

            VirtualArmor = 10;
            
            SetSkill(SkillName.Tactics, 100.0, 100.0);

			SetSkill( SkillName.MagicResist, 45.0, 50.0 );
			
			SetSkill( SkillName.Wrestling, 50.0, 55.0 );

			Fame = 1000;
			Karma = -1000;
		}

		public override void GenerateLoot()
		{
            base.PackAddGoldTier(2);
            base.PackAddArtifactChanceTier(2);
            base.PackAddMapChanceTier(2);

            PackItem(new SulfurousAsh(10));
		}

		public override bool HasBreath{ get{ return true; } } // fire breath enabled
		public override int Hides{ get{ return 10; } }
		public override HideType HideType{ get{ return HideType.Spined; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Meat; } }
		public override PackInstinct PackInstinct{ get{ return PackInstinct.Feline; } }

		public HellCat(Serial serial) : base(serial)
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