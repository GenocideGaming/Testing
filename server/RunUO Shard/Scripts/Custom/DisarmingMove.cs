using System;
using System.Collections.Generic;
using System.Text;
using Server.Commands;
using Server.Mobiles;
using Server.Items;

namespace Server.Scripts.Custom
{
    public class DisarmingMove
    {

        public static void Initialize()
        {
            CommandSystem.Register("Disarm", AccessLevel.Player, new CommandEventHandler(OnCommand_DisarmingMove));
        }

        [Usage("Disarm")]
        [Description("Performs a disarming move.")]
        public static void OnCommand_DisarmingMove(CommandEventArgs e)
        {
            PlayerMobile player = e.Mobile as PlayerMobile;
            if (player == null) { return; }

            int manaNeeded = FeatureList.DisarmingMove.ManaCost;

            if (player.Mana < manaNeeded)
            {
                player.SendMessage(FeatureList.DisarmingMove.NotEnoughManaText);
                return;
            }
            if (!HasFreeHands(player))
            {
                player.SendMessage(FeatureList.DisarmingMove.HandsNotFreeMessage);
                return;
            }

            if (player.Skills[SkillName.Wrestling].Value < FeatureList.DisarmingMove.WrestlingSkillNeeded
                || player.Skills[SkillName.Tactics].Value < FeatureList.DisarmingMove.TacticsSkillNeeded)
            {
                player.SendMessage(FeatureList.DisarmingMove.SkillTooLowMessage);
                return;
            }

            if (player.NextDisarmingMove > DateTime.Now)
            {
                player.SendMessage(FeatureList.DisarmingMove.TimerNotElapsedText);
                return;
            }
            player.Mana -= manaNeeded;
            player.DisarmReady = true;
            player.SendMessage(FeatureList.DisarmingMove.OnActivateText);

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
