using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

// Sammelt alle Rezepte des Spiels und beantwortet Abfragen wie
// "Was kann ein Smelter herstellen?" oder "Wie bekomme ich Circuit Boards?".
// Anlegen über: Create > Factory > Recipe Database
// Befüllen: Rechtsklick auf den Komponenten-Header im Inspector > "Alle Rezepte im Projekt sammeln"
[CreateAssetMenu(fileName = "RecipeDatabase", menuName = "Factory/Recipe Database")]
public class RecipeDatabase : ScriptableObject
{
    [SerializeField] private List<Recipe> recipes = new List<Recipe>();

    public IReadOnlyList<Recipe> Recipes => recipes;

    public List<Recipe> GetRecipesFor(BuildingData building)
    {
        return recipes.FindAll(r => r != null && r.ProducedIn == building);
    }

    public List<Recipe> GetRecipesProducing(ItemData item)
    {
        return recipes.FindAll(r => r != null && r.Produces(item));
    }

    public List<Recipe> GetRecipesUsing(ItemData item)
    {
        return recipes.FindAll(r => r != null && r.Uses(item));
    }

#if UNITY_EDITOR
    [ContextMenu("Alle Rezepte im Projekt sammeln")]
    public void CollectAllRecipes()
    {
        recipes.Clear();
        foreach (string guid in AssetDatabase.FindAssets("t:" + nameof(Recipe)))
            recipes.Add(AssetDatabase.LoadAssetAtPath<Recipe>(AssetDatabase.GUIDToAssetPath(guid)));

        recipes.Sort((a, b) => string.CompareOrdinal(a.name, b.name));
        EditorUtility.SetDirty(this);
    }
#endif
}
