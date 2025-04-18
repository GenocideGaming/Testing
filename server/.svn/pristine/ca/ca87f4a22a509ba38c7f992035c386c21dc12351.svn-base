using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a skeletal dragon corpse" )]
	public class SkeletalDragon : BaseCreature
	{
		[Constructable]
		public SkeletalDragon () : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a skeletal dragon";
			Body = 104;
			BaseSoundID = 0x488;

			SetStr( 898, 1030 );
			SetDex( 68, 200 );
			SetInt( 488, 620 );

            SetHits(800, 1000);

            SetDamage(12, 18);

            VirtualArmor = 25;

            
            
            

            SetSkill(SkillName.Tactics, 100.0, 100.0);

            SetSkill(SkillName.EvalInt, 50.0, 55.0);
            SetSkill(SkillName.Magery, 50.0, 55.0);
            SetSkill(SkillName.MagicResist, 60.0, 65.0);
            SetSkill(SkillName.Wrestling, 75.0, 80.0);

			Fame = 22500;
			Karma = -22500;

			VirtualArmor = 80;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich, 4 );
			AddLoot( LootPack.Gems, 5 );
		}

		public override bool ReacquireOnMovement{ get{ return true; } }
		public override bool HasBreath{ get{ return true; } } // fire breath enabled
		public override int BreathFireDamage{ get{ return 0; } }
		public override int BreathColdDamage{ get{ return 100; } }
		public override int BreathEffectHue{ get{ return 0x480; } }
		public override double BonusPetDamageScalar{ get{ return (Core.SE)? 3.0 : 1.0; } }
		// TODO: Undead summoning?

		public override bool AutoDispel{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override bool BleedImmune{ get{ return true; } }
		public override int Meat{ get{ return 19; } } // where's it hiding these? :)
		public override int Hides{ get{ return 20; } }
		public override HideType HideType{ get{ return HideType.Barbed; } }

		public SkeletalDragon( Serial serial ) : base( serial )
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