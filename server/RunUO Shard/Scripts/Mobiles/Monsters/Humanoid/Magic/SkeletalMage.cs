using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a skeletal corpse" )]
	public class SkeletalMage : BaseCreature
	{
		[Constructable]
		public SkeletalMage() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a skeletal mage";
			Body = 148;
			BaseSoundID = 451;

            SpellCastAnimation = 11;
            SpellCastFrameCount = 5;

            SetStr(75, 80);
            SetDex(35, 40);
            SetInt(300, 400);

            SetHits(125, 150);
            SetMana(300, 400);

            SetDamage(3, 5);

            VirtualArmor = 15;

            SetSkill(SkillName.Tactics, 100.0, 100.0);

            SetSkill(SkillName.Meditation, 100.0, 100.0);
            SetSkill(SkillName.EvalInt, 55.0, 60.0);
            SetSkill(SkillName.Magery, 55.0, 60.0);

            SetSkill(SkillName.MagicResist, 55.0, 60.0);

            SetSkill(SkillName.Wrestling, 45.0, 50.0);

            Fame = 3000;
            Karma = -3000;
        }

        public override void GenerateLoot()
        {
            base.PackAddGoldTier(4);
            base.PackAddArtifactChanceTier(4);
            base.PackAddMapChanceTier(4);

            base.PackAddScrollTier(2);
            base.PackAddReagentTier(2);

            PackItem(new Bone());
            PackItem(new BonePile());

            if (0.2 > Utility.RandomDouble())
                PackItem(new BoneDust());
        }
		
		public override bool BleedImmune{ get{ return true; } }

		public override OppositionGroup OppositionGroup
		{
			get{ return OppositionGroup.FeyAndUndead; }
		}

		public override Poison PoisonImmune{ get{ return Poison.Regular; } }

		public SkeletalMage( Serial serial ) : base( serial )
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