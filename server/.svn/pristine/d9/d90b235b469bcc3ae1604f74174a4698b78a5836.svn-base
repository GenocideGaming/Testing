using System;
using System.Collections.Generic;
using System.Text;
using Server.Commands;

namespace Server.Scripts.Custom.Citizenship
{
    class CommonwealthGenerator
    {

        public static void Initialize()
        {
            CommandSystem.Register("GenerateTownships", AccessLevel.Administrator, new CommandEventHandler(OnCommand_GenerateCommonwealths));            
        }
        public static void OnCommand_GenerateCommonwealths(CommandEventArgs e)
        {

            new CommonwealthPersistence();
            List<ICommonwealth> commonwealths = Commonwealth.Commonwealths;

            foreach (Commonwealth cw in commonwealths)
            {
                Generate(cw, e.Mobile);
            }

            e.Mobile.SendMessage("Finished generating commonwealths");
        }
        public static void Generate(Commonwealth cw, Mobile from)
        {
            //TODO add things that need to be generated when the system is initialized
            //Test generate a commonwealth stone right where the gm stands
            CommonwealthStone cwStone = new CommonwealthStone(cw);
            cwStone.MoveToWorld(cw.TownStoneLocation, Map.Felucca);
            Console.WriteLine("Generated a township stone at loc: " + cw.TownStoneLocation);


        }
    }
}
