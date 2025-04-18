using System;
using System.Collections.Generic;

namespace Server.Scripts.Custom.AntiRail
{
    public class ItemSet
    {
        List<int> items;

        public ItemSet()
        {
            items = new List<int>();
        }

        public ItemSet(string name)
        {
            switch (name)
            {
                case "bottles": 
                    items = GenerateBottles(); break;
                case "posts":
                    items = GeneratePosts(); break;
                case "cookware":
                    items = GenerateCookware(); break;
                case "lanterns":
                    items = GenerateLanterns(); break;
                case "chairs":
                    items = GenerateChairs(); break;
                case "signs":
                    items = GenerateSigns(); break;
                case "flowers":
                    items = GenerateFlowers(); break;
                case "wands":
                    items = GenerateWands(); break;
                case "jars":
                    items = GenerateJars(); break;
                case "scrolls":
                    items = GenerateScrolls();
                    break;
                case "instruments":
                    items = GenerateInstruments(); break;
                case "potions":
                    items = GeneratePotions(); break;
                case "axes":
                    items = GenerateAxes(); break;
                case "gravestones":
                    items = GenerateGravestones(); break;
                case "helmets":
                    items = GenerateHelmets(); break;
                case "bowls":
                    items = GenerateBowls(); break;
                case "hats":
                    items = GenerateHats(); break;
                case "boards":
                    items = GenerateBoards(); break;
                case "ingots":
                    items = GenerateIngots(); break;
                case "shields":
                    items = GenerateShields(); break;
                case "cactii":
                    items = GenerateCactii(); break;
                case "bolts":
                    items = GenerateBolts(); break;
                case "decorativeshields":
                    items = GenerateDecorativeShields(); break;
                case "skullcandles":
                    items = GenerateSkullCandles(); break;
                case "headtrophies":
                    items = GenerateHeadTropies(); break;
                default:
                    throw new ArgumentException("An invalid ItemSet name was passed to the ItemSet constructor()");
            }
        }

        private List<int> GenerateHeadTropies()
        {
            List<int> returnMe = new List<int>();

            returnMe.Add(0x1E60);
            returnMe.Add(0x1E67);
            returnMe.Add(0x1E61);
            returnMe.Add(0x1E68);
            returnMe.Add(0x1E65);
            returnMe.Add(0x1E6C);
            returnMe.Add(0x1E66);
            returnMe.Add(0x1E6D);


            return returnMe;
        }

        private List<int> GenerateSkullCandles()
        {
            List<int> returnMe = new List<int>();

            for (int i = 0x1853; i <= 0x185A; i++)
                returnMe.Add(i);

            return returnMe;
        }

        private List<int> GenerateDecorativeShields()
        {
            List<int> returnMe = new List<int>();

            for (int i = 0x156C; i <= 0x1581; i++)
                returnMe.Add(i);

            return returnMe;
        }

        private List<int> GenerateBolts()
        {
            List<int> returnMe = new List<int>();

            for (int i = 0x0F95; i <= 0x0F9C; i++)
                returnMe.Add(i);

            return returnMe;
        }

        private List<int> GenerateCactii()
        {
            List<int> returnMe = new List<int>();

            for (int i = 0x0D25; i <= 0x0D27; i++)
                returnMe.Add(i);

            returnMe.Add(0x1E0F);
            returnMe.Add(0x1E10);
            returnMe.Add(0x1E13);
            returnMe.Add(0x1E14);

            return returnMe;
        }

        private List<int> GenerateShields()
        {
            List<int> returnMe = new List<int>();

            for (int i = 0x1B72; i <= 0x1B7B; i++)
                returnMe.Add(i);

            return returnMe;
        }

        private List<int> GenerateIngots()
        {
            List<int> returnMe = new List<int>();

            for (int i = 0x1BEF; i <= 0x1BF4; i++)
                returnMe.Add(i);

            return returnMe;
        }

        private List<int> GenerateBoards()
        {
            List<int> returnMe = new List<int>();

            for (int i = 0x1BD7; i <= 0x1BDC; i++)
                returnMe.Add(i);

            return returnMe;
        }

        private List<int> GenerateHats()
        {
            List<int> returnMe = new List<int>();

            for (int i = 0x1713; i <= 0x171C; i++)
                returnMe.Add(i);

            return returnMe;
        }

        private List<int> GenerateBowls()
        {
            List<int> returnMe = new List<int>();

            for (int i = 0x15F8; i <= 0x1602; i++)
                returnMe.Add(i);

            return returnMe;
        }

        private List<int> GenerateHelmets()
        {
            List<int> returnMe = new List<int>();

            returnMe.Add(0x1408);
            returnMe.Add(0x1409);
            returnMe.Add(0x140C);
            returnMe.Add(0x140D);
            returnMe.Add(0x140E);
            returnMe.Add(0x140F);
            returnMe.Add(0x1412);

            return returnMe;
        }

        private List<int> GenerateGravestones()
        {
            List<int> returnMe = new List<int>();

            for (int i = 0x1165; i <= 0x1182; i++)
                returnMe.Add(i);

            return returnMe;
        }

        private List<int> GenerateAxes()
        {
            List<int> returnMe = new List<int>();

            for (int i = 0x0F43; i <= 0x0F4C; i++)
                returnMe.Add(i);

            return returnMe;
        }

        private List<int> GeneratePotions()
        {
            List<int> returnMe = new List<int>();

            for (int i = 0x0F06; i <= 0x0F0E; i++)
                returnMe.Add(i);

            return returnMe;
        }

        private List<int> GenerateInstruments()
        {
            List<int> returnMe = new List<int>();

            returnMe.Add(0x0EB3);
            returnMe.Add(0x0EB4);
            returnMe.Add(0x0EB2);
            returnMe.Add(0x0E9C);
            returnMe.Add(0x0E9E);

            return returnMe;
        }

        private List<int> GenerateScrolls()
        {
            List<int> returnMe = new List<int>();


            for (int i = 0x0E34; i <= 0x0E3A; i++)
                returnMe.Add(i);

            for (int i = 0x0EF3; i <= 0x0EF9; i++)
                returnMe.Add(i);

            return returnMe;
        }

        private List<int> GenerateJars()
        {
            List<int> returnMe = new List<int>();

            for (int i = 0x0E44; i <= 0x0E4F; i++)
                returnMe.Add(i);

            return returnMe;
        }

        private List<int> GenerateWands()
        {
            List<int> returnMe = new List<int>();

            returnMe.Add(0x0DF2);
            returnMe.Add(0x0DF3);
            returnMe.Add(0x0DF4);
            returnMe.Add(0x0DF5);

            return returnMe;
        }

        private List<int> GenerateFlowers()
        {
            List<int> returnMe = new List<int>();

            returnMe.Add(0x0C83);
            returnMe.Add(0x0C84);
            returnMe.Add(0x0C85);
            returnMe.Add(0x0C86);
            returnMe.Add(0x0C88);
            returnMe.Add(0x0C89);
            returnMe.Add(0x0C8A);
            returnMe.Add(0x0C8B);
            returnMe.Add(0x0C8C);
            returnMe.Add(0x0CC1);

            return returnMe;
        }

        private List<int> GenerateSigns()
        {
            List<int> returnMe = new List<int>();

            returnMe.Add(0x0B95);
            returnMe.Add(0x0B96);

            for (int i=0x0BA3; i<= 0x0C0E; i++)
                returnMe.Add(i);

            return returnMe;
        }

        private List<int> GenerateChairs()
        {
            List<int> returnMe = new List<int>();

            for (int i = 0x0B4E; i <= 0x0B5D; i++)
                returnMe.Add(i);

            return returnMe;
        }

        private List<int> GenerateLanterns()
        {
            List<int> returnMe = new List<int>();

            returnMe.Add(0x0A15);
            returnMe.Add(0x0A16);
            returnMe.Add(0x0A17);
            returnMe.Add(0x0A18);
            returnMe.Add(0x0A1A);
            returnMe.Add(0x0A1B);
            returnMe.Add(0x0A1B);
            returnMe.Add(0x0A1B);
            returnMe.Add(0x0A1C);
            returnMe.Add(0x0A1D);

            return returnMe;
        }

        private List<int> GenerateCookware()
        {
            List<int> returnMe = new List<int>();

            for (int i = 0x09DC; i <= 0x09E6; i++)
                returnMe.Add(i);

            return returnMe;
        }

        private List<int> GeneratePosts()
        {
            List<int> returnMe = new List<int>();

            returnMe.Add(0x03A5);
            returnMe.Add(0x03AA);
            returnMe.Add(0x03AB);
            returnMe.Add(0x03AC);
            returnMe.Add(0x03AD);
            returnMe.Add(0x03AE);
            returnMe.Add(0x03AF);
            returnMe.Add(0x03B0);
        
            return returnMe;
        }

        private List<int> GenerateBottles()
        {
            List<int> returnMe = new List<int>();

            returnMe.Add(0x099B);
            returnMe.Add(0x099C);
            returnMe.Add(0x099D);
            returnMe.Add(0x099E);
            returnMe.Add(0x099F);
            returnMe.Add(0x09A0);
            returnMe.Add(0x09A1);
            returnMe.Add(0x09A2);
            returnMe.Add(0x09C4);
            returnMe.Add(0x09C5);
            returnMe.Add(0x09C6);
            returnMe.Add(0x09C7);

            return returnMe;

        }

        public void Add(int itemID) { items.Add(itemID); }

        public Stack<int> RandomSelection(int count)
        {
            Stack<int> selection = new Stack<int>();

            for (int i = 0; i < count; i++)
            {
                selection.Push(RandomItem());
            }

            return selection;
        }

        public int RandomItem()
        {
            int index = Utility.Random(items.Count);
            return items[index];
        }
    }
}
