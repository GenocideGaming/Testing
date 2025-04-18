using System;
using Server.Items;
using Server.Scripts.Engines.Factions.Instances.Factions;
using Server.Factions;

namespace Server.Mobiles 
{
	[CorpseName( "a squire corpse" )]
	public class POSquire : BaseCreature
	{		
		[Constructable]
		public POSquire() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = NameList.RandomName("male");
            Title = "the Squire";
            Body = 0x190;
			Hue = Utility.RandomSkinHue();
            Female = false;

            SetStr(91, 115);
            SetDex(61, 85);
            SetInt(81, 95);

            SetDamage(10, 14);

            SetSkill(SkillName.Healing, 80.0, 90.0);
            SetSkill(SkillName.Anatomy, 80.0, 90.0);
            SetSkill(SkillName.MagicResist, 80.0, 90.0);
            SetSkill(SkillName.Swords, 80.0, 90.0);
            SetSkill(SkillName.Tactics, 80.0, 90.0);
            SetSkill(SkillName.Wrestling, 80.0, 90.0);

            VirtualArmor = 8;
            AddItem(Immovable(new VikingSword()));
            AddItem(Immovable(new ChainChest()));
            AddItem(Immovable(new ChainLegs()));
            AddItem(Immovable(new CloseHelm()));
            AddItem(Immovable(new RingmailGloves()));
            AddItem(Immovable(Rehued(new Boots(), 2141)));
            AddItem(Immovable(Rehued(new OrderShield(), 2141)));
            AddItem(Immovable(Rehued(new BodySash(), 2141)));

            Fame = 550;
            Karma = 550;

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

        public POSquire(Serial serial)
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
