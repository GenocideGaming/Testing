using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "an ophidian corpse" )]
	[TypeAlias( "Server.Mobiles.OphidianAvenger" )]
	public class OphidianKnight : BaseCreature
	{
		private static string[] m_Names = new string[]
			{
				"an ophidian knight-errant",
				"an ophidian avenger"
			};

		[Constructable]
		public OphidianKnight() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = m_Names[Utility.Random( m_Names.Length )];
			Body = 86;
			BaseSoundID = 634;

            SetStr(300, 305);
            SetDex(85, 90);
            SetInt(20, 25);

            SetHits(325, 350);          

            SetDamage(18, 28);

            VirtualArmor = 15;

            SetSkill(SkillName.Tactics, 100.0, 100.0);

            SetSkill(SkillName.MagicResist, 45.0, 50.0);

            SetSkill(SkillName.Wrestling, 80.0, 85.0);

            Fame = 4000;
            Karma = -4000;
        }

        public override void GenerateLoot()
        {
            base.PackAddGoldTier(7);
            base.PackAddArtifactChanceTier(7);
            base.PackAddMapChanceTier(6);

            base.PackAddRandomPotionTier(4);
            base.PackAddPoisonPotionTier(7);

            PackItem(new Bardiche());
        }

		public override int Meat{ get{ return 2; } }

		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override Poison HitPoison{ get{ return Poison.Deadly; } }		

		public override OppositionGroup OppositionGroup
		{
			get{ return OppositionGroup.TerathansAndOphidians; }
		}

		public OphidianKnight( Serial serial ) : base( serial )
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