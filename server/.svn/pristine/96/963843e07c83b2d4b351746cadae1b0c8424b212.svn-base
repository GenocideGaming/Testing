using System;
using Server;
using Server.Items;
using Server.Factions;

namespace Server.Mobiles
{
	[CorpseName( "a daemon corpse" )]
	public class Daemon : BaseCreature
	{
		public override double DispelDifficulty{ get{ return 125.0; } }
		public override double DispelFocus{ get{ return 45.0; } }
		public override Ethics.Ethic EthicAllegiance { get { return Ethics.Ethic.Evil; } }

		[Constructable]
		public Daemon () : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = NameList.RandomName( "daemon" );
			Body = 9;
			BaseSoundID = 357;

            SpellCastAnimation = 12;
            SpellCastFrameCount = 7;

            SetStr(500, 550);
            SetDex(110, 115);
            SetInt(1000, 1200);

            SetHits(800, 900);
            SetMana(800, 1000);

            SetDamage(20, 30);

            VirtualArmor = 10;

            SetSkill(SkillName.Tactics, 100.0, 100.0);

            SetSkill(SkillName.EvalInt, 80.0, 85.0);
            SetSkill(SkillName.Meditation, 100.0, 100.0);
            SetSkill(SkillName.Magery, 80.0, 85.0);

            SetSkill(SkillName.MagicResist, 85.0, 90.0);

            SetSkill(SkillName.Wrestling, 85.0, 90.0);

			Fame = 15000;
			Karma = -15000;

			ControlSlots = 2;
		}		 
		
		public override void GenerateLoot()
		{
            base.PackAddGoldTier(8);
            base.PackAddArtifactChanceTier(8);
            base.PackAddMapChanceTier(8);

            base.PackAddScrollTier(6);
            base.PackAddReagentTier(3);

            base.PackAddArtifactChanceTier(6);

            PackItem(new Longsword());
		}

		public override bool CanRummageCorpses{ get{ return true; } }		
		
		public override int Meat{ get{ return 1; } }

		public Daemon( Serial serial ) : base( serial )
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
