using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a dread spider corpse" )]
	public class DreadSpider : BaseCreature
	{
		[Constructable]
		public DreadSpider () : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a dread spider";
			Body = 11;
			BaseSoundID = 1170;

			SetStr( 95, 100 );
			SetDex( 50, 55 );
			SetInt( 500, 600 );

			SetHits( 175, 200 );
            SetMana( 500, 600);

			SetDamage( 5, 10 );

            VirtualArmor = 10;

            SetSkill(SkillName.Tactics, 100.0, 100.0);

			SetSkill( SkillName.EvalInt, 65.0, 70.0 );
			SetSkill( SkillName.Magery, 65.0, 70.0 );
            SetSkill( SkillName.Meditation, 100.0, 100.0);

			SetSkill( SkillName.MagicResist, 65.0, 70.0 );

			SetSkill( SkillName.Wrestling, 60.0, 65.0 );

			Fame = 5000;
			Karma = -5000;

            Tamable = false;
            ControlSlots = 3;   //made it 3. Wyverns were 2, but I think these guys are more boss. 
            MinTameSkill = 96.0;
		}		 
		
		public override void GenerateLoot()
		{
            base.PackAddGoldTier(6);
            base.PackAddArtifactChanceTier(6);
            base.PackAddMapChanceTier(6);

            base.PackAddPoisonPotionTier(7);
            base.PackAddScrollTier(4);

            PackItem(new SpidersSilk(15));

            if (0.5 > Utility.RandomDouble())
                PackItem(new DrowGland());
		}

        public override Poison PoisonImmune { get { return Poison.Lethal; } }
		public override Poison HitPoison{ get{ return Poison.Deadly; } }		

		public DreadSpider( Serial serial ) : base( serial )
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

			if ( BaseSoundID == 263 )
				BaseSoundID = 1170;
		}
	}
}