using System;
using UnityEditor;
using UnityEngine;

// Erzeugt die Items, Gebäude und Rezepte aus dem Itemdiagramm als Assets unter Assets/Data.
// Aufruf über das Menü: Factory > Standarddaten aus Itemdiagramm erzeugen
// Bereits vorhandene Assets werden NICHT überschrieben, man kann das Script also gefahrlos erneut ausführen.
// Mengen, Zeiten und Größen sind Platzhalter und sollen im Inspector angepasst werden.
public static class FactoryDataGenerator
{
    private const string Root = "Assets/Data";
    private const string ItemsPath = Root + "/Items";
    private const string RecipesPath = Root + "/Recipes";
    private const string BuildingsPath = Root + "/Buildings";

    [MenuItem("Factory/Standarddaten aus Itemdiagramm erzeugen")]
    public static void Generate()
    {
        // ---------- Gebäude ----------
        BuildingData miner = Building("Production", "Miner", "Miner", BuildingCategory.Production, 2, 2);
        BuildingData waterExtractor = Building("Production", "WaterExtractor", "Water Extractor", BuildingCategory.Production, 2, 2);
        BuildingData oilExtractor = Building("Production", "OilExtractor", "Oil Extractor", BuildingCategory.Production, 3, 3);
        BuildingData smelter = Building("Production", "Smelter", "Smelter", BuildingCategory.Production, 2, 2);
        BuildingData foundry = Building("Production", "Foundry", "Foundry", BuildingCategory.Production, 3, 3);
        BuildingData assembler = Building("Production", "Assembler", "Assembler", BuildingCategory.Production, 3, 3);
        BuildingData assemblerMk2 = Building("Production", "AssemblerMk2", "Assembler Mk 2", BuildingCategory.Production, 3, 4);
        BuildingData assemblerMk3 = Building("Production", "AssemblerMk3", "Assembler Mk 3", BuildingCategory.Production, 4, 4);
        BuildingData refinery = Building("Production", "Refinery", "Refinery", BuildingCategory.Production, 3, 4);
        Building("Production", "ResearchLab", "Research Lab", BuildingCategory.Production, 3, 3);

        Building("Energy", "WindTurbine", "Wind Turbine", BuildingCategory.Energy, 2, 2);
        Building("Energy", "SolarPowerplant", "Solar Powerplant", BuildingCategory.Energy, 3, 3);
        Building("Energy", "CoalPowerplant", "Coal Powerplant", BuildingCategory.Energy, 3, 3);
        Building("Energy", "FuelPowerplant", "Fuel/Biomass Powerplant", BuildingCategory.Energy, 3, 3);
        Building("Energy", "NuclearPowerplant", "Nuclear Powerplant", BuildingCategory.Energy, 5, 5);

        Building("Logistics", "ConveyorBelt", "Conveyor Belt", BuildingCategory.Logistics, 1, 1);
        Building("Logistics", "Splitter", "Splitter", BuildingCategory.Logistics, 1, 1);
        Building("Logistics", "Merger", "Merger", BuildingCategory.Logistics, 1, 1);
        Building("Logistics", "PowerPole", "Power Pole", BuildingCategory.Logistics, 1, 1);

        // ---------- Items: Rohstoffe ----------
        ItemData ironOre = Item("Raw", "IronOre", "Iron Ore");
        ItemData copperOre = Item("Raw", "CopperOre", "Copper Ore");
        ItemData stone = Item("Raw", "Stone", "Stone");
        Item("Raw", "Wood", "Wood");
        ItemData water = Item("Raw", "Water", "Water", ItemForm.Fluid);
        ItemData crudeOil = Item("Raw", "CrudeOil", "Crude Oil", ItemForm.Fluid);
        ItemData coal = Item("Raw", "Coal", "Coal");
        ItemData sulfur = Item("Raw", "Sulfur", "Sulfur");
        ItemData uraniumOre = Item("Raw", "UraniumOre", "Uranium Ore");

        // ---------- Items: Zwischenprodukte ----------
        ItemData ironIngot = Item("Intermediate", "IronIngot", "Iron Ingot");
        ItemData steelIngot = Item("Intermediate", "SteelIngot", "Steel Ingot");
        ItemData copperIngot = Item("Intermediate", "CopperIngot", "Copper Ingot");
        ItemData ironGear = Item("Intermediate", "IronGear", "Iron Gear");
        ItemData screws = Item("Intermediate", "Screws", "Screws");
        ItemData ironPlate = Item("Intermediate", "IronPlate", "Iron Plate");
        ItemData copperWire = Item("Intermediate", "CopperWire", "Copper Wire");
        ItemData concrete = Item("Intermediate", "Concrete", "Concrete");
        ItemData lightOil = Item("Intermediate", "LightOil", "Light Oil", ItemForm.Fluid);
        ItemData heavyOil = Item("Intermediate", "HeavyOil", "Heavy Oil", ItemForm.Fluid);
        ItemData plastic = Item("Intermediate", "Plastic", "Plastic");
        ItemData rubber = Item("Intermediate", "Rubber", "Rubber");
        ItemData fuel = Item("Intermediate", "Fuel", "Fuel", ItemForm.Fluid);
        ItemData sulfuricAcid = Item("Intermediate", "SulfuricAcid", "Sulfuric Acid", ItemForm.Fluid);

        // ---------- Items: Endprodukte ----------
        ItemData reinforcedSteelPlate = Item("Advanced", "ReinforcedSteelPlate", "Reinforced Steel Plate");
        ItemData circuitBoard = Item("Advanced", "CircuitBoard", "Circuit Board");
        ItemData lightAlloy = Item("Advanced", "LightAlloy", "Light Alloy");
        ItemData battery = Item("Advanced", "Battery", "Battery");
        ItemData uraniumFuelRod = Item("Advanced", "UraniumFuelRod", "Uranium Fuel Rod");
        ItemData foundation = Item("Advanced", "Foundation", "Foundation");

        // ---------- Rezepte: Abbau ----------
        CreateRecipe("Miner", "IronOre", miner, 1f, In(), Out(S(ironOre, 1)));
        CreateRecipe("Miner", "CopperOre", miner, 1f, In(), Out(S(copperOre, 1)));
        CreateRecipe("Miner", "Stone", miner, 1f, In(), Out(S(stone, 1)));
        CreateRecipe("Miner", "Coal", miner, 1f, In(), Out(S(coal, 1)));
        CreateRecipe("Miner", "Sulfur", miner, 1f, In(), Out(S(sulfur, 1)));
        CreateRecipe("Miner", "UraniumOre", miner, 2f, In(), Out(S(uraniumOre, 1)));
        CreateRecipe("WaterExtractor", "Water", waterExtractor, 1f, In(), Out(S(water, 1)));
        CreateRecipe("OilExtractor", "CrudeOil", oilExtractor, 1f, In(), Out(S(crudeOil, 1)));

        // ---------- Rezepte: Smelter ----------
        CreateRecipe("Smelter", "IronIngot", smelter, 2f, In(S(ironOre, 1)), Out(S(ironIngot, 1)));
        CreateRecipe("Smelter", "CopperIngot", smelter, 2f, In(S(copperOre, 1)), Out(S(copperIngot, 1)));

        // ---------- Rezepte: Foundry ----------
        CreateRecipe("Foundry", "SteelIngot", foundry, 3f, In(S(ironOre, 1), S(coal, 2)), Out(S(steelIngot, 1)));

        // ---------- Rezepte: Assembler (1 Input) ----------
        CreateRecipe("Assembler", "IronGear", assembler, 2f, In(S(ironIngot, 2)), Out(S(ironGear, 1)));
        CreateRecipe("Assembler", "Screws", assembler, 1f, In(S(ironIngot, 1)), Out(S(screws, 4)));
        CreateRecipe("Assembler", "IronPlate", assembler, 2f, In(S(ironIngot, 3)), Out(S(ironPlate, 1)));
        CreateRecipe("Assembler", "CopperWire", assembler, 1f, In(S(copperIngot, 1)), Out(S(copperWire, 2)));
        CreateRecipe("Assembler", "Foundation", assembler, 4f, In(S(concrete, 4)), Out(S(foundation, 1)));
        CreateRecipe("Assembler", "UraniumFuelRod", assembler, 10f, In(S(uraniumOre, 5)), Out(S(uraniumFuelRod, 1)));

        // ---------- Rezepte: Assembler Mk 2 (2 Inputs) ----------
        CreateRecipe("AssemblerMk2", "Concrete", assemblerMk2, 2f, In(S(stone, 2), S(water, 1)), Out(S(concrete, 1)));
        CreateRecipe("AssemblerMk2", "ReinforcedSteelPlate", assemblerMk2, 6f, In(S(steelIngot, 2), S(screws, 4)), Out(S(reinforcedSteelPlate, 1)));
        CreateRecipe("AssemblerMk2", "CircuitBoard", assemblerMk2, 6f, In(S(ironPlate, 1), S(copperWire, 2)), Out(S(circuitBoard, 1)));
        CreateRecipe("AssemblerMk2", "Battery", assemblerMk2, 8f, In(S(rubber, 1), S(sulfuricAcid, 2)), Out(S(battery, 1)));

        // ---------- Rezepte: Assembler Mk 3 (3 Inputs) ----------
        CreateRecipe("AssemblerMk3", "LightAlloy", assemblerMk3, 6f, In(S(ironIngot, 3), S(copperIngot, 3), S(plastic, 3)), Out(S(lightAlloy, 1)));

        // ---------- Rezepte: Refinery ----------
        CreateRecipe("Refinery", "LightOil", refinery, 4f, In(S(crudeOil, 2)), Out(S(lightOil, 1)));
        CreateRecipe("Refinery", "HeavyOil", refinery, 4f, In(S(crudeOil, 2)), Out(S(heavyOil, 1)));
        CreateRecipe("Refinery", "Plastic", refinery, 3f, In(S(lightOil, 1)), Out(S(plastic, 1)));
        CreateRecipe("Refinery", "Rubber", refinery, 3f, In(S(lightOil, 1)), Out(S(rubber, 1)));
        CreateRecipe("Refinery", "Fuel", refinery, 3f, In(S(heavyOil, 1), S(coal, 1)), Out(S(fuel, 2)));
        CreateRecipe("Refinery", "SulfuricAcid", refinery, 3f, In(S(sulfur, 1), S(water, 1)), Out(S(sulfuricAcid, 1)));

        // ---------- Datenbanken ----------
        ItemDatabase itemDatabase = LoadOrCreate<ItemDatabase>(Root, "ItemDatabase", so => { });
        itemDatabase.CollectAllItems();
        RecipeDatabase recipeDatabase = LoadOrCreate<RecipeDatabase>(Root, "RecipeDatabase", so => { });
        recipeDatabase.CollectAllRecipes();

        AssetDatabase.SaveAssets();
        Debug.Log($"Factory-Daten erzeugt: {itemDatabase.Items.Count} Items, {recipeDatabase.Recipes.Count} Rezepte.");
    }

    private static ItemData Item(string group, string id, string displayName, ItemForm form = ItemForm.Solid)
    {
        return LoadOrCreate<ItemData>($"{ItemsPath}/{group}", id, so =>
        {
            so.FindProperty("displayName").stringValue = displayName;
            so.FindProperty("form").enumValueIndex = (int)form;
        });
    }

    private static BuildingData Building(string group, string id, string displayName, BuildingCategory category, int width, int depth)
    {
        return LoadOrCreate<BuildingData>($"{BuildingsPath}/{group}", id, so =>
        {
            so.FindProperty("displayName").stringValue = displayName;
            so.FindProperty("category").enumValueIndex = (int)category;
            so.FindProperty("size").vector2IntValue = new Vector2Int(width, depth);
        });
    }

    private static Recipe CreateRecipe(string group, string id, BuildingData producedIn, float craftTime, ItemStack[] inputs, ItemStack[] outputs)
    {
        return LoadOrCreate<Recipe>($"{RecipesPath}/{group}", id, so =>
        {
            so.FindProperty("producedIn").objectReferenceValue = producedIn;
            so.FindProperty("craftTime").floatValue = craftTime;
            SetStacks(so.FindProperty("inputs"), inputs);
            SetStacks(so.FindProperty("outputs"), outputs);
        });
    }

    private static ItemStack S(ItemData item, int amount) => new ItemStack(item, amount);
    private static ItemStack[] In(params ItemStack[] stacks) => stacks;
    private static ItemStack[] Out(params ItemStack[] stacks) => stacks;

    private static void SetStacks(SerializedProperty list, ItemStack[] stacks)
    {
        list.arraySize = stacks.Length;
        for (int i = 0; i < stacks.Length; i++)
        {
            SerializedProperty element = list.GetArrayElementAtIndex(i);
            element.FindPropertyRelative("item").objectReferenceValue = stacks[i].Item;
            element.FindPropertyRelative("amount").intValue = stacks[i].Amount;
        }
    }

    private static T LoadOrCreate<T>(string folder, string id, Action<SerializedObject> fill) where T : ScriptableObject
    {
        string path = $"{folder}/{id}.asset";
        T existing = AssetDatabase.LoadAssetAtPath<T>(path);
        if (existing != null)
            return existing;

        EnsureFolder(folder);
        T asset = ScriptableObject.CreateInstance<T>();
        AssetDatabase.CreateAsset(asset, path);

        SerializedObject so = new SerializedObject(asset);
        fill(so);
        so.ApplyModifiedPropertiesWithoutUndo();
        return asset;
    }

    private static void EnsureFolder(string folder)
    {
        if (AssetDatabase.IsValidFolder(folder))
            return;

        int slash = folder.LastIndexOf('/');
        string parent = folder.Substring(0, slash);
        EnsureFolder(parent);
        AssetDatabase.CreateFolder(parent, folder.Substring(slash + 1));
    }
}
