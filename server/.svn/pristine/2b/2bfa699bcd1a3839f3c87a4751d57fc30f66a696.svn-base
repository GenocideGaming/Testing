using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Scripts.Custom
{
    public class CommonwealthDefinition
    {
        private int mSortingIndex; //WTF does this do?
        private string mRegion;
        private TextDefinition mTownName;

        public int SortingIndex { get { return mSortingIndex; } }
        public string Region { get { return mRegion; } }
        public TextDefinition TownName { get { return mTownName; } }

        public CommonwealthDefinition(int sort, string region, string townName)
        {
            mSortingIndex = sort;
            mRegion = region;
            mTownName = new TextDefinition(townName);
        }

    }
}
