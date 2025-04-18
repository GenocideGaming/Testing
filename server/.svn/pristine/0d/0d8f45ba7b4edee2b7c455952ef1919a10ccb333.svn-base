using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.Items;

namespace Server.Mobiles
{    
    [CorpseName("a human corpse")]
    public class HumanMob : BaseCreature
    {
        //public override bool ShowFameTitle { get { return false; } }

        private int m_OwningSerial = -1;
        public int OwningSerial { get { return m_OwningSerial; } set { m_OwningSerial = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public override ulong TeamFlags
        {
	        get 
	        { 
		         return base.TeamFlags;
	        }
	        set 
	        { 
		        base.TeamFlags = value;
                Item sash = FindItemOnLayer(Layer.MiddleTorso);
                Item cloak = FindItemOnLayer(Layer.Cloak);
                if (sash == null) return;
                if (cloak == null) return;
                switch (value)
                {
                    case (ulong)AITeamList.TeamFlags.Team1:
                        sash.Hue = 2145; // pedran
                        cloak.Hue = 2145;
                        break;
                    case (ulong)AITeamList.TeamFlags.Team2:
                        sash.Hue = 2133; // arbor
                        cloak.Hue = 2133;
                        break;
                    case (ulong)AITeamList.TeamFlags.Team3:
                        sash.Hue = 2149; // vermell
                        cloak.Hue = 2149;
                        break;
                    case (ulong)AITeamList.TeamFlags.Team4:
                        sash.Hue = 2137; // calor
                        cloak.Hue = 2137;
                        break;
                    case (ulong)AITeamList.TeamFlags.Team5:
                        sash.Hue = 2141; // lillano
                        cloak.Hue = 2141;
                        break;
                    case (ulong)AITeamList.TeamFlags.Team6:
                        sash.Hue = 38;
                        cloak.Hue = 38;
                        break;
                    case (ulong)AITeamList.TeamFlags.Team7:
                        sash.Hue = 95;
                        cloak.Hue = 95;
                        break;
                }
	        }
        }

        [Constructable]
        public HumanMob()
            : base(AIType.AI_Generic, FightMode.Closest, 10, 1, 0.25, 0.8)
        {
            SpeechHue = Utility.RandomDyedHue();
            Name = "a human";
            Hue = 2155;

            if (this.Female = Utility.RandomBool())
            {
                this.Body = 0x191;
            }
            else
            {
                this.Body = 0x190;
            }

            Utility.AssignRandomHair(this, 2155);

            SetStr(100, 100);
            SetDex(25, 25);
            SetInt(25, 25);

            SetHits(100, 100);
            SetStam(25, 25);
            SetMana(25, 25);

            VirtualArmor = 0;
            BardImmuneCustom = true;
            WeaponDamage = true;
            TakesNormalDamage = true;
            Pseu_AllowInterrupts = true;
            //Pseu_KeepKillCredit = true;
            //Pseu_ConsumeReagents = true;
            //Pseu_SpellBookRequired = true;
            Pseu_SpellDelay = TimeSpan.Zero;
            ClearHandsOnCast = true;
            PowerWords = true;

            Fame = 0;
            Karma = 0;
            
            Item cloak = new Cloak();
            cloak.Movable = false;
            AddItem(cloak);

            Item bodySash = new BodySash();
            bodySash.Movable = false;
            AddItem(bodySash);

            Backpack pack = new Backpack();

            pack.Movable = false;

            AddItem(pack);

            /*
            LeatherChest leatherChest = new LeatherChest();
            leatherChest.Movable = false;
            leatherChest.Quality = ArmorQuality.Exceptional;
            AddItem(leatherChest);

            LeatherArms leatherArms = new LeatherArms();
            leatherArms.Movable = false;
            leatherArms.Quality = ArmorQuality.Exceptional;
            AddItem(leatherArms);

            LeatherLegs leatherLegs = new LeatherLegs();
            leatherLegs.Movable = false;
            leatherLegs.Quality = ArmorQuality.Exceptional;
            AddItem(leatherLegs);

            LeatherGloves leatherGloves = new LeatherGloves();
            leatherGloves.Movable = false;
            leatherGloves.Quality = ArmorQuality.Exceptional;
            AddItem(leatherGloves);

            LeatherGorget leatherGorget = new LeatherGorget();
            leatherGorget.Movable = false;
            leatherGorget.Quality = ArmorQuality.Exceptional;
            AddItem(leatherGorget);

            LeatherCap leatherCap= new LeatherCap();
            leatherCap.Movable = false;
            leatherCap.Quality = ArmorQuality.Exceptional;
            AddItem(leatherCap);
            */
        }

        public override void GenerateLoot()
        {
            /*TotalRefreshPotion refresh = new TotalRefreshPotion();
            GreaterStrengthPotion strength = new GreaterStrengthPotion();
            GreaterAgilityPotion agility = new GreaterAgilityPotion();
            GreaterCurePotion cure = new GreaterCurePotion();
            GreaterHealPotion heal = new GreaterHealPotion();
            GreaterExplosionPotion explosion = new GreaterExplosionPotion();
            refresh.Amount = 5;
            strength.Amount = 2;
            agility.Amount = 2;
            cure.Amount = 5;
            heal.Amount = 5;
            explosion.Amount = 3;
            PackItem(refresh);
            PackItem(strength);
            PackItem(agility);
            PackItem(cure);
            PackItem(heal);
            PackItem(explosion);
            Spellbook spellbook = new Spellbook(0xffffffffffffffffL);
            spellbook.Movable = false;
            PackItem(spellbook);*/
        }

        public HumanMob(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version 
            writer.Write((int)m_OwningSerial);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (version >= 1)
            {
                m_OwningSerial = reader.ReadInt();
            }
        }
    }
}
