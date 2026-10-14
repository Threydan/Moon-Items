using HarmonyLib;
using MelonLoader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Moon_Items.Items
{
    class CrescentMoon : Item
    {
        public CrescentMoon()
        {
            Name = "Crescent Moon";
            SpriteData.Add(new ItemSpriteData(ItemSpriteUsage.Default, "[MoonItems]CrescentMoon"));
            Rarity = ItemRarity.Rare;
            Tags = new List<ItemTag>
            {
                ItemTag.WordEntryManipulation
            };
            ItemFunctionTags = new List<ItemFunctionTag>
            {
                ItemFunctionTag.Tech
            };
            Cost = 11;
        }

        public override string GetDescription()
        {
            return "You can teleport to the corners at any time";
        }
    }
}
