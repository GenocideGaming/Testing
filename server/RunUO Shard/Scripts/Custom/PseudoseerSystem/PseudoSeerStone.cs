using System;
using System.Collections.Generic;
using Server.Accounting;
using Server.Gumps;
using Server.Mobiles;

namespace Server.Games
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
    
    class PseudoSeerStone : Item
    {
        public static void Initialize()
        {
            if (PseudoSeerStone.Instance == null) { return; }
            for (int i = 0; i < PseudoseerUserNamesToReinstate.Count; i++)
            {
                try 
                {
                    IAccount account = Accounts.GetAccount(PseudoseerUserNamesToReinstate[i]);
                    if (account != null)
                    {
                        PseudoSeerStone.Instance.PseudoSeers.Add(account, PseudoseerPermissionsToReinstate[i]);
                    }
                } 
                catch
                {
                    Console.WriteLine("Account unable to be reinstated as pseudoseer: " + PseudoseerUserNamesToReinstate[i]);
                }
                
            }
        }


        private static PseudoSeerStone m_Instance;
        public static PseudoSeerStone Instance { get { return m_Instance; } }
        public static List<string> PseudoseerUserNamesToReinstate = new List<string>();
        public static List<string> PseudoseerPermissionsToReinstate = new List<string>();
        protected readonly Dictionary<IAccount, string> m_pseudoSeers = new Dictionary<IAccount, string>();
        public Dictionary<IAccount, string> PseudoSeers { get { return m_pseudoSeers; } }

        protected Timer m_pseudoSeerTimer;
        public Timer PseudoSeerTimerInstance { get { return m_pseudoSeerTimer; } }

        protected DateTime m_startTime;
        protected TimeSpan m_duration;
        [CommandProperty(CreaturePossession.FullAccessStaffLevel)]
        public TimeSpan SeerDuration { get { return m_duration; } set { m_duration = value; } }
        
        protected bool m_msgStaff;
        [CommandProperty(CreaturePossession.FullAccessStaffLevel)]
		public bool MessageStaff{ get{ return m_msgStaff; } set{ m_msgStaff = value; } }

        protected double m_CreatureLootDropMultiplier;
        [CommandProperty(CreaturePossession.FullAccessStaffLevel)]
        public double CreatureLootDropMultiplier { get { return m_CreatureLootDropMultiplier; } set { m_CreatureLootDropMultiplier = value; } }

        [CommandProperty(CreaturePossession.FullAccessStaffLevel)]
        public TimeSpan TimeLeft { get { return m_timerRunning ? (m_duration - (DateTime.Now - m_startTime))
                                                               : TimeSpan.MaxValue; } }

        protected bool m_MovePseerToLastPossessed = true;
        [CommandProperty(CreaturePossession.FullAccessStaffLevel)]
        public bool MovePSeerToLastPossessed
        {
            get { return m_MovePseerToLastPossessed; }
            set { m_MovePseerToLastPossessed = value; }
        }

        protected bool m_timerRunning;
        [CommandProperty(CreaturePossession.FullAccessStaffLevel)]
        public bool _TimerRunning {  get { return m_timerRunning; } 
            set
            {
                if (value == m_timerRunning)  return;
                m_timerRunning = value;
                if (m_timerRunning == false) 
                {
                    if (m_pseudoSeerTimer != null) {
                        m_pseudoSeerTimer.Stop();
                    }
                } 
                else 
                {
                    m_startTime = DateTime.Now;
                    m_pseudoSeerTimer = new PseudoSeerTimer();
                    m_pseudoSeerTimer.Start();
                }
            }
        }

        [CommandProperty(CreaturePossession.FullAccessStaffLevel)]
        public bool _ClearPseudoSeers {  get { return false; } 
            set
            {
                if (value == false)  return;
                CleanUp();
            }
        }

        [CommandProperty(CreaturePossession.FullAccessStaffLevel)]
        public Mobile PseudoSeerAdd
        {
            get { return null; }
            set
            {
                PlayerMobile pm = value as PlayerMobile;
                
                if (pm!=null && pm.Account!=null)
                {
                    PseudoSeers[pm.Account] = CurrentPermissionsClipboard;
                    pm.SendGump(new PossessGump(pm));
                }
            }
        }
        [CommandProperty(CreaturePossession.FullAccessStaffLevel)]
        public Mobile PseudoSeerRemove
        {
            get { return null; }
            set
            {
                PlayerMobile pm = value as PlayerMobile;
                if (pm != null && pm.Account != null && PseudoSeers.ContainsKey(pm.Account))
                {
                    PseudoSeers.Remove(pm.Account);
                    pm.CloseGump(typeof(PossessGump));
                }
            }
        }

        public static string PermissionsClipboard = "";
        [CommandProperty(CreaturePossession.FullAccessStaffLevel)]
        public string CurrentPermissionsClipboard
        {
            get { return PermissionsClipboard; }
            set { PermissionsClipboard = value; }
        }
     
        [Constructable]
        public PseudoSeerStone() : base(0xEDC)
        {
            this.Name = "Pseudoseer Stone";
            this.Movable = false;
            this.SeerDuration = TimeSpan.MaxValue;
            this.MessageStaff = true;

            if (m_Instance != null)
            {
                // there can only be one PseudoSeerStone game stone in the world
                m_Instance.Delete();
                Server.Commands.CommandHandlers.BroadcastMessage(CreaturePossession.FullAccessStaffLevel, 0x489, 
                    "Old PseudoSeerStone gamestone has been deleted as new one was added.");
            }
            m_Instance = this;
        }

        public PseudoSeerStone(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
		{
			base.Serialize( writer );

			writer.Write( (int)4 );//version
            // verision 4
            writer.Write((double)m_CreatureLootDropMultiplier);
            // version 3
            writer.Write(m_MovePseerToLastPossessed);
            // version 2
            List<Mobile> mobilesToWrite = new List<Mobile>();
            foreach (IAccount key in PseudoSeers.Keys)
            {
                Mobile lastChar = key.GetPseudoSeerLastCharacter();
                if (lastChar != null)
                {
                    mobilesToWrite.Add(lastChar);
                }
                else
                {
                    if (key is Account)
                    {
                        Account account = key as Account;
                        // attempt to add first mobile on the account as pseudoseer--will use it's account in deserialize
                        if (account.Mobiles != null && account.Mobiles.Length > 0 && account.Mobiles[0] != null && account == account.Mobiles[0].Account)
                        {
                            mobilesToWrite.Add(account.Mobiles[0]);
                        }
                    }
                }
            }
            writer.Write((int)mobilesToWrite.Count);
            foreach (Mobile mob in mobilesToWrite)
            {
                writer.Write((string)mob.Account.Username);
                writer.Write((string)PseudoSeers[mob.Account]);
            }
            
            writer.Write(CurrentPermissionsClipboard);
            writer.Write(SeerDuration);
		    writer.Write(MessageStaff);
            writer.Write((int)0); // not used anymore
            // NOTE: The pseudoseer list & possessed monsters list are not serialized because
            // if server goes down, pseudoseers lose connection to possessed monsters (and cannot log back into them)
            // and the pseudoseer list is emptied, meaning they can no longer use the [possess command
		}

        public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

            if (version >= 4)
            {
                m_CreatureLootDropMultiplier = reader.ReadDouble();
            }
            if (version >= 3)
            {
                m_MovePseerToLastPossessed = reader.ReadBool();
            }
            if (version >= 2) // retain pseudoseer state... DOES NOT WORK... can't access account through mob :(
            {
                int numPseudoseers = reader.ReadInt();
                for (int i = 0; i < numPseudoseers; i++)
                {
                    PseudoseerUserNamesToReinstate.Add(reader.ReadString());
                    PseudoseerPermissionsToReinstate.Add(reader.ReadString());
                }
            }
            if (version >= 1)
            {
                CurrentPermissionsClipboard = reader.ReadString();
            }
            SeerDuration = reader.ReadTimeSpan();
            MessageStaff = reader.ReadBool();
            reader.ReadInt(); // don't use this anymore

            m_Instance = this;
		}

        
        public string GetPermissionsFor(IAccount account)
        {
            if (account == null || !PseudoSeers.ContainsKey(account))
                return null;
            return PseudoSeers[account];
        }

        public override void OnAfterDelete()
        {
            CleanUp();
            m_Instance = null;
            base.OnAfterDelete();
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (CreaturePossession.IsAuthorizedStaff(from))
            {
                from.SendGump(new PseudoSeersControlGump(from));
                /*
                Gump gump = new Gump(0, 0);
                gump.AddPage( 0 );
				gump.AddBackground(0, 0, 420, 420, 5054);
                gump.AddHtml(10, 10, 400, 400, "<p>To use this stone, you can set the following attributes:</p>"
                                + "**<u>_ClearPseudoSeers</u>: If set to true, current pseudoseers are removed and possesed monsters are kicked. This attribute remains false, and can be set to true anytime.\n"
                                + "**<u>_TimerRunning</u>: Start / stop the timer for clearing the pseudoseer list (see SeerDuration).\n"
                                + "**<u>MessageStaff</u>: Messages between pseudoseers forwarded to staff (not implemented yet)\n"
                                + "**<u>PermissionAdd</u>: Add a monster group to CurrentPermissionsClipboard (selecting None = remove all groups).  This attribute remains \"None\", as making a selection only adds to the bit flags in CurrentPermissionsClipboard.  NOTE: All is at the end of the list\n"
                                + "**<u>PermissionRemove</u>: Opposite of PermissionAdd\n"
                                + "**<u>CurrentPermissionsClipboard</u>: bit flags for monster groups that pseudoseers can be possessed.  Permissions are assigned when a pseudoseer is added using PseudoSeerAdd.\n"
                                + "**<u>PseudoSeerAdd</u>: Add pseudoseer to the list (or update Permissions for an existing one)\n"
                                + "**<u>PseudoSeerRemove</u>: Remove a pseudoseer (does NOT kick monsters possessed by that pseudoseer)\n"
                                + "**<u>SeerDuration</u>: If _TimerRunning is true, after this amount of time, _ClearPseudoSeers is called as above\n"
                                + "**<u>TimeLeft</u>: Self-explanatory", true, true);
                from.SendGump(gump);
                */
            }
            else
            {
                from.SendMessage("Sorry, but you don't have permission access this.");
            }
            base.OnDoubleClick(from);
        }

        virtual protected void CleanUp()
        {
            if (PseudoSeerTimerInstance != null) {
                PseudoSeerTimerInstance.Stop();
            }
            // need to close all the PossessionGumps for existing PseudoSeers
            foreach (IAccount account in PseudoSeers.Keys) {
                Mobile pseudoSeerLastCharacter = account.GetPseudoSeerLastCharacter();
                if (pseudoSeerLastCharacter != null && pseudoSeerLastCharacter.NetState != null)
                {
                    pseudoSeerLastCharacter.CloseGump(typeof(PossessGump));
                }
            }
            PseudoSeers.Clear();
            CreaturePossession.BootAllPossessions();
        }

        public void PseudoSeerMessage(string message, params object[] args)
        {
            PseudoSeerMessage(String.Format(message, args));
        }

        public void PseudoSeerMessage(string message)
        {
            foreach (PlayerMobile member in PseudoSeers.Keys)
            {
                member.SendMessage(0x489, message);
            }
            if (m_msgStaff) Server.Commands.CommandHandlers.BroadcastMessage(CreaturePossession.FullAccessStaffLevel, 0x489, message);
        }

        protected class PseudoSeerTimer : Timer
        {
            public PseudoSeerTimer() : base(TimeSpan.FromMinutes(1.0))
            {
                Priority = TimerPriority.OneMinute;
            }

            protected override void OnTick()
            {
                if (PseudoSeerStone.Instance == null)
                {
                    Stop();
                    return;
                }
                PseudoSeerStone.Instance.CleanUp();
                PseudoSeerStone.Instance._TimerRunning = false;
                Stop();
            }
        }
        

    }
}