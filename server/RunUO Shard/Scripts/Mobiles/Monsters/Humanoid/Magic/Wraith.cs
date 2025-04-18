using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a ghostly corpse" )]
	public class Wraith : BaseCreature
	{
		[Constructable]
		public Wraith() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a wraith";
			Body = 26;
			Hue = 0x4001;
			BaseSoundID = 0x482;

            SpellCastAnimation = 6;
            SpellCastFrameCount = 4;

            SetStr(75, 80);
            SetDex(25, 30);
            SetInt(300, 400);

            SetHits(75, 100);
            SetMana(300, 400);

            SetDamage(3, 6);

            VirtualArmor = 5;

            SetSkill(SkillName.Tactics, 100.0, 100.0);

            SetSkill(SkillName.Meditation, 100.0, 100.0);
            SetSkill(SkillName.EvalInt, 40.0, 45.0);
            SetSkill(SkillName.Magery, 40.0, 45.0);

            SetSkill(SkillName.MagicResist, 40.0, 45.0);

            SetSkill(SkillName.Wrestling, 75.0, 80.0);

            Fame = 4000;
            Karma = -4000;
        }

        public override void GenerateLoot()
        {
            base.PackAddGoldTier(4);
            base.PackAddArtifactChanceTier(4);
            base.PackAddMapChanceTier(4);

            base.PackAddScrollTier(2);
            base.PackAddReagentTier(2);

            if (0.2 > Utility.RandomDouble())
                PackItem(new BoneDust());
        }
		
		public override bool BleedImmune{ get{ return true; } }

		public override OppositionGroup OppositionGroup
		{
			get{ return OppositionGroup.FeyAndUndead; }
		}

		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }

		public Wraith( Serial serial ) : base( serial )
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


        public override void AggressiveAction(Mobile aggressor, bool criminal)
        {
            base.AggressiveAction(aggressor, criminal);

            if (aggressor.HueMod == 1882)
            {
                AOS.Damage(aggressor, 50, 0, 100, 0, 0, 0);
                aggressor.BodyMod = 0;
                aggressor.HueMod = -1;
                aggressor.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);
                aggressor.PlaySound(0x307);
                aggressor.SendMessage("Your skin is scorched as the undead paint burns away!"); // Your skin is scorched as the tribal paint burns away!
            }
        }
	}
}