using System;
using Server.Mobiles;
using Server.Scripts;
using Server.Items;

namespace Server.Commands
{
    public class StunPunch
    {
        public static void Initialize()
        {
            if (FeatureList.StunPunch.Enabled)
            {
                CommandSystem.Register("StunPunch", AccessLevel.Player, new CommandEventHandler(StunPunch_OnCommand));
            }
        }

        [Usage("StunPunch")]
        [Description("Performs a special \"Stun Punch\" move.")]
        private static void StunPunch_OnCommand(CommandEventArgs e)
        {
            PlayerMobile player = e.Mobile as PlayerMobile;
            if (player == null) { return; }

            if (!PlayerHasRequiredSkill(player))
            {
                player.SendMessage(FeatureList.StunPunch.NotEnoughSkillText);
                return;
            }

            int manaNeeded = FeatureList.StunPunch.ManaActivationCost;

            if (player.Mana < manaNeeded)
            {
                player.SendMessage(FeatureList.StunPunch.NotEnoughManaText);
                return;
            }

            if (!HasFreeHands(player))
            {
                player.SendMessage(FeatureList.StunPunch.HandsNotFreeMessage);
                return;
            }

            if (player.NextStunPunch > DateTime.Now)
            {
                player.SendMessage(FeatureList.StunPunch.TimerNotElapsedText);
                return;
            }

            player.Mana -= FeatureList.StunPunch.ManaActivationCost;    // Lose as soon as you activate the ability
            player.StunReady = true;

            player.SendMessage(FeatureList.StunPunch.OnActivateText);
        }

        private static bool PlayerHasRequiredSkill(PlayerMobile player)
        {
            foreach (SkillName skillName in FeatureList.StunPunch.SkillsRequiredToAttempt)
            {
                if (player.Skills[skillName].Value < FeatureList.StunPunch.MinimumSkillRequired)
                    return false;
            }

            return true;
        }

        private static bool HasFreeHands(Mobile m)
        {
            Item item = m.FindItemOnLayer(Layer.OneHanded);

            if (item != null && !(item is Spellbook))
                return false;

            return m.FindItemOnLayer(Layer.TwoHanded) == null;
        }
    }
}
