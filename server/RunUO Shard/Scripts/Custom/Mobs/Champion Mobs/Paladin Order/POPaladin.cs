using System;
using Server.Items;
using Server.Factions;
using Server.Scripts.Engines.Factions.Instances.Factions;

namespace Server.Mobiles
{
	public class POPaladin : BaseCreature
	{
		[Constructable]
        //Had to change A_Paladin to AI_Melee. Need to import additional AI's.7/9/22
        public POPaladin()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, .2, .4)
		{
            Body = 0x190;
            Name = NameList.RandomName("male");
            Title = "the Paladin";

            SetStr(126, 150);
            SetDex(61, 85);
            SetInt(81, 95);

            AddItem(new VirtualMountItem(this));
            AddItem(new PlateHelm());
            AddItem(new PlateLegs());
            AddItem(new PlateArms());
            AddItem(new PlateGloves());
            AddItem(new PlateGorget());
            AddItem(new PlateChest());
            AddItem(Immovable(Rehued(new Lance(), 2141)));
            AddItem(Immovable(Rehued(new OrderShield(), 2141)));
            AddItem(Rehued(new BodySash(), 2141));
            AddItem(Rehued(new Kilt(), 2141));

            SetSkill(SkillName.Swords, 100.0, 110.0);
            SetSkill(SkillName.Wrestling, 100.0, 110.0);
            SetSkill(SkillName.Tactics, 100.0, 110.0);
            SetSkill(SkillName.MagicResist, 100.0, 110.0);
            SetSkill(SkillName.Healing, 100.0, 110.0);
            SetSkill(SkillName.Anatomy, 100.0, 110.0);
            SetSkill(SkillName.Chivalry, 100.0, 110.0);
            SetSkill(SkillName.Focus, 100.0, 110.0);

            Fame = 1050;
            Karma = -1050;
            VirtualArmor = 40;

        }

        public POPaladin(Serial serial)
			: base(serial)
		{ }
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
        public override Faction FactionAllegiance { get { return PaladinOrder.Instance; } }
        public override bool InitialInnocent { get { return true; } }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            c.Delete();
        }
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

            public void OnRiderDamaged(int amount, Mobile from, bool willKill)
            {
                throw new NotImplementedException();
            }
        }
        public class VirtualMountItem : Item, IMountItem
        {
            private readonly VirtualMount m_Mount;
            private Mobile m_Rider;
            public VirtualMountItem(Mobile mob)
                : base(793)
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
        public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}
