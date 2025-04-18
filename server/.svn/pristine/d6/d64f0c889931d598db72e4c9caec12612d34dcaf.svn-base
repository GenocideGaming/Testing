using System;
using System.Collections.Generic;
using System.Text;
using Server.Factions;
using Server.Network;
using Server.Scripts;
using Server.Commands;
using Server.Gumps;
using Server.Mobiles;
using Server.Scripts.Custom.WebService;
using Server.Scripts.Custom.Citizenship;
using Server.Custom;
using Server.Regions;
using Server.Scripts.Custom.VorshunWarsEquipment;
using System.IO;

namespace Server.Items
{
    public class WorldWarsController : Item
    {
        public static void Initialize()
        {
            CommandSystem.Register("nextworldwars", AccessLevel.Player, new CommandEventHandler(OnCommand_TimeUntilNextWar));
            CommandSystem.Register("wwscore", AccessLevel.Player, new CommandEventHandler(OnCommand_WorldWarScore));
            if (Instance != null)
            {
                // hacky way to ensure that everything is cleaned up in VorshunRegion (e.g. if there was a crash mid-game)
                Instance.m_GameTimer = new GameTimer(Instance, TimeSpan.FromHours(FeatureList.WorldWars.GameLengthInHours));
                Instance.Cleanup();
                VorshunStorage.CleanUp();
                Instance.m_GameTimer = null;
            }
        }

        public static void OnCommand_WorldWarScore(CommandEventArgs e)
        {
            if (Instance != null)
            {
                e.Mobile.SendGump(new WorldWarsResultsGump(Instance.m_Scores));
            }
            else
            {
                e.Mobile.SendMessage("World Wars are currently disabled!");
            }
        }

        public static void OnCommand_TimeUntilNextWar(CommandEventArgs e)
        {
            WorldWarsController controller = WorldWarsController.Instance;
            if (controller == null || controller.NextEventTime == DateTime.MaxValue)
            {
                e.Mobile.SendMessage("World wars are currently disabled.");
                return;
            }
            if (controller.NextEventTime < DateTime.Now)
            {
                e.Mobile.SendMessage("The next world wars will begin within an hour.");
            }
            else
            {
                TimeSpan timeBeforeNextWar = controller.NextEventTime - DateTime.Now;
                e.Mobile.SendMessage("The next world wars will take place in {0} days {1} hours {2} minutes.", timeBeforeNextWar.Days, timeBeforeNextWar.Hours, timeBeforeNextWar.Minutes);
            }
        }

        private static WorldWarsController m_Instance;
        public static WorldWarsController Instance { get { return m_Instance; } }

        private WorldWarsFlag[] m_Flags;
        private Dictionary<Point3D, int> m_FlagInfos;

        private Dictionary<Faction, int> m_Scores;
        private GameTimer m_GameTimer;
        
        private StartupTimer m_StartupTimer;

        private DateTime m_NextEventTime;
        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime NextEventTime { get { return m_NextEventTime; } }
        [CommandProperty(AccessLevel.GameMaster)]
        public double SetNextEventHoursFromNow
        { // USE XMLSPAWNER TO SET THIS!
            get { return -1; }
            set { m_NextEventTime = DateTime.Now + TimeSpan.FromHours(value); } 
        }
        private bool m_PetsAllowed = true;
        [CommandProperty(AccessLevel.GameMaster)]
        public bool PetsAllowed { get { return m_PetsAllowed; } set { m_PetsAllowed = value; } }

        private bool m_PetsReturn = true;
        [CommandProperty(AccessLevel.GameMaster)]
        public bool PetsReturn { get { return m_PetsReturn; } set { m_PetsReturn = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool ResetScores
        {
            get { return false; }
            set
            {
                List<Faction> factionsToUpdate = new List<Faction>();
                foreach (KeyValuePair<Faction, int> kv in m_Scores)
                {
                    factionsToUpdate.Add(kv.Key);
                }
                foreach (Faction faction in factionsToUpdate)
                {
                    m_Scores[faction] = 0;
                }
                LogScore("Scores Reset!");
            }
        }

        private int m_GameTimeInMinutes = 60;
        [CommandProperty(AccessLevel.GameMaster)]
        public int GameTimeInMinutes
        {
            get { return m_GameTimeInMinutes; }
            set { m_GameTimeInMinutes = value; }
        } 

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Active 
        { 
            get { if (m_GameTimer != null) return true; else return false; }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public bool StartGame { get { return false; } 
            set 
            {
                if (Active) return;
                if (value == true)
                {
                    SetupGame();
                }
            } 
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool StopGame
        {
            get { return false; }
            set
            {
                if (!Active) return;
                if (value == true)
                {
                    EndGame();
                }
            }
        }

        private string m_LastWinner;
        [CommandProperty(AccessLevel.GameMaster)]
        public string LastWinner
        {
            get { return m_LastWinner; }
            set { m_LastWinner = value; }
        }

        private TimeSpan m_GameTimeLeft;

        public TimeSpan GameTimeLeft { get { return m_GameTimeLeft; } set { m_GameTimeLeft = value; } }

        [Constructable]
        public WorldWarsController() : base(0x1B74) 
        {

            if (m_Instance == null || m_Instance.Deleted)
            {
                m_Instance = this;
            }
            else
            {
                base.Delete();
                return;
            }

            Movable = false;
            Visible = false;
            Name = "World Wars Controller";
            m_Scores = new Dictionary<Faction, int>();
            m_Scores.Add(Faction.Find("Lillano"), 0);
            m_Scores.Add(Faction.Find("Pedran"), 0);
            m_Scores.Add(Faction.Find("Arbor"), 0);
            m_Scores.Add(Faction.Find("Calor"), 0);
            m_Scores.Add(Faction.Find("Vermell"), 0);

            m_FlagInfos = new Dictionary<Point3D, int>();
            m_FlagInfos.Add(new Point3D(531, 1860, 26), 1);
            m_FlagInfos.Add(new Point3D(613, 1826, 26), 1);
            m_FlagInfos.Add(new Point3D(683, 1846, 26), 1);
            m_FlagInfos.Add(new Point3D(666, 1939, 26), 1);
            m_FlagInfos.Add(new Point3D(605, 1925, 26), 1);
            m_FlagInfos.Add(new Point3D(658, 1895, 46), 2);
            m_FlagInfos.Add(new Point3D(556, 1884, 46), 2);
            m_FlagInfos.Add(new Point3D(617, 1880, 60), 3);

            m_NextEventTime = DateTime.MaxValue;

            DestroyGameItemsByLocation();
        }

        public WorldWarsController(bool isTest)
        {
            if (m_Instance == null || m_Instance.Deleted)
                m_Instance = this;
            else
                base.Delete();
        }
        public WorldWarsController(Serial serial) : base(serial) { m_Instance = this; }

        public void SetupGame()
        {
            if (m_StartupTimer == null)
            {
                Broadcast("World wars will begin shortly, visit your township's stronghold to join! Only citizenship is required - you do NOT need to be in the militia. All supplies are provided; what you bring will be taken and returned to you.");

                BuildFlags();

                WorldWarGate.OpenGates();

                m_StartupTimer = new StartupTimer(this);
                m_StartupTimer.Start();
            }
        }

        public void StartGameCall()
        {
            if (m_StartupTimer != null && m_GameTimer == null)
            {
                Broadcast("World wars have started! Destroy and control as many flags as possible!");

                m_StartupTimer.Stop();
                m_StartupTimer = null;

                WorldWarPlank.OpenTeleporters();
                m_GameTimer = new GameTimer(this, TimeSpan.FromMinutes(m_GameTimeInMinutes));
                m_GameTimer.Start();
            }
        }
        public void EndGame()
        {
            if (m_GameTimer != null)
            {
                Faction winner = FindWinner();
                string winnerName = winner != null ? winner.Definition.TownshipName : "nobody";
                Broadcast("The world wars have ended. The winner is " + winnerName);
                LastWinner = winnerName;
                //DatabaseController.AddWorldWar(m_Scores);
                Cleanup();

                m_GameTimer.Stop();
                m_GameTimer = null;
            }
        }
        private Faction FindWinner()
        {
            Faction highest = null;
            int highestScore = 0;
            string log = DateTime.Now + "\t";
            foreach (KeyValuePair<Faction, int> kv in m_Scores)
	        {
                try
                {
                    log += kv.Key.Definition.TownshipName + "\t" + kv.Value + "\t";
                }
                catch(Exception e)
                {
                    log += e.Message;
                }
                if(kv.Value >= highestScore)
                {
                    highestScore = kv.Value;
                    highest = kv.Key;
                }
	        }
            LogScore(log);
            return highest == null ? null : (m_Scores[highest] == 0 ? null : highest);
        }
        public List<Mobile> MobilesToCleanupLater = new List<Mobile>();
        public void Cleanup()
        { 
            MobilesToCleanupLater = new List<Mobile>();
            foreach (KeyValuePair<Mobile, List<Item>> kv in VorshunStorage.Storage)
            {
                MobilesToCleanupLater.Add(kv.Key);
            }
            RemovePlayersFromIslandAndStagingArea();
            DestroyGameItemsByLocation();
            Timer.DelayCall(TimeSpan.FromMinutes(0.5), new TimerCallback(CleanUpCallback));
        }

        public void CleanUpCallback()
        {
            foreach (Mobile mob in MobilesToCleanupLater)
            {
                if (VorshunStorage.ContainsStuffBelongingTo(mob))
                {
                    Map map = mob.Map;
                    mob.MoveToWorld(WorldWarsController.Instance.GetRemoveToLocation(mob), map);
                    try
                    {
                        mob.CloseGump(typeof(ChooseEquipmentGump));
                    }
                    catch { }
                    VorshunStorage.RestoreOriginalItems(mob);
                }
            }
        }

        public static void LogScore(string msg)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter("VorshunScores.txt", true))
                {
                    writer.WriteLine(msg);
                }
            }
            catch { }
        }

        public void Update()
        {
            foreach (WorldWarsFlag flag in m_Flags)
            {
                ScorePointsForFlagTick(flag);
            }
        }
        private void BuildFlags()
        {
            int i = 0;
            m_Flags = new WorldWarsFlag[8];
            foreach (KeyValuePair<Point3D, int> kv in m_FlagInfos)
            {
                m_Flags[i] = new WorldWarsFlag(this, kv.Value, null);
                m_Flags[i].MoveToWorld(kv.Key, Map.Felucca);
                i++;
            }
         
        }
        private void DestroyFlags()
        {
            for (int i = 0; i < m_Flags.Length; i++)
            {
                if(m_Flags[i] != null)
                    m_Flags[i].Delete();
                m_Flags[i] = null;
            }
            m_Flags = null;
        }
        private void DestroyGameItemsByLocation()
        {

            foreach (KeyValuePair<Point3D, int> kv in m_FlagInfos)
            {
                IPooledEnumerable items = Map.Felucca.GetItemsInRange(kv.Key, 3);
                List<IEntity> toDelete = new List<IEntity>();
                foreach(Item item in items)
                {
                    if (item is Item)
                    {
                        toDelete.Add(item);
                    }
                }
                items.Free();
                for (int i = 0; i < toDelete.Count; ++i)
                    toDelete[i].Delete();
            }

            WorldWarGate.RemoveGates();
            WorldWarPlank.RemoveTeleporters();
        }
        private void RemovePlayersFromIslandAndStagingArea()
        {
            List<Mobile> mobs = null;
            List<BaseCreature> petsToReturn = new List<BaseCreature>();
            foreach (KeyValuePair<string, Region> entry in Map.Regions)
            {
                if (entry.Value is StagingAreaRegion || entry.Value is VorshunRegion)
                {
                    mobs = entry.Value.GetMobiles();
                    foreach (Mobile mob in mobs)
                    {
                        if (mob is PlayerMobile)
                        {
                            mob.MoveToWorld(GetRemoveToLocation(mob), Map.Felucca);
                            mob.CloseGump(typeof(ChooseEquipmentGump));
                            mob.SendGump(new WorldWarsResultsGump(m_Scores));
                        }
                        else if (WorldWarsController.Instance != null && WorldWarsController.Instance.PetsReturn && mob is BaseCreature)
                        {
                            BaseCreature bc = mob as BaseCreature;
                            if (bc.Controlled && bc.ControlMaster != null && bc.ControlMaster is PlayerMobile)
                            {
                                petsToReturn.Add(bc);
                            }
                        }
                    }
                }
            }
            foreach (BaseCreature pet in petsToReturn)
            {
                pet.MoveToWorld(pet.ControlMaster.Location, Map.Felucca);
            }
        }
        public Point3D GetRemoveToLocation(Mobile player)
        {
            PlayerMobile pm = player as PlayerMobile;
            if (pm != null 
                && pm.CitizenshipPlayerState != null 
                && pm.CitizenshipPlayerState.Commonwealth != null 
                && pm.CitizenshipPlayerState.Commonwealth.HeroHomeRegion != null)
            {
                return pm.CitizenshipPlayerState.Commonwealth.HeroHomeRegion.GoLocation;
            }
            else
            {
                return WorldWarGate.DefaultSendHomeLocation;
            }
        }
        private void Broadcast(string message)
        {
            foreach (NetState state in NetState.Instances)
            {
                Mobile player = state.Mobile;
                if (player != null)
                {
                    player.SendMessage(38, message);
                }

            }
        }
        public void ScorePointsForFlagTick(WorldWarsFlag flag)
        {
            if (flag == null)
            {
                return;
            }
            if (flag.Militia != null)
            {
                m_Scores[flag.Militia] += flag.Value;
            }

        }
        public void ScorePointsForFlagCapture(Mobile from, WorldWarsFlag flag, bool createLever)
        {
            ICommonwealth commonwealth = Commonwealth.Find(from);
            if (commonwealth != null)
            {
                m_Scores[commonwealth.Militia] += flag.Value;
                commonwealth.Broadcast("Your militia has succeeded in destroying a flag!");
            }

            if (createLever)
            {
                FlagLever flagLever = new FlagLever(this, flag);
                flagLever.MoveToWorld(new Point3D(flag.Location.X, flag.Location.Y, flag.Location.Z - 20), Map.Felucca); //offset the lever to the ground
            }

            int flagIndex = GetFlagIndex(flag.Location);
            m_Flags[flagIndex].Delete();
            m_Flags[flagIndex] = null;

            PlayerMobile player = from as PlayerMobile;
            if (player != null)
                player.WorldWarsFlagsCapturedThisSession++;
        }
        public void SpawnNewFlag(FlagLever lever, Mobile flagActivator)
        {
            int flagIndex = GetFlagIndex(lever.Location);
            if (flagIndex >= 0)
            {
                m_Flags[flagIndex] = new WorldWarsFlag(this, Commonwealth.Find(flagActivator), lever.ActivatingFlagValue, lever);
                m_Flags[flagIndex].MoveToWorld(new Point3D(lever.Location.X, lever.Location.Y, lever.Location.Z + 20), Map.Felucca); //offset flag into the air

            }
      
        }
        private int GetFlagIndex(Point3D position)
        {
            int i = 0;
            foreach (KeyValuePair<Point3D, int> kv in m_FlagInfos)
            {
                if (kv.Key.X == position.X && kv.Key.Y == position.Y)
                    return i;
                i++;
            }
            return -1;
        }

        //this returns a list of who owns which flag, this is ordered by the list of flag locations to be read by the worldwarsgump
        public WorldWarsFlag[] GetFlags()
        {
            return m_Flags;
        }
        
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1); // version
            //version 1
            writer.Write((bool)m_PetsAllowed);
            writer.Write((bool)m_PetsReturn);
            //version 0
            writer.Write((DateTime)m_NextEventTime);
            writer.Write((int)m_GameTimeInMinutes);
            writer.Write((string)m_LastWinner);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            if (version >= 1)
            {
                m_PetsAllowed = reader.ReadBool();
                m_PetsReturn = reader.ReadBool();
            }
            //version 0
            m_NextEventTime = reader.ReadDateTime();
            m_GameTimeInMinutes = reader.ReadInt();
            m_LastWinner = reader.ReadString();

            m_Scores = new Dictionary<Faction, int>();
            m_Scores.Add(Faction.Find("Lillano"), 0);
            m_Scores.Add(Faction.Find("Pedran"), 0);
            m_Scores.Add(Faction.Find("Arbor"), 0);
            m_Scores.Add(Faction.Find("Calor"), 0);
            m_Scores.Add(Faction.Find("Vermell"), 0);

            m_FlagInfos = new Dictionary<Point3D, int>();
            m_FlagInfos.Add(new Point3D(531, 1860, 26), 1);
            m_FlagInfos.Add(new Point3D(613, 1826, 26), 1);
            m_FlagInfos.Add(new Point3D(683, 1846, 26), 1);
            m_FlagInfos.Add(new Point3D(666, 1939, 26), 1);
            m_FlagInfos.Add(new Point3D(605, 1925, 26), 1);
            m_FlagInfos.Add(new Point3D(658, 1895, 46), 2);
            m_FlagInfos.Add(new Point3D(556, 1884, 46), 2);
            m_FlagInfos.Add(new Point3D(617, 1880, 60), 3);

            // cleanup occurs in Initialize function in case it was a crash mid-game
        }
        public override void OnDelete()
        {
            if (m_StartupTimer != null)
            {
                m_StartupTimer.Stop();
            }
            if (Active)
            {
                EndGame();
            }
            base.OnDelete();
        }
       
        private class StartupTimer : Timer
        {
            private WorldWarsController mController;

            public StartupTimer(WorldWarsController controller)
                : base(TimeSpan.FromMinutes(2.0), TimeSpan.FromMinutes(2.0))
            {
                mController = controller;
            }

            protected override void OnTick()
            {
                base.OnTick();
                mController.StartGameCall();
            }
        }

        private class GameTimer : Timer
        {
            private WorldWarsController mController;
            private TimeSpan mGameLength;
            private DateTime startTime;

            public GameTimer(WorldWarsController controller, TimeSpan gameLength) : base(TimeSpan.FromSeconds(0.0), TimeSpan.FromMinutes(1.0))
            {
                mController = controller;
                mGameLength = gameLength;
                startTime = DateTime.Now;
            }

            protected override void OnTick()
            {
                base.OnTick();
                mController.Update();
                if (startTime + mGameLength < DateTime.Now)
                {
                    mController.GameTimeLeft = TimeSpan.Zero;
                    mController.EndGame();
                }
                else
                {
                    mController.GameTimeLeft = (startTime + mGameLength) - DateTime.Now;
                }
            }
        }
    }

    
}
