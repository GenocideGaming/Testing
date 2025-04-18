using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "an elder gazer corpse" )]
	public class ElderGazer : BaseCreature
	{
		[Constructable]
		public ElderGazer () : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "an elder gazer";
			Body = 22;
			BaseSoundID = 377;
            Hue = 824;

            SpellCastAnimation = 6;
            SpellCastFrameCount = 4;

			SetStr( 200, 205 );
			SetDex( 40, 45 );
			SetInt( 1800, 2000 );

			SetHits( 800, 900 );
            SetMana( 1800, 2000 );

			SetDamage( 10, 16 );

            VirtualArmor = 5;

            SetSkill( SkillName.Tactics, 100.0, 100.0);

            SetSkill( SkillName.Meditation, 100.0, 105.0);			
			SetSkill( SkillName.EvalInt, 110.0, 115.0 );
			SetSkill( SkillName.Magery, 110.0, 115.0 );

			SetSkill( SkillName.MagicResist, 110.0, 115.0 );

			SetSkill( SkillName.Wrestling, 55.0, 60.0 );

			Fame = 12500;
			Karma = -12500;
        }        

        public override void GenerateLoot()
        {
            base.PackAddGoldTier(8);
            base.PackAddArtifactChanceTier(8);
            base.PackAddMapChanceTier(8);

            base.PackAddScrollTier(6); 
            base.PackAddReagentTier(5);            
        }

		public ElderGazer( Serial serial ) : base( serial )
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