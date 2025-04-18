using System;
using System.Collections.Generic;
using System.Text;
using Server.Accounting;
using Server.Gumps;
using Server.Mobiles;
using System.Text.RegularExpressions;

namespace Server.Scripts.Custom
{
    public static class UniqueNameChecker
    {
        //returns true if the name can be used
        public static bool CheckName(Mobile from, string name)
        {
            if (name == string.Empty)
                return false;
            if (name.Length >= 16)
                return false;
            if (!Regex.IsMatch(name, "^['-.abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ ]+$"))
                return false;

            bool foundConflict = false;
            foreach (IAccount account in Accounts.GetAccounts())
            {
                for (int j = 0; j < account.Length; j++)
                {
                    if (account[j] != null && account[j] != from && name.Equals(account[j].Name, StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (from.Frozen)
                            from.SendGump(new PlayerRenameGump(from));
                        else
                            FreezePlayerAndSendGump(from);

                        foundConflict = true;
                        break;
                    }
                }
                if (foundConflict)
                    break;
            }
          
            if (!foundConflict)
            {
                PlayerMobile pm = from as PlayerMobile;
                if (pm == null) { return true; }

                if (pm.HasBadName)
                {
                    from.Frozen = false;
                    pm.HasBadName = false;
                }
                from.Name = name;
                return true;
            }
            else
                return false;
        }

        public static void FreezePlayerAndSendGump(Mobile from)
        {
            PlayerMobile pm = from as PlayerMobile;
            if (pm == null) { return; }

            from.Freeze(TimeSpan.FromHours(72));
            ((PlayerMobile)from).HasBadName = true;
            from.SendGump(new PlayerRenameGump(from));
        }

    }
}
