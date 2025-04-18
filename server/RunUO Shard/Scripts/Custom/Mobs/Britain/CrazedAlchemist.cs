using System;
using Server.Items;
using Server.Misc;

namespace Server.Mobiles
{
    [CorpseName("an alchemist corpse")]
    public class CrazedAlchemist : BaseCreature
    {
        private DateTime m_NextBomb;
        private int m_Thrown;
        [Constructable]
        public CrazedAlchemist() : base(AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Body = 183;

            this.Name = "a crazed alchemist";

            var robe = new Robe(Utility.RandomNeutralHue());
            var sandals = new Sandals();
            this.AddItem(robe);
            this.AddItem(sandals);

            this.SetStr(147, 215);
            this.SetDex(91, 115);
            this.SetInt(61, 85);

            this.SetHits(95, 123);

            this.SetDamage(1, 8);


            this.SetSkill(SkillName.MagicResist, 70.1, 85.0);
            this.SetSkill(SkillName.Swords, 60.1, 85.0);
            this.SetSkill(SkillName.Tactics, 75.1, 90.0);
            this.SetSkill(SkillName.Wrestling, 60.1, 85.0);

            this.Fame = 1250;
            this.Karma = -1250;

            this.VirtualArmor = 30;

            this.PackItem(new SulfurousAsh(Utility.RandomMinMax(6, 10)));
            this.PackItem(new MandrakeRoot(Utility.RandomMinMax(6, 10)));
            this.PackItem(new BlackPearl(Utility.RandomMinMax(6, 10)));
            this.PackItem(new MortarPestle());
            this.PackItem(new LesserExplosionPotion());

        }

        public CrazedAlchemist(Serial serial)
            : base(serial)
        {
        }

        public override bool CanRummageCorpses
        {
            get
            {
                return true;
            }
        }

        public override bool AlwaysMurderer
        {
            get
            {
                return true;
            }
        }

        public override void GenerateLoot()
        {
            base.PackAddGoldTier(3);
            base.PackAddArtifactChanceTier(3);
            base.PackAddMapChanceTier(3);
        }

        public override void OnActionCombat()
        {
            Mobile combatant = this.Combatant as Mobile;

            if (combatant == null || combatant.Deleted || combatant.Map != this.Map || !this.InRange(combatant, 12) || !this.CanBeHarmful(combatant) || !this.InLOS(combatant))
                return;

            if (DateTime.UtcNow >= this.m_NextBomb)
            {
                this.ThrowBomb(combatant);

                this.m_Thrown++;

                if (0.75 >= Utility.RandomDouble() && (this.m_Thrown % 2) == 1) // 75% chance to quickly throw another bomb
                    this.m_NextBomb = DateTime.UtcNow + TimeSpan.FromSeconds(1.5);
                else
                    this.m_NextBomb = DateTime.UtcNow + TimeSpan.FromSeconds(3.0 + (10.0 * Utility.RandomDouble())); // 5-15 seconds
            }
        }

        public void ThrowBomb(Mobile m)
        {
            this.DoHarmful(m);

            this.MovingParticles(m, 0x1C19, 1, 0, false, true, 0, 0, 9502, 6014, 0x11D, EffectLayer.Waist, 0);

            new InternalTimer(m, this).Start();
        }

        public override int GetDeathSound()
        {
            return 0x423;
        }

        public override int GetHurtSound()
        {
            return 0x436;
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

        private class InternalTimer : Timer
        {
            private readonly Mobile m_Mobile;
            private readonly Mobile m_From;
            public InternalTimer(Mobile m, Mobile from)
                : base(TimeSpan.FromSeconds(1.0))
            {
                this.m_Mobile = m;
                this.m_From = from;
                this.Priority = TimerPriority.TwoFiftyMS;
            }

            protected override void OnTick()
            {
                this.m_Mobile.PlaySound(0x11D);
                AOS.Damage(this.m_Mobile, this.m_From, Utility.RandomMinMax(10, 20), 0, 100, 0, 0, 0);
            }
        }
    }
}
