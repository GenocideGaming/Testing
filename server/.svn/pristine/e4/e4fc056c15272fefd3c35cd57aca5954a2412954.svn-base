using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Guilds;
using Server.Multis;
using Server.Mobiles;
using Server.Engines.PartySystem;
using Server.Factions;
using Server.Scripts.Custom.Citizenship;

namespace Server.Misc
{
	public class NotorietyHandlers
	{
		public static void Initialize()
		{
            Notoriety.Hues[Notoriety.Innocent]		= 0x59;
			Notoriety.Hues[Notoriety.Ally]			= 0x3F;
			Notoriety.Hues[Notoriety.CanBeAttacked]	= 0x3B2;
			Notoriety.Hues[Notoriety.Criminal]		= 0x3B2;
			Notoriety.Hues[Notoriety.Enemy]			= 0x90;
			Notoriety.Hues[Notoriety.Murderer]		= 0x22;
			Notoriety.Hues[Notoriety.Invulnerable]	= 0x35;
            //Notoriety.Hues[Notoriety.Parole]        = 0x20;  // these only change mobile's name color, not highlight color
            //Notoriety.Hues[Notoriety.Outcast]       = 0x21;  // highlight color depends on the noto entry# (e.g. outcast is 9),
                                                             // the rest (as far as I could tell for 0-20) are a BRIGHT grey
                                                             // although you can get ethereal highlight
			Notoriety.Handler = new NotorietyHandler( MobileNotoriety );

			Mobile.AllowBeneficialHandler = new AllowBeneficialHandler( Mobile_AllowBeneficial );
			Mobile.AllowHarmfulHandler = new AllowHarmfulHandler( Mobile_AllowHarmful );
		}

		private enum GuildStatus { None, Peaceful, Waring }

		private static GuildStatus GetGuildStatus( Mobile m )
		{
			if( m.Guild == null )
				return GuildStatus.None;
			else if( ((Guild)m.Guild).Enemies.Count == 0 && m.Guild.Type == GuildType.Regular )
				return GuildStatus.Peaceful;

			return GuildStatus.Waring;
		}

		private static bool CheckBeneficialStatus( GuildStatus from, GuildStatus target )
		{
			if( from == GuildStatus.Waring || target == GuildStatus.Waring )
				return false;

			return true;
		}

		/*private static bool CheckHarmfulStatus( GuildStatus from, GuildStatus target )
		{
			if ( from == GuildStatus.Waring && target == GuildStatus.Waring )
				return true;

			return false;
		}*/

		public static bool Mobile_AllowBeneficial( Mobile from, Mobile target )
		{
			if( from == null || target == null || from.AccessLevel > AccessLevel.Player || target.AccessLevel > AccessLevel.Player )
				return true;

			Map map = from.Map;

			#region Relationship between members of militias and townships
		    PlayerMobile fromPM = from as PlayerMobile;
            Faction targetFaction = Faction.Find( target, true );

            // check if they are on the same non-Player team (see AITeamList).. player team is 1 (all players are on that team
            // so if (from.TeamFlags & target.TeamFlags) is > 1, then it means they have a team in common beyond the player team
            // which means they can heal each other, regardless of militia status
            if ((from.TeamFlags & target.TeamFlags) > 1) // same team
            {
                if ((int)AITeamList.NotoType < (int)AITeamList.NotoTypeEnum.ALLIES_NORMAL_NO_HEAL_MILITIA)
                {
                    return true;
                }
            }
            else if ((from.TeamFlags & AITeamList.CustomTeamFlags) > 1 && (target.TeamFlags & AITeamList.CustomTeamFlags) > 1) // separate teams
            {
                return false;
            }
            
            
            if (fromPM != null && fromPM.CitizenshipPlayerState != null && targetFaction != null)
            {
                if (fromPM.CitizenshipPlayerState.Commonwealth == targetFaction.OwningCommonwealth)
                    return true;
            }

            if ((!Core.ML || map == Faction.Facet) && targetFaction != null)
            {
                if (Faction.Find(from, true) != targetFaction)
                    return false;
            }
			#endregion

			if( map != null && (map.Rules & MapRules.BeneficialRestrictions) == 0 )
				return true; // In felucca, anything goes

			if( !from.Player )
				return true; // NPCs have no restrictions

			if( target is BaseCreature && !((BaseCreature)target).Controlled )
				return false; // Players cannot heal uncontrolled mobiles

			Guild fromGuild = from.Guild as Guild;
			Guild targetGuild = target.Guild as Guild;

			if( fromGuild != null && targetGuild != null && (targetGuild == fromGuild || fromGuild.IsAlly( targetGuild )) )
				return true; // Guild members can be beneficial

			return CheckBeneficialStatus( GetGuildStatus( from ), GetGuildStatus( target ) );
		}

		public static bool Mobile_AllowHarmful( Mobile from, Mobile target )
		{
			if( from == null || target == null || from.AccessLevel > AccessLevel.Player || target.AccessLevel > AccessLevel.Player )
				return true;

			Map map = from.Map;

            if (((from.TeamFlags & target.TeamFlags) & AITeamList.CustomTeamFlags) > 1) // same team
            {
                if (AITeamList.TeamHarm == false)
                {
                    return false;
                }
            }
                
            if (from is PlayerMobile && target is PlayerMobile)
            {
                PlayerMobile pm = (PlayerMobile)from;
                if (pm.PlayerMurdererStatus == PlayerMobile.MurdererStatus.Parole
                    && MobileNotoriety(from, target) == Notoriety.Innocent)
                {
                    return false;
                }
            }

			if( map != null && (map.Rules & MapRules.HarmfulRestrictions) == 0 )
				return true; // In felucca, anything goes

			BaseCreature bc = from as BaseCreature;

			if( !from.Player && !(bc != null && bc.GetMaster() != null && bc.GetMaster().AccessLevel == AccessLevel.Player ) )
			{
				return true; // Uncontrolled NPCs are only restricted by the young system
			}

			Guild fromGuild = GetGuildFor( from.Guild as Guild, from );
			Guild targetGuild = GetGuildFor( target.Guild as Guild, target );

			if( fromGuild != null && targetGuild != null && (fromGuild == targetGuild || fromGuild.IsAlly( targetGuild ) || fromGuild.IsEnemy( targetGuild )) )
				return true; // Guild allies or enemies can be harmful

			if( target is BaseCreature && (((BaseCreature)target).Controlled || (((BaseCreature)target).Summoned && from != ((BaseCreature)target).SummonMaster)) )
				return false; // Cannot harm other controlled mobiles

			if( target.Player )
				return false; // Cannot harm other players

			if( !(target is BaseCreature && ((BaseCreature)target).InitialInnocent) )
			{
				if( Notoriety.Compute( from, target ) == Notoriety.Innocent )
					return false; // Cannot harm innocent mobiles
			}

			return true;
		}

		public static Guild GetGuildFor( Guild def, Mobile m )
		{
			Guild g = def;

			BaseCreature c = m as BaseCreature;

            if (c != null && c.Controlled && c.ControlMaster != null && !(c is BaseEscortable))
			{
				c.DisplayGuildTitle = false;

				if( c.Map != Map.Internal && (Core.AOS || Guild.NewGuildSystem || c.ControlOrder == OrderType.Attack || c.ControlOrder == OrderType.Guard) )
					g = (Guild)(c.Guild = c.ControlMaster.Guild);
				else if( c.Map == Map.Internal || c.ControlMaster.Guild == null )
					g = (Guild)(c.Guild = null);
			}

			return g;
		}

		public static int CorpseNotoriety( Mobile source, Corpse target )
		{
			if( target.AccessLevel > AccessLevel.Player )
				return Notoriety.CanBeAttacked;

			Body body = (Body)target.Amount;

            Mobile mobileOwner = target.Owner as Mobile;
            if (mobileOwner != null)
            {
                if ((source.TeamFlags & mobileOwner.TeamFlags) > 1) // same team
                {
                    if (AITeamList.NotoType == AITeamList.NotoTypeEnum.ALLIES_GREEN)
                    {
                        return Notoriety.Ally;
                    }
                }
                else if ((source.TeamFlags & AITeamList.CustomTeamFlags) > 1 && (mobileOwner.TeamFlags & AITeamList.CustomTeamFlags) > 1) // they are on different custom teams
                { // doesn't work for mobs, only players, b/c mobs reset their teamflags when control master is set to null!
                    return Notoriety.Enemy;
                }
            }

			BaseCreature cretOwner = target.Owner as BaseCreature;

			if( cretOwner != null )
			{                
                Guild sourceGuild = GetGuildFor( source.Guild as Guild, source );
				Guild targetGuild = GetGuildFor( target.Guild as Guild, target.Owner );

				if( sourceGuild != null && targetGuild != null )
				{
					if( sourceGuild == targetGuild || sourceGuild.IsAlly( targetGuild ) )
						return Notoriety.Ally;
					else if( sourceGuild.IsEnemy( targetGuild ) )
						return Notoriety.Enemy;
				}

				Faction srcFaction = Faction.Find( source, true, true );
				Faction trgFaction = Faction.Find( target.Owner, true, true );

				if( srcFaction != null && trgFaction != null && srcFaction != trgFaction && source.Map == Faction.Facet )
					return Notoriety.Enemy;

				if( CheckHouseFlag( source, target.Owner, target.Location, target.Map ) )
					return Notoriety.CanBeAttacked;

				int actual = Notoriety.CanBeAttacked;

				if( target.Kills >= 5 || (body.IsMonster && IsSummoned( target.Owner as BaseCreature )) || (target.Owner is BaseCreature && (((BaseCreature)target.Owner).AlwaysMurderer || ((BaseCreature)target.Owner).IsAnimatedDead)) )
					actual = Notoriety.Murderer;

				if( DateTime.Now >= (target.TimeOfDeath + Corpse.MonsterLootRightSacrifice) )
					return actual;

				Party sourceParty = Party.Get( source );

				List<Mobile> list = target.Aggressors;

				for( int i = 0; i < list.Count; ++i )
				{
					if( list[i] == source || (sourceParty != null && Party.Get( list[i] ) == sourceParty) )
						return actual;
				}

                if (list.Count == 0)
                    return Notoriety.CanBeAttacked;

				return Notoriety.Innocent;
			}
			else
			{
                if (source.Kills >= 5 && target.Owner is PlayerMobile && ((PlayerMobile)target.Owner).BountyHunter)
                    return Notoriety.Enemy;

				if( target.Kills >= 5 || (body.IsMonster && IsSummoned( target.Owner as BaseCreature )) || (target.Owner is BaseCreature && (((BaseCreature)target.Owner).AlwaysMurderer || ((BaseCreature)target.Owner).IsAnimatedDead)) )
					return Notoriety.Murderer;

				if (target.Criminal && target.Map != null && ((target.Map.Rules & MapRules.HarmfulRestrictions) == 0))
					return Notoriety.Criminal;

				Guild sourceGuild = GetGuildFor( source.Guild as Guild, source );
				Guild targetGuild = GetGuildFor( target.Guild as Guild, target.Owner );

				if( sourceGuild != null && targetGuild != null )
				{
					if( sourceGuild == targetGuild || sourceGuild.IsAlly( targetGuild ) )
						return Notoriety.Ally;
					else if( sourceGuild.IsEnemy( targetGuild ) )
						return Notoriety.Enemy;
				}

				Faction srcFaction = Faction.Find( source, true, true );
				Faction trgFaction = Faction.Find( target.Owner, true, true );

				if( srcFaction != null && trgFaction != null && srcFaction != trgFaction && source.Map == Faction.Facet )
				{
                    return Notoriety.Enemy;
				}

				if( target.Owner != null && target.Owner is BaseCreature && ((BaseCreature)target.Owner).AlwaysAttackable )
					return Notoriety.CanBeAttacked;

				if( CheckHouseFlag( source, target.Owner, target.Location, target.Map ) )
					return Notoriety.CanBeAttacked;

				if( !(target.Owner is PlayerMobile) && !IsPet( target.Owner as BaseCreature ) )
					return Notoriety.CanBeAttacked;

				List<Mobile> list = target.Aggressors;

				for( int i = 0; i < list.Count; ++i )
				{
					if( list[i] == source )
						return Notoriety.CanBeAttacked;
				}

                if (target.IsBones)
                {
                    return Notoriety.CanBeAttacked;
                }

				return Notoriety.Innocent;
			}
		}

        public static int MobileNotoriety(Mobile source, Mobile target)
        {
            if (source is BaseCreature)
            {
                BaseCreature bc = (BaseCreature)source;
                if ((IsPet(bc) && bc.ControlMaster != null) || (bc.Summoned && bc.SummonMaster != null))
                {
                    // first check whether the pet does not see the target as innocent, in which case just
                    // use the pet's Noto... otherwise, check use the control master's noto
                    int petNoto = MobileNotoriety(source, target, false);
                    if (petNoto != Notoriety.Innocent)
                    {
                        return petNoto;
                    }
                    return MobileNotoriety(source, target, true); // will return owner's notoriety
                }
            }
            return MobileNotoriety(source, target, false);
        }

        public static int MobileNotoriety(Mobile source, Mobile target, Boolean sourceIsPet)
        {
            if (Core.AOS && (target.Blessed || (target is BaseVendor && ((BaseVendor)target).IsInvulnerable) || target is PlayerVendor || target is TownCrier))
                return Notoriety.Invulnerable;

            if (target.AccessLevel > AccessLevel.Player)
                return Notoriety.CanBeAttacked;

            if (sourceIsPet)
            {
                BaseCreature bc = (BaseCreature)source;
                if ((IsPet(bc) && bc.ControlMaster != null))
                {
                    return MobileNotoriety(bc.ControlMaster, target, false);
                }
                if ((bc.Summoned && bc.SummonMaster != null))
                {
                    return MobileNotoriety(bc.SummonMaster, target, false);
                }
            }
            bool sameTeam = false;
            if ((source.TeamFlags & target.TeamFlags) > 1) // same team
            {
                if (AITeamList.NotoType == AITeamList.NotoTypeEnum.ALLIES_GREEN)
                {
                    return Notoriety.Ally;
                }
                sameTeam = true;
            }
            else if ((source.TeamFlags & AITeamList.CustomTeamFlags) > 1 && (target.TeamFlags & AITeamList.CustomTeamFlags) > 1) // they are on different custom teams
            {
                return Notoriety.Enemy;
            }
            // otherwise use normal notoriety

            if (source.Player && !target.Player && source is PlayerMobile && target is BaseCreature)
            {
                BaseCreature bc = (BaseCreature)target;

                Mobile master = bc.GetMaster();

                if (master != null && master.AccessLevel > AccessLevel.Player)
                    return Notoriety.CanBeAttacked;

                master = bc.ControlMaster;

                if (Core.ML && master != null)
                {
                    if ((source == master && CheckAggressor(target.Aggressors, source)) || (CheckAggressor(source.Aggressors, bc)))
                        return Notoriety.CanBeAttacked;
                    else
                        return MobileNotoriety(source, master);
                }

                if (!bc.Summoned && !bc.Controlled && ((PlayerMobile)source).EnemyOfOneType == target.GetType())
                    return Notoriety.Enemy;
            }

            if (source.Kills >= 5 && target is PlayerMobile && ((PlayerMobile)target).BountyHunter)
                return Notoriety.Enemy;

            if (target.Kills >= 5 || (target.Body.IsMonster && IsSummoned(target as BaseCreature) && !(target is BaseFamiliar) && !(target is Golem)) || (target is BaseCreature && (((BaseCreature)target).AlwaysMurderer || ((BaseCreature)target).IsAnimatedDead)))
                return Notoriety.Murderer;

            if (target.Criminal)
                return Notoriety.Criminal;

            if (!sameTeam
                || AITeamList.NotoType == AITeamList.NotoTypeEnum.ALLIES_NORMAL_NO_HEAL_MILITIA
                || AITeamList.NotoType == AITeamList.NotoTypeEnum.ALLIES_NORMAL_ALLOW_HEAL_MILITIA)
            {
                // only make warring guilds / militias noto as enemies if they aren't on the same
                // team and we aren't on AITeamList.NotoTypeEnum.ALLIES_NORMAL_IGNORE_MILITIA
                Guild sourceGuild = GetGuildFor(source.Guild as Guild, source);
                Guild targetGuild = GetGuildFor(target.Guild as Guild, target);

                if (sourceGuild != null && targetGuild != null)
                {
                    if (sourceGuild == targetGuild || sourceGuild.IsAlly(targetGuild))
                        return Notoriety.Ally;
                    else if (sourceGuild.IsEnemy(targetGuild))
                        return Notoriety.Enemy;
                }

                Faction srcFaction = Faction.Find(source, true, true);
                Faction trgFaction = Faction.Find(target, true, true);
                PlayerMobile targetPM = target as PlayerMobile;

                if (targetPM != null && targetPM.CitizenshipPlayerState != null && srcFaction != null &&
                    srcFaction.OwningCommonwealth != targetPM.CitizenshipPlayerState.Commonwealth &&
                    target.AssistedOwnMilitia)
                    return Notoriety.Enemy;

                if (srcFaction != null && trgFaction != null && srcFaction != trgFaction && source.Map == Faction.Facet)
                    return Notoriety.Enemy;
            }
            else if (AITeamList.NotoType == AITeamList.NotoTypeEnum.ALLIES_NORMAL_IGNORE_MILITIA)
            {
                // they must be on same team to get in here... make guildies green to each other
                // but don't make enemies orange
                Guild sourceGuild = GetGuildFor(source.Guild as Guild, source);
                Guild targetGuild = GetGuildFor(target.Guild as Guild, target);

                if (sourceGuild != null && targetGuild != null)
                {
                    if (sourceGuild == targetGuild || sourceGuild.IsAlly(targetGuild))
                        return Notoriety.Ally;
                }
            }

            if (SkillHandlers.Stealing.ClassicMode && target is PlayerMobile && ((PlayerMobile)target).PermaFlags.Contains(source))
                return Notoriety.CanBeAttacked;

            if (target is BaseCreature && ((BaseCreature)target).AlwaysAttackable)
                return Notoriety.CanBeAttacked;

            if (CheckHouseFlag(source, target, target.Location, target.Map))
                return Notoriety.CanBeAttacked;

            if (!(target is BaseCreature && ((BaseCreature)target).InitialInnocent))   //If Target is NOT A baseCreature, OR it's a BC and the BC is initial innocent...
            {
                if (!target.Body.IsHuman && !target.Body.IsGhost && !IsPet(target as BaseCreature) && !(target is PlayerMobile) || !Core.ML && !target.CanBeginAction(typeof(Server.Spells.Seventh.PolymorphSpell)))
                {
                    if (!(target is BaseCreature) || !((BaseCreature)target).InnocentDefault) 
                    {
                        return Notoriety.CanBeAttacked;
                    }                   
                }
                
            }

            if (CheckAggressor(source.Aggressors, target))
                return Notoriety.CanBeAttacked;

            if (CheckAggressed(source.Aggressed, target))
                return Notoriety.CanBeAttacked;

            if (target is BaseCreature)
            {
                BaseCreature bc = (BaseCreature)target;

                if (bc.Controlled && bc.ControlMaster == source && bc.CheckControlChance(source))
                {
                    return Notoriety.Ally;
                }

                else if (bc.Controlled && bc.ControlMaster == source)
                {
                    return Notoriety.Innocent;
                }
            }

            if (source is BaseCreature)
            {
                BaseCreature bc = (BaseCreature)source;

                Mobile master = bc.GetMaster();
                if (master != null)
                    if (CheckAggressor(master.Aggressors, target) || MobileNotoriety(master, target) == Notoriety.CanBeAttacked)
                        return Notoriety.CanBeAttacked;
            }

            return Notoriety.Innocent;
        }

		public static bool CheckHouseFlag( Mobile from, Mobile m, Point3D p, Map map )
		{
			BaseHouse house = BaseHouse.FindHouseAt( p, map, 16 );

			if( house == null || house.Public || !house.IsFriend( from ) )
				return false;

			if( m != null && house.IsFriend( m ) )
				return false;

			BaseCreature c = m as BaseCreature;

			if( c != null && !c.Deleted && c.Controlled && c.ControlMaster != null )
				return !house.IsFriend( c.ControlMaster );

			return true;
		}

		public static bool IsPet( BaseCreature c )
		{
			return (c != null && c.Controlled);
		}

		public static bool IsSummoned( BaseCreature c )
		{
			return (c != null && /*c.Controlled &&*/ c.Summoned);
		}

		public static bool CheckAggressor( List<AggressorInfo> list, Mobile target )
		{
            for (int i = 0; i < list.Count; ++i)
                if (list[i].Attacker == target)
                    return true;
			return false;
		}

		public static bool CheckAggressed( List<AggressorInfo> list, Mobile target )
		{
			for( int i = 0; i < list.Count; ++i )
			{
				AggressorInfo info = list[i];
				if( !info.CriminalAggression && info.Defender == target )
					return true;
			}

			return false;
		}
	}
}