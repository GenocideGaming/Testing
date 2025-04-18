using System; 
using System.Collections; 
using System.Collections.Generic;
using Server;
using Server.Misc; 
using Server.Items; 
using Server.Mobiles;
using Server.Gumps;
using Server.Targeting;
//using Server.OneTime;

namespace Server.Mobiles 
{ 
	public class BaseChild : BaseCreature //IOneTime
	{
       // public int OneTimeType = 3; //second : 3 = second, 4 = minute, 5 = hour, 6 = day (Pick a time interval 3-6)
		//public int OneTimeType { get; set; } //onetime edit
        public override bool ReacquireOnMovement{ get { return false; } }
		public override TimeSpan ReacquireDelay{ get { return TimeSpan.FromSeconds( 1.0 ); } }

		public int talkingtimer;
		public int type;
		public int body;
		public bool talk;
		public Mobile target;
		public bool stealingtimer;
		public int stealingcount;
		public bool action;
		public int actioncount;
		
		public override bool CanMoveOverObstacles { get { return true; } }
		public override bool CanDestroyObstacles { get { return true; } }
		
		public override bool HandlesOnSpeech( Mobile from ) 
		{ 
			return true; 
		} 
		
		
		public BaseChild(AIType ai, FightMode fm, int PR, int FR, double AS, double PS) : base( ai, fm, PR, FR, AS, PS )
        {
            SpeechHue = Utility.RandomDyedHue(); 
			RangePerception = BaseCreature.DefaultRangePerception*2;
			Criminal = false;
			//AIFullSpeedActive = AIFullSpeedPassive = true; // Force full speed
			//OneTimeType = 3;
			Karma = 5;
			talkingtimer = 0;
			stealingcount = 0;
			talk = true;
			stealingtimer = true;
			target = null;
			switch (Utility.Random(4))
			{
				case 0: type = 1; break; //standard
				case 1: type = 2; break; // annoying
				case 2: type = 3; break; // beggar
				case 3: type = 4; break; //thief
			}
			switch (Utility.Random(4))
			{
				case 0: body = 0; Body = 428; break; //standard
				case 1: body = 1; Body = 421; break; // annoying
				case 2: body = 2; Body = 422; break; // beggar
				case 3: body = 3; Body = 428; break; //thief
			}
			SetSkill(SkillName.DetectHidden, 0.0, 50.0);
		}

		public override bool IsEnemy( Mobile m )
		{
		    return false;
		}

		public override void OnMovement( Mobile m, Point3D oldLocation )
		{
			if ( !m.Frozen && InRange( m, 3 ) && !InRange( oldLocation, 3 ) && InLOS( m ) && m is PlayerMobile )
			{
				if (this.type == 2 || this.type == 3 || this.type == 4 && this.target == null && Utility.RandomDouble() > 0.98 ) // look to acquire a new target
					this.target = m;					
			}
			
			if (m is PlayerMobile && !m.Hidden && m.AccessLevel == AccessLevel.Player && ((BaseChild)this).talk && this.Combatant == null && this.target == null )
			{

				switch (Utility.Random(16))
				{
							case 0: Say("Wow!  A knight!"); break;
							case 1: Say("I wanna be like him one day."); break;
							case 2: Say("It's " + target.Name + "!"); break;
							case 3: Say("Look! It's " + target.Name ); break;
							case 4: Say("An adventurer! An adventurer!"); break;
							case 5: Say("What did you kill today " + target.Name + " ??"); break;
							case 6: Say("When I grow up, ill be just like " + target.Name + "."); break;
							case 7: Say(target.Name + " is shorter than I imagined..."); break;
							case 8: Say("I want armor like that!"); break;
				}
					
				((BaseChild)this).talk = true;
			}
		}				
	

		public override void OnThink()
		{

			if (this.target != null && this.target is PlayerMobile ) // has a target
			{	
				if ( !InLOS( this.target) || (int)this.GetDistanceToSqrt(this.target) >  15) // lost the target
					target = null;
				
				if (target != null && !this.Controlled)
				{
					((BaseCreature)this).Controlled = true;
					((BaseCreature)this).ControlOrder = OrderType.Follow;
					((BaseCreature)this).ControlTarget = this.target;
				}
				
				if (this.type == 1 || this.type == 4 && this.action)
					BeAnnoying(this.target);
				if (this.type == 2 && this.action) // beggar
					SBegging(this.target);
					
			}

			else if(this.target == null && this.Controlled)//no longer has a target, reset
			{
					((BaseCreature)this).Controlled = false;
					((BaseCreature)this).ControlOrder = OrderType.None;
					((BaseCreature)this).ControlTarget = null;				
			}
			
			base.OnThink();
					
		}

		public void SBegging( Mobile target)
		{
			
			if ( this.talk )
			{
				switch (Utility.Random(9))
				{
							case 0: Say("Please!  Please give me some food!"); break;
							case 1: Say("A few coins Sire?"); break;
							case 2: Say("Please " + target.Name + ", im hungry!"); break;
							case 3: Say("a few coins will go a long way for me and my brothers, " + target.Name ); break;
							case 4: Say("I didn't eat yesterday...."); break;
							case 5: Say("Please " + target.Name + ", a few coins."); break;
							case 6: Say("My shoes are old and full of holes, " + target.Name + "."); break;
							case 7: Say("I have nothing, " + target.Name + "."); break;
							case 8: Say("Give me a few coins please."); break;
				}
					
				((BaseChild)this).talk = true;
				((BaseChild)this).action = true;
				
			}			
			
			
		}
		
		
		public void BeAnnoying( Mobile target)
		{
			if ( this.talk )
			{
				switch (Utility.Random(9))
				{
							case 0: Say("Why are you wearing that?"); break;
							case 1: Say("What does this do?"); break;
							case 2: Say("I bet you die a lot, " + target.Name + "."); break;
							case 3: Say("Why did you do that, " + target.Name + "??" ); break;
							case 4: Say("You have a funny hat!  It's ugly!"); break;
							case 5: Say("My father could have at thee, " + target.Name + ".  He's stronger!"); break;
							case 6: Say("You stink like an old cow,  " + target.Name + "."); break;
							case 7: Say("you're not that strong, " + target.Name + "."); break;
							case 8: Say("You stink!"); break;
				}
					
				((BaseChild)this).talk = true;
			}
			
			if (Utility.RandomDouble() > 0.80 && this.InRange( target, 1 ) )  // trip them
				target.Direction = target.GetDirectionTo( this );
				
			//else if (Utility.RandomDouble() > 0.80 && this.InRange( target, 1 ) ) // push them
			//	target.WalkRandom( 0, 3, 1); // BaseAI method -- nocompile
				
			// affect their pet (command.Stay)
			
			// swap gold for copper
			
			// other ideas?
			
			((BaseChild)this).action = true;
			
		}
		
    public override void OnSpeech( SpeechEventArgs e ) 
    {
      	if( e.Mobile.InRange( this, 4 ))
      	{
			if (this.type == 2 || (Utility.RandomBool() && this.type == 4) )
			{

				if ( ( e.Speech.ToLower() == "follow" ) )
				{
					Say("Follow me  me me me me me me me me !" );
				}
				else if ( ( e.Speech.ToLower() == "buy" ) )
				{
					Yell("Buy something for me!! Buy something for me!! Buy something for me!! Buy something for me!! Buy something for me!!" );
					Yell("NOOOOOW!!");
				}
				else if ( ( e.Speech.ToLower() == "sell" ) )
				{
					Yell("SELL!  SELL! SELL! WAAAAH NO DON'T SELL IT!");
				}
				else if ( ( e.Speech.ToLower() == "sell" ) )
				{
					Yell("NO!  I DON'T WANT TO STAY HERE!  NO NO NO NO NO!");
				}
				else
				{
					switch (Utility.Random(4))
					{
								case 0: this.Say( "why " + e.Speech + "?" ); break;
								case 1: this.Say( e.Speech ); break;
								case 2: this.Yell(" Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah Blah "); break;
								case 3: this.Yell(" Not " + e.Speech + " !"); break;
					}

				}
				((BaseChild)this).talk = true;	
			}						
      	}
    } 
		
		public override void OnDamagedBySpell(Mobile attacker)
        {
			// let them say sad things
			
			if (Utility.RandomDouble() > 0.80) //they hide
				this.Hidden = true;
			else if (this.Controlled)
			{
					//((BaseCreature)this).target = null;
				//this.FocusMob = attacker;
				//((BaseCreature)this).ControlOrder = OrderType.Attack;
				//BaseAI.DoActionFlee(); not working
			}
				
			base.OnDamagedBySpell(attacker);

		}
		
        public override void OnGotMeleeAttack(Mobile attacker)
        {
	
			// let them say sad things
			
			if (Utility.RandomDouble() > 0.80) //they hide
				this.Hidden = true;
				
			else if (this.Controlled)
			{	
					//((BaseCreature)this).target = null;
				//this.FocusMob = attacker;
				//DoActionFlee(); not working
				//((BaseCreature)this).ControlOrder = OrderType.Attack;
			}

			base.OnGotMeleeAttack(attacker);
		}

        public void OneTimeTick()
        {

			if (((BaseChild)this).talkingtimer >= 2 && !((BaseChild)this).talk ) // talking timer
			{
				((BaseChild)this).talk = true;
				((BaseChild)this).talkingtimer = 0;
			}
			if ( !((BaseChild)this).talk )
				((BaseChild)this).talkingtimer += 1;
			
			if ( !((BaseChild)this).stealingtimer && ((BaseChild)this).stealingcount >= Utility.RandomMinMax(5, 10));
			{
					((BaseChild)this).stealingtimer = true;
					((BaseChild)this).stealingcount = 0;
			}
			if ( !((BaseChild)this).stealingtimer )
				((BaseChild)this).stealingcount += 1;

			if (((BaseChild)this).actioncount >= 2 && !((BaseChild)this).action ) // talking timer
			{
				((BaseChild)this).action = true;
				((BaseChild)this).actioncount = 0;
			}
			if ( !((BaseChild)this).action )
				((BaseChild)this).actioncount += 1;

		}

 
		public override bool OnBeforeDeath()
		{
			if (Utility.RandomDouble() < 0.70)
			{
				// add heart?
			}
			
			return base.OnBeforeDeath();

		}
		
		
		public BaseChild( Serial serial ) : base( serial ) 
		{ 
		} 

		public override void Serialize( GenericWriter writer ) 
		{ 
			base.Serialize( writer ); 

			writer.Write( (int) 0 ); // version 
			writer.Write( (int) type );
			writer.Write( (int) body );
		} 

		public override void Deserialize( GenericReader reader ) 
		{ 
			base.Deserialize( reader ); 

			int version = reader.ReadInt(); 
			int type = reader.ReadInt();
			int body = reader.ReadInt();

            switch (body)
            {
                case 0: Body = 428; break;
                case 1: Body = 0x24; break;
                case 2: Body = 0x26; break;
                case 3: Body = 428; break;
            }


            //AIFullSpeedActive = AIFullSpeedPassive = true; // Force full speed
        }

    }
}

