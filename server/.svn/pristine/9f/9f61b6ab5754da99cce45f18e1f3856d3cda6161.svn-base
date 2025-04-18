using System;
using System.Collections.Generic;
using System.Text;
using Server.Commands;
using System.IO;
using Server.Mobiles;

namespace Server.Custom
{ 
    class DungeonGenerator
    {
        public const string DunGenConfigDir = "DunGenConfigs";
        public const string DunGenConfigDirMobiles = "DunGenConfigs\\mobiles\\";
        public const string DunGenOutputDir = "DunGen"; // inside xmlspawner's base directory
        public const string BaseXMLFileNameHead = "dunGenBaseHead.xml";
        public const string BaseXMLFileNameTail = "dunGenBaseTail.xml";

        //The boundaries of the dungeon
            private static int minX;
            private static int minY;
            private static int maxX;
            private static int maxY;

        // TileConfig Defaults
            private static double currentonewayprobability;
            private static double currenttwowayprobability;
            private static double currentthreewayprobability;
            private static double currentfourwayprobability;

            // NOTE: These are CUMULATIVE probabilities, and therefore the order that the if (roll < x) statements are performed IS IMPORTANT!
            private static double oneWayForceOneWayProbability;
            private static double oneWayForceTwoWayProbability;
            private static double oneWayForceThreeWayProbability;
            // don't need a FourWayProbability b/c that's what's left over
            private static double twoWayForceTwoWayProbability;
            private static double twoWayForceThreeWayProbability;
            private static double threeWayForceThreeWayProbability;

            // weights on particular directions (for twoway tiles)
            private static double currentstraightprobability;
            private static double currentturnprobability;
            private static double straightCumulativeProbability;
            // don't need a turn probability b/c it's what's left over

        //Define constants and permutation start values

        private static int currentTileSize;
        private static int currentBitflagsWidth;
        private static int currentBitflagsHeight;

        private static List<TileEntry>[] possibleTiles;
        private static double[] totalWeightings; // used for dice rolls

        // bits go like this:
        // NWES (int)
        // 0000 0
        // 0001 1 = S
        // 0010 2 = E
        // 0011 3 = ES
        // 0100 4 = W
        // 0101 5 = WS
        // 0110 6 = WE
        // 0111 7 = WES
        // 1000 8 = N
        // 1001 9 = NS
        // 1010 10 = NE
        // 1011 11 = NES
        // 1100 12 = NW
        // 1101 13 = NWS
        // 1110 14 = NWE
        // 1111 15 = NWES

        private const int NORTH = 8;
        private const int WEST = 4;
        private const int EAST = 2;
        private const int SOUTH = 1;
        private static int[,] bitflags;

        //Evil Filler Variables
        private static int[,] mobileSpawnLevel;
        private static string spawnLevelStrategy;
        private static string mobileConfigFile;
        private static double mobilePromoteChance;
        private static double mobileDemoteChance;
        private static double mobileKeepRankChance;
        private static double mobileSkipSpawnChance;
        private static int minSpawnLevel;
        private static int maxSpawnLevel;
        private static int maxBosses;
        private static int currBosses;
        private static int spawnOffsetA;
        private static int spawnOffsetB;
        private static int spawnHomeRange;


        public static void Initialize()
        {
            CommandSystem.Register("DunGen", AccessLevel.GameMaster, new CommandEventHandler(DunGen_Command));
        }
        public static void setDefaults()
        {          
            //The boundaries of the dungeon
            minX = 5140;
            minY = 2856;
            maxX = 5466;
            maxY = 3540;

            // TileConfig Defaults
            currentonewayprobability = 0.5;
            currenttwowayprobability = 1.0;
            currentthreewayprobability = 0.5;
            currentfourwayprobability = 0.25;

            // weights on particular directions (for twoway tiles)
            currentstraightprobability = 1.0;
            currentturnprobability = 1.0;
            // don't need a turn probability b/c it's what's left over

            //Define constants and permutation start values
            currentTileSize = -1;
            currentBitflagsWidth = 30;
            currentBitflagsHeight = 30;
                        
            //Mobile Default config
            mobileConfigFile = DunGenConfigDirMobiles + "drow.xml";

            //normalised between 3 states
            mobilePromoteChance = 1;
            mobileDemoteChance = 0;
            mobileKeepRankChance = 0;

            //value from 0 to 1
            spawnLevelStrategy = "+";
            mobileSkipSpawnChance = 0;
            minSpawnLevel = 1;
            maxSpawnLevel = 5;
            maxBosses = 1;
            currBosses = 0;

            //use default algorithm instead
            spawnOffsetA = 999;
            spawnOffsetB = 999;
            spawnHomeRange = 999;
        }

        public static void DunGen_Command(CommandEventArgs e)
        {
            //Re-initialise variables
            setDefaults();
            Console.WriteLine();
            Console.WriteLine("DunGen Loaded...");

            string[] args = e.Arguments;
            string output_file_contents = "";
            // default response_msg
            string response_msg = "Usage: [dungen <floorConfigFile> <mobileConfigFile> [optional: themeConfigFile]";
            string readfile_status_msg = null;
            if (args != null)
            {
                if (args.Length > 0)
                {
                    output_file_contents = ReadFileString(LocateFile(BaseXMLFileNameHead));
                    response_msg = "";
                    List<string[]> floorConfigFileArgs = ReadFile(LocateFile(args[0]), out readfile_status_msg);
                    response_msg += readfile_status_msg;

                    // how many edges
                    currentonewayprobability = 0.5;
                    currenttwowayprobability = 1.0;
                    currentthreewayprobability = 0.5;
                    currentfourwayprobability = 0.25;

                    // weights on particular directions (for twoway tiles)
                    currentstraightprobability = 1.0;
                    currentturnprobability = 1.0;
                    currentTileSize = -1;

                    possibleTiles = new List<TileEntry>[16];
                    totalWeightings = new double[16]; // used for dice rolls


                    for (int i = 0; i < 16; i++)
                    {
                        possibleTiles[i] = new List<TileEntry>();
                        totalWeightings[i] = 0.0d;
                    }

                    //parse config file
                    foreach (string[] lineArgs in floorConfigFileArgs)
                    {
                        if (lineArgs.Length < 2) continue;
                        try
                        {
                            //process as a bit flag if the first 'word' on a line is an integer
                            int bitflags = int.Parse(lineArgs[0]);
                            if (bitflags < 0 || bitflags > 15)
                            {
                                response_msg += "\nConfig file entry began with number < 0 or > 15, which is not allowed! That entry is ignored.";
                                continue;
                            }
                            string multiFileName = lineArgs[1];
                            double weighting = 1.0;
                            if (lineArgs.Length > 2)
                            {
                                try { weighting = double.Parse(lineArgs[2]); }
                                catch { response_msg += "\nFailed to parse a weighting value. Will use 1.0 instead."; }
                            }
                            possibleTiles[bitflags].Add(new TileEntry(weighting, multiFileName, bitflags));
                            totalWeightings[bitflags] += weighting;
                        }
                        catch
                        {
                            //the first 'word' is not an integer
                            switch (lineArgs[0].ToLower())
                            {
                                case "onewayprobability":
                                    try { currentonewayprobability = double.Parse(lineArgs[1]); }
                                    catch (Exception exc) { response_msg += "\nFailed to process onewayprobability in config file: " + exc.Message; }
                                    break;
                                case "twowayprobability":
                                    try { currenttwowayprobability = double.Parse(lineArgs[1]); }
                                    catch (Exception exc) { response_msg += "\nFailed to process twowayprobability in config file: " + exc.Message; }
                                    break;
                                case "threewayprobability":
                                    try { currentthreewayprobability = double.Parse(lineArgs[1]); }
                                    catch (Exception exc) { response_msg += "\nFailed to process threewayprobability in config file: " + exc.Message; }
                                    break;
                                case "fourwayprobability":
                                    try { currentfourwayprobability = double.Parse(lineArgs[1]); }
                                    catch (Exception exc) { response_msg += "\nFailed to process fourwayprobability in config file: " + exc.Message; }
                                    break;
                                case "turnprobability":
                                    try { currentturnprobability = double.Parse(lineArgs[1]); }
                                    catch (Exception exc) { response_msg += "\nFailed to process turnprobability in config file: " + exc.Message; }
                                    break;
                                case "straightprobability":
                                    try { currentstraightprobability = double.Parse(lineArgs[1]); }
                                    catch (Exception exc) { response_msg += "\nFailed to process straightprobability in config file: " + exc.Message; }
                                    break;
                                case "tilesize":
                                    try { currentTileSize = int.Parse(lineArgs[1]); }
                                    catch (Exception exc) { response_msg += "\nFailed to process tilesize in config file: " + exc.Message; }
                                    break;
                                //Mobile Config
                                //@TODO check if mobileConfigFile exists
                                case "mobileconfigfile":
                                    try { mobileConfigFile = DunGenConfigDirMobiles + lineArgs[1]; }
                                    catch (Exception exc) { response_msg += "\nFailed to process mobileConfigFile in config file: " + exc.Message; }
                                    break;
                                case "spawnlevelstrategy":
                                    try { spawnLevelStrategy = lineArgs[1].ToLower(); }
                                    catch (Exception exc) { response_msg += "\nFailed to process spawnLevelStrategy in config file: " + exc.Message; }
                                    break;
                                case "spawnoffseta":
                                    try { spawnOffsetA = int.Parse(lineArgs[1]); }
                                    catch (Exception exc) { response_msg += "\nFailed to process spawnOffsetA in config file: " + exc.Message; }
                                    break;
                                case "spawnoffsetb":
                                    try { spawnOffsetB = int.Parse(lineArgs[1]); }
                                    catch (Exception exc) { response_msg += "\nFailed to process spawnOffsetB in config file: " + exc.Message; }
                                    break;
                                case "spawnhomerange":
                                    try { spawnHomeRange = int.Parse(lineArgs[1]); }
                                    catch (Exception exc) { response_msg += "\nFailed to process spawnHomeRange in config file: " + exc.Message; }
                                    break;
                                case "mobilepromotechance":
                                    try { mobilePromoteChance = double.Parse(lineArgs[1]); }
                                    catch (Exception exc) { response_msg += "\nFailed to process mobilePromoteChance in config file: " + exc.Message; }
                                                                        break;
                                case "mobiledemotechance":
                                    try { mobileDemoteChance = double.Parse(lineArgs[1]); }
                                    catch (Exception exc) { response_msg += "\nFailed to process mobileDemoteChance in config file: " + exc.Message; }
                                    break;
                                case "mobilekeeprankchance":
                                    try { mobileKeepRankChance = double.Parse(lineArgs[1]); }
                                    catch (Exception exc) { response_msg += "\nFailed to process mobileKeepRankChance in config file: " + exc.Message; }
                                    break;
                                case "mobileskipspawnchance":
                                    try { mobileSkipSpawnChance = double.Parse(lineArgs[1]); }
                                    catch (Exception exc) { response_msg += "\nFailed to process mobileSkipSpawnChance in config file: " + exc.Message; }
                                    break;
                                case "minspawnlevel":
                                    try { minSpawnLevel = int.Parse(lineArgs[1]); }
                                    catch (Exception exc) { response_msg += "\nFailed to process minSpawnLevel in config file: " + exc.Message; }
                                    break;
                                case "maxspawnlevel":
                                    try { maxSpawnLevel = int.Parse(lineArgs[1]); }
                                    catch (Exception exc) { response_msg += "\nFailed to process maxSpawnLevel in config file: " + exc.Message; }
                                    break;
                                case "maxbosses":
                                    try { maxBosses = int.Parse(lineArgs[1]); }
                                    catch (Exception exc) { response_msg += "\nFailed to process maxBosses in config file: " + exc.Message; }
                                    break;
                            }
                        }
                    }

                    if (currentTileSize == -1)
                    {
                        response_msg = "\nNo tilesize parameter was found in the config file! Cannot process dungeon!";
                    }
                    else
                    {

                        if (args.Length > 1)
                        {
                            List<string[]> mobileConfigFileArgs = ReadFile(args[0], out readfile_status_msg);
                            // TODO handle this stuff
                        }
                        // figure out the size of the possible 2-D array:
                        // position in Atria's map for the dungeon
                        

                        int maxWidth = maxX - minX;
                        int maxHeight = maxY - minY;

                        currentBitflagsWidth = maxWidth / currentTileSize;
                        currentBitflagsHeight = maxHeight / currentTileSize;

                        bitflags = new int[currentBitflagsWidth, currentBitflagsHeight];
                        
                        int mapCenterX = minX + (maxX - minX) / 2;
                        int mapCenterY = minY + (maxY - minY) / 2;
                        int centerX = currentBitflagsWidth / 2;
                        int centerY = currentBitflagsHeight / 2;

                        //Normalise Tilefilling Probability
                            double onewayForceNormalizationFactor = currentonewayprobability + currenttwowayprobability + currentthreewayprobability + currentfourwayprobability;
                            double twowayForceNormalizationFactor = currenttwowayprobability + currentthreewayprobability + currentfourwayprobability;
                            double threewayForceNormalizationFactor = currentthreewayprobability + currentfourwayprobability;

                            double turnOrStraightNormalizationFactor = currentturnprobability + currentstraightprobability;

                            straightCumulativeProbability = currentstraightprobability / turnOrStraightNormalizationFactor;

                            // NOTE: These are CUMULATIVE probabilities, and therefore the order that the if (roll < x) statements are performed IS IMPORTANT!
                            oneWayForceOneWayProbability = currentonewayprobability / onewayForceNormalizationFactor;
                            oneWayForceTwoWayProbability = oneWayForceOneWayProbability + currenttwowayprobability / onewayForceNormalizationFactor;
                            oneWayForceThreeWayProbability = oneWayForceTwoWayProbability + currentthreewayprobability / onewayForceNormalizationFactor;
                            // don't need a FourWayProbability b/c that's what's left over

                            twoWayForceTwoWayProbability = currenttwowayprobability / twowayForceNormalizationFactor;
                            twoWayForceThreeWayProbability = twoWayForceTwoWayProbability + currentthreewayprobability / twowayForceNormalizationFactor;

                            threeWayForceThreeWayProbability = currentthreewayprobability / threewayForceNormalizationFactor;
                        

                        //Recursively build square sections from the center outward                      
                                
                            bitflags[centerX, centerY] = 15; // binary = 1111

                            // have to individually call recursiveWalk for each adjacent square
                            // b/c if we pass the centersquare, then it will return at the very first
                            // statement (something put in to avoid reprocessing already finished tiles)
                            // N,E,W,S
                            recursiveWalk(centerX, centerY - 1);
                            recursiveWalk(centerX + 1, centerY);
                            recursiveWalk(centerX - 1, centerY);
                            recursiveWalk(centerX, centerY + 1);
                        
                        //Initialise mobileSpawn variables and normalise probabilites
                            mobileSpawnLevel = new int[currentBitflagsWidth, currentBitflagsHeight];
                            //mobileSpawnLevel[centerX, centerY] = minSpawnLevel;

                            //normalise promotion/demotion chance and assign a roll value for promotion/demotion
                            double mobileSpawnLevelNormalizationFactor = mobilePromoteChance + mobileDemoteChance + mobileKeepRankChance;
                            mobilePromoteChance = mobilePromoteChance / mobileSpawnLevelNormalizationFactor;
                            mobileDemoteChance = mobilePromoteChance + (mobileDemoteChance / mobileSpawnLevelNormalizationFactor);

                        //Recursive N,E,S,W
                            mobWalk(centerX, centerY, -1);                        

                        //Build The XMLspawner Output file
                        int penX;
                        int penY;

                        //default spawnOffset is half the distance between the top left corner and the center of the square
                        //this denotes the spawn radius
                        //int spawnOffset = Convert.ToInt16(Math.Floor(Convert.ToDouble(currentTileSize / 4)));
                        
                        if (spawnOffsetA == 999)
                        {
                            //spawnOffsetA = spawnOffset;
                            spawnOffsetA = 1;
                        }
                        if (spawnOffsetB == 999)
                        {
                            //spawnOffsetB = spawnOffset * 3;
                            spawnOffsetB = currentTileSize - spawnOffsetA;
                        }
                        if (spawnHomeRange == 999)
                        {
                            //Default
                            //home range extends the max wander range 1/4 of the way into the next square
                            //spawnHomeRange = Convert.ToInt16(Math.Floor(Convert.ToDouble(currentTileSize / 2))) + spawnOffset;
                            spawnHomeRange = currentTileSize;
                        }

                        for (int y = 0; y < currentBitflagsHeight; y++)
                        {
                            for (int x = 0; x < currentBitflagsWidth; x++)
                            {
                                if (bitflags[x, y] == 0) continue;
                                penX = (minX + x * currentTileSize);
                                penY = (minY + y * currentTileSize);
                                
                                output_file_contents += ":OBJ=#XY," + penX + "," + penY 
                                                          + ",0 ; *MULTIADDON," + getMultiFileName(bitflags[x, y]) 
                                                          + ":MX=1:SP=1:RT=0:TO=0:CA=0:KL=0:SB=1\r\n"

                                                      + ":OBJ=XmlSpawner/ConfigFile/" + mobileConfigFile + "/loadconfig/true/"
                                                          + "location/(" + (penX + spawnOffsetA) + "," + (penY + spawnOffsetA)
                                                          + ",0)/X1_Y1/(" + (penX + spawnOffsetA) + "," + (penY + spawnOffsetA)
                                                          + ",0)/X2_Y2/(" + (penX + spawnOffsetB) + "," + (penY + spawnOffsetB)

                                                          +",0)/HomeRange/"+spawnHomeRange+"/"
                                                          + "SequentialSpawn/" + mobileSpawnLevel[x, y] 
                                                          //spawn if MaxLevel is spawned OR skipSpawn does not get rolled
                                                          + "/running/" + ((mobileSpawnLevel[x,y]==maxSpawnLevel)||(mobileSkipSpawnChance < Utility.RandomDouble()))
                                                          +":MX=1:SP=1:RT=0:TO=0:CA=0:KL=0:SB=1\r\n";                                
                            }
                        }

                        Console.WriteLine("Bosses:"+currBosses);

                        output_file_contents += ReadFileString(LocateFile(BaseXMLFileNameTail));
                        string outputfilename = WriteOutputFile(output_file_contents);

                        if (args.Length > 1 && outputfilename != null)
                        {
                            if (args[1].ToLower() == "-load")
                            {
                                // pass it to the load command handler, as if it was typed by the GM
                                // hacky true, but it works
                                XmlSpawner.Load_OnCommand(new CommandEventArgs(e.Mobile, "xmlload", outputfilename, new string[] { outputfilename }));
                            }
                            else
                            {
                                e.Mobile.SendMessage("Argument '" + args[1] + "' ignored.  The only other acceptable argument is '-load', which loads the generated xml file.");
                            }
                        }

                        /* // used for debugging to ensure map comes up without errors
                        string test = "";
                        for (int y = 0; y < currentBitflagsHeight; y++)
                        {
                            for (int x = 0; x < currentBitflagsWidth; x++)
                            {
                                test += getStringDirection(bitflags[x,y]) + "\t";
                            }
                            test += "\n";
                        }
                        WriteOutputFile(test);
                         */
                    }
                }
            }
            if (response_msg != null)
            {
                e.Mobile.SendMessage(response_msg);
            }
        }
        public static string LocateFile(string filename)
        {
            bool found = false;

            string dirname = null;

            if (System.IO.Directory.Exists(DunGenConfigDir) == true)
            {
                // get it from the defaults directory if it exists
                dirname = String.Format("{0}/{1}", DunGenConfigDir, filename);
                found = System.IO.File.Exists(dirname) || System.IO.Directory.Exists(dirname);
            }

            if (!found)
            {
                // otherwise just get it from the main installation dir
                dirname = filename;
            }

            return dirname;
        }
        public static List<string[]> ReadFile(string filename, out string response_msg)
        {
            response_msg = "";
            string line;
            List<string[]> output = new List<string[]>();
            // Read the file and display it line by line.
            try
            {
                System.IO.StreamReader file =
                   new System.IO.StreamReader(filename);
                while ((line = file.ReadLine()) != null)
                {
                    string[] args = (line.Trim()).Split();
                    if (args.Length < 1 || args[0].StartsWith("#"))
                    {
                        continue;
                    }
                    try
                    {
                        output.Add(args); 
                    }
                    catch (Exception e)
                    {
                        using (StreamWriter writer = new StreamWriter("ERROR-DungeonGen.txt", true))
                        {
                            writer.WriteLine("ERROR: " + e.Message);
                            response_msg += "ERROR: " + e.Message;
                        }
                    }
                }
                file.Close();
                response_msg += "Successfully loaded file: " + filename;
            }
            catch (Exception e)
            {
                using (StreamWriter writer = new StreamWriter("ERROR-DungeonGen.txt", true))
                {
                    writer.WriteLine("ERROR: " + e.Message);
                    response_msg += "ERROR: " + e.Message;
                }
            }
            return output;
        }
        public static string ReadFileString(string filename)
        {
            string output = "";
            try
            {
                System.IO.StreamReader file =
                   new System.IO.StreamReader(filename);
                output = file.ReadToEnd();
                file.Close();
            }
            catch (Exception e)
            {
                using (StreamWriter writer = new StreamWriter("ERROR-DungeonGen.txt", true))
                {
                    writer.WriteLine("ERROR: " + e.Message);
                }
            }
            return output;
        }
        private static void recursiveWalk(int x, int y)
        {
            if (bitflags[x, y] != 0) return; // in case this square was handled through another branch walk
            int possibleBranches = 0;
            int forcedBranches = 0;
            
            if (y - 1 >= 0) // looking north
            {
                if (bitflags[x, y - 1] == 0) 
                {
                    // add north as a possible branch
                    possibleBranches += NORTH;
                }
                else if ((bitflags[x, y - 1] & SOUTH) > 0) // tile to the north has south flag
                {
                    // add north as a necessary branch
                    forcedBranches += NORTH;
                } // else it can't connect there
                
            }
            if (x + 1 < currentBitflagsWidth) // looking east
            {
                if (bitflags[x + 1, y] == 0) 
                {
                    // add east as a possible branch
                    possibleBranches += EAST;
                }
                else if ((bitflags[x + 1, y] & WEST) > 0) // tile to the east has west flag
                {
                    // add east as a necessary branch
                    forcedBranches += EAST;
                } // else it can't connect there
                            }
            if (y + 1 < currentBitflagsHeight) // looking south
            {
                if (bitflags[x, y + 1] == 0)
                {
                    // add south as a possible branch
                    possibleBranches += SOUTH;
                }
                else if ((bitflags[x, y + 1] & NORTH) > 0) // tile to the south has north flag
                {
                    // add south as a necessary branch
                    forcedBranches += SOUTH;
                } // else it can't connect there

            }
            if (x - 1 >= 0) // looking west
            {
                if (bitflags[x - 1, y] == 0)
                {
                    // add west as a possible branch
                    possibleBranches += WEST;
                }
                else if ((bitflags[x - 1, y] & EAST) > 0) // tile to the west has east flag
                {
                    // add west as a necessary branch
                    forcedBranches += WEST;
                } // else it can't connect there
            }
                
            //debug += "force " + getStringDirection(forcedBranches) + " | possible " + getStringDirection(possibleBranches) + " | "
            int forceNorth = (forcedBranches & NORTH) >> 3;
            int forceWest = (forcedBranches & WEST) >> 2;
            int forceEast = (forcedBranches & EAST) >> 1;
            int forceSouth = (forcedBranches & SOUTH);
            int possibleNorth = (possibleBranches & NORTH) >> 3;
            int possibleWest = (possibleBranches & WEST) >> 2;
            int possibleEast = (possibleBranches & EAST) >> 1;
            int possibleSouth = (possibleBranches & SOUTH);
            
            // assign the flags for this tile
            int newBitFlagValueForThisTile = forcedBranches; // start off with the forced branches, then add in the possible ones
            int numForcedDirections = forceNorth + forceEast + forceWest + forceSouth;
            int numPossibleDirections = possibleNorth + possibleEast + possibleWest + possibleSouth;

            
            double roll = Utility.RandomDouble();

            if (possibleBranches > 0) 
                switch (numForcedDirections)
            {
                case 1:
                    if (roll < oneWayForceOneWayProbability) {  } // don't do anything, the forced flag takes care of it
                    else if (roll < oneWayForceTwoWayProbability) 
                    { 
                        newBitFlagValueForThisTile += (numPossibleDirections <= 1) ? possibleBranches : getTwoWayFlag(possibleBranches,forcedBranches);
                    }
                    else if (roll < oneWayForceThreeWayProbability) 
                    {
                        newBitFlagValueForThisTile += numPossibleDirections <= 2 ? possibleBranches : getThreeWayFlags(possibleBranches, forcedBranches, numForcedDirections);     
                    }
                    else { newBitFlagValueForThisTile += possibleBranches; } //four way (or whatever is possible)
                    break;
                case 2:
                    if (roll < twoWayForceTwoWayProbability) { } // don't do anything, the forced flags takes care of it
                    else if (roll < twoWayForceThreeWayProbability) 
                    {
                        newBitFlagValueForThisTile += numPossibleDirections <= 1 ? possibleBranches : getThreeWayFlags(possibleBranches, forcedBranches, numForcedDirections);
                    }
                    else { newBitFlagValueForThisTile += possibleBranches; } //four way (or whatever is possible)
                    break;
                case 3:
                    if (roll < threeWayForceThreeWayProbability) { } // don't do anything, the forced flags takes care of it
                    else { newBitFlagValueForThisTile += possibleBranches; } //four way (or whatever is possible)
                    break;
                case 4: // it already must be in all 4 directions--don't do anything
                    break;
                default:
                    Console.WriteLine("DunGen reached impossible " + numForcedDirections + " forced directions in recursion.");
                    break;

            }
            //debug += "newflags " + getStringDirection(newBitFlagValueForThisTile);
            bitflags[x, y] = newBitFlagValueForThisTile;

            // recursively process nearby tiles that connect to this one
            if ((newBitFlagValueForThisTile & NORTH) > 0) // N
            {
                recursiveWalk(x, y - 1);
            }
            if ((newBitFlagValueForThisTile & WEST) > 0)  // W
            {
                recursiveWalk(x - 1, y);
            }
            if ((newBitFlagValueForThisTile & EAST) > 0)  // E
            {
                recursiveWalk(x + 1, y);
            }
            if ((newBitFlagValueForThisTile & SOUTH) > 0)  // S
            {
                recursiveWalk(x, y + 1);
            }
        }

        private static void mobWalk(int x, int y, int currSpawnLevel)
        {

            if (mobileSpawnLevel[x, y] != 0)
            {
                Console.WriteLine("__Already walked:" + mobileSpawnLevel[x, y] + "at [go " + (minX + x * currentTileSize) + " " + (minY + y * currentTileSize));
                return; // in case this square was handled through another branch walk
            }


            if (currSpawnLevel == -1)
            {
                //first time it has been called
                currSpawnLevel = minSpawnLevel;
            }
            else
            {
                //promote/demote calc
                //reroll for mob promote/demote
                double roll = Utility.RandomDouble();
                if (roll <= mobilePromoteChance)
                {
                    if (currSpawnLevel < maxSpawnLevel)
                    {
                        //Promote the Spawn if Max has not been reached
                        currSpawnLevel++;
                        // Console.WriteLine("____Promoting:" + currSpawnLevel);
                    }
                }
                else if (roll <= mobileDemoteChance)
                {

                    if (currSpawnLevel > minSpawnLevel)
                    {
                        //if demote
                        currSpawnLevel--;
                        // Console.WriteLine("____Demoting:" + currSpawnLevel);
                    }
                }
                else
                {
                    //Keep spawn level the same
                    //Console.WriteLine("____SpawnLevel:" + currSpawnLevel);
                }

                if (currSpawnLevel == maxSpawnLevel)
                {
                    if (currBosses < maxBosses)
                    {
                        Console.WriteLine("BOSS Spawned    [go " + (minX + x * currentTileSize) + " " + (minY + y * currentTileSize));
                        currBosses++;
                    }
                    else
                    {
                        //reduce spawn level, maximum bosses have already been reached
                        //Console.WriteLine("----BOSS Demoted\n");
                        currSpawnLevel--;
                    }
                }
            }
            //set the current square's spawn level
            mobileSpawnLevel[x, y] = currSpawnLevel;

            //Looking North
            if (y - 1 >= 0) // looking north
            {
                if ((bitflags[x, y - 1] & SOUTH) > 0) // tile to the north has south flag
                {
                    if (mobileSpawnLevel[x, y - 1] == 0)
                    {
                        mobWalk(x, y - 1, currSpawnLevel);
                    }
                }
            }
            if (x + 1 < currentBitflagsWidth) // looking east
            {
                   
                if ((bitflags[x + 1, y] & WEST) > 0) // tile to the east has west flag
                {
                    if (mobileSpawnLevel[x + 1, y] == 0)
                    {
                        mobWalk(x + 1, y, currSpawnLevel);
                    }
                }
            }
            if (y + 1 < currentBitflagsHeight) // looking south
            {
                //Looking South
                if ((bitflags[x, y + 1] & NORTH) > 0) // tile to the south has north flag
                {
                    if (mobileSpawnLevel[x, y + 1] == 0)
                    {
                        mobWalk(x, y + 1, currSpawnLevel);
                    }
                }

            }
            if (x - 1 >= 0) // looking west
            {
                //Looking West
                if ((bitflags[x - 1, y] & EAST) > 0) // tile to the west has east flag
                {
                    if (mobileSpawnLevel[x - 1, y] == 0)
                    {
                        mobWalk(x - 1, y, currSpawnLevel);
                    }
                }
            }
        }
        private static string getStringDirection(int bitflags)
        {
            string output = "";
            output += (bitflags & NORTH) > 0 ? "N" : "";
            output += (bitflags & WEST) > 0 ? "W" : "";
            output += (bitflags & EAST) > 0 ? "E" : "";
            output += (bitflags & SOUTH) > 0 ? "S" : "";
            if (output == "NWES")
                return "all";
            return output;
        }
        private static int getTwoWayFlag(int possibleBranches, int forcedBranch) // should only have one forced branch
        {
            double roll = Utility.RandomDouble();
            if (roll < straightCumulativeProbability)
            { // try to go straight if possible
                // figure out which way is "straight" (opposite the forcedbranch)
                switch (forcedBranch)
                {
                    case 1: if ((possibleBranches & NORTH) > 0) return NORTH; break; //forced S, so go N if possible
                    case 2: if ((possibleBranches & WEST) > 0) return WEST; break; //forced E, so go W
                    case 4: if ((possibleBranches & EAST) > 0) return EAST; break; //forced W, so go E
                    case 8: if ((possibleBranches & SOUTH) > 0) return SOUTH; break; //forced N, so go S
                }
                // if we got here, then it means the straight branch wasn't a possible branch, so we need to randomly choose
                // between the other two branches (first finding out if there are even 2 possible branches to choose from)
            }
            // turn by default
            
            int straightbranch = -1;
            switch (forcedBranch)
            {
                case SOUTH: if ((possibleBranches & NORTH) > 0) straightbranch = SOUTH; break; //forced S, so can't go N
                case EAST: if ((possibleBranches & WEST) > 0) straightbranch = WEST; break; //forced E, so can't go W
                case WEST: if ((possibleBranches & EAST) > 0) straightbranch = EAST; break; //forced W, so can't go E
                case NORTH: if ((possibleBranches & SOUTH) > 0) straightbranch = SOUTH; break; //forced N, so can't go S
            }
            List<int> possibleBranchesList = new List<int>();
            if ((possibleBranches & NORTH) > 0 && straightbranch != NORTH) possibleBranchesList.Add(NORTH);
            if ((possibleBranches & WEST) > 0 && straightbranch != WEST) possibleBranchesList.Add(WEST);
            if ((possibleBranches & EAST) > 0 && straightbranch != EAST) possibleBranchesList.Add(EAST);
            if ((possibleBranches & SOUTH) > 0 && straightbranch != SOUTH) possibleBranchesList.Add(SOUTH);

            return possibleBranchesList[Utility.Random(possibleBranchesList.Count)];
        }
        private static int getThreeWayFlags(int possibleBranches, int forcedBranch, int numForcedBranches)
        {            
            if (numForcedBranches == 1) // have to pick between the 3 possible branches
            {
                List<int> possibilities = new List<int>();
                if ((possibleBranches & 8) > 0) possibilities.Add(8);
                if ((possibleBranches & 4) > 0) possibilities.Add(4);
                if ((possibleBranches & 2) > 0) possibilities.Add(2);
                if ((possibleBranches & 1) > 0) possibilities.Add(1);
                // should only have 3 possibilities
                int excluded = possibilities[Utility.RandomMinMax(0, 2)];
                return possibleBranches - excluded;
            }
            else // numForcedBranches == 2
            {
                List<int> possibilities = new List<int>();
                if ((possibleBranches & 8) > 0) possibilities.Add(8);
                if ((possibleBranches & 4) > 0) possibilities.Add(4);
                if ((possibleBranches & 2) > 0) possibilities.Add(2);
                if ((possibleBranches & 1) > 0) possibilities.Add(1);
                // should only have 2 possibilities
                int excluded = possibilities[Utility.RandomMinMax(0, 1)];
                return possibleBranches - excluded;
            }
        }
        private static string WriteOutputFile(string contents)
        {
            try
            {
                string newFileName = "Generated" + DateTime.Now.ToString() + ".xml"; //12/9/2012 2:02:33 AM
                
                newFileName = newFileName.Replace('/', '-');
                newFileName = XmlSpawner.XmlSpawnDir + "/" + DunGenOutputDir + "/" + newFileName; 
                newFileName = newFileName.Replace(':', ' ');
                using (StreamWriter writer = new StreamWriter(newFileName, true))
                {
                    writer.Write(contents);
                }
                return newFileName;
            }
            catch (Exception e)
            {
                using (StreamWriter writer = new StreamWriter("ERROR-DungeonGen.txt", true))
                {
                    writer.WriteLine("ERROR: " + e.Message);
                }
            }
            return null;
        }
        private static string getMultiFileName(int bitflags)
        {
            List<TileEntry> availableMultis = possibleTiles[bitflags];
            if (availableMultis.Count == 0) 
            { 
                Console.WriteLine("getMultiFileName error: no Tiles in the " + getStringDirection(bitflags) + " directions!");
                return "ERROR-NO TILES AVAILABLE";
            }
            double roll = Utility.RandomDouble();
            double cumulativeWeighting = 0.0d;
            foreach (TileEntry tile in availableMultis)
            {
                cumulativeWeighting += tile.weighting / totalWeightings[bitflags];
                if (roll < cumulativeWeighting)
                {
                    return tile.multiFile;
                }
            }
            Console.WriteLine("getMultiFileName error: did not choose any tile!  Should always have at least one tile!");
            return "ERROR-NO TILE CHOSEN!";
        }

        private class TileEntry
        {
            public double weighting = 1.0;
            public string multiFile = "";
            public int bitflags = 0;
            public int spawnlevel = 1;

            //deprecated
            public TileEntry(double weighting, string multiFile, int bitflags)
            {
                this.weighting = weighting;
                this.multiFile = multiFile;
                this.bitflags = bitflags;
            }

        }
    }
}
