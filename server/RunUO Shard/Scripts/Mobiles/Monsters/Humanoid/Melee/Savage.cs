using System;
using Server;
using Server.Misc;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a savage corpse" )]
	public class Savage : BaseCreature
	{
		[Constructable]
		public Savage() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = NameList.RandomName( "savage" );

            if (Utility.RandomBool())
            {
                Body = 184;

                Item leatherskirt = new LeatherSkirt();
                leatherskirt.Movable = false;
                AddItem(leatherskirt);
            }

            else
            {
                Body = 183;

                Item leatherskirt = new LeatherSkirt();
                leatherskirt.Movable = false;
                AddItem(leatherskirt);
            }

            SetStr(125, 130);
            SetDex(50, 55);
            SetInt(25, 30);

            SetHits(150, 175);            

            SetDamage(5, 10);

            VirtualArmor = 5;

            SetSkill(SkillName.Tactics, 100.0, 100.0);            

            SetSkill(SkillName.MagicResist, 25.0, 30.0);

            SetSkill(SkillName.Archery, 60.0, 65.0);
            SetSkill(SkillName.Fencing, 60.0, 65.0);
            SetSkill(SkillName.Macing, 60.0, 65.0);
            SetSkill(SkillName.Wrestling, 60.0, 65.0);

			Fame = 1000;
			Karma = -1000;

            switch (Utility.Random(4))
            {
                case 0: AddItem(new Spear()); break;
                case 1: AddItem(new ShortSpear()); break;
                case 2: AddItem(new Club()); break;
                case 3:
                {
                    AddItem(new Bow()); 
                    AddItem(new Arrow(20));

                    break;
                }
            }

            AddItem(new BoneHelm());
		}		 
		
		public override void GenerateLoot()
		{
            base.PackAddGoldTier(3);
            base.PackAddArtifactChanceTier(3);
            base.PackAddMapChanceTier(3);

            base.PackAddRandomPotionTier(1);

            if (0.05 > Utility.RandomDouble())
                PackItem(new TribalSpear());

            if (0.1 > Utility.RandomDouble())
                PackItem(new TribalBerry()); 
		}

		public override int Meat{ get{ return 1; } }
		public override bool AlwaysMurderer{ get{ return true; } }
		public override bool ShowFameTitle{ get{ return false; } }

		public override OppositionGroup OppositionGroup
		{
			get{ return OppositionGroup.SavagesAndOrcs; }
		}

		public override bool IsEnemy( Mobile m )
		{
			if ( m.BodyMod == 183 || m.BodyMod == 184 )
				return false;

			return base.IsEnemy( m );
		}

		public override void AggressiveAction( Mobile aggressor, bool criminal )
		{
			base.AggressiveAction( aggressor, criminal );

			if ( aggressor.HueMod == 0 )
			{
				AOS.Damage( aggressor, 50, 0, 100, 0, 0, 0 );
				aggressor.BodyMod = 0;
				aggressor.HueMod = -1;
				aggressor.FixedParticles( 0x36BD, 20, 10, 5044, EffectLayer.Head );
				aggressor.PlaySound( 0x307 );
				aggressor.SendLocalizedMessage( 1040008 ); // Your skin is scorched as the tribal paint burns away!

			}
		}

		public override void AlterMeleeDamageTo( Mobile to, ref int damage )
		{
			if ( to is Dragon || to is WhiteWyrm || to is SwampDragon || to is Drake || to is Nightmare || to is Hiryu || to is LesserHiryu || to is Daemon )
				damage *= 3;
		}

		public Savage( Serial serial ) : base( serial )
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