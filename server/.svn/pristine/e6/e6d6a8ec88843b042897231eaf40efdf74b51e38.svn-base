using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a shadow wyrm corpse" )]
	public class ShadowWyrm : BaseCreature
	{
		[Constructable]
		public ShadowWyrm() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a shadow wyrm";
			Body = 106;
			BaseSoundID = 362;

            SpellCastAnimation = 11;
            SpellCastFrameCount = 5;

            SetStr(400, 405);
            SetDex(65, 70);
            SetInt(1400, 1600);

            SetHits(800, 900);
            SetMana(1400, 1600);

            SetDamage(18, 28);

            VirtualArmor = 20;

            SetSkill(SkillName.Tactics, 100.0, 100.0);

            SetSkill(SkillName.EvalInt, 60.0, 65.0);
            SetSkill(SkillName.Meditation, 100, 100.0);
            SetSkill(SkillName.Magery, 60.0, 65.0);

            SetSkill(SkillName.MagicResist, 60.0, 65.0);

            SetSkill(SkillName.Wrestling, 70.0, 75.0);

			Fame = 22500;
			Karma = -22500;

            Tamable = true;
            ControlSlots = 5;
            MinTameSkill = 99.9;   
		}        

        public override void GenerateLoot()
        {
            base.PackAddGoldTier(10);
            base.PackAddArtifactChanceTier(10);
            base.PackAddMapChanceTier(10);

            base.PackAddScrollTier(8);
            base.PackAddReagentTier(4); 
            base.PackAddPoisonPotionTier(8);

            PackItem(new Nightshade(50));

            PackItem(new BonePile());
            PackItem(new BonePile());
        }

        public override bool HasBreath { get { return true; } } // fire breath enabled
        public override Poison PoisonImmune { get { return Poison.Lethal; } }
        public override Poison HitPoison { get { return Poison.Deadly; } }

		public override int GetIdleSound()
		{
			return 0x2D5;
		}

		public override int GetHurtSound()
		{
			return 0x2D1;
		}

        

		public override bool ReacquireOnMovement{ get{ return true; } }	

		public override bool AutoDispel{ get{ return true; } }

		public override int Meat{ get{ return 19; } }
		public override int Hides{ get{ return 20; } }
		public override int Scales{ get{ return 10; } }
		public override ScaleType ScaleType{ get{ return ScaleType.Black; } }
		public override HideType HideType{ get{ return HideType.Barbed; } }

		public ShadowWyrm( Serial serial ) : base( serial )
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