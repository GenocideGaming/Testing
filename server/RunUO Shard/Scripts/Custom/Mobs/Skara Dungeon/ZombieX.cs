// abomination by SpookyRobert....
//Edted and revised for RUNUO 2.2 by FinalTwist

//don't forget to make distro edits per .txt

using System;
using System.Collections.Generic;
using Server.Items;
using Server.Factions;

namespace Server.Mobiles
{
    [CorpseName("an infected corpse")]
    public class Infect : BaseUndead
    {
        public override bool CanRegenHits { get { return true; } }

        public override Ethics.Ethic EthicAllegiance { get { return Ethics.Ethic.Evil; } }

        public override bool IsScaredOfScaryThings { get { return false; } }
        public override bool IsScaryToPets { get { return true; } }
        private RotTimer m_Timer;

        [Constructable]
        public Infect() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.1, .3)
        {
            Name = "an abomination";
            Body = 256;
            BaseSoundID = 471;
            Hue = 2711;

            SetStr(200);
            SetDex(25);
            SetInt(10);

            SetHits(500);
            SetStam(150);
            SetMana(0);

            SetDamage(10, 15);

            SetDamageType(ResistanceType.Physical, 25);
            SetDamageType(ResistanceType.Cold, 25);
            SetDamageType(ResistanceType.Poison, 50);

            SetResistance(ResistanceType.Physical, 50, 80);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 99, 100);
            SetResistance(ResistanceType.Poison, 99, 100);
            SetResistance(ResistanceType.Energy, 40, 60);

            SetSkill(SkillName.Poisoning, 120.0);
            SetSkill(SkillName.MagicResist, 100.0);
            SetSkill(SkillName.Tactics, 100.0);
            SetSkill(SkillName.Wrestling, 90.1, 100.0);

            PassiveSpeed = .4;
            ActiveSpeed = .15;

            Fame = 8000;
            Karma = -8000;

            VirtualArmor = 50;
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich, 2);
        }

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (!m.Hidden && InRange(m, 8) && InLOS(m))
            {
                if (IsEnemy(m) && m.AccessLevel < AccessLevel.Counselor)
                {
                    Combatant = m;
                    Warmode = true;
                }
            }
            //	//This deals with the Zombie's calls to other abomination's in the area
            if (m is Zombie || m is RottingCorpse && Combatant != null)  //m is abomination
            {
                if (m.Combatant == null)
                {
                    m.Combatant = Combatant;
                    m.Say("*Moan*");  // what does the zombie getting called say
                    Say("*MMooaann*"); // what does the zombie doing the calling say
                }
            }

            return;
        }

        public override void OnGotMeleeAttack(Mobile attacker)
        {
            if (HitsMaxSeed < 10)
            {
                HitsMaxSeed = 10;
            }
            if (Hits < HitsMax / 6)
            {
                CantWalk = true;
            }
            if (attacker is PlayerMobile)
            {
                switch (Utility.Random(2))
                {
                    case 0:
                        Str -= 1;
                        //this.Say(" losing 1 Str from getting hit");
                        break;
                        //case 1: this.HitsMaxSeed -= Utility.RandomMinMax(1,10); break;
                }
            }
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (defender is BaseCreature)
            {
                switch (Utility.Random(4))
                {
                    case 0:
                        Str -= 1;
                        //this.Say(" losing 1 Str from hitting");
                        break;
                }
            }
        }

        public Item CloneItem(Item item)
        {
            if ((item.Layer != Layer.Bank) && (item.Layer != Layer.Backpack))
            {
                Item newItem = new Item(item.ItemID);
                newItem.Hue = item.Hue;
                newItem.Layer = item.Layer;

                return newItem;
            }

            return null;
        }

        public virtual void NewZombie(Mobile from)
        {
            //What Should the Zombie virus not affect
            if (from is BaseUndead || (from is BaseCreature && ((BaseCreature)from).Summoned))
            {
                return;
            }
            else
            {
                double dhits, dstr;

                Female = from.Female;
                Body = from.Body;
                Hue = 768;
                Name = from.Name;
                Title = "*Infected*";

                HairItemID = from.HairItemID;
                HairHue = from.HairHue;
                FacialHairItemID = from.FacialHairItemID;
                FacialHairHue = from.FacialHairHue;

                SetResistance(ResistanceType.Physical, 40, 70);
                SetResistance(ResistanceType.Fire, 20, 30);
                SetResistance(ResistanceType.Cold, 99, 100);
                SetResistance(ResistanceType.Poison, 99, 100);
                SetResistance(ResistanceType.Energy, 30, 60);

                dhits = from.HitsMax * 1.2;
                dstr = from.Str * 0.80;

                for (int i = 0; i < from.Skills.Length; ++i)
                {
                    Skills[i].Base = from.Skills[i].Base * 0.80;
                    Skills[i].Cap = from.Skills[i].Cap;
                }

                HitsMaxSeed = from.HitsMax + (int)dhits;
                Hits = from.HitsMax + (int)dhits;
                Str = from.Str + (int)dstr;

                //region moveitems
                if (from.Items != null && from.Backpack != null)
                {
                    List<Item> mobitems = new List<Item>(from.Items);
		
					foreach (Item item in mobitems)
						{
							if (item.Layer == Layer.Backpack || item.Layer == Layer.Bank || item.Insured || item.LootType == LootType.Blessed || item.LootType == LootType.Newbied) continue;
							EquipItem(item);
						}

                    
                    mobitems = new List<Item>(from.Backpack.Items);
                    if (Backpack == null)
                    {
                        Backpack backpack = new Backpack();
                        backpack.Movable = false;
                        AddItem(backpack);
                    }
                    foreach (Item item in mobitems)
                    {
                        if (item.Insured || item.LootType == LootType.Blessed || item.LootType == LootType.Newbied) continue;
                        Backpack.AddItem(item);
                    }

                    if (from.Mounted)
                    {
                        if (from.Mount is EtherealMount)
                        {
                            EtherealMount pet = from.Mount as EtherealMount;
                            pet.Internalize();
                            pet.Rider = null;
                        }
                        else if (from.Mount is BaseMount)
                        {
                            BaseMount petm = from.Mount as BaseMount;
                            petm.Rider = null;
                            BaseCreature pet = petm as BaseCreature;
                            pet.Internalize();
                            pet.ControlTarget = null;
                            pet.SetControlMaster(null);
                            pet.SummonMaster = null;
                            pet.MoveToWorld(Location, Map);
                        }
                    }
                }
                //endregion

                MoveToWorld(from.Location, from.Map);
                if (from is BaseCreature && !((BaseCreature)from).IsBonded)
                    from.Delete();
            }
            return;
        }

        private class RotTimer : Timer
        {
            private Mobile m_Mobile;

            //Change timespan to change the rate of decay

            public RotTimer(Mobile m) : base(TimeSpan.FromMinutes(5.0))
            {
                m_Mobile = m;
            }

            protected override void OnTick()
            {
                RotTimer rotTimer = new RotTimer(m_Mobile);
                if (m_Mobile.Str < 2)
                {
                    m_Mobile.Kill();
                    Stop();
                }
                else
                {
                    if (m_Mobile.Hits > m_Mobile.HitsMax / 2)
                    {
                        m_Mobile.Hits -= m_Mobile.HitsMax / 100;
                        //m_Mobile.Say(" losing 1% Hits");
                    }

                    if (m_Mobile.Hits <= m_Mobile.HitsMax / 2)
                    {
                        m_Mobile.Hits -= 1;
                        //m_Mobile.Say(" losing 1 Hitpoint");
                    }

                    if (m_Mobile.Hits < m_Mobile.HitsMax / 4)
                    {
                        m_Mobile.CantWalk = true;
                        //m_Mobile.Say(" Cant Walk Anymore");
                    }

                    if (m_Mobile.Str > 100)
                    {
                        m_Mobile.Str -= m_Mobile.Str / 30;
                        //m_Mobile.Say(" losing 3.33% Str");
                    }

                    if (m_Mobile.Str <= 100)
                    {
                        m_Mobile.Str -= 1;
                        //m_Mobile.Say(" losing 1 Str point");
                    }
                    rotTimer.Start();
                }
            }
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool BleedImmune { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } }
        public override Poison HitPoison { get { return Poison.Lethal; } }
        public override bool CanRummageCorpses { get { return true; } }
        /*
        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
            if (Utility.RandomDouble() < 0.99)
                c.DropItem(new ZombieKillerPotion());
            if (Utility.RandomDouble() < 0.99)
                c.DropItem(new UndeadSlayerDeed());
        }
        */
        public Infect(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            //Remove timer here to stop decay but don't forget the one Constructable section!
            //m_Timer = new RotTimer(this);
            //m_Timer.Start();
        }
    }
}
