using System.Collections.Generic;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Assets.PrefabTemplates;
using UpgradesLIB.Items.Equipment;

namespace AirBladderUpgrades.Items.Capacity_Upgrades
{
    public class AirBladderCapacityUpgradeMk1 //all of these are nearly the same, just some name changes and very little behavior. let me know if i should change mk1, mk2, or 3.
    {
        public static CustomPrefab mk1capacityprefab;
        public static PrefabInfo mk1capacityprefabinfo;
        public static TechType techType = TechType.VehiclePowerUpgradeModule;
        public static void Register()
        {
            mk1capacityprefabinfo = PrefabInfo.WithTechType("AirBladderCapacityUpgradeMk1", "Air Bladder Capacity Upgrade Mk 1", "Mk 1 Capacity for the Air Bladder. Doubles the Oxygen Capacity in the Air Bladder").WithIcon(SpriteManager.Get(TechType.AirBladder));
            var upgradedata = new UpgradeData(2);
            UpgradeData.Upgradedata.Add(mk1capacityprefabinfo.TechType, upgradedata);
            mk1capacityprefab = new CustomPrefab(mk1capacityprefabinfo);
            var clone = new CloneTemplate(mk1capacityprefabinfo, techType);
            mk1capacityprefab.SetGameObject(clone);
            mk1capacityprefab.SetRecipe(new Nautilus.Crafting.RecipeData()
            {
                craftAmount = 1,
                Ingredients = new List<Ingredient>()
                {
                    new (TechType.Battery,1),
                    new (TechType.Silicone,1)
                }
            })
            .WithFabricatorType(Handheldprefab.HandheldfabTreeType)
            .WithStepsToFabricatorTab("Tools", "AirBladderTab")
            .WithCraftingTime(5f);
            mk1capacityprefab.SetUnlock(TechType.AirBladder);
            mk1capacityprefab.SetPdaGroupCategory(UpgradesLIB.Plugin.toolupgrademodules, Plugin.AirBladderCategory);
            mk1capacityprefab.Register();
            Plugin.Logger.LogInfo("Prefab AirBladderCapacityUpgradeMk1 successfully initialized!");
        }
    }
}
