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
    }
}