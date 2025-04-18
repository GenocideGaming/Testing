using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a blood elemental corpse" )]
	public class BloodElemental : BaseCreature
	{
		[Constructable]
		public BloodElemental () : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a blood elemental";
			Body = 16;
			BaseSoundID = 278;
            Hue = 33;

            SpellCastAnimation = 3;
            SpellCastFrameCount = 6;

			SetStr( 495, 500 );
			SetDex( 75, 80 );
			SetInt( 800, 1000 );

			SetHits( 700, 800 );
            SetMana( 800, 1000 );

			SetDamage( 20, 40 );

            VirtualArmor = 5;

            SetSkill( SkillName.Tactics, 100.0, 100.0);

			SetSkill( SkillName.EvalInt, 70.0, 75.0 );
			SetSkill( SkillName.Magery, 70.0, 75.0 );
            SetSkill( SkillName.Meditation, 100.0, 100.0);

			SetSkill( SkillName.MagicResist, 95.0, 100.0 );

			SetSkill( SkillName.Wrestling, 65.0, 70.0 );

			Fame = 12500;
			Karma = -12500;

            CanSwim = true;
		}	 
		
		public override void GenerateLoot()
		{
            base.PackAddGoldTier(8);
            base.PackAddArtifactChanceTier(6);
            base.PackAddMapChanceTier(6);

            base.PackAddScrollTier(5);

            PackItem(new Bloodmoss(30));            
		}		

		public BloodElemental( Serial serial ) : base( serial )
		{
		}
        
        public override bool BleedImmune { get { return true; } }        

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