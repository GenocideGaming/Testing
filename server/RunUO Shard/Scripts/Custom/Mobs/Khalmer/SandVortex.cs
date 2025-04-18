using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a sand vortex corpse" )]
	public class SandVortex : BaseCreature
	{
		[Constructable]
		public SandVortex() : base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a sand vortex";
			Body = 790;
			BaseSoundID = 263;                        

			SetStr( 96, 120 );
			SetDex( 171, 195 );
			SetInt( 76, 100 );

			SetHits( 100, 125 );

			SetDamage( 8, 14 );

            VirtualArmor = 5;
            
            SetSkill(SkillName.Tactics, 100.0, 100.0);

            SetSkill(SkillName.MagicResist, 95.0, 100.0);

            SetSkill(SkillName.Wrestling, 85.0, 90.0);

			Fame = 4500;
			Karma = -4500;
		}

        public override void GenerateLoot()
        {
            base.PackAddGoldTier(4);
            base.PackAddArtifactChanceTier(4);
            base.PackAddMapChanceTier(4);

            base.PackAddGemTier(2);

            PackItem(new IronOre());
        }

		private DateTime m_NextAttack;

		public override void OnActionCombat()
		{
			Mobile combatant = Combatant;

			if ( combatant == null || combatant.Deleted || combatant.Map != Map || !InRange( combatant, 12 ) || !CanBeHarmful( combatant ) || !InLOS( combatant ) )
				return;

			if ( DateTime.Now >= m_NextAttack )
			{
				SandAttack( combatant );
				m_NextAttack = DateTime.Now + TimeSpan.FromSeconds( 10.0 + (10.0 * Utility.RandomDouble()) );
			}
		}

		public void SandAttack( Mobile m )
		{
			DoHarmful( m );

			m.FixedParticles( 0x36B0, 10, 25, 9540, 2413, 0, EffectLayer.Waist );

			new InternalTimer( m, this ).Start();
		}

		private class InternalTimer : Timer
		{
			private Mobile m_Mobile, m_From;

			public InternalTimer( Mobile m, Mobile from ) : base( TimeSpan.FromSeconds( 1.0 ) )
			{
				m_Mobile = m;
				m_From = from;
				Priority = TimerPriority.TwoFiftyMS;
			}

			protected override void OnTick()
			{
				m_Mobile.PlaySound( 0x4CF );
				AOS.Damage( m_Mobile, m_From, Utility.RandomMinMax( 15, 30 ), 90, 10, 0, 0, 0 );
			}
		}

		public SandVortex( Serial serial ) : base( serial )
		{
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