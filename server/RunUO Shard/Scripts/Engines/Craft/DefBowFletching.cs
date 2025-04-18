using System;
using Server.Items;

namespace Server.Engines.Craft
{
	public class DefBowFletching : CraftSystem
	{
		public override SkillName MainSkill
		{
			get	{ return SkillName.Fletching; }
		}

		public override int GumpTitleNumber
		{
			get { return 1044006; } // <CENTER>BOWCRAFT AND FLETCHING MENU</CENTER>
		}

		private static CraftSystem m_CraftSystem;

		public static CraftSystem CraftSystem
		{
			get
			{
				if ( m_CraftSystem == null )
					m_CraftSystem = new DefBowFletching();

				return m_CraftSystem;
			}
		}

		public override double GetChanceAtMin( CraftItem item )
		{
			return 0.5; // 50%
		}

		private DefBowFletching() : base( 1, 1, 1.25 )// base( 1, 2, 1.7 )
		{
		}

		public override int CanCraft( Mobile from, BaseTool tool, Type itemType )
		{
			if( tool == null || tool.Deleted || tool.UsesRemaining < 0 )
				return 1044038; // You have worn out your tool!
			else if ( !BaseTool.CheckAccessible( tool, from ) )
				return 1044263; // The tool must be on your person to use.

			return 0;
		}

		public override void PlayCraftEffect( Mobile from )
		{
			// no animation
			//if ( from.Body.Type == BodyType.Human && !from.Mounted )
			//	from.Animate( 33, 5, 1, true, false, 0 );

			from.PlaySound( 0x55 );
		}

		public override int PlayEndingEffect( Mobile from, bool failed, bool lostMaterial, bool toolBroken, int quality, bool makersMark, CraftItem item )
		{
			if ( toolBroken )
				from.SendLocalizedMessage( 1044038 ); // You have worn out your tool

			if ( failed )
			{
				if ( lostMaterial )
					return 1044043; // You failed to create the item, and some of your materials are lost.
				else
					return 1044157; // You failed to create the item, but no materials were lost.
			}
			else
			{
				if ( quality == 0 )
					return 502785; // You were barely able to make this item.  It's quality is below average.
				else if ( makersMark && quality == 2 )
					return 1044156; // You create an exceptional quality item and affix your maker's mark.
				else if ( quality == 2 )
					return 1044155; // You create an exceptional quality item.
				else				
					return 1044154; // You create the item.
			}
		}

		public override CraftECA ECA{ get{ return CraftECA.FiftyPercentChanceMinusTenPercent; } }

		public override void InitCraftList()
		{
			int index = -1;

			MarkOption = true;
			Repair = false;
            CanEnhance = false;

            SetSubRes(typeof(Log), 1072643);

            AddSubRes(typeof(Log), 1072643, 00.0, 1044041, 1072652);
            AddSubRes(typeof(OakLog), 1072644, 65.0, 1044041, 1072652);
            AddSubRes(typeof(AshLog), 1072645, 80.0, 1044041, 1072652);
            AddSubRes(typeof(YewLog), 1072646, 95.0, 1044041, 1072652);
            AddSubRes(typeof(HeartwoodLog), 1072647, 100.0, 1044041, 1072652);

            SetSubRes2(typeof(ArtifactOfMight), 1063490);

            AddSubRes2(typeof(ArtifactOfMight), 1063491, -100.0, 1063520, 1063521);
            AddSubRes2(typeof(ArtifactOfForce), 1063492, 70.0, 1063520, 1063521);
            AddSubRes2(typeof(ArtifactOfPower), 1063493, 80.0, 1063520, 1063521);
            AddSubRes2(typeof(ArtifactOfVanquishing), 1063494, 90.0, 1063520, 1063521);
            AddSubRes2(typeof(ArtifactOfSurpassingAccuracy), 1063501, -100.0, 1063520, 1063521);
            AddSubRes2(typeof(ArtifactOfEminentAccuracy), 1063502, 70.0, 1063520, 1063521);
            AddSubRes2(typeof(ArtifactOfExceedingAccuracy), 1063503, 80.0, 1063520, 1063521);
            AddSubRes2(typeof(ArtifactOfSupremeAccuracy), 1063504, 90.0, 1063520, 1063521);
            AddSubRes2(typeof(ArtifactOfSilver), 1063510, 75.0, 1063520, 1063521);
            AddSubRes2(typeof(ArtifactOfElementalSlaying), 1063511, 75.0, 1063520, 1063521);
            AddSubRes2(typeof(ArtifactOfArachnidSlaying), 1063512, 75.0, 1063520, 1063521);
            AddSubRes2(typeof(ArtifactOfRepondSlaying), 1063513, 75.0, 1063520, 1063521);
            AddSubRes2(typeof(ArtifactOfReptileSlaying), 1063514, 75.0, 1063520, 1063521);
		}
	}
}