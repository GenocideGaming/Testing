using System;
using Server.Items;
using Server.Scripts.Engines.Factions.Instances.Factions;
using Server.Factions;
using Server.Misc;

namespace Server.Mobiles
{
    public class ForsakenDK : BaseCreature
    {
        [Constructable]
        public ForsakenDK() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a dread knight";
            Body = 0x190;
            Hue = 2406;

            SetStr(116, 125);
            SetDex(61, 85);
            SetInt(81, 95);

            SetDamage(20, 24);

            VirtualArmor = 16;

            SetSkill(SkillName.Swords, 90.0, 100.0);
            SetSkill(SkillName.Wrestling, 90.0, 100.0);
            SetSkill(SkillName.Tactics, 90.0, 100.0);
            SetSkill(SkillName.MagicResist, 90.0, 100.0);
            SetSkill(SkillName.Healing, 90.0, 100.0);
            SetSkill(SkillName.Anatomy, 90.0, 100.0);

            AddItem(Rehued(new Kilt(), 2149));
            AddItem(Rehued(new BodySash(), 2149));
            AddItem(Rehued(new BoneHelm(), 1109));
            AddItem(Rehued(new DaemonLegs(), 1109));
            AddItem(Rehued(new DaemonChest(), 1109));
            AddItem(Rehued(new DaemonArms(), 1109));
            AddItem(Rehued(new DaemonGloves(), 1109));
            AddItem(Rehued(new Sandals(), 2149));
            AddItem(Immovable(Rehued(new ExecutionersAxe(), 2149)));

            Fame = 750;
            Karma = -750;

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
        public ForsakenDK(Serial serial)
            : base(serial)
        {
        }
        public override Faction FactionAllegiance { get { return Forsaken.Instance; } }
        //public override double HealChance { get { return 0.5; } }
        public override bool ClickTitle{get{return true;}}
        public override bool AlwaysAttackable{get{return true;}}
        public override bool ShowFameTitle{get{return false;}}

        public override OppositionGroup OppositionGroup
        {get{return OppositionGroup.FeyAndUndead; }}

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            c.Delete();
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
