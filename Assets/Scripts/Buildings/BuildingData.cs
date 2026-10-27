using System.Collections.Generic;
using UnityEngine;

// Beschreibt einen Gebäudetyp (nicht ein platziertes Gebäude in der Welt).
// Anlegen im Project-Fenster über: Create > Factory > Building
[CreateAssetMenu(fileName = "NewBuilding", menuName = "Factory/Building")]
public class BuildingData : ScriptableObject
{
    [SerializeField] private string displayName;
    [SerializeField, TextArea] private string description;
    [SerializeField] private Sprite icon;
    [SerializeField] private BuildingCategory category = BuildingCategory.Production;

    [Header("Grid")]
    [SerializeField] private Vector2Int size = Vector2Int.one;
    [SerializeField] private GameObject prefab;

    [Header("Energie")]
    [Tooltip("Verbrauch in MW (Produktionsgebäude)")]
    [SerializeField, Min(0f)] private float powerConsumption;
    [Tooltip("Erzeugung in MW (Energiegebäude)")]
    [SerializeField, Min(0f)] private float powerProduction;

    [Header("Bau")]
    [SerializeField] private List<ItemStack> buildCost = new List<ItemStack>();

    public string Id => name;
    public string DisplayName => string.IsNullOrEmpty(displayName) ? name : displayName;
    public string Description => description;
    public Sprite Icon => icon;
    public BuildingCategory Category => category;
    public Vector2Int Size => size;
    public GameObject Prefab => prefab;
    public float PowerConsumption => powerConsumption;
    public float PowerProduction => powerProduction;
    public IReadOnlyList<ItemStack> BuildCost => buildCost;
}
