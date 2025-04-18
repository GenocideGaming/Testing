using Server;
using System;
using Server.Items;
using Server.Factions;
using Server.Mobiles;
using System.Collections.Generic;
using Server.Scripts.Engines.Factions.Instances.Factions;

namespace Server.Mobiles
{

    public class Gojira : BaseCreature
    {
        [Constructable]
        public Gojira() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
        {
            Name = "Gojira";
            Title = "the Chieftain";

            Body = 400;
            Hue = 0X8A5;
            BaseSoundID = 0x45A;

            AddItem(new OrcHelm());
            AddItem(Rehued(new BodySash(), 2137));
            AddItem(new ThighBoots());
            AddItem(Rehued(new Kilt(), 2137));
            AddItem(Immovable(Rehued(new WarHammer(), 2133)));
            AddItem(Rehued(new PlateArms(), 2133));
            AddItem(Rehued(new RingmailGloves(), 2133));
            AddItem(Rehued(new RingmailChest(), 2133));
            AddItem(new VirtualMountItem(this));

            SetStr(500);
            SetDex(200);
            SetInt(100);

            SetHits(10000);
            SetStam(500);
            SetMana(300);

            SetDamage(25, 35);

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 70;

            SetSkill(SkillName.EvalInt, 195.0, 220.0);
            SetSkill(SkillName.Mysticism, 195.0, 220.0);
            SetSkill(SkillName.Meditation, 195.0, 200.0);
            SetSkill(SkillName.MagicResist, 100.0, 120.0);
            SetSkill(SkillName.Tactics, 195.0, 220.0);
            SetSkill(SkillName.Wrestling, 195.0, 220.0);
            SetSkill(SkillName.Macing, 195.0, 220.0);
            SetSkill(SkillName.Anatomy, 195.0, 220.0);
            SetSkill(SkillName.Healing, 195.0, 220.0);
            SetSkill(SkillName.Focus, 195);

            AddItem(new VirtualMountItem(this) { Hue = 2406 });
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 3);
            AddItem(PowerScroll.CreateRandomNoCraft(5, 5));
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
        public override Faction FactionAllegiance { get { return Urukhai.Instance; } }
        public override bool AlwaysMurderer { get { return true; } }
        public override bool AutoDispel { get { return true; } }
        public override double AutoDispelChance { get { return 1.0; } }
        public override bool BardImmune { get { return !true; } }
        public override bool Unprovokable { get { return true; } }
        public override bool Uncalmable { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Deadly; } }
        public override bool ShowFameTitle { get { return false; } }
        public override bool ClickTitle { get { return true; } }

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
                : base(224)
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
        public Gojira(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int v = reader.ReadInt();
        }
    }
}
