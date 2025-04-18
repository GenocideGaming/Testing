using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "a giant serpent corpse" )]
	[TypeAlias( "Server.Mobiles.Serpant" )]
	public class GiantSerpent : BaseCreature
	{
		[Constructable]
		public GiantSerpent() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a giant serpent";
			Body = 0x15;
			Hue = Utility.RandomSnakeHue();
			BaseSoundID = 219;

			SetStr( 125, 130 );
			SetDex( 60, 65 );
			SetInt( 10, 15 );

            SetHits( 100, 125 );
			SetMana( 0 );

			SetDamage( 5, 10 );

            VirtualArmor = 20;

            SetSkill(SkillName.Tactics, 100.0, 100.0);
			
			SetSkill( SkillName.MagicResist, 25.0, 30.0 );
			
			SetSkill( SkillName.Wrestling, 60.0, 65.0 );

			Fame = 2500;
			Karma = -2500;

            Tamable = true;
            ControlSlots = 2;
            MinTameSkill = 80.0;			
		}        

        public override void GenerateLoot()
        {
            base.PackAddGoldTier(3);
            base.PackAddArtifactChanceTier(3);
            base.PackAddMapChanceTier(3);

            base.PackAddPoisonPotionTier(7);

            PackItem(new Bone());
            PackItem(Loot.RandomBodyPart());
            PackItem(Loot.RandomBodyPart());
        }

		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override Poison HitPoison{ get{ return (Poison.Deadly); } }

		public override bool DeathAdderCharmable{ get{ return true; } }

		public override int Meat{ get{ return 4; } }
		public override int Hides{ get{ return 15; } }
		public override HideType HideType{ get{ return HideType.Spined; } }

		public GiantSerpent(Serial serial) : base(serial)
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

			if ( BaseSoundID == -1 )
				BaseSoundID = 219;
		}
	}
}