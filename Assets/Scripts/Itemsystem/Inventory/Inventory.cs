using System.Collections.Generic;

// Einfaches Inventar: speichert pro Item eine Menge (noch ohne Slots).
// Wird von Maschinen als Input-/Output-Puffer verwendet, später auch vom Spieler.
public class Inventory
{
    private readonly Dictionary<ItemData, int> amounts = new Dictionary<ItemData, int>();

    public IReadOnlyDictionary<ItemData, int> Contents => amounts;

    public int GetAmount(ItemData item)
    {
        return item != null && amounts.TryGetValue(item, out int amount) ? amount : 0;
    }

    public void Clear()
    {
        amounts.Clear();
    }

    public void Add(ItemData item, int amount)
    {
        if (item == null || amount <= 0)
            return;

        amounts[item] = GetAmount(item) + amount;
    }

    public void Add(IReadOnlyList<ItemStack> stacks)
    {
        foreach (ItemStack stack in stacks)
            Add(stack.Item, stack.Amount);
    }

    public bool Remove(ItemData item, int amount)
    {
        int current = GetAmount(item);
        if (amount <= 0 || current < amount)
            return false;

        if (current == amount)
            amounts.Remove(item);
        else
            amounts[item] = current - amount;
        return true;
    }

    // Entfernt alle Stacks oder gar nichts, falls etwas fehlt.
    public bool Remove(IReadOnlyList<ItemStack> stacks)
    {
        if (!Has(stacks))
            return false;

        foreach (ItemStack stack in stacks)
            Remove(stack.Item, stack.Amount);
        return true;
    }

    public bool Has(IReadOnlyList<ItemStack> stacks)
    {
        foreach (ItemStack stack in stacks)
        {
            if (GetAmount(stack.Item) < stack.Amount)
                return false;
        }
        return true;
    }
}
