using HarmonyLib;
using MelonLoader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Moon_Items.Items
{
    class HalfMoon : Item
    {
        public HalfMoon()
        {
            Name = "Half Moon";
            SpriteData.Add(new ItemSpriteData(ItemSpriteUsage.Default, "[MoonItems]HalfMoon_0"));
            Rarity = ItemRarity.Common;
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
            return "You can teleport to the tile opposite on the board";
        }
    }
}
