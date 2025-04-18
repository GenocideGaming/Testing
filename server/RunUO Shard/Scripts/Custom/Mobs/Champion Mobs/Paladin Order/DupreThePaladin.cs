using System;
using Server.Items;
using Server.Factions;
using Server.Scripts.Engines.Factions.Instances.Factions;


namespace Server.Mobiles 
{
	public class DupreThePaladin : BaseCreature
	{			
		[Constructable]
        //Had to change A_Paladin to AI_Melee. Need to import additional AI's.7/9/22
        public DupreThePaladin() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Dupre";
            Title = "The Paladin";
            Body = 0x190;
            Female = false;

            SetStr(500);
            SetDex(150);
            SetInt(300);

            SetHits(10000);
            SetStam(400);
            SetMana(400);

            SetDamage(25, 35);

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 70;

            SetSkill( SkillName.EvalInt, 195.0, 220.0 );
			SetSkill( SkillName.Magery, 195.0, 220.0 );
			SetSkill( SkillName.Meditation, 195.0, 200.0 );
			SetSkill( SkillName.MagicResist, 100.0, 120.0 );
			SetSkill( SkillName.Tactics, 195.0, 220.0 );
			SetSkill( SkillName.Wrestling, 195.0, 220.0 );
            SetSkill(SkillName.Swords, 195.0, 220.0);
            SetSkill(SkillName.Anatomy, 195.0, 220.0);
            SetSkill(SkillName.Healing, 195.0, 220.0);

            AddItem(Rehued(new PlateHelm(), 2141));
            AddItem(Rehued(new PlateLegs(), 2141));
            AddItem(Rehued(new PlateArms(), 2141));
            AddItem(Rehued(new PlateGloves(), 2141));
            AddItem(Rehued(new PlateChest(), 2141));
            AddItem(Rehued(new PlateGorget(), 2141));
            AddItem(Immovable(Rehued(new VikingSword(), 1150)));
            AddItem(Immovable(Rehued(new DupresShield(), 1150)));
            AddItem(Rehued(new BodySash(), 1150));
            AddItem(Rehued(new Cloak(), 1150));
            AddItem(new VirtualMountItem(this));
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 3);
            AddItem(PowerScroll.CreateRandomNoCraft(5, 5));
        }
        public Item Rehued(Item item, int hue)
        {
            item.Hue = hue;
            return item;
        }

        public Item Immovable(Item item)
        {
            item.Movable = false;
            return item;
        }

        public override Faction FactionAllegiance { get { return PaladinOrder.Instance; } }
        public override bool AutoDispel { get { return true;} }
        public override double AutoDispelChance { get {return 1.0; } }
        public override bool BardImmune { get {return !true; } }
        public override bool Unprovokable { get {return true; } }
        public override bool Uncalmable { get {return true; } }
        public override Poison PoisonImmune { get { return Poison.Deadly; } }
        public override bool ClickTitle { get { return true; } }
        public override bool ShowFameTitle { get { return false; } }

        public class VirtualMount : IMount
        {
            private readonly VirtualMountItem m_Item;
            public VirtualMount(VirtualMountItem item)
            {
                this.m_Item = item;
            }

            public Mobile Rider
            {
                get
                {
                    return this.m_Item.Rider;
                }
                set
                {
                }
            }
            public virtual void OnRiderDamaged(Mobile from, ref int amount, bool willKill)
            {
            }

            public void OnRiderDamaged(int amount, Mobile from, bool willKill)
            {
                throw new NotImplementedException();
            }
        }
        public class VirtualMountItem : Item, IMountItem
        {
            private readonly VirtualMount m_Mount;
            private Mobile m_Rider;
            public VirtualMountItem(Mobile mob)
                : base(0x11C)
            {
                this.Layer = Layer.Mount;

                this.m_Rider = mob;
                this.m_Mount = new VirtualMount(this);
            }

            public VirtualMountItem(Serial serial)
                : base(serial)
            {
                this.m_Mount = new VirtualMount(this);
            }

            public Mobile Rider
            {
                get
                {
                    return this.m_Rider;
                }
            }
            public IMount Mount
            {
                get
                {
                    return this.m_Mount;
                }
            }
            public override void Serialize(GenericWriter writer)
            {
                base.Serialize(writer);

                writer.Write((int)0); // version

                writer.Write((Mobile)this.m_Rider);
            }

            public override void Deserialize(GenericReader reader)
            {
                base.Deserialize(reader);

                int version = reader.ReadInt();

                this.m_Rider = reader.ReadMobile();

                if (this.m_Rider == null)
                    this.Delete();
            }
        }
        public DupreThePaladin(Serial serial)
            : base(serial)
		{
		}

        public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}
