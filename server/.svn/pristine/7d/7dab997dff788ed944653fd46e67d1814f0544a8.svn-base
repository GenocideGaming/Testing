using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "a lava lizard corpse" )]
	[TypeAlias( "Server.Mobiles.Lavalizard" )]
	public class LavaLizard : BaseCreature
	{
		[Constructable]
		public LavaLizard() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a lava lizard";
			Body = 0xCE;
			Hue = Utility.RandomList( 0x647, 0x650, 0x659, 0x662, 0x66B, 0x674 );
			BaseSoundID = 0x5A;

			SetStr( 150, 155 );
			SetDex( 50, 55 );
			SetInt( 10, 15 );

            SetHits( 200, 225);
			SetMana( 0 );

			SetDamage( 7, 14 );

            VirtualArmor = 20;
            
            SetSkill( SkillName.Tactics, 100.0, 100.0);

			SetSkill( SkillName.MagicResist, 45.0, 50.0 );
			
			SetSkill( SkillName.Wrestling, 65.0, 70.0 );

			Fame = 3000;
			Karma = -3000;			

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 75.0;			
		}       

        public override void GenerateLoot()
        {
            base.PackAddGoldTier(4);
            base.PackAddArtifactChanceTier(4);
            base.PackAddMapChanceTier(4);

            PackItem(new SulfurousAsh(15));

            PackItem(Loot.RandomBodyPart());
        }

		public override bool HasBreath{ get{ return true; } } // fire breath enabled
		public override int Hides{ get{ return 12; } }
		public override HideType HideType{ get{ return HideType.Spined; } }

		public LavaLizard(Serial serial) : base(serial)
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