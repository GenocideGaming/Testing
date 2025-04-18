using System;
using System.Collections.Generic;
using System.Text;
using Server.Commands;
using Server.Mobiles;

namespace Server.Scripts.Custom.Citizenship
{
    public class CommonwealthPersistence : Item
    {
        private int mCommonwealthCount;

        public override string DefaultName
        {
            get { return "MyCommonwealth Persistance - Internal"; }
        }

        public CommonwealthPersistence()
            : base(1)
        {
            Movable = false;
        }

        public CommonwealthPersistence(Serial serial)
            : base(serial)
        {
        }
        private enum PersistedType
        {
            Terminator,
            Commonwealth
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); //version

            List<ICommonwealth> commonwealths = Commonwealth.Commonwealths;

            mCommonwealthCount = commonwealths.Count;
            writer.WriteEncodedInt(mCommonwealthCount);

            for (int i = 0; i < commonwealths.Count; i++)
            {
                commonwealths[i].State.Serialize(writer);
             
            }

        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            mCommonwealthCount = reader.ReadEncodedInt();

            for (int i = 0; i < mCommonwealthCount; i++)
            {
                new CommonwealthState(reader);
            }
         
        }

        public static void Initialize()
        {
            CommandSystem.Register("DeleteCommonwealthPersistence", AccessLevel.Administrator, new CommandEventHandler(OnCommand_DeleteCommonwealthPersistence));
        }
        public static void OnCommand_DeleteCommonwealthPersistence(CommandEventArgs e)
        {
            Item itemToRemove = null;
            foreach (Item item in World.Items.Values)
            {
                if (item.Parent != null || item.Map != Map.Internal)
                    continue;

                Type type = item.GetType();

                if (type == typeof(CommonwealthPersistence))
                    itemToRemove = item;
            }

            if(itemToRemove != null)
                itemToRemove.Delete();

            //clear all of the mobile's citizenship states
            foreach (Mobile mobile in World.Mobiles.Values)
            {
                if (mobile is PlayerMobile)
                {
                    ((PlayerMobile)mobile).CitizenshipPlayerState = null;
                    mobile.InvalidateProperties();
                }
            }

            //clear all of the commonwealth states
            foreach (Commonwealth commonwealth in Commonwealth.Commonwealths)
            {
                commonwealth.State = null;
                commonwealth.State = new CommonwealthState(commonwealth);
            }
            e.Mobile.SendMessage("MyCommonwealth persistence object was deleted, you can now recreated with [GenerateTownships");
        }
    }
}
