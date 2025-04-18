using System;
using System.Collections.Generic;
using System.Text;
using Server.Mobiles;
using Server.Targeting;
using Server.Targets;
using System.Collections;
using Server.Factions;
using Server.Scripts.Custom;
using Server.Scripts.Custom.Citizenship.CommonwealthBonuses;
using Server.Scripts.Custom.Citizenship;
using Server.Scripts;

namespace Server.Items
{
    public class DamageableItemMilitiaRestricted : Item
    {
        public enum ItemLevel
        {
            NotSet,
            VeryEasy,
            Easy,
            Average,
            Hard,
            VeryHard,
            Insane
        }

        private int m_Hits;
        private int m_HitsMax;
        private int m_StartID;
        private int m_DestroyedID;
        private int m_HalfHitsID;
        private bool m_DropsLoot;
        private ItemLevel m_ItemLevel;
        private IDamageableItemMilitiaRestricted m_Child;
        private Faction m_Militia;

        [CommandProperty(AccessLevel.GameMaster)]
        public IDamageableItemMilitiaRestricted Link
        {
            get
            {
                return m_Child;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool DropsLoot
        {
            get
            {
                return m_DropsLoot;
            }
            set
            {
                m_DropsLoot = value;

                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public ItemLevel Level
        {
            get
            {
                return m_ItemLevel;
            }
            set
            {
                m_ItemLevel = value;

                double bonus = (double)(((int)m_ItemLevel * 100.0) * ((int)m_ItemLevel * 5));

                HitsMax = ((int)(100 + bonus));
                Hits = ((int)(100 + bonus));

                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int IDStart
        {
            get
            {
                return m_StartID;
            }
            set
            {
                if (value < 0)
                    m_StartID = 0;
                else if (value > int.MaxValue)
                    m_StartID = int.MaxValue;
                else
                    m_StartID = value;

                if (m_Hits >= (m_HitsMax * 0.5))
                {
                    if (ItemID != m_StartID)
                        ItemID = m_StartID;
                }

                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int IDHalfHits
        {
            get
            {
                return m_HalfHitsID;
            }
            set
            {
                if (value < 0)
                    m_HalfHitsID = 0;
                else if (value > int.MaxValue)
                    m_HalfHitsID = int.MaxValue;
                else
                    m_HalfHitsID = value;

                if (m_Hits < (m_HitsMax * 0.5))
                {
                    if (ItemID != m_HalfHitsID)
                        ItemID = m_HalfHitsID;
                }

                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int IDDestroyed
        {
            get
            {
                return m_DestroyedID;
            }
            set
            {
                if (value < 0)
                    m_DestroyedID = 0;
                else if (value > int.MaxValue)
                    m_DestroyedID = int.MaxValue;
                else
                    m_DestroyedID = value;

                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Hits
        {
            get
            {
                return m_Hits;
            }
            set
            {
                if (value > m_HitsMax)
                    m_Hits = m_HitsMax;
                else
                    m_Hits = value;

                if (m_Child != null && (m_Hits > m_Child.Hits || m_Hits < m_Child.Hits))
                    UpdateHitsToEntity();

                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int HitsMax
        {
            get
            {
                return m_HitsMax;
            }
            set
            {
                if (value > int.MaxValue)
                    m_HitsMax = int.MaxValue;
                else
                    m_HitsMax = value;

                if (Hits > m_HitsMax)
                    Hits = m_HitsMax;

                if (m_Child != null && (m_HitsMax > m_Child.HitsMax || m_HitsMax < m_Child.HitsMax))
                    UpdateMaxHitsToEntity();

                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public string MilitiaName { 
            get 
            {
                if (Militia != null && Militia.Definition!= null)
                    return Militia.Definition.TownshipName;
                return null;
            } 
            set 
            {
                if (Faction.Factions == null) return;
                foreach (Faction faction in Faction.Factions)
                {
                    if (faction.Definition.TownshipName == value)
                    {
                        Militia = faction;
                        return;
                    }
                }
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Faction Militia { get { return m_Militia; } set { m_Militia = value; m_Child.Militia = value; } }
        [Constructable]
        public DamageableItemMilitiaRestricted(int StartID, int HalfID, int DestroyID, Faction militia)
            : base(StartID)
        {
            Name = "Damageable Item";
            Hue = 0;
            Movable = true;

            Level = ItemLevel.NotSet;

            IDStart = StartID;
            IDHalfHits = HalfID;
            IDDestroyed = DestroyID;

            m_Militia = militia;
        }

        [Constructable]
        public DamageableItemMilitiaRestricted() : this(130, 130, 130, null)
        {
        }

        public virtual void OnDamage(int amount, Mobile from, bool willKill)
        {
            return;
        }

        public virtual bool OnBeforeDestroyed()
        {
            return true;
        }

        public virtual void OnDestroyed(WoodenBox lootbox)
        {
            return;
        }

        public void UpdateMaxHitsToEntity()
        {
            m_Child.SetHits(HitsMax);
        }

        public void UpdateHitsToEntity()
        {
            m_Child.Hits = Hits;
        }

        public virtual void Damage(int amount, Mobile from, bool willKill)
        {
            if (willKill)
            {
                Destroy();
                return;
            }

            Hits -= amount;

            if (Hits >= (HitsMax * 0.5))
            {
                ItemID = IDStart;
            }
            else if (Hits < (HitsMax * 0.5))
            {
                ItemID = IDHalfHits;
            }
            else if (Hits <= 0)
            {
                Destroy();
                return;
            }

            OnDamage(amount, from, willKill);
        }

        public virtual bool Destroy()
        {
            if (this == null || this.Deleted)
                return false;

            if (OnBeforeDestroyed())
            {
                if (m_Child != null && !m_Child.Deleted && !m_Child.Alive)
                {
                    WoodenBox lootbox = new WoodenBox();
                    lootbox.Name = this.Name + " Loot Box";

                    OnDestroyed(lootbox);

                    if (lootbox != null && !lootbox.Deleted)
                    {
                        if (DropsLoot)
                            lootbox.MoveToWorld(new Point3D(this.X, this.Y, this.Z), this.Map);
                        else
                            lootbox.Delete();
                    }

                    if (m_Child != null)
                        m_Child.Delete();

                    Delete();

                    return true;
                }
                else
                {
                    WoodenBox lootbox = new WoodenBox();
                    lootbox.Name = this.Name + " Loot Box";

                    OnDestroyed(lootbox);

                    if (lootbox != null && !lootbox.Deleted)
                    {
                        if (DropsLoot)
                            lootbox.MoveToWorld(new Point3D(this.X, this.Y, this.Z), this.Map);
                        else
                            lootbox.Delete();
                    }

                    Delete();
                    return true;
                }
            }

            return false;
        }


        //Provides the Parent Item (this) with a new Entity Link
        public void ProvideEntity()
        {
            if (m_Child != null)
            {
                m_Child.Delete();
            }

            IDamageableItemMilitiaRestricted Idam = new IDamageableItemMilitiaRestricted(this);

            if (Idam != null && !Idam.Deleted && this.Map != null)
            {
                m_Child = Idam;
                m_Child.Militia = m_Militia;
                m_Child.Update();
            }
        }

        //If the child Link is not at our location, bring it back!
        public override void OnLocationChange(Point3D oldLocation)
        {
            if (this.Location != oldLocation)
            {
                if (m_Child != null && !m_Child.Deleted)
                {
                    if (m_Child.Location == oldLocation)
                        m_Child.Update();
                }
            }

            base.OnLocationChange(oldLocation);
        }

        //This Wraps the target and resets the targeted ITEM to it's child BASECREATURE
        //Unfortunately, there is no accessible Array for a player's followers,
        //so we must use the GetMobilesInRage(int range) method for our reference checks!
        public override bool CheckTarget(Mobile from, Target targ, object targeted)
        {
            #region CheckEntity

            //Check to see if we have an Entity Link (Child BaseCreature)
            //If not, create one!
            //(Without Combatant Change, since this is for pets)

            PlayerMobile pm = from as PlayerMobile;

            if (pm != null)
            {
                if (m_Child == null || m_Child.Deleted)
                    ProvideEntity();
                if (m_Child != null && !m_Child.Deleted)
                {
                    m_Child.Update();
                }
            }
            #endregion

            if (targ is AIControlMobileTarget && targeted == this)
            {
                /* PETS CANNOT ATTACK FLAGS/DOORS
                //Wrap the target
                AIControlMobileTarget t = targ as AIControlMobileTarget;
                //Get the OrderType
                OrderType order = t.Order;

                //Search for our controlled pets within screen range
                foreach (Mobile m in from.GetMobilesInRange(16))
                {
                    if (!(m is BaseCreature))
                        continue;

                    BaseCreature bc = m as BaseCreature;

                    if (from != null && !from.Deleted && from.Alive)
                    {
                        if (bc == null || bc.Deleted || !bc.Alive || !bc.Controlled || bc.ControlMaster != from)
                            continue;

                        //Reset the pet's ControlTarget and OrderType.
                        bc.ControlTarget = m_Child;
                        bc.ControlOrder = t.Order;
                    }
                }
                 * */
            }

            return base.CheckTarget(from, targ, targeted);
        }

        public override void OnDoubleClick(Mobile from)
        {
            #region CheckEntity

            //Check to see if we have an Entity Link (Child BaseCreature)
            //If not, create one!
            //(With Combatant change to simulate Attacking)

            PlayerMobile pm = from as PlayerMobile;

            if (pm != null)
            {
                if (m_Child != null && !m_Child.Deleted)
                {
                    m_Child.Update();
                    pm.Warmode = true;
                    pm.Combatant = m_Child;
                }
                else
                {
                    ProvideEntity();

                    if (m_Child != null && !m_Child.Deleted)
                    {
                        m_Child.Update();
                        pm.Warmode = true;
                        pm.Combatant = m_Child;
                    }
                }
            }
            #endregion

            base.OnDoubleClick(from);
        }

        public override bool OnDragLift(Mobile from)
        {
            return (from.AccessLevel >= AccessLevel.Counselor);
        }

        public override void Delete()
        {
            base.Delete();

            if (m_Child != null && !m_Child.Deleted)
            {
                m_Child.Delete();
                return;
            }
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            ArrayList strings = new ArrayList();

            strings.Add("-Strength-");
            strings.Add(m_Hits + "/" + m_HitsMax);

            string toAdd = "";
            int amount = strings.Count;
            int current = 1;

            foreach (string str in strings)
            {
                toAdd += str;

                if (current != amount)
                    toAdd += "\n";

                ++current;
            }

            if (toAdd != "")
                list.Add(1070722, toAdd);
        }

        public DamageableItemMilitiaRestricted(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version

            writer.Write((Mobile)m_Child);
            writer.Write((int)m_StartID);
            writer.Write((int)m_HalfHitsID);
            writer.Write((int)m_DestroyedID);
            writer.Write((int)m_ItemLevel);
            writer.Write((int)m_Hits);
            writer.Write((int)m_HitsMax);
            writer.Write((bool)Movable);
            Faction.WriteReference(writer, m_Militia);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            m_Child = (IDamageableItemMilitiaRestricted)reader.ReadMobile();
            m_StartID = (int)reader.ReadInt();
            m_HalfHitsID = (int)reader.ReadInt();
            m_DestroyedID = (int)reader.ReadInt();
            m_ItemLevel = (ItemLevel)reader.ReadInt();
            m_Hits = (int)reader.ReadInt();
            m_HitsMax = (int)reader.ReadInt();
            Movable = (bool)reader.ReadBool();
            m_Militia = Faction.ReadReference(reader);
        }


    }

    public class IDamageableItemMilitiaRestricted : BaseCreature
    {
        public override Poison PoisonImmune
        {
            get
            {
                return Poison.Lethal;
            }
        }

        public override bool BleedImmune { get { return true; } }

        private DamageableItemMilitiaRestricted m_Parent;

        [CommandProperty(AccessLevel.GameMaster)]
        public DamageableItemMilitiaRestricted Link
        {
            get
            {
                return m_Parent;
            }
        }
        private Faction m_Militia;
        [CommandProperty(AccessLevel.GameMaster)]
        public Faction Militia { get { return m_Militia; } set { m_Militia = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public string MilitiaName
        {
            get
            {
                if (m_Militia != null && m_Militia.Definition != null)
                    return m_Militia.Definition.TownshipName;
                return null;
            }
            set
            {
                if (Faction.Factions == null) return;
                foreach (Faction faction in Faction.Factions)
                {
                    if (faction.Definition.TownshipName == value)
                    {
                        m_Parent.Militia = faction;
                        return;
                    }
                }
            }
        }

        [Constructable]
        public IDamageableItemMilitiaRestricted(DamageableItemMilitiaRestricted parent)
            : base(AIType.AI_Melee, FightMode.None, 1, 1, 0.2, 0.4)
        {
            if (parent != null && !parent.Deleted)
                m_Parent = parent;

            //Nullify the name, so it doeesn't pop up when we come into range
            Name = null;

            Body = 803; //Mustache is barely visible!
            BodyValue = 803; //Mustache is barely visible!
            Hue = 0;
            BaseSoundID = 0; //QUIET!!!
            Fame = 0;
            Karma = 0;
            ControlSlots = 0;
            Tamable = false;

            Frozen = true;
            Paralyzed = true;
            CantWalk = true;

            DamageMin = 0;
            DamageMax = 0;

            SetStr(m_Parent.HitsMax);
            SetHits(m_Parent.HitsMax);
            Hits = m_Parent.Hits;

            for (int skill = 0; skill < this.Skills.Length; skill++)
            {
                this.Skills[(SkillName)skill].Cap = 1.0 + ((int)m_Parent.Level * 0.05);
                this.Skills[(SkillName)skill].Base = 1.0 + ((int)m_Parent.Level * 0.05);
            }

            m_Militia = parent.Militia;

            Update();
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            ArrayList strings = new ArrayList();

            strings.Add("-Strength-");
            strings.Add(m_Parent.Hits + "/" + m_Parent.HitsMax);

            string toAdd = "";
            int amount = strings.Count;
            int current = 1;

            foreach (string str in strings)
            {
                toAdd += str;

                if (current != amount)
                    toAdd += "\n";

                ++current;
            }

            if (toAdd != "")
                list.Add(1070722, toAdd);
        }

        public void Update()
        {
            if (this == null || this.Deleted)
                return;

            if (m_Parent != null && !m_Parent.Deleted)
            {
                this.Home = m_Parent.Location;
                this.Location = m_Parent.Location;
                this.Map = m_Parent.Map;

                return;
            }

            if (m_Parent == null || m_Parent.Deleted)
            {
                Delete();
                return;
            }
        }

        public override void Delete()
        {
            base.Delete();
        }

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            
            base.OnDamage(amount, from, willKill);
            if (m_Parent != null && !m_Parent.Deleted)
            {
                m_Parent.Damage(amount, from, willKill);
            }
        }
        
        public override void Damage(int amount, Mobile from)
        {
            if (from == null)
            {
                Console.WriteLine("Can't find who this {0} damage is coming from, so the destructable item will ignore it.");
                return;
            }

            ICommonwealth commonwealth = Commonwealth.Find(from);
            Faction militia = Faction.Find(from);
            if ((militia != null && militia != Militia)
                || (WorldWarsController.Instance != null
                    && WorldWarsController.Instance.Active
                    && commonwealth != null 
                    && commonwealth.Militia != Militia))
            {
                amount = GetModifiedDamageAmount(amount, commonwealth);
                base.Damage(amount, from);
            }
            else if ((militia != null && militia == Militia)
                    ||(WorldWarsController.Instance != null
                        && WorldWarsController.Instance.Active
                        && commonwealth != null
                        && commonwealth.Militia != Militia))
            {
                from.SendMessage("You cannot attack this item, it belongs to your militia!");
            }
            else
            {
                from.SendMessage("Only militia members can attack this item!");
            }
        }
        // hacky way to get to base.Damage from WorldWarsFlas
        public void BaseDamage(int amount, Mobile from)
        {
            base.Damage(amount, from);
        }

        public int GetModifiedDamageAmount(int amount, ICommonwealth cw)
        {
            int returnAmount = amount;

            if ((cw.GetHeroSettings() & (int)HeroBonusTypes.SiegeTools) != 0)
            {
                returnAmount = (int)(returnAmount * FeatureList.Citizenship.SiegeToolsDamageBonus);

                for (int i = 0; i < cw.CapturedHeroes.Count; i++)
                {
                    returnAmount = (int)(returnAmount * FeatureList.Citizenship.HeroBonusIncreasePerCapturedHero);
                }
            }

            if (!cw.Hero.IsHome)
            {
                returnAmount = (int)(returnAmount * FeatureList.Citizenship.AbsentHeroHeroBonusReduction);
            }

      
            return returnAmount;
        }
        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (m_Parent != null && !m_Parent.Deleted)
                m_Parent.Destroy();
        }

        public IDamageableItemMilitiaRestricted(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);

            writer.Write((Item)m_Parent);
            writer.Write((bool)Frozen);
            writer.Write((bool)Paralyzed);
            writer.Write((bool)CantWalk);
            writer.Write((int)DamageMin);
            writer.Write((int)DamageMax);
            writer.Write((int)BodyValue);
            Faction.WriteReference(writer, Militia);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_Parent = (DamageableItemMilitiaRestricted)reader.ReadItem();
            Frozen = (bool)reader.ReadBool();
            Paralyzed = (bool)reader.ReadBool();
            CantWalk = (bool)reader.ReadBool();
            DamageMin = (int)reader.ReadInt();
            DamageMax = (int)reader.ReadInt();
            BodyValue = (int)reader.ReadInt();
            Militia = Faction.ReadReference(reader);
        }
    }

}
