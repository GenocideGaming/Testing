using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	[CorpseName( "a sheep corpse" )]
	public class Sheep : BaseCreature, ICarvable
	{
		private DateTime m_NextWoolTime;

		[CommandProperty( AccessLevel.GameMaster )]
		public DateTime NextWoolTime
		{
			get{ return m_NextWoolTime; }
			set{ m_NextWoolTime = value; Body = ( DateTime.Now >= m_NextWoolTime ) ? 0xCF : 0xDF; }
		}

		public void Carve( Mobile from, Item item )
		{
			if ( DateTime.Now < m_NextWoolTime )
			{
				// This sheep is not yet ready to be shorn.
				PrivateOverheadMessage( MessageType.Regular, 0x3B2, 500449, from.NetState );
				return;
			}

			from.SendLocalizedMessage( 500452 ); // You place the gathered wool into your backpack.
			from.AddToBackpack( new Wool( 3 ) );

            PlayerMobile player = from as PlayerMobile;
            if (player != null)
                player.SheepShornThisSession++;

            NextWoolTime = DateTime.Now + TimeSpan.FromMinutes(5.0);
		}

		public override void OnThink()
		{
			base.OnThink();
			Body = ( DateTime.Now >= m_NextWoolTime ) ? 0xCF : 0xDF;
		}

		[Constructable]
		public Sheep() : base( AIType.AI_Generic, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			Name = "a sheep";
			Body = 0xCF;
			BaseSoundID = 0xD6;

			SetStr( 19 );
			SetDex( 25 );
			SetInt( 5 );

            SetHits(25, 35);
			SetMana( 0 );

            SetDamage(2, 4);

            VirtualArmor = 5;


            SetSkill(SkillName.Tactics, 100.0, 100.0);

			SetSkill( SkillName.MagicResist, 5.0 );
            SetSkill(SkillName.Wrestling, 20.0, 25.0);

			Fame = 300;
			Karma = 0;			

			Tamable = false;
			ControlSlots = 1;
			MinTameSkill = 11.1;
		}

		public override int Meat{ get{ return 3; } }
		public override MeatType MeatType{ get{ return MeatType.LambLeg; } }
		public override FoodType FavoriteFood{ get{ return FoodType.FruitsAndVegies | FoodType.GrainsAndHay; } }

		public override int Wool{ get{ return (Body == 0xCF ? 2 : 0); } }

		public Sheep( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 );

			writer.WriteDeltaTime( m_NextWoolTime );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 1:
				{
					NextWoolTime = reader.ReadDeltaTime();
					break;
				}
			}
		}
	}
}