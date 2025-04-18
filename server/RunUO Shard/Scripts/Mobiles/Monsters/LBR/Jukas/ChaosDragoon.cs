using System;
using Server;
using Server.Misc;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a chaos dragoon corpse" )]
	public class ChaosDragoon : BaseCreature
	{
		[Constructable]
		public ChaosDragoon() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.15, 0.4 )
		{
			Name = "a chaos dragoon";
			Body = 0x190;
			Hue = Utility.RandomSkinHue();

			SetStr( 176, 225 );
			SetDex( 81, 95 );
			SetInt( 61, 85 );

			SetHits( 275, 300 );

            SetDamage(5, 10); //Uses Weapon

            VirtualArmor = 60;

            
            
            

            SetSkill(SkillName.Tactics, 100.0, 100.0); //Uses Weapon
			
			SetSkill( SkillName.Healing, 60.3, 90.0 );		
			SetSkill( SkillName.MagicResist, 65.0, 70 );
			SetSkill( SkillName.Swords, 80, 85 );
            SetSkill(SkillName.Macing, 80, 85);
            SetSkill(SkillName.Fencing, 80, 85);

			Fame = 5000;
			Karma = -5000;

			CraftResource res = CraftResource.None;

			switch (Utility.Random( 6 ))
			{
				case 0: res = CraftResource.BlackScales; break;
				case 1: res = CraftResource.RedScales; break;
				case 2: res = CraftResource.BlueScales; break;
				case 3: res = CraftResource.YellowScales; break;
				case 4: res = CraftResource.GreenScales; break;
				case 5: res = CraftResource.WhiteScales; break;
			}

			BaseWeapon melee = null;

			switch (Utility.Random( 3 ))
			{
				case 0: melee = new Kryss(); break;
				case 1: melee = new Broadsword(); break;
				case 2: melee = new Katana(); break;
			}

			melee.Movable = false;
			AddItem( melee );

			DragonHelm helm = new DragonHelm();
			helm.Resource = res;
			helm.Movable = false;
			AddItem( helm );

			DragonChest chest = new DragonChest();
			chest.Resource = res;
			chest.Movable = false;
			AddItem( chest );

			DragonArms arms = new DragonArms();
			arms.Resource = res;
			arms.Movable = false;
			AddItem( arms );

			DragonGloves gloves = new DragonGloves();
			gloves.Resource = res;
			gloves.Movable = false;
			AddItem( gloves );

			DragonLegs legs = new DragonLegs();
			legs.Resource = res;
			legs.Movable = false;
			AddItem( legs );

			ChaosShield shield = new ChaosShield();
			shield.Movable = false;
			AddItem( shield );

			AddItem( new Shirt() );
			AddItem( new Boots() );

			int amount = Utility.RandomMinMax( 1, 3 );

		

			new SwampDragon().Rider = this;
		}

		public override int GetIdleSound()
		{
			return 0x2CE;
		}

		public override int GetDeathSound()
		{
			return 0x2CC;
		}

		public override int GetHurtSound()
		{
			return 0x2D1;
		}

		public override int GetAttackSound()
		{
			return 0x2C8;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich );
			//AddLoot( LootPack.Gems );	
		}

		public override bool HasBreath{ get{ return true; } }
		public override bool AutoDispel{ get{ return true; } }
		public override bool BardImmune{ get{ return !Core.AOS; } }
		public override bool CanRummageCorpses{ get{ return true; } }
		public override bool AlwaysMurderer{ get{ return true; } }
		public override bool ShowFameTitle{ get{ return false; } }

		public override bool OnBeforeDeath()
		{
			IMount mount = this.Mount;

			if ( mount != null )
				mount.Rider = null;

			return base.OnBeforeDeath();
		}

		public override void AlterMeleeDamageTo( Mobile to, ref int damage )
		{
			if ( to is Dragon || to is WhiteWyrm || to is SwampDragon || to is Drake || to is Nightmare || to is Hiryu || to is LesserHiryu || to is Daemon )
				damage *= 3;
		}

		public ChaosDragoon( Serial serial ) : base( serial )
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
