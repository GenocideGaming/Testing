using System;
using Server.Items;
using Server.Scripts.Engines.Factions.Instances.Factions;
using Server.Factions;
using Server.Misc;

namespace Server.Mobiles
{
    public class ForsakenMinion : BaseCreature
    {
        [Constructable]
        public ForsakenMinion() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a minion";
            Body = 0x190;

            SetStr(91, 115);
            SetDex(61, 85);
            SetInt(81, 95);

            SetDamage(10, 14);

            SetSkill(SkillName.Healing, 80.0, 90.0);
            SetSkill(SkillName.Anatomy, 80.0, 90.0);
            SetSkill(SkillName.MagicResist, 80.0, 90.0);
            SetSkill(SkillName.Swords, 80.0, 90.0);
            SetSkill(SkillName.Tactics, 80.0, 90.0);
            SetSkill(SkillName.Wrestling, 80.0, 90.0);

            VirtualArmor = 8;
            AddItem(Immovable(new BoneHarvester()));
            AddItem(Rehued(new DaemonLegs(), 2149));
            AddItem(Rehued(new DaemonChest(), 2149));
            AddItem(Rehued(new DaemonArms(), 2149));
            AddItem(Rehued(new DaemonGloves(), 2149));
            AddItem(Rehued(new DaemonHelm(), 2149));

            Fame = 550;
            Karma = -550;

        }

        public ForsakenMinion(Serial serial)
            : base(serial)
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
        public override Faction FactionAllegiance { get { return Forsaken.Instance; } }

        public override bool ClickTitle{get{return true;}}

        public override bool AlwaysAttackable{get{return true;}}

        public override bool ShowFameTitle{get{return false;}}

        public override OppositionGroup OppositionGroup
        {get{return OppositionGroup.FeyAndUndead;}}

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
