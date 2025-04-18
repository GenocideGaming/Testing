using System;
using Server;
using Server.Misc;
using Server.Scripts.Engines.Factions.Instances.Factions;
using Server.Factions;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
	public class UrukShaman : BaseCreature
	{

		[Constructable]
        //Had to change A_Mystic to AI_Mage. Need to import additional AI's.7/9/22

        public UrukShaman () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
            Name = "an orc shaman";
            SpeechHue = Utility.RandomDyedHue();
            Body = 140;
            Hue = 2155;
            BaseSoundID = 0x45A;

            SpellCastAnimation = 11;
            SpellCastFrameCount = 5;

            SetStr(126, 150);
            SetDex(61, 85);
            SetInt(81, 95);

            SetSkill(SkillName.Macing, 100.0, 110.0);
            SetSkill(SkillName.Wrestling, 100.0, 110.0);
            SetSkill(SkillName.Tactics, 100.0, 110.0);
            SetSkill(SkillName.MagicResist, 100.0, 110.0);
            SetSkill(SkillName.Healing, 100.0, 110.0);
            SetSkill(SkillName.Anatomy, 100.0, 110.0);

            SetSkill(SkillName.Spellweaving, 100.0, 110.0);
            SetSkill(SkillName.EvalInt, 100.0, 110.0);
            SetSkill(SkillName.Meditation, 100.0, 110.0);
            SetSkill(SkillName.Mysticism, 100.0, 110.0);
            SetSkill(SkillName.Focus, 100.0, 110.0);


            Fame = 1750;
            Karma = -1750;

            VirtualArmor = 40;
		}

        public UrukShaman(Serial serial) : base(serial)
        {
        }
        //public override double HealChance { get { return 1.0; } }
        public override InhumanSpeech SpeechType { get { return InhumanSpeech.Orc; } }
        public override Faction FactionAllegiance { get { return Urukhai.Instance; } }
        public override bool AlwaysAttackable { get { return true; } }
        public override OppositionGroup OppositionGroup
        {
            get
            {
                return OppositionGroup.SavagesAndOrcs;
            }
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            c.Delete();
        }

        public override bool IsEnemy( Mobile m )
		{
			if ( m.Player && m.FindItemOnLayer( Layer.Helm ) is OrcishKinMask )
				return false;

			return base.IsEnemy( m );
		}

		public override void AggressiveAction( Mobile aggressor, bool criminal )
		{
			base.AggressiveAction( aggressor, criminal );

			Item item = aggressor.FindItemOnLayer( Layer.Helm );

			if ( item is OrcishKinMask )
			{
				AOS.Damage( aggressor, 50, 0, 100, 0, 0, 0 );
				item.Delete();
				aggressor.FixedParticles( 0x36BD, 20, 10, 5044, EffectLayer.Head );
				aggressor.PlaySound( 0x307 );
			}
		}

		public override bool ShowFameTitle{ get{ return false; } }
		public override bool ClickTitle{ get{ return false; } }

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
