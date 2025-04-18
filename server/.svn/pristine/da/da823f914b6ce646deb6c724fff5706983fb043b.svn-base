using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a dragon corpse" )]
	public class AncientWyrm : BaseCreature
	{
		[Constructable]
		public AncientWyrm () : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "an ancient wyrm";
			Body = 46;
			BaseSoundID = 362;

            SpellCastAnimation = 11;
            SpellCastFrameCount = 5;

            SetStr(400, 405);
            SetDex(65, 70);
            SetInt(1600, 1800);

            SetHits(1500, 1600);
            SetMana(1600, 1800);

            SetDamage(25, 35);

            VirtualArmor = 20;

            SetSkill(SkillName.Tactics, 100.0, 100.0);

            SetSkill(SkillName.EvalInt, 60.0, 65.0);
            SetSkill(SkillName.Meditation, 100, 100.0);
            SetSkill(SkillName.Magery, 60.0, 65.0);

            SetSkill(SkillName.MagicResist, 60.0, 65.0);

            SetSkill(SkillName.Wrestling, 75.0, 80.0);

			Fame = 22500;
			Karma = -22500;
		}	 
		
		public override void GenerateLoot()
		{
            base.PackAddGoldTier(12);
            base.PackAddArtifactChanceTier(12);
            base.PackAddMapChanceTier(12);

            base.PackAddScrollTier(8);
            base.PackAddScrollTier(7);

            base.PackAddReagentTier(6);
            base.PackAddReagentTier(5);

            PackItem(new BonePile());
            PackItem(new BonePile());
		}

		public override int GetIdleSound()
		{
			return 0x2D3;
		}

		public override int GetHurtSound()
		{
			return 0x2D1;
		}

        public override bool HasBreath { get { return true; } } // fire breath enabled
        public override Poison PoisonImmune { get { return Poison.Lethal; } }

		public override bool ReacquireOnMovement{ get{ return true; } }
		
		public override bool AutoDispel{ get{ return true; } }
		public override HideType HideType{ get{ return HideType.Barbed; } }
		public override int Hides{ get{ return 40; } }
		public override int Meat{ get{ return 19; } }
		public override int Scales{ get{ return 12; } }
		public override ScaleType ScaleType{ get{ return (ScaleType)Utility.Random( 4 ); } }	

		public AncientWyrm( Serial serial ) : base( serial )
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