using System;
using Server;
using Server.Items;
using Server.Factions;

namespace Server.Mobiles
{
    [CorpseName("a druid's corpse")]
    public class BearDruid : BaseCreature
    {
        

        [Constructable]
        public BearDruid()
            : base(AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an old druid"; // pretty much a demon
            Body = 0x190;
            Hue = 2156;

            SetStr(500, 550);
            SetDex(110, 115);
            SetInt(1000, 1200);

            SetHits(800, 900);
            SetMana(800, 1000);

            SetDamage(20, 30);

            VirtualArmor = 10;

            SetSkill(SkillName.Tactics, 100.0, 100.0);

            SetSkill(SkillName.EvalInt, 80.0, 85.0);
            SetSkill(SkillName.Meditation, 100.0, 100.0);
            SetSkill(SkillName.Magery, 80.0, 85.0);

            SetSkill(SkillName.MagicResist, 85.0, 90.0);

            SetSkill(SkillName.Wrestling, 85.0, 90.0);

            Fame = 500;
            Karma = -500;

            Item Sandals = new Sandals();
            Sandals.Movable = false;
            Sandals.Hue = 2156;
            AddItem(Sandals);

            Item BodySash = new Surcoat();
            BodySash.Movable = false;
            BodySash.Hue = 2156;
            AddItem(BodySash);

            Item LeatherChest = new LeatherChest();
            LeatherChest.Movable = false;
            LeatherChest.Hue = 2156;
            AddItem(LeatherChest);

            Item LeatherArms = new LeatherArms();
            LeatherArms.Movable = false;
            LeatherArms.Hue = 2156;
            AddItem(LeatherArms);

            Item LeatherLegs = new LeatherLegs();
            LeatherLegs.Movable = false;
            LeatherLegs.Hue = 2156;
            AddItem(LeatherLegs);

            Item LeatherGloves = new LeatherGloves();
            LeatherGloves.Movable = false;
            LeatherGloves.Hue = 2156;
            AddItem(LeatherGloves);

            Item LeatherGorget = new LeatherGorget();
            LeatherGorget.Movable = false;
            AddItem(LeatherGorget);

            Item Hood = new BearMask ();
            Hood.Movable = false;
            Hood.Hue = 0;
            AddItem(Hood);      
        }


        public override void GenerateLoot()
        {
            
        }

        public override bool CanRummageCorpses { get { return true; } }

        public override int Meat { get { return 1; } }

        public BearDruid(Serial serial)
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
            BearformDruid rm = new BearformDruid();
            rm.Team = this.Team;
            rm.Combatant = this.Combatant;
            rm.NoKillAwards = true;
            Effects.PlaySound(this, Map, 0x208);
            Effects.SendLocationEffect(Location, Map, 0x3709, 30, 10, 542, 0);
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
