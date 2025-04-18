using System;

namespace Server.Items
{
    public class ProduceBarrel : Item
	{
		[Constructable]
        public ProduceBarrel()
            : base(Utility.RandomList(0x473E, 0x473F, 0x4740, 0x4741, 0x4742, 0x4743, 0x4744, 0x4745, 0x4746, 0x4747, 0x4748, 0x4749, 0x474A, 0x474B, 0x474C))
		{
			Movable = true;
			Stackable = false;
		}

        public ProduceBarrel(Serial serial)
            : base(serial)
		{            
		}

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "a barrel of produce");
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
