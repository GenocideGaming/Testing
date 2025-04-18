using System;
using System.Collections.Generic;
using Server.Misc;
using Server.Network;
using Server.Mobiles;
using Server.Scripts;
using Server.Scripts.Custom.BountySystem;
using Server.Scripts.Custom.WebService;

namespace Server.Gumps
{
	public class ReportMurdererGump : Gump
	{
        public ReportMurdererGump(Mobile victum, List<Mobile> killers)
            : this(victum, killers, 0)
        {
        }

        private ReportMurdererGump(Mobile victum, List<Mobile> killers, int idx)
            : base(0, 0)
        {
            m_Killers = killers;
            m_Victum = victum;
            m_Idx = idx;
        }

		private int m_Idx;
		private List<Mobile> m_Killers;
		private Mobile m_Victum;

		public static void Initialize()
		{
			EventSink.PlayerDeath += new PlayerDeathEventHandler( EventSink_PlayerDeath );
            PacketHandlers.Register(0xAC, 0, true, new OnPacketReceive(BountyEntryResponse));
        }

		public static void EventSink_PlayerDeath( PlayerDeathEventArgs e )
		{
			Mobile m = e.Mobile;

			List<Mobile> killers = new List<Mobile>();
			List<Mobile> toGive  = new List<Mobile>();

			foreach ( AggressorInfo ai in m.Aggressors )
			{
				if ( ai.Attacker.Player && ai.CanReportMurder && !ai.Reported )
				{
				    killers.Add(ai.Attacker);
				    ai.Reported = true;
				    ai.CanReportMurder = false;
				}

				if ( ai.Attacker.Player && (DateTime.Now - ai.LastCombatTime) < TimeSpan.FromSeconds( 30.0 ) && !toGive.Contains( ai.Attacker ) )
					toGive.Add( ai.Attacker );
			}

			foreach ( AggressorInfo ai in m.Aggressed )
			{
				if ( ai.Defender.Player && (DateTime.Now - ai.LastCombatTime) < TimeSpan.FromSeconds( 30.0 ) && !toGive.Contains( ai.Defender ) )
					toGive.Add( ai.Defender );
			}

			foreach ( Mobile g in toGive )
			{
				int n = Notoriety.Compute( g, m );

				int theirKarma = m.Karma, ourKarma = g.Karma;
				bool innocent = ( n == Notoriety.Innocent );
				bool criminal = ( n == Notoriety.Criminal || n == Notoriety.Murderer );

				int fameAward = m.Fame / 200;
				int karmaAward = 0;

				if ( innocent )
					karmaAward = ( ourKarma > -2500 ? -850 : -110 - (m.Karma / 100) );
				else if ( criminal )
					karmaAward = 50;

				Titles.AwardFame( g, fameAward, false );
				Titles.AwardKarma( g, karmaAward, true );
                // modification to support XmlQuest Killtasks
                //Server.Items.XmlQuest.RegisterKill(m, g); // Not here, otherwise if multiple people are attacking, it muffs up who really killed them, b/c this actually goes through all the XmlDeathActions!
			}

			if ( m is PlayerMobile && m.Skills[SkillName.Stealing].Value > 30.0)
				return;

			if ( killers.Count > 0 )
				new GumpTimer( m, killers ).Start();
		}

        private static bool InAmnestyRegion(Mobile slain)
        {
            if (slain.Region.IsPartOf("Roache"))
                return true;

            if (slain.Region.IsPartOf("OrcFort"))
                return true;

            if (slain.Region.IsPartOf("OrcCompound"))
                return true;

            if (slain.Region.IsPartOf("PirateShip"))
                return true;
            
            if (slain.Region.IsPartOf("DrowCave"))
                return true;

            if (slain.Region.IsPartOf("UndeadCave"))
                return true;


            return false;
        }

		private class GumpTimer : Timer
		{
			private Mobile m_Victim;
			private List<Mobile> m_Killers;

			public GumpTimer( Mobile victim, List<Mobile> killers ) : base( TimeSpan.FromSeconds( 3.0 ) )
			{
				m_Victim = victim;
				m_Killers = killers;
			}

			protected override void OnTick()
			{
                foreach (Mobile killer in m_Killers)
                {
                    Item[] goldInBank = m_Victim.BankBox.FindItemsByType(typeof(Server.Items.Gold), true);

                    int total = 0;
                    for (int i = 0; i < goldInBank.Length; i++)
                        total += goldInBank[i].Amount;

                    if (total > FeatureList.MurderSystem.MaxVictimBounty)
                        total = FeatureList.MurderSystem.MaxVictimBounty;

                    m_Victim.Send(new BountyGump(killer, total));
                }
			}
		}

        private class BountyGump : Packet
        {
            public BountyGump(Mobile killer, int maxGold)
                : base(0xAB) // <- Standard text entry gump, see: http://wolfpack.berlios.de/index.php/UO_Protocol_0xAB
            {
                string prompt = String.Format("Do you wish to place a bounty on the head of {0}?", killer.Name);
                string subText = String.Format("({0}gp max.)", maxGold);

                EnsureCapacity(1 + 2 + 4 + 1 + 1 + 2 + prompt.Length + 1 + 1 + 1 + 4 + 2 + subText.Length + 1);

                m_Stream.Write((int)killer.Serial); // id  
                m_Stream.Write((byte)0); // 'typeid'
                m_Stream.Write((byte)0); // 'index'

                m_Stream.Write((short)(prompt.Length + 1)); // textlen 
                m_Stream.WriteAsciiNull(prompt);

                m_Stream.Write((bool)true); // enable cancel btn
                m_Stream.Write((byte)2); // style, 0=disable, 1=normal, 2=numeric
                m_Stream.Write((int)maxGold); // 'format' when style=1 format=maxlen, style=2 format=max # val

                m_Stream.Write((short)(subText.Length + 1));
                m_Stream.WriteAsciiNull(subText);
            }
        }

        private static void BountyEntryResponse(NetState ns, PacketReader pvSrc)
        {
            Mobile killerMobile = World.FindMobile((Serial)pvSrc.ReadInt32());
            Mobile victim = ns.Mobile;

            if (!(killerMobile is PlayerMobile))
                return;
            PlayerMobile killer = (PlayerMobile)killerMobile;
            
            if (victim == null)
                return;
            
            byte typeid = pvSrc.ReadByte();
            byte index = pvSrc.ReadByte();
            bool cancel = pvSrc.ReadByte() == 0;
            short responseLength = pvSrc.ReadInt16();
            string resp = pvSrc.ReadString();

            if (killer != null && !cancel)
            {
                int bounty = Utility.ToInt32(resp);

                if (!InAmnestyRegion(victim))
                {
                    if (bounty > FeatureList.MurderSystem.MaxVictimBounty)
                        bounty = FeatureList.MurderSystem.MaxVictimBounty;

                    bounty = victim.BankBox.ConsumeUpTo(typeof(Server.Items.Gold), bounty, true);

                    killer.AutomatedBounty += FeatureList.MurderSystem.AutomaticBountyPerKill;
                    killer.PlayerBounty += bounty;
                }
                else if (bounty > 0)
                {
                    victim.SendMessage("You may not place bounties in this area. " +
                                       "Your gold has remained in your bank box.");
                }

                killer.ResetKillTime();

                killer.Kills++;

                if (killer.Kills >= 5)
                    BountyRegistry.Add(killer, killer.Bounty, DateTime.Now);

                if (bounty > 0)
                    killer.SendAsciiMessage("{0} has placed a bounty of {1}gp on your head!", victim.Name, bounty);
                else
                    killer.SendLocalizedMessage(1049067);//You have been reported for murder!

                if (killer.Kills == 5)
                {
                    killer.SendLocalizedMessage(502134);//You are now known as a murderer!
                    killer.PlayerMurdererStatus = PlayerMobile.MurdererStatus.None;
                    killer.Delta(MobileDelta.Noto);
                    killer.InvalidateProperties();
                }

            }
        }
	}
}