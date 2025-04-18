using System;
using System.Collections.Generic;
using System.Text;
using Server.Gumps;
using Server.Mobiles;

namespace Server.Scripts.Custom.Citizenship
{
    public class CommonwealthStone : Item
    {
        private ICommonwealth mCommonwealth;

        public ICommonwealth MyCommonwealth { get { return mCommonwealth; } set { mCommonwealth = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public String CapturedHeroes
        {
            get
            {
                if (mCommonwealth == null || mCommonwealth.CapturedHeroes == null) return null;
                string output = "";
                foreach (TownHero captive in mCommonwealth.CapturedHeroes)
                {
                    if (captive.OriginalOwner != null && captive.OriginalOwner.TownRegion != null)
                    {
                        output += captive.OriginalOwner.TownRegion.Name + ", ";
                    }
                    else
                    {
                        output += "ERROR, ";
                    }
                }
                return output;
            }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public Boolean CanAssignCommander
        {
            get
            {
                if (mCommonwealth == null) { return false; }
                return mCommonwealth.CanAssignCommander;
            }
            set
            {
                if (mCommonwealth == null) { return; }
                mCommonwealth.CanAssignCommander = value;
            }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Commander
        {
            get
            {
                if (mCommonwealth == null && mCommonwealth.Militia != null) { return null; }
                return mCommonwealth.Militia.Commander;
            }
            set
            {
                if (mCommonwealth == null && mCommonwealth.Militia != null) { return; }
                mCommonwealth.Militia.Commander = value;
            }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Minister
        {
            get
            {
                if (mCommonwealth == null) { return null; }
                return mCommonwealth.Minister;
            }
            set
            {
                if (mCommonwealth == null) { return; }
                mCommonwealth.Minister = value;
            }
        }


        [Constructable]
        public CommonwealthStone(ICommonwealth commonwealth)
            : base(0xEDC)
        {
            mCommonwealth = commonwealth;
            Movable = false;

        }
        public CommonwealthStone(Serial serial)
            : base(serial)
        {
        }
        public override void OnDoubleClick(Mobile from)
        {

            if (mCommonwealth == null)
            {
                Console.WriteLine("MyCommonwealth on stone is null.");
                return;
            }

            from.SendGump(new TownInfoGump(from, mCommonwealth));
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); //version
            Commonwealth.WriteReference(writer, mCommonwealth);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            mCommonwealth = Commonwealth.ReadReference(reader);
        }
    }
}
