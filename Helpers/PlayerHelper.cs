using Terraria;

namespace Crystals.Helpers
{
    public class PlayerHelper
    {
        public static void HealMana(int amount , Player player)
        {
            player.statLife += amount;
            if (Main.myPlayer == player.whoAmI)
            {
                player.ManaEffect(amount);
            }
            if (player.statMana > player.statManaMax2)
            {
                player.statLife = player.statManaMax2;
            }
        }

        public static bool HasAccessoryEquipped(Player Player, int type)
        {
            for (int i = 3; i < 10; i++)
            {
                if (Player.IsItemSlotUnlockedAndUsable(i))
                {
                    if (!Player.armor[i].IsAir && Player.armor[i].type == type)
                        return true;
                }
            }

            return false;
        }
    }
}