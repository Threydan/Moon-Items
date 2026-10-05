using HarmonyLib;
using MelonLoader;
using Moon_Items.Items;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;



namespace Moon_Items
{
    //This adds all the items into the game. There's definitely a better way to call upon all my items than each individually, but this works too.
    //Thank you Siveran for making your mod(Llong) open source, otherwise this part would've stumped me for a while!
    [HarmonyPatch(typeof(Assembly), "GetTypes", MethodType.Normal)]
    [HarmonyPatch([])]
    static class Assembly_GetTypes_Patch
    {
        static void Postfix(Assembly __instance, ref Type[] __result)
        {
            if (__instance.GetName().Name == "Assembly-CSharp")
            {
                __result = [.. __result, .. Assembly.GetAssembly(typeof(HalfMoon)).GetTypes(),
                            .. Assembly.GetAssembly(typeof(SmilingMoon)).GetTypes(), 
                            .. Assembly.GetAssembly(typeof(CrescentMoon)).GetTypes()];
            }
        }
    }

    //This allows the GiveItem command to actually give the items in this mod. I couldn't figure out
    //how to add all my items automatically (despite being helped), so i call them individually.
    //Thank you K2 in the Buried Things discord for giving me this patch!
    [HarmonyPatch(typeof(ConsoleController), "GiveItem", new System.Type[] { typeof(string[]) })]
    public static class AddItemsToConsolePool_Patch
    {
        public static void Prefix(ref string[] args)
        {
            if (args.Length != 1)
            {
                System.Type type = System.Type.GetType(args[1]);
                if (type == null)
                {
                    MelonLogger.Msg("Can't find item in the game, checking modded items");
                    List<Item> moddedItems = new List<Item>
                    {
                        new HalfMoon(),
                        new SmilingMoon(),
                        new CrescentMoon()
                    };

                    string argument = args[1];
                    if (moddedItems.Exists(item => item.GetType().ToString().Split('.').Last() == argument))
                    {
                        MelonLogger.Msg("Modded item found");
                        type = moddedItems.FirstOrDefault(item => item.GetType().ToString().Split('.').Last() == argument).GetType();
                        if (type != null && type.IsSubclassOf(typeof(Item)))
                        {
                            Item item2 = System.Activator.CreateInstance(type) as Item;
                            if (args.Length >= 3 && (args[2] == "-f" || args[2] == "-foil"))
                            {
                                item2.IsFoil = true;
                            }
                            GameStatics.GetPlayer().AddItemToInventory(item2);
                            CharacterInfoPanel.SingletonInventoryVisualController.PopulateAll();
                            MelonLogger.Msg("Gave Item: " + item2.Name);
                        }
                    }
                    else
                        MelonLogger.Msg("Item Not Found Even In Modded Items");
                    return;
                }
            }
        }
    }
}