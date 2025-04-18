using System;
using System.Collections;
using Server.Items;
using Server.ContextMenus;
using Server.Misc;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a Galven Guard sentry corpse")]
    public class DungeonGuardMelee : BaseCreature
    {
        public override bool ShowFameTitle
        {
            get
            {
                return false;
            }
        }       

        private TalkTimer mTalkTimer;
        private DateTime m_LastSpeech;

        [Constructable]
        public DungeonGuardMelee()
            : base(AIType.AI_Generic, FightMode.Closest, 10, 1, 0.25, 0.8)
        {
            SpeechHue = Utility.RandomDyedHue();
            Name = "a Galven Guard sentry";
            Hue = 2155;
 
            if (this.Female = Utility.RandomBool())
            {
                this.Body = 0x191;
            }
            else
            {
                this.Body = 0x190;
            }

            Int32 guardColor = 2155;

            switch (Utility.Random(4))
            {
                case 0:
                    guardColor = 38; //Beige
                break;

                case 1:
                    guardColor = 38; //Red
                break;

                case 2:
                    guardColor = 95; //Blue
                break;

                case 3:
                    guardColor = 95; //Green
                break;
            }
 
            Utility.AssignRandomHair(this, 2155);

            SetStr(100, 100);
            SetDex(80, 80);
            SetInt(10, 10);

            SetHits(500, 600);
            SetStam(80, 80);
            SetMana(10, 10);

            SetDamage(20, 40);

            VirtualArmor = 40;

            SetSkill(SkillName.Tactics, 100.0, 100.0);

            SetSkill(SkillName.Fencing, 95.0, 100.0);
            SetSkill(SkillName.Macing, 95.0, 100.0);
            SetSkill(SkillName.Swords, 95.0, 100.0);            
            SetSkill(SkillName.Wrestling, 95.0, 100.0);

            SetSkill(SkillName.Healing, 10.0, 15.0);

            SetSkill(SkillName.Parry, 75.0, 80.0); 

            SetSkill(SkillName.MagicResist, 95.0, 100.0);            

            Fame = 5000;
            Karma = -5000;

            Item Cloak = new Cloak();
            Cloak.Movable = false;
            Cloak.Hue = guardColor;
            AddItem(Cloak);

            Item BodySash = new BodySash();
            BodySash.Movable = false;
            BodySash.Hue = guardColor;
            AddItem(BodySash);

            Item PlateChest = new PlateChest();
            PlateChest.Movable = false;
            AddItem(PlateChest);

            Item PlateArms = new PlateArms();
            PlateArms.Movable = false;
            AddItem(PlateArms);

            Item PlateLegs = new PlateLegs();
            PlateLegs.Movable = false;
            AddItem(PlateLegs);

            Item PlateGloves = new PlateGloves();
            PlateGloves.Movable = false;
            AddItem(PlateGloves);

            Item PlateGorget = new PlateGorget();
            PlateGorget.Movable = false;
            AddItem(PlateGorget);

            switch (Utility.Random(5))
            {
                case 0:
                    {
                        AddItem(new WarMace());
                        AddItem(new MetalKiteShield());
                    }
                break;

                case 1:
                {
                    AddItem(new VikingSword());
                    AddItem(new MetalKiteShield());
                }
                break;

                case 2:
                {
                    AddItem(new Broadsword());
                    AddItem(new MetalKiteShield());
                }
                break;

                case 3:
                {                    
                    AddItem(new Bardiche());
                    SetDamage(35, 60);
                }
                break;

                case 4:
                {
                    AddItem(new Halberd());
                    SetDamage(35, 60);
                }
                break;
            }  

            mTalkTimer = new TalkTimer(this, TimeSpan.FromMinutes(.5), TimeSpan.FromMinutes(1));
            mTalkTimer.Start();
        }

        public override bool Unprovokable { get { return true; } } //Can't Be Provoked

        public override void GenerateLoot()
        {
            base.PackAddGoldTier(6);
            base.PackAddRandomPotionTier(4);

            PackItem(new Torch());
            PackItem(new Kindling());
            PackItem(new Bedroll());
            PackItem(new ChickenLeg());
        }       

        public DungeonGuardMelee(Serial serial)
            : base(serial)
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

            mTalkTimer = new TalkTimer(this, TimeSpan.FromMinutes(.5), TimeSpan.FromMinutes(1.0));
            mTalkTimer.Start();
        }

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            // check netstate for pseudoseer system--don't allow them to auto talk while possessed
            if (!this.Deleted && this.NetState == null && !this.Warmode && !m.Frozen && DateTime.Now >= m_LastSpeech && InRange(m, 4) && !InRange(oldLocation, 4) && InLOS(m))
            {
                //Only Talk to Players
                if (m.Player && m.AccessLevel == AccessLevel.Player)
                {
                    m_LastSpeech = DateTime.Now + TimeSpan.FromSeconds(20);

                    SayRandomSpeech();
                }
            }
        }

        private static string[] RandomSpeech = new string[] 
        {
            "The Galven Guard has secured this area..",
            "Ahem...",
            "Excuse me.",
            "You don't belong here.",
            "...",
            "Back away, please. This area has been secured by the Galven Guard.",
            "Move along, citizen.",
            "Nothing to see here."           
        };

        private static string[] RandomCombatSpeech = new string[] 
        {
            "You shall be brought to justice!",
            "You will pay for your crimes!",
            "Do not resist, criminal!",
            "Surrender your arms!",
            "Stand down, villain!"
        };       

        private static string[] RandomEmotes = new string[]
        {
            "*glares menacingly*",
            "*mutters*",
            "*scans the horizon*",
            "*clears throat*",
            "*breathes deeply*",
            "*shifts weight*",
            "*taps foot*",
            "*cracks knuckles*",
            "*sighs*",
            "*glances at the sky*",
            "*coughs*",
            "My feet are killing me..."   

        };

        public void SayRandomCombatSpeech()
        {
            int i = Utility.Random(RandomCombatSpeech.Length);

            Say(RandomCombatSpeech[i]);
        }

        public void SayRandomSpeech()
        {
            int i = Utility.Random(RandomSpeech.Length);
           
            Say(RandomSpeech[i]);
        }

        public void SayRandomEmote()
        {
            int i = Utility.Random(RandomEmotes.Length);

            Say(RandomEmotes[i]);           
        }

        private class TalkTimer : Timer
        {
            private DungeonGuardMelee mDungeonGuardMelee;

            public TalkTimer(DungeonGuardMelee DungeonGuardMelee, TimeSpan delay, TimeSpan interval)
                : base(delay, interval)
            {
                mDungeonGuardMelee = DungeonGuardMelee;
            }

            protected override void OnTick()
            {
                base.OnTick();
                if (mDungeonGuardMelee.Deleted || mDungeonGuardMelee.NetState != null) { return; }
                //In Warmode
                if (mDungeonGuardMelee.Warmode)
                {
                     mDungeonGuardMelee.SayRandomCombatSpeech();
                }

                //Wandering
                else
                {
                    mDungeonGuardMelee.SayRandomEmote();
                }
            }
        }
    }
}