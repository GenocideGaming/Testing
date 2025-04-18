using System;
using System.Collections;
using System.Collections.Generic;
using Server.Accounting;
using Server.Commands;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Items;
using Server.Regions;

namespace Server.Games
{
    /*
     * (February 2013)
     * The idea of a pseudoseer system was initially conceived by "a bazooka" of UOSA / "mob of monsters" of Rel Por (PM "the bazookas" on the
     * UOSA forums.uosecondage.com to contact) and proposed to Derrick, the creator of uosecondage as a new way to generate in-game content 
     * through more player interactions.  I (a bazooka) initially thought of a system that simply changed a playermobile to look like / have the attributes of a "possessed" mob,
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
     * Anyone is free to use / modify / redistribute this code so long as this byline remains.  NOTE that there is also 
     * a modification in the Account.cs in the core in order to allow reconnecting with the original playermobile after
     * they are done possessing an NPC mob.
     */
    public class CreaturePossession
    {
        public const AccessLevel FullAccessStaffLevel = AccessLevel.GameMaster;

        private static readonly List<BaseCreature> m_possessedMonsters = new List<BaseCreature>();
        internal static List<BaseCreature> PossessedMonsters { get { return m_possessedMonsters; } }

        public static void Initialize()
        {
            CommandSystem.Register("pseudoseeradd", AccessLevel.GameMaster, new CommandEventHandler(PseudoseerAdd_Command));
            CommandSystem.Register("unpossess", AccessLevel.Player, new CommandEventHandler(Unpossess_Command));
            CommandSystem.Register("possess", AccessLevel.Player, new CommandEventHandler(Possess_Command));
            CommandSystem.Register("direct", AccessLevel.Player, new CommandEventHandler(Direct_Command));

            CommandSystem.Register("otherpossess", AccessLevel.GameMaster, new CommandEventHandler(OtherPossess_Command));
            CommandSystem.Register("teamnoto", AccessLevel.GameMaster, new CommandEventHandler(TeamNoto_Command));
            CommandSystem.Register("teamharm", AccessLevel.GameMaster, new CommandEventHandler(TeamHarm_Command));
            CommandSystem.Register("psmovetolastpossessed", AccessLevel.GameMaster, new CommandEventHandler(PseudoseerMoveToLastPossessed));
            CommandSystem.Register("DungeonReport", AccessLevel.Player, new CommandEventHandler(DungeonReport_Command));
            CommandSystem.Register("DungeonGo", AccessLevel.Player, new CommandEventHandler(DungeonGo_Command));
            CommandSystem.Register("ps", AccessLevel.Player, new CommandEventHandler(PseudoseerChat_Command));
            CommandSystem.Register("say", AccessLevel.Player, new CommandEventHandler(Say_Command));
            CommandSystem.Register("LootMultiplier", AccessLevel.GameMaster, new CommandEventHandler(LootMultiplier_Command));
            //CommandSystem.Register("teamallowenemyhealing", AccessLevel.GameMaster, new CommandEventHandler(EnemyHealing_Command));
        }

        public static void Say_Command(CommandEventArgs e)
        {
            PlayerMobile player = GetAssociatedPlayerMobile(e.Mobile);

            if (e.Mobile.AccessLevel > AccessLevel.Player
                ||
                (player != null
                && player.PseudoSeerPermissions != null
                && player.PseudoSeerPermissions != ""))
            {
                if (e.Arguments == null || e.Arguments.Length == 0)
                {
                    e.Mobile.SendMessage("You must provide something for your target to say!  e.g '[say Hello!'");
                }
                else
                {
                    e.Mobile.Target = new SayTarget(e.ArgString);
                }
            }
        }

        private class SayTarget : Target
        {
            private string said = null;

            public SayTarget(string saying)
                : base(-1, false, TargetFlags.None)
            {
                said = saying;
            }

            protected override void OnTarget(Mobile from, object target)
            {
                if (from == null || target == null) return;
                if (said == null) 
                {
                    from.SendMessage("You must provide something for them to say!");
                    return;
                }
                if (!(target is Mobile))
                {
                    from.SendMessage("You must target a mobile!");
                    return;
                }
                Mobile mob = target as Mobile;
                if (from.AccessLevel > AccessLevel.Player || (target is BaseCreature && HasPermissionsToPossess(from, target as BaseCreature)))
                {
                    mob.Say(said);
                }
            }
        }

        public static void Say_Target(Mobile from, object o)
        {
            if (from == null || o == null) return;
            if (!(o is Mobile)) 
            {
                
            }
        }

        public static void LootMultiplier_Command(CommandEventArgs e)
        {
            if (PseudoSeerStone.Instance == null)
            {
                e.Mobile.SendMessage("There is no pseudoseer stone instance!  You must '[add pseudoseerstone' to use this command, since the multiplier is stored on the stone!");
                return;
            }
            if (e.Arguments == null || e.Arguments.Length == 0)
            {
                e.Mobile.SendMessage("You must give a float value!  e.g '[lootmultiplier 2' would double the loot dropped by all mobs in the world.'");
                return;
            }
            try
            {
                double value = Double.Parse(e.Arguments[0]);
                if (value >= 0)
                {
                    PseudoSeerStone.Instance.CreatureLootDropMultiplier = value;
                }
                else
                {
                    e.Mobile.SendMessage("You must provide a number greater than 0!");
                }
            }
            catch
            {
                e.Mobile.SendMessage("You must give a float value!  e.g '[lootmultiplier 2' would double the loot dropped by all mobs in the world.'");
            }
        }

        public static void PseudoseerChat_Command(CommandEventArgs e)
        {
            if (HasAnyPossessPermissions(e.Mobile))
            {
                if (PseudoSeerStone.Instance == null) {
                    e.Mobile.SendMessage("No pseudoseerstone exists... cannot chat.");
                    return;
                }
                PlayerMobile player = GetAssociatedPlayerMobile(e.Mobile);

                string name;
                if (player == null) { name = null; }
                else { name = player.Name; }
                foreach (Mobile mob in PseudoSeersControlGump.PseudoseerControlledMobiles())
                {
                    if (mob.Deleted || mob.NetState == null)
                    {
                        continue;
                    }
                    mob.SendMessage(38, "[PS] " + name + ": " + e.ArgString);
                }
            }
            else
            {
                e.Mobile.SendMessage("You do not have access to that command.");
            }
        }

        public static PlayerMobile GetAssociatedPlayerMobile(Mobile m)
        {
            PlayerMobile player = m as PlayerMobile;
            if (player == null) 
            {
                BaseCreature bc = m as BaseCreature;
                if (bc != null
                    && !bc.Deleted
                    && bc.NetState != null
                    && bc.NetState.Account != null)
                {
                    player = bc.NetState.Account.GetPseudoSeerLastCharacter() as PlayerMobile;
                }
            }
            return player;
        }

        public static void DungeonReport_Command(CommandEventArgs e)
        {
            PlayerMobile player = GetAssociatedPlayerMobile(e.Mobile);
            
            if (e.Mobile.AccessLevel > AccessLevel.Player 
                ||
                (player != null
                && player.PseudoSeerPermissions != null
                && player.PseudoSeerPermissions != ""
                && player.Pseu_DungeonWatchAllowed))
            {
                string message = "Players in each region: \n";
                foreach (Region region in Region.Regions)
                {
                    if (region is DungeonRegion)
                    {
                        message += region.Name + ": " + region.GetPlayerCount() + "\n";
                    }
                }
                e.Mobile.SendMessage(message);
            }
            else
            {
                e.Mobile.SendMessage("You do not have access to that command.");
            }
        }

        public static void DungeonGo_Command(CommandEventArgs e)
        {
            string[] args = e.Arguments;
            PlayerMobile player = GetAssociatedPlayerMobile(e.Mobile);
            if (e.Mobile.AccessLevel == AccessLevel.Player
                    &&
                        (player == null
                        || 
                        player.PseudoSeerPermissions == null
                        ||
                        player.PseudoSeerPermissions == ""
                        ||
                        player.Pseu_DungeonWatchAllowed == false))
            {
                e.Mobile.SendMessage("You do not have access to that command.");
                return;
            }
            if (args.Length == 0)
            {
                e.Mobile.SendMessage("You must provide a dungeon name, e.g. '[dungeongo endium'. Do '[dungeonreport' to get a list of possible names.");
                return;
            }

            foreach (Region region in Region.Regions)
            {
                if (region is DungeonRegion && region.Name.ToLower().Trim() == args[0].ToLower().Trim())
                {
                    if (region.GoLocation.Equals(new Point3D()) == false)
                    {
                        e.Mobile.Location = region.GoLocation;
                        return;
                    }
                }
            }
            e.Mobile.SendMessage(args[0] + " is not a valid location.  Use [dungeonreport to get a list of valid locations.");
        }

        public static void PseudoseerAdd_Command(CommandEventArgs e)
        {
            string[] args = e.Arguments;
            if (args != null)
            {
                if (args.Length > 0)
                { 
                    string permissions = "";
                    foreach (string arg in args)
                    {
                        if (arg == "all")
                        {
                            permissions = "all";
                            break;
                        }
                        permissions += arg + " ";
                    }
                    PseudoSeerStone.PermissionsClipboard = permissions.Trim();
                    e.Mobile.SendMessage("Permissions clipboard is now: " + permissions);
                    e.Mobile.SendMessage("Target a player to add as a pseudoseer / update their permissions.");
                    e.Mobile.Target = new PseudoseerAddTarget(permissions);
                }
                else
                {
                    e.Mobile.SendMessage("You must specify permissions!  E.g. '[pseudoseeradd orc orcishlord daemon'");
                }
            }
        }

        private class PseudoseerAddTarget : Target
        {
            public PseudoseerAddTarget(string permissions)
                : base(-1, false, TargetFlags.None)
            {
            }

            protected override void OnTarget(Mobile gm, object target)
            {
                PlayerMobile pm = target as PlayerMobile;
                if (pm == null)
                {
                    gm.SendMessage("Not a valid player target!");
                    return;
                }
                if (PseudoSeerStone.Instance == null)
                {
                    gm.SendMessage("ERROR: No Pseudoseerstone exists!  [add pseudoseerstone somewhere in the world and try again!");
                    return;
                }
                PseudoSeerStone.Instance.PseudoSeerAdd = pm;
                gm.SendMessage("They have been successfully added with the following permissions:\n"
                              + PseudoSeerStone.Instance.CurrentPermissionsClipboard);

            }
        }

        public static void OtherPossess_Command(CommandEventArgs e)
        {
            if (PseudoSeerStone.Instance == null)
            {
                e.Mobile.SendMessage("No pseudoseerstone exists.  You must [add pseudoseerstone somewhere in the world before you can use this command");
            }
            else
            {
                e.Mobile.BeginTarget(-1, false, TargetFlags.None, new TargetCallback(OtherPossess_PlayerTarget));
                e.Mobile.SendMessage("Target a player to add as pseudoseer with no permissions (unless they are already a pseudoseer). You will be prompted again to target a monster for them specifically to possess.");
            }
        }

        public static void OtherPossess_PlayerTarget(Mobile gm, object o)
        {
            if (o is PlayerMobile && ((PlayerMobile)o).AccessLevel == AccessLevel.Player)
            {
                gm.Target = new OtherPossessMonsterTarget(o as PlayerMobile);
                gm.SendMessage("Target a monster for that player to possess.");
            }
            else
            {
                gm.SendMessage("Not a valid PlayerMobile.");
            }
        }

        private class OtherPossessMonsterTarget : Target
        {
            private PlayerMobile m_Player;

            public OtherPossessMonsterTarget(PlayerMobile player)
                : base(-1, false, TargetFlags.None)
            {
                if (player == null) { return; }
                m_Player = player;
            }

            protected override void OnTarget(Mobile gm, object target)
			{
                if (m_Player == null) {
                    gm.SendMessage("ERROR: somehow didn't get a player target first!");
                    return;
                }
                if (PseudoSeerStone.Instance == null) 
                {
                    gm.SendMessage("ERROR: somehow didn't have a pseudoseerstone available!  [add pseudoseerstone somewhere in the world to try again!");
                    return;
                }

                if (target is BaseCreature )
                {
                    // player isn't already a pseudoseer
                    if (!PseudoSeerStone.Instance.PseudoSeers.ContainsKey(m_Player.Account)) {
                        // have to temporarily store the clipboard so we can add a pseudoseer with no permissions
                        string oldClipboard = PseudoSeerStone.PermissionsClipboard;
                        PseudoSeerStone.PermissionsClipboard = "";
                        PseudoSeerStone.Instance.PseudoSeerAdd = m_Player;
                        PseudoSeerStone.PermissionsClipboard = oldClipboard;
                    }
                    ForcePossessCreature(gm, m_Player, target as BaseCreature);
                }
                else
                {
                    gm.SendMessage("Not a valid Monster to be possessed.  Player NOT added as no-permission pseudoseer.");
                }
			}
        }

        public static void TeamNoto_Command(CommandEventArgs e)
        {
            string[] args = e.Arguments;
            if (args != null)
            {
                bool error = false;
                if (args.Length > 0)
                {
                    try 
                    {
                        int val = int.Parse(args[0]);
                        if (val < 0 || val > 3)
                        {
                            error = true;
                        }
                        else
                        {
                            AITeamList.NotoType = (AITeamList.NotoTypeEnum)Enum.ToObject(typeof(AITeamList.NotoTypeEnum), val);
                        }
                    }
                    catch (Exception) { error = true; }
                }
                else
                {
                    error = true;
                }
                if (error)
                {
                    e.Mobile.SendMessage("Current noto type = " + AITeamList.NotoType + "\n... change with [teamnoto (int)\n"
                                            + "For all options below, all enemy team members will flag orange, regardless of other noto:\n"
                                            + "   0 = all allies are green\n"
                                            + "   1 = all allies are blue/grey/red but enemy militia are not orange\n"
                                            + "   2 = all allies standard noto (enemy militia/guilds flag orange)"
                                            + "   3 = all allies standard noto, and cannot heal allies in an enemy militia");
                }
            }
        }

        public static void TeamHarm_Command(CommandEventArgs e)
        {
            string[] args = e.Arguments;
            if (args != null)
            {
                bool error = false;
                if (args.Length > 0)
                {
                    try
                    {
                        string val = args[0].ToLower().Trim();
                        if (val == "true")
                        {
                            AITeamList.TeamHarm = true;
                            e.Mobile.SendMessage("Successfully set to true.  Teams can now harm each other");
                        }
                        else if (val == "false")
                        {
                            AITeamList.TeamHarm = false;
                            e.Mobile.SendMessage("Successfully set to false.  Teams cannot harm each other anymore.");
                        }
                        else
                        {
                            error = true;
                        }
                    }
                    catch (Exception) { error = true; }
                }
                else
                {
                    error = true;
                }
                if (error)
                {
                    e.Mobile.SendMessage("You must provide either true or false as an argument!");
                }
            }
        }

        public static void PseudoseerMoveToLastPossessed(CommandEventArgs e)
        {
            if (PseudoSeerStone.Instance == null)
            {
                e.Mobile.SendMessage("There was no pseudoseer stone!  You must [add pseudoseerstone somewhere in the world first!");
                return;
            }
            string[] args = e.Arguments;
            if (args != null)
            {
                bool error = false;
                if (args.Length > 0)
                {
                    try
                    {
                        string val = args[0].ToLower().Trim();
                        if (val == "true")
                        {
                            PseudoSeerStone.Instance.MovePSeerToLastPossessed = true;
                            e.Mobile.SendMessage("Successfully set to true.  Pseudoseers will now move to the position of the creature they last possessed when unpossessing.");
                        }
                        else if (val == "false")
                        {
                            PseudoSeerStone.Instance.MovePSeerToLastPossessed = false;
                            e.Mobile.SendMessage("Successfully set to false.  Pseudoseers will NOT move to the position of the creature they last possessed when unpossessing.");
                        }
                        else
                        {
                            error = true;
                        }
                    }
                    catch (Exception) { error = true; }
                }
                else
                {
                    error = true;
                }
                if (error)
                {
                    e.Mobile.SendMessage("You must provide either true or false as an argument!");
                }
            }
        }

        public static void Direct_Command(CommandEventArgs e)
        {
            if (CreaturePossession.HasAnyPossessPermissions(e.Mobile))
            {
                string[] args = e.Arguments;
                if (args != null)
                {
                    if (args.Length > 0)
                    {
                        if (args[0].ToLower() == "force")
                        {
                            e.Mobile.Target = new DirectTarget(true);
                            e.Mobile.SendMessage("Target location to direct nearby mobs.");
                        }
                        else
                        {
                            e.Mobile.SendMessage("Use either '[direct' or '[direct force' with this command!");
                        }
                        
                    }
                    else
                    {
                        e.Mobile.SendMessage("Target location to direct nearby mobs (use '[direct force' to move in combat).");
                        e.Mobile.Target = new DirectTarget(false);
                    }
                }
            }
        }

        private class DirectTarget : Target
        {
            public bool Force = false;
            public DirectTarget(bool force)
                : base(-1, true, TargetFlags.None)
            {
                this.Force = force;
            }

            protected override void OnTarget(Mobile from, object target)
            {
                // fix this with a waypoint?  timeout on waypoint?  Dictionary for mobile lookup?
                if (!(target is IPoint3D))
                {
                    from.SendMessage("Not a valid target.");
                    return;
                }
                IPoint2D targetLocation = target as IPoint2D;
                foreach (Mobile m in from.GetMobilesInRange(10))
                {
                    if (m is BaseCreature && HasPermissionsToPossess(from, (BaseCreature)m) && !m.Blessed)
                    {
                        BaseCreature bc = m as BaseCreature;
                        bc.TargetLocation = targetLocation;
                        if (this.Force && bc.AIObject != null)
                        {
                            bc.ForceWaypoint = true;
                            bc.AIObject.WanderMode();
                        }
                    }
                }
            }
        }       

        public static void Unpossess_Command(CommandEventArgs e)
        {
            if (e.Mobile is PlayerMobile) { return; }
            if (!AttemptReturnToOriginalBody(e.Mobile.NetState))
            {
                e.Mobile.NetState.Dispose();
            }
        }

        public static bool AttemptReturnToOriginalBody(NetState monsterNetState)
        {
            if (monsterNetState == null) { return false; }
            if (PseudoSeerStone.Instance == null || monsterNetState == null || monsterNetState.Account == null || monsterNetState.Account.GetPseudoSeerLastCharacter() == null)
            {
                return false;
            }
            PlayerMobile pseudoSeerLastCharacter = (PlayerMobile)monsterNetState.Account.GetPseudoSeerLastCharacter();
            Point3D newLocation = new Point3D(0,0,0);
            if (monsterNetState.Mobile != null 
                && pseudoSeerLastCharacter.PseudoSeerPermissions != null
                && pseudoSeerLastCharacter.PseudoSeerPermissions != ""
                && PseudoSeerStone.Instance.MovePSeerToLastPossessed)
            {
                newLocation = monsterNetState.Mobile.Location; 
            }
            bool output = CreaturePossession.ConnectClientToPC(monsterNetState, pseudoSeerLastCharacter);
            // do it after they log back in
            if (!(newLocation.X==0 && newLocation.Y==0 && newLocation.Z==0)) { pseudoSeerLastCharacter.Location = newLocation; }
            return output;
        }

        public static void Possess_Command(CommandEventArgs e)
        {
            OnPossessTargetRequest(e.Mobile);
        }

        public static void OnPossessTargetRequest(Mobile from)
        {
            if (CreaturePossession.HasAnyPossessPermissions(from))
            {
                from.BeginTarget(-1, false, TargetFlags.None, new TargetCallback(OnPossessTarget));
                from.SendMessage("Target a creature to possess.");
            }
        }
                
        public static void OnPossessTarget(Mobile from, object o)
        {
            if (o is BaseCreature)
                CreaturePossession.TryPossessCreature(from, (BaseCreature)o);
            else
                from.SendMessage("Not a valid Monster to be possessed.");
        }

        public static void TryPossessCreature(Mobile from, BaseCreature Subject)
        {
            if (HasPermissionsToPossess(from, Subject))
            {
                if (Subject.NetState != null)
                {
                    from.SendMessage("Target creature is already under player control!");
                    return;
                }
                if (IsInHouseOrBoat(Subject.Location, Map.Felucca))
                {
                    from.SendMessage("You cannot possess a creature in a house or boat.");
                    return;
                }
                if (Subject.Controlled && !IsAuthorizedStaff(from))
                {
                    from.SendMessage("You cannot possess tamed or summoned creatures!");
                    return;
                }
                Subject.HasBeenPseudoseerControlled = true;
                if (Subject.Backpack == null)
                {
                    // create a backpack for things like animals that have no backpack
                    // ... this prevents client crash in case somebody has their pack auto-opening on login
                    Subject.PackItem(new Gold(1)); 
                }
                ConnectClientToNPC(from.NetState, Subject);
            }
            else
            {
                from.SendMessage("You are not permitted to possess that creature type, only staff can grant this permission.");
            }
        }

        public static void ForcePossessCreature(Mobile gm, PlayerMobile player, BaseCreature Subject)
        {
            if (Subject == null || player == null)
            {
                if (gm != null) gm.SendMessage("ForcePossessCreature: Player or Subject was null!  Can't continue with possession.");
                return;
            }
            if (Subject.NetState != null)
            {
                if (gm != null) gm.SendMessage("Target creature is already under player control!");
                return;
            }
            Subject.HasBeenPseudoseerControlled = true;
            ConnectClientToNPC(player.NetState, Subject);
        }

        public static bool HasPermissionsToPossess(Mobile from, BaseCreature Subject)
        {
            return from != null && (from.AccessLevel >= AccessLevel.Counselor || HasPermissionsToPossess(from.NetState, Subject));
        }

        public static bool HasPermissionsToPossess(NetState from, BaseCreature Subject)
        {
            return from != null && IsAuthorizedAccount(from.Account, Subject);
        }

        public static bool HasAnyPossessPermissions(Mobile from)
        {
            return from != null && (from.AccessLevel >= AccessLevel.Counselor || HasAnyPossessPermissions(from.NetState));
        }

        public static bool HasAnyPossessPermissions(NetState from)
        {
            return from != null && 
                (CreaturePossession.IsAuthorizedStaff(from) || (PseudoSeerStone.Instance != null && PseudoSeerStone.Instance.GetPermissionsFor(from.Account) != null));
        }

        static bool IsAuthorizedAccount(IAccount account, BaseCreature Subject)
        {
            if (account == null) return false;
            
            if (IsAuthorizedStaff(account)) return true;
            
            if (PseudoSeerStone.Instance == null) return false;
            
            Type creaturetype = Subject.GetType();
            string perms = PseudoSeerStone.Instance.GetPermissionsFor(account);
            if (perms == null)
            {
                return false;
            }
            string[] permittedTypeStrings = perms.Split();
            string[] typesegments = (creaturetype.ToString()).Split('.'); // string is something like Server.Mobiles.Orc
            if (typesegments.Length == 0)
            {
                return false;
            }
            foreach (string permittedTypeString in permittedTypeStrings)
            {
                if (permittedTypeString.ToLower() == typesegments[typesegments.Length - 1].ToLower() || permittedTypeString == "all")
                {
                    return true;
                }

            }
            return false;
        }

        internal static bool IsAuthorizedStaff(Mobile from)
        {
            return from.AccessLevel >= AccessLevel.Counselor || (from != null && IsAuthorizedStaff(from.NetState));
        }

        internal static bool IsAuthorizedStaff(NetState from)
        {
            return from != null && (IsAuthorizedStaff(from.Account) || (from.Mobile != null && from.Mobile.AccessLevel >= AccessLevel.Counselor));
        }

        internal static bool IsAuthorizedStaff(IAccount account)
        {
            return account!=null && account.AccessLevel >= FullAccessStaffLevel;
        }

        internal static bool ConnectClientToNPC(NetState client, BaseCreature Subject)
        {
            if (Subject.NetState == null)
            {
                Mobile clientMobile = client.Mobile;
                if (client.Account != null && clientMobile is PlayerMobile) client.Account.SetPseudoSeerLastCharacter(client.Mobile);
                
                Subject.NetState = client;

                if (clientMobile != null)
                {
                    clientMobile.NetState = null;
                }

                Subject.NetState.Mobile = Subject;

                PacketHandlers.DoLogin(Subject.NetState, Subject);

                PossessedMonsters.Add(Subject);
                return true;
            }
            return false;
        }

        internal static bool ConnectClientToPC(NetState client, PlayerMobile Subject)
        {
            if (Subject != null && Subject.NetState == null)
            {
                Mobile clientMobile = client.Mobile;

                Subject.NetState = client;

                if (clientMobile != null)
                    clientMobile.NetState = null;

                Subject.NetState.Mobile = Subject;

                PacketHandlers.DoLogin(Subject.NetState, Subject);
                return true;
            }
            return false;
        }

        internal static void BootAllPossessions()
        {
            for (int i = 0; i < PossessedMonsters.Count; i++)
            {
                if (PossessedMonsters[i].Deleted || PossessedMonsters[i].NetState == null)
                    continue;

                //boot any player controlled monsters, attempting to return them to their original body if possible
                if (AttemptReturnToOriginalBody(PossessedMonsters[i].NetState) == false)
                {
                    PossessedMonsters[i].NetState.Dispose();
                }
            }
            PossessedMonsters.Clear();
        }

        // from Derrick
        public static bool IsInHouseOrBoat(Point3D location, Map map)
        {
            Region region = Region.Find(location, map);
            return (region != null && region.GetRegion(typeof(Regions.HouseRegion)) != null)
                || Server.Multis.BaseBoat.FindBoatAt(location, map) != null;
        }

        static void LogoutIfPlayer(Mobile from)
        {
            if (from is PlayerMobile && from.Map != Map.Internal)
                EventSink.InvokeLogout(new LogoutEventArgs(from));
        }
    }
}
