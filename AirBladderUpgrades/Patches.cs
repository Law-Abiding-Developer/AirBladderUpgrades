using System.Collections.Generic;
using AirBladderUpgrades.Items.Capacity_Upgrades;
using HarmonyLib;
using UnityEngine;
using AirBladderUpgrades.Items.Force_Upgrades;

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
            var allowedtech = new TechType[7]
            {
                TechType.Bleach, AirBladderCapacityUpgradeMk1.mk1capacityprefabinfo.TechType,
                AirBladderCapacityUpgradeMk2.mk2capacityprefabinfo.TechType,
                AirBladderCapacityUpgradeMk3.mk3capacityprefabinfo.TechType,
                ABForceUpgrades.ForcePIs[0].TechType, ABForceUpgrades.ForcePIs[1].TechType,
                ABForceUpgrades.ForcePIs[2].TechType
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
            if (__instance.oxygen > __instance.oxygenCapacity) __instance.oxygen = __instance.oxygenCapacity;
            var capacity = UpgradeData.GetHighestUpgrade(__instance, out var bleach);
            __instance.buoyancyForce = 0.8f * capacity.ForceMultiplier;
            if (bleach)
            {
                __instance.oxygenCapacity = 0f;
            }
            else __instance.oxygenCapacity = 15f * capacity.CapacityMultiplier/capacity.ForceMultiplier;
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
        
        public float ForceMultiplier;

        public UpgradeData(float capacityMultiplier = 1, float  forceMultiplier = 1)
        {
            CapacityMultiplier = capacityMultiplier;
            ForceMultiplier = forceMultiplier;
        }

        public static UpgradeData GetHighestUpgrade(AirBladder instance, out bool isBleach)
        {
            isBleach = false;
            var tempstorage  = instance.GetComponent<StorageContainer>();
            if (tempstorage == null)
            {
                Plugin.Logger.LogError("Failed to find the storage container for the Air Bladder! WTF Happened.");
                isBleach = true;
                return null;
            }

            UpgradeData data = new UpgradeData();
            for (int upgradeType = 0; upgradeType < 2; upgradeType++)
            {
                float highestmultiplier = 1;
                foreach (var item in tempstorage.container.GetItemTypes())
                {
                    if (item == TechType.Bleach)
                    {
                        ErrorMessage.AddWarning(
                            "The Air Bladder's ability to do work has been removed to protect you. And the environment");
                        isBleach = true;
                        break;
                    }

                    if (!Upgradedata.TryGetValue(item, out var upgrade))
                    {
                        Plugin.Logger.LogError($"Failed to find the upgrade data for: {item}!");
                        continue;
                    }

                    switch (upgradeType)
                    {
                        case 0:
                            highestmultiplier = Mathf.Max(highestmultiplier, upgrade.CapacityMultiplier);
                            break;
                        case 1:
                            highestmultiplier = Mathf.Min(highestmultiplier, upgrade.ForceMultiplier);
                            break;
                    }
                }
                switch (upgradeType)
                {
                    case 0:
                        data.CapacityMultiplier = highestmultiplier;
                        break;
                    case 1:
                        data.ForceMultiplier = highestmultiplier;
                        break;
                }
            }
            return data;
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
