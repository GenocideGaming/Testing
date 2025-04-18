using System;
using Server.Network;

namespace Server.Items
{

	public class RangerTicket : Item
	{
		[Constructable]
		public RangerTicket() : base (0x14F0)
		{
			LootType = LootType.Blessed;
		}

		public RangerTicket (Serial serial) : base (serial)
		{
		}
		
		public override int LabelNumber
        {
            get
            {
                return 1041234;
            }
        }// Ticket for a piece of Ranger armor

      	public override void OnDoubleClick(Mobile from) 
      	{
			if (!IsChildOf(from.Backpack))
		{
            from.SendLocalizedMessage(1042001);
        }
        else
            {
                switch (Utility.Random(5))
                {
                    case 0: from.AddToBackpack(new RangerArms()); break;
                    case 1: from.AddToBackpack(new RangerChest()); break;
                    case 2: from.AddToBackpack(new RangerGloves()); break;                   
                    case 3: from.AddToBackpack(new RangerGorget()); break;
                    case 4: from.AddToBackpack(new RangerLegs()); break; 
                }
                this.Delete();
				from.SendLocalizedMessage(502064); // A piece of Ranger armor has been placed in your backpack.
			}

		}
		
		public override void Serialize (GenericWriter writer)
		{
			base.Serialize ( writer );
			writer.Write ( (int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize ( reader );
			int version = reader.ReadInt();
		}
	}
}