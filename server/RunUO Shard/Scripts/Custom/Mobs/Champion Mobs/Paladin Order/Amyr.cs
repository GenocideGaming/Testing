using System;
using Server;
using Server.Misc;
using Server.Scripts.Engines.Factions.Instances.Factions;
using Server.Factions;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
	public class Amyr : BaseCreature
	{

        [Constructable]
        public Amyr() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Body = 0x190;
            Name = "an amyr";
            AddItem(Immovable(Rehued(new BookOfChivalry(), 1150)));
            AddItem(Rehued(new HoodedShroudOfShadows(), 2141));
            AddItem(Rehued(new HalfApron(), 1150));
            AddItem(Rehued(new Lantern(), 1150));
            AddItem(Rehued(new Sandals(), 1150));

            SetStr(151, 175);
            SetDex(61, 85);
            SetInt(151, 175);

            VirtualArmor = 50;

            SetSkill(SkillName.Macing, 110.0, 120.0);
            SetSkill(SkillName.Wrestling, 110.0, 120.0);
            SetSkill(SkillName.Tactics, 110.0, 120.0);
            SetSkill(SkillName.MagicResist, 110.0, 120.0);
            SetSkill(SkillName.Healing, 110.0, 120.0);
            SetSkill(SkillName.Anatomy, 110.0, 120.0);

            SetSkill(SkillName.Magery, 110.0, 120.0);
            SetSkill(SkillName.EvalInt, 110.0, 120.0);
            SetSkill(SkillName.Meditation, 110.0, 120.0);

            Fame = 1550;
            Karma = -1550;

        }

        public Amyr(Serial serial) : base(serial)
        {
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
        //public override double HealChance { get { return 1.0; } }
        public override Ethics.Ethic EthicAllegiance { get { return Ethics.Ethic.Hero; } }
		public override bool ShowFameTitle{ get{ return false; } }
		public override bool ClickTitle{ get{ return true; } }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            c.Delete();
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
