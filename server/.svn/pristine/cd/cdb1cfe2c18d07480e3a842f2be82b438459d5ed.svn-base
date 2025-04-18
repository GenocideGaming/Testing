using System;
using Server.Mobiles;
using Server.Scripts;

namespace Server.Commands
{
    public class ViolentSwing
    {
        public static void Initialize()
        {
            if (FeatureList.ViolentSwingMechanic.Enabled)
            {
                CommandSystem.Register("ViolentSwing", AccessLevel.Player, new CommandEventHandler(ViolentSwing_OnCommand));
            }
        }

        [Usage("ViolentSwing")]
        [Description("Performs a special \"Violent Swing\" move.")]
        private static void ViolentSwing_OnCommand(CommandEventArgs e)
        {
            PlayerMobile player = e.Mobile as PlayerMobile;
            if (player == null) { return; }

            if (player.Weapon is Items.BaseRanged)
            {
                player.SendMessage("You cannot violently swing while holding that weapon!");
                return;
            }

            if (player.Skills[SkillName.Anatomy].Value < 50.0)
            {
                player.SendMessage(FeatureList.ViolentSwingMechanic.NotEnoughSkillText);
                return;
            }

            int stamNeeded = FeatureList.ViolentSwingMechanic.StaminaRequired;

            if (player.Stam < stamNeeded)   // need 35 stamina to use the ability
            {
                player.SendMessage(FeatureList.ViolentSwingMechanic.NotEnoughStamText);
                return;
            }

            if (player.NextViolentSwing > DateTime.Now)
            {
                player.SendMessage(FeatureList.ViolentSwingMechanic.TimerNotElapsedText);
                return;
            }
            
            if (!player.ViolentSwingReady)
            {
	            player.Stam -= FeatureList.ViolentSwingMechanic.StaminaRequired;    // Lose the stamina as soon as you activate the ability
	            player.ViolentSwingReady = true;
	            player.SendMessage(FeatureList.ViolentSwingMechanic.OnActivateText);
            }
        }
    }
}
