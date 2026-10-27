using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

// Legt in der offenen Scene eine Test-Produktionskette aus DebugMachines an:
//   Iron Ore   -> Iron Ingot   -> Iron Plate  \
//                                              -> Circuit Board
//   Copper Ore -> Copper Ingot -> Copper Wire /
// Voraussetzung: Factory > Standarddaten aus Itemdiagramm erzeugen wurde bereits ausgeführt.
public static class DebugChainCreator
{
    private const string RecipesPath = "Assets/Data/Recipes";

    [MenuItem("Factory/Test-Kette in Scene erstellen")]
    public static void Create()
    {
        Recipe ironOre = LoadRecipe("Miner/IronOre");
        Recipe ironIngot = LoadRecipe("Smelter/IronIngot");
        Recipe ironPlate = LoadRecipe("Assembler/IronPlate");
        Recipe copperOre = LoadRecipe("Miner/CopperOre");
        Recipe copperIngot = LoadRecipe("Smelter/CopperIngot");
        Recipe copperWire = LoadRecipe("Assembler/CopperWire");
        Recipe circuitBoard = LoadRecipe("Assembler/CircuitBoard");

        if (ironOre == null || ironIngot == null || ironPlate == null || copperOre == null
            || copperIngot == null || copperWire == null || circuitBoard == null)
        {
            EditorUtility.DisplayDialog("Test-Kette",
                "Rezepte nicht gefunden. Bitte zuerst 'Factory > Standarddaten aus Itemdiagramm erzeugen' ausführen.", "OK");
            return;
        }

        GameObject root = new GameObject("Test-Kette (Circuit Board)");
        Undo.RegisterCreatedObjectUndo(root, "Test-Kette erstellen");

        DebugMachine circuitAssembler = CreateMachine(root, "Assembler: Circuit Board", circuitBoard, null, 6);
        DebugMachine plateAssembler = CreateMachine(root, "Assembler: Iron Plate", ironPlate, circuitAssembler, 2);
        DebugMachine wireAssembler = CreateMachine(root, "Assembler: Copper Wire", copperWire, circuitAssembler, 5);
        DebugMachine ironSmelter = CreateMachine(root, "Smelter: Iron Ingot", ironIngot, plateAssembler, 1);
        DebugMachine copperSmelter = CreateMachine(root, "Smelter: Copper Ingot", copperIngot, wireAssembler, 4);
        DebugMachine ironMiner = CreateMachine(root, "Miner: Iron Ore", ironOre, ironSmelter, 0);
        DebugMachine copperMiner = CreateMachine(root, "Miner: Copper Ore", copperOre, copperSmelter, 3);

        // Hierarchie in Reihenfolge der Kette sortieren
        DebugMachine[] order = { ironMiner, ironSmelter, plateAssembler, copperMiner, copperSmelter, wireAssembler, circuitAssembler };
        for (int i = 0; i < order.Length; i++)
            order[i].transform.SetSiblingIndex(i);

        Selection.activeGameObject = root;
        EditorSceneManager.MarkSceneDirty(root.scene);
    }

    private static DebugMachine CreateMachine(GameObject parent, string name, Recipe recipe, DebugMachine outputTarget, int displayOrder)
    {
        GameObject go = new GameObject(name);
        go.transform.SetParent(parent.transform);
        DebugMachine machine = go.AddComponent<DebugMachine>();

        SerializedObject so = new SerializedObject(machine);
        so.FindProperty("recipe").objectReferenceValue = recipe;
        so.FindProperty("outputTarget").objectReferenceValue = outputTarget;
        so.FindProperty("displayOrder").intValue = displayOrder;
        so.ApplyModifiedPropertiesWithoutUndo();
        return machine;
    }

    private static Recipe LoadRecipe(string path)
    {
        return AssetDatabase.LoadAssetAtPath<Recipe>($"{RecipesPath}/{path}.asset");
    }
}
