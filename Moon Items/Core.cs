using System.Reflection;
using HarmonyLib;
using MelonLoader;
using System;
using System.Collections.Generic;
using UnityEngine;

[assembly: MelonInfo(typeof(Moon_Items.Core), "Moon Items", "0.2.1", "Threydan", null)]
[assembly: MelonGame("Buried Things", "Cursed Words")]


namespace Moon_Items
{
    public class Core : MelonMod
    {
        public static Dictionary<string, Sprite> ItemSprites = [];
        public override void OnInitializeMelon()
        {
            LoggerInstance.Msg("[MoonItems] Initialized.");
        }
        public override void OnApplicationStart()
        {
            //Look for the asset bundle and retrieve all the sprites
            string itemsPath = Path.Combine([Application.dataPath, "../Mods/moonsprites"]);
            string itemsModManagerInstallPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "/moonsprites";
            AssetBundle itemsAssetBundle = AssetBundle.LoadFromFile(itemsPath) ?? AssetBundle.LoadFromFile(itemsModManagerInstallPath);
            if (itemsAssetBundle == null)
            {
                MelonLogger.Warning($"[MoonItems] We couldn't find the item bundle from {itemsPath} or {itemsModManagerInstallPath}");
            }
            else
            {
                MelonLogger.Msg("[MoonItems] item bundle found, retrieving sprites!");
                UnityEngine.Object[] itemAssets = itemsAssetBundle.LoadAllAssets();
                foreach (var item in itemAssets)
                {
                    if (item.GetType() == typeof(Sprite))
                    { 
                        MelonLogger.Msg($"[MoonItems] Sprite with name {item.name} found!");
                        ItemSprites[item.name] = item as Sprite;
                    }
                    else
                    {
                        MelonLogger.Msg($"[MoonItems] item not understood, item name: {item.name}");
                    }
                }
            }
        }
    }
}