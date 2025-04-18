using System;
using Server;
using Server.Misc;
using Server.Scripts.Engines.Factions.Instances.Factions;
using Server.Factions;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
	public class ForsakenNecromancer : BaseCreature
	{

		[Constructable]
        //Had to change A_NecroMage to AI_Mage. Need to import additional AI's.7/9/22
		public ForsakenNecromancer () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
            Body = 0x190;
            Hue = 0x3C6;
            Name = "a necromancer";
            AddItem(Rehued(new BoneHelm(), 2149));
            AddItem(Rehued(new Skirt(), 2149));
            AddItem(Rehued(new Sandals(), 1109));
            AddItem(Rehued(new Lantern(), 2149));
            AddItem(Rehued(new HalfApron(), 1109));
            AddItem(Rehued(new Doublet(), 2149));
            AddItem(Rehued(new LeatherChest(), 1109));
            AddItem(Rehued(new LeatherGorget(), 1109));
            AddItem(Rehued(new LeatherArms(), 1109));
            AddItem(Rehued(new LeatherLegs(), 1109));
            AddItem(Rehued(new LeatherGloves(), 1109));
            AddItem(Immovable(Rehued(new NecromancerSpellbook(), 2149)));
            SetStr(126, 150);
            SetDex(61, 85);
            SetInt(81, 95);

            SetSkill(SkillName.Wrestling, 100.0, 110.0);
            SetSkill(SkillName.MagicResist, 100.0, 110.0);
            SetSkill(SkillName.Healing, 100.0, 110.0);
            SetSkill(SkillName.Anatomy, 100.0, 110.0);
            SetSkill(SkillName.Magery, 100.0, 110.0);
            SetSkill(SkillName.EvalInt, 100.0, 110.0);
            SetSkill(SkillName.Meditation, 100.0, 110.0);
            SetSkill(SkillName.Necromancy, 100.0, 110.0);
            SetSkill(SkillName.SpiritSpeak, 100.0, 110.0);

            Fame = 1050;
            Karma = -1050;

            VirtualArmor = 40;

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
        public ForsakenNecromancer(Serial serial) : base(serial)
        {
        }

        public override Faction FactionAllegiance { get { return Forsaken.Instance; } }
        //public override double HealChance { get { return 1.0; } }

        public override bool AlwaysAttackable { get { return true; } }

        public override OppositionGroup OppositionGroup
        {
            get
            {
                return OppositionGroup.FeyAndUndead;
            }
        }

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
