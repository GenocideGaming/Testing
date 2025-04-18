using System;
using System.Collections.Generic;
using Server.Network;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;
using Server.Scripts;

namespace Server.Spells.Seventh
{
    public class MeteorSwarmSpell : MagerySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                "Meteor Swarm", "Flam Kal Des Ylem",
                233,
                9042,
                false,
                Reagent.Bloodmoss,
                Reagent.MandrakeRoot,
                Reagent.SulfurousAsh,
                Reagent.SpidersSilk
            );

        public override int BaseDamage { get { return 37; } }
        public override int DamageVariation { get { return 5; } }
        public override double InterruptChance { get { return 100; } }
        public override double ResistDifficulty { get { return 0; } }

        public override SpellCircle Circle { get { return SpellCircle.Seventh; } }

        public MeteorSwarmSpell(Mobile caster, Item scroll)
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
                    IPoint3D targetLocation = casterCreature.SpellTarget.Location as IPoint3D;

                    if (targetLocation != null)
                    {
                        this.Target(targetLocation);
                    }                   
                }
            }

            else
            {
                Caster.Target = new InternalTarget(this);
            }
        }

        public override bool DelayedDamage { get { return true; } }

        public void Target(IPoint3D p)
        {
            if (!Caster.CanSee(p))
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }

            else if (SpellHelper.CheckTown(p, Caster) && CheckSequence())
            {
                BaseCreature creatureCaster = Caster as BaseCreature;                

                //Creature
                if (creatureCaster != null && (!creatureCaster.Deleted && creatureCaster.NetState == null)) // not a pseudoseer
                {
                    SpellHelper.Turn(creatureCaster, p);                    

                    if (p is Item)
                        p = ((Item)p).GetWorldLocation();

                    List<Mobile> targets = new List<Mobile>();

                    Map map = creatureCaster.Map;

                    if (map != null)
                    {
                        //Increased Range for High Level Creature Casters
                        double magerySkill = creatureCaster.Skills[SkillName.Magery].Value;

                        int radius = 3 + (int)(Math.Floor((magerySkill - 75) / 25));

                        if (radius < 3)
                            radius = 3;

                        IPooledEnumerable eable = map.GetMobilesInRange(new Point3D(p), radius);
                        
                        foreach (Mobile m in eable)
                        {
                            if (creatureCaster.CanBeHarmful(m, false))
                            {
                                Boolean attackable = false;

                                //Current Target
                                if (m == creatureCaster.Combatant)
                                    attackable = true;

                                //Player 
                                if (m.Player)
                                {
                                    attackable = true;
                                }

                                //Creature
                                BaseCreature creatureTarget = m as BaseCreature;

                                if (creatureTarget != null)
                                {
                                    //Attack Creature Only if Enemy or If Controlled By Player
                                    if (creatureCaster.GetFactionAllegiance(creatureTarget) == BaseCreature.Allegiance.Enemy || creatureCaster.GetEthicAllegiance(creatureTarget) == BaseCreature.Allegiance.Enemy)
                                    {
                                        attackable = true;
                                    }

                                    //Controlled
                                    if (creatureTarget.Controlled && creatureTarget.ControlMaster != null)
                                    {
                                        //Controlled By Player
                                        if (creatureTarget.ControlMaster.Player)
                                        {
                                            attackable = true;
                                        }
                                    }
                                }

                                //Same Team
                                if (AITeamList.CheckSameTeam(creatureCaster, m)) // AITeamList.CheckTeamOld(creatureCaster, m)
                                {
                                    attackable = false;
                                }

                                //Is Aggressor
                                for (int i = 0; i < creatureCaster.Aggressors.Count; ++i)
                                {
                                    AggressorInfo info = creatureCaster.Aggressors[i];
                                    Mobile attacker = info.Attacker;
                                    Mobile defender = info.Defender;

                                    if (attacker == m || defender == m)
                                    {
                                        //Will Override Same Team if Aggressor
                                        attackable = true;
                                        break;
                                    }
                                }

                                //Add Mobile to Effect Radius
                                if (attackable)
                                    targets.Add(m);
                            }
                        }                        

                        eable.Free();
                    }

                    if (targets.Count > 0)
                    {
                        Effects.PlaySound(p, creatureCaster.Map, 0x160);

                        for (int i = 0; i < targets.Count; ++i)
                        {
                            Mobile m = targets[i];

                            //Damage Equal to Targets
                            double damage = GetDamage(m);
                            bool interrupt = CheckInterrupt(m);

                            creatureCaster.DoHarmful(m);

                            SpellHelper.Damage(this, m, damage, interrupt);

                            creatureCaster.MovingParticles(m, 0x36D4, 7, 0, false, true, 9501, 1, 0, 0x100);
                        }
                    }
                }

                //Player
                else
                {
                    SpellHelper.Turn(Caster, p);

                    if (p is Item)
                        p = ((Item)p).GetWorldLocation();

                    List<Mobile> targets = new List<Mobile>();

                    Map map = Caster.Map;

                    if (map != null)
                    {
                        IPooledEnumerable eable = map.GetMobilesInRange(new Point3D(p), 2);

                        foreach (Mobile m in eable)
                        {
                            if (Caster.CanBeHarmful(m, false))
                            {
                                targets.Add(m);
                            }
                        }

                        eable.Free();
                    }

                    if (targets.Count > 0)
                    {
                        Effects.PlaySound(p, Caster.Map, 0x160);

                        // old handler
                        /*for (int i = 0; i < targets.Count; ++i)
                        {
                            Mobile m = targets[i];

                            double damage = GetDamage(m) / (double)targets.Count;
                            bool interrupt = CheckInterrupt(m);

                            Caster.DoHarmful(m);

                            SpellHelper.Damage(this, m, damage, interrupt);

                            Caster.MovingParticles(m, 0x36D4, 7, 0, false, true, 9501, 1, 0, 0x100);
                        }*/
                        for (int i = 0; i < targets.Count; ++i)
                        {
                            Mobile m = targets[i];

                            double damage = GetDamage(m);
                            damage -= (targets.Count - 1) * FeatureList.SpellChanges.MeteorSwarmDamageReductionPerTarget;
                            if (damage < FeatureList.SpellChanges.MeteorSwarmDamageReductionPerTarget)
                                damage = FeatureList.SpellChanges.MeteorSwarmDamageReductionPerTarget;
                            bool interrupt = CheckInterrupt(m);

                            Caster.DoHarmful(m);
                            if (CheckSpecialReflect(Caster, m) == false)
                            {
                                SpellHelper.Damage(this, m, damage, interrupt);
                                Caster.MovingParticles(m, 0x36D4, 7, 0, false, true, 9501, 1, 0, 0x100);
                            }
                        }
                    }
                }
            }

            FinishSequence();
        }

        public bool CheckSpecialReflect(Mobile caster, Mobile target)
        {
            // skill gain not allowed from MS
            
            //Check for Resist SkillGain
            //int maxSkill = (1 + (int)circle) * 10;
            //maxSkill += (1 + ((int)circle / 6)) * 25;

            
            //if (target.Skills[SkillName.MagicResist].Value < maxSkill)
            //    target.CheckSkill(SkillName.MagicResist, 0.0, target.Skills[SkillName.MagicResist].Cap);

            if (target.MagicDamageAbsorb > 0)
            {
                target.MagicDamageAbsorb -= 1; // only 1 for meteorswarm

                if (target.MagicDamageAbsorb < 0)
                    target.MagicDamageAbsorb = 0;

                target.FixedEffect(0x37B9, 10, 5);
                return true;
            }
            return false;
        }

        private class InternalTarget : Target
        {
            private MeteorSwarmSpell m_Owner;

            public InternalTarget(MeteorSwarmSpell owner)
                : base(Core.ML ? 10 : 12, true, TargetFlags.None)
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