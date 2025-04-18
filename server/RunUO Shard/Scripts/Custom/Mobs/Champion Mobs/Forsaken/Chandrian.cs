using System;
using Server;
using Server.Misc;
using Server.Factions;
using Server.Items;
using Server.Mobiles;
using Server.Scripts.Engines.Factions.Instances.Factions;

namespace Server.Mobiles
{
	public class Chandrian : BaseCreature
	{

        [Constructable]
        public Chandrian() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Body = 0x190;
            Hue = 1;
            Name = "a chandrian";

            AddItem(Rehued(new HoodedShroudOfShadows(), 1109));
            AddItem(Immovable(Rehued(new BoneHarvester(), 2149)));
            AddItem(Immovable(Rehued(new ChaosShield(), 2149)));
            AddItem(Immovable(Rehued(new HalfApron(), 2149)));

            SetStr(151, 175);
            SetDex(61, 85);
            SetInt(151, 175);

            VirtualArmor = 50;

            SetSkill(SkillName.Wrestling, 110.0, 120.0);
            SetSkill(SkillName.MagicResist, 110.0, 120.0);
            SetSkill(SkillName.Healing, 110.0, 120.0);
            SetSkill(SkillName.Anatomy, 110.0, 120.0);
            SetSkill(SkillName.Magery, 110.0, 120.0);
            SetSkill(SkillName.EvalInt, 110.0, 120.0);
            SetSkill(SkillName.Meditation, 110.0, 120.0);

            Fame = 1750;
            Karma = -1750;

        }

        public Chandrian(Serial serial) : base(serial)
        {
        }
        public Item Rehued(Item item, int hue)
        {
            item.Hue = hue;
            return item;
        }
        public Item Immovable(Item item)
        {
            item.Movable = false;
            return item;
        }
        public override bool AlwaysAttackable { get { return true; } }
        public override Faction FactionAllegiance { get { return Forsaken.Instance; } }
        public override OppositionGroup OppositionGroup
        {
            get
            {
                return OppositionGroup.FeyAndUndead;
            }
        }

        //public override double HealChance { get { return 1.0; } }
        public override bool ShowFameTitle{ get{ return false; } }
		public override bool ClickTitle{ get{ return true; } }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            c.Delete();
        }
        //Added to fix list. Commented out during project merger so it wouldn't effect the build. 7/9/22
        /*
        public class VirtualMount : IMount
        {
            private readonly VirtualMountItem m_Item;
            public VirtualMount(VirtualMountItem item)
            {
                this.m_Item = item;
            }

            public Mobile Rider
            {
                get
                {
                    return this.m_Item.Rider;
                }
                set
                {
                }
            }
            public virtual void OnRiderDamaged(Mobile from, ref int amount, bool willKill)
            {
            }
        }
        public class VirtualMountItem : Item, IMountItem
        {
            private readonly VirtualMount m_Mount;
            private Mobile m_Rider;
            public VirtualMountItem(Mobile mob)
                : base(0x3EA0)
            {
                this.Layer = Layer.Mount;

                this.m_Rider = mob;
                this.m_Mount = new VirtualMount(this);
            }

            public VirtualMountItem(Serial serial)
                : base(serial)
            {
                this.m_Mount = new VirtualMount(this);
            }

            public Mobile Rider
            {
                get
                {
                    return this.m_Rider;
                }
            }
            public IMount Mount
            {
                get
                {
                    return this.m_Mount;
                }
            }
        public override void Serialize(GenericWriter writer)
            {
                base.Serialize(writer);

                writer.Write((int)0); // version

                writer.Write((Mobile)this.m_Rider);
            }

            public override void Deserialize(GenericReader reader)
            {
                base.Deserialize(reader);

                int version = reader.ReadInt();

                this.m_Rider = reader.ReadMobile();

                if (this.m_Rider == null)
                    this.Delete();
            }
        }
        */
        public override void Serialize( GenericWriter writer )
		{
		    base.Serialize( writer );
	        writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}
