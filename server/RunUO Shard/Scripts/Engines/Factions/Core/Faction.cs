using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Guilds;
using Server.Mobiles;
using Server.Prompts;
using Server.Targeting;
using Server.Accounting;
using Server.Commands;
using Server.Commands.Generic;
using Server.Scripts.Custom.Citizenship;
using Server.Regions;
using Server.Scripts;
using Server.Scripts.Custom.WebService;

namespace Server.Factions
{
	[CustomEnum( new string[]{ "Forsaken", "Paladin Order", "Uruk-hai"} )]
	public abstract class Faction : IComparable
	{
		public int ZeroRankOffset;

		private FactionDefinition m_Definition;
		private FactionState m_State;

		public FactionDefinition Definition
		{
			get{ return m_Definition; }
			set
			{
				m_Definition = value;
			}
		}

		public FactionState State
		{
			get{ return m_State; }
			set{ m_State = value; }
		}

		public Mobile Commander
		{
			get{ return m_State.Commander; }
			set{ m_State.Commander = value; }
		}

		public int SilverTax
		{
			get{ return m_State.SilverTax; }
			set{ m_State.SilverTax = value; }
		}
        public DateTime LastSilverTaxChange
        {
            get { return m_State.LastSilverTaxChange; }
            set { m_State.LastSilverTaxChange = value; }
        }
        public bool CanChangeSilverTaxRate
        {
            get
            {
                if (DateTime.Now - LastSilverTaxChange > TimeSpan.FromHours(FeatureList.Militias.SilverTaxChangeBlockTimeInHours))
                    return true;
                return false;
            }
        }
		public int Silver
		{
			get{ return m_State.Silver; }
			set{ m_State.Silver = value; }
		}

		public List<PlayerState> Members
		{
			get{ return m_State.Members; }
			set{ m_State.Members = value; }
		}

        public ICommonwealth OwningCommonwealth
        {
            get { return m_State.MyCommonwealth; }
            set { m_State.MyCommonwealth = value; }
        }
		public static readonly TimeSpan LeavePeriod = TimeSpan.FromDays(FeatureList.Militias.LeaveTimeInDays);

		public bool FactionMessageReady
		{
			get{ return m_State.FactionMessageReady; }
		}

		public void Broadcast( string text )
		{
			Broadcast( 0x3B2, text );
		}

		public void Broadcast( int hue, string text )
		{
			List<PlayerState> members = Members;

			for ( int i = 0; i < members.Count; ++i )
				members[i].Mobile.SendMessage( hue, text );
		}

		public void Broadcast( int number )
		{
			List<PlayerState> members = Members;

			for ( int i = 0; i < members.Count; ++i )
				members[i].Mobile.SendLocalizedMessage( number );
		}

		public void Broadcast( string format, params object[] args )
		{
			Broadcast( String.Format( format, args ) );
		}

		public void Broadcast( int hue, string format, params object[] args )
		{
			Broadcast( hue, String.Format( format, args ) );
		}

		public void BeginBroadcast( Mobile from )
		{
            if (m_State.Silver - FeatureList.Militias.CommanderBroadcastSilverCost < 0)
            {
                from.SendMessage("You do not have enough silver available to broadcast a message!");
                return;
            }

			from.SendLocalizedMessage( 1010265 ); // Enter Faction Message
			from.Prompt = new BroadcastPrompt( this );
            m_State.Silver -= FeatureList.Militias.CommanderBroadcastSilverCost;
		}

		public void EndBroadcast( Mobile from, string text )
		{
			if ( from.AccessLevel == AccessLevel.Player )
				m_State.RegisterBroadcast();

			Broadcast( Definition.HueBroadcast, "{0} [Commander] {1} : {2}", from.Name, Definition.FriendlyName, text );
		}

		private class BroadcastPrompt : Prompt
		{
			private Faction m_Faction;

			public BroadcastPrompt( Faction faction )
			{
				m_Faction = faction;
			}

			public override void OnResponse( Mobile from, string text )
			{
				m_Faction.EndBroadcast( from, text );
			}
		}

		public static void HandleAtrophy()
		{
			foreach ( Faction f in Factions )
			{
				if ( !f.State.IsAtrophyReady )
					return;
			}

			List<PlayerState> activePlayers = new List<PlayerState>();

			foreach ( Faction f in Factions )
			{
				foreach ( PlayerState ps in f.Members )
				{
					if ( ps.KillPoints > 0 && ps.IsActive )
						activePlayers.Add( ps );
				}
			}

			int distrib = 0;

			foreach ( Faction f in Factions )
				distrib += f.State.CheckAtrophy();

			if ( activePlayers.Count == 0 )
				return;

			for ( int i = 0; i < distrib; ++i )
				activePlayers[Utility.Random( activePlayers.Count )].KillPoints++;
		}

		public static void DistributePoints( int distrib ) {
			List<PlayerState> activePlayers = new List<PlayerState>();

			foreach ( Faction f in Factions ) {
				foreach ( PlayerState ps in f.Members ) {
					if ( ps.KillPoints > 0 && ps.IsActive ) {
						activePlayers.Add( ps );
					}
				}
			}

			if ( activePlayers.Count > 0 ) {
				for ( int i = 0; i < distrib; ++i ) {
					activePlayers[Utility.Random( activePlayers.Count )].KillPoints++;
				}
			}
		}

		public void BeginHonorLeadership( Mobile from )
		{
			from.SendLocalizedMessage( 502090 ); // Click on the player whom you wish to honor.
			from.BeginTarget( 12, false, TargetFlags.None, new TargetCallback( HonorLeadership_OnTarget ) );
		}

		public void HonorLeadership_OnTarget( Mobile from, object obj )
		{
			if ( obj is Mobile )
			{
				Mobile recv = (Mobile) obj;

				PlayerState giveState = PlayerState.Find( from );
				PlayerState recvState = PlayerState.Find( recv );

				if ( giveState == null )
					return;

				if ( recvState == null || recvState.Faction != giveState.Faction )
				{
					from.SendLocalizedMessage( 1042497 ); // Only faction mates can be honored this way.
				}
				else if ( giveState.KillPoints < 5 )
				{
					from.SendLocalizedMessage( 1042499 ); // You must have at least five kill points to honor them.
				}
				else
				{
					recvState.LastHonorTime = DateTime.Now;
					giveState.KillPoints -= 5;
					recvState.KillPoints += 4;

					// TODO: Confirm no message sent to giver
					recv.SendLocalizedMessage( 1042500 ); // You have been honored with four kill points.
				}
			}
			else
			{
				from.SendLocalizedMessage( 1042496 ); // You may only honor another player.
			}
		}

		public virtual void AddMember( Mobile mob )
		{
			Members.Insert( ZeroRankOffset, new PlayerState( mob, this, Members ) );

			mob.AddToBackpack( FactionItem.Imbue( new Robe(), this, false, Definition.HuePrimary ) );
			mob.SendLocalizedMessage( 1010374 ); // You have been granted a robe which signifies your faction

			mob.InvalidateProperties();
			mob.Delta( MobileDelta.Noto );

			mob.FixedEffect( 0x373A, 10, 30 );
			mob.PlaySound( 0x209 );
		}

		public static bool IsNearType( Mobile mob, Type type, int range )
		{
			bool mobs = type.IsSubclassOf( typeof( Mobile ) );
			bool items = type.IsSubclassOf( typeof( Item ) );

			IPooledEnumerable eable;

			if ( mobs )
				eable = mob.GetMobilesInRange( range );
			else if ( items )
				eable = mob.GetItemsInRange( range );
			else
				return false;

			foreach ( object obj in eable )
			{
				if ( type.IsAssignableFrom( obj.GetType() ) )
				{
					eable.Free();
					return true;
				}
			}

			eable.Free();
			return false;
		}

		public static bool IsNearType( Mobile mob, Type[] types, int range )
		{
			IPooledEnumerable eable = mob.GetObjectsInRange( range );

			foreach( object obj in eable )
			{
				Type objType = obj.GetType();

				for( int i = 0; i < types.Length; i++ )
				{
					if( types[i].IsAssignableFrom( objType ) )
					{
						eable.Free();
						return true;
					}
				}
			}

			eable.Free();
			return false;
		}

		public void RemovePlayerState( PlayerState pl )
		{
			if ( pl == null || !Members.Contains( pl ) )
				return;

			int killPoints = pl.KillPoints;

			if ( pl.RankIndex != -1 ) {
				while ( ( pl.RankIndex + 1 ) < ZeroRankOffset ) {
					PlayerState pNext = Members[pl.RankIndex+1] as PlayerState;
					Members[pl.RankIndex+1] = pl;
					Members[pl.RankIndex] = pNext;
					pl.RankIndex++;
					pNext.RankIndex--;
				}

				ZeroRankOffset--;
			}

			Members.Remove( pl );

			PlayerMobile pm = (PlayerMobile)pl.Mobile;
			if ( pm == null )
				return;

			Mobile mob = pl.Mobile;
			if ( pm.FactionPlayerState == pl ) {
				pm.FactionPlayerState = null;

				mob.InvalidateProperties();
				mob.Delta( MobileDelta.Noto );

                if (Commander == mob)
                {
                    Commander = null;
                    OwningCommonwealth.CanAssignCommander = true;
                    OwningCommonwealth.Broadcast("The faction commander has left his post, a new commander can now be assigned.");
                }

				pm.ValidateEquipment();
			}

			if ( killPoints > 0 )
				DistributePoints( killPoints );
		}

		public void RemoveMember( Mobile mob )
		{
			PlayerState pl = PlayerState.Find( mob );

			if ( pl == null || !Members.Contains( pl ) )
				return;

			int killPoints = pl.KillPoints;

			if ( pl.RankIndex != -1 ) {
				while ( ( pl.RankIndex + 1 ) < ZeroRankOffset ) {
					PlayerState pNext = Members[pl.RankIndex+1];
					Members[pl.RankIndex+1] = pl;
					Members[pl.RankIndex] = pNext;
					pl.RankIndex++;
					pNext.RankIndex--;
				}

				ZeroRankOffset--;
			}

			Members.Remove( pl );

			if ( mob is PlayerMobile )
				((PlayerMobile)mob).FactionPlayerState = null;

			mob.InvalidateProperties();
			mob.Delta( MobileDelta.Noto );

            if (Commander == mob)
            {
                OwningCommonwealth.CanAssignCommander = true;
                OwningCommonwealth.Broadcast("The faction commander has left his post, a new commander can now be assigned.");
                Commander = null;
            }

			if ( mob is PlayerMobile )
				((PlayerMobile)mob).ValidateEquipment();

			if ( killPoints > 0 )
				DistributePoints( killPoints );

            //DatabaseController.UpdateCharacterMilitia(mob, false);
		}

		public void JoinAlone( Mobile mob )
		{
			AddMember( mob );
			mob.SendLocalizedMessage( 1005058 ); // You have joined the faction
		}

		private bool AlreadyHasCharInFaction( Mobile mob )
		{
			Account acct = mob.Account as Account;

			if ( acct != null )
			{
				for ( int i = 0; i < acct.Length; ++i )
				{
					Mobile c = acct[i];

					if ( Find( c ) != null  && Find(c) != this)
						return true;
				}
			}

			return false;
		}

		public static bool IsFactionBanned( Mobile mob )
		{
			Account acct = mob.Account as Account;

			if ( acct == null )
				return false;

			return ( acct.GetTag( "FactionBanned" ) != null );
		}

		public void OnJoinAccepted( Mobile mob )
		{
			PlayerMobile pm = mob as PlayerMobile;

			if ( pm == null )
				return; // sanity

			PlayerState pl = PlayerState.Find( pm );

            if ( pl != null && pl.IsLeaving )
				pm.SendLocalizedMessage( 1005051 ); // You cannot use the faction stone until you have finished quitting your current faction
			else if ( AlreadyHasCharInFaction( pm ) )
				pm.SendLocalizedMessage( 1005059 ); // You cannot join a faction because you already declared your allegiance with another character
			else if ( IsFactionBanned( mob ) )
				pm.SendLocalizedMessage( 1005052 ); // You are currently banned from the faction system
			else if ( !CanHandleInflux( 1 ) )
			{
				pm.SendLocalizedMessage( 1018031 ); // In the interest of faction stability, this faction declines to accept new members for now.
			}
			else
			{
				JoinAlone( mob );
			}
		}

		public bool IsCommander( Mobile mob )
		{
			if ( mob == null )
				return false;

			return ( mob.AccessLevel >= AccessLevel.GameMaster || mob == Commander );
		}

		public Faction()
		{
			m_State = new FactionState( this );
		}

		public override string ToString()
		{
			return m_Definition.FriendlyName;
		}

		public int CompareTo( object obj )
		{
			return m_Definition.Sort - ((Faction)obj).m_Definition.Sort;
		}

		public static bool CheckLeaveTimer( Mobile mob )
		{
			PlayerState pl = PlayerState.Find( mob );

			if ( pl == null || !pl.IsLeaving )
				return false;

			if ( (pl.Leaving + LeavePeriod) >= DateTime.Now )
				return false;

			mob.SendLocalizedMessage( 1005163 ); // You have now quit your faction

			pl.Faction.RemoveMember( mob );

			return true;
		}

		public static void Initialize()
		{
			EventSink.Login += new LoginEventHandler( EventSink_Login );
			EventSink.Logout += new LogoutEventHandler( EventSink_Logout );

			Timer.DelayCall( TimeSpan.FromMinutes( 1.0 ), TimeSpan.FromMinutes( 10.0 ), new TimerCallback( HandleAtrophy ) );

			Timer.DelayCall( TimeSpan.FromSeconds( 30.0 ), TimeSpan.FromSeconds( 30.0 ), new TimerCallback( ProcessTick ) );

			CommandSystem.Register( "FactionCommander", AccessLevel.Administrator, new CommandEventHandler( FactionCommander_OnCommand ) );
			CommandSystem.Register( "FactionItemReset", AccessLevel.Administrator, new CommandEventHandler( FactionItemReset_OnCommand ) );
			CommandSystem.Register( "FactionReset", AccessLevel.Administrator, new CommandEventHandler( FactionReset_OnCommand ) );
			CommandSystem.Register( "FactionTownReset", AccessLevel.Administrator, new CommandEventHandler( FactionTownReset_OnCommand ) );
            CommandSystem.Register("FactionMessage", AccessLevel.Player, new CommandEventHandler(MilitiaMessage_OnCommand));
		}
        public static void MilitiaMessage_OnCommand(CommandEventArgs e)
        {
            PlayerMobile player = e.Mobile as PlayerMobile;
            if (player != null && player.FactionPlayerState != null)
            {
                Faction militia = Find(player);
                if (militia.IsCommander(player))
                    militia.BeginBroadcast(player);
                else
                    player.SendMessage("Only faction commanders can send faction messages!");
            }


        }
		public static void FactionTownReset_OnCommand( CommandEventArgs e )
		{
			List<Town> towns = Town.Towns;

			for ( int i = 0; i < towns.Count; ++i )
			{
				towns[i].Silver = 0;
				towns[i].Militia = null;
			}

			List<Faction> factions = Faction.Factions;

			for ( int i = 0; i < factions.Count; ++i )
			{
				Faction f = factions[i];

				List<FactionItem> list = new List<FactionItem>( f.State.FactionItems );

				for ( int j = 0; j < list.Count; ++j )
				{
					FactionItem fi = list[j];

					if ( fi.Expiration == DateTime.MinValue )
						fi.Item.Delete();
					else
						fi.Detach();
				}
			}
		}

		public static void FactionReset_OnCommand( CommandEventArgs e )
		{
			List<Town> towns = Town.Towns;

			for ( int i = 0; i < towns.Count; ++i )
			{
				towns[i].Silver = 0;
				towns[i].Militia = null;
			}

			List<Faction> factions = Faction.Factions;

			for ( int i = 0; i < factions.Count; ++i )
			{
				Faction f = factions[i];

				List<PlayerState> playerStateList = new List<PlayerState>( f.Members );

				for( int j = 0; j < playerStateList.Count; ++j )
					f.RemoveMember( playerStateList[j].Mobile );

				List<FactionItem> factionItemList = new List<FactionItem>( f.State.FactionItems );

				for( int j = 0; j < factionItemList.Count; ++j )
				{
					FactionItem fi = (FactionItem)factionItemList[j];

					if ( fi.Expiration == DateTime.MinValue )
						fi.Item.Delete();
					else
						fi.Detach();
				}

				List<BaseFactionTrap> factionTrapList = new List<BaseFactionTrap>( f.Traps );

				for( int j = 0; j < factionTrapList.Count; ++j )
					factionTrapList[j].Delete();
			}
		}

		public static void FactionItemReset_OnCommand( CommandEventArgs e )
		{
			ArrayList pots = new ArrayList();

			foreach ( Item item in World.Items.Values )
			{
				if ( item is IFactionItem && !(item is HoodedShroudOfShadows) )
					pots.Add( item );
			}

			int[] hues = new int[Factions.Count * 2];

			for ( int i = 0; i < Factions.Count; ++i )
			{
				hues[0+(i*2)] = Factions[i].Definition.HuePrimary;
				hues[1+(i*2)] = Factions[i].Definition.HueSecondary;
			}

			int count = 0;

			for ( int i = 0; i < pots.Count; ++i )
			{
				Item item = (Item)pots[i];
				IFactionItem fci = (IFactionItem)item;

				if ( fci.FactionItemState != null || item.LootType != LootType.Blessed )
					continue;

				bool isHued = false;

				for ( int j = 0; j < hues.Length; ++j )
				{
					if ( item.Hue == hues[j] )
					{
						isHued = true;
						break;
					}
				}

				if ( isHued )
				{
					fci.FactionItemState = null;
					++count;
				}
			}

			e.Mobile.SendMessage( "{0} items reset", count );
		}

		public static void FactionCommander_OnCommand( CommandEventArgs e )
		{
			e.Mobile.SendMessage("Target a player to make them the faction commander.");
			e.Mobile.BeginTarget( -1, false, TargetFlags.None, new TargetCallback( FactionCommander_OnTarget ) );
		}

		public static void FactionCommander_OnTarget( Mobile from, object obj )
		{
			if ( obj is PlayerMobile )
			{
				Mobile targ = (Mobile)obj;
				PlayerState pl = PlayerState.Find( targ );
                PlayerCitizenshipState pcs = PlayerCitizenshipState.Find(targ);

                if (targ == from)
                {
                    from.SendMessage("You cannot appoint yourself as commander!");
                }
				else if ( pl != null  && pcs != null && pcs.Commonwealth.Militia == pl.Faction)
				{
					pl.Faction.Commander = targ;
                    pl.Faction.OwningCommonwealth.CanAssignCommander = false;
                    //DatabaseController.UpdateMilitiaCommander(targ, pl.Faction);
					from.SendMessage("You have appointed them as faction commander.");
				}
                else
                {
                    from.SendMessage("That player cannot be appointed as commander of your faction.");
                }
			}
			else
			{
				from.SendMessage("That would not make a very suitable faction commander!");
			}
		}

		public static void FactionKick_OnCommand( CommandEventArgs e )
		{
			e.Mobile.SendMessage( "Target a player to remove them from their faction." );
			e.Mobile.BeginTarget( -1, false, TargetFlags.None, new TargetCallback( FactionKick_OnTarget ) );
		}

		public static void FactionKick_OnTarget( Mobile from, object obj )
		{
			if ( obj is Mobile )
			{
				Mobile mob = (Mobile) obj;
				PlayerState pl = PlayerState.Find( (Mobile) mob );

				if ( pl != null )
				{
					pl.Faction.RemoveMember( mob );

					mob.SendMessage( "You have been kicked from your faction." );
					from.SendMessage( "They have been kicked from their faction." );
				}
				else
				{
					from.SendMessage( "They are not in a faction." );
				}
			}
			else
			{
				from.SendMessage( "That is not a player." );
			}
		}

		public static void ProcessTick()
		{
			
		}

		public static void HandleDeath( Mobile mob )
		{
			HandleDeath( mob, null );
		}

		#region Skill Loss
		public const double SkillLossFactor = 1.0 / 3;
		public static readonly TimeSpan SkillLossPeriod = TimeSpan.FromMinutes( FeatureList.Militias.SkillLossTimeInMinutes );

		private static Dictionary<Mobile, SkillLossContext> m_SkillLoss = new Dictionary<Mobile, SkillLossContext>();

		private class SkillLossContext
		{
			public Timer m_Timer;
			public List<SkillMod> m_Mods;
		}

		public static bool InSkillLoss( Mobile mob )
		{
			return m_SkillLoss.ContainsKey( mob );
		}

		private static double FindTwentyPercent(double amount)
        {
            double newAmount = (amount / 100.0) * 20.0;

            return newAmount;
        }

		public static void ApplyBountyHunterSkillLoss( Mobile mob )
		{
			if ( InSkillLoss( mob ) )
				return;

			SkillLossContext context = new SkillLossContext();
			m_SkillLoss[mob] = context;

			List<SkillMod> mods = context.m_Mods = new List<SkillMod>();

			for ( int i = 0; i < mob.Skills.Length; ++i )
			{
				Skill sk = mob.Skills[i];
				double baseValue = sk.Base;

				if ( baseValue > 0 )
				{
					SkillMod mod = new DefaultSkillMod( sk.SkillName, true, -(FindTwentyPercent(mob.Skills[SkillName.Swords].Value)) );

					mods.Add( mod );
					mob.AddSkillMod( mod );
				}
			}

			context.m_Timer = Timer.DelayCall( TimeSpan.FromMinutes( 15 ), new TimerStateCallback( ClearSkillLoss_Callback ), mob );
		}

		public static void ApplySkillLoss( Mobile mob )
		{
			if ( InSkillLoss( mob ) )
				return;

			SkillLossContext context = new SkillLossContext();
			m_SkillLoss[mob] = context;

			List<SkillMod> mods = context.m_Mods = new List<SkillMod>();

			for ( int i = 0; i < mob.Skills.Length; ++i )
			{
				Skill sk = mob.Skills[i];
				double baseValue = sk.Base;

				if ( baseValue > 0 )
				{
					SkillMod mod = new DefaultSkillMod( sk.SkillName, true, -(baseValue * SkillLossFactor) );

					mods.Add( mod );
					mob.AddSkillMod( mod );
				}
			}

			context.m_Timer = Timer.DelayCall( SkillLossPeriod, new TimerStateCallback( ClearSkillLoss_Callback ), mob );
		}

		private static void ClearSkillLoss_Callback( object state )
		{
			ClearSkillLoss( (Mobile) state );
		}

		public static bool ClearSkillLoss( Mobile mob )
		{
			SkillLossContext context;

			if ( !m_SkillLoss.TryGetValue( mob, out context ) )
				return false;

			m_SkillLoss.Remove( mob );

			List<SkillMod> mods = context.m_Mods;

			for ( int i = 0; i < mods.Count; ++i )
				mob.RemoveSkillMod( mods[i] );

			context.m_Timer.Stop();

			return true;
		}
		#endregion

		public int AwardSilver( Mobile mob, int silver )
		{
			if ( silver <= 0 )
				return 0;

			int tithed = ( silver * SilverTax ) / 100;

			Silver += tithed;

			silver = silver - tithed;

			if ( silver > 0 )
				mob.AddToBackpack( new Silver( silver ) );

			return silver;
		}
        private void AwardSilverToEntireMilitia(int silver, string message)
        {
            foreach (PlayerState player in Members)
            {
                if (player.Mobile.Map == Map.Internal)
                    continue;

                AwardSilver(player.Mobile, silver);
            }
            Broadcast(message);
        }
        public void AwardSilverForHeroKidnap(TownHero hero)
        {
            AwardSilverToEntireMilitia(FeatureList.Militias.SilverRewardForHeroKidnap, "You have been awarded silver for successfully kidnapping a town hero!");
        }
        public void AwardSilverForHeroCapture(TownHero hero)
        {
            AwardSilverToEntireMilitia(FeatureList.Militias.SilverRewardForHeroCapture, "You have been awarded silver for successfully capturing a town hero!");
        }

        public void AwardSilverForHeroRescue(TownHero hero)
        {
            AwardSilverToEntireMilitia(FeatureList.Militias.SilverRewardForHeroRescue, "You have been awarded silver for successfully rescuing your town hero!");
        }
        public void AwardSilverForHeroRecapture(TownHero hero)
        {
            if (hero != OwningCommonwealth.Hero)
                return;

            AwardSilverToEntireMilitia(FeatureList.Militias.SilverRewardForHeroRecapture, "You have been awarded silver for successfully recapturing your town hero!");
        }
        private void AwardSilverToPlayersInRegion(List<Mobile> playersInRegion, int reward, string message)
        {
            for (int i = 0; i < playersInRegion.Count; i++)
            {
                PlayerState playerState = PlayerState.Find(playersInRegion[i]);
                if (playerState != null && playerState.Faction == this)
                {
                    AwardSilver(playersInRegion[i], reward);
                    playersInRegion[i].SendMessage(message);
                }
            }
        }

      
        public virtual int MaximumTraps{ get{ return FeatureList.Militias.MaximumTraps; } }

		public List<BaseFactionTrap> Traps
		{
			get{ return m_State.Traps; }
			set{ m_State.Traps = value; }
		}

		public const int StabilityFactor = 300; // 300% greater (3 times) than smallest faction
		public const int StabilityActivation = 200; // Stablity code goes into effect when largest faction has > 200 people

		public static Faction FindSmallestFaction()
		{
			List<Faction> factions = Factions;
			Faction smallest = null;

			for ( int i = 0; i < factions.Count; ++i )
			{
				Faction faction = factions[i];

				if ( smallest == null || faction.Members.Count < smallest.Members.Count )
					smallest = faction;
			}

			return smallest;
		}

		public static bool StabilityActive()
		{
			List<Faction> factions = Factions;

			for ( int i = 0; i < factions.Count; ++i )
			{
				Faction faction = factions[i];

				if ( faction.Members.Count > StabilityActivation )
					return true;
			}

			return false;
		}

		public bool CanHandleInflux( int influx )
		{
			if( !StabilityActive())
				return true;

			Faction smallest = FindSmallestFaction();

			if ( smallest == null )
				return true; // sanity

			if ( StabilityFactor > 0 && (((this.Members.Count + influx) * 100) / StabilityFactor) > smallest.Members.Count )
				return false;

			return true;
		}

		public static void HandleDeath( Mobile victim, Mobile killer )
		{
			if ( killer == null )
				killer = victim.FindMostRecentDamager( true );

			PlayerState killerState = PlayerState.Find( killer );

			Container pack = victim.Backpack;

			if ( killerState == null
                || (victim.TeamFlags > 1 && killer.TeamFlags > 1)) // make sure they aren't in some kind of team event
				return;

			if ( victim is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)victim;
				Faction victimFaction = bc.FactionAllegiance;

				if ( bc.Map == Faction.Facet && victimFaction != null && killerState.Faction != victimFaction )
				{
					int silver = killerState.Faction.AwardSilver( killer, bc.FactionSilverWorth );

					if ( silver > 0 )
						killer.SendLocalizedMessage( 1042748, silver.ToString( "N0" ) ); // Thou hast earned ~1_AMOUNT~ silver for vanquishing the vile creature.
				}

				#region Ethics
				if ( bc.Map == Faction.Facet && bc.GetEthicAllegiance( killer ) == BaseCreature.Allegiance.Enemy )
				{
					Ethics.Player killerEPL = Ethics.Player.Find( killer );

					if ( killerEPL != null && ( 100 - killerEPL.Power ) > Utility.Random( 100 ) )
					{
						++killerEPL.Power;
						++killerEPL.History;
					}
				}
				#endregion

				return;
			}

			PlayerState victimState = PlayerState.Find( victim );

			if ( victimState == null )
				return;

			if ( killer == victim || killerState.Faction != victimState.Faction )
				ApplySkillLoss( victim );

			if ( killerState.Faction != victimState.Faction )
			{
				if ( victimState.KillPoints <= -6 )
				{
					killer.SendLocalizedMessage( 501693 ); // This victim is not worth enough to get kill points from. 

					#region Ethics
					Ethics.Player killerEPL = Ethics.Player.Find( killer );
					Ethics.Player victimEPL = Ethics.Player.Find( victim );

					if ( killerEPL != null && victimEPL != null && victimEPL.Power > 0 && victimState.CanGiveSilverTo( killer ) )
					{
						int powerTransfer = Math.Max( 1, victimEPL.Power / 5 );

						if ( powerTransfer > ( 100 - killerEPL.Power ) )
							powerTransfer = 100 - killerEPL.Power;

						if ( powerTransfer > 0 )
						{
							victimEPL.Power -= ( powerTransfer + 1 ) / 2;
							killerEPL.Power += powerTransfer;

							killerEPL.History += powerTransfer;

							victimState.OnGivenSilverTo( killer );
						}
					}
					#endregion
				}
				else
				{
					int award = Math.Max( victimState.KillPoints / 10 + 1, 1 );

					if ( award > 10 )
						award = 10;

					if ( victimState.CanGiveSilverTo( killer ) )
					{
                        victimState.IsActive = true;

                        if (1 > Utility.Random(3))
                            killerState.IsActive = true;

                        int silver = 0;

                        silver = killerState.Faction.AwardSilver(killer, FeatureList.Militias.SilverRewardForKill);

                        if (silver > 0)
                            killer.SendLocalizedMessage(1042736, String.Format("{0:N0} silver\t{1}", silver, victim.Name)); // You have earned ~1_SILVER_AMOUNT~ pieces for vanquishing ~2_PLAYER_NAME~!


						victimState.KillPoints -= award;
						killerState.KillPoints += award;

                        //Database stuff
                        PlayerMobile victimPM = victim as PlayerMobile;
                        PlayerMobile killerPM = killer as PlayerMobile;

                        if (victimPM != null)
                        {
                            victimPM.MilitiaDeathsThisSession++;
                        }
                        if (killerPM != null)
                        {
                            killerPM.MilitiaKillsThisSession++;
                            killerPM.SilverEarnedThisSession += award;
                        }
                      
						int offset = ( award != 1 ? 0 : 2 ); // for pluralization

						string args = String.Format( "{0}\t{1}\t{2}", award, victim.Name, killer.Name );

						killer.SendLocalizedMessage( 1042737 + offset, args ); // Thou hast been honored with ~1_KILL_POINTS~ kill point(s) for vanquishing ~2_DEAD_PLAYER~!
						victim.SendLocalizedMessage( 1042738 + offset, args ); // Thou has lost ~1_KILL_POINTS~ kill point(s) to ~3_ATTACKER_NAME~ for being vanquished!

						#region Ethics
						Ethics.Player killerEPL = Ethics.Player.Find( killer );
						Ethics.Player victimEPL = Ethics.Player.Find( victim );

						if ( killerEPL != null && victimEPL != null && victimEPL.Power > 0 )
						{
							int powerTransfer = Math.Max( 1, victimEPL.Power / 5 );

							if ( powerTransfer > ( 100 - killerEPL.Power ) )
								powerTransfer = 100 - killerEPL.Power;

							if ( powerTransfer > 0 )
							{
								victimEPL.Power -= ( powerTransfer + 1 ) / 2;
								killerEPL.Power += powerTransfer;

								killerEPL.History += powerTransfer;
							}
						}
						#endregion

						victimState.OnGivenSilverTo( killer );
					}
					else
					{
						killer.SendLocalizedMessage( 1042231 ); // You have recently defeated this enemy and thus their death brings you no honor.
					}
				}
			}
		}

		private static void EventSink_Logout( LogoutEventArgs e )
		{

		}

		private static void EventSink_Login( LoginEventArgs e )
		{
			Mobile mob = e.Mobile;

			CheckLeaveTimer( mob );
		}

		public static readonly Map Facet = Map.Felucca;

		public static void WriteReference( GenericWriter writer, Faction fact )
		{
			int idx = Factions.IndexOf( fact );

			writer.WriteEncodedInt( (int) (idx + 1) );
		}

		public static List<Faction> Factions{ get{ return Reflector.Factions; } }

		public static Faction ReadReference( GenericReader reader )
		{
			int idx = reader.ReadEncodedInt() - 1;

			if ( idx >= 0 && idx < Factions.Count )
				return Factions[idx];

			return null;
		}

		public static Faction Find( Mobile mob )
		{
			return Find( mob, false, false );
		}

		public static Faction Find( Mobile mob, bool inherit )
		{
			return Find( mob, inherit, false );
		}

		public static Faction Find( Mobile mob, bool inherit, bool creatureAllegiances )
		{
			PlayerState pl = PlayerState.Find( mob );

			if ( pl != null )
				return pl.Faction;

			if ( inherit && mob is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)mob;

				if ( bc.Controlled )
					return Find( bc.ControlMaster, false );
				else if ( bc.Summoned )
					return Find( bc.SummonMaster, false );
				else if ( creatureAllegiances && mob is BaseFactionGuard )
					return ((BaseFactionGuard)mob).Faction;
				else if ( creatureAllegiances )
					return bc.FactionAllegiance;
			}

			return null;
		}
        public static Faction Find(string townName)
        {
            foreach (Faction faction in Faction.Factions)
            {
                if (faction.Definition.TownshipName == townName)
                    return faction;
            }
            return null;
        }
		public static Faction Parse( string name )
		{
			List<Faction> factions = Factions;

			for ( int i = 0; i < factions.Count; ++i )
			{
				Faction faction = factions[i];

				if ( Insensitive.Equals( faction.Definition.FriendlyName, name ) )
					return faction;
			}

			return null;
		}
	}

	public enum FactionKickType
	{
		Kick,
		Ban,
		Unban
	}

	public class FactionKickCommand : BaseCommand
	{
		private FactionKickType m_KickType;

		public FactionKickCommand( FactionKickType kickType )
		{
			m_KickType = kickType;

			AccessLevel = AccessLevel.GameMaster;
			Supports = CommandSupport.AllMobiles;
			ObjectTypes = ObjectTypes.Mobiles;

			switch ( m_KickType )
			{
				case FactionKickType.Kick:
				{
					Commands = new string[]{ "FactionKick" };
					Usage = "FactionKick";
					Description = "Kicks the targeted player out of his current faction. This does not prevent them from rejoining.";
					break;
				}
				case FactionKickType.Ban:
				{
					Commands = new string[]{ "FactionBan" };
					Usage = "FactionBan";
					Description = "Bans the account of a targeted player from joining factions. All players on the account are removed from their current faction, if any.";
					break;
				}
				case FactionKickType.Unban:
				{
					Commands = new string[]{ "FactionUnban" };
					Usage = "FactionUnban";
					Description = "Unbans the account of a targeted player from joining factions.";
					break;
				}
			}
		}

		public override void Execute( CommandEventArgs e, object obj )
		{
			Mobile mob = (Mobile)obj;

			switch ( m_KickType )
			{
				case FactionKickType.Kick:
				{
					PlayerState pl = PlayerState.Find( mob );

					if ( pl != null )
					{
						pl.Faction.RemoveMember( mob );
						mob.SendMessage( "You have been kicked from your faction." );
						AddResponse( "They have been kicked from their faction." );
					}
					else
					{
						LogFailure( "They are not in a faction." );
					}

					break;
				}
				case FactionKickType.Ban:
				{
					Account acct = mob.Account as Account;

					if ( acct != null )
					{
						if ( acct.GetTag( "FactionBanned" ) == null )
						{
							acct.SetTag( "FactionBanned", "true" );
							AddResponse( "The account has been banned from joining factions." );
						}
						else
						{
							AddResponse( "The account is already banned from joining factions." );
						}

						for ( int i = 0; i < acct.Length; ++i )
						{
							mob = acct[i];

							if ( mob != null )
							{
								PlayerState pl = PlayerState.Find( mob );

								if ( pl != null )
								{
									pl.Faction.RemoveMember( mob );
									mob.SendMessage( "You have been kicked from your faction." );
									AddResponse( "They have been kicked from their faction." );
								}
							}
						}
					}
					else
					{
						LogFailure( "They have no assigned account." );
					}

					break;
				}
				case FactionKickType.Unban:
				{
					Account acct = mob.Account as Account;

					if ( acct != null )
					{
						if ( acct.GetTag( "FactionBanned" ) == null )
						{
							AddResponse( "The account is not already banned from joining factions." );
						}
						else
						{
							acct.RemoveTag( "FactionBanned" );
							AddResponse( "The account may now freely join factions." );
						}
					}
					else
					{
						LogFailure( "They have no assigned account." );
					}

					break;
				}
			}
		}
	}
}