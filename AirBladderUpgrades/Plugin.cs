using System.Collections;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Nautilus.Utility;
using UnityEngine;
using AirBladderUpgrades;
using System;
using AirBladderUpgrades.Items.Capacity_Upgrades;
using Nautilus.Handlers;
using JetBrains.Annotations;
using Nautilus.Options;
using ToolsUpgradesLIB;
using UpgradesLIB;
using UpgradesLIB.Items.Equipment;

namespace AirBladderUpgrades
{
    /*
     * Todo List: Complete for now
     */
    [BepInPlugin("com.lawabidingtroller.airbladderupgrades", "AirBladderUpgrades", "0.1.0")]//next version to release
    [BepInDependency("com.snmodding.nautilus")]
    [BepInDependency("com.lawabidingmodder.upgradeslib")]
    public class Plugin : BaseUnityPlugin
    {
        public new static ManualLogSource Logger { get; private set; }

        private static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();

        public static ModOptions ModOptions;
        
        public static GameInput.Button OpenUpgradesButton = EnumHandler.AddEntry<GameInput.Button>("OpenABUpgrades")
            .CreateInput("Open Air Bladder Upgrades")
            .WithKeyboardBinding(GameInputHandler.Paths.Keyboard.V)
            .WithCategory("Tools Upgrades");
        
        public static TechCategory AirBladderCategory = EnumHandler.AddEntry<TechCategory>("Air Bladder Upgrades").WithPdaInfo("Air Bladder Upgrades").RegisterToTechGroup(UpgradesLIB.Plugin.toolupgrademodules);
        
        private void Awake()
        {
            // set project-scoped logger instance
            Logger = base.Logger;

            Logger.LogInfo("Loading Air Bladder Upgrades...");
            // register harmony patches, if there are any
            Harmony.CreateAndPatchAll(Assembly, $"AirBladderUpgrades");
            Logger.LogInfo("Awake method is running. Dependencies exist. Completing plugin load...");

            var allowedtech = new TechType[] { TechType.Bleach, AirBladderCapacityUpgradeMk1.mk1capacityprefabinfo.TechType, AirBladderCapacityUpgradeMk2.mk2capacityprefabinfo.TechType, AirBladderCapacityUpgradeMk3.mk3capacityprefabinfo.TechType };//create allowed tech temp variable, wont work because of how i set this method up
            StartCoroutine(UpgradesLIB.Plugin.CreateUpgradesContainer(TechType.AirBladder, 
                "AirBladderStorage", 
                "AirBladderStorageChild", 
                2, 
                2));//call the method
            InitializePrefabs();//initialize the custom upgrades for this mod

            Logger.LogInfo("Creating Mod Options...");
            ModOptions = OptionsPanelHandler.RegisterModOptions<ModOptions>();//register the mod options

            //create a tab for the air bladder's ugprades
            CraftTreeHandler.AddTabNode(Handheldprefab.HandheldfabGadget.CraftTreeType, "AirBladderTab", "Air Bladder", SpriteManager.Get(TechType.AirBladder),  "Tools");
            
            
            
            Logger.LogInfo("Plugin fully loaded successfully!"); //thats all for this method, so fully loaded is sufficeint
        }

        private void InitializePrefabs() //actually register all of the custom prefabs
        {
            Logger.LogInfo("Initializing prefabs...");
            AirBladderCapacityUpgradeMk1.Register();
            AirBladderCapacityUpgradeMk2.Register();
            AirBladderCapacityUpgradeMk3.Register();
            Logger.LogInfo("Most, if not, all, prefabs initalized successfully!");
        }
    }
}