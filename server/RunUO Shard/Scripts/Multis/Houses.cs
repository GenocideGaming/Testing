using System;
using System.Collections;
using System.Data;
using Server;
using Server.Items;
using Server.Multis.Deeds;

namespace Server.Multis
{
    public class Tent : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-3, -3, 7, 7), new Rectangle2D(-1, 4, 3, 1) };
        public Tent(Mobile owner, int id)
            : base(id, owner, 200, 1)
        {
            SetEastSign(3, 4, 1);
        }

        public Tent(Serial serial)
            : base(serial)
        {
        }

        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(3, 5, 0);
            }
        }

        public override int DefaultPrice
        {
            get
            {
                return 16666;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.TwoStoryFoundations[0];
            }
        }

        public override HouseDeed GetDeed()
        {
            switch (ItemID)
            {
                case 0x71:
                    return new BlueTentDeed();
                case 0x72:
                    return new GreenTentDeed();
                default:
                    return new BlueTentDeed();
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class SmallOldHouse : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-3, -3, 7, 7), new Rectangle2D(-1, 4, 3, 1) };
        public SmallOldHouse(Mobile owner, int id)
            : base(id, owner, 425, 3)
        {
            uint keyValue = CreateKeys(owner);

            AddSouthDoor(0, 3, 7, keyValue);

            SetSign(2, 4, 5);
        }

        public SmallOldHouse(Serial serial)
            : base(serial)
        {
        }

        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(2, 4, 0);
            }
        }
        public override int DefaultPrice
        {
            get
            {
                return 30000;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.TwoStoryFoundations[0];
            }
        }
        public override HouseDeed GetDeed()
        {
            switch (ItemID)
            {
                case 0x64:
                    return new StonePlasterHouseDeed();
                case 0x66:
                    return new FieldStoneHouseDeed();
                case 0x68:
                    return new SmallBrickHouseDeed();
                case 0x6A:
                    return new WoodHouseDeed();
                case 0x6C:
                    return new WoodPlasterHouseDeed();
                case 0x6E:
                default:
                    return new ThatchedRoofCottageDeed();
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class SmallOldHouseE : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-3, -3, 7, 7), new Rectangle2D(4, -1, 1, 3) };
        public SmallOldHouseE(Mobile owner, int id)
            : base(id, owner, 425, 3)
        {
            uint keyValue = CreateKeys(owner);

            AddEastDoor(true, 4, -2, 5, keyValue);

            SetEastSign(4, -2, 0);
        }

        public SmallOldHouseE(Serial serial)
            : base(serial)
        {
        }

        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(4, -2, 0);
            }
        }
        public override int DefaultPrice
        {
            get
            {
                return 30000;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.TwoStoryFoundations[0];
            }
        }
        public override HouseDeed GetDeed()
        {
            switch (ItemID)
            {
                case 0x15F:
                    return new FieldStoneHouseDeedE();
                case 0x160:
                    return new SmallBrickHouseDeedE();
                default:
                    return new FieldStoneHouseDeedE();
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class GuildHouse : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-7, -7, 14, 14), new Rectangle2D(-2, 7, 4, 1) };
        public GuildHouse(Mobile owner)
            : base(0x74, owner, 1100, 8)
        {
            uint keyValue = CreateKeys(owner);

            AddSouthDoors(3, 7, 5, keyValue);

            SetSign(4, 8, 16);

            AddSouthDoor(-3, -1, 7);
            AddSouthDoor(3, -1, 7);
        }

        public GuildHouse(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 283333;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.ThreeStoryFoundations[20];
            }
        }
        public override int ConvertOffsetX
        {
            get
            {
                return -1;
            }
        }
        public override int ConvertOffsetY
        {
            get
            {
                return -1;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(3, 7, 0);
            }
        }
        public override HouseDeed GetDeed()
        {
            return new BrickHouseDeed();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class TwoStoryHouse : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-7, 0, 14, 7), new Rectangle2D(-7, -7, 9, 7), new Rectangle2D(-4, 7, 4, 1) };
        public TwoStoryHouse(Mobile owner, int id)
            : base(id, owner, 1370, 10)
        {
            uint keyValue = CreateKeys(owner);

            AddSouthDoors(1, 7, 0, keyValue);

            SetSign(2, 8, 16);

            AddSouthDoor(-3, 0, 7);
            AddSouthDoor(id == 0x76 ? -2 : -3, 0, 27);
        }

        public TwoStoryHouse(Serial serial)
            : base(serial)
        {
        }

        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(1, 7, 0);
            }
        }
        public override int DefaultPrice
        {
            get
            {
                return 416666;
            }
        }
        public override HouseDeed GetDeed()
        {
            switch (ItemID)
            {
                case 0x76:
                    return new TwoStoryWoodPlasterHouseDeed();
                case 0x78:
                default:
                    return new TwoStoryStonePlasterHouseDeed();
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class Tower : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-7, -7, 16, 14), new Rectangle2D(-1, 7, 4, 2), new Rectangle2D(-11, 0, 4, 7), new Rectangle2D(9, 0, 4, 7) };
        public Tower(Mobile owner)
            : base(0x7A, owner, 2119, 15)
        {
            uint keyValue = CreateKeys(owner);

            AddSouthDoors(false, 0, 6, 6, keyValue);

            SetSign(4, 7, 5);

            AddSouthDoor(false, 3, -2, 6, keyValue);
            AddEastDoor(false, 1, 4, 26);
            AddEastDoor(false, 1, 4, 46);
        }

        public Tower(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 3000000;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.ThreeStoryFoundations[37];
            }
        }
        public override int ConvertOffsetY
        {
            get
            {
                return -1;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(4, 7, 0);
            }
        }
        public override HouseDeed GetDeed()
        {
            return new TowerDeed();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class TowerE : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-7, -7, 14, 16), new Rectangle2D(7, -1, 2, 4), new Rectangle2D(1, 9, 7, 4), new Rectangle2D(1, -11, 7, 4) };
        public TowerE(Mobile owner)
            : base(0xEB, owner, 2119, 15)
        {
            uint keyValue = CreateKeys(owner);

            AddEastDoors(false, 6, 0, 6, keyValue);

            SetEastSign(7, -1, 5);

            AddEastDoor(false, -3, 3, 5, keyValue);
            AddSouthDoor(false, 3, 1, 25);
            AddSouthDoor(false, 4, 1, 45);
        }

        public TowerE(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 3000000;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.ThreeStoryFoundations[37];
            }
        }
        public override int ConvertOffsetY
        {
            get
            {
                return -1;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(7, -2, 0);
            }
        }
        public override HouseDeed GetDeed()
        {
            return new TowerDeedE();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class Keep : BaseHouse//warning: ODD shape!
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-11, -11, 7, 8), new Rectangle2D(-11, 5, 7, 8), new Rectangle2D(6, -11, 7, 8), new Rectangle2D(6, 5, 7, 8), new Rectangle2D(-9, -3, 5, 8), new Rectangle2D(6, -3, 5, 8), new Rectangle2D(-4, -9, 10, 20), new Rectangle2D(-1, 11, 4, 1) };
        public Keep(Mobile owner)
            : base(0x81, owner, 2625, 18)
        {
            uint keyValue = CreateKeys(owner);

            AddSouthDoors(false, 0, 10, 6, keyValue);
            AddSouthDoor(false, -7, -2, 6, keyValue);
            AddEastDoor(false, -5, -7, 6, keyValue);
            AddEastDoors(false, -5, 1, 26);
            AddEastDoors(false, 6, 1, 26);

            SetSign(4, 11, 5);
        }

        public Keep(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 4000000;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(4, 11, 0);
            }
        }
        /*
        protected override bool IsInsideSpecial(Point3D p, StaticTile[] tiles)
        {
            return p.X >= X -3 && p.X <= X + 9 && p.Y >= Y -8 && p.Y <= Y + 4;
        }
        */
        public override HouseDeed GetDeed()
        {
            return new KeepDeed();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class Castle : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-15, -15, 31, 31), new Rectangle2D(-1, 16, 4, 1) };
        public Castle(Mobile owner)
            : base(0x7E, owner, 4076, 35)
        {
            uint keyValue = CreateKeys(owner);

            AddSouthDoors(false, 0, 15, 6, keyValue);

            SetSign(4, 16, 5);

            AddSouthDoors(false, 0, 11, 6, true);
            AddSouthDoors(false, 0, 5, 6, keyValue);
            AddSouthDoors(false, -1, -11, 6, keyValue);
        }

        public Castle(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 5000000;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(4, 16, 0);
            }
        }
        /*
        protected override bool IsInsideSpecial(Point3D p, StaticTile[] tiles)
        {
            return p.X >= X - 10 && p.X <= X + 10 && p.Y >= Y - 10 && p.Y <= Y + 10;
        }
        */
        public override HouseDeed GetDeed()
        {
            return new CastleDeed();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class LargePatioHouse : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-7, -7, 15, 14), new Rectangle2D(-5, 7, 4, 1) };
        public LargePatioHouse(Mobile owner)
            : base(0x8C, owner, 1100, 8)
        {
            uint keyValue = CreateKeys(owner);

            AddSouthDoors(-4, 6, 7, keyValue);

            SetSign(0, 7, 5);

            AddEastDoor(true, 1, 4, 7, keyValue);
            AddEastDoor(true, 1, -4, 7, keyValue);
            AddSouthDoor(4, -1, 7, keyValue);
        }

        public LargePatioHouse(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 333333;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.ThreeStoryFoundations[29];
            }
        }
        public override int ConvertOffsetY
        {
            get
            {
                return -1;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(0, 7, 0);
            }
        }
        public override HouseDeed GetDeed()
        {
            return new LargePatioDeed();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class LargeMarbleHouse : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-7, -7, 15, 14), new Rectangle2D(-6, 7, 6, 1) };
        public LargeMarbleHouse(Mobile owner)
            : base(0x96, owner, 1370, 10)
        {
            uint keyValue = CreateKeys(owner);

            AddSouthDoors(false, -4, 3, 4, keyValue);

            SetSign(0, 7, 0);
        }

        public LargeMarbleHouse(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 416666;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.ThreeStoryFoundations[29];
            }
        }
        public override int ConvertOffsetY
        {
            get
            {
                return -1;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(0, 7, 0);
            }
        }
        public override HouseDeed GetDeed()
        {
            return new LargeMarbleDeed();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class SmallTower : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-3, -3, 8, 7), new Rectangle2D(2, 4, 3, 1) };
        public SmallTower(Mobile owner)
            : base(0x98, owner, 580, 4)
        {
            uint keyValue = CreateKeys(owner);

            AddSouthDoor(false, 3, 3, 6, keyValue);

            SetSign(1, 4, 5);
        }

        public SmallTower(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 50000;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.TwoStoryFoundations[6];
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(1, 4, 0);
            }
        }
        public override HouseDeed GetDeed()
        {
            return new SmallTowerDeed();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class SmallTowerE : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-3, -3, 7, 8), new Rectangle2D(4, 0, 1, 3) };
        public SmallTowerE(Mobile owner)
            : base(0x13F, owner, 580, 4)
        {
            uint keyValue = CreateKeys(owner);

            AddEastDoor(false, 3, 1, 6, keyValue);

            SetEastSign(4, 0, 5);
        }

        public SmallTowerE(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 50000;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.TwoStoryFoundations[6];
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(4, -1, 0);
            }
        }
        public override HouseDeed GetDeed()
        {
            return new SmallTowerDeedE();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class LogCabin : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-3, -6, 8, 13) };
        public LogCabin(Mobile owner)
            : base(0x9A, owner, 1100, 8)
        {
            uint keyValue = CreateKeys(owner);

            AddSouthDoor(1, 4, 8, keyValue);

            SetSign(5, 8, 20);

            AddSouthDoor(1, 0, 29);
        }

        public LogCabin(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 150000;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.TwoStoryFoundations[12];
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(5, 8, 0);
            }
        }
        public override HouseDeed GetDeed()
        {
            return new LogCabinDeed();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class SandStonePatio : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-5, -4, 12, 8), new Rectangle2D(-2, 4, 3, 1) };
        public SandStonePatio(Mobile owner)
            : base(0x9C, owner, 850, 6)
        {
            uint keyValue = CreateKeys(owner);

            AddSouthDoor(-1, 3, 6, keyValue);

            SetSign(2, 4, 1);
        }

        public SandStonePatio(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 116666;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.TwoStoryFoundations[35];
            }
        }
        public override int ConvertOffsetY
        {
            get
            {
                return -1;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(2, 4, 0);
            }
        }
        public override HouseDeed GetDeed()
        {
            return new SandstonePatioDeed();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class TwoStoryVilla : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-5, -5, 11, 11), new Rectangle2D(2, 6, 4, 1) };
        public TwoStoryVilla(Mobile owner)
            : base(0x9E, owner, 1100, 8)
        {
            uint keyValue = CreateKeys(owner);

            AddSouthDoors(3, 1, 5, keyValue);

            SetSign(1, 6, 3);

            AddEastDoor(true, 1, 0, 25, keyValue);
            AddSouthDoor(-3, -1, 25);
        }

        public TwoStoryVilla(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 283333;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.TwoStoryFoundations[31];
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(1, 6, 0);
            }
        }
        public override HouseDeed GetDeed()
        {
            return new VillaDeed();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class SmallShop : BaseHouse
    {
        public static Rectangle2D[] AreaArray1 = new Rectangle2D[] { new Rectangle2D(-3, -3, 7, 7), new Rectangle2D(-1, 4, 4, 1) };
        public static Rectangle2D[] AreaArray2 = new Rectangle2D[] { new Rectangle2D(-3, -3, 7, 7), new Rectangle2D(-2, 4, 3, 1) };
        public SmallShop(Mobile owner, int id)
            : base(id, owner, 425, 3)
        {
            uint keyValue = CreateKeys(owner);

            BaseDoor door = MakeDoor(false, DoorFacing.EastCW);

            door.KeyValue = keyValue;

            if (door is BaseHouseDoor)
                ((BaseHouseDoor)door).Facing = DoorFacing.EastCCW;

            //AddDoor(door, -2, 0, id == 0xA2 ? 24 : 27);

            AddSouthDoor( false, -2, 0, 27 - (id == 0xA2 ? 3 : 0), keyValue );

            SetSign(3, 4, 7 - (id == 0xA2 ? 2 : 0));
        }

        public SmallShop(Serial serial)
            : base(serial)
        {
        }

        public override Rectangle2D[] Area
        {
            get
            {
                return (ItemID == 0x40A2 ? AreaArray1 : AreaArray2);
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(3, 4, 0);
            }
        }
        public override int DefaultPrice
        {
            get
            {
                return 40000;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.TwoStoryFoundations[0];
            }
        }
        public override HouseDeed GetDeed()
        {
            switch (ItemID)
            {
                case 0xA0:
                    return new StoneWorkshopDeed();
                case 0xA2:
                default:
                    return new MarbleWorkshopDeed();
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    //Phase 2 Housing
    //Blackrock Collection
    public class BlackrockSmallTowerE : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-4, -3, 8, 7), new Rectangle2D(4, 0, 1, 3) };
        public BlackrockSmallTowerE(Mobile owner)
            : base(0x104, owner, 580, 4)
        {
            uint keyValue = CreateKeys(owner);

            AddEastDoor(true, 3, 1, 5, keyValue);

            SetEastSign(4, 0, 5);
        }

        public BlackrockSmallTowerE(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 50000;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.TwoStoryFoundations[6];
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(4, -1, 0);
            }
        }
        public override HouseDeed GetDeed()
        {
            return new BlackrockSmallTowerDeedE();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class BlackrockScoutTower : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-3, -4, 8, 8), new Rectangle2D(0, 4, 3, 1) };
        public BlackrockScoutTower(Mobile owner)
            : base(0xF2, owner, 580, 4)
        {
            uint keyValue = CreateKeys(owner);

            AddSouthDoor(1, 3, 5, keyValue);

            SetSign(4, 4, 0);
        }

        public BlackrockScoutTower(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 50000;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.TwoStoryFoundations[6];
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(3, 4, 0);
            }
        }
        public override HouseDeed GetDeed()
        {
            return new BlackrockScoutTowerDeed();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class BlackrockTowerE : BaseHouse     // Blackrock Tower E has floating tile on 2nd floor and one of the doors opens but the house does not allow them to step on that tile due to wall blocking. 11-7-21

    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-5, -4, 9, 8), new Rectangle2D(4, -1, 1, 4) };
        public BlackrockTowerE(Mobile owner)
            : base(0xEF, owner, 715, 5)
        {
            uint keyValue = CreateKeys(owner);

            AddEastDoors(true, 3, 0, 4, keyValue);

            SetEastSign(4, 4, 4);
        }

        public BlackrockTowerE(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 83333;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.TwoStoryFoundations[6];
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(4, 3, 0);
            }
        }
        public override HouseDeed GetDeed()
        {
            return new BlackrockTowerDeedE();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class BlackrockOutpost : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-6, -5, 13, 10), new Rectangle2D(-1, 5, 4, 1) };
        public BlackrockOutpost(Mobile owner)
            : base(0xF1, owner, 1100, 8)
        {
            uint keyValue = CreateKeys(owner);

            AddSouthDoors(0, 4, 5, keyValue);

            SetSign(-1, 5, 5);
        }

        public BlackrockOutpost(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 183333;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.TwoStoryFoundations[35];
            }
        }
        public override int ConvertOffsetY
        {
            get
            {
                return -1;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(-2, 5, 0);
            }
        }
        public override HouseDeed GetDeed()
        {
            return new BlackrockOutpostDeed();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class BlackrockKeep : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-9, -5, 18, 11), new Rectangle2D(-1, 5, 4, 1) };
        public BlackrockKeep(Mobile owner)
            : base(0xF0, owner, 1775, 13)
        {
            uint keyValue = CreateKeys(owner);

            AddSouthDoors(-1, 4, 5, keyValue);

            SetSign(-2, 6, 0);
        }

        public BlackrockKeep(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 500000;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.ThreeStoryFoundations[37];
            }
        }
        public override int ConvertOffsetY
        {
            get
            {
                return -1;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(-2, 6, 0);
            }
        }
        public override HouseDeed GetDeed()
        {
            return new BlackrockKeepDeed();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class BlackrockFort : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-13, -11, 27, 23), new Rectangle2D(2, 12, 2, 1) };
        public BlackrockFort(Mobile owner)
            : base(0xF3, owner, 2320, 16)
        {
            uint keyValue = CreateKeys(owner);

            AddSouthDoors(2, 11, 5, keyValue);
            AddEastDoor(true, -6, 8, 5);
            AddSouthDoor(-9, -4, 5);
            AddSouthDoor(10, -4, 5);

            SetSign(-12, 12, 12);
        }

        public BlackrockFort(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 3000000;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(-12, 12, 0);
            }
        }
        public override HouseDeed GetDeed()
        {
            return new BlackrockFortDeed();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    //Skara Collection
    public class SkaraSmallShoppe : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-3, -5, 7, 10), new Rectangle2D(-1, 5, 3, 1), new Rectangle2D(4, 0, 1, 3), new Rectangle2D(4, -4, 1, 3) };
        public SkaraSmallShoppe(Mobile owner)
            : base(0x11B, owner, 580, 4)
        {
            uint keyValue = CreateKeys(owner);

            AddSouthDoor(0, 4, 5, keyValue);
            AddEastDoor(true, 1, 1, 5, keyValue);
            AddEastDoor(true, 1, -3, 5, keyValue);

            SetSign(-1, 5, 5);
        }

        public SkaraSmallShoppe(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 83333;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.TwoStoryFoundations[35];
            }
        }
        public override int ConvertOffsetY
        {
            get
            {
                return -1;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(-2, 5, 5);
            }
        }
        public override HouseDeed GetDeed()
        {
            return new SkaraSmallShoppeDeed();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class SkaraVerandaE : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-5, -5, 9, 10), new Rectangle2D(4, -1, 2, 4) };
        public SkaraVerandaE(Mobile owner)
            : base(0xF8, owner, 850, 6)
        {
            uint keyValue = CreateKeys(owner);

            AddEastDoor(true, 3, 0, 10, keyValue);

            SetSign(3, 5, 1);
        }

        public SkaraVerandaE(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 116666;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.TwoStoryFoundations[35];
            }
        }
        public override int ConvertOffsetY
        {
            get
            {
                return -1;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(3, 5, 0);
            }
        }
        public override HouseDeed GetDeed()
        {
            return new SkaraVerandaEDeed();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class SkaraSimpleHouse : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-7, -5, 13, 9), new Rectangle2D(-2, 4, 4, 2) };
        public SkaraSimpleHouse(Mobile owner)
            : base(0xF6, owner, 985, 7)
        {
            uint keyValue = CreateKeys(owner);

            AddSouthDoors(-1, 4, 5, keyValue);

            SetSign(-6, 4, 0);
        }

        public SkaraSimpleHouse(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 150000;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.TwoStoryFoundations[35];
            }
        }
        public override int ConvertOffsetY
        {
            get
            {
                return -1;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(-6, 4, 0);
            }
        }
        public override HouseDeed GetDeed()
        {
            return new SkaraSimpleHouseDeed();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class SkaraRanchHouseE : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-7, -7, 13, 7), new Rectangle2D(-6, -0, 8, 7), new Rectangle2D(6, -5, 1, 4) };
        public SkaraRanchHouseE(Mobile owner)
            : base(0x11C, owner, 1370, 10)
        {
            uint keyValue = CreateKeys(owner);

            AddEastDoors(true, 5, -4, 5, keyValue);
            AddSouthDoors(-3, -1, 5);

            SetEastSign(6, -6, 0);

        }

        public SkaraRanchHouseE(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 283333;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.TwoStoryFoundations[35];
            }
        }
        public override int ConvertOffsetY
        {
            get
            {
                return -1;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(6, -6, 0);
            }
        }
        public override HouseDeed GetDeed()
        {
            return new SkaraRanchHouseEDeed();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class SkaraAdobe : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-7, -5, 14, 9), new Rectangle2D(3, 4, 3, 1) };
        public SkaraAdobe(Mobile owner)
            : base(0x11E, owner, 1370, 10)
        {
            uint keyValue = CreateKeys(owner);

            AddSouthDoor(4, 3, 5, keyValue);
            AddSouthDoor(-2, 0, 25, keyValue);

            SetSign(3, 4, 5);
        }

        public SkaraAdobe(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 333333;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.TwoStoryFoundations[35];
            }
        }
        public override int ConvertOffsetY
        {
            get
            {
                return -1;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(2, 4, 0);
            }
        }
        public override HouseDeed GetDeed()
        {
            return new SkaraAdobeDeed();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class SkaraCourtyardShoppe : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-10, -7, 17, 14), new Rectangle2D(7, -6, 2, 3), new Rectangle2D(-6, 7, 3, 2) };
        public SkaraCourtyardShoppe(Mobile owner)
            : base(0x11A, owner, 1640, 12)
        {
            uint keyValue = CreateKeys(owner);

            AddSouthDoor(-5, 7, 5, keyValue);
            AddEastDoor(true, -4, 4, 5, keyValue);
            AddEastDoor(true, -4, 0, 5, keyValue);

            AddSouthDoor(0, -4, 5, keyValue);
            AddSouthDoor(4, -4, 5, keyValue);
            AddEastDoor(true, 7, -5, 5, keyValue);

            SetSign(-6, 8, 4);
        }

        public SkaraCourtyardShoppe(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 500000;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.TwoStoryFoundations[35];
            }
        }
        public override int ConvertOffsetY
        {
            get
            {
                return -1;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(-7, 8, 0);
            }
        }
        public override HouseDeed GetDeed()
        {
            return new SkaraCourtyardShoppeDeed();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class SkaraCourtyardHouse : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-10, -7, 17, 14), new Rectangle2D(7, -6, 2, 3), new Rectangle2D(-6, 7, 3, 2) };
        public SkaraCourtyardHouse(Mobile owner)
            : base(0x198, owner, 1640, 12)
        {
            uint keyValue = CreateKeys(owner);

            AddSouthDoor(-5, 7, 5, keyValue);
            AddSouthDoors(2, -2, 5, keyValue);
            AddEastDoor(true, 7, -5, 5, keyValue);
            AddSouthDoor(-5, 2, 25);

            SetSign(-6, 8, 4);
        }

        public SkaraCourtyardHouse(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 500000;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.TwoStoryFoundations[31];
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(-7, 8, 0);
            }
        }
        public override HouseDeed GetDeed()
        {
            return new SkaraCourtyardHouseDeed();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class SkaraManor : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-9, -9, 18, 16), new Rectangle2D(-6, 7, 4, 2) };
        public SkaraManor(Mobile owner)
            : base(0x107, owner, 1775, 13)
        {
            uint keyValue = CreateKeys(owner);

            AddSouthDoors(-5, 7, 5, keyValue);
            AddSouthDoors(0, 4, 1, keyValue);

            AddEastDoor(true, -2, 1, 5);
            AddEastDoor(3, 1, 5);

            AddEastDoor(-2, -4, 25, 0);
            AddEastDoor(3, -4, 25, 0);
            AddSouthDoors(6, 5, 25, keyValue);

            SetSign(-6, 8, 5);

        }

        public SkaraManor(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 550000;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.TwoStoryFoundations[31];
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(-7, 8, 0);
            }
        }
        public override HouseDeed GetDeed()
        {
            return new SkaraManorDeed();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class SkaraTwoStoryRanchHouse : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-7, -9, 9, 17), new Rectangle2D(2, -8, 5, 8), new Rectangle2D(3, 0, 4, 1) };
        public SkaraTwoStoryRanchHouse(Mobile owner)
            : base(0x10E, owner, 1950, 14)
        {
            uint keyValue = CreateKeys(owner);

            AddSouthDoors(4, -1, 5, keyValue);
            AddSouthDoors(-3, -1, 5);

            AddEastDoors(true, 1, -5, 5, keyValue);

            AddSouthDoors(-3, 4, 25, keyValue);
            AddEastDoors(true, 1, -5, 25);

            SetSign(2, 0, 3);
        }

        public SkaraTwoStoryRanchHouse(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 2666666;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.TwoStoryFoundations[35];
            }
        }
        public override int ConvertOffsetY
        {
            get
            {
                return -1;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(2, 0, 0);
            }
        }
        public override HouseDeed GetDeed()
        {
            return new SkaraTwoStoryRanchHouseDeed();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class SkaraVillaE : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-7, -2, 11, 9), new Rectangle2D(-7, -11, 15, 9), new Rectangle2D(5, 1, 1, 4), new Rectangle2D(-4, 7, 6, 1) };
        public SkaraVillaE(Mobile owner)
            : base(0x148, owner, 1950, 14)
        {
            uint keyValue = CreateKeys(owner);

            AddEastDoors(true, 4, 2, 5, keyValue);
            AddEastDoors(true, 1, -6, 25, keyValue);
            SetEastSign(5, 7, 0);
        }

        public SkaraVillaE(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 2666666;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.TwoStoryFoundations[35];
            }
        }
        public override int ConvertOffsetY
        {
            get
            {
                return -1;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(5, 7, 0);
            }
        }
        public override HouseDeed GetDeed()
        {
            return new SkaraVillaEDeed();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    //Trinsic Collection
    public class TrinsicShop : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-5, -6, 9, 11), new Rectangle2D(-2, 5, 3, 1) };
        public TrinsicShop(Mobile owner)
            : base(0xF5, owner, 850, 6)
        {
            uint keyValue = CreateKeys(owner);

            AddSouthDoor(-1, 4, 5, keyValue);

            SetSign(2, 5, 3);
        }

        public TrinsicShop(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 116666;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.TwoStoryFoundations[35];
            }
        }
        public override int ConvertOffsetY
        {
            get
            {
                return -1;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(2, 5, 0);
            }
        }
        public override HouseDeed GetDeed()
        {
            return new TrinsicShopDeed();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class TrinsicVilla : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-5, -5, 11, 11) };
        public TrinsicVilla(Mobile owner)
            : base(0xE2, owner, 1100, 8)
        {
            uint keyValue = CreateKeys(owner);

            AddSouthDoors(-1, 4, 5, keyValue);
            AddEastDoor(true, 3, 0, 25, keyValue);

            SetSign(-4, 6, 0);
        }

        public TrinsicVilla(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 216666;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.TwoStoryFoundations[35];
            }
        }
        public override int ConvertOffsetY
        {
            get
            {
                return -1;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(-4, 6, 0);
            }
        }
        public override HouseDeed GetDeed()
        {
            return new TrinsicVillaDeed();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class TrinsicPatioResidenceE : BaseHouse

    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-7, -5, 13, 9), new Rectangle2D(6, -2, 1, 4) };
        public TrinsicPatioResidenceE(Mobile owner)
            : base(0xE5, owner, 1235, 9)
        {
            uint keyValue = CreateKeys(owner);

            AddEastDoors(true, 5, -1, 4, keyValue);

            SetEastSign(6, -2, 4);
        }

        public TrinsicPatioResidenceE(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 250000;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.TwoStoryFoundations[6];
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(6, -3, 0);
            }
        }
        public override HouseDeed GetDeed()
        {
            return new TrinsicPatioResidenceDeedE();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class TrinsicPatioVilla : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-7, -5, 13, 10) };
        public TrinsicPatioVilla(Mobile owner)
            : base(0xE4, owner, 1235, 9)
        {
            uint keyValue = CreateKeys(owner);

            AddSouthDoor(-1, 3, 5, keyValue);
            AddEastDoors(true, 4, -1, 25);

            SetSign(-3, 5, 0);
        }

        public TrinsicPatioVilla(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 250000;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.TwoStoryFoundations[35];
            }
        }
        public override int ConvertOffsetY
        {
            get
            {
                return -1;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(-3, 5, 0);
            }
        }
        public override HouseDeed GetDeed()
        {
            return new TrinsicPatioVillaDeed();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class TrinsicFortifiedVillaE : BaseHouse

    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-7, -7, 14, 5), new Rectangle2D(-9, -2, 16, 8) };
        public TrinsicFortifiedVillaE(Mobile owner)
            : base(0xE6, owner, 1950, 14)
        {
            uint keyValue = CreateKeys(owner);

            AddEastDoor(true, 5, 2, 5, keyValue);
            AddSouthDoor(-1, -2, 5, keyValue);

            AddEastDoor(true, -1, 2, 25);
            AddSouthDoor(-5, -2, 25);

            SetEastSign(7, 0, 0);
        }

        public TrinsicFortifiedVillaE(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 2666666;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.TwoStoryFoundations[6];
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(7, 0, 0);
            }
        }
        public override HouseDeed GetDeed()
        {
            return new TrinsicFortifiedVillaDeedE();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class TrinsicCourtyardHouse : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-9, -8, 19, 15), new Rectangle2D(-7, 7, 8, 1), new Rectangle2D(-6, 8, 6, 1) };
        public TrinsicCourtyardHouse(Mobile owner)
            : base(0xDe, owner, 1950, 14)
        {
            uint keyValue = CreateKeys(owner);

            AddSouthDoors(-4, 6, 5, keyValue);
            AddEastDoor(true, 2, 3, 5, keyValue);
            AddSouthDoor(6, -1, 5, keyValue);

            SetSign(-8, 7, 0);

        }

        public TrinsicCourtyardHouse(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 2666666;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.ThreeStoryFoundations[31];
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(-8, 7, 0);
            }
        }
        public override HouseDeed GetDeed()
        {
            return new TrinsicCourtyardHouseDeed();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class TrinsicManor : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-10, -6, 20, 13) };
        public TrinsicManor(Mobile owner)
            : base(0xF4, owner, 2119, 15)
        {
            uint keyValue = CreateKeys(owner);

            AddSouthDoors(-1, 3, 5, keyValue);
            AddEastDoor(2, 1, 5);
            AddEastDoors(true, -3, 1, 25, keyValue);

            SetEastSign(10, 4, 0);
        }

        public TrinsicManor(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 3000000;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.ThreeStoryFoundations[37];
            }
        }
        public override int ConvertOffsetY
        {
            get
            {
                return -1;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(10, 4, 0);
            }
        }
        public override HouseDeed GetDeed()
        {
            return new TrinsicManorDeed();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class TrinsicKeepHome : BaseHouse//warning: ODD shape!
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-11, -11, 7, 8), new Rectangle2D(-11, 5, 7, 8), new Rectangle2D(6, -11, 7, 8), new Rectangle2D(6, 5, 7, 8), new Rectangle2D(-9, -3, 5, 8), new Rectangle2D(6, -3, 5, 8), new Rectangle2D(-4, -9, 10, 20), new Rectangle2D(-1, 11, 4, 1) };
        public TrinsicKeepHome(Mobile owner)
            : base(0x82, owner, 2625, 18)
        {
            uint keyValue = CreateKeys(owner);

            AddSouthDoors(false, 0, 10, 6, keyValue);
            AddSouthDoor(false, -7, -2, 6, keyValue);
            AddEastDoor(false, -5, -7, 6, keyValue);
            AddEastDoors(false, -5, 1, 26);
            AddEastDoors(false, 6, 1, 26);

            SetSign(4, 11, 5);
        }

        public TrinsicKeepHome(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 4000000;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(4, 11, 0);
            }
        }
        public override HouseDeed GetDeed()
        {
            return new TrinsicKeepDeed();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    //Yew Collection
    public class SmallYewCabin : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-5, -5, 11, 8), new Rectangle2D(0, 3, 4, 1) };
        public SmallYewCabin(Mobile owner)
            : base(0x135, owner, 715, 5)
        {
            uint keyValue = CreateKeys(owner);

            AddSouthDoors(1, 2, 5, keyValue);
            AddSouthDoor(3, -2, 5, keyValue);

            SetSign(3, 3, 5);
        }

        public SmallYewCabin(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 83333;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.TwoStoryFoundations[35];
            }
        }
        public override int ConvertOffsetY
        {
            get
            {
                return -1;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(4, 3, 0);
            }
        }
        public override HouseDeed GetDeed()
        {
            return new SmallYewCabinDeed();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class YewLogHomeE : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-5, -5, 11, 11), new Rectangle2D(-4, 6, 3, 1), new Rectangle2D(6, -3, 1, 4) };
        public YewLogHomeE(Mobile owner)
            : base(0x144, owner, 850, 6)
        {
            uint keyValue = CreateKeys(owner);

            AddEastDoors(true, 5, -2, 5, keyValue);
            AddSouthDoor(-3, 5, 5, keyValue);

            AddEastDoor(true, -1, 4, 5);

            SetEastSign(6, -3, 5);
        }

        public YewLogHomeE(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 116666;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.TwoStoryFoundations[35];
            }
        }
        public override int ConvertOffsetY
        {
            get
            {
                return -1;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(6, -4, 0);
            }
        }
        public override HouseDeed GetDeed()
        {
            return new YewLogHomeDeedE();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class YewResidenceE : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-6, -7, 12, 14), new Rectangle2D(6, 4, 1, 3) };
        public YewResidenceE(Mobile owner)
            : base(0x139, owner, 1505, 11)
        {
            uint keyValue = CreateKeys(owner);

            AddEastDoors(true, 3, 4, 5, keyValue);

            AddSouthDoor(-4, -4, 26);
            AddSouthDoor(1, 2, 26);
            AddEastDoor(true, 3, -5, 26, keyValue);

            SetEastSign(4, 6, 8);
        }

        public YewResidenceE(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 333333;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.TwoStoryFoundations[35];
            }
        }
        public override int ConvertOffsetY
        {
            get
            {
                return -1;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(4, 7, 0);
            }
        }
        public override HouseDeed GetDeed()
        {
            return new YewResidenceDeedE();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class YewVerandaE : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-9, -9, 15, 18), new Rectangle2D(6, -9, 2, 4), new Rectangle2D(6, 0, 2, 2) };
        public YewVerandaE(Mobile owner)
            : base(0x114, owner, 2119, 15)
        {
            uint keyValue = CreateKeys(owner);

            AddEastDoors(true, 2, 0, 11, keyValue);
            AddSouthDoor(-4, 2, 11);
            AddSouthDoor(0, 2, 11);
            SetEastSign(6, -1, 11);
        }

        public YewVerandaE(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 783333;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.TwoStoryFoundations[35];
            }
        }
        public override int ConvertOffsetY
        {
            get
            {
                return -1;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(6, -1, 0);
            }
        }
        public override HouseDeed GetDeed()
        {
            return new YewVerandaDeedE();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class YewTwoStoryResidenceE : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-9, -8, 18, 16), new Rectangle2D(-3, 8, 3, 1), new Rectangle2D(9, -4, 1, 4) };
        public YewTwoStoryResidenceE(Mobile owner)
            : base(0x137, owner, 2119, 15)
        {
            uint keyValue = CreateKeys(owner);

            AddEastDoors(true, 8, -3, 5, keyValue);
            AddSouthDoor(-2, 7, 5, keyValue);
            AddEastDoor(true, -4, 3, 5, keyValue);
            AddSouthDoor(-7, -5, 5, keyValue);

            AddSouthDoor(-6, 2, 25);
            AddEastDoors(true, 4, -1, 25);

            SetEastSign(9, -6, 0);

        }

        public YewTwoStoryResidenceE(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 783333;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.TwoStoryFoundations[35];
            }
        }
        public override int ConvertOffsetY
        {
            get
            {
                return -1;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(9, -6, 0);
            }
        }
        public override HouseDeed GetDeed()
        {
            return new YewTwoStoryResidenceDeedE();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    //Phase 3 Houses
    //Deceit Collection
    public class DeceitSmallHouse : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-3, -3, 7, 7), new Rectangle2D(-1, 4, 3, 1) };
        public DeceitSmallHouse(Mobile owner, int id)
            : base(0x16B, owner, 425, 3)
        {
            uint keyValue = CreateKeys(owner);

            AddSouthDoor(0, 3, 7, keyValue);

            SetSign(-2, 4, 5);
        }

        public DeceitSmallHouse(Serial serial)
            : base(serial)
        {
        }

        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(-2, 4, 0);
            }
        }
        public override int DefaultPrice
        {
            get
            {
                return 30000;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.TwoStoryFoundations[0];
            }
        }
        public override HouseDeed GetDeed()
        {
            return new DeceitSmallHouseDeed();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class DeceitWatchtower : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-5, -5, 10, 9), new Rectangle2D(-2, 4, 4, 1) };
        public DeceitWatchtower(Mobile owner)
            : base(0x16D, owner, 985, 7)
        {
            uint keyValue = CreateKeys(owner);

            AddSouthDoors(-1, 3, 5, keyValue);
            AddEastDoor(true, 3, -3, 26, keyValue);
            AddEastDoor(true, -3, -2, 46);

            SetSign(-3, 4, 5);

        }

        public DeceitWatchtower(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 150000;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.ThreeStoryFoundations[20];
            }
        }
        public override int ConvertOffsetX
        {
            get
            {
                return -1;
            }
        }
        public override int ConvertOffsetY
        {
            get
            {
                return -1;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(-3, 4, 0);
            }
        }
        public override HouseDeed GetDeed()
        {
            return new DeceitWatchtowerDeed();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class DeceitLodge : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-5, -7, 11, 15), new Rectangle2D(-1, 8, 3, 1) };
        public DeceitLodge(Mobile owner)
            : base(0x168, owner, 1235, 9)
        {
            uint keyValue = CreateKeys(owner);

            AddSouthDoor(0, 7, 5, keyValue);

            SetSign(-2, 8, 4);
        }

        public DeceitLodge(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 250000;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.TwoStoryFoundations[12];
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(-2, 8, 0);
            }
        }
        public override HouseDeed GetDeed()
        {
            return new DeceitLodgeDeed();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class DeceitGreatLodge : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-5, -7, 11, 15), new Rectangle2D(-1, 8, 3, 1) };
        public DeceitGreatLodge(Mobile owner)
            : base(0x167, owner, 1505, 11)
        {
            uint keyValue = CreateKeys(owner);

            AddSouthDoor(0, 7, 5, keyValue);

            SetSign(-2, 8, 4);
        }

        public DeceitGreatLodge(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 333333;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.TwoStoryFoundations[12];
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(-2, 8, 0);
            }
        }
        public override HouseDeed GetDeed()
        {
            return new DeceitGreatLodgeDeed();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class DeceitChalet : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-8, -7, 15, 15), new Rectangle2D(-3, 8, 6, 1), new Rectangle2D(7, -3, 1, 6) };
        public DeceitChalet(Mobile owner)
            : base(0x166, owner, 1640, 12)
        {
            uint keyValue = CreateKeys(owner);

            AddSouthDoors(-1, 7, 5, keyValue);

            SetSign(-3, 8, 5);
        }

        public DeceitChalet(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 666666;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.ThreeStoryFoundations[20];
            }
        }
        public override int ConvertOffsetX
        {
            get
            {
                return -1;
            }
        }
        public override int ConvertOffsetY
        {
            get
            {
                return -1;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(-4, 8, 0);
            }
        }
        public override HouseDeed GetDeed()
        {
            return new DeceitChaletDeed();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class DeceitLonghouseE : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-10, -5, 16, 11), new Rectangle2D(6, -2, 4, 7), new Rectangle2D(10, -1, 1, 3), new Rectangle2D(-9, 6, 3, 1) };
        public DeceitLonghouseE(Mobile owner)
            : base(0x169, owner, 2119, 15)
        {
            uint keyValue = CreateKeys(owner);

            AddSouthDoor(-8, 5, 5, keyValue);
            AddEastDoor(true, 9, 0, 5, keyValue);

            SetEastSign(10, -2, 5);
        }

        public DeceitLonghouseE(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 3000000;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.ThreeStoryFoundations[37];
            }
        }
        public override int ConvertOffsetY
        {
            get
            {
                return -1;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(10, -2, 0);
            }
        }
        public override HouseDeed GetDeed()
        {
            return new DeceitLonghouseDeedE();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class DeceitGreatHall : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-10, -9, 17, 11), new Rectangle2D(7, -9, 3, 7), new Rectangle2D(-7, 2, 11, 6), new Rectangle2D(10, -7, 1, 3) , new Rectangle2D(-3, 8, 3, 1) };
        public DeceitGreatHall(Mobile owner)
            : base(0x16A, owner, 2320, 16)
        {
            uint keyValue = CreateKeys(owner);

            AddSouthDoor(-2, 7, 5, keyValue);
            AddEastDoor(true, 9, -6, 5, keyValue);
            AddEastDoors(true, 6, -6, 25);

            SetSign(-6, 8, 5);
        }

        public DeceitGreatHall(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 3000000;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.ThreeStoryFoundations[37];
            }
        }
        public override int ConvertOffsetY
        {
            get
            {
                return -1;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(-6, 8, 0);
            }
        }
        public override HouseDeed GetDeed()
        {
            return new DeceitGreatHallDeed();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class DeceitFarmsteadE : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-9, -10, 17, 20), new Rectangle2D(8, 6, 1, 4), new Rectangle2D(8, -9, 1, 3) };
        public DeceitFarmsteadE(Mobile owner)
            : base(0x165, owner, 2475, 17)
        {
            uint keyValue = CreateKeys(owner);

            AddEastDoors(true, 7, 7, 5, keyValue);
            AddEastDoor(true, 7, -8, 5, keyValue);
            AddSouthDoor(4, 0, 5, keyValue);

            SetEastSign(8, 6, 5);
        }

        public DeceitFarmsteadE(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 3333333;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.ThreeStoryFoundations[37];
            }
        }
        public override int ConvertOffsetY
        {
            get
            {
                return -1;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(8, 4, 0);
            }
        }
        public override HouseDeed GetDeed()
        {
            return new DeceitFarmsteadDeedE();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class DeceitGrandLodge : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-5, -12, 9, 9), new Rectangle2D(-13, -4, 25, 9), new Rectangle2D(-5, 4, 9, 6), new Rectangle2D(-5, 10, 10, 3), new Rectangle2D(4, -11, 1, 3) };
        public DeceitGrandLodge(Mobile owner)
            : base(0x16C, owner, 2625, 18)
        {
            uint keyValue = CreateKeys(owner);

            AddSouthDoors(-1, 9, 5, keyValue);
            AddEastDoors(true, 3, 0, 5);
            AddEastDoors(true, -5, 0, 5);
            AddEastDoor(true, 3, -10, 5, keyValue);

            AddSouthDoors(-1, 6, 25, keyValue);
            AddSouthDoors(-1, -6, 25);

            AddSouthDoors(-1, 4, 45);
            AddEastDoors(true, 3, 0, 45);
            AddEastDoors(true, -5, 0, 45);

            SetSign(-5, 10, 5);
        }

        public DeceitGrandLodge(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 4000000;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.ThreeStoryFoundations[37];
            }
        }
        public override int ConvertOffsetY
        {
            get
            {
                return -1;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(-6, 10, 0);
            }
        }
        public override HouseDeed GetDeed()
        {
            return new DeceitGrandLodgeDeed();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    //Minoc Collection
    public class MinocSmallHouse : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-3, -3, 7, 7), new Rectangle2D(-1, 4, 3, 1) };
        public MinocSmallHouse(Mobile owner, int id)
            : base(0x173, owner, 425, 3)
        {
            uint keyValue = CreateKeys(owner);

            AddSouthDoor(0, 3, 7, keyValue);

            SetSign(-2, 4, 0);
        }

        public MinocSmallHouse(Serial serial)
            : base(serial)
        {
        }

        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(-2, 4, 0);
            }
        }
        public override int DefaultPrice
        {
            get
            {
                return 30000;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.TwoStoryFoundations[0];
            }
        }
        public override HouseDeed GetDeed()
        {
            return new MinocSmallHouseDeed();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class MinocVillaE : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-5, -5, 10, 10), new Rectangle2D(5, 0, 1, 4) };
        public MinocVillaE(Mobile owner)
            : base(0x176, owner, 985, 7)
        {
            uint keyValue = CreateKeys(owner);

            AddEastDoors(true, 4, 1, 5, keyValue);
            AddSouthDoor(true, 2, 0, 25, keyValue);
            SetEastSign(5, -1, 0);
        }

        public MinocVillaE(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 150000;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.TwoStoryFoundations[35];
            }
        }
        public override int ConvertOffsetY
        {
            get
            {
                return -1;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(5, -1, 0);
            }
        }
        public override HouseDeed GetDeed()
        {
            return new MinocVillaEDeed();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class MinocLoftE : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-5, -7, 11, 14), new Rectangle2D(6, -4, 1, 4) };
        public MinocLoftE(Mobile owner)
            : base(0x174, owner, 1235, 9)
        {
            uint keyValue = CreateKeys(owner);

            AddEastDoors(true, 5, -3, 5, keyValue);
            AddSouthDoor(true, 3, 1, 25, keyValue);
            SetEastSign(6, 0, 0);
        }

        public MinocLoftE(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 250000;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.TwoStoryFoundations[35];
            }
        }
        public override int ConvertOffsetY
        {
            get
            {
                return -1;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(6, 0, 0);
            }
        }
        public override HouseDeed GetDeed()
        {
            return new MinocLoftEDeed();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class MinocLargeVilla : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-7, -9, 14, 17) };
        public MinocLargeVilla(Mobile owner)
            : base(0x170, owner, 1950, 14)
        {
            uint keyValue = CreateKeys(owner);

            AddSouthDoors(3, 1, 5, keyValue);
            AddSouthDoor(4, -2, 25, keyValue);


            SetSign(0, 8, 0);

        }

        public MinocLargeVilla(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 2666666;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.TwoStoryFoundations[31];
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(0, 8, 0);
            }
        }
        public override HouseDeed GetDeed()
        {
            return new MinocLargeVillaDeed();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class MinocBungalo : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-7, -9, 14, 17), new Rectangle2D(-7, 8, 7, 1), new Rectangle2D(7, -9, 1, 12) };
        public MinocBungalo(Mobile owner)
            : base(0x177, owner, 3200, 15)
        {
            uint keyValue = CreateKeys(owner);

            AddSouthDoors(3, 2, 5, keyValue);

            SetSign(1, 3, 5);

        }

        public MinocBungalo(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 3000000;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.TwoStoryFoundations[31];
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(-7, 10, 0);
            }
        }
        public override HouseDeed GetDeed()
        {
            return new MinocBungaloDeed();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    //Vesper Collection
    public class VesperShop : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-6, -4, 12, 9), new Rectangle2D(0, 4, 3, 1) };
        public VesperShop(Mobile owner)
            : base(0x143, owner, 985, 7)
        {
            uint keyValue = CreateKeys(owner);

            AddSouthDoors(2, 3, 5, keyValue);
            AddEastDoor(true, -1, -1, 5, keyValue);

            SetSign(4, 4, 5);
        }

        public VesperShop(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 150000;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.TwoStoryFoundations[6];
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(5, 4, 0);
            }
        }
        public override HouseDeed GetDeed()
        {
            return new VesperShopDeed();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class VesperSlateRanchHouse : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-7, -5, 13, 9), new Rectangle2D(-7, 3, 9, 3), new Rectangle2D(-4, 6, 4, 1) };
        public VesperSlateRanchHouse(Mobile owner)
            : base(0x13C, owner, 1100, 8)
        {
            uint keyValue = CreateKeys(owner);

            AddSouthDoors(-3, 5, 5, keyValue);
            AddSouthDoor(-4, -1, 5);
            AddEastDoor(true, -3, -3, 5);

            SetSign(-6, 6, 0);
        }

        public VesperSlateRanchHouse(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 183333;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.TwoStoryFoundations[35];
            }
        }
        public override int ConvertOffsetY
        {
            get
            {
                return -1;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(-6, 6, 0);
            }
        }
        public override HouseDeed GetDeed()
        {
            return new VesperSlateRanchHouseDeed();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class VesperCabin : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-6, -5, 9, 11), new Rectangle2D(-1, -7, 7, 6), new Rectangle2D(3, -1, 3, 1), new Rectangle2D(3, -1, 3, 1), new Rectangle2D(-6, 6, 4, 1) };
        public VesperCabin(Mobile owner)
            : base(0x136, owner, 1100, 8)
        {
            uint keyValue = CreateKeys(owner);

            AddEastDoors(true, -3, 3, 8, keyValue);
            AddSouthDoor(4, -2, 8, keyValue);

            SetEastSign(6, -2, 2);

        }

        public VesperCabin(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 183333;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.TwoStoryFoundations[12];
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(6, -2, 0);
            }
        }
        public override HouseDeed GetDeed()
        {
            return new VesperCabinDeed();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class VesperOverlookE : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-7, -7, 9, 13), new Rectangle2D(2, -4, 4, 7) };
        public VesperOverlookE(Mobile owner)
            : base(0x145, owner, 1505, 11)
        {
            uint keyValue = CreateKeys(owner);

            AddEastDoor(true, 1, -1, 5, keyValue);
            AddEastDoor(true, 1, 1, 25);

            SetEastSign(2, 5, 5);
        }

        public VesperOverlookE(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 333333;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.TwoStoryFoundations[35];
            }
        }
        public override int ConvertOffsetY
        {
            get
            {
                return -1;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(2, 5, 0);
            }
        }
        public override HouseDeed GetDeed()
        {
            return new VesperOverlookEDeed();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class VesperTwoStoryCabin : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-6, -7, 12, 15)};
        public VesperTwoStoryCabin(Mobile owner)
            : base(0x138, owner, 1505, 11)
        {
            uint keyValue = CreateKeys(owner);

            AddSouthDoors(2, 6, 5, keyValue);
            AddSouthDoor(-3, -1, 5);
            AddEastDoor(true, 0, -1, 25);
            AddEastDoor(true, 0, 4, 25, keyValue);

            SetSign(4, 7, 5);

        }

        public VesperTwoStoryCabin(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 416666;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.TwoStoryFoundations[31];
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(4, 7, 0);
            }
        }
        public override HouseDeed GetDeed()
        {
            return new VesperTwoStoryCabinDeed();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class VesperCompound : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-13, -13, 26, 25) };
        public VesperCompound(Mobile owner)
            : base(0x146, owner, 2625, 18)
        {
            uint keyValue = CreateKeys(owner);

            AddSouthDoors(true, -1, 8, 11, keyValue);
            AddSouthDoors(true, -1, 4, 11, keyValue);
            AddSouthDoors(true, -8, 2, 11, true);
            AddSouthDoors(-8, -3, 11);
            AddEastDoors(true, 7, 0, 11, keyValue);
            AddEastDoors(true, 4, -5, 11);

            AddSouthDoors(true, 7, 5, 31, true);

            AddEastDoors(true, 0, -5, 51);
            AddSouthDoor(1, -3, 51);
            AddSouthDoor(0, 4, 51);

            AddEastDoor(true, 7, -8, 72);

            SetSign(-3, 9, 12);

        }

        public VesperCompound(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 4000000;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(-3, 12, 0);
            }
        }

        public override HouseDeed GetDeed()
        {
            return new VesperCompoundDeed();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    //Wind Collection
    public class WindSmallTowerE : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-4, -4, 8, 8) };
        public WindSmallTowerE(Mobile owner)
            : base(0x151, owner, 580, 4)
        {
            uint keyValue = CreateKeys(owner);

            AddEastDoor(false, 2, 0, 6, keyValue);

            SetEastSign(4, -3, 0);
        }

        public WindSmallTowerE(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 50000;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.TwoStoryFoundations[6];
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(4, -3, 0);
            }
        }
        public override HouseDeed GetDeed()
        {
            return new WindSmallTowerDeedE();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class WindSmallTower : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-4, -5, 9, 9), new Rectangle2D(-1, 4, 4, 1) };
        public WindSmallTower(Mobile owner)
            : base(0xDD, owner, 715, 5)
        {
            uint keyValue = CreateKeys(owner);

            AddSouthDoors(false, 0, 3, 6, keyValue);
            AddEastDoor(false, -2, 2, 25, keyValue);

            SetSign(-3, 4, 0);
        }

        public WindSmallTower(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 83333;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.TwoStoryFoundations[6];
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(-3, 4, 0);
            }
        }
        public override HouseDeed GetDeed()
        {
            return new WindSmallTowerDeed();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class WindMansion : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-14, -12, 28, 18), new Rectangle2D(-12, 6, 24, 2), new Rectangle2D(-11, 8, 22, 3), new Rectangle2D(-7, 11, 15, 2) };
        public WindMansion(Mobile owner)
            : base(0x189, owner, 2625, 18)
        {
            uint keyValue = CreateKeys(owner);

            AddSouthDoors(false, -1, 3, 5, keyValue);
            AddSouthDoors(false, -1, -1, 25, keyValue);

            AddEastDoor(false, -14, -4, 5, keyValue);
            AddEastDoor(false, 8, -9, 5, keyValue);

            AddEastDoor(false, -9, -6, 45, keyValue);
            AddEastDoor(false, 8, -6, 45, keyValue);

            SetEastSign(14, 3, 0);

        }

        public WindMansion(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 4000000;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(14, 3, 0);
            }
        }

        public override HouseDeed GetDeed()
        {
            return new WindMansionDeed();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    //Phase 4 Houses
    //Nujel'm Collection
    public class NujelmSmallHouseE : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-3, -3, 7, 7), new Rectangle2D(4, -1, 1, 3) };
        public NujelmSmallHouseE(Mobile owner, int id)
            : base(0x14D, owner, 425, 3)
        {
            uint keyValue = CreateKeys(owner);

            AddEastDoor(true, 3, 0, 6, keyValue);

            SetEastSign(4, -1, 5);
        }

        public NujelmSmallHouseE(Serial serial)
            : base(serial)
        {
        }

        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(4, -2, 0);
            }
        }
        public override int DefaultPrice
        {
            get
            {
                return 30000;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.TwoStoryFoundations[0];
            }
        }
        public override HouseDeed GetDeed()
        {
            return new NujelmSmallHouseDeedE();
        }


        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class NujelmTwoStoryBalcony : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-4, -4, 8, 8), new Rectangle2D(-1, 4, 3, 1) };
        public NujelmTwoStoryBalcony(Mobile owner)
            : base(0x14C, owner, 580, 4)
        {
            uint keyValue = CreateKeys(owner);

            AddSouthDoor(0, 3, 5, keyValue);
            AddSouthDoor(0, 2, 25, keyValue);


            SetSign(-4, 4, 0);
        }

        public NujelmTwoStoryBalcony(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 50000;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.TwoStoryFoundations[35];
            }
        }
        public override int ConvertOffsetY
        {
            get
            {
                return -1;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(-4, 4, 0);
            }
        }
        public override HouseDeed GetDeed()
        {
            return new NujelmTwoStoryBalconyDeed();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class NujelmVillaE : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-5, -8, 10, 16), new Rectangle2D(5, 2, 2, 4), new Rectangle2D(5, -7, 2, 6) };
        public NujelmVillaE(Mobile owner, int id)
            : base(0x14E, owner, 1425, 11)
        {
            uint keyValue = CreateKeys(owner);

            AddEastDoors(true, 4, 3, 6, keyValue);
            AddEastDoor(true, 4, -4, 26, keyValue);

            SetEastSign(5, 7, 0);
        }

        public NujelmVillaE(Serial serial)
            : base(serial)
        {
        }

        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(5, 7, 0);
            }
        }
        public override int DefaultPrice
        {
            get
            {
                return 333333;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.TwoStoryFoundations[0];
            }
        }
        public override HouseDeed GetDeed()
        {
            return new NujelmVillaDeedE();
        }


        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class NujelmCourtyardManor : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-10, -8, 20, 7),new Rectangle2D(-8, -1, 16, 3), new Rectangle2D(-10, 2, 20, 8), new Rectangle2D(-3, 10, 7, 1) };
        public NujelmCourtyardManor(Mobile owner)
            : base(0x14B, owner, 1950, 14)
        {
            uint keyValue = CreateKeys(owner);

            AddSouthDoor(-1, 7, 5, keyValue);
            AddSouthDoor(1, 7, 5, keyValue);
            AddEastDoors(true, -4, 0, 5, keyValue);
            AddSouthDoor(0, -5, 5, keyValue);
            AddEastDoors(true, 3, 0, 5, keyValue);

            AddSouthDoor(-6, -2, 5);
            AddSouthDoor(-6, 2, 5);
            AddSouthDoor(6, -2, 5);
            AddSouthDoor(6, 2, 5);

            AddEastDoor(true, -3, -6, 5);
            AddEastDoor(2, -6, 5, 0);

            AddEastDoor(true, -3, 7, 25, keyValue);
            AddEastDoor(2, 7, 25, keyValue);

            SetSign(-3, 10, 5);
        }

        public NujelmCourtyardManor(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 2666666;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.TwoStoryFoundations[35];
            }
        }
        public override int ConvertOffsetY
        {
            get
            {
                return -1;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(-4, 10, 0);
            }
        }
        public override HouseDeed GetDeed()
        {
            return new NujelmCourtyardManorDeed();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class NujelmTwoStoryRanchHouse : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-15, -9, 28, 17), new Rectangle2D(-11, 8, 4, 2), new Rectangle2D(13, -8, 1, 3) };
        public NujelmTwoStoryRanchHouse(Mobile owner)
            : base(0x14A, owner, 2320, 16)
        {
            uint keyValue = CreateKeys(owner);

            AddSouthDoors(-1, -2, 5, keyValue);
            AddEastDoor(-5, -5, 5, 0);
            AddEastDoor(3, -5, 5, 0);
            AddEastDoor(true, 12, -7, 5, keyValue);

            AddSouthDoor(-3, -3, 25, keyValue);
            AddEastDoor(-5, -6, 25, 0);
            AddEastDoor(3, -6, 25, 0);
            AddEastDoor(-13, -2, 25, 0);
            AddEastDoor(-13, 4, 25, 0);
            AddSouthDoor(-9, 7, 25, keyValue);

            SetSign(-5, 8, 0);
        }

        public NujelmTwoStoryRanchHouse(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 3000000;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.TwoStoryFoundations[35];
            }
        }
        public override int ConvertOffsetY
        {
            get
            {
                return -1;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(-5, 9, 0);
            }
        }
        public override HouseDeed GetDeed()
        {
            return new NujelmTwoStoryRanchHouseDeed();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    //Magincia Collection
    public class MaginciaWagon : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-3, -2, 8, 4), new Rectangle2D(1, 2, 1, 1) };
        public MaginciaWagon(Mobile owner, int id)
            : base(0x153, owner, 425, 3)
        {
            uint keyValue = CreateKeys(owner);

            AddSouthDoor(1, 1, 10, keyValue);

            SetSign(-2, 2, 8);
        }

        public MaginciaWagon(Serial serial)
            : base(serial)
        {
        }

        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(-2, 2, 0);
            }
        }
        public override int DefaultPrice
        {
            get
            {
                return 83333;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.TwoStoryFoundations[0];
            }
        }
        public override HouseDeed GetDeed()
        {
            return new MaginciaWagonDeed();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class MaginciaWagonE : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-2, -3, 4, 8), new Rectangle2D(2, 1, 1, 1) };
        public MaginciaWagonE(Mobile owner, int id)
            : base(0x154, owner, 425, 3)
        {
            uint keyValue = CreateKeys(owner);

            AddEastDoor(1, 1, 10, keyValue);

            SetEastSign(2, -2, 8);
        }

        public MaginciaWagonE(Serial serial)
            : base(serial)
        {
        }

        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(2, -2, 0);
            }
        }
        public override int DefaultPrice
        {
            get
            {
                return 83333;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.TwoStoryFoundations[0];
            }
        }
        public override HouseDeed GetDeed()
        {
            return new MaginciaWagonDeedE();
        }


        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class MaginciaHalfShoppeE : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-4, -3, 8, 7), new Rectangle2D(6, -3, 1, 4) };
        public MaginciaHalfShoppeE(Mobile owner)
            : base(0x10B, owner, 580, 4)
        {
            uint keyValue = CreateKeys(owner);

            AddEastDoor(true, 2, 2, 5, keyValue);

            SetSign(2, 4, 0);
        }

        public MaginciaHalfShoppeE(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 50000;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.TwoStoryFoundations[35];
            }
        }
        public override int ConvertOffsetY
        {
            get
            {
                return -1;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(2, 4, 0);
            }
        }
        public override HouseDeed GetDeed()
        {
            return new MaginciaHalfShoppeEDeed();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class MaginciaShoppeE : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-4, -3, 9, 7) };
        public MaginciaShoppeE(Mobile owner)
            : base(0x10F, owner, 580, 4)
        {
            uint keyValue = CreateKeys(owner);

            AddEastDoors(true, 1, 0, 5, keyValue);

            SetSign(2, 4, 0);
        }

        public MaginciaShoppeE(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 50000;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.TwoStoryFoundations[35];
            }
        }
        public override int ConvertOffsetY
        {
            get
            {
                return -1;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(2, 4, 0);
            }
        }
        public override HouseDeed GetDeed()
        {
            return new MaginciaShoppeEDeed();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class MaginciaOutpostE : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-5, -6, 9, 12), new Rectangle2D(4, 1, 1, 3) };
        public MaginciaOutpostE(Mobile owner)
            : base(0x11D, owner, 1100, 8)
        {
            uint keyValue = CreateKeys(owner);

            AddEastDoor(true, 3, 2, 5, keyValue);
            AddSouthDoor(-2, 3, 25, keyValue);
            AddSouthDoor(1, 3, 25, keyValue);

            SetEastSign(4, 5, 5);
        }

        public MaginciaOutpostE(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 183333;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.TwoStoryFoundations[35];
            }
        }
        public override int ConvertOffsetY
        {
            get
            {
                return -1;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(4, 5, 0);
            }
        }
        public override HouseDeed GetDeed()
        {
            return new MaginciaOutpostEDeed();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class MaginciaTwoStoryHouseE : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-5, -6, 9, 12), new Rectangle2D(4, -5, 1, 3) };
        public MaginciaTwoStoryHouseE(Mobile owner)
            : base(0x10D, owner, 1100, 8)
        {
            uint keyValue = CreateKeys(owner);

            AddEastDoor(true, 3, -4, 5, keyValue);
            AddSouthDoor(1, 2, 5, keyValue);

            SetSign(-4, 6, 5);
        }

        public MaginciaTwoStoryHouseE(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 183333;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.TwoStoryFoundations[35];
            }
        }
        public override int ConvertOffsetY
        {
            get
            {
                return -1;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(-4, 6, 0);
            }
        }
        public override HouseDeed GetDeed()
        {
            return new MaginciaTwoStoryHouseEDeed();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class MaginciaResidenceE : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-5, -7, 9, 12), new Rectangle2D(4, -6, 1, 3) };
        public MaginciaResidenceE(Mobile owner)
            : base(0x111, owner, 1235, 9)
        {
            uint keyValue = CreateKeys(owner);

            AddEastDoor(true, 3, -5, 5, keyValue);
            AddSouthDoor(1, -3, 25);

            SetEastSign(4, -3, 0);
        }

        public MaginciaResidenceE(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 250000;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.TwoStoryFoundations[35];
            }
        }
        public override int ConvertOffsetY
        {
            get
            {
                return -1;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(4, -3, 0);
            }
        }
        public override HouseDeed GetDeed()
        {
            return new MaginciaResidenceEDeed();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class MaginciaVilla : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-7, -5, 13, 10), new Rectangle2D(-3, 5, 4, 1) };
        public MaginciaVilla(Mobile owner)
            : base(0x10C, owner, 1505, 11)
        {
            uint keyValue = CreateKeys(owner);

            AddSouthDoors(-2, 4, 5, keyValue);
            AddEastDoor(-3, -3, 45, 0);
            AddEastDoor(-3, 2, 45, 0);

            SetSign(-5, 5, 0);
        }

        public MaginciaVilla(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 333333;
            }
        }
        public override HousePlacementEntry ConvertEntry
        {
            get
            {
                return HousePlacementEntry.TwoStoryFoundations[35];
            }
        }
        public override int ConvertOffsetY
        {
            get
            {
                return -1;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(-5, 5, 0);
            }
        }
        public override HouseDeed GetDeed()
        {
            return new MaginciaVillaDeed();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    //Lord & Lady Collection
    public class SkaraLordCompound : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-15, -15, 30, 30), new Rectangle2D(-1, 16, 4, 1) };
        public SkaraLordCompound(Mobile owner)
            : base(0xD2, owner, 9000, 50)
        {
            uint keyValue = CreateKeys(owner);

            AddSouthDoors(0, 13, 1, keyValue);
            AddSouthDoors(0, -6, 6, keyValue);
            AddSouthDoors(-9, -5, 6);
            AddEastDoors(true, -7, 9, 1, keyValue);
            AddEastDoors(true, 6, 9, 1, keyValue);
            AddEastDoors(true, 6, 5, 1, keyValue);

            AddEastDoors(true, -6, -10, 26);
            AddSouthDoor(-7, -5, 26, keyValue);

            SetSign(-2, 15, 0);

        }

        public SkaraLordCompound(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 8000000;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(-2, 15, 0);
            }
        }

        public override HouseDeed GetDeed()
        {
            return new SkaraLordCompoundDeed();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class TrinsicLordFortress : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-15, -15, 31, 31) };
        public TrinsicLordFortress(Mobile owner)
            : base(0xF9, owner, 9000, 50)
        {
            uint keyValue = CreateKeys(owner);

            AddSouthDoors(true, -13, 7, 10, true);
            AddSouthDoors(true, 13, 7, 10, true);
            AddSouthDoors(0, 0, 5, keyValue);
            AddSouthDoors(-9, -2, 5, keyValue);
            AddSouthDoors(9, -2, 5, keyValue);
            AddEastDoors(true, -3, -11, 5);
            AddEastDoors(true, 4, -11, 5);

            AddSouthDoors(-9, -2, 45);
            AddSouthDoors(9, -2, 45);

            SetSign(-9, 16, 0);

        }

        public TrinsicLordFortress(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 8000000;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(-9, 16, 0);
            }
        }

        public override HouseDeed GetDeed()
        {
            return new TrinsicLordFortressDeed();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class YewLordFarmstead : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-15, -15, 29, 29), new Rectangle2D(-1, 16, 4, 1) };
        public YewLordFarmstead(Mobile owner)
            : base(0xD3, owner, 9000, 50)
        {
            uint keyValue = CreateKeys(owner);

            AddSouthDoors(-3, 12, 1, keyValue);
            AddSouthDoors(-10, 2, 1, keyValue);
            AddSouthDoors(5, -6, 1, keyValue);
            AddEastDoors(true, -4, -2, 1);
            AddEastDoors(true, 1, -12, 1);

            AddEastDoors(true, -7, -4, 22);
            AddEastDoors(true, -7, -13, 22);

            SetSign(-5, 14, 0);

        }

        public YewLordFarmstead(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 8000000;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(-5, 14, 0);
            }
        }

        public override HouseDeed GetDeed()
        {
            return new YewLordFarmsteadDeed();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class WindCompound : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-15, -15, 31, 29) };
        public WindCompound(Mobile owner)
            : base(0xD5, owner, 9000, 50)
        {
            uint keyValue = CreateKeys(owner);

            AddSouthDoors(false, 0, 11, 6, keyValue);

            AddSouthDoors(false, -10, -5, 6, keyValue);
            AddSouthDoors(false, 10, -5, 6, keyValue);

            AddSouthDoors(false, -10, -1, 26);
            AddSouthDoors(false, 10, -1, 26);

            AddEastDoors(false, -5, -11, 26, 0);
            AddEastDoors(false, 5, -11, 26);

            SetSign(-4, 14, 1);
        }

        public WindCompound(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 8000000;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(-4, 14, 0);
            }
        }

        public override HouseDeed GetDeed()
        {
            return new WindCompoundDeed();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class NujelmLordCompound : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-15, -15, 31, 31) };
        public NujelmLordCompound(Mobile owner)
            : base(0x188, owner, 9000, 50)
        {
            uint keyValue = CreateKeys(owner);

            AddSouthDoors(-11, 14, 6, keyValue);
            AddSouthDoors(3, 14, 6, keyValue);
            AddEastDoors(true, 14, 7, 6, keyValue);

            AddEastDoors(true, 9, -7, 25);

            AddEastDoors(true, 8, -11, 45, keyValue);
            AddSouthDoors(-11, -6, 45);
            AddSouthDoors(-3, -6, 45, keyValue);
            AddSouthDoors(-11, 8, 45, keyValue);

            SetSign(-13, 16, 0);

        }

        public NujelmLordCompound(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 8000000;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(-13, 16, 0);
            }
        }

        public override HouseDeed GetDeed()
        {
            return new NujelmLordCompoundDeed();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class MaginciaLordCompound : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-15, -15, 31, 31) };
        public MaginciaLordCompound(Mobile owner)
            : base(0xFA, owner, 9000, 50)
        {
            uint keyValue = CreateKeys(owner);

            AddSouthDoors(1, 15, 0, keyValue);
            AddEastDoor(true, -5, 10, 5);
            AddSouthDoor(-12, 9, 5);
            AddEastDoors(true, -5, -2, 5);
            AddSouthDoor(-8, -4, 5);
            AddEastDoor(true, -5, -12, 5);
            AddEastDoor(6, -12, 5, 0);

            AddSouthDoor(-9, -6, 25);
            AddSouthDoor(1, -8, 25, keyValue);
            AddSouthDoors(true, 10, -6, 25, true);
            AddSouthDoors(10, -5, 25, keyValue);
            AddEastDoor(true, -2, -12, 25);

            SetSign(-5, 16, 0);

        }

        public MaginciaLordCompound(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 8000000;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(-5, -16, 0);
            }
        }

        public override HouseDeed GetDeed()
        {
            return new MaginciaLordCompoundDeed();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class GrandKeep : BaseHouse//warning: ODD shape!
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-14, -14, 8, 9), new Rectangle2D(7, -14, 8, 9), new Rectangle2D(-7, -12, 15, 7),
                                                                    new Rectangle2D(-12, -6, 25, 13), new Rectangle2D(-14, 6, 8, 9), new Rectangle2D(-7, 6, 15, 7), new Rectangle2D(7, 6, 8, 9),
                                                                    new Rectangle2D(-1, 13, 4, 1), new Rectangle2D(13, -1, 1, 4)};
        public GrandKeep(Mobile owner)
            : base(0x181, owner, 9000, 50)
        {
            uint keyValue = CreateKeys(owner);

            AddSouthDoors(false, 0, 12, 6, keyValue);
            AddSouthDoor(false, -9, -4, 6, keyValue);
            AddEastDoor(false, -7, -9, 6, keyValue);
            AddEastDoors(false, 12, 0, 6, keyValue);
            AddEastDoor(false, 7, -3, 6, keyValue);

            AddEastDoors(false, -7, 0, 26, keyValue);
            AddEastDoors(false, 7, 0, 26, keyValue);
            AddSouthDoors(false, 0, 6, 26, keyValue);

            SetSign(-2, 13, 0);
        }

        public GrandKeep(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 8000000;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(-2, 13, 0);
            }
        }
        public override HouseDeed GetDeed()
        {
            return new GrandKeepDeed();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class Fortress : BaseHouse
    {
        public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-18, -17, 37, 34), new Rectangle2D(-2, 17, 4, 1) };
        public Fortress(Mobile owner)
            : base(0x80, owner, 10000, 52)
        {
            uint keyValue = CreateKeys(owner);

            AddSouthDoors(false, -1, 16, 5, keyValue);

            SetSign(-3, 17, 0);

            AddSouthDoors(false, -1, 9, 5, true);
            AddSouthDoors(false, -1, 3, 5, keyValue);
            AddSouthDoors(false, 13, 2, 5);
            AddSouthDoors(false, -14, 2, 5);
            AddEastDoors(false, -10, 12, 5);
            AddEastDoors(false, 9, 12, 5, 0);


            AddEastDoors(false, -10, 12, 25);
            AddEastDoors(false, 9, 12, 25);
            AddSouthDoors(false, 13, 5, 25);
            AddSouthDoors(false, -14, 5, 25);
            AddSouthDoors(false, 13, -5, 25, keyValue);
            AddSouthDoors(false, -14, -5, 25, keyValue);

            AddSouthDoors(false, -1, -5, 45, keyValue);
            AddEastDoors(false, -10, -11, 45, keyValue);
            AddEastDoors(false, 9, -11, 45, keyValue);

        }

        public Fortress(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultPrice
        {
            get
            {
                return 8666666;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return AreaArray;
            }
        }
        public override Point3D BaseBanLocation
        {
            get
            {
                return new Point3D(5, 17, 0);
            }
        }

        public override HouseDeed GetDeed()
        {
            return new FortressDeed();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
