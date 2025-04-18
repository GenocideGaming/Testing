using System;
using Server.Scripts.Custom.Citizenship;

namespace Server.Items
{
    public class WorldWarStatue : Item
    {
        public static WorldWarStatue Instance;
        
        private string m_LastWinner;
        [CommandProperty(AccessLevel.GameMaster)]
        public string LastWinner { 
            get { return m_LastWinner; } 
            set 
            {
                if (Commonwealth.Commonwealths != null)
                {
                    foreach (ICommonwealth town in Commonwealth.Commonwealths)
                    {
                        if (town.Definition.TownName == m_LastWinner)
                        {
                            m_LastWinner = null;// make sure it takes away the bonus
                            town.ScaleBonuses(); 
                        }
                    }
                }
                m_LastWinner = value;
                if (Commonwealth.Commonwealths != null) {
                    foreach (ICommonwealth town in Commonwealth.Commonwealths)
                    {
                        if (town.Definition.TownName == m_LastWinner)
                        {
                            town.OnWorldWarWin();
                        }
                    }
                }
            }
        }

        [Constructable]
        public WorldWarStatue()
            : base(4822)
        {
            Movable = false;
            Stackable = false;
            if (Instance != null)
            {
                Instance.Delete();
            }
            Instance = this;
        }

        public WorldWarStatue(Serial serial)
            : base(serial)
        {

        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
            writer.Write((string)m_LastWinner);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            if (version >= 0) 
            {
                m_LastWinner = reader.ReadString();
            }
        }
    }
}
