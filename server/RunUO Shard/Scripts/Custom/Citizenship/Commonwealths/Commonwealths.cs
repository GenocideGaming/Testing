using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Scripts.Custom.Citizenship.Commonwealths
{
    public class BlackrockCommonwealth : Commonwealth
    {
        private static Commonwealth mInstance;

        public static Commonwealth Instance { get { return mInstance; } }

        public override Point3D TownStoneLocation { get { return new Point3D(6491, 859, 0); } }

        public override Point3D[] StrongholdDestructableDoorLocations
        {
            get
            {
                //first two are linked hero doors (capture), last two are the cell doors (rescue)
                return new Point3D[] { new Point3D(618, 1677, 0), new Point3D(619, 1677, 0), new Point3D(608, 1689, 0), new Point3D(608, 1684, 0) };
            }
        }

        public override Point3D HeroGoLocation
        {
            get
            {
                return new Point3D(619, 1673, 0);
            }
        }

        public override Point3D[] CapturedHeroGoLocations
        {
            get
            {
                return new Point3D[] { new Point3D(605, 1688, 0), new Point3D(605, 1683, 0) };
            }
        }
        public BlackrockCommonwealth()
        {
            mInstance = this;

            Definition = new CommonwealthDefinition(2, "Blackrock", "Blackrock");
        }
    }
    public class TrinsicCommonwealth : Commonwealth
    {
        private static Commonwealth mInstance;

        public static Commonwealth Instance { get { return mInstance; } }

        public override Point3D TownStoneLocation { get { return new Point3D(1905, 2721, 20); } }

        public override Point3D[] StrongholdDestructableDoorLocations
        {
            get
            {
                //first two are linked hero doors (capture), last two are the cell doors (rescue)
                return new Point3D[] { new Point3D(2003, 2726, 30), new Point3D(2004, 2726, 30), new Point3D(2015, 2723, 30), new Point3D(2015, 2717, 30) };
            }
        }

        public override Point3D HeroGoLocation
        {
            get
            {
                return new Point3D(2003, 2719, 30);
            }
        }

        public override Point3D[] CapturedHeroGoLocations
        {
            get
            {
                return new Point3D[] { new Point3D(2012, 2723, 30), new Point3D(2012, 2717, 30) };
            }
        }
        public TrinsicCommonwealth()
        {
            mInstance = this;

            Definition = new CommonwealthDefinition(1, "Trinsic", "Trinsic");
        }
    }
    public class YewCommonwealth : Commonwealth
    {
        private static Commonwealth mInstance;

        public static Commonwealth Instance { get { return mInstance; } }

        public override Point3D TownStoneLocation { get { return new Point3D(534, 991, 0); } }

        public override Point3D[] StrongholdDestructableDoorLocations
        {
            get
            {
                //first two are linked hero doors (capture), last two are the cell doors (rescue)
                return new Point3D[] { new Point3D(947, 710, 0), new Point3D(948, 710, 0), new Point3D(959, 706, 0), new Point3D(959, 700, 0) };
            }
        }

        public override Point3D HeroGoLocation
        {
            get
            {
                return new Point3D(948, 703, 0);
            }
        }

        public override Point3D[] CapturedHeroGoLocations
        {
            get
            {
                return new Point3D[] { new Point3D(956, 706, 0), new Point3D(956, 699, 0) };
            }
        }
        public YewCommonwealth()
        {
            mInstance = this;

            Definition = new CommonwealthDefinition(0, "Yew", "Yew");
        }
    }
}
