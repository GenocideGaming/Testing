using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "an ophidian corpse" )]
	[TypeAlias( "Server.Mobiles.OphidianJusticar", "Server.Mobiles.OphidianZealot" )]
	public class OphidianArchmage : BaseCreature
	{
		private static string[] m_Names = new string[]
			{
				"an ophidian justicar",
				"an ophidian zealot"
			};

		[Constructable]
		public OphidianArchmage() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = m_Names[Utility.Random( m_Names.Length )];
			Body = 85;
			BaseSoundID = 639;

            SpellCastAnimation = 13;
            SpellCastFrameCount = 5;

            SetStr(150, 155);
            SetDex(60, 65);
            SetInt(800, 1000);

            SetHits(325, 350);
            SetMana(800, 1000);

            SetDamage(5, 10);

            VirtualArmor = 15;

            SetSkill(SkillName.Tactics, 100.0, 100.0);

            SetSkill(SkillName.Meditation, 100.0, 100.0);
            SetSkill(SkillName.EvalInt, 85.0, 90.0);
            SetSkill(SkillName.Magery, 85.0, 90.0);

            SetSkill(SkillName.MagicResist, 85.0, 90.0);

            SetSkill(SkillName.Wrestling, 70.0, 75.0);

			Fame = 11500;
			Karma = -11500;			
		}
		
		public override void GenerateLoot()
		{
            base.PackAddGoldTier(7);
            base.PackAddArtifactChanceTier(7);
            base.PackAddMapChanceTier(7);

            base.PackAddScrollTier(5);
            base.PackAddReagentTier(5);
		}

        public override Poison PoisonImmune { get { return Poison.Lethal; } }

		public override int Meat{ get{ return 1; } }

		public override OppositionGroup OppositionGroup
		{
			get{ return OppositionGroup.TerathansAndOphidians; }
		}

		public OphidianArchmage( Serial serial ) : base( serial )
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