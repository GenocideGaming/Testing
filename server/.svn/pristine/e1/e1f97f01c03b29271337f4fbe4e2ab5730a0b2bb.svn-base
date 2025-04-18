using System;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;
using Server.Items;
using Server.Scripts;

namespace Server.SkillHandlers
{
    public class Provocation
    {
        public static void Initialize()
        {
            SkillInfo.Table[(int)SkillName.Provocation].Callback = new SkillUseCallback(OnUse);
        }

        public static TimeSpan OnUse(Mobile m)
        {
            m.RevealingAction();

            BaseInstrument.PickInstrument(m, new InstrumentPickedCallback(OnPickedInstrument));

            return TimeSpan.FromSeconds(1.0); // Cannot use another skill for 1 second
        }

        public static void OnPickedInstrument(Mobile from, BaseInstrument instrument)
        {
            from.RevealingAction();
            from.SendLocalizedMessage(501587); // Whom do you wish to incite?
            from.Target = new InternalFirstTarget(from, instrument);
        }

        private class InternalFirstTarget : Target
        {
            private BaseInstrument m_Instrument;

            public InternalFirstTarget(Mobile from, BaseInstrument instrument)
                : base(BaseInstrument.GetBardRange(from, SkillName.Provocation), false, TargetFlags.None)
            {
                m_Instrument = instrument;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                from.RevealingAction();

                //Creature that can be harmed
                if (targeted is BaseCreature && from.CanBeHarmful((Mobile)targeted, true))
                {
                    BaseCreature creature = (BaseCreature)targeted;

                    //Has Instrument
                    if (!m_Instrument.IsChildOf(from.Backpack))
                    {
                        from.SendLocalizedMessage(1062488); // The instrument you are trying to play is no longer in your backpack!
                    }

                    //Non-Followers
                    else if (creature.Controlled)
                    {
                        from.SendLocalizedMessage(501590); // They are too loyal to their master to be provoked.
                    }

                    //If Creature Is A Paragon or Too Difficult
                    else if (creature.IsParagon || GetMobileDifficulty.GetDifficultyValue(creature) >= FeatureList.BardChanges.MaxProvocationDifficulty)
                    {
                        from.SendLocalizedMessage(1049446); // You have no chance of provoking those creatures.
                    }

                    else
                    {
                        from.RevealingAction();
                        m_Instrument.PlayInstrumentWell(from);
                        from.SendLocalizedMessage(1008085); // You play your music and your target becomes angered..  Whom do you wish them to attack?
                        from.Target = new InternalSecondTarget(from, m_Instrument, creature);
                    }
                }

                else
                {
                    from.SendLocalizedMessage(501589); // You can't incite that!
                }
            }
        }

        private class InternalSecondTarget : Target
        {
            private BaseCreature m_Creature;
            private BaseInstrument m_Instrument;

            public InternalSecondTarget(Mobile from, BaseInstrument instrument, BaseCreature creature)
                : base(BaseInstrument.GetBardRange(from, SkillName.Provocation), false, TargetFlags.None)
            {
                m_Instrument = instrument;
                m_Creature = creature;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                from.RevealingAction();

                Mobile target = targeted as Mobile;

                if (target != null && from.CanBeHarmful((Mobile)target, true))
                { 
                    if (!m_Instrument.IsChildOf(from.Backpack))
                    {
                        from.SendLocalizedMessage(1062488); // The instrument you are trying to play is no longer in your backpack!
                    }

                    else if (m_Creature.Unprovokable)
                    {
                        from.SendLocalizedMessage(1049446); // You have no chance of provoking those creatures.
                    }

                    //Creature Insn't Bard Range
                    else if (m_Creature.Map != target.Map || !m_Creature.InRange(target, BaseInstrument.GetBardRange(from, SkillName.Provocation)))
                    {
                        from.SendLocalizedMessage(1049450); // The creatures you are trying to provoke are too far away from each other for your music to have an effect.
                    }

                    //Target Isn't Itself
                    else if (m_Creature != target)
                    {   
                        //Creatures Can Harm Each Other
                        if (from.CanBeHarmful(m_Creature, true) && from.CanBeHarmful(target, true))
                        {
                            //No Possible Chance of Provoking
                            if (target is BaseCreature && ((BaseCreature)target).BardImmune)
                            {
                                from.NextSkillTime = DateTime.Now + TimeSpan.FromSeconds(5.0);
                                from.SendLocalizedMessage(1049446); // You have no chance of provoking those creatures.

                                return;
                            }
                            
                            //Pass Musicianship Check
                            if (!BaseInstrument.CheckMusicianship(from))
                            {
                                from.SendLocalizedMessage(500612); // You play poorly, and there is no effect.

                                from.NextSkillTime = DateTime.Now + TimeSpan.FromSeconds(5.0);
                                m_Instrument.PlayInstrumentBadly(from);
                                m_Instrument.ConsumeUse(from);
                            }

                            else
                            {                               
                                double bardSkill = from.Skills[SkillName.Provocation].Value;
                                
                                double creatureDifficulty = m_Instrument.GetDifficultyFor(m_Creature);
                                double creatureDifficultyScaled = 100 * (creatureDifficulty / FeatureList.BardChanges.MaxProvocationDifficulty);

                                double targetDifficulty = m_Instrument.GetDifficultyFor(target);
                                double targetDifficultyScaled = 100 * (targetDifficulty / FeatureList.BardChanges.MaxProvocationDifficulty);
                                
                                //If Target Creature Has 0 Difficulty (Likely its target is a player), Use Creature's Own Difficulty as Target Value
                                if (targetDifficultyScaled == 0)
                                {
                                    targetDifficultyScaled = creatureDifficultyScaled;
                                }

                                double targetAvgDifficulty = (creatureDifficultyScaled + targetDifficultyScaled) * 0.5;
                                
                                //Determine Min Provocation Needed
                                double minProvokeNeeded = targetAvgDifficulty;
                                double chance = ((bardSkill - minProvokeNeeded) * (100 / FeatureList.BardChanges.BardDifficultyModifier)) / 100;
                                                                
                                //Cap Maximum Success Chance
                                if (chance > FeatureList.BardChanges.ProvocationMaximumSuccessRate)
                                {
                                    chance = FeatureList.BardChanges.ProvocationMaximumSuccessRate;
                                }

                                bool success = chance >= Utility.RandomDouble();

                                //No Possible Chance of Provoking
                                if (chance <= 0)
                                {
                                    from.NextSkillTime = DateTime.Now + TimeSpan.FromSeconds(5.0);
                                    from.SendLocalizedMessage(1049446); // You have no chance of provoking those creatures.

                                    return;
                                }

                                //Skill Gain Check
                                from.CheckTargetSkill(SkillName.Provocation, target, minProvokeNeeded, minProvokeNeeded * FeatureList.BardChanges.BardDifficultySkillRangeMultiplier);
                                
                                //Fail Provocation Check
                                if (!success)
                                {
                                    from.SendLocalizedMessage(501599); // Your music fails to incite enough anger.

                                    from.NextSkillTime = DateTime.Now + TimeSpan.FromSeconds(5.0);
                                    m_Instrument.PlayInstrumentBadly(from);
                                    m_Instrument.ConsumeUse(from);
                                }

                                //Pass Provocation Check
                                else
                                {
                                    from.SendLocalizedMessage(501602); // Your music succeeds, as you start a fight.

                                    from.NextSkillTime = DateTime.Now + TimeSpan.FromSeconds(10.0);
                                    m_Instrument.PlayInstrumentWell(from);
                                    m_Instrument.ConsumeUse(from);
                                    m_Creature.Provoke(from, target, true);
                                }
                            }
                        }
                    }


                    else
                    {
                        from.SendLocalizedMessage(501593); // You can't tell someone to attack themselves!
                    }
                }

                else
                {
                    from.SendLocalizedMessage(501589); // You can't incite that!
                }
            }
        }
    }
}