using System;
using System.Collections.Generic;
using Server;
using Server.Misc;
using Server.Items;
using Server.Gumps;

namespace Server.Mobiles
{
	public abstract class BaseHealer : BaseVendor
	{
		private List<SBInfo> m_SBInfos = new List<SBInfo>();
		protected override List<SBInfo> SBInfos{ get { return m_SBInfos; } }

		public override bool IsActiveVendor{ get{ return false; } }
		public override bool IsInvulnerable{ get{ return false; } }

		public override void InitSBInfo()
		{
		}

		public BaseHealer() : base( null )
		{
			if ( !IsInvulnerable )
			{
				AI = AIType.AI_Mage;
				ActiveSpeed = 0.2;
				PassiveSpeed = 0.8;
				RangePerception = BaseCreature.DefaultRangePerception;
				FightMode = FightMode.Aggressor;
			}

			SpeechHue = 0;

			SetStr( 304, 400 );
			SetDex( 102, 150 );
			SetInt( 204, 300 );

            SetHits( 600, 700);
            SetMana( 900, 1000);

			SetDamage( 10, 15 );

            VirtualArmor = 0;

            SetSkill( SkillName.Tactics, 100, 100.0 );

			SetSkill( SkillName.Anatomy, 30.0, 40.0 );			
			SetSkill( SkillName.Healing, 30.0, 40.0 );

            SetSkill( SkillName.EvalInt, 95.0, 100.0);
			SetSkill( SkillName.Magery, 95.0, 100.0 );
			SetSkill( SkillName.MagicResist, 100.0, 100.0 );

            SetSkill(SkillName.Wrestling, 90.0, 95.0);
            SetSkill(SkillName.Macing, 90.0, 95.0);				

			Fame = 1000;
			Karma = 10000;           			
		}        

		public override VendorShoeType ShoeType{ get{ return VendorShoeType.Sandals; } }

		public virtual int GetRobeColor()
		{
			return Utility.RandomYellowHue();
		}

		public override void InitOutfit()
		{
			base.InitOutfit();

			AddItem( new Robe( GetRobeColor() ) );
		}

		public virtual bool HealsYoungPlayers{ get{ return true; } }

		public virtual bool CheckResurrect( Mobile m )
		{
			return true;
		}

		private DateTime m_NextResurrect;
		private static TimeSpan ResurrectDelay = TimeSpan.FromSeconds( 2.0 );

		public virtual void OfferResurrection( Mobile m )
		{
			Direction = GetDirectionTo( m );

			m.PlaySound(0x1F2);
			m.FixedEffect( 0x376A, 10, 16 );

			m.CloseGump( typeof( ResurrectGump ) );
			m.SendGump( new ResurrectGump( m, ResurrectMessage.Healer ) );
		}

		public virtual void OfferHeal( PlayerMobile m )
		{
			Direction = GetDirectionTo( m );

			Say( 501228 ); // I can do no more for you at this time.
		}

		public override void OnMovement( Mobile m, Point3D oldLocation )
		{
			if ( !m.Frozen && DateTime.Now >= m_NextResurrect && InRange( m, 4 ) && !InRange( oldLocation, 4 ) && InLOS( m ) )
			{
				if ( !m.Alive )
				{
					m_NextResurrect = DateTime.Now + ResurrectDelay;

					if ( m.Map == null || !m.Map.CanFit( m.Location, 16, false, false ) )
					{
						m.SendLocalizedMessage( 502391 ); // Thou can not be resurrected there!
					}
					else if ( CheckResurrect( m ) )
					{
						OfferResurrection( m );
					}
				}
				
			}
		}

		public BaseHealer( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			if ( !IsInvulnerable )
			{
				AI = AIType.AI_Mage;
				ActiveSpeed = 0.2;
				PassiveSpeed = 0.8;
				RangePerception = BaseCreature.DefaultRangePerception;
				FightMode = FightMode.Aggressor;
			}
		}
	}
}