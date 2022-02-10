using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using StardewValley;
using Microsoft.Xna.Framework;
using StardewModdingAPI.Utilities;

namespace RidgesideVillage
{
    //Corrects the location name in the "X has begun in Y" message
    internal static class HarmonyPatch_Telephone
    {
        private static IMonitor Monitor { get; set; }
        private static IModHelper Helper { get; set; }


        internal static void ApplyPatch(Harmony harmony, IModHelper helper)
        {
            Helper = helper;
            Log.Trace($"Applying Harmony Patch \"{nameof(HarmonyPatch_Telephone)}\".");
            harmony.Patch(
                original: AccessTools.Method(typeof(Game1), "ShowTelephoneMenu"),
                postfix: new HarmonyMethod(typeof(HarmonyPatch_Telephone), nameof(ShowTelephoneMenu_Postfix))
            );
        }

        internal static void ShowTelephoneMenu_Postfix()
        {
            try
            {
                Game1.playSound("openBox");
                List<Response> responses = new List<Response>();
                responses.Add(new Response("SeedShop", "Pierre again"));
                responses.Add(new Response("HangUp", Game1.content.LoadString("Strings\\Locations:MineCart_Destination_Cancel")));
                Game1.currentLocation.createQuestionDialogue(Game1.content.LoadString("Strings\\Characters:Phone_SelectNumber"), responses.ToArray(), "telephone");
            }
            catch (Exception e)
            {
                Log.Error($"Harmony patch \"{nameof(ShowTelephoneMenu_Postfix)}\" has encountered an error. \n{e.ToString()}");
            }
        }

    }
}
