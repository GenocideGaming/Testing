using System;
using Server.Items;
using Server.Misc;
using Server.Scripts.Engines.Factions.Instances.Factions;
using Server.Factions;
using System.Collections;
using Server.Targeting;

namespace Server.Mobiles
{
    public class UrukRaider : BaseCreature
    {
        [Constructable]
        public UrukRaider()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            SpeechHue = Utility.RandomDyedHue();

            Body = 7;
            Name = "an orc raider";

            this.SetStr(116, 125);
            this.SetDex(61, 85);
            this.SetInt(81, 95);

            SetDamage(20, 24);

            this.VirtualArmor = 16;

            this.SetSkill(SkillName.Fencing, 90.0, 100.0);
            this.SetSkill(SkillName.Wrestling, 90.0, 100.0);
            this.SetSkill(SkillName.Tactics, 90.0, 100.0);
            this.SetSkill(SkillName.MagicResist, 90.0, 100.0);
            this.SetSkill(SkillName.Healing, 90.0, 100.0);
            this.SetSkill(SkillName.Anatomy, 90.0, 100.0);

            Fame = 750;
            Karma = -750;

        }

        public UrukRaider(Serial serial)
            : base(serial)
        {
        }

        public override Faction FactionAllegiance { get { return Urukhai.Instance; } }
        //public override double HealChance { get { return 0.5; } }
        public override bool AlwaysAttackable { get { return true; } }
        public override InhumanSpeech SpeechType
        {
            get
            {
                return InhumanSpeech.Orc;
            }
        }
        public override bool ClickTitle
        {
            get
            {
                return false;
            }
        }

        public override bool ShowFameTitle
        {
            get
            {
                return false;
            }
        }
        public override OppositionGroup OppositionGroup
        {
            get
            {
                return OppositionGroup.SavagesAndOrcs;
            }
        }

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
