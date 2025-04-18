using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a daemon corpse")]
    public class SummonedDaemon : BaseCreature
    {
        public override double DispelDifficulty { get { return 125.0; } }
        public override double DispelFocus { get { return 45.0; } }

        [Constructable]
        public SummonedDaemon()
            : base(AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            //Name = NameList.RandomName( "daemon" );
            Name = "a daemon";
            Body = Core.AOS ? 10 : 9;
            BaseSoundID = 357;

            SetStr(476, 505);
            SetDex(76, 95);
            SetInt(150, 200);

            SetHits(225, 250);

            SetDamage(14, 18);

            VirtualArmor = 10;

            SetSkill(SkillName.Tactics, 100.0, 100.0);

            SetSkill(SkillName.EvalInt, 60.0, 65.0);
            SetSkill(SkillName.Magery, 60.0, 65.0);
            SetSkill(SkillName.MagicResist, 60.0, 65.0);
            SetSkill(SkillName.Wrestling, 70.0, 75.0);

            Fame = 5000;
            Karma = -5000;

            ControlSlots = 2;
        }

        public SummonedDaemon(Serial serial)
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
            int version = reader.ReadInt();
        }
    }
}