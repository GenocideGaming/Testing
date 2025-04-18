using System;
using Server.Commands;
using System.Collections.Generic;
using Server.Scripts.Custom;

namespace Server.Factions
{
	public class Generator
	{
		public static void Initialize()
		{
			CommandSystem.Register( "GenerateFactions", AccessLevel.Administrator, new CommandEventHandler( GenerateFactions_OnCommand ) );
		}

		public static void GenerateFactions_OnCommand( CommandEventArgs e )
		{
			new FactionPersistance();

          
			List<Faction> factions = Faction.Factions;

			foreach ( Faction faction in factions )
				Generate( faction );

			List<Town> towns = Town.Towns;

            Console.WriteLine("Factions have been generated.");
			/*foreach ( Town town in towns )
				Generate( town ); */
		}
      
		public static void Generate( Town town )
		{
			Map facet = Faction.Facet;

			TownDefinition def = town.Definition;

			if ( !CheckExistance( def.TownStone, facet, typeof( TownStone ) ) )
				new TownStone( town ).MoveToWorld( def.TownStone, facet );
		}

		public static void Generate( Faction faction )
		{
			Map facet = Faction.Facet;

			List<Town> towns = Town.Towns;
            Console.WriteLine("Checking if faction stone exists for " + faction.Definition.Name);
            if (!CheckExistance(faction.Definition.MilitiaStonePosition, facet, typeof(FactionStone)))
            {
                Console.WriteLine("Creating new faction stone for " + faction.Definition.Name);
                new FactionStone(faction).MoveToWorld(faction.Definition.MilitiaStonePosition, facet);
            }
		}

		private static bool CheckExistance( Point3D loc, Map facet, Type type )
		{
			foreach ( Item item in facet.GetItemsInRange( loc, 0 ) )
			{
				if ( type.IsAssignableFrom( item.GetType() ) )
					return true;
			}

			return false;
		}
	}
}