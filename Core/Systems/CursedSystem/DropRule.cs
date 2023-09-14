using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;

namespace Crystals.Core.Systems.CursedSystem
{
    internal class DropRule : IItemDropRuleCondition
    {
        private static LocalizedText Description;

        public DropRule()
        {
            Description ??= Language.GetOrRegister("Mods.Crystals.Core.CursedSystem.StupidDropRule");
        }

        public bool CanDrop(DropAttemptInfo info)
        {
            NPC npc = info.npc;

            return !npc.friendly 
                && npc.GetGlobalNPC<CursedNPC>().IsNPCCursed == true
                ;
        }

        public bool CanShowItemDropInUI()
        {
            return true;
        }

        public string GetConditionDescription()
        {
            return Description.Value;
        }
    }
}
