using Server;
using System;
using System.Collections;
using Server.Multis;
using Server.Targeting;
using Server.Items;
using Server.Mobiles;

namespace Server.Multis.Deeds
{
	public class HousePlacementTarget : MultiTarget
	{
		private HouseDeed m_Deed;

		public HousePlacementTarget( HouseDeed deed ) : base( deed.MultiID, deed.Offset )
		{
			m_Deed = deed;
		}

		protected override void OnTarget( Mobile from, object o )
		{
			IPoint3D ip = o as IPoint3D;

			if ( ip != null )
			{
				if ( ip is Item )
					ip = ((Item)ip).GetWorldTop();

				Point3D p = new Point3D( ip );

				Region reg = Region.Find( new Point3D( p ), from.Map );

				if ( from.AccessLevel >= AccessLevel.GameMaster || reg.AllowHousing( from, p ) )
					m_Deed.OnPlacement( from, p );
				else if ( reg.IsPartOf( typeof( TreasureRegion ) ) )
					from.SendLocalizedMessage( 1043287 ); // The house could not be created here.  Either something is blocking the house, or the house would not be on valid terrain.
				else
					from.SendLocalizedMessage( 501265 ); // Housing can not be created in this area.
			}
		}
	}

	public abstract class HouseDeed : Item
	{
		private int m_MultiID;
		private Point3D m_Offset;

		[CommandProperty( AccessLevel.GameMaster )]
		public int MultiID
		{
			get
			{
				return m_MultiID;
			}
			set
			{
				m_MultiID = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Point3D Offset
		{
			get
			{
				return m_Offset;
			}
			set
			{
				m_Offset = value;
			}
		}

		public HouseDeed( int id, Point3D offset ) : base( 0x14F0 )
		{
			Weight = 1.0;
			LootType = LootType.Newbied;

			m_MultiID = id;
			m_Offset = offset;
		}

		public HouseDeed( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version

			writer.Write( m_Offset );

			writer.Write( m_MultiID );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 1:
				{
					m_Offset = reader.ReadPoint3D();

					goto case 0;
				}
				case 0:
				{
					m_MultiID = reader.ReadInt();

					break;
				}
			}

			if ( Weight == 0.0 )
				Weight = 1.0;
		}

		public override void OnDoubleClick( Mobile from )
		{
            if (from is BaseCreature)
            {
                from.SendMessage("You can't place a house as a monster!"); // pseudoseer is possessing a monster
                return;
            }
            
            if ( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			}
			else if ( from.AccessLevel < AccessLevel.GameMaster && BaseHouse.HasAccountHouse( from ) )
			{
				from.SendLocalizedMessage( 501271 ); // You already own a house, you may not place another!
			}
			else
			{
				from.SendLocalizedMessage( 1010433 ); /* House placement cancellation could result in a
													   * 60 second delay in the return of your deed.
													   */

				from.Target = new HousePlacementTarget( this );
			}
		}

		public abstract BaseHouse GetHouse( Mobile owner );
		public abstract Rectangle2D[] Area{ get; }

		public void OnPlacement( Mobile from, Point3D p )
		{
			if ( Deleted )
				return;

			if ( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			}
			else if ( from.AccessLevel < AccessLevel.GameMaster && BaseHouse.HasAccountHouse( from ) )
			{
				from.SendLocalizedMessage( 501271 ); // You already own a house, you may not place another!
			}
			else
			{
				ArrayList toMove;
				Point3D center = new Point3D( p.X - m_Offset.X, p.Y - m_Offset.Y, p.Z - m_Offset.Z );
				HousePlacementResult res = HousePlacement.Check( from, m_MultiID, center, out toMove );

				switch ( res )
				{
					case HousePlacementResult.Valid:
					{
						BaseHouse house = GetHouse( from );
						house.MoveToWorld( center, from.Map );
						Delete();

						for ( int i = 0; i < toMove.Count; ++i )
						{
							object o = toMove[i];

							if ( o is Mobile )
								((Mobile)o).Location = house.BanLocation;
							else if ( o is Item )
								((Item)o).Location = house.BanLocation;
						}

						break;
					}
					case HousePlacementResult.BadItem:
					case HousePlacementResult.BadLand:
					case HousePlacementResult.BadStatic:
					case HousePlacementResult.BadRegionHidden:
					{
						from.SendLocalizedMessage( 1043287 ); // The house could not be created here.  Either something is blocking the house, or the house would not be on valid terrain.
						break;
					}
					case HousePlacementResult.NoSurface:
					{
						from.SendMessage( "The house could not be created here.  Part of the foundation would not be on any surface." );
						break;
					}
					case HousePlacementResult.BadRegion:
					{
						from.SendLocalizedMessage( 501265 ); // Housing cannot be created in this area.
						break;
					}
					case HousePlacementResult.BadRegionTemp:
					{
						from.SendLocalizedMessage( 501270 ); //Lord British has decreed a 'no build' period, thus you cannot build this house at this time.
						break;
					}
				}
			}
		}
	}

    //Traditional Houses
    public class BlueTentDeed : HouseDeed
    {
        [Constructable]
        public BlueTentDeed()
            : base(0x71, new Point3D(0, 4, 0))
        {
        }

        public BlueTentDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1041586;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return Tent.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new Tent(owner, 0x71);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class GreenTentDeed : HouseDeed
    {
        [Constructable]
        public GreenTentDeed()
            : base(0x72, new Point3D(0, 4, 0))
        {
        }

        public GreenTentDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1041587;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return Tent.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new Tent(owner, 0x72);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class StonePlasterHouseDeed : HouseDeed
    {
        [Constructable]
        public StonePlasterHouseDeed()
            : base(0x64, new Point3D(0, 4, 0))
        {
        }

        public StonePlasterHouseDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1041211;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return SmallOldHouse.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new SmallOldHouse(owner, 0x64);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class FieldStoneHouseDeed : HouseDeed
    {
        [Constructable]
        public FieldStoneHouseDeed()
            : base(0x66, new Point3D(0, 4, 0))
        {
        }

        public FieldStoneHouseDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1041212;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return SmallOldHouse.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new SmallOldHouse(owner, 0x66);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class FieldStoneHouseDeedE : HouseDeed
    {
        [Constructable]
        public FieldStoneHouseDeedE()
            : base(0x15F, new Point3D(4, 0, 0))
        {
        }

        public FieldStoneHouseDeedE(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1041583;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return SmallOldHouseE.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new SmallOldHouseE(owner, 0x15F);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class SmallBrickHouseDeed : HouseDeed
    {
        [Constructable]
        public SmallBrickHouseDeed()
            : base(0x68, new Point3D(0, 4, 0))
        {
        }

        public SmallBrickHouseDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1041213;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return SmallOldHouse.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new SmallOldHouse(owner, 0x68);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class SmallBrickHouseDeedE : HouseDeed
    {
        [Constructable]
        public SmallBrickHouseDeedE()
            : base(0x69, new Point3D(4, 0, 0))
        {
        }

        public SmallBrickHouseDeedE(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1041584;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return SmallOldHouseE.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new SmallOldHouseE(owner, 0x69);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class WoodHouseDeed : HouseDeed
    {
        [Constructable]
        public WoodHouseDeed()
            : base(0x6A, new Point3D(0, 4, 0))
        {
        }

        public WoodHouseDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1041214;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return SmallOldHouse.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new SmallOldHouse(owner, 0x6A);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class WoodPlasterHouseDeed : HouseDeed
    {
        [Constructable]
        public WoodPlasterHouseDeed()
            : base(0x6C, new Point3D(0, 4, 0))
        {
        }

        public WoodPlasterHouseDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1041215;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return SmallOldHouse.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new SmallOldHouse(owner, 0x6C);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class ThatchedRoofCottageDeed : HouseDeed
    {
        [Constructable]
        public ThatchedRoofCottageDeed()
            : base(0x6E, new Point3D(0, 4, 0))
        {
        }

        public ThatchedRoofCottageDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1041216;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return SmallOldHouse.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new SmallOldHouse(owner, 0x6E);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class BrickHouseDeed : HouseDeed
    {
        [Constructable]
        public BrickHouseDeed()
            : base(0x74, new Point3D(-1, 7, 0))
        {
        }

        public BrickHouseDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1041219;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return GuildHouse.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new GuildHouse(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class TwoStoryWoodPlasterHouseDeed : HouseDeed
    {
        [Constructable]
        public TwoStoryWoodPlasterHouseDeed()
            : base(0x76, new Point3D(-3, 7, 0))
        {
        }

        public TwoStoryWoodPlasterHouseDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1041220;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return TwoStoryHouse.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new TwoStoryHouse(owner, 0x76);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class TwoStoryStonePlasterHouseDeed : HouseDeed
    {
        [Constructable]
        public TwoStoryStonePlasterHouseDeed()
            : base(0x78, new Point3D(-3, 7, 0))
        {
        }

        public TwoStoryStonePlasterHouseDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1041221;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return TwoStoryHouse.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new TwoStoryHouse(owner, 0x78);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class TowerDeed : HouseDeed
    {
        [Constructable]
        public TowerDeed()
            : base(0x7A, new Point3D(0, 7, 0))
        {
        }

        public TowerDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1041222;
            }
        }

        public override Rectangle2D[] Area
        {
            get
            {
                return Tower.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new Tower(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class TowerDeedE : HouseDeed
    {
        [Constructable]
        public TowerDeedE()
            : base(0xEB, new Point3D(8, 0, 0))
        {
        }

        public TowerDeedE(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1031240;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return TowerE.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new TowerE(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class KeepDeed : HouseDeed
    {
        [Constructable]
        public KeepDeed()
            : base(0x81, new Point3D(0, 11, 0))
        {
        }

        public KeepDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1041223;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return Keep.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new Keep(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class CastleDeed : HouseDeed
    {
        [Constructable]
        public CastleDeed()
            : base(0x7E, new Point3D(0, 16, 0))
        {
        }

        public CastleDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1041224;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return Castle.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new Castle(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class LargePatioDeed : HouseDeed
    {
        [Constructable]
        public LargePatioDeed()
            : base(0x8C, new Point3D(-4, 7, 0))
        {
        }

        public LargePatioDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1041231;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return LargePatioHouse.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new LargePatioHouse(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class LargeMarbleDeed : HouseDeed
    {
        [Constructable]
        public LargeMarbleDeed()
            : base(0x96, new Point3D(-4, 7, 0))
        {
        }

        public LargeMarbleDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1041236;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return LargeMarbleHouse.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new LargeMarbleHouse(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class SmallTowerDeed : HouseDeed
    {
        [Constructable]
        public SmallTowerDeed()
            : base(0x98, new Point3D(3, 4, 0))
        {
        }

        public SmallTowerDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1041237;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return SmallTower.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new SmallTower(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class SmallTowerDeedE : HouseDeed
    {
        [Constructable]
        public SmallTowerDeedE()
            : base(0x13F, new Point3D(4, 1, 0))
        {
        }

        public SmallTowerDeedE(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1031241;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return SmallTowerE.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new SmallTowerE(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class LogCabinDeed : HouseDeed
    {
        [Constructable]
        public LogCabinDeed()
            : base(0x9A, new Point3D(1, 6, 0))
        {
        }

        public LogCabinDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1041238;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return LogCabin.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new LogCabin(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class SandstonePatioDeed : HouseDeed
    {
        [Constructable]
        public SandstonePatioDeed()
            : base(0x9C, new Point3D(-1, 4, 0))
        {
        }

        public SandstonePatioDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1041239;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return SandStonePatio.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new SandStonePatio(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class VillaDeed : HouseDeed
    {
        [Constructable]
        public VillaDeed()
            : base(0x9E, new Point3D(3, 6, 0))
        {
        }

        public VillaDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1041240;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return TwoStoryVilla.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new TwoStoryVilla(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class StoneWorkshopDeed : HouseDeed
    {
        [Constructable]
        public StoneWorkshopDeed()
            : base(0xA0, new Point3D(-1, 4, 0))
        {
        }

        public StoneWorkshopDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1041241;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return SmallShop.AreaArray2;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new SmallShop(owner, 0xA0);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class MarbleWorkshopDeed : HouseDeed
    {
        [Constructable]
        public MarbleWorkshopDeed()
            : base(0xA2, new Point3D(-1, 4, 0))
        {
        }

        public MarbleWorkshopDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1041242;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return SmallShop.AreaArray1;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new SmallShop(owner, 0xA2);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    //Phase 2 Houses
    //Blackrock Collection
    public class BlackrockSmallTowerDeedE : HouseDeed
    {
        [Constructable]
        public BlackrockSmallTowerDeedE()
            : base(0x104, new Point3D(4, 0, 0))
        {
        }

        public BlackrockSmallTowerDeedE(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1031242;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return BlackrockSmallTowerE.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new BlackrockSmallTowerE(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class BlackrockScoutTowerDeed : HouseDeed
    {
        [Constructable]
        public BlackrockScoutTowerDeed()
            : base(0xF2, new Point3D(0, 4, 0))
        {
        }

        public BlackrockScoutTowerDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1031243;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return BlackrockScoutTower.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new BlackrockScoutTower(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class BlackrockTowerDeedE : HouseDeed
    {
        [Constructable]
        public BlackrockTowerDeedE()
            : base(0xEF, new Point3D(4, 0, 0))
        {
        }

        public BlackrockTowerDeedE(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1031244;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return BlackrockTowerE.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new BlackrockTowerE(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class BlackrockOutpostDeed : HouseDeed
    {
        [Constructable]
        public BlackrockOutpostDeed()
            : base(0xF1, new Point3D(0, 5, 0))
        {
        }

        public BlackrockOutpostDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1031245;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return BlackrockOutpost.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new BlackrockOutpost(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class BlackrockKeepDeed : HouseDeed
    {
        [Constructable]
        public BlackrockKeepDeed()
            : base(0xF0, new Point3D(0, 6, 0))
        {
        }

        public BlackrockKeepDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1031246;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return BlackrockKeep.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new BlackrockKeep(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class BlackrockFortDeed : HouseDeed
    {
        [Constructable]
        public BlackrockFortDeed()
            : base(0xF3, new Point3D(0, 11, 0))
        {
        }

        public BlackrockFortDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1031247;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return BlackrockFort.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new BlackrockFort(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    //Skara Collection
    public class SkaraSmallShoppeDeed : HouseDeed
    {
        [Constructable]
        public SkaraSmallShoppeDeed()
            : base(0x11B, new Point3D(0, 5, 0))
        {
        }

        public SkaraSmallShoppeDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1031250;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return SkaraSmallShoppe.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new SkaraSmallShoppe(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class SkaraVerandaEDeed : HouseDeed
    {
        [Constructable]
        public SkaraVerandaEDeed()
            : base(0xF8, new Point3D(4, 0, 0))
        {
        }

        public SkaraVerandaEDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1031248;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return SkaraVerandaE.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new SkaraVerandaE(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class SkaraSimpleHouseDeed : HouseDeed
    {
        [Constructable]
        public SkaraSimpleHouseDeed()
            : base(0xF6, new Point3D(-1, 5, 0))
        {
        }

        public SkaraSimpleHouseDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1031249;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return SkaraSimpleHouse.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new SkaraSimpleHouse(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class SkaraRanchHouseEDeed : HouseDeed
    {
        [Constructable]
        public SkaraRanchHouseEDeed()
            : base(0x11C, new Point3D(5, 0, 0))
        {
        }

        public SkaraRanchHouseEDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1031252;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return SkaraRanchHouseE.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new SkaraRanchHouseE(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class SkaraAdobeDeed : HouseDeed
    {
        [Constructable]
        public SkaraAdobeDeed()
            : base(0x11E, new Point3D(0, 4, 0))
        {
        }

        public SkaraAdobeDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1031253;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return SkaraAdobe.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new SkaraAdobe(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class SkaraCourtyardShoppeDeed : HouseDeed
    {
        [Constructable]
        public SkaraCourtyardShoppeDeed()
            : base(0x11A, new Point3D(0, 6, 0))
        {
        }

        public SkaraCourtyardShoppeDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1031254;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return SkaraCourtyardShoppe.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new SkaraCourtyardShoppe(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class SkaraCourtyardHouseDeed : HouseDeed
    {
        [Constructable]
        public SkaraCourtyardHouseDeed()
            : base(0x198, new Point3D(0, 6, 0))
        {
        }

        public SkaraCourtyardHouseDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1031318;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return SkaraCourtyardHouse.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new SkaraCourtyardHouse(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class SkaraManorDeed : HouseDeed
    {
        [Constructable]
        public SkaraManorDeed()
            : base(0x107, new Point3D(0, 7, 0))
        {
        }

        public SkaraManorDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1031319;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return SkaraManor.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new SkaraManor(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class SkaraTwoStoryRanchHouseDeed : HouseDeed
    {
        [Constructable]
        public SkaraTwoStoryRanchHouseDeed()
            : base(0x10E, new Point3D(0, 0, 0))
        {
        }

        public SkaraTwoStoryRanchHouseDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1031255;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return SkaraTwoStoryRanchHouse.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new SkaraTwoStoryRanchHouse(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class SkaraVillaEDeed : HouseDeed
    {
        [Constructable]
        public SkaraVillaEDeed()
            : base(0x148, new Point3D(5, 0, 0))
        {
        }

        public SkaraVillaEDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1031256;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return SkaraVillaE.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new SkaraVillaE(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    //Trinsic Collection
    public class TrinsicShopDeed : HouseDeed
    {
        [Constructable]
        public TrinsicShopDeed()
            : base(0xF5, new Point3D(-1, 5, 0))
        {
        }

        public TrinsicShopDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1031257;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return TrinsicShop.AreaArray;
            }

        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new TrinsicShop(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class TrinsicVillaDeed : HouseDeed
    {
        [Constructable]
        public TrinsicVillaDeed()
            : base(0xE2, new Point3D(0, 6, 0))
        {
        }

        public TrinsicVillaDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1031258;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return TrinsicVilla.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new TrinsicVilla(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class TrinsicPatioResidenceDeedE : HouseDeed
    {
        [Constructable]
        public TrinsicPatioResidenceDeedE()
            : base(0xE5, new Point3D(6, 0, 0))
        {
        }

        public TrinsicPatioResidenceDeedE(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1031259;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return TrinsicPatioResidenceE.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new TrinsicPatioResidenceE(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class TrinsicPatioVillaDeed : HouseDeed
    {
        [Constructable]
        public TrinsicPatioVillaDeed()
            : base(0xE4, new Point3D(-1, 4, 0))
        {
        }

        public TrinsicPatioVillaDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1031260;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return TrinsicPatioVilla.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new TrinsicPatioVilla(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class TrinsicFortifiedVillaDeedE : HouseDeed
    {
        [Constructable]
        public TrinsicFortifiedVillaDeedE()
            : base(0xE6, new Point3D(6, 0, 0))
        {
        }

        public TrinsicFortifiedVillaDeedE(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1031261;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return TrinsicFortifiedVillaE.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new TrinsicFortifiedVillaE(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class TrinsicCourtyardHouseDeed : HouseDeed
    {
        [Constructable]
        public TrinsicCourtyardHouseDeed()
            : base(0xDE, new Point3D(0, 7, 0))
        {
        }

        public TrinsicCourtyardHouseDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1031320;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return TrinsicCourtyardHouse.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new TrinsicCourtyardHouse(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class TrinsicManorDeed : HouseDeed
    {
        [Constructable]
        public TrinsicManorDeed()
            : base(0xF4, new Point3D(-1, 6, 0))
        {
        }

        public TrinsicManorDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1031262;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return TrinsicManor.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new TrinsicManor(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class TrinsicKeepDeed : HouseDeed
    {
        [Constructable]
        public TrinsicKeepDeed()
            : base(0x82, new Point3D(0, 11, 0))
        {
        }

        public TrinsicKeepDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1031263;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return TrinsicKeepHome.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new TrinsicKeepHome(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    //Yew Collection
    public class SmallYewCabinDeed : HouseDeed
    {
        [Constructable]
        public SmallYewCabinDeed()
            : base(0x135, new Point3D(0, 3, 0))
        {
        }

        public SmallYewCabinDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1031264;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return SmallYewCabin.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new SmallYewCabin(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class YewLogHomeDeedE : HouseDeed
    {
        [Constructable]
        public YewLogHomeDeedE()
            : base(0x144, new Point3D(0, 5, 0))
        {
        }

        public YewLogHomeDeedE(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1031265;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return YewLogHomeE.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new YewLogHomeE(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class YewResidenceDeedE : HouseDeed
    {
        [Constructable]
        public YewResidenceDeedE()
            : base(0x139, new Point3D(6, 0, 0))
        {
        }

        public YewResidenceDeedE(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1031266;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return YewResidenceE.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new YewResidenceE(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class YewVerandaDeedE : HouseDeed
    {
        [Constructable]
        public YewVerandaDeedE()
            : base(0x114, new Point3D(7, 0, 0))
        {
        }

        public YewVerandaDeedE(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1031267;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return YewVerandaE.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new YewVerandaE(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class YewTwoStoryResidenceDeedE : HouseDeed
    {
        [Constructable]
        public YewTwoStoryResidenceDeedE()
            : base(0x137, new Point3D(9, -3, 0))
        {
        }

        public YewTwoStoryResidenceDeedE(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1031268;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return YewTwoStoryResidenceE.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new YewTwoStoryResidenceE(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    //Phase 3 Houses
    //Deceit Collection
    public class DeceitSmallHouseDeed : HouseDeed
    {
        [Constructable]
        public DeceitSmallHouseDeed()
            : base(0x16B, new Point3D(0, 4, 0))
        {
        }

        public DeceitSmallHouseDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1031269;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return DeceitSmallHouse.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new DeceitSmallHouse(owner, 0x66);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class DeceitWatchtowerDeed : HouseDeed
    {
        [Constructable]
        public DeceitWatchtowerDeed()
            : base(0x16D, new Point3D(-1, 4, 0))
        {
        }

        public DeceitWatchtowerDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1031270;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return DeceitWatchtower.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new DeceitWatchtower(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class DeceitLodgeDeed : HouseDeed
    {
        [Constructable]
        public DeceitLodgeDeed()
            : base(0x168, new Point3D(0, 8, 0))
        {
        }

        public DeceitLodgeDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1031271;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return DeceitLodge.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new DeceitLodge(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class DeceitGreatLodgeDeed : HouseDeed
    {
        [Constructable]
        public DeceitGreatLodgeDeed()
            : base(0x167, new Point3D(0, 7, 0))
        {
        }

        public DeceitGreatLodgeDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1031272;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return DeceitGreatLodge.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new DeceitGreatLodge(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class DeceitChaletDeed : HouseDeed
    {
        [Constructable]
        public DeceitChaletDeed()
            : base(0x166, new Point3D(-1, 7, 0))
        {
        }

        public DeceitChaletDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1031273;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return DeceitChalet.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new DeceitChalet(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class DeceitLonghouseDeedE : HouseDeed
    {
        [Constructable]
        public DeceitLonghouseDeedE()
            : base(0x169, new Point3D(10, 0, 0))
        {
        }

        public DeceitLonghouseDeedE(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1031274;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return DeceitLonghouseE.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new DeceitLonghouseE(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class DeceitGreatHallDeed : HouseDeed
    {
        [Constructable]
        public DeceitGreatHallDeed()
            : base(0x16A, new Point3D(-2, 8, 0))
        {
        }

        public DeceitGreatHallDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1031275;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return DeceitGreatHall.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new DeceitGreatHall(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class DeceitFarmsteadDeedE : HouseDeed
    {
        [Constructable]
        public DeceitFarmsteadDeedE()
            : base(0x165, new Point3D(8, 0, 0))
        {
        }

        public DeceitFarmsteadDeedE(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1031276;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return DeceitFarmsteadE.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new DeceitFarmsteadE(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class DeceitGrandLodgeDeed : HouseDeed
    {
        [Constructable]
        public DeceitGrandLodgeDeed()
            : base(0x16C, new Point3D(-1, 12, 0))
        {
        }

        public DeceitGrandLodgeDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1031277;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return DeceitGrandLodge.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new DeceitGrandLodge(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    //Minoc Collection
    public class MinocSmallHouseDeed : HouseDeed
    {
        [Constructable]
        public MinocSmallHouseDeed()
            : base(0x173, new Point3D(0, 4, 0))
        {
        }

        public MinocSmallHouseDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1031283;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return MinocSmallHouse.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new MinocSmallHouse(owner, 0x66);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class MinocVillaEDeed : HouseDeed
    {
        [Constructable]
        public MinocVillaEDeed()
            : base(0x176, new Point3D(4, 0, 0))
        {
        }

        public MinocVillaEDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1031284;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return MinocVillaE.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new MinocVillaE(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class MinocLoftEDeed : HouseDeed
    {
        [Constructable]
        public MinocLoftEDeed()
            : base(0x174, new Point3D(6, 0, 0))
        {
        }

        public MinocLoftEDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1031287;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return MinocLoftE.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new MinocLoftE(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class MinocLargeVillaDeed : HouseDeed
    {
        [Constructable]
        public MinocLargeVillaDeed()
            : base(0x170, new Point3D(0, 7, 0))
        {
        }

        public MinocLargeVillaDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1041289;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return MinocLargeVilla.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new MinocLargeVilla(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class MinocBungaloDeed : HouseDeed
    {
        [Constructable]
        public MinocBungaloDeed()
            : base(0x177, new Point3D(0, 8, 0))
        {
        }

        public MinocBungaloDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1041290;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return MinocBungalo.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new MinocBungalo(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    //Vesper Collection
    public class VesperShopDeed : HouseDeed
    {
        [Constructable]
        public VesperShopDeed()
            : base(0x143, new Point3D(0, 4, 0))
        {
        }

        public VesperShopDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1031278;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return VesperShop.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new VesperShop(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class VesperSlateRanchHouseDeed : HouseDeed
    {
        [Constructable]
        public VesperSlateRanchHouseDeed()
            : base(0x13C, new Point3D(0, 6, 0))
        {
        }

        public VesperSlateRanchHouseDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1031286;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return VesperSlateRanchHouse.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new VesperSlateRanchHouse(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class VesperCabinDeed : HouseDeed
    {
        [Constructable]
        public VesperCabinDeed()
            : base(0x136, new Point3D(0, 6, 0))
        {
        }

        public VesperCabinDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1031279;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return VesperCabin.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new VesperCabin(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class VesperOverlookEDeed : HouseDeed
    {
        [Constructable]
        public VesperOverlookEDeed()
            : base(0x145, new Point3D(5, 0, 0))
        {
        }

        public VesperOverlookEDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1031280;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return VesperOverlookE.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new VesperOverlookE(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class VesperTwoStoryCabinDeed : HouseDeed
    {
        [Constructable]
        public VesperTwoStoryCabinDeed()
            : base(0x138, new Point3D(0, 6, 0))
        {
        }

        public VesperTwoStoryCabinDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1031281;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return VesperTwoStoryCabin.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new VesperTwoStoryCabin(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class VesperCompoundDeed : HouseDeed
    {
        [Constructable]
        public VesperCompoundDeed()
            : base(0x146, new Point3D(-1, 11, 0))
        {
        }

        public VesperCompoundDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1031282;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return VesperCompound.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new VesperCompound(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    //Wind Collection

    public class WindSmallTowerDeedE : HouseDeed
    {
        [Constructable]
        public WindSmallTowerDeedE()
            : base(0x151, new Point3D(4, 0, 0))
        {
        }

        public WindSmallTowerDeedE(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1031321;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return WindSmallTowerE.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new WindSmallTowerE(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class WindSmallTowerDeed : HouseDeed
    {
        [Constructable]
        public WindSmallTowerDeed()
            : base(0xDD, new Point3D(1, 4, 0))
        {
        }

        public WindSmallTowerDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1041322;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return WindSmallTower.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new WindSmallTower(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class WindMansionDeed : HouseDeed
    {
        [Constructable]
        public WindMansionDeed()
            : base(0x189, new Point3D(-1, 12, 0))
        {
        }

        public WindMansionDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1031323;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return WindMansion.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new WindMansion(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    //Phase 4 Houses
    //Nujel'm Collection
    public class NujelmSmallHouseDeedE : HouseDeed
    {
        [Constructable]
        public NujelmSmallHouseDeedE()
            : base(0x14D, new Point3D(4, 0, 0))
        {
        }

        public NujelmSmallHouseDeedE(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1031298;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return NujelmSmallHouseE.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new NujelmSmallHouseE(owner, 0x15F);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class NujelmTwoStoryBalconyDeed : HouseDeed
    {
        [Constructable]
        public NujelmTwoStoryBalconyDeed()
            : base(0x14C, new Point3D(0, 4, 0))
        {
        }

        public NujelmTwoStoryBalconyDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1031294;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return NujelmTwoStoryBalcony.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new NujelmTwoStoryBalcony(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class NujelmVillaDeedE : HouseDeed
    {
        [Constructable]
        public NujelmVillaDeedE()
            : base(0x14E, new Point3D(5, 0, 0))
        {
        }

        public NujelmVillaDeedE(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1031295;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return NujelmVillaE.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new NujelmVillaE(owner, 0x15F);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class NujelmCourtyardManorDeed : HouseDeed
    {
        [Constructable]
        public NujelmCourtyardManorDeed()
            : base(0x14B, new Point3D(-1, 10, 0))
        {
        }

        public NujelmCourtyardManorDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1031293;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return NujelmCourtyardManor.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new NujelmCourtyardManor(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class NujelmTwoStoryRanchHouseDeed : HouseDeed
    {
        [Constructable]
        public NujelmTwoStoryRanchHouseDeed()
            : base(0x14A, new Point3D(0, 7, 0))
        {
        }

        public NujelmTwoStoryRanchHouseDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1031294;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return NujelmTwoStoryRanchHouse.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new NujelmTwoStoryRanchHouse(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }


    //Magincia Collection
    public class MaginciaWagonDeed : HouseDeed
    {
        [Constructable]
        public MaginciaWagonDeed()
            : base(0x153, new Point3D(0, 2, 0))
        {
        }

        public MaginciaWagonDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1031301;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return MaginciaWagon.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new MaginciaWagon(owner, 0x66);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class MaginciaWagonDeedE : HouseDeed
    {
        [Constructable]
        public MaginciaWagonDeedE()
            : base(0x154, new Point3D(2, 0, 0))
        {
        }

        public MaginciaWagonDeedE(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1031302;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return MaginciaWagonE.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new MaginciaWagonE(owner, 0x15F);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class MaginciaHalfShoppeEDeed : HouseDeed
    {
        [Constructable]
        public MaginciaHalfShoppeEDeed()
            : base(0x10B, new Point3D(4, 0, 0))
        {
        }

        public MaginciaHalfShoppeEDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1031299;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return MaginciaHalfShoppeE.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new MaginciaHalfShoppeE(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class MaginciaShoppeEDeed : HouseDeed
    {
        [Constructable]
        public MaginciaShoppeEDeed()
            : base(0x10F, new Point3D(4, 0, 0))
        {
        }

        public MaginciaShoppeEDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1031300;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return MaginciaShoppeE.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new MaginciaShoppeE(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class MaginciaOutpostEDeed : HouseDeed
    {
        [Constructable]
        public MaginciaOutpostEDeed()
            : base(0x11D, new Point3D(4, 0, 0))
        {
        }

        public MaginciaOutpostEDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1031306;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return MaginciaOutpostE.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new MaginciaOutpostE(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class MaginciaTwoStoryHouseEDeed : HouseDeed
    {
        [Constructable]
        public MaginciaTwoStoryHouseEDeed()
            : base(0x10D, new Point3D(4, 0, 0))
        {
        }

        public MaginciaTwoStoryHouseEDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1031251;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return MaginciaTwoStoryHouseE.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new MaginciaTwoStoryHouseE(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class MaginciaResidenceEDeed : HouseDeed
    {
        [Constructable]
        public MaginciaResidenceEDeed()
            : base(0x111, new Point3D(4, 0, 0))
        {
        }

        public MaginciaResidenceEDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1031303;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return MaginciaResidenceE.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new MaginciaResidenceE(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class MaginciaVillaDeed : HouseDeed
    {
        [Constructable]
        public MaginciaVillaDeed()
            : base(0x10C, new Point3D(-1, 5, 0))
        {
        }

        public MaginciaVillaDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1031304;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return MaginciaVilla.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new MaginciaVilla(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    //Lord & Lady Collection
    public class SkaraLordCompoundDeed : HouseDeed
    {
        [Constructable]
        public SkaraLordCompoundDeed()
            : base(0xD2, new Point3D(0, 14, 0))
        {
        }

        public SkaraLordCompoundDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1031326;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return SkaraLordCompound.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new SkaraLordCompound(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class TrinsicLordFortressDeed : HouseDeed
    {
        [Constructable]
        public TrinsicLordFortressDeed()
            : base(0xF9, new Point3D(0, 15, 0))
        {
        }

        public TrinsicLordFortressDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1031327;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return TrinsicLordFortress.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new TrinsicLordFortress(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class YewLordFarmsteadDeed : HouseDeed
    {
        [Constructable]
        public YewLordFarmsteadDeed()
            : base(0xD3, new Point3D(0, 13, 0))
        {
        }

        public YewLordFarmsteadDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1031328;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return YewLordFarmstead.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new YewLordFarmstead(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class WindCompoundDeed : HouseDeed
    {
        [Constructable]
        public WindCompoundDeed()
            : base(0xD5, new Point3D(0, 14, 0))
        {
        }

        public WindCompoundDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1031324;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return WindCompound.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new WindCompound(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class NujelmLordCompoundDeed : HouseDeed
    {
        [Constructable]
        public NujelmLordCompoundDeed()
            : base(0x188, new Point3D(0, 15, 0))
        {
        }

        public NujelmLordCompoundDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1031329;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return NujelmLordCompound.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new NujelmLordCompound(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class MaginciaLordCompoundDeed : HouseDeed
    {
        [Constructable]
        public MaginciaLordCompoundDeed()
            : base(0xFA, new Point3D(0, 15, 0))
        {
        }

        public MaginciaLordCompoundDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1031330;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return MaginciaLordCompound.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new MaginciaLordCompound(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class GrandKeepDeed : HouseDeed
    {
        [Constructable]
        public GrandKeepDeed()
            : base(0x181, new Point3D(0, 12, 0))
        {
        }

        public GrandKeepDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1031325;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return GrandKeep.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new GrandKeep(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class FortressDeed : HouseDeed
    {
        [Constructable]
        public FortressDeed()
            : base(0x80, new Point3D(0, 16, 0))
        {
        }

        public FortressDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1041500;
            }
        }
        public override Rectangle2D[] Area
        {
            get
            {
                return Fortress.AreaArray;
            }
        }
        public override BaseHouse GetHouse(Mobile owner)
        {
            return new Fortress(owner);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}