using System;
using System.Collections.Generic;
using Server;
using Server.ContextMenus;
using Server.Engines.PartySystem;
using Server.Gumps;
using Server.Multis;
using Server.Network;
using Server.Scripts;

namespace Server.Items
{
    public class TreasureMapChest : LockableContainer
    {
        public override int LabelNumber { get { return 3000541; } }

        private int m_Level;
        private DateTime m_DeleteTime;
        private Timer m_Timer;
        private Mobile m_Owner;
        private bool m_Temporary;

        private List<Mobile> m_Guardians;

        [CommandProperty(AccessLevel.GameMaster)]
        public int Level { get { return m_Level; } set { m_Level = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Owner { get { return m_Owner; } set { m_Owner = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime DeleteTime { get { return m_DeleteTime; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Temporary { get { return m_Temporary; } set { m_Temporary = value; } }

        public List<Mobile> Guardians { get { return m_Guardians; } }

        [Constructable]
        public TreasureMapChest(int level)
            : this(null, level, false)
        {
        }

        public TreasureMapChest(Mobile owner, int level, bool temporary)
            : base(0xE40)
        {
            m_Owner = owner;
            m_Level = level;
            m_DeleteTime = DateTime.Now + TimeSpan.FromHours(3.0);   // how long treasure chests stays in world for

            m_Temporary = temporary;
            m_Guardians = new List<Mobile>();

            m_Timer = new DeleteTimer(this, m_DeleteTime);
            m_Timer.Start();

            Fill(this, level);
        }

        public static void Fill(LockableContainer cont, int level)
        {
            cont.Movable = false;
            cont.Locked = true;

            cont.TrapType = TrapType.ExplosionTrap;

            cont.TrapPower = level * Utility.RandomMinMax(25, 50);
            cont.TrapLevel = level;

            switch (level)
            {
                case 1: cont.RequiredSkill = 15; break;
                case 2: cont.RequiredSkill = 30; break;
                case 3: cont.RequiredSkill = 45; break;
                case 4: cont.RequiredSkill = 60; break;
                case 5: cont.RequiredSkill = 75; break;
                case 6: cont.RequiredSkill = 90; break;
            }

            cont.LockLevel = cont.RequiredSkill;            

            int gold = 1000 * level;
            int reagentAmount = 50 * level;
            int gemAmount = 5 * level;

            int clothBoltsAmount = 10 * level;
            int boardsAmount = 150 * level;
            int ingotsAmount = 150 * level;
            int cutLeatherAmount = 150 * level;

            double slayerArtifactChance = ((double)level * .10) + 0;

            double level1ArtifactChance = ((double)level * .25) + .20;
            double level2ArtifactChance = ((double)level * .15);
            double level3ArtifactChance = ((double)level * .15) - .20;
            double level4ArtifactChance = ((double)level * .10) - .30;

            double level5SpellChance = ((double)level * .25) + .75;
            double level6SpellChance = ((double)level * .25) + .5;
            double level7SpellChance = ((double)level * .25) + .25;
            double level8SpellChance = ((double)level * .25) + 0;

            double rareChance = ((double)level * FeatureList.TreasureMapChest.RareChancePerLevel);

            //Gold
            cont.DropItem(new Gold(gold));

            //Reagents
            Item regeants1 = Loot.RandomReagent();
            regeants1.Amount = reagentAmount;
            cont.DropItem(regeants1);

            Item regeants2 = Loot.RandomReagent();
            regeants2.Amount = reagentAmount;
            cont.DropItem(regeants2);

            //Gems
            Item gems1 = Loot.RandomGem();
            gems1.Amount = gemAmount;
            cont.DropItem(gems1);

            Item gems2 = Loot.RandomGem();
            gems2.Amount = gemAmount;
            cont.DropItem(gems2);

            //Resource Drop
            switch (Utility.Random(4))
            {
                //ClothBolts
                case 0:                   
                    Item bolts = new BoltOfCloth();
                    bolts.Amount = clothBoltsAmount;
                    cont.DropItem(bolts);  
                break;

                //Boards
                case 1:
                    Item boards = new Board();
                    boards.Amount = boardsAmount;
                    cont.DropItem(boards);
                break;

                //Ingots
                case 2:                  
                   Item ingots = new IronIngot();
                   ingots.Amount = ingotsAmount;
                   cont.DropItem(ingots);
                break;

                //Ingots
                case 3:
                    Item cutLeather = new Leather();
                    cutLeather.Amount = cutLeatherAmount;
                    cont.DropItem(cutLeather);
                break;
            }

            //Artifacts
            if (slayerArtifactChance >= Utility.RandomDouble())
                cont.DropItem(Loot.RandomSlayerArtifact());

            if (level1ArtifactChance >= Utility.RandomDouble())
                cont.DropItem(Loot.RandomArtifact(1));

            if (level2ArtifactChance >= Utility.RandomDouble())
                cont.DropItem(Loot.RandomArtifact(2));

            if (level3ArtifactChance >= Utility.RandomDouble())
                cont.DropItem(Loot.RandomArtifact(3));

            if (level4ArtifactChance >= Utility.RandomDouble())
                cont.DropItem(Loot.RandomArtifact(4));           

            //Scrolls
            if (level5SpellChance >= Utility.RandomDouble())
            {
                int index = (5 - 1) * 8;
                cont.DropItem(Loot.RandomScroll(index, index + 7, SpellbookType.Regular));
                cont.DropItem(Loot.RandomScroll(index, index + 7, SpellbookType.Regular));                
            }

            if (level6SpellChance >= Utility.RandomDouble())
            {
                int index = (6 - 1) * 8;
                cont.DropItem(Loot.RandomScroll(index, index + 7, SpellbookType.Regular));
                cont.DropItem(Loot.RandomScroll(index, index + 7, SpellbookType.Regular));
            }

            if (level7SpellChance >= Utility.RandomDouble())
            {
                int index = (7 - 1) * 8;
                cont.DropItem(Loot.RandomScroll(index, index + 7, SpellbookType.Regular));
                cont.DropItem(Loot.RandomScroll(index, index + 7, SpellbookType.Regular));
            }

            if (level8SpellChance >= Utility.RandomDouble())
            {
                int index = (8 - 1) * 8;
                cont.DropItem(Loot.RandomScroll(index, index + 7, SpellbookType.Regular));
                cont.DropItem(Loot.RandomScroll(index, index + 7, SpellbookType.Regular));
            }

            //Rares
            if (rareChance >= Utility.RandomDouble())
            {
                switch (Utility.Random(6))
                {
                    case 0: cont.DropItem(new BottlesOfAle()); break;
                    case 1: cont.DropItem(new BottlesOfLiquor()); break;
                    case 2: cont.DropItem(new BottlesOfWine()); break;
                    case 3: cont.DropItem(new GuttedFish()); break;
                    case 4: cont.DropItem(new TropicalFruit()); break;
                    case 5: cont.DropItem(new Shackles()); break;
                }
            }
        }

        public override bool CheckLocked(Mobile from)
        {
            if (!this.Locked)
                return false;

            if (this.Level == 0 && from.AccessLevel < AccessLevel.GameMaster)
            {
                foreach (Mobile m in this.Guardians)
                {
                    if (m.Alive)
                    {
                        from.SendLocalizedMessage(1046448); // You must first kill the guardians before you may open this chest.
                        return true;
                    }
                }

                LockPick(from);

                return false;
            }

            else
            {
                return base.CheckLocked(from);
            }
        }

        private List<Type> m_Lifted = new List<Type>();

        private bool CheckLoot(Mobile m, bool criminalAction)
        {
            if (Items.Count == 0)
            {
                //from.SendLocalizedMessage(1048124, "", 0x8A5); // The old, rusted chest crumbles when you hit it.
                this.Delete();
            }

            if (m_Temporary)
                return false;

            if (m.AccessLevel >= AccessLevel.GameMaster || m_Owner == null || m == m_Owner)
                return true;

            Party p = Party.Get(m_Owner);

            if (p != null && p.Contains(m))
                return true;

            Map map = this.Map;

            if (map != null && (map.Rules & MapRules.HarmfulRestrictions) == 0)
            {
                if (criminalAction)
                    m.CriminalAction(true, false);
                else
                    m.SendLocalizedMessage(1010630); // Taking someone else's treasure is a criminal offense!

                return true;
            }

            m.SendLocalizedMessage(1010631); // You did not discover this chest!

            return false;
        }

        public override bool IsDecoContainer
        {
            get { return false; }
        }

        public override bool CheckItemUse(Mobile from, Item item)
        {
            return CheckLoot(from, item != this) && base.CheckItemUse(from, item);
        }

        public override bool CheckLift(Mobile from, Item item, ref LRReason reject)
        {
            return CheckLoot(from, true) && base.CheckLift(from, item, ref reject);
        }

        public override void OnItemLifted(Mobile from, Item item)
        {
            bool notYetLifted = !m_Lifted.Contains(item.GetType());

            from.RevealingAction();
                        
            if (notYetLifted)
            {
                m_Lifted.Add(item.GetType());

                if (0.1 >= Utility.RandomDouble()) // 10% chance to spawn a new monster
                    TreasureMap.Spawn(m_Level, GetWorldLocation(), Map, from, false);
            }

            base.OnItemLifted(from, item);
        }

        public override bool CheckHold(Mobile m, Item item, bool message, bool checkItems, int plusItems, int plusWeight)
        {
            if (m.AccessLevel < AccessLevel.GameMaster)
            {
                m.SendLocalizedMessage(1048122, "", 0x8A5); // The chest refuses to be filled with treasure again.
                return false;
            }

            return base.CheckHold(m, item, message, checkItems, plusItems, plusWeight);
        }

        public TreasureMapChest(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)2); // version

            writer.Write(m_Guardians, true);
            writer.Write((bool)m_Temporary);

            writer.Write(m_Owner);

            writer.Write((int)m_Level);
            writer.WriteDeltaTime(m_DeleteTime);
            //writer.Write(m_Lifted, true);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 2:
                    {
                        m_Guardians = reader.ReadStrongMobileList();
                        m_Temporary = reader.ReadBool();

                        goto case 1;
                    }

                case 1:
                    {
                        m_Owner = reader.ReadMobile();

                        goto case 0;
                    }

                case 0:
                    {
                        m_Level = reader.ReadInt();
                        m_DeleteTime = reader.ReadDeltaTime();
                        //m_Lifted = reader.ReadStrongItemList();

                        if (version < 2)
                            m_Guardians = new List<Mobile>();

                        break;
                    }
            }

            if (!m_Temporary)
            {
                m_Timer = new DeleteTimer(this, m_DeleteTime);
                m_Timer.Start();
            }
            else
            {
                Delete();
            }
        }

        public override void OnAfterDelete()
        {
            if (m_Timer != null)
                m_Timer.Stop();

            m_Timer = null;

            base.OnAfterDelete();
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);

            if (from.Alive)
                list.Add(new RemoveEntry(from, this));
        }

        public void BeginRemove(Mobile from)
        {
            if (!from.Alive)
                return;

            from.CloseGump(typeof(RemoveGump));
            from.SendGump(new RemoveGump(from, this));
        }

        public void EndRemove(Mobile from)
        {
            if (Deleted || from != m_Owner || !from.InRange(GetWorldLocation(), 3))
                return;

            from.SendLocalizedMessage(1048124, "", 0x8A5); // The old, rusted chest crumbles when you hit it.
            this.Delete();
        }

        private class RemoveGump : Gump
        {
            private Mobile m_From;
            private TreasureMapChest m_Chest;

            public RemoveGump(Mobile from, TreasureMapChest chest)
                : base(15, 15)
            {
                m_From = from;
                m_Chest = chest;

                Closable = false;
                Disposable = false;

                AddPage(0);

                AddBackground(30, 0, 240, 240, 2620);

                AddHtmlLocalized(45, 15, 200, 80, 1048125, 0xFFFFFF, false, false); // When this treasure chest is removed, any items still inside of it will be lost.
                AddHtmlLocalized(45, 95, 200, 60, 1048126, 0xFFFFFF, false, false); // Are you certain you're ready to remove this chest?

                AddButton(40, 153, 4005, 4007, 1, GumpButtonType.Reply, 0);
                AddHtmlLocalized(75, 155, 180, 40, 1048127, 0xFFFFFF, false, false); // Remove the Treasure Chest

                AddButton(40, 195, 4005, 4007, 2, GumpButtonType.Reply, 0);
                AddHtmlLocalized(75, 197, 180, 35, 1006045, 0xFFFFFF, false, false); // Cancel
            }

            public override void OnResponse(NetState sender, RelayInfo info)
            {
                if (info.ButtonID == 1)
                    m_Chest.EndRemove(m_From);
            }
        }

        private class RemoveEntry : ContextMenuEntry
        {
            private Mobile m_From;
            private TreasureMapChest m_Chest;

            public RemoveEntry(Mobile from, TreasureMapChest chest)
                : base(6149, 3)
            {
                m_From = from;
                m_Chest = chest;

                Enabled = (from == chest.Owner);
            }

            public override void OnClick()
            {
                if (m_Chest.Deleted || m_From != m_Chest.Owner || !m_From.CheckAlive())
                    return;

                m_Chest.BeginRemove(m_From);
            }
        }

        private class DeleteTimer : Timer
        {
            private Item m_Item;

            public DeleteTimer(Item item, DateTime time)
                : base(time - DateTime.Now)
            {
                m_Item = item;
                Priority = TimerPriority.OneMinute;
            }

            protected override void OnTick()
            {
                m_Item.Delete();
            }
        }
    }
}
