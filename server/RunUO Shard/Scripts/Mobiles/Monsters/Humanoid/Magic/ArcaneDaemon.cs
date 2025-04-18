using System;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "an arcane daemon corpse" )]
	public class ArcaneDaemon : BaseCreature
	{
		public override WeaponAbility GetWeaponAbility()
		{
			return WeaponAbility.ConcussionBlow;
		}

		[Constructable]
		public ArcaneDaemon() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "an arcane daemon";
			Body = 0x310;
			BaseSoundID = 0x47D;

            SpellCastAnimation = 13;
            SpellCastFrameCount = 7;

			SetStr( 400, 405 );
			SetDex( 80, 85 );
			SetInt( 500, 600 );

			SetHits( 400, 500 );

			SetDamage( 15, 25 );

            VirtualArmor = 10;

            SetSkill( SkillName.Tactics, 100.0, 100.0);
            			
            SetSkill( SkillName.EvalInt, 90.0, 95.0);
			SetSkill( SkillName.Magery, 90.0, 95.0 );           
			SetSkill( SkillName.Meditation, 65.0, 70.0 );

            SetSkill( SkillName.MagicResist, 90.0, 95.0);

            SetSkill( SkillName.Wrestling, 70.0, 75.0);

			Fame = 7000;
			Karma = -10000;			
		}

		public override void GenerateLoot()
		{
            base.PackAddGoldTier(6);
            base.PackAddArtifactChanceTier(6);           
            base.PackAddMapChanceTier(6);

            base.PackAddScrollTier(4);
            base.PackAddReagentTier(2);

            base.PackAddArtifactChanceTier(4);

            PackItem(new Longsword());
		}

		public override Poison PoisonImmune{ get{ return Poison.Deadly; } }

		public ArcaneDaemon( Serial serial ) : base( serial )
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