using System;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a wyvern corpse" )]
	public class Wyvern : BaseCreature
	{
		[Constructable]
		public Wyvern () : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a wyvern";
			Body = 62;
			BaseSoundID = 362;

            Hue = Utility.RandomGreenHue();

            SetStr(450, 500);
            SetDex(70, 75);
            SetInt(25, 30);

            SetHits(275, 300);

            SetDamage(10, 14);

            VirtualArmor = 20;

            SetSkill( SkillName.Tactics, 100.0, 100.0);

            SetSkill( SkillName.MagicResist, 45.0, 50.0);

            SetSkill( SkillName.Wrestling, 65.0, 70.0);

			Fame = 4000;
			Karma = -4000;

            Tamable = true;
            ControlSlots = 2;
            MinTameSkill = 92.0;
		}        

        public override void GenerateLoot()
        {
            base.PackAddGoldTier(5);
            base.PackAddArtifactChanceTier(5);
            base.PackAddMapChanceTier(5);

            base.PackAddPoisonPotionTier(7);

            PackItem(new Nightshade(10));

            PackItem(new Bone());
        }

		public override bool ReacquireOnMovement{ get{ return true; } }

		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override Poison HitPoison{ get{ return Poison.Deadly; } }		

		public override int Meat{ get{ return 10; } }
		public override int Hides{ get{ return 20; } }
		public override HideType HideType{ get{ return HideType.Horned; } }

		public override int GetAttackSound()
		{
			return 713;
		}

		public override int GetAngerSound()
		{
			return 718;
		}

		public override int GetDeathSound()
		{
			return 716;
		}

		public override int GetHurtSound()
		{
			return 721;
		}

		public override int GetIdleSound()
		{
			return 725;
		}

		public Wyvern( Serial serial ) : base( serial )
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