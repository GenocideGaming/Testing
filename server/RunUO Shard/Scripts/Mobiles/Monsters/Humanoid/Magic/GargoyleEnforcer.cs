using System;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a gargoyle corpse" )]
	public class GargoyleEnforcer : BaseCreature
	{
		public override WeaponAbility GetWeaponAbility()
		{
			return WeaponAbility.WhirlwindAttack;
		}

		[Constructable]
		public GargoyleEnforcer() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Gargoyle Enforcer";
			Body = 0x2F2;
			BaseSoundID = 0x174;

            SetStr(350, 355);
            SetDex(40, 45);
            SetInt(600, 800);

            SetHits(300, 400);
            SetMana(600, 800);

            SetDamage(12, 20);

            VirtualArmor = 25;

            SetSkill(SkillName.Tactics, 100.0, 100.0);

            SetSkill(SkillName.EvalInt, 70.0, 75.0);
            SetSkill(SkillName.Magery, 70.0, 75.0);
            SetSkill(SkillName.Meditation, 100.0, 100.0);

            SetSkill(SkillName.MagicResist, 70.0, 75.0);

            SetSkill(SkillName.Wrestling, 80.0, 85.0);

			Fame = 5000;
			Karma = -5000;
        }        

        public override void GenerateLoot()
        {
            base.PackAddGoldTier(6);
            base.PackAddArtifactChanceTier(6);
            base.PackAddMapChanceTier(6);

            base.PackAddScrollTier(4);
            base.PackAddReagentTier(2);
            base.PackAddGemTier(2);
        }

		public override int Meat{ get{ return 1; } }

		public GargoyleEnforcer( Serial serial ) : base( serial )
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