using System;
using Server.Misc;
using Server.Network;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	public class KhaldunZealot : BaseCreature
	{
		public override bool ClickTitle{ get{ return false; } }
		public override bool ShowFameTitle{ get{ return false; } }

		[Constructable]
		public KhaldunZealot(): base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Body = 0x190;
			Name = "Zealot of Khaldun";
			Title = "the Knight";
			Hue = 0;

			SetStr( 351, 400 );
			SetDex( 151, 165 );
			SetInt( 76, 100 );

			SetHits( 600, 750 );

			SetDamage( 3, 5 ); //Uses Weapon

            VirtualArmor = 30;

            
            
            

            SetSkill(SkillName.Tactics, 100.0, 100.0); //Uses Weapon		
			
			SetSkill( SkillName.MagicResist, 70.0, 75.0 );
            SetSkill(SkillName.Swords, 95.0, 100.0);
            SetSkill(SkillName.Wrestling, 85.0, 90.0);

			Fame = 10000;
			Karma = -10000;			

			VikingSword weapon = new VikingSword();
			weapon.Hue = 0x835;
			weapon.Movable = false;
			AddItem( weapon );

			MetalShield shield = new MetalShield();
			shield.Hue = 0x835;
			shield.Movable = false;
			AddItem( shield );

			BoneHelm helm = new BoneHelm();
			helm.Hue = 0x835;
			AddItem( helm );

			BoneArms arms = new BoneArms();
			arms.Hue = 0x835;
			AddItem( arms );

			BoneGloves gloves = new BoneGloves();
			gloves.Hue = 0x835;
			AddItem( gloves );

			BoneChest tunic = new BoneChest();
			tunic.Hue = 0x835;
			AddItem( tunic );

			BoneLegs legs = new BoneLegs();
			legs.Hue = 0x835;
			AddItem( legs );

			AddItem( new Boots() );
		}

		public override int GetIdleSound()
		{
			return 0x184;
		}

		public override int GetAngerSound()
		{
			return 0x286;
		}

		public override int GetDeathSound()
		{
			return 0x288;
		}

		public override int GetHurtSound()
		{
			return 0x19F;
		}

		public override bool AlwaysMurderer{ get{ return true; } }
		public override bool Unprovokable{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Deadly; } }

		public KhaldunZealot( Serial serial ) : base( serial )
		{
		}

		public override bool OnBeforeDeath()
		{
			BoneKnight rm = new BoneKnight();
			rm.Team = this.Team;
			rm.Combatant = this.Combatant;
			rm.NoKillAwards = true;

			if ( rm.Backpack == null )
			{
				Backpack pack = new Backpack();
				pack.Movable = false;
				rm.AddItem( pack );
			}

			for ( int i = 0; i < 2; i++ )
			{
				LootPack.FilthyRich.Generate( this, rm.Backpack, true, 0 );
				LootPack.FilthyRich.Generate( this, rm.Backpack, false, 0 );
			}

			Effects.PlaySound(this, Map, GetDeathSound());
			Effects.SendLocationEffect( Location, Map, 0x3709, 30, 10, 0x835, 0 );
			rm.MoveToWorld( Location, Map );

			Delete();
			return false;
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}