using System;
using Server.Mobiles;
using Server.Targeting;
using Server.Items;

namespace Server.Commands
{
    /*
     * (February 2013)
     * The idea of a pseudoseer system was initially conceived by "a bazooka" of UOSA (PM "the bazookas" on the UOSA forums.uosecondage.com to contact)
     * and proposed to Derrick, the creator of uosecondage as a new way to generate in-game content through more player interactions.
     * I (a bazooka) initially thought of a system that simply changed a playermobile to look like / have the attributes of a "possessed" mob,
     * and there are some implementations of this on the RunUO forums... however, this approach is risky in that it potentially could create
     * a possible exploits and issues with ensuring that the Playermobile is returned to normal.
     * 
     * Derrick came up with the idea of simply transferring NetState over to the mob.  I (a bazooka) played around with it and determined that
     * this was certainly possible and, in fact, it worked extremely well to simply have the player "log in" as the NPC.  Thus the pseudoseer
     * system was born.  Derrick and I tested it significantly, and Derrick has integrated it with UOSA (~May 2012).
     * 
     * When I joined Rel Por as a developer (mob of monsters), I integrated it in Rel Por.  I also redid the permission system so it is now by string
     * rather than grouped (by monster type) bit flags.  i.e. to add somebody as a pseudoseer you would do "[pseudoseeradd orc balron ..." or set 
     * the PseudoSeerPermissions property of the playermobile to a space delimited string of NPC types.
     * 
     * Anyone is free to use / modify / redistribute this code so long as this byline remains.
     */
    
    class BreathCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("breath", AccessLevel.Player, new CommandEventHandler(Breath_Command));
        }

        public static void Breath_Command(CommandEventArgs e)
        {
            if (e.Mobile is BaseCreature)
            {
                BaseCreature creature = (BaseCreature)e.Mobile;
                if (creature.CanBreath)
                {
                    if (DateTime.Now >= creature.NextBreathTime)
                    {
                        if (e.Length > 0 && e.Arguments[0].ToLower() == "target")
                        {
                            e.Mobile.BeginTarget(-1, false, TargetFlags.None, new TargetCallback(FireBreathTarget));
                        }
                        else
                        {
                            if (creature.Combatant != null)
                            {
                                Mobile target = creature.Combatant;
                                AttemptFireBreath(creature, target);
                            }
                            else
                            {
                                creature.SendMessage("You aren't in combat with anything.  Use \"[breath target\" to breath on a specific target, "
                                    + "or use \"[breath\" again after entering combat.");
                            }
                        }
                    }
                    else
                    {
                        e.Mobile.SendMessage("You are not ready to breath fire again!  You must wait another " + (int)(creature.NextBreathTime - DateTime.Now).TotalSeconds + " seconds!");
                    }
                }
                else
                {
                    e.Mobile.Say("[breath " + e.ArgString);
                }
            }
            else
            {
                e.Mobile.Say("[breath " + e.ArgString);
            }
        }

        private static void FireBreathTarget(Mobile from, object o)
        {
            if (from is BaseCreature)
            {
                AttemptFireBreath((BaseCreature)from, (Mobile)o);
            }
            else
            {
                from.SendMessage("You aren't a BaseCreature!  This shouldn't be possible... please contact Staff.");
            }
        }

        private static void AttemptFireBreath(BaseCreature creature, Mobile target)
        {
            if (!creature.InLOS(target))
            {
                creature.SendMessage("Target cannot be seen.");
                return;
            }
            if (!target.InRange(creature, creature.BreathRange))
            {
                creature.SendMessage("That is too far away.");
                return;
            }
            if (target != null && target.Alive && !target.IsDeadBondedPet && creature.CanBeHarmful(target) &&
                target.Map == creature.Map && !creature.IsDeadBondedPet &&
                !creature.BardPacified)
            {
                creature.BreathStart(target);
                creature.SetNextBreathTime();
            }
            else
            {
                creature.SendMessage("You can't breathe fire on that!");
            }
        }

    }
}
