using System;
using System.Collections;
using Server.Items;
using Server.Targeting;


namespace Server.Mobiles
{
	[CorpseName( "a rotting corpse" )]
	public class Zombie : BaseCreature
	{
		[Constructable]
		public Zombie() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a zombie";
			Body = 3;
			BaseSoundID = 471;

			SetStr( 46, 70 );
			SetDex( 31, 50 );
			SetInt( 26, 40 );

			SetHits( 125, 150 );

			SetDamage( 3, 6 );

            VirtualArmor = 5;

            SetSkill(SkillName.Tactics, 100.0, 100.0);

			SetSkill( SkillName.MagicResist, 15.0, 20.0 );
			SetSkill( SkillName.Wrestling, 5.0, 10.0 );

			Fame = 600;
			Karma = -600;			
			
			switch ( Utility.Random( 10 ))
			{
				case 0: PackItem( new LeftArm() ); break;
				case 1: PackItem( new RightArm() ); break;
				case 2: PackItem( new Torso() ); break;
				case 3: PackItem( new Bone() ); break;
				case 4: PackItem( new RibCage() ); break;
				case 5: PackItem( new RibCage() ); break;
				case 6: PackItem( new BonePile() ); break;
				case 7: PackItem( new BonePile() ); break;
				case 8: PackItem( new BonePile() ); break;
				case 9: PackItem( new BonePile() ); break;
			}

			switch (Utility.Random(3))
			{
				case 0: PackItem(new Bandage(1)); break;
				case 1: PackItem(new Bandage(2)); break;
				case 2: PackItem(new Bandage(3)); break;

			}

		}

		public override void GenerateLoot()
		{
            base.PackAddGoldTier(1);
            base.PackAddArtifactChanceTier(1);
            base.PackAddMapChanceTier(1);

            if (0.1 > Utility.RandomDouble())
                PackItem(new BoneDust());
		}

		public override bool BleedImmune{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Regular; } }

		public Zombie( Serial serial ) : base( serial )
		{
		}

		public override OppositionGroup OppositionGroup
		{
			get{ return OppositionGroup.FeyAndUndead; }
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


        public override void AggressiveAction(Mobile aggressor, bool criminal)
        {
            base.AggressiveAction(aggressor, criminal);

            if (aggressor.HueMod == 1882)
            {
                AOS.Damage(aggressor, 50, 0, 100, 0, 0, 0);
                aggressor.BodyMod = 0;
                aggressor.HueMod = -1;
                aggressor.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);
                aggressor.PlaySound(0x307);
                aggressor.SendMessage("Your skin is scorched as the undead paint burns away!"); // Your skin is scorched as the tribal paint burns away!
            }
        }
	}
}