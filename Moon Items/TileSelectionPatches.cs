using HarmonyLib;
using MelonLoader;
using Moon_Items.Items;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Tilemaps;
using static System.Net.WebRequestMethods;

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

            Tile currentTile = currentTiles[^1];
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
                Tile tile = gridData.GetAvailableTileAtCoordinates(
                    gridData.Dimensions.x - 1 - currentTile.Coordinates.x,
                    gridData.Dimensions.y - 1 - currentTile.Coordinates.y);
                if (tile != null && !currentTiles.Exists(tile4 => tile4 == tile))
                {
                    __result.Add
                        (
                            new TileSelection(tile, TileSelectionMethod.FullMoon, tile.IsDisplayingAsVariableLetter(tileSelectionManager))
                        );
                }
            }

            if (list.Exists((Item item4) => item4 is SmilingMoon))
            {
                SmilingMoon moonitem = list.FirstOrDefault((Item item4) => item4 is SmilingMoon) as SmilingMoon;
                Tile tile = gridData.GetAvailableTileAtCoordinates(
                    moonitem.RandomTiles[currentTile.Coordinates.x][currentTile.Coordinates.y].x,
                    moonitem.RandomTiles[currentTile.Coordinates.x][currentTile.Coordinates.y].y);
                if (tile != null && !currentTiles.Contains(tile))
                {
                    __result.Add(new TileSelection(tile, TileSelectionMethod.FullMoon, tile.IsDisplayingAsVariableLetter(tileSelectionManager)));
                }
            }

            if (list.Exists((Item item4) => item4 is CrescentMoon))
            {
                List<Tile> Corners = new List<Tile>
                {
                    gridData.GetAvailableTileAtCoordinates(0, 0),
                    gridData.GetAvailableTileAtCoordinates(0, gridData.Dimensions.y - 1),
                    gridData.GetAvailableTileAtCoordinates(gridData.Dimensions.x - 1, 0),
                    gridData.GetAvailableTileAtCoordinates(gridData.Dimensions.x - 1, gridData.Dimensions.y - 1)
                };
                __result.AddRange(from tile2 in Corners
                    where tile2 != null && !currentTiles.Contains(tile2)
                    select new TileSelection(tile2, TileSelectionMethod.FullMoon, tile2.IsDisplayingAsVariableLetter(tileSelectionManager)));
            }
            #endregion
        }
    }
}