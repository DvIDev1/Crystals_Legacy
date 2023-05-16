namespace Crystals.Core.Systems.SynergySystem
{
    public class Synergy
    {
        /*
        
                public Item?[] items;
        
                public ItemID[] armor;
        
                public Item?[] accessories;
        
                public string SynergyText;
        
                public bool isActive;
        
                public Synergy(Item[] items, Item[] armor, Item[] accessories, string synergyText)
                {
                    this.items = items;
                    this.armor = armor;
                    this.accessories = accessories;
                    SynergyText = synergyText;
                }
        
                public bool checkActive(Player player)
                {
                    foreach (var arm in player.armor)
                    {
                        if (!arm.wornArmor && arm.type != armor)
                        {
                            return false;
                        }
                    }
                    
                    foreach (var item in items)
                    {
                        if (!player.inventory.Contains(item))
                        {
                            return false;
                        }
                    }
                    
                    foreach (var acc in accessories)
                    {
                        if (!player.armor.Contains(acc) && acc.accessory)
                        {
                            return false;
                        }
                    }
        
                    isActive = true;
                    return true;
                }
        
                public void DisplaySynergyText()
                {
                    if (isActive)
                    {
                        
                    }
                }*/

        /*class SynergySystem : ModSystem
        {
            public override void Load()
            {
                Synergy wol = new Synergy();
            }

            public override void PreUpdateItems()
            {
                
            }
        }*/
    }
}