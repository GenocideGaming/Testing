using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "a lava serpent corpse" )]
	[TypeAlias( "Server.Mobiles.Lavaserpant" )]
	public class LavaSerpent : BaseCreature
	{
		[Constructable]
		public LavaSerpent() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a lava serpent";
			Body = 90;
			BaseSoundID = 219;

            SetStr(125, 130);
            SetDex(60, 65);
            SetInt(10, 15);

            SetHits(125, 150);
            SetMana(0);

            SetDamage(6, 12);

            VirtualArmor = 20;

            SetSkill(SkillName.Tactics, 100.0, 100.0);
            
            SetSkill(SkillName.MagicResist, 25.0, 30.0);

            SetSkill(SkillName.Wrestling, 65.0, 70.0);

			Fame = 4500;
			Karma = -4500;

            Tamable = true;
            ControlSlots = 2;
            MinTameSkill = 75.0;			
		}

		public override void GenerateLoot()
		{
            base.PackAddGoldTier(3);
            base.PackAddArtifactChanceTier(3);
            base.PackAddMapChanceTier(3);

            base.PackAddPoisonPotionTier(6);

            PackItem(new SulfurousAsh(15));

            PackItem(new Bone());
            PackItem(Loot.RandomBodyPart());
            PackItem(Loot.RandomBodyPart());
		}

        public override Poison PoisonImmune { get { return Poison.Deadly; } }
        public override Poison HitPoison { get { return Poison.Greater;  } }

		public override bool DeathAdderCharmable{ get{ return true; } }

		public override bool HasBreath{ get{ return true; } } // fire breath enabled
		public override int Meat{ get{ return 4; } }
		public override int Hides{ get{ return 15; } }
		public override HideType HideType{ get{ return HideType.Spined; } }

		public LavaSerpent(Serial serial) : base(serial)
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