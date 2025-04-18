using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a cyclopean corpse" )]
	public class Cyclops : BaseCreature
	{
		[Constructable]
		public Cyclops() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a cyclopean warrior";
			Body = 76;
			BaseSoundID = 604;

            SetStr(600, 605);
            SetDex(45, 50);
            SetInt(25, 30);

            SetHits(600, 700);            

            SetDamage(25, 40);

            VirtualArmor = 5;

            SetSkill(SkillName.Tactics, 100.0, 100.0);

            SetSkill(SkillName.MagicResist, 45.0, 50.0);

            SetSkill(SkillName.Wrestling, 50.0, 55.0);

			Fame = 4500;
			Karma = -4500;			
		}        

        public override void GenerateLoot()
        {
            base.PackAddGoldTier(5);
            base.PackAddArtifactChanceTier(5);
            base.PackAddMapChanceTier(5);

            base.PackAddGemTier(3);
        }

		public override int Meat{ get{ return 4; } }		

		public Cyclops( Serial serial ) : base( serial )
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