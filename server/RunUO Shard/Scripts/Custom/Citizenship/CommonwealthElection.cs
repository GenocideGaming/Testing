using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using Server.Mobiles;
using Server.Commands;
using Server.Targeting;
using Server.Factions;

namespace Server.Scripts.Custom.Citizenship
{
    public class CommonwealthElection
    {
        public static readonly TimeSpan PendingPeriod = TimeSpan.FromDays(FeatureList.Citizenship.PendingTimeInDays);
		public static readonly TimeSpan CampaignPeriod = TimeSpan.FromHours(FeatureList.Citizenship.CampaigningTimeInHours);
		public static readonly TimeSpan VotingPeriod = TimeSpan.FromHours(FeatureList.Citizenship.VotingTimeInHours);

		public const int MaxCandidates = 10;
		public const int CandidateRank = 5;

		private ICommonwealth mCommonwealth;
		private List<Candidate> mCandidates;

		private ElectionState mState;
		private DateTime mLastStateTime;

		public ICommonwealth MyCommonwealth{ get{ return mCommonwealth; } }

		public List<Candidate> Candidates { get { return mCandidates; } }

		public ElectionState State{ get{ return mState; } set{ mState = value; mLastStateTime = DateTime.Now; } }
		public DateTime LastStateTime{ get{ return mLastStateTime; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public ElectionState CurrentState{ get{ return mState; } }

		[CommandProperty( AccessLevel.GameMaster, AccessLevel.Administrator )]
		public TimeSpan NextStateTime
		{
			get
			{
				TimeSpan period;

				switch ( mState )
				{
					default:
					case ElectionState.Pending: period = PendingPeriod; break;
					case ElectionState.Election: period = VotingPeriod; break;
					case ElectionState.Campaign: period = CampaignPeriod; break;
				}

				TimeSpan until = (mLastStateTime + period) - DateTime.Now;

				if ( until < TimeSpan.Zero )
					until = TimeSpan.Zero;

				return until;
			}
			set
			{
				TimeSpan period;

				switch ( mState )
				{
					default:
					case ElectionState.Pending: period = PendingPeriod; break;
					case ElectionState.Election: period = VotingPeriod; break;
					case ElectionState.Campaign: period = CampaignPeriod; break;
				}

				mLastStateTime = DateTime.Now - period + value;
			}
		}

		private Timer mTimer;

		public void StartTimer()
		{
			mTimer = Timer.DelayCall( TimeSpan.FromMinutes( 1.5 ), TimeSpan.FromMinutes( 1.0 ), new TimerCallback( Slice ) );
		}

		public CommonwealthElection( Commonwealth commonwealth )
		{
			mCommonwealth = commonwealth;
			mCandidates = new List<Candidate>();
            State = ElectionState.Campaign;
			
		}

		public CommonwealthElection( GenericReader reader )
		{
			int version = reader.ReadEncodedInt();

			switch ( version )
			{
				case 0:
				{
					mCommonwealth = Commonwealth.ReadReference( reader );

					mLastStateTime = reader.ReadDateTime();
					mState = (ElectionState)reader.ReadEncodedInt();

					mCandidates = new List<Candidate>();

					int count = reader.ReadEncodedInt();

					for ( int i = 0; i < count; ++i )
					{
						Candidate cd = new Candidate( reader );

						if ( cd.Mobile != null )
							mCandidates.Add( cd );
					}

					break;
				}
			}

			StartTimer();
		}

		public void Serialize( GenericWriter writer )
		{
			writer.WriteEncodedInt( (int) 0 ); // version

			Commonwealth.WriteReference( writer, mCommonwealth );
			writer.Write( (DateTime) mLastStateTime );
			writer.WriteEncodedInt( (int) mState );

			writer.WriteEncodedInt( mCandidates.Count );

			for ( int i = 0; i < mCandidates.Count; ++i )
				mCandidates[i].Serialize( writer );
		}

		public void AddCandidate( Mobile mob )
		{
            if ( !IsViableCandidate(mob))
            {
                mob.SendMessage("You must be a citizen of {0} to stand for election here!", this.MyCommonwealth.Definition.TownName.ToString());
                return;
            }

			if ( IsAlreadyACandidate( mob ) )
				return;

			mCandidates.Add( new Candidate( mob ) );
            mob.SendMessage("You are now running for minister of " + mCommonwealth.Definition.TownName); // You are now running for office.
		}

        private bool IsViableCandidate(Mobile mob)
        {
            ICommonwealth playerCommonwealth = Commonwealth.Find(mob);
            if (playerCommonwealth == null)
                return false;

            if (playerCommonwealth != null && !playerCommonwealth.Equals(this.MyCommonwealth))
                return false;

            return true;
        }
        public void AddVoter(Mobile mob, Candidate cd)
        {
            if (mState == ElectionState.Election)
            {
                cd.Voters.Add(new Voter(mob, cd.Mobile));
            }
        }
		public void RemoveVoter( Mobile mob )
		{
			if ( mState == ElectionState.Election )
			{
				for ( int i = 0; i < mCandidates.Count; ++i )
				{
					List<Voter> voters = mCandidates[i].Voters;

					for ( int j = 0; j < voters.Count; ++j )
					{
						Voter voter = voters[j];

						if ( voter.From == mob )
							voters.RemoveAt( j-- );
					}
				}
			}
		}

		public void RemoveCandidate( Mobile mob )
		{
			Candidate cd = FindCandidate( mob );

			if ( cd == null )
				return;

			mCandidates.Remove( cd );
			mob.SendMessage("You have been removed as a candidate for minister of your township"); //i think this is the right message to send

			if ( mState == ElectionState.Election )
			{
				if ( mCandidates.Count == 1 )
				{
					mCommonwealth.Broadcast("There is only a single candidate left for minister of your township."); 

					Candidate winner = mCandidates[0];

					Mobile winMob = winner.Mobile;
					PlayerCitizenshipState pcs = PlayerCitizenshipState.Find( winMob );

					if ( pcs == null || pcs.Commonwealth != mCommonwealth || winMob == mCommonwealth.Minister )
					{
						mCommonwealth.Broadcast("Township leader has not changed");
					}
					else
					{
						mCommonwealth.Broadcast("Your township has a new minister!"); 
						mCommonwealth.Minister = winMob;
					}

					mCandidates.Clear();
					State = ElectionState.Pending;
				}
				else if ( mCandidates.Count == 0 ) // well, I guess this'll never happen
				{
					mCommonwealth.Broadcast("There are no longer any candidates for minister of your township. Oh no!"); 

					mCandidates.Clear();
					State = ElectionState.Pending;
				}
			}
		}

		public bool IsAlreadyACandidate( Mobile mob )
		{
			return ( FindCandidate( mob ) != null );
		}

		public bool CanVote( Mobile mob )
		{
            PlayerCitizenshipState pcs = PlayerCitizenshipState.Find(mob);

			return ( mState == ElectionState.Election && !HasVoted( mob ) && pcs != null && pcs.Commonwealth == mCommonwealth);
		}

		public bool HasVoted( Mobile mob )
		{
			return ( FindVoter( mob ) != null );
		}

		public Candidate FindCandidate( Mobile mob )
		{
			for ( int i = 0; i < mCandidates.Count; ++i )
			{
				if ( mCandidates[i].Mobile == mob )
					return mCandidates[i];
			}

			return null;
		}

		public Candidate FindVoter( Mobile mob )
		{
			for ( int i = 0; i < mCandidates.Count; ++i )
			{
				List<Voter> voters = mCandidates[i].Voters;

				for ( int j = 0; j < voters.Count; ++j )
				{
					Voter voter = voters[j];

					if ( voter.From == mob )
						return mCandidates[i];
				}
			}

			return null;
		}

		public bool CanBeCandidate( Mobile mob )
		{
			if ( IsAlreadyACandidate( mob ) )
				return false;

			if ( mCandidates.Count >= MaxCandidates )
				return false;

			if ( mState != ElectionState.Campaign )
				return false; // sanity..

			PlayerCitizenshipState pcs = PlayerCitizenshipState.Find( mob );

			return ( pcs != null && pcs.Commonwealth == mCommonwealth);
		}

		public void Slice()
		{
			if ( mCommonwealth.Election != this )
			{
				if ( mTimer != null )
					mTimer.Stop();

				mTimer = null;

				return;
			}

			switch ( mState )
			{
				case ElectionState.Pending:
				{
					if ( (mLastStateTime + PendingPeriod) > DateTime.Now )
						break;

					mCommonwealth.Broadcast("Campaigning for minister election has begun.");

					mCandidates.Clear();
					State = ElectionState.Campaign;

					break;
				}
				case ElectionState.Campaign:
				{
					if ( (mLastStateTime + CampaignPeriod) > DateTime.Now )
						break;

					if ( mCandidates.Count == 0 )
					{
						mCommonwealth.Broadcast("Nobody ran for the office of mayor for your township,"); // Nobody ran for office.
						State = ElectionState.Pending;
					}
					else if ( mCandidates.Count == 1 )
					{
						mCommonwealth.Broadcast("There was only one player who ran for the office of mayor,"); // Only one member ran for office.

						Candidate winner = mCandidates[0];

						Mobile mob = winner.Mobile;
						PlayerCitizenshipState pcs = PlayerCitizenshipState.Find( mob );

						if ( pcs == null || pcs.Commonwealth != mCommonwealth || mob == mCommonwealth.Minister )
						{
							mCommonwealth.Broadcast("Township leader has not changed, "+ mob.Name + " will rule for another term!"); // MyCommonwealth leadership has not changed.
						}
						else
						{
							mCommonwealth.Broadcast("Your township has a new minister, all hail "+ mob.Name); // The MyCommonwealth has a new minister.
							mCommonwealth.Minister = mob;
						}

						mCandidates.Clear();
                        mCommonwealth.OnGovernmentChange();
						State = ElectionState.Pending;
					}
					else
					{
						mCommonwealth.Broadcast("Voting for your township's minister has begun!");
						State = ElectionState.Election;
					}

					break;
				}
				case ElectionState.Election:
				{
					if ( (mLastStateTime + VotingPeriod) > DateTime.Now )
						break;

					mCommonwealth.Broadcast("The results from your township's minister election are in."); 

					Candidate winner = null;

					for ( int i = 0; i < mCandidates.Count; ++i )
					{
						Candidate cd = mCandidates[i];

						PlayerCitizenshipState pl = PlayerCitizenshipState.Find( cd.Mobile );

						if ( pl == null || pl.Commonwealth!= mCommonwealth )
							continue;

						//cd.CleanMuleVotes();

						if ( winner == null || cd.Votes > winner.Votes )
							winner = cd;
					}

					if ( winner == null )
					{
						mCommonwealth.Broadcast("Your township's leader has not changed, "+ mCommonwealth.Minister.Name + " will rule for another term."); 
					}
					else if ( winner.Mobile == mCommonwealth.Minister)
					{
                        mCommonwealth.Broadcast("Your township's leader has not changed, " + mCommonwealth.Minister.Name + " will rule for another term.");
					}
					else
					{
						mCommonwealth.Broadcast("Your township has a new minister, all hail "+ winner.Mobile.Name); // The faction has a new commander.
						mCommonwealth.Minister = winner.Mobile;
					}

					mCandidates.Clear();
                    mCommonwealth.OnGovernmentChange();
					State = ElectionState.Pending;

					break;
				}
			}
		}
        public static void Initialize()
        {
            CommandSystem.Register("ManageElection", AccessLevel.Administrator, new CommandEventHandler(OnCommand_ManageElection));
        }
        public static void OnCommand_ManageElection(CommandEventArgs e)
        {
            e.Mobile.Target = new TownStoneTarget();
        }

        private class TownStoneTarget : Target
        {
            public TownStoneTarget() : base(10, false, TargetFlags.None) { }
            protected override void OnTarget(Mobile from, object targeted)
            {
                base.OnTarget(from, targeted);
                if (targeted is CommonwealthStone)
                {
                    CommonwealthStone stone = targeted as CommonwealthStone;
                    from.SendGump(new ElectionManagementGump(stone.MyCommonwealth.Election));
                }
                else
                {
                    from.SendMessage("Please target a town stone to manage an election");
                }
            }
        }
    }
    public class Candidate
	{
		private Mobile m_Mobile;
		private List<Voter> m_Voters;

		public Mobile Mobile{ get{ return m_Mobile; } }
		public List<Voter> Voters { get { return m_Voters; } }

		public int Votes{ get{ return m_Voters.Count; } }

		public void CleanMuleVotes()
		{
			for ( int i = 0; i < m_Voters.Count; ++i )
			{
				Voter voter = (Voter)m_Voters[i];

				if ( (int)voter.AcquireFields()[3] < 90 )
					m_Voters.RemoveAt( i-- );
			}
		}

		public Candidate( Mobile mob )
		{
			m_Mobile = mob;
			m_Voters = new List<Voter>();
		}

		public Candidate( GenericReader reader )
		{
			int version = reader.ReadEncodedInt();

			switch ( version )
			{
				case 1:
				{
					m_Mobile = reader.ReadMobile();

					int count = reader.ReadEncodedInt();
					m_Voters = new List<Voter>( count );

					for ( int i = 0; i < count; ++i )
					{
						Voter voter = new Voter( reader, m_Mobile );

						if ( voter.From != null )
							m_Voters.Add( voter );
					}

					break;
				}
				case 0:
				{
					m_Mobile = reader.ReadMobile();

					List<Mobile> mobs = reader.ReadStrongMobileList();
					m_Voters = new List<Voter>( mobs.Count );

					for ( int i = 0; i < mobs.Count; ++i )
						m_Voters.Add( new Voter( mobs[i], m_Mobile ) );

					break;
				}
			}
		}

		public void Serialize( GenericWriter writer )
		{
			writer.WriteEncodedInt( (int) 1 ); // version

			writer.Write( (Mobile) m_Mobile );

			writer.WriteEncodedInt( (int) m_Voters.Count );

			for ( int i = 0; i < m_Voters.Count; ++i )
				((Voter)m_Voters[i]).Serialize( writer );
		}
	}

    public class Voter
    {
        private Mobile m_From;
        private Mobile m_Candidate;

        private IPAddress m_Address;
        private DateTime m_Time;

        public Mobile From
        {
            get { return m_From; }
        }

        public Mobile Candidate
        {
            get { return m_Candidate; }
        }

        public IPAddress Address
        {
            get { return m_Address; }
        }

        public DateTime Time
        {
            get { return m_Time; }
        }

        public object[] AcquireFields()
        {
            TimeSpan gameTime = TimeSpan.Zero;

            if (m_From is PlayerMobile)
                gameTime = ((PlayerMobile)m_From).GameTime;

            int kp = 0;

            PlayerCitizenshipState pl = PlayerCitizenshipState.Find(m_From);

            //if (pl != null)
            //    kp = pl.KillPoints; removed these for now since we don't have factions implemented correctly

            int sk = m_From.Skills.Total;

            int factorSkills = 50 + ((sk * 100) / 10000);
            int factorKillPts = 100 + (kp * 2);
            int factorGameTime = 50 + (int)((gameTime.Ticks * 100) / TimeSpan.TicksPerDay);

            int totalFactor = (factorSkills * factorKillPts * Math.Max(factorGameTime, 100)) / 10000;

            if (totalFactor > 100)
                totalFactor = 100;
            else if (totalFactor < 0)
                totalFactor = 0;

            return new object[] { m_From, m_Address, m_Time, totalFactor };
        }

        public Voter(Mobile from, Mobile candidate)
        {
            m_From = from;
            m_Candidate = candidate;

            if (m_From.NetState != null)
                m_Address = m_From.NetState.Address;
            else
                m_Address = IPAddress.None;

            m_Time = DateTime.Now;
        }

        public Voter(GenericReader reader, Mobile candidate)
        {
            m_Candidate = candidate;

            int version = reader.ReadEncodedInt();

            switch (version)
            {
                case 0:
                    {
                        m_From = reader.ReadMobile();
                        m_Address = Utility.Intern(reader.ReadIPAddress());
                        m_Time = reader.ReadDateTime();

                        break;
                    }
            }
        }

        public void Serialize(GenericWriter writer)
        {
            writer.WriteEncodedInt((int)0);

            writer.Write((Mobile)m_From);
            writer.Write((IPAddress)m_Address);
            writer.Write((DateTime)m_Time);
        }
    }

	public enum ElectionState
	{
		Pending,
		Campaign,
		Election
	}

}
