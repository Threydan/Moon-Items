using HarmonyLib;
using MelonLoader;
using Unity.VisualScripting;
using UnityEngine;

namespace Moon_Items.Items
{
    class SmilingMoon : Item
    {
        public SmilingMoon()
        {
            Name = "Smiling Moon";
            SpriteData.Add(new ItemSpriteData(ItemSpriteUsage.Default, "FullMoon"));
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
        public List<List<Vector2Int>> RandomTiles = new List<List<Vector2Int>> { };

        public override string GetDescription()
        {
            return "You can Teleport randomly across the board (predetermined for each grid)";
        }

        public override GridData ApplyStartOfGridEffect(GridData gridData, int gridNumber, int numberOfGrids, List<HistoricWord> previousWords, List<BoardGenVizInfo> vizSteps, bool isReroll)
        {
            //set every vector (up to the dimension of the grid) to a vector that points to a random location on hte grid
            for (int i = 0; i < gridData.Dimensions.x; i++)
            {
                for (int j = 0; j < gridData.Dimensions.y; j++)
                {
                    RandomTiles[i][j] = new Vector2Int(UnityEngine.Random.Range(0, gridData.Dimensions.x), UnityEngine.Random.Range(0, gridData.Dimensions.y));
                }
            }
            return gridData;
        }

        public override void OnAcquire()
        {
            MelonLogger.Msg(" ------OnAcquire has run");
            //populate the list with a 5x5 grid of vectors
            for (int i = 0; i < 5; i++)
            {
                RandomTiles.Add(new List<Vector2Int> { });
                for (int j = 0; j < 5; j++)
                {
                    RandomTiles[i].Add(new Vector2Int());
                }
            }
        }
    }
}
