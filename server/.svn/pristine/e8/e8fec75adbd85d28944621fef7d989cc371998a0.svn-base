using System;
using Server.Targeting;
using Server.Network;
using Server.Scripts;
using Server.Items;
using Server.Factions;
using Server.Mobiles;
using Server.Scripts.Custom.Citizenship;

namespace Server.Spells.Fifth
{
    public class MindBlastSpell : MagerySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                "Mind Blast", "Por Corp Wis",
                218,
                Core.AOS ? 9002 : 9032,
                Reagent.BlackPearl,
                Reagent.MandrakeRoot,
                Reagent.Nightshade,
                Reagent.SulfurousAsh
            );

        public override int BaseDamage { get { return 15; } }
        public override int DamageVariation { get { return 0; } }
        public override double InterruptChance { get { return 100; } }
        public override double ResistDifficulty { get { return 0; } }

        public override SpellCircle Circle { get { return SpellCircle.Fifth; } }

        public MindBlastSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            BaseCreature casterCreature = Caster as BaseCreature;

            if (casterCreature != null && casterCreature.NetState == null)
            {
                if (casterCreature.SpellTarget != null)
                {
                    this.Target(casterCreature.SpellTarget);
                }
            }

            else
            {
                Caster.Target = new InternalTarget(this);
            }
        }        

        public override bool DelayedDamage { get { return true; } }

        public void Target(Mobile m)
        {
            if (!Caster.CanSee(m) || m.Hidden)
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }

            else if (CheckHSequence(m))
            {
                Mobile from = Caster, target = m;

                SpellHelper.Turn(from, target);

                SpellHelper.CheckReflect((int)this.Circle, ref from, ref target);

                double damage = BaseDamage;
                bool interrupt = CheckInterrupt(m);    

                from.FixedParticles(0x374A, 10, 15, 2038, EffectLayer.Head);

                target.FixedParticles(0x374A, 10, 15, 5038, EffectLayer.Head);
                target.PlaySound(0x213);

                SpellHelper.Damage(this, target, damage, interrupt);

                SpellHelper.CalculateDiminishingReturns(this, Caster, m);
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private MindBlastSpell m_Owner;

            public InternalTarget(MindBlastSpell owner)
                : base(Core.ML ? 10 : 12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                // This whole thing is ugly as balls and should probably be refactored at some point
                if (o is IDamageableItemMilitiaRestricted)
                    o = ((IDamageableItemMilitiaRestricted)o).Link;
                else if (o is DamageableItem)
                {
                    o = ((DamageableItem)o).Link;
                    m_Owner.Target((Mobile)o);
                    return;
                }
                else if (o is Mobile)
                {
                    m_Owner.Target((Mobile)o);
                    return;
                }

                if (o is DamageableItemMilitiaRestricted)
                {
                    Mobile newTarget = m_Owner.GetMilitiaRestrictedMobileIfAttackable(from, (DamageableItemMilitiaRestricted)o);
                    if (newTarget != null) m_Owner.Target(newTarget);
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}