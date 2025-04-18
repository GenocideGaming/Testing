using System;
using Server.Items;
using Server.Misc;
using Server.Scripts.Engines.Factions.Instances.Factions;
using Server.Factions;
using System.Collections;
using Server.Targeting;
using Server.Mobiles;

namespace Server.Mobiles
{
    public class UrukScout : BaseCreature
    {
        [Constructable]
        public UrukScout()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an orc scout";
            SpeechHue = Utility.RandomDyedHue();
            Body = 17;
            Hue = 1436;
            BaseSoundID = 0x45A;

            SetStr(151, 175);
            SetDex(61, 85);
            SetInt(151, 175);

            VirtualArmor = 50;

            SetSkill(SkillName.Archery, 110.0, 120.0);
            SetSkill(SkillName.Wrestling, 110.0, 120.0);
            SetSkill(SkillName.Tactics, 110.0, 120.0);
            SetSkill(SkillName.MagicResist, 110.0, 120.0);
            SetSkill(SkillName.Healing, 110.0, 120.0);
            SetSkill(SkillName.Anatomy, 110.0, 120.0);

            SetSkill(SkillName.Magery, 110.0, 120.0);
            SetSkill(SkillName.EvalInt, 110.0, 120.0);
            SetSkill(SkillName.Meditation, 110.0, 120.0);

            Fame = 1750;
            Karma = -1750;
        }

        public UrukScout(Serial serial)
            : base(serial)
        {
        }
        public override Faction FactionAllegiance { get { return Urukhai.Instance; } }
        //public override double HealChance { get { return 1.0; } }
        public override InhumanSpeech SpeechType { get { return InhumanSpeech.Orc; } }
        public override bool ClickTitle { get { return true;} }
        public override bool AlwaysAttackable{ get { return true; } }

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
