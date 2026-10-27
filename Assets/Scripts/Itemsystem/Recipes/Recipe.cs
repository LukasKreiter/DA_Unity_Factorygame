using System.Collections.Generic;
using UnityEngine;

// Ein Rezept: wandelt Inputs in einem bestimmten Gebäude in Outputs um.
// Anlegen im Project-Fenster über: Create > Factory > Recipe
// Rezepte ohne Inputs sind erlaubt (z. B. Miner: nichts -> Iron Ore).
[CreateAssetMenu(fileName = "NewRecipe", menuName = "Factory/Recipe")]
public class Recipe : ScriptableObject
{
    [SerializeField] private string displayName;
    [SerializeField] private List<ItemStack> inputs = new List<ItemStack>();
    [SerializeField] private List<ItemStack> outputs = new List<ItemStack>();
    [Tooltip("Dauer eines Durchlaufs in Sekunden")]
    [SerializeField, Min(0.1f)] private float craftTime = 1f;
    [SerializeField] private BuildingData producedIn;

    public string Id => name;
    public string DisplayName => string.IsNullOrEmpty(displayName) ? name : displayName;
    public IReadOnlyList<ItemStack> Inputs => inputs;
    public IReadOnlyList<ItemStack> Outputs => outputs;
    public float CraftTime => craftTime;
    public BuildingData ProducedIn => producedIn;

    public bool Uses(ItemData item)
    {
        return Contains(inputs, item);
    }

    public bool Produces(ItemData item)
    {
        return Contains(outputs, item);
    }

    private static bool Contains(List<ItemStack> stacks, ItemData item)
    {
        foreach (ItemStack stack in stacks)
        {
            if (stack.Item == item)
                return true;
        }
        return false;
    }

    private void OnValidate()
    {
        if (outputs.Count == 0)
            Debug.LogWarning($"Rezept '{name}' hat keine Outputs.", this);
        if (producedIn == null)
            Debug.LogWarning($"Rezept '{name}' ist keinem Gebäude zugeordnet.", this);
    }
}
