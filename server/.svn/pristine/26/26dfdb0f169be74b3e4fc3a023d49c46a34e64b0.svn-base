using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a gazer corpse" )]
	public class Gazer : BaseCreature
	{
		[Constructable]
		public Gazer () : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a gazer";
			Body = 22;
			BaseSoundID = 377;

            SpellCastAnimation = 6;
            SpellCastFrameCount = 4;

            SetStr(100, 125);
            SetDex(40, 45);
            SetInt(500, 600);

            SetHits(100, 125);
            SetMana(500, 600);

            SetDamage(4, 8);

            VirtualArmor = 5;

            SetSkill(SkillName.Tactics, 100.0, 100.0);

            SetSkill(SkillName.Meditation, 100.0, 100.0);
            SetSkill(SkillName.EvalInt, 60.0, 65.0);
            SetSkill(SkillName.Magery, 60.0, 65.0);

            SetSkill(SkillName.MagicResist, 60.0, 65.0);

            SetSkill(SkillName.Wrestling, 45.0, 50.0);

			Fame = 3500;
			Karma = -3500;			
		}       

        public override void GenerateLoot()
        {
            base.PackAddGoldTier(4);
            base.PackAddArtifactChanceTier(4);
            base.PackAddMapChanceTier(4);

            base.PackAddScrollTier(2);
            base.PackAddReagentTier(2);
        }
		
		public override int Meat{ get{ return 1; } }

		public Gazer( Serial serial ) : base( serial )
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