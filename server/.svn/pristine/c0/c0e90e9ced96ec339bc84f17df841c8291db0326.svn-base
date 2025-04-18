using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a dust elemental corpse")]
    public class DustElemental : BaseCreature
    {
        [Constructable]
        public DustElemental()
            : base(AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a dust elemental";
            Body = 196;
            BaseSoundID = 0x4FE;

            SetStr(150, 155);
            SetDex(60, 65);
            SetInt(25, 30);

            SetHits(175, 200);

            SetDamage(8, 12);

            VirtualArmor = 30;
            
            SetSkill( SkillName.Tactics, 100.0, 100.0);

            SetSkill( SkillName.MagicResist, 75.0, 100.0);

            SetSkill( SkillName.Wrestling, 80.0, 85.0);

            SetSkill( SkillName.Healing, 20.0, 25.0);

            Fame = 3500;
            Karma = -3500;
        }        

        public override void GenerateLoot()
        {
            base.PackAddGoldTier(5);
            base.PackAddArtifactChanceTier(5);
            base.PackAddMapChanceTier(5);

            base.PackAddGemTier(2);

            PackItem(new IronOre());
        }        

        public DustElemental(Serial serial)
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