using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName("a lava snake corpse")]
	[TypeAlias( "Server.Mobiles.Lavasnake" )]
	public class LavaSnake : BaseCreature
	{
		[Constructable]
		public LavaSnake() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a lava snake";
			Body = 52;
			Hue = Utility.RandomList( 0x647, 0x650, 0x659, 0x662, 0x66B, 0x674 );
			BaseSoundID = 0xDB;

			SetStr( 30, 35 );
			SetDex( 30, 35 );
			SetInt( 10, 15 );

            SetHits(20, 30);
            SetMana(0);

            SetDamage(4, 6);

            VirtualArmor = 20;
            
            SetSkill(SkillName.Tactics, 100.0, 100.0);

            SetSkill(SkillName.MagicResist, 35.0, 40.0);

            SetSkill(SkillName.Wrestling, 30.0, 35.0);

			Fame = 600;
			Karma = -600;

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = 45.0;			
		}

		public override void GenerateLoot()
		{
            base.PackAddGoldTier(2);
            base.PackAddArtifactChanceTier(2);
            base.PackAddMapChanceTier(2);

            PackItem(new SulfurousAsh(5));            
		}

		public override bool DeathAdderCharmable{ get{ return true; } }

        public override Poison PoisonImmune { get { return Poison.Regular; } }
        public override Poison HitPoison { get { return Poison.Lesser; } }

		public override bool HasBreath{ get{ return true; } } // fire breath enabled
		public override int Meat{ get{ return 1; } }

		public LavaSnake(Serial serial) : base(serial)
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