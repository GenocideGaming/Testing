using System;
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server.Misc;

namespace Server.Mobiles
{
	[CorpseName( "an orcish corpse" )]
	public class OrcBrute : BaseCreature
	{
		[Constructable]
		public OrcBrute() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Body = 189;

			Name = "an orc brute";
			BaseSoundID = 0x45A;

			SetStr( 767, 945 );
			SetDex( 66, 75 );
			SetInt( 46, 70 );

			SetHits( 700, 800 );

			SetDamage( 18, 24 );

            VirtualArmor = 5;

            SetSkill(SkillName.Tactics, 100.0, 100.0);
			SetSkill( SkillName.MagicResist, 20.0, 30.0 );
            SetSkill(SkillName.Macing, 60.0, 65.0);
			SetSkill( SkillName.Wrestling, 60.0, 65.0 );

			Fame = 15000;
			Karma = -15000;			
		}
		
		

		public override void GenerateLoot()
		{
            base.PackAddGoldTier(7);
            base.PackAddArtifactChanceTier(5);
            base.PackAddMapChanceTier(5);

            base.PackAddGemTier(3);

            PackItem(new Club());

            if (0.12 > Utility.RandomDouble())
                PackItem(new OrcishKinMask());

            if (0.5 > Utility.RandomDouble())
                PackItem(new OrcishHeart());
		}

		public override bool BardImmune{ get{ return !Core.AOS; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override int Meat{ get{ return 2; } }

		public override bool CanRummageCorpses{ get{ return true; } }

        public override OppositionGroup OppositionGroup
        {
            get { return OppositionGroup.SavagesAndOrcs; }
        }

        public override bool IsEnemy(Mobile m)
        {
            if (m.Player && m.FindItemOnLayer(Layer.Helm) is OrcishKinMask)
                return false;

            return base.IsEnemy(m);
        }

        public override void AggressiveAction(Mobile aggressor, bool criminal)
        {
            base.AggressiveAction(aggressor, criminal);

            if (aggressor.HueMod == 1451)
            {
                AOS.Damage(aggressor, 50, 0, 100, 0, 0, 0);
                aggressor.BodyMod = 0;
                aggressor.HueMod = -1;
                aggressor.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);
                aggressor.PlaySound(0x307);
                aggressor.SendMessage("Your skin is scorched as the orcish paint burns away!"); // Your skin is scorched as the tribal paint burns away!
            }
        }

		

		

		public OrcBrute( Serial serial ) : base( serial )
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
