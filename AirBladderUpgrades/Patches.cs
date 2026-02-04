using System.Collections.Generic;
using AirBladderUpgrades.Items.Capacity_Upgrades;
using HarmonyLib;
using UnityEngine;

namespace AirBladderUpgrades
{
    [HarmonyPatch(typeof(AirBladder))] //patch the air bladder
    public class AirBladderPatches
    {
        [HarmonyPatch(nameof(AirBladder.Start))]
        [HarmonyPostfix]
        public static void Start_Postfix(AirBladder __instance)
        {
            if (__instance == null) return; 
            var tempstorage = __instance.GetComponent<StorageContainer>();
            if (tempstorage == null) return;
            tempstorage.container._label = "AIR BLADDER";
            var allowedtech = new TechType[4]
            {
                TechType.Bleach, AirBladderCapacityUpgradeMk1.mk1capacityprefabinfo.TechType,
                AirBladderCapacityUpgradeMk2.mk2capacityprefabinfo.TechType, 
                AirBladderCapacityUpgradeMk3.mk3capacityprefabinfo.TechType,
            };
            tempstorage.container.SetAllowedTechTypes(allowedtech);
            
        }

        [HarmonyPatch(nameof(AirBladder.Update))]
        [HarmonyPostfix]
        public static void Update_Postfix(AirBladder __instance)
        {
            if (__instance == null) return;
            var tempstorage = __instance.GetComponent<StorageContainer>();
            if (tempstorage == null) return;
            if (GameInput.GetButtonDown(Plugin.OpenUpgradesButton)) //check if the keybind to open the storage container is pressed
            {
                Plugin.Logger.LogInfo("Open Storage Container Key pressed for Air Bladder!");
                if (tempstorage.open) //check if its already open
                {
                    ErrorMessage.AddMessage("Close 'AIR BLADDER' to open it!"); 
                    return;
                }
                tempstorage.Open();//open it
            }
        }

        [HarmonyPatch(nameof(AirBladder.UpdateInflateState))] //patch the updateinflatestate method, which checks the current capacity
        [HarmonyPrefix]
        public static void UpdateInflateState_Prefix(AirBladder __instance) //change the capcity before it checks calls the method
        {

            if (__instance == null) return; //check if the instance is null
            var capacity = UpgradeData.GetCapacity(__instance, out var bleach);
            if (!bleach && __instance.oxygenCapacity == 0)
            {
                __instance.oxygenCapacity = 5f;
            }
            __instance.oxygenCapacity *= capacity;
        }

        [HarmonyPatch(nameof(AirBladder.OnDestroy))]
        [HarmonyPostfix]
        public static void OnDestroy_Postfix(AirBladder __instance)
        {
            __instance.oxygenCapacity = 5f;
        }
    }

    public class UpgradeData
    {
        public static Dictionary<TechType, UpgradeData> Upgradedata = new Dictionary<TechType, UpgradeData>();
        
        public float CapacityMultiplier;

        public UpgradeData(float capacityMultiplier = 0)
        {
            
        }

        public static float GetCapacity(AirBladder instance, out bool isBleach)
        {
            isBleach = false;
            var tempstorage  = instance.GetComponent<StorageContainer>();
            if (tempstorage == null)
            {
                Plugin.Logger.LogError("Failed to find the storage container for the Air Bladder! WTF Happened.");
                isBleach = true;
                return 0;
            }

            UpgradeData upgrade;
            float highestcapacity = 0;
            foreach (var item in tempstorage.container.GetItemTypes())
            {
                if (item == TechType.Bleach)
                {
                    ErrorMessage.AddWarning("The Air Bladder's ability to do work has been removed to protect you. And the environment");
                    isBleach = true;
                    break;
                }
                if (!Upgradedata.TryGetValue(item, out upgrade))
                {
                    Plugin.Logger.LogError($"Failed to find the upgrade data for: {item}!");
                    continue;
                }
                highestcapacity = Mathf.Max(highestcapacity, upgrade.CapacityMultiplier);
            }
            return highestcapacity;
        }
    }

    public class AirBladderData
    {
        public float DefaultCapcity;

        public AirBladderData(float defaultCapcity)
        {
            DefaultCapcity = defaultCapcity;
        }
    }
}
