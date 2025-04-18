using System;
using System.Collections.Generic;
using Server.Misc;
using Server.Network;
using Server.Mobiles;
using Server.Scripts;
using Server.Scripts.Custom.VorshunWarsEquipment;

namespace Server.Gumps
{
    public class SetNameEntryGump : Gump
    {
        Dictionary<Layer, int> currentEquipment;
        int savetoSlot;

        public SetNameEntryGump(Dictionary<Layer, int> equipment, int slot, string defaultName = "")
            : base(0, 0)
        {
            currentEquipment = equipment;
            savetoSlot = slot;

            this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;

			AddPage(0);
			AddBackground(245, 217, 328, 119, 3500);
			AddLabel(268, 237, 0, @"What do you wish to name this equipment set?");
			AddBackground(266, 265, 287, 23, 9300);
			AddTextEntry(277, 266, 265, 20, 0, 0, defaultName);
			AddButton(483, 296, 247, 248, 1, GumpButtonType.Reply, 0);
        }

        

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            switch(info.ButtonID)
            {
                case 0:
				{
                    if (from.HasGump(typeof(ChooseEquipmentGump)))
                        from.CloseGump(typeof(ChooseEquipmentGump));

                    from.SendGump(new ChooseEquipmentGump(from, currentEquipment));
					break;
				}
                case 1:
                {
                    string name;
                    if (info.TextEntries[0].Text.Length > 25)
                        name = info.TextEntries[0].Text.Substring(0, 25);
                    else
                        name = info.TextEntries[0].Text;
                    /*
                    Console.WriteLine("Saving set with name '{0}' containing {1} items:", name, currentEquipment.Count);
                    foreach (Layer layer in currentEquipment.Keys)
                        Console.WriteLine("ID: " + currentEquipment[layer]);*/
                    EquipmentSetRegistry.StoreSet(from, name, currentEquipment, savetoSlot);

                    if (from.HasGump(typeof(ChooseEquipmentGump)))
                        from.CloseGump(typeof(ChooseEquipmentGump));

                    from.SendGump(new ChooseEquipmentGump(from, currentEquipment));
                    break;
                }

            }
        }
    }
}