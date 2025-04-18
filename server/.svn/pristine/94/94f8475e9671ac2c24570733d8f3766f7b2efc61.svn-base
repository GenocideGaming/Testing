using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server
{
    public class CoreFactory
    {
        public static bool IsTest { get; set; }
        public static void SetTest(bool isTest)
        {
            IsTest = isTest;
            MapFactory.IsTest = isTest;
            SerialFactory.IsTest = isTest;
            WorldFactory.IsTest = isTest;

            if(isTest)
            {
                MapFactory.SetupMaps();
            }
        }
    }
   public class MapFactory
   {
       public static bool IsTest { get; set; }
      
       private static Map[] m_Maps;

       public static void SetupMaps()
       {
           m_Maps = new Map[10];
           for (int i = 0; i < m_Maps.Length; i++)
           {
               m_Maps[i] = new Map(i, i, i, 100, 100, 0, "Fake Map", MapRules.FeluccaRules);
           }
       }

       public static Map[] Maps 
       { 
           get 
           {
               return IsTest ? m_Maps : Map.Maps;
           }
       }
       public static Map Internal
       {
           get { return IsTest ? Maps[7] : Map.Internal; }
       }
       public static Map Felucca
       {
           get { return IsTest ? Maps[0] : Map.Felucca; }
       }
       public static Map Trammel
       {
           get { return IsTest ? Maps[1] : Map.Trammel; }
       }
    
   }

    public class SerialFactory
    {
        public static bool IsTest { get; set; }
        
        private static Serial m_ItemI = Serial.MinusOne;
        private static Serial m_MobileI = Serial.MinusOne;

        public static Serial NewMobile
        {
            get
            {
                if(IsTest)
                {
                    m_MobileI++;
                    return m_MobileI;
                }
                else
                {
                    return Serial.NewMobile;
                }
            }
        }

        public static Serial NewItem
        {
            get
            {
                if(IsTest)
                {
                    m_ItemI++;
                    return m_ItemI;
                }

                return Serial.NewItem;
            }
        }
    }

    public class WorldFactory
    {
        public static bool IsTest { get; set; }

        public static List<Type> m_MobileTypes { get { return IsTest ? new List<Type>() : World.m_MobileTypes; } }
        public static List<Type> m_ItemTypes { get { return IsTest ? new List<Type>() : World.m_ItemTypes; } }
        public static void AddMobile(Mobile m)
        {
            if (IsTest)
            {
                return;
            }
            else
            {
                World.AddMobile(m);
            }
        }
        public static void AddItem(Item item)
        {
            if(IsTest)
            {
                return;
            }
            else
            {
                World.AddItem(item);
            }
        }
        public static void RemoveItem(Item item)
        {
            if(IsTest)
            {
                return;
            }
            else
            {
                World.RemoveItem(item);
            }
        }
        public static Mobile FindMobile(Serial serial)
        {
            return IsTest ? new Mobile() : World.FindMobile(serial);
        }
        public static Item FindItem(Serial serial)
        {
            return IsTest ? new Item(0) : World.FindItem(serial);
        }
    }
}
