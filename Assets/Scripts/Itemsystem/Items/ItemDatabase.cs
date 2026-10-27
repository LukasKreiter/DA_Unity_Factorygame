using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

// Sammelt alle Items des Spiels. Es sollte genau ein Asset davon geben.
// Anlegen über: Create > Factory > Item Database
// Befüllen: Rechtsklick auf den Komponenten-Header im Inspector > "Alle Items im Projekt sammeln"
[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Factory/Item Database")]
public class ItemDatabase : ScriptableObject
{
    [SerializeField] private List<ItemData> items = new List<ItemData>();

    private Dictionary<string, ItemData> itemsById;

    public IReadOnlyList<ItemData> Items => items;

    public ItemData Get(string id)
    {
        if (itemsById == null)
            BuildLookup();

        itemsById.TryGetValue(id, out ItemData item);
        return item;
    }

    private void BuildLookup()
    {
        itemsById = new Dictionary<string, ItemData>();
        foreach (ItemData item in items)
        {
            if (item == null)
                continue;
            if (itemsById.ContainsKey(item.Id))
                Debug.LogError($"Item-ID '{item.Id}' ist doppelt vergeben.", this);
            else
                itemsById.Add(item.Id, item);
        }
    }

    private void OnEnable()
    {
        itemsById = null;
    }

    private void OnValidate()
    {
        itemsById = null;
    }

#if UNITY_EDITOR
    [ContextMenu("Alle Items im Projekt sammeln")]
    public void CollectAllItems()
    {
        items.Clear();
        foreach (string guid in AssetDatabase.FindAssets("t:" + nameof(ItemData)))
            items.Add(AssetDatabase.LoadAssetAtPath<ItemData>(AssetDatabase.GUIDToAssetPath(guid)));

        items.Sort((a, b) => string.CompareOrdinal(a.name, b.name));
        itemsById = null;
        EditorUtility.SetDirty(this);
    }
#endif
}
