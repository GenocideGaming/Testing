using System;
using Server.Items;
using Server.Scripts.Engines.Factions.Instances.Factions;
using Server.Factions;

namespace Server.Mobiles 
{
	[CorpseName( "a knight corpse" )]
	public class POKnight : BaseCreature
	{			
		[Constructable]
		public POKnight() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
            Name = "Sir " + NameList.RandomName("male");
            Title = "the Knight";
            Body = 0x190;
			Hue = Utility.RandomSkinHue();
            Female = false;

            SetStr(116, 125);
            SetDex(61, 85);
            SetInt(81, 95);

            SetDamage(20, 24);

            VirtualArmor = 16;

            SetSkill(SkillName.Swords, 90.0, 100.0);
            SetSkill(SkillName.Wrestling, 90.0, 100.0);
            SetSkill(SkillName.Tactics, 90.0, 100.0);
            SetSkill(SkillName.MagicResist, 90.0, 100.0);
            SetSkill(SkillName.Healing, 90.0, 100.0);
            SetSkill(SkillName.Anatomy, 90.0, 100.0);

            AddItem(Immovable(new Halberd()));
            AddItem(Immovable(new PlateHelm()));
            AddItem(Immovable(new PlateArms()));
            AddItem(Immovable(new PlateGorget()));
            AddItem(Immovable(new PlateGloves()));
            AddItem(Immovable(new PlateLegs()));
            AddItem(Immovable(new PlateChest()));
            AddItem(Immovable(Rehued(new BodySash(), 2141)));

            Fame = 750;
            Karma = -750;

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
        public override bool InitialInnocent { get { return true; } }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            c.Delete();
        }

        public POKnight(Serial serial)
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
