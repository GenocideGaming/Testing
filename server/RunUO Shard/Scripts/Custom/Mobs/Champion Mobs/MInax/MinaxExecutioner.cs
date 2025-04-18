using System;
using Server.Items;

namespace Server.Mobiles 
{ 
    public class MinaxExecutioner : BaseCreature 
    { 
        [Constructable] 
        public MinaxExecutioner()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        { 
            SpeechHue = Utility.RandomDyedHue(); 
            Name = "an Executioner"; 
            Hue = Utility.RandomSkinHue(); 

            if (Female = Utility.RandomBool()) 
            { 
                Body = 0x191; 
                AddItem(new Skirt(Utility.RandomRedHue())); 
            }
            else 
            { 
                Body = 0x190; 
                AddItem(new ShortPants(Utility.RandomRedHue())); 
            }

            SetStr(126, 150);
            SetDex(61, 85);
            SetInt(81, 95);

            SetHits(132, 139);
            SetDamage(23, 26);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 35, 45);
            SetResistance(ResistanceType.Fire, 25, 30);
            SetResistance(ResistanceType.Cold, 25, 30);
            SetResistance(ResistanceType.Poison, 10, 20);
            SetResistance(ResistanceType.Energy, 10, 20);

            SetSkill(SkillName.Anatomy, 125.0);
            SetSkill(SkillName.Fencing, 46.0, 77.5);
            SetSkill(SkillName.Macing, 35.0, 57.5);
            SetSkill(SkillName.Poisoning, 60.0, 82.5);
            SetSkill(SkillName.MagicResist, 83.5, 92.5);
            SetSkill(SkillName.Swords, 125.0);
            SetSkill(SkillName.Tactics, 125.0);
            SetSkill(SkillName.Lumberjacking, 125.0);

            Fame = 550;
            Karma = -550;

            VirtualArmor = 40;

            AddItem(new ThighBoots(Utility.RandomRedHue())); 
            AddItem(new Surcoat(Utility.RandomRedHue()));    
            AddItem(new ExecutionersAxe());

            Utility.AssignRandomHair(this);
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average, 2);
            AddLoot(LootPack.Meager);
        }

        public MinaxExecutioner(Serial serial)
            : base(serial)
        { 
        }

        public override bool AlwaysAttackable
        {
            get
            {
                return true;
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
