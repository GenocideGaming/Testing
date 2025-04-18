using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a ghostly corpse" )]
	public class Ghoul : BaseCreature
	{
		[Constructable]
		public Ghoul() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a ghoul";
			Body = 153;
			BaseSoundID = 0x482;

			SetStr( 75, 80 );
			SetDex( 25, 30 );
			SetInt( 15, 20 );
			
			SetMana( 0 );

            SetHits(75, 100);

            SetDamage(3, 6);

            VirtualArmor = 5;

            SetSkill(SkillName.Tactics, 100.0, 100.0);

            SetSkill(SkillName.MagicResist, 20.0, 25.0);

            SetSkill(SkillName.Wrestling, 15.0, 20.0);

			Fame = 2500;
			Karma = -2500;
		}
		
		public override void GenerateLoot()
		{
            base.PackAddGoldTier(1);
            base.PackAddArtifactChanceTier(1);
            base.PackAddMapChanceTier(1);

            base.PackAddBandageTier(1);

            if (0.1 > Utility.RandomDouble())
                PackItem(new BoneDust());
		}

		public override bool BleedImmune{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Regular; } }

		public Ghoul( Serial serial ) : base( serial )
		{
		}

		public override OppositionGroup OppositionGroup
		{
			get{ return OppositionGroup.FeyAndUndead; }
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