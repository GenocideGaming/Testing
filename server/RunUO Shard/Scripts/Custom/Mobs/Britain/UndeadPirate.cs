using System;
using Server.Items;

namespace Server.Mobiles
{
	public class UndeadPirate : BaseCreature
	{
		public override bool ClickTitle { get { return false; } }

		[Constructable]
		public UndeadPirate() : base(AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4)
		{
			SpeechHue = Utility.RandomDyedHue();
			Hue = 768;

			if (this.Female = Utility.RandomBool())
			{
				Body = 0x191;
                Name = Utility.Random(2) == 0 ? "an undead pirate" : "an undead swashbuckler";
                AddItem(new ShortPants(Utility.RandomNeutralHue()));
			}

			else
			{
				Body = 0x190;
                Name = Utility.Random(2) == 0 ? "an undead pirate" : "an undead swashbuckler";
                AddItem(new ShortPants(Utility.RandomNeutralHue()));
			}

			switch (Utility.Random(2))
			{
				case 0: AddItem(new ThighBoots(Utility.RandomNeutralHue())); break;
			}

			switch (Utility.Random(3))
			{
				case 0: AddItem(new FancyShirt()); break;
				case 1: AddItem(new Shirt()); break;
				case 2: AddItem(new BodySash()); break;
			}

			switch (Utility.Random(3))
			{
				case 0: AddItem(new TricorneHat()); break;
				case 1: AddItem(new SkullCap()); break;
				case 2: AddItem(new Bandana()); break;
			}

			SetStr(100, 105);
			SetDex(45, 50);
			SetInt(20, 25);

			SetHits(50, 75);

			SetDamage(3, 7);

			VirtualArmor = 5;

			SetSkill(SkillName.Tactics, 100.0, 100.0); //Uses Weapon

			SetSkill(SkillName.MagicResist, 35.0, 40.0);

			SetSkill(SkillName.Swords, 65.0, 70.0);
			SetSkill(SkillName.Archery, 65.0, 70.0);
			SetSkill(SkillName.Macing, 65.0, 70.0);
			SetSkill(SkillName.Fencing, 65.0, 70.0);
			SetSkill(SkillName.Wrestling, 65.0, 70.0);

			Fame = 1000;
			Karma = -1000;

			switch (Utility.Random(6))
			{
				case 0: AddItem(new Scimitar()); break;
				case 1: AddItem(new Cutlass()); break;
				case 2: AddItem(new Broadsword()); break;
				case 3: AddItem(new Maul()); break;
				case 4: AddItem(new Club()); break;
				case 5:
					{
						AddItem(new Crossbow());
						PackItem(new CrossbowBolts(25));

						break;
					};
			}

			Utility.AssignRandomHair(this);
		}

		public override void GenerateLoot()
		{
			base.PackAddGoldTier(2);
			base.PackAddArtifactChanceTier(2);
			base.PackAddMapChanceTier(2);

			PackItem(new BeverageBottle(BeverageType.Ale));

		}

		public override bool AlwaysAttackable { get { return true; } }

		public UndeadPirate(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}
