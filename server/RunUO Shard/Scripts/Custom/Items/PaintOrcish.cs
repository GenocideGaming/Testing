using System;
using Server;
using Server.Mobiles;
using Server.Spells;

namespace Server.Items
{
	public class OrcishPaint : Item
	{
		public override int LabelNumber{ get{ return 1063581; } } // orcish kin paint

		[Constructable]
		public OrcishPaint() : base( 0x9EC )
		{
			Hue = 1451;
			Weight = 2.0;
			Stackable = Core.ML;
		}

		public OrcishPaint( Serial serial ) : base( serial )
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
					from.HueMod = 1451;

					from.SendLocalizedMessage( 1063586 ); // You now bear the markings of the orc.  Your body paint will last about a week or you can remove it with an oil cloth.

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