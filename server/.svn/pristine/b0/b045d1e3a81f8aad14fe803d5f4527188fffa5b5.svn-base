using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "an ophidian corpse" )]
	public class OphidianWarrior : BaseCreature
	{
		private static string[] m_Names = new string[]
			{
				"an ophidian warrior",
				"an ophidian enforcer"
			};

		[Constructable]
		public OphidianWarrior() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = m_Names[Utility.Random( m_Names.Length )];
			Body = 86;
			BaseSoundID = 634;

            SetStr(145, 150);
            SetDex(75, 80);
            SetInt(20, 25);

            SetHits(225, 250);            

            SetDamage(14, 22);

            VirtualArmor = 15;

            SetSkill(SkillName.Tactics, 100.0, 100.0);

            SetSkill(SkillName.MagicResist, 45.0, 50.0);

            SetSkill(SkillName.Wrestling, 75.0, 80.0);

            Fame = 4000;
            Karma = -4000;
        }

        public override void GenerateLoot()
        {
            base.PackAddGoldTier(5);
            base.PackAddArtifactChanceTier(4);
            base.PackAddMapChanceTier(4);

            base.PackAddRandomPotionTier(3);    

            PackItem(new Bardiche());
        }

		public override int Meat{ get{ return 1; } }

        public override Poison PoisonImmune { get { return Poison.Lethal; } }

		public override OppositionGroup OppositionGroup
		{
			get{ return OppositionGroup.TerathansAndOphidians; }
		}

		public OphidianWarrior( Serial serial ) : base( serial )
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