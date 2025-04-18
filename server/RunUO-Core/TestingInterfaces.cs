using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server
{
    public interface IMap
    {
        int GetAverageZ(int x, int y);
        int GetAverageZ(int x, int y, ref int z, ref int avg, ref int top);

        Map Internal { get; }
    }
   public class TestableMapAdapter : IMap
   {
        public virtual Map Internal
        {
            get { return new Map(0, 0, 0, 1, 1, 0, "Internal", MapRules.FeluccaRules); }
        }
        public virtual int GetAverageZ(int x, int y)
        {
            return 0;
        }
        public virtual int GetAverageZ(int x, int y, ref int z, ref int avg, ref int top)
        {
            return 0;
        }

   }
}
