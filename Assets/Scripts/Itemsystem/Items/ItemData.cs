using UnityEngine;

// Ein Item (Rohstoff, Zwischen- oder Endprodukt).
// Anlegen im Project-Fenster über: Create > Factory > Item
// Der Asset-Name (z. B. "IronIngot") dient als eindeutige ID, etwa für Speicherstände.
[CreateAssetMenu(fileName = "NewItem", menuName = "Factory/Item")]
public class ItemData : ScriptableObject
{
    [SerializeField] private string displayName;
    [SerializeField, TextArea] private string description;
    [SerializeField] private Sprite icon;
    [SerializeField, Min(1)] private int maxStackSize = 100;
    [SerializeField] private ItemForm form = ItemForm.Solid;

    public string Id => name;
    public string DisplayName => string.IsNullOrEmpty(displayName) ? name : displayName;
    public string Description => description;
    public Sprite Icon => icon;
    public int MaxStackSize => maxStackSize;
    public ItemForm Form => form;
}
