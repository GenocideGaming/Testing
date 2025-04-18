using System;
using Server.Network;

namespace Server
{
	public class CurrentExpansion
	{
		public static void Configure()
		{
			Core.Expansion = Expansion.T2A;

			Mobile.InsuranceEnabled = false;
			ObjectPropertyList.Enabled = false;
			Mobile.VisibleDamageType = Core.T2A ? VisibleDamageType.Related : VisibleDamageType.None;
			Mobile.GuildClickMessage = true;
			Mobile.AsciiClickMessage = true;
		}
	}
}
