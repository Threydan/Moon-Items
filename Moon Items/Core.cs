using System.Reflection;
using HarmonyLib;
using MelonLoader;
using System;
using System.Collections.Generic;
using UnityEngine;

[assembly: MelonInfo(typeof(Moon_Items.Core), "Moon Items", "0.1.0", "Threydan", null)]
[assembly: MelonGame("Buried Things", "Cursed Words")]

/* Todo: Balance item costs
 *       Decide item rarity
 *       Consider tags
 */

namespace Moon_Items
{
    public class Core : MelonMod
    {
        public override void OnInitializeMelon()
        {
            LoggerInstance.Msg("Initialized.");
        }
    }
}