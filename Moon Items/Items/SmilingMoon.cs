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
            bool isCOTVrun = GameStatics.GetPlayer().CurrentRunProgress.Challenge?.ChallengeName == "Call Of The Void";
            //set every vector in RandomTiles (up to the dimension of the grid) to a vector that points to a random location on the grid
            for (int i = 0; i < gridData.Dimensions.x; i++)
            {
                for (int j = 0; j < gridData.Dimensions.y; j++)
                {
                    Vector2Int vector = new Vector2Int(UnityEngine.Random.Range(0, gridData.Dimensions.x), UnityEngine.Random.Range(0, gridData.Dimensions.y));
                    if (isCOTVrun == true)
                    {
                        //Clamp the value to have either x or y become either 0 or 5 so that the random vector will always point to a location on the edge of the grid
                        //This does mean that the 4 corner tiles are twice as likely to appear, but this is the simplest way to make sure that getting this item in a COTV run is worthwhile
                        int MinOrMax = Convert.ToBoolean(UnityEngine.Random.Range(0, 2)) ? 0 : 5;
                        if (Convert.ToBoolean(UnityEngine.Random.Range(0, 2))) { vector.x = MinOrMax; }
                        else { vector.y = MinOrMax; }
                    }
                    RandomTiles[i][j] = vector;
                }
            }
            return gridData;
        }

        public override void OnAcquire()
        {
            //populate RandomTiles with a 2d 6x6 grid of vectors (It's 6x6 instead of 5x5 so that this item works in a Call Of The Void run)
            for (int i = 0; i < 6; i++)
            {
                RandomTiles.Add(new List<Vector2Int> { });
                for (int j = 0; j < 6; j++)
                {
                    RandomTiles[i].Add(new Vector2Int());
                }
            }
        }
    }
}
