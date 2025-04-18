using System;
using Server.Items;
using Server.Scripts.Engines.Factions.Instances.Factions;
using Server.Factions;
using Server.Misc;

namespace Server.Mobiles
{
    public class UrukGrunt : BaseCreature
    {
        [Constructable]
        public UrukGrunt()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an orc grunt";
            Body = 17;
            BaseSoundID = 0x45A;

            SetStr(91, 115);
            SetDex(61, 85);
            SetInt(81, 95);

            SetDamage(10, 14);

            SetSkill(SkillName.Healing, 80.0, 90.0);
            SetSkill(SkillName.Anatomy, 80.0, 90.0);
            SetSkill(SkillName.MagicResist, 80.0, 90.0);
            SetSkill(SkillName.Macing, 80.0, 90.0);
            SetSkill(SkillName.Tactics, 80.0, 90.0);
            SetSkill(SkillName.Wrestling, 80.0, 90.0);

            VirtualArmor = 8;

            Fame = 550;
            Karma = -550;

        }

        public UrukGrunt(Serial serial)
            : base(serial)
        {
        }
        public override Faction FactionAllegiance { get { return Urukhai.Instance; } }
        public override InhumanSpeech SpeechType{get{return InhumanSpeech.Orc;}}

        public override bool ClickTitle{get{return true;}}

        public override bool AlwaysAttackable{get{return true;}}

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
