using System;
using Server.Mobiles;
using Server.Scripts;
using Server.Items;

namespace Server.Commands
{
    public class ToxicStrike
    {
        public static void Initialize()
        {
            CommandSystem.Register("ToxicStrike", AccessLevel.Player, new CommandEventHandler(ToxicStrike_OnCommand));
        }

        [Usage("ToxicStrike")]
        [Description("A special move which poisons your enemy.")]
        private static void ToxicStrike_OnCommand(CommandEventArgs e)
        {
            PlayerMobile player = e.Mobile as PlayerMobile;
            if (player == null) { return; }

            if (player.ToxicSwingReady)
            {
                player.SendMessage("You are no longer preparing to strike a toxic blow.");
                player.ToxicSwingReady = false;
                return;
            }

            if (player.Skills[SkillName.Poisoning].Value < 20)
            {
                player.SendMessage("You are not skilled enough to attempt a toxic strike.");
                return;
            }

            if (player.NextToxicSwing > DateTime.Now)
            {
                player.SendMessage("You must wait before attempting another toxic strike.");
                return;
            }

            BaseWeapon weapon = player.Weapon as BaseWeapon;

            if (weapon == null || weapon.Poison == null || weapon.PoisonCharges <= 0)
            {
                player.SendMessage("You must have a poisoned weapon equipped to perform a toxic strike.");
                return;
            }
            
            player.ToxicSwingReady = true;
            player.SendMessage("You prepare to strike a toxic blow.");
        }
    }
}
