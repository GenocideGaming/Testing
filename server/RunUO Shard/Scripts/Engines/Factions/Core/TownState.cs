using System;
using Server.Scripts.Custom.Citizenship;

namespace Server.Factions
{
	public class TownState
	{
		private Town m_Town;
		private Faction m_Militia;
        private ICommonwealth mCommonwealth;

        private Mobile mMinister;

		private int m_Silver;

		private DateTime m_LastIncome;

		public Town Town
		{
			get{ return m_Town; }
			set{ m_Town = value; }
		}
        public ICommonwealth MyCommonwealth { get { return mCommonwealth; } set { mCommonwealth = value; } }
		public Faction Militia
		{
			get{ return m_Militia; }
			set{ m_Militia = value; }
		}

        public Mobile Minister 
        {
            get { return mMinister; }
            set
            {
                if (mMinister != null)
                {
                    PlayerCitizenshipState pl = PlayerCitizenshipState.Find(mMinister);

                    if (pl != null)
                        pl.Mayor = null;
                }
                mMinister = value;

                if (mMinister != null)
                {
                    PlayerCitizenshipState pl = PlayerCitizenshipState.Find(mMinister);
                    if (pl != null)
                        pl.Mayor = m_Town;
                }
            }
        }

		public int Silver
		{
			get{ return m_Silver; }
			set{ m_Silver = value; }
		}


		public DateTime LastIncome
		{
			get{ return m_LastIncome; }
			set{ m_LastIncome = value; }
		}

		public TownState( Town town )
		{
			m_Town = town;
		}

		public TownState( GenericReader reader )
		{
			int version = reader.ReadEncodedInt();

			switch ( version )
			{
                case 4:
                {
                    //added commonwealth/citizenship stuff
                    mCommonwealth = Commonwealth.ReadReference(reader);
                    goto case 3;
                }
				case 3:
				{
					m_LastIncome = reader.ReadDateTime();

					goto case 2;
				}
				case 2:
				{
					goto case 1;
				}
				case 1:
				{
					m_Silver = reader.ReadEncodedInt();

					goto case 0;
				}
				case 0:
				{
					m_Town = Town.ReadReference( reader );
					m_Militia = Faction.ReadReference( reader );
                   
                    mMinister = reader.ReadMobile();

					m_Town.State = this;

					break;
				}
			}
		}

		public void Serialize( GenericWriter writer )
		{
			writer.WriteEncodedInt( (int) 4 ); // version

            Commonwealth.WriteReference(writer, mCommonwealth); //citizenship stuff

			writer.Write( (DateTime) m_LastIncome );
			writer.WriteEncodedInt( (int) m_Silver );

			Town.WriteReference( writer, m_Town );
			Faction.WriteReference( writer, m_Militia );

           
            writer.Write((Mobile)mMinister);
		}
	}
}