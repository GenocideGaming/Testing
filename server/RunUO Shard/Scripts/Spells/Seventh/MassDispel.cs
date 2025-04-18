using System;
using System.Collections.Generic;
using Server.Misc;
using Server.Network;
using Server.Items;
using Server.Targeting;
using Server.Mobiles;
using Server.Spells.Fifth;

namespace Server.Spells.Seventh
{
    public class MassDispelSpell : MagerySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                "Mass Dispel", "Vas An Ort",
                263,
                9002,
                Reagent.Garlic,
                Reagent.MandrakeRoot,
                Reagent.BlackPearl,
                Reagent.SulfurousAsh
            );

        public override SpellCircle Circle { get { return SpellCircle.Seventh; } }

        public MassDispelSpell(Mobile caster, Item scroll)
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

        public void Target(IPoint3D p)
        {
            if (!Caster.CanSee(p))
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }

            else if (CheckSequence())
            {
                SpellHelper.Turn(Caster, p);

                SpellHelper.GetSurfaceTop(ref p);

                List<Mobile> targets = new List<Mobile>();
                List<PlayerMobile> playerTargets = new List<PlayerMobile>();

                Map map = Caster.Map;

                if (map != null)
                {
                    IPooledEnumerable eable = map.GetMobilesInRange(new Point3D(p), 8);

                    foreach (Mobile m in eable)
                    {
                        if (m is PlayerMobile && (m.IsBodyMod || m.NameMod != null || m.MagicDamageAbsorb > 0))
                        {
                            playerTargets.Add(m as PlayerMobile);
                        }

                        if (m is BaseCreature && !(m is PlayerMobile) && Caster.CanBeHarmful(m, false))
                        {
                            targets.Add(m);
                        }
                    }

                    eable.Free();
                }

                for (int i = 0; i < targets.Count; ++i)
                {
                    Mobile m = targets[i];

                    BaseCreature bc = m as BaseCreature;

                    //If Creature is Null
                    if (bc == null)
                        continue;

                    Effects.SendLocationParticles(EffectItem.Create(bc.Location, bc.Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5015);
                    Effects.PlaySound(bc.Location, bc.Map, 0x22C);

                    //Remove Magic Reflect
                    if (bc.MagicDamageAbsorb > 0)
                    {
                        bc.MagicDamageAbsorb = 0;

                        BuffInfo.RemoveBuff(bc, BuffIcon.MagicReflection);

                        Caster.DoHarmful(bc);
                    }

                    //If Not Dispellable
                    if (!bc.IsDispellable)
                    {
                        continue;
                    }

                    double creatureValue = GetMobileDifficulty.GetDifficultyValue(m);
                    double dispelSkill = Caster.Skills[SkillName.Magery].Value;
                    double resistBonus = (m.Skills[SkillName.MagicResist].Value / 10) / 100;

                    double chance = .50 + ((dispelSkill - creatureValue) / 100) - resistBonus;

                    //Successful Dispel
                    if (chance >= Utility.RandomDouble())
                    {
                        m.Delete();
                    }

                    //Failed Dispel
                    else
                    {
                        double damage = dispelSkill / 2;

                        SpellHelper.Damage(this, m, damage, false);
                    }
                }

                foreach (PlayerMobile pm in playerTargets)
                {
                    if ((pm.IsBodyMod || pm.NameMod != null))
                    {
                        pm.BodyMod = 0;
                        pm.HueMod = -1;
                        pm.NameMod = null;
                        pm.SetHairMods(-1, -1);

                        BaseArmor.ValidateMobile(pm);
                        BaseClothing.ValidateMobile(pm);

                        pm.EndAction(typeof(PolymorphSpell));
                        pm.EndAction(typeof(IncognitoSpell));

                        Caster.DoHarmful(pm);

                        Effects.SendLocationParticles(EffectItem.Create(pm.Location, pm.Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5015);
                        Effects.PlaySound(pm.Location, pm.Map, 0x22C);
                    }

                    if (pm.MagicDamageAbsorb > 0)
                    {
                        pm.MagicDamageAbsorb = 0;

                        BuffInfo.RemoveBuff(pm, BuffIcon.MagicReflection);

                        Caster.DoHarmful(pm);

                        Effects.SendLocationParticles(EffectItem.Create(pm.Location, pm.Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5015);
                        Effects.PlaySound(pm.Location, pm.Map, 0x22C);
                    }
                }
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private MassDispelSpell m_Owner;

            public InternalTarget(MassDispelSpell owner)
                : base(12, true, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                IPoint3D p = o as IPoint3D;

                if (p != null)
                    m_Owner.Target(p);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}