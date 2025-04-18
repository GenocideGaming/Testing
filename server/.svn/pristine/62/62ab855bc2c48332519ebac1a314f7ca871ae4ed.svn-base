using System;

namespace Server.Factions
{
	public class TownDefinition
	{
		private int m_Sort;

		private string m_Region;

		private string m_FriendlyName;

		private TextDefinition m_TownName;
		private TextDefinition m_TownStoneHeader;
		private TextDefinition m_TownStoneName;

		private Point3D m_TownStone;

		public int Sort{ get{ return m_Sort; } }

		public string Region{ get{ return m_Region; } }
		public string FriendlyName{ get{ return m_FriendlyName; } }

		public TextDefinition TownName{ get{ return m_TownName; } }
		public TextDefinition TownStoneHeader{ get{ return m_TownStoneHeader; } }
		public TextDefinition TownStoneName{ get{ return m_TownStoneName; } }

		public Point3D TownStone{ get{ return m_TownStone; } }

		public TownDefinition( int sort, string region, string friendlyName, TextDefinition townName, TextDefinition townStoneHeader, TextDefinition townStoneName, Point3D townStone )
		{
			m_Sort = sort;
			m_Region = region;
			m_FriendlyName = friendlyName;
			m_TownName = townName;
			m_TownStoneHeader = townStoneHeader;
			m_TownStoneName = townStoneName;
			m_TownStone = townStone;
		}
	}
}