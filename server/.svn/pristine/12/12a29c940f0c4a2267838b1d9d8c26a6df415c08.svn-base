using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a fire elemental corpse")]
    public class FireElemental : BaseCreature
    {
        public override double DispelDifficulty { get { return 117.5; } }
        public override double DispelFocus { get { return 45.0; } }

        [Constructable]
        public FireElemental()
            : base(AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a fire elemental";
            Body = 15;
            BaseSoundID = 838;

            SpellCastAnimation = 6;
            SpellCastFrameCount = 4;

            SetStr(150, 155);
            SetDex(80, 85);
            SetInt(400, 500);

            SetHits(150, 175);
            SetMana(400, 500);

            SetDamage(12, 18);

            VirtualArmor = 5;

            SetSkill(SkillName.Tactics, 100.0, 100.0);

            SetSkill(SkillName.EvalInt, 70, 75.0);
            SetSkill(SkillName.Magery, 70.0, 75.0);
            SetSkill(SkillName.Meditation, 100.0, 100.0);

            SetSkill(SkillName.MagicResist, 95.0, 100.0);

            SetSkill(SkillName.Wrestling, 70.0, 75.0);

            Fame = 4500;
            Karma = -4500;

            ControlSlots = 2;
        }        

        public override void GenerateLoot()
        {
            base.PackAddGoldTier(5);
            base.PackAddArtifactChanceTier(5);
            base.PackAddMapChanceTier(5);

            base.PackAddScrollTier(3);

            PackItem(new SulfurousAsh(15));

            AddItem(new LightSource());            
        }

        public override bool BleedImmune { get { return true; } }       

        public FireElemental(Serial serial)
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

            if (BaseSoundID == 274)
                BaseSoundID = 838;
        }
    }
}
