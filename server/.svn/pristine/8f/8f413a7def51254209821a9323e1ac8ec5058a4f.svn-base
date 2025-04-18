using System;
using Server;
using Server.Mobiles;
using Server.Spells;

namespace Server.Items
{
	public class UndeadPaint : Item
	{
		public override int LabelNumber{ get{ return 1063585; } } // undead kin paint

		[Constructable]
		public UndeadPaint() : base( 0x9EC )
		{
			Hue = 1882;
			Weight = 2.0;
			Stackable = Core.ML;
		}

		public UndeadPaint( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( IsChildOf( from.Backpack ) )
			{
				if ( !from.CanBeginAction( typeof( Spells.Fifth.IncognitoSpell ) ) )
				{
					from.SendLocalizedMessage( 501698 ); // You cannot disguise yourself while incognitoed.
				}
				else if ( !from.CanBeginAction( typeof( Spells.Seventh.PolymorphSpell ) ) )
				{
					from.SendLocalizedMessage( 501699 ); // You cannot disguise yourself while polymorphed.
				}
				else if( TransformationSpellHelper.UnderTransformation( from ) )
				{
					from.SendLocalizedMessage( 501699 ); // You cannot disguise yourself while polymorphed.
				}
				else
				{
					from.HueMod = 1882;

					from.SendLocalizedMessage( 1063588 ); // You now bear the markings of the undead.  Your body paint will last about a week or you can remove it with an oil cloth.

					Consume();
				}
			}
			else
			{
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			}
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