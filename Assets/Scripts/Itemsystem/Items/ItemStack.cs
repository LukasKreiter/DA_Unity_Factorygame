using System;
using UnityEngine;

// Ein Item mit Menge, z. B. "3x Iron Ingot". Wird für Rezepte, Baukosten und später Inventare verwendet.
[Serializable]
public struct ItemStack
{
    [SerializeField] private ItemData item;
    [SerializeField, Min(1)] private int amount;

    public ItemStack(ItemData item, int amount)
    {
        this.item = item;
        this.amount = amount;
    }

    public ItemData Item => item;
    public int Amount => amount;

    public override string ToString()
    {
        return $"{amount}x {(item != null ? item.DisplayName : "<leer>")}";
    }
}
