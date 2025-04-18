using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	public class WanderingHealer : BaseHealer
	{
		public override bool CanTeach{ get{ return true; } }

		public override bool CheckTeach( SkillName skill, Mobile from )
		{
			if ( !base.CheckTeach( skill, from ) )
				return false;

			return ( skill == SkillName.Anatomy )
				|| ( skill == SkillName.Camping )
				|| ( skill == SkillName.Forensics )
				|| ( skill == SkillName.Healing )
				|| ( skill == SkillName.SpiritSpeak );
		}

        [Constructable]
		public WanderingHealer()
		{
			Title = "the wandering healer";			

			SetSkill( SkillName.Camping, 80.0, 100.0 );
			SetSkill( SkillName.Forensics, 80.0, 100.0 );
			SetSkill( SkillName.SpiritSpeak, 80.0, 100.0 );

            Blessed = false;

            AddItem(new GnarledStaff());
		}

        public override void GenerateLoot()
        {      
            //Gold Coming From BaseType
            base.PackAddArtifactChanceTier(4);
            base.PackAddMapChanceTier(4);

            base.PackAddBandageTier(4);

            PackItem(new Scissors());
            PackItem(new BoltOfCloth());
        }

		public override bool ClickTitle{ get{ return false; } } // Do not display title in OnSingleClick

		public override bool CheckResurrect( Mobile m )
		{
			if ( m.Criminal )
			{
				Say( 501222 ); // Thou art a criminal.  I shall not resurrect thee.
				return false;
			}
			else if ( m.Kills >= 5 )
			{
				Say( 501223 ); // Thou'rt not a decent and good person. I shall not resurrect thee.
				return false;
			}
			else if ( m.Karma < 0 )
			{
				Say( 501224 ); // Thou hast strayed from the path of virtue, but thou still deservest a second chance.
			}

			return true;
		}

		public WanderingHealer( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}