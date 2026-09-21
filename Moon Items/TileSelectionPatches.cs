using HarmonyLib;
using MelonLoader;
using Moon_Items.Items;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Moon_Items
{
    [HarmonyPatch(typeof(GridUtilitySingleton), "GetValidNextTiles", MethodType.Normal)]
    static class GridUtilitySingleton_GetValidNextTiles_patch
    {
        static void Postfix(List<TileSelection> __result, GridData gridData, List<Tile> currentTiles, TileSelectionManager tileSelectionManager, bool noInventory)
        {
            //Since Postfix cannot access variables created inside of the function (at least from what i can tell), this code
            //has to be repeated. It might be possible with other harmonypatch methods but i'm not good enough yet to understand those
            #region Original code that needs to be repeated
            if (currentTiles.Count() == 0) 
            {
                return; 
            }

            Player player = GameStatics.GetPlayer();
            List<Item> list = (noInventory ? new List<Item>() : player.GetAllItems());
            Item item = list.Find((Item item4) => item4 is RandomAccessMemory);
            Item item2 = list.Find((Item item4) => item4 is Snapshot);
            List<Item> list2 = list.Where((Item item4) => item4 is Frankenstein).ToList();
            if (item != null)
            {
                RandomAccessMemory randomAccessMemory = item as RandomAccessMemory;
                list.AddRange(randomAccessMemory.ItemsInMemory);
            }
            if (item2 != null)
            {
                Snapshot snapshot = item2 as Snapshot;
                if (snapshot.SnapshottedItem != null)
                {
                    list.Add(snapshot.SnapshottedItem);
                }
            }
            foreach (Item item4 in list2)
            {
                Frankenstein frankenstein = item4 as Frankenstein;
                list.AddRange(frankenstein.StitchedItems);
            }
            #endregion

            #region items
            if (list.Exists((Item item4) => item4 is HalfMoon))
            {
                Tile currentTile = currentTiles[^1];
                Tile tile = gridData.GetAvailableTileAtCoordinates(
                    gridData.Dimensions.x - currentTile.Coordinates.x - 1, 
                    gridData.Dimensions.y - currentTile.Coordinates.y - 1);
                if (tile != null && !currentTiles.Exists(tile4 => tile4 == tile))
                {
                    __result.AddRange
                        (
                            [new TileSelection(tile, TileSelectionMethod.FullMoon, tile.IsDisplayingAsVariableLetter(tileSelectionManager))]
                        );
                }
            }

            #endregion
        }
    }
}