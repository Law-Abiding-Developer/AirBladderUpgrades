using System.Collections.Generic;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Assets.PrefabTemplates;
using UpgradesLIB.Items.Equipment;

namespace AirBladderUpgrades.Items.Capacity_Upgrades
{
    public class AirBladderCapacityUpgradeMk3
    {
        public static CustomPrefab mk3capacityprefab;
        public static PrefabInfo mk3capacityprefabinfo;
        public static TechType techType = TechType.VehiclePowerUpgradeModule;
        public static void Register()
        {
            mk3capacityprefabinfo = PrefabInfo.WithTechType("AirBladderCapacityUpgradeMk3", "Air Bladder Capacity Upgrade Mk 3", "Mk 3 Capacity for the Air Bladder. Multiples the Oxygen Capacity of the Air Bladder by 7x.").WithIcon(SpriteManager.Get(TechType.AirBladder));
            var upgradedata = new UpgradeData(7);
            UpgradeData.Upgradedata.Add(mk3capacityprefabinfo.TechType, upgradedata);
            mk3capacityprefab = new CustomPrefab(mk3capacityprefabinfo);
            var clone = new CloneTemplate(mk3capacityprefabinfo, techType);
            mk3capacityprefab.SetGameObject(clone);
            mk3capacityprefab.SetRecipe(new Nautilus.Crafting.RecipeData()
            {
                craftAmount = 1,
                Ingredients = new List<Ingredient>()
                {
                    new (TechType.Gold,1),
                    new (TechType.Titanium,1),
                    new (AirBladderCapacityUpgradeMk2.mk2capacityprefabinfo.TechType,1)
                }
            })
            .WithFabricatorType(Handheldprefab.HandheldfabTreeType)
            .WithStepsToFabricatorTab("Tools", "AirBladderTab")
            .WithCraftingTime(5f);
            mk3capacityprefab.SetUnlock(TechType.AirBladder);
            mk3capacityprefab.SetPdaGroupCategory(UpgradesLIB.Plugin.toolupgrademodules, Plugin.AirBladderCategory);
            mk3capacityprefab.Register();
            Plugin.Logger.LogInfo("Prefab AirBladderCapacityUpgradeMk3 successfully initialized!");
        }
    }
}
