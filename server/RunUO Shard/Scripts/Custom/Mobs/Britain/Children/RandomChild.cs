using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Spells;
using Server.Network;
using System.Collections.Generic;
using Server.Engines.CannedEvil;

namespace Server.Mobiles
{
	[CorpseName( "an orphan corpse" )]
	public class RandomChild : BaseCreature
	{

		[Constructable]
		public RandomChild()
			: base( AIType.AI_Generic, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
            Name = "an orphan";

			SetStr( 14, 15 );
			SetDex( 40, 60 );
			SetInt( 11, 12 );

			SetHits( 50 );

			SetDamage( 3, 5 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 55, 85 );
			SetResistance( ResistanceType.Fire, 60, 80 );
			SetResistance( ResistanceType.Cold, 55, 85 );
			SetResistance( ResistanceType.Poison, 70, 90 );
			SetResistance( ResistanceType.Energy, 65, 75 );

			Fame = 10;
			Karma = 10;

			VirtualArmor = 5;

            switch (Utility.Random(4))
            {
                case 0: Body = 267; break; //standard
                case 1: Body = 269; break; // annoying
                case 2: Body = 271; break; // beggar
                case 3: Body = 272; break; //thief
            }
            SetSkill(SkillName.DetectHidden, 0.0, 50.0);

        }

        public override void GenerateLoot()
		{
            base.PackAddGoldTier(2);
            base.PackAddArtifactChanceTier(2);
            base.PackAddMapChanceTier(2);
        }

        public override void OnDeath( Container c )
		{
			base.OnDeath( c );

		}
		
		public override bool OnBeforeDeath()
		{

			return base.OnBeforeDeath();
		}


		public RandomChild( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 ); // version

		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

		}
	}
}
