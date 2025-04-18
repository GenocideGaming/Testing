using System;
using Server.Items;
using Server.Misc;

namespace Server.Mobiles
{
    [CorpseName("an orcish corpse")]
    public class OrcChopper : BaseCreature
    {
        [Constructable]
        public OrcChopper()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an orc chopper";
            Body = 7;
            BaseSoundID = 0x45A;
            Hue = 0x96D;

            SetStr(147, 150);
            SetDex(91, 115);
            SetInt(61, 85);

            SetHits(95, 133);

            SetDamage(4, 14);


            SetSkill(SkillName.MagicResist, 60.1, 85.0);
            SetSkill(SkillName.Tactics, 75.1, 90.0);
            SetSkill(SkillName.Wrestling, 60.1, 85.0);

            Fame = 250;
            Karma = -250;

            PackItem(new Log(Utility.RandomMinMax(1, 10)));
            PackItem(new Board(Utility.RandomMinMax(10, 20)));
            PackItem(new Hatchet());

            // TODO: Skull?
            switch (Utility.Random(7))
            {
                case 0:
                    PackItem(new Arrow());
                    break;
                case 1:
                    PackItem(new Lockpick());
                    break;
                case 2:
                    PackItem(new Shaft());
                    break;
                case 3:
                    PackItem(new Ribs());
                    break;
                case 4:
                    PackItem(new Bandage());
                    break;
                case 5:
                    PackItem(new BeverageBottle(BeverageType.Wine));
                    break;
                case 6:
                    PackItem(new Jug(BeverageType.Cider));
                    break;
            }

        }

        public OrcChopper(Serial serial)
            : base(serial)
        {
        }

        public override InhumanSpeech SpeechType
        {
            get
            {
                return InhumanSpeech.Orc;
            }
        }
        public override bool CanRummageCorpses
        {
            get
            {
                return true;
            }
        }
        public override int Meat
        {
            get
            {
                return 1;
            }
        }

        public override OppositionGroup OppositionGroup
        {
            get { return OppositionGroup.SavagesAndOrcs; }
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            c.DropItem(new DoubleAxe());
        }

        public override void GenerateLoot()
        {
            base.PackAddGoldTier(2);
            base.PackAddArtifactChanceTier(2);
            base.PackAddMapChanceTier(2);

            if (0.50 > Utility.RandomDouble())
                PackItem(new OrcHelm());

            if (0.1 > Utility.RandomDouble())
                PackItem(new OrcishHeart());
        }
        public override bool IsEnemy(Mobile m)
        {
            if (m.Player && m.FindItemOnLayer(Layer.Helm) is OrcishKinMask)
                return false;

            return base.IsEnemy(m);
        }

        public override void AggressiveAction(Mobile aggressor, bool criminal)
        {
            base.AggressiveAction(aggressor, criminal);

            if (aggressor.HueMod == 1451)
            {
                AOS.Damage(aggressor, 50, 0, 100, 0, 0, 0);
                aggressor.BodyMod = 0;
                aggressor.HueMod = -1;
                aggressor.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);
                aggressor.PlaySound(0x307);
                aggressor.SendMessage("Your skin is scorched as the orcish paint burns away!"); // Your skin is scorched as the tribal paint burns away!
            }
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
