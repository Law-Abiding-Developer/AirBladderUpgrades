using System.Collections.Generic;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Assets.PrefabTemplates;
using UpgradesLIB.Items.Equipment;

namespace AirBladderUpgrades.Items.Force_Upgrades;

public class ABForceUpgrades
{
    public static CustomPrefab[] ForcePrefabs = new CustomPrefab[3];
    public static PrefabInfo[] ForcePIs =  new PrefabInfo[3];
    private static readonly TechType _techTypeToCopy = TechType.VehiclePowerUpgradeModule;

    public static void Register()
    {
        for (var i = 0; i < 3; i++)
        {
            var multiplier = Sequence(i);
            ForcePIs[i] = PrefabInfo.WithTechType("AirBladderForceUpgradeMk" + (i+1), "Air Bladder Force Upgrade Mk " + (i+1), $"Mk {i+1} Force for the Air Bladder. Increases the amount of force the air bladder applies by {multiplier}x").WithIcon(SpriteManager.Get(TechType.AirBladder));
            var upgradedata = new UpgradeData(0, multiplier);
            UpgradeData.Upgradedata.Add(ForcePIs[i].TechType, upgradedata);
            ForcePrefabs[i] = new CustomPrefab(ForcePIs[i]);
            var clone = new CloneTemplate(ForcePIs[i], _techTypeToCopy);
            ForcePrefabs[i].SetGameObject(clone);
            ForcePrefabs[i].SetRecipe(new Nautilus.Crafting.RecipeData()
                {
                    craftAmount = 1,
                    Ingredients = GetIngredients(i)
                })
                .WithFabricatorType(Handheldprefab.HandheldfabTreeType)
                .WithStepsToFabricatorTab("Tools", "AirBladderTab")
                .WithCraftingTime(5f);
            ForcePrefabs[i].SetUnlock(TechType.AirBladder);
            ForcePrefabs[i].SetPdaGroupCategory(UpgradesLIB.Plugin.toolupgrademodules, Plugin.AirBladderCategory);
            ForcePrefabs[i].Register();
            Plugin.Logger.LogInfo($"Prefab AirBladderForceUpgradeMk{i} successfully initialized!");
        }
    }

    private static int _currentSquence = 2;
    private static int Sequence(int index)
    {
        if (index == 0) return _currentSquence;
        _currentSquence += index+1;
        return _currentSquence;
    }

    private static List<Ingredient> GetIngredients(int index)
    {
        switch (index)
        {
            case 0:
                return new List<Ingredient>()
                {
                    new(TechType.Titanium, 1),
                    new(TechType.Lithium, 1),
                };
            case 1:
                return new List<Ingredient>()
                {
                    new(TechType.Lithium, 1),
                    new(ForcePIs[0].TechType,1)
                };
            case 2:
                return new List<Ingredient>()
                {
                    new(TechType.JeweledDiskPiece, 2),
                    new(ForcePIs[1].TechType,1)
                };
        }

        return null;
    }
}