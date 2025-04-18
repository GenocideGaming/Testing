using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "an ophidian corpse" )]
	[TypeAlias( "Server.Mobiles.OphidianShaman" )]
	public class OphidianMage : BaseCreature
	{
		private static string[] m_Names = new string[]
			{
				"an ophidian apprentice mage",
				"an ophidian shaman"
			};

		[Constructable]
		public OphidianMage() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = m_Names[Utility.Random( m_Names.Length )];
			Body = 85;
			BaseSoundID = 639;

            SpellCastAnimation = 13;
            SpellCastFrameCount = 5;

			SetStr( 150, 155 );
			SetDex( 60, 65 );
			SetInt( 500, 600 );

            SetHits( 150, 175 );
            SetMana( 500, 600 );

			SetDamage( 5, 10 );

            VirtualArmor = 15;

            SetSkill( SkillName.Tactics, 100.0, 100.0);

            SetSkill( SkillName.Meditation, 100.0, 100.0);
			SetSkill( SkillName.EvalInt, 75.0, 80.0 );
			SetSkill( SkillName.Magery, 75.0, 80.0 );

			SetSkill( SkillName.MagicResist, 75.0, 80.0 );

			SetSkill( SkillName.Wrestling, 60.0, 65.0 );

			Fame = 4000;
			Karma = -4000;			
		}
		
		public override void GenerateLoot()
		{
            base.PackAddGoldTier(6);
            base.PackAddArtifactChanceTier(6);
            base.PackAddMapChanceTier(6);

            base.PackAddScrollTier(4);
            base.PackAddReagentTier(4);
		}

		public override int Meat{ get{ return 1; } }

        public override Poison PoisonImmune { get { return Poison.Lethal; } }
		

		public override OppositionGroup OppositionGroup
		{
			get{ return OppositionGroup.TerathansAndOphidians; }
		}

		public OphidianMage( Serial serial ) : base( serial )
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