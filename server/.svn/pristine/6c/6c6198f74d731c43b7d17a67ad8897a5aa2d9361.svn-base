using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Scripts.Custom.Citizenship;

namespace Server.Scripts.Custom.VorshunWarsEquipment
{
    public static class VorshunArmory
    {
        public static void Restock(Mobile m)
        {
            //m.AddToBackpack(new Spellbook(0xffffffffffffffffL));
        }

        public static void PutConflictingItemInPack(Item item, Mobile m)
        {
            if (item == null || m == null || item.Layer == null) { return; }

            Item existingItem = m.FindItemOnLayer(item.Layer);
            if (existingItem != null)
            {
                m.RemoveItem(existingItem);
                if (existingItem.Movable == false)
                {
                    existingItem.Delete();
                }
                else
                {
                    m.Backpack.AddItem(existingItem);
                }
            }
        }

        public static void GiveStarterBackpack(Mobile m)
        {
            Backpack pack = new Backpack();
            ICommonwealth town = Commonwealth.Find(m);

            if (town == null)
                return;

            pack.Hue = CitizenDyeHue.Find(town.Definition.TownName.String);
            pack.AddItem(new Spellbook(0xffffffffffffffffL));
            m.AddItem(pack);

            Restock(m);
        }

        public static void GiveDefaultClothing(Mobile m)
        {
            ICommonwealth township = Commonwealth.Find(m);

            int hue;
            if (township == null)
                hue = 0;
            else
                hue = CitizenDyeHue.Find(township.Definition.TownName);

            Item pants = new LongPants(hue);
            Item shirt = new Shirt(hue);
            Item boots = new Boots(hue);

            pants.Movable = false;
            shirt.Movable = false;
            boots.Movable = false;

            m.AddItem(pants);
            m.AddItem(shirt);
            m.AddItem(boots);
        }
    }
}
