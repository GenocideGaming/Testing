using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Server;
using Server.Network;
using Server.Commands;
using System.IO;

namespace Server.Misc
{
	public class IPLimiter
	{
        public const string ExemptionFileName = "IPExemptions.txt";
        public static bool Enabled = true;
		public static bool SocketBlock = true; // true to block at connection, false to block at login request

		public static int MaxAddresses = 2;
		
		public static List<IPAddress> Exemptions = new List<IPAddress>();	//For hosting services where there are cases where IPs can be proxied

        public static void Initialize()
        {
            LoadExemptions(null);
            // add command listener
            CommandSystem.Register("WhiteList", AccessLevel.GameMaster, new CommandEventHandler(WhiteList_Command));
        }

        public static void LoadExemptions(Mobile gm)
        {
            Exemptions.Clear();
            // open IPExemptions.txt, fill in Exemptions
            string line;

            // Read the file and display it line by line.
            try
            {
                System.IO.StreamReader file =
                   new System.IO.StreamReader(ExemptionFileName);
                while ((line = file.ReadLine()) != null)
                {
                    string[] args = (line.Trim()).Split();
                    if (args.Length < 1 || args[0].StartsWith("#"))
                    {
                        continue;
                    }
                    try
                    {
                        
                        Exemptions.Add(IPAddress.Parse(args[0])); // the rest of the line is ignored
                    }
                    catch (Exception e)
                    {
                        using (StreamWriter writer = new StreamWriter("ERROR-IPLimiter.txt", true))
                        {
                            writer.WriteLine("ERROR: " + e.Message);
                        }
                    }
                }
                file.Close();
                if (gm != null) { gm.SendMessage("Successfully loaded IP addressess"); }
            }
            catch (Exception e)
            {
                using (StreamWriter writer = new StreamWriter("ERROR-IPLimiter.txt", true))
                {
                    writer.WriteLine("ERROR: " + e.Message);
                }
                if (gm != null) { gm.SendMessage("Error: " + e.Message); }
            }
        }

        public static void WhiteList_Command(CommandEventArgs e)
        {
            string[] args = e.Arguments;
            string error_msg = null;
            if (args != null)
            {
                if (args.Length > 0)
                {
                    if (args[0].ToLower() == "reload")
                    {
                        LoadExemptions(e.Mobile);
                    }
                    else if (args[0].ToLower() == "add" && args.Length > 1)
                    {
                        try
                        {
                            IPAddress.Parse(args[1]);
                            string ipEntry = null;
                            // if it was a valid IP address it will continue from here
                            using (StreamWriter writer = new StreamWriter(ExemptionFileName, true))
                            {
                                ipEntry = args[1] + " \t#";
                                for (int i = 2; i < args.Length; i++)
                                {
                                    ipEntry += args[i] + " ";
                                }
                                writer.WriteLine(ipEntry);
                            }
                            e.Mobile.SendMessage("Successfully added entry to " + ExemptionFileName + ": " + ipEntry);
                        }
                        catch (Exception exception)
                        {
                            error_msg = "IP address parse error: " + exception.Message;
                        }
                    }
                    else
                    {
                        error_msg = "Must supply arguments: either 'reload' or 'add [IP] [description]', e.g. [whitelist add 121.4.5.165 those two fat dudes";
                    }
                }
                else
                {
                    error_msg = "Must supply arguments: either 'reload' or 'add [IP] [description]', e.g. [whitelist add 121.4.5.165 those two fat dudes";
                }
            }
            if (error_msg != null)
            {
                e.Mobile.SendMessage(error_msg);
            }
        }

		public static bool IsExempt( IPAddress ip )
		{
            foreach (IPAddress exemptIP in Exemptions)
			{
                if (ip.Equals(exemptIP))
					return true;
			}

			return false;
		}

		public static bool Verify( IPAddress ourAddress )
		{
			if ( !Enabled || IsExempt( ourAddress ) )
				return true;

			List<NetState> netStates = NetState.Instances;

			int count = 0;

			for ( int i = 0; i < netStates.Count; ++i )
			{
				NetState compState = netStates[i];

				if ( ourAddress.Equals( compState.Address ) )
				{
					++count;

					if ( count >= MaxAddresses )
						return false;
				}
			}

			return true;
		}
	}
}