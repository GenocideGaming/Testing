using System;
using System.Collections.Generic;
using Server.Commands;
using Server.Mobiles;

namespace Server.Factions
{
	public class FactionPersistance : Item
	{
		private static FactionPersistance m_Instance;

		public static FactionPersistance Instance{ get{ return m_Instance; } }

		public override string DefaultName
		{
			get { return "Faction Persistance - Internal"; }
		}

		public FactionPersistance() : base( 1 )
		{
			Movable = false;

			if ( m_Instance == null || m_Instance.Deleted )
				m_Instance = this;
			else
				base.Delete();
		}

		private enum PersistedType
		{
			Terminator,
			Faction,
			Town
		}

		public FactionPersistance( Serial serial ) : base( serial )
		{
			m_Instance = this;
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			List<Faction> factions = Faction.Factions;

			for ( int i = 0; i < factions.Count; ++i )
			{
				writer.WriteEncodedInt( (int) PersistedType.Faction );
				factions[i].State.Serialize( writer );
			}

			List<Town> towns = Town.Towns;

			for ( int i = 0; i < towns.Count; ++i )
			{
				writer.WriteEncodedInt( (int) PersistedType.Town );
				towns[i].State.Serialize( writer );
			}

			writer.WriteEncodedInt( (int) PersistedType.Terminator );
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 0:
				{
					PersistedType type;

					while ( (type = (PersistedType)reader.ReadEncodedInt()) != PersistedType.Terminator )
					{
						switch ( type )
						{
							case PersistedType.Faction: new FactionState( reader ); break;
							case PersistedType.Town: new TownState( reader ); break;
						}
					}

					break;
				}
			}
		}

		public override void Delete()
		{
		}
          public static void Initialize()
        {
            CommandSystem.Register("DeleteFactionPersistence", AccessLevel.Administrator, new CommandEventHandler(OnCommand_DeleteMilitiaPersistence));
        }
        public static void OnCommand_DeleteMilitiaPersistence(CommandEventArgs e)
        {
            Item itemToRemove = null;
            foreach (Item item in World.Items.Values)
            {
                if (item.Parent != null || item.Map != Map.Internal)
                    continue;

                Type type = item.GetType();

                if (type == typeof(FactionPersistance))
                    itemToRemove = item;
            }

            if(itemToRemove != null)
                itemToRemove.Delete();

            //clear all of the mobile's citizenship states
            foreach (Mobile mobile in World.Mobiles.Values)
            {
                if (mobile is PlayerMobile)
                {
                    ((PlayerMobile)mobile).CitizenshipPlayerState = null;
                    mobile.InvalidateProperties();
                }
            }

            //clear all of the commonwealth states
            foreach (Faction faction in Faction.Factions)
            {
                faction.State = null;
                faction.State = new FactionState(faction);
            }
            e.Mobile.SendMessage("MyCommonwealth persistence object was deleted, you can now recreated with [GenerateTownships");
        }
    

	}
}