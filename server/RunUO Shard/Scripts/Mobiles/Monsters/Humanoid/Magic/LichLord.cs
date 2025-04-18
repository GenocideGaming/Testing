using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a liche's corpse" )]
	public class LichLord : BaseCreature
	{
		[Constructable]
		public LichLord() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a lich lord";
			Body = 24;
			BaseSoundID = 412;

            SpellCastAnimation = 12;
            SpellCastFrameCount = 7;

            SetStr(200, 205);
            SetDex(45, 50);
            SetInt(800, 1000);

            SetHits(500, 600);
            SetMana(1400, 1600);

            SetDamage(10, 20);

            VirtualArmor = 5;

            SetSkill(SkillName.Tactics, 100.0, 100.0);

            SetSkill(SkillName.EvalInt, 95.0, 100.0);
            SetSkill(SkillName.Magery, 95.0, 100.0);
            SetSkill(SkillName.Meditation, 100.0, 100.0);

            SetSkill(SkillName.MagicResist, 95.0, 100.0);

            SetSkill(SkillName.Wrestling, 65.0, 70.0);

			Fame = 18000;
			Karma = -18000;
		}

        public override void GenerateLoot()
        {
            base.PackAddGoldTier(8);
            base.PackAddArtifactChanceTier(8);
            base.PackAddMapChanceTier(8);

            base.PackAddScrollTier(6);
            base.PackAddReagentTier(5);

            PackItem(new BonePile());
            PackItem(new GnarledStaff());

            if (0.99 > Utility.RandomDouble())
                PackItem(new BoneDust());
        }

        public override Poison PoisonImmune { get { return Poison.Lethal; } }
		

		public override OppositionGroup OppositionGroup
		{
			get{ return OppositionGroup.FeyAndUndead; }
		}

		public override bool CanRummageCorpses{ get{ return true; } }
		public override bool BleedImmune{ get{ return true; } }
		
		public LichLord( Serial serial ) : base( serial )
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