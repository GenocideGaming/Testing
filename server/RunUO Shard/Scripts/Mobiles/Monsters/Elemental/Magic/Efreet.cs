using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "an efreet corpse" )]
	public class Efreet : BaseCreature
	{
		[Constructable]
		public Efreet () : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "an efreet";
			Body = 15;
			BaseSoundID = 768;
            Hue = 39;

            SpellCastAnimation = 2;
            SpellCastFrameCount = 4;

			SetStr( 195, 200 );
			SetDex( 80, 85 );
			SetInt( 600, 700 );

			SetHits( 350, 375 );
            SetMana( 600, 700);

			SetDamage( 15, 25 );

            VirtualArmor = 5;

            SetSkill( SkillName.Tactics, 100.0, 100.0);

			SetSkill( SkillName.EvalInt, 80.0, 85.0 );
			SetSkill( SkillName.Magery, 80.0, 85.0 );
            SetSkill( SkillName.Meditation, 100.0, 100.0);

			SetSkill( SkillName.MagicResist, 95.0, 100.0 );            

            SetSkill(SkillName.Wrestling, 75.0, 80.0);

			Fame = 10000;
			Karma = -10000;
		}                

        public override void GenerateLoot()
        {
            base.PackAddGoldTier(6);
            base.PackAddArtifactChanceTier(6);
            base.PackAddMapChanceTier(6);

            base.PackAddScrollTier(4);

            PackItem(new SulfurousAsh(20));

            AddItem(new LightSource());
        }	

		public Efreet( Serial serial ) : base( serial )
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
