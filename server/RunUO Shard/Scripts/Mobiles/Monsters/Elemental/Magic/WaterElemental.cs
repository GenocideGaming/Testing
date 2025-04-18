using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a water elemental corpse")]
    public class WaterElemental : BaseCreature
    {
        public override double DispelDifficulty { get { return 117.5; } }
        public override double DispelFocus { get { return 45.0; } }

        [Constructable]
        public WaterElemental()
            : base(AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a water elemental";
            Body = 16;
            BaseSoundID = 278;

            SpellCastAnimation = 3;
            SpellCastFrameCount = 4;

            SetStr(195, 200);
            SetDex(60, 65);
            SetInt(500, 600);

            SetHits(175, 200);
            SetMana(500, 650);

            SetDamage(5, 10);

            VirtualArmor = 5;

            SetSkill(SkillName.Tactics, 100.0, 100.0);

            SetSkill(SkillName.EvalInt, 65.0, 70.0);
            SetSkill(SkillName.Magery, 65.0, 70.0);              
            SetSkill(SkillName.Meditation, 100.0, 100.0);

            SetSkill(SkillName.MagicResist, 195.0, 200.0);          

            SetSkill(SkillName.Wrestling, 60.0, 65.0);

            Fame = 4500;
            Karma = -4500;

            ControlSlots = 2;

            CanSwim = true;
        }        

        public override void GenerateLoot()
        {
            base.PackAddGoldTier(5);
            base.PackAddArtifactChanceTier(5);
            base.PackAddMapChanceTier(5);

            base.PackAddScrollTier(3);
            base.PackAddReagentTier(3);
        }

        public override bool BleedImmune { get { return true; } }        

        public WaterElemental(Serial serial)
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