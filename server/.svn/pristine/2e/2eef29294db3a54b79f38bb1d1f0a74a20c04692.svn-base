using System;
using Server;
using Server.Misc;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a savage corpse" )]
	public class SavageRider : BaseCreature
	{
		[Constructable]
		public SavageRider() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.25, 0.4 )
		{
			Name = NameList.RandomName( "savage rider" );

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

            SetHits(225, 250);

            SetDamage(7, 14);

            VirtualArmor = 10;

            SetSkill( SkillName.Tactics, 100.0, 100.0 );

            SetSkill( SkillName.MagicResist, 25.0, 30.0 );

            SetSkill( SkillName.Archery, 65.0, 70.0 );
            SetSkill( SkillName.Fencing, 65.0, 70.0 );
            SetSkill( SkillName.Wrestling, 65.0, 70.0 );

			Fame = 2500;
			Karma = -2500;

            switch (Utility.Random(3))
            {
                case 0: AddItem(new Spear()); break;
                case 1: AddItem(new ShortSpear()); break;                
                case 2:
                    {
                        AddItem(new Bow());
                        AddItem(new Arrow(20));

                        break;
                    }
            }

            AddItem(new BoneGloves());
            AddItem(new BoneHelm());			

			new SavageRidgeback().Rider = this;
		}		 
		
		public override void GenerateLoot()
		{
            base.PackAddGoldTier(4);
            base.PackAddArtifactChanceTier(4);
            base.PackAddMapChanceTier(4);

            base.PackAddRandomPotionTier(3);

            if (0.1 > Utility.RandomDouble())
                PackItem(new TribalSpear());

            if (0.15 > Utility.RandomDouble())
                PackItem(new TribalBerry()); 
		}

		public override int Meat{ get{ return 1; } }
		public override bool AlwaysMurderer{ get{ return true; } }
		public override bool ShowFameTitle{ get{ return false; } }

		public override OppositionGroup OppositionGroup
		{
			get{ return OppositionGroup.SavagesAndOrcs; }
		}

		public override bool OnBeforeDeath()
		{
			IMount mount = this.Mount;

			if ( mount != null )
				mount.Rider = null;

			if ( mount is Mobile )
				((Mobile)mount).Delete();

			return base.OnBeforeDeath();
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

		public SavageRider( Serial serial ) : base( serial )
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