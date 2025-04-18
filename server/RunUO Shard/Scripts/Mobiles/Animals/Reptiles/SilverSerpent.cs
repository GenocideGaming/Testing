using System;
using Server.Mobiles;
using Server.Factions;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName("a silver serpent corpse")]
	[TypeAlias( "Server.Mobiles.Silverserpant" )]
	public class SilverSerpent : BaseCreature
	{
		public override Ethics.Ethic EthicAllegiance { get { return Ethics.Ethic.Hero; } }

		[Constructable]
		public SilverSerpent() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Body = 92;
			Name = "a silver serpent";
			BaseSoundID = 219;

			SetStr( 150, 155 );
			SetDex( 60, 65 );
			SetInt( 15, 20 );

            SetHits(125, 150);
            SetMana(0);

            SetDamage(6, 12);

            VirtualArmor = 20;            
            
            SetSkill(SkillName.Tactics, 100.0, 100.0);

			SetSkill( SkillName.MagicResist, 95.0, 100.0 );
			
			SetSkill( SkillName.Wrestling, 65.0, 70.0 );

			Fame = 7000;
			Karma = -7000;

            Tamable = true;
            ControlSlots = 3;
            MinTameSkill = 98.0;
		}        

        public override void GenerateLoot()
        {
            base.PackAddGoldTier(5);
            base.PackAddArtifactChanceTier(5);
            base.PackAddMapChanceTier(5);

            base.PackAddPoisonPotionTier(8);

            PackItem(new Bone());
            PackItem(Loot.RandomBodyPart());
            PackItem(Loot.RandomBodyPart());
        }

        public override Poison PoisonImmune { get { return Poison.Lethal; } }
        public override Poison HitPoison { get { return Poison.Lethal; } }

		public override bool DeathAdderCharmable{ get{ return true; } }
		public override int Meat{ get{ return 1; } }		

		public SilverSerpent(Serial serial) : base(serial)
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