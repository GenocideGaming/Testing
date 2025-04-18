using System;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;
using Server.Items;
using Server.Scripts;

namespace Server.SkillHandlers
{
    public class Peacemaking
    {
        public static void Initialize()
        {
            SkillInfo.Table[(int)SkillName.Peacemaking].Callback = new SkillUseCallback(OnUse);
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
            from.SendLocalizedMessage(1049525); // Whom do you wish to calm?
            from.Target = new InternalTarget(from, instrument);
            from.NextSkillTime = DateTime.Now + TimeSpan.FromHours(6.0);
        }

        private class InternalTarget : Target
        {
            private BaseInstrument m_Instrument;
            private bool m_SetSkillTime = true;

            public InternalTarget(Mobile from, BaseInstrument instrument)
                : base(BaseInstrument.GetBardRange(from, SkillName.Peacemaking), false, TargetFlags.None)
            {
                m_Instrument = instrument;
            }

            protected override void OnTargetFinish(Mobile from)
            {
                if (m_SetSkillTime)
                    from.NextSkillTime = DateTime.Now;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                from.RevealingAction();

                if (!(targeted is Mobile))
                {
                    from.SendLocalizedMessage(1049528); // You cannot calm that!
                }

                else if (!m_Instrument.IsChildOf(from.Backpack))
                {
                    from.SendLocalizedMessage(1062488); // The instrument you are trying to play is no longer in your backpack!
                }

                else
                {
                    m_SetSkillTime = false;
                    from.NextSkillTime = DateTime.Now + TimeSpan.FromSeconds(10.0);

                    //Target Self: Entire Area of Effect
                    if (targeted == from)
                    {
                        //Fail Musicianship Check
                        if (!BaseInstrument.CheckMusicianship(from))
                        {
                            from.SendLocalizedMessage(500612); // You play poorly, and there is no effect.
                            m_Instrument.PlayInstrumentBadly(from);
                            m_Instrument.ConsumeUse(from);
                        }

                        //Fail Peacemaking Check
                        else if (!from.CheckSkill(SkillName.Peacemaking, 0.0, 100.0))
                        {
                            from.SendLocalizedMessage(500613); // You attempt to calm everyone, but fail.
                            m_Instrument.PlayInstrumentBadly(from);
                            m_Instrument.ConsumeUse(from);
                        }

                        //Peacemaking Success
                        else
                        {
                            from.NextSkillTime = DateTime.Now + TimeSpan.FromSeconds(5.0);
                            m_Instrument.PlayInstrumentWell(from);
                            m_Instrument.ConsumeUse(from);

                            Map map = from.Map;

                            if (map != null)
                            {
                                int range = BaseInstrument.GetBardRange(from, SkillName.Peacemaking);

                                bool calmed = false;

                                foreach (Mobile m in from.GetMobilesInRange(range))
                                {
                                    if ((m is BaseCreature && ((BaseCreature)m).Uncalmable) || (m is BaseCreature && ((BaseCreature)m).AreaPeaceImmune) || m == from || !from.CanBeHarmful(m, false))
                                        continue;

                                    calmed = true;

                                    m.SendLocalizedMessage(500616); // You hear lovely music, and forget to continue battling!

                                    if (m is BaseCreature && !((BaseCreature)m).BardPacified)
                                        ((BaseCreature)m).Pacify(from, DateTime.Now + TimeSpan.FromSeconds(1.0));
                                }

                                if (!calmed)
                                    from.SendLocalizedMessage(1049648); // You play hypnotic music, but there is nothing in range for you to calm.
                                else
                                    from.SendLocalizedMessage(500615); // You play your hypnotic music, stopping the battle.
                            }
                        }
                    }

                    //Target Other
                    else
                    {
                        Mobile targ = (Mobile)targeted;

                        //Cant Be harmed
                        if (!from.CanBeHarmful(targ, false))
                        {
                            from.SendLocalizedMessage(1049528);
                            m_SetSkillTime = true;
                        }

                        //Uncalmable Creature
                        else if (targ is BaseCreature && ((BaseCreature)targ).Uncalmable)
                        {
                            from.SendLocalizedMessage(1049526); // You have no chance of calming that creature.
                            m_SetSkillTime = true;
                        }

                        //Already Pacified
                        else if (targ is BaseCreature && ((BaseCreature)targ).BardPacified)
                        {
                            from.SendLocalizedMessage(1049527); // That creature is already being calmed.
                            m_SetSkillTime = true;
                        }

                        //Fail Musicianship Check
                        else if (!BaseInstrument.CheckMusicianship(from))
                        {
                            from.SendLocalizedMessage(500612); // You play poorly, and there is no effect.
                            from.NextSkillTime = DateTime.Now + TimeSpan.FromSeconds(5.0);
                            m_Instrument.PlayInstrumentBadly(from);
                            m_Instrument.ConsumeUse(from);
                        }

                        //Check Peacemaking
                        else
                        {
                            //Get Skill of Bard and Difficulty of Target
                            double bardSkill = from.Skills[SkillName.Peacemaking].Value;
                            double targetDifficulty = m_Instrument.GetDifficultyFor(targ);
                            double targetScaledDifficulty = 100 * (targetDifficulty / FeatureList.BardChanges.MaxPeacemakingDifficulty);
                             
                            //Player
                            if (targetScaledDifficulty == 0)
                            {
                                targetScaledDifficulty = 87.5; // 100 - 87.5 = 12.5 // 12.5 * (100 / 25)% = 50% chance
                            }                            

                            //Determine Min Peacemaking Needed
                            double minPeacemakingNeeded = targetScaledDifficulty;
                            double chance = ((bardSkill - minPeacemakingNeeded) * (100 / FeatureList.BardChanges.BardDifficultyModifier)) / 100;
                                
                            //Cap Maximum Success Rate Possible
                            if (chance > FeatureList.BardChanges.PeacemakingMaximumSuccessRate)
                            {
                                chance = FeatureList.BardChanges.PeacemakingMaximumSuccessRate;
                            }

                            bool success = chance >= Utility.RandomDouble();

                            //No Possible Chance of Calming
                            if (chance <= 0)
                            {
                                from.NextSkillTime = DateTime.Now + TimeSpan.FromSeconds(5.0);
                                from.SendLocalizedMessage(1049526); // You have no chance of calming that creature.

                                return;
                            }

                            //Skill Gain Check
                            from.CheckTargetSkill(SkillName.Provocation, targ, minPeacemakingNeeded, minPeacemakingNeeded * FeatureList.BardChanges.BardDifficultySkillRangeMultiplier);

                            //Fail Peacemaking Check
                            if (!success)
                            {
                                from.SendLocalizedMessage(1049531); // You attempt to calm your target, but fail.
                                m_Instrument.PlayInstrumentBadly(from);
                                m_Instrument.ConsumeUse(from);
                            }

                            //Pass Peacemaking Check
                            else
                            {
                                m_Instrument.PlayInstrumentWell(from);
                                m_Instrument.ConsumeUse(from);

                                from.NextSkillTime = DateTime.Now + TimeSpan.FromSeconds(7.0);

                                //Creature
                                if (targ is BaseCreature)
                                {
                                    BaseCreature bc = (BaseCreature)targ;

                                    from.SendLocalizedMessage(1049532); // You play hypnotic music, calming your target.                                    								

                                    double seconds = FeatureList.BardChanges.PeacemakingDuration;

                                    bc.Pacify(from, DateTime.Now + TimeSpan.FromSeconds(seconds));
                                }

                                //Player or Mobile
                                else
                                {
                                    from.SendLocalizedMessage(1049532); // You play hypnotic music, calming your target.

                                    targ.SendLocalizedMessage(500616); // You hear lovely music, and forget to continue battling!

                                    targ.Combatant = null;
                                    targ.Warmode = false;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}