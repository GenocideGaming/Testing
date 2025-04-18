using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a druids's corpse")]
    public class DeerDruid : BaseCreature
    {
        [Constructable]
        public DeerDruid()
            : base(AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an old druid"; // pretty much a lich lord
            Body = 0x191;
            Hue = 2156;

            SetStr(200, 205);
            SetDex(45, 50);
            SetInt(800, 1000);

            SetHits(500, 600);
            SetMana(1400, 1600);

            SetDamage(10, 20);

            VirtualArmor = 5;

            SetSkill(SkillName.Tactics, 100.0, 100.0);

            SetSkill(SkillName.EvalInt, 95.0, 100.0);
            SetSkill(SkillName.Magery, 95.0, 100.0);
            SetSkill(SkillName.Meditation, 100.0, 100.0);

            SetSkill(SkillName.MagicResist, 95.0, 100.0);

            SetSkill(SkillName.Wrestling, 65.0, 70.0);

            Fame = 500;
            Karma = -500;


            Item Sandals = new Sandals();
            Sandals.Movable = false;
            Sandals.Hue = 2156;
            AddItem(Sandals);

            Item BodySash = new DeerMask ();
            BodySash.Movable = false;
            BodySash.Hue = 0;
            AddItem(BodySash);

            Item LeatherChest = new BodySash ();
            LeatherChest.Movable = false;
            LeatherChest.Hue = 2156;
            AddItem(LeatherChest);

            Item LeatherArms = new FancyShirt();
            LeatherArms.Movable = false;
            LeatherArms.Hue = 2156;
            AddItem(LeatherArms);

            Item LeatherLegs = new Skirt();
            LeatherLegs.Movable = false;
            LeatherLegs.Hue = 2156;
            AddItem(LeatherLegs);

            Item LeatherGloves = new LeatherGloves();
            LeatherGloves.Movable = false;
            LeatherGloves.Hue = 2155;
            AddItem(LeatherGloves);

            Item LeatherGorget = new HalfApron();
            LeatherGorget.Movable = false;
            LeatherGorget.Hue = 2156;
            AddItem(LeatherGorget);
        }

        public override void GenerateLoot()
        {
            
        }

        public override Poison PoisonImmune { get { return Poison.Lethal; } }


        public override OppositionGroup OppositionGroup
        {
            get { return OppositionGroup.FeyAndUndead; }
        }

        public override bool CanRummageCorpses { get { return true; } }
        public override bool BleedImmune { get { return true; } }

        public DeerDruid(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override bool OnBeforeDeath()
        {
            DeerformDruid rm = new DeerformDruid();
            rm.Team = this.Team;
            rm.Combatant = this.Combatant;
            rm.NoKillAwards = true;
            Effects.PlaySound(this, Map, 0x208);
            Effects.SendLocationEffect(Location, Map, 0x3709, 30, 10, 557, 0);
            rm.MoveToWorld(Location, Map);

            Delete();

            return false;
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}