using System.Collections.Generic;
using System.Text;
using UnityEngine;

// Test-Maschine ohne Grafik: verarbeitet ein Rezept mit eigenem Inventar und zeigt ihren Zustand im Play-Mode an.
// Mehrere Maschinen lassen sich über "Output Target" zu einer Produktionskette verbinden.
// Schnell anlegen über das Menü: Factory > Test-Kette in Scene erstellen
public class DebugMachine : MonoBehaviour
{
    public enum State
    {
        NoRecipe,
        WaitingForInputs,
        Working,
        OutputFull,
    }

    private static readonly List<DebugMachine> activeMachines = new List<DebugMachine>();

    [SerializeField] private Recipe recipe;
    [SerializeField] private List<ItemStack> startInventory = new List<ItemStack>();
    [Tooltip("Outputs werden automatisch an diese Maschine weitergegeben, sofern deren Rezept sie als Input braucht.")]
    [SerializeField] private DebugMachine outputTarget;
    [SerializeField, Min(0f)] private float speedMultiplier = 1f;
    [Tooltip("Position der Anzeige von links nach rechts")]
    [SerializeField] private int displayOrder;

    private readonly Inventory inventory = new Inventory();
    private bool isCrafting;
    private float craftProgress;
    private int craftedCount;

    public Inventory Inventory => inventory;
    public Recipe Recipe => recipe;
    public State CurrentState { get; private set; }

    private void Awake()
    {
        inventory.Add(startInventory);
    }

    private void OnEnable()
    {
        activeMachines.Add(this);
        activeMachines.Sort((a, b) => a.displayOrder.CompareTo(b.displayOrder));
    }

    private void OnDisable()
    {
        activeMachines.Remove(this);
    }

    private void Update()
    {
        if (recipe == null)
        {
            CurrentState = State.NoRecipe;
            return;
        }

        if (!isCrafting)
            TryStartCrafting();

        if (isCrafting)
        {
            CurrentState = State.Working;
            craftProgress += Time.deltaTime * speedMultiplier;
            if (craftProgress >= recipe.CraftTime)
            {
                inventory.Add(recipe.Outputs);
                isCrafting = false;
                craftedCount++;
            }
        }

        PushOutputs();
    }

    private void TryStartCrafting()
    {
        if (!inventory.Has(recipe.Inputs))
        {
            CurrentState = State.WaitingForInputs;
            return;
        }
        if (!HasOutputSpace())
        {
            CurrentState = State.OutputFull;
            return;
        }

        inventory.Remove(recipe.Inputs);
        isCrafting = true;
        craftProgress = 0f;
    }

    private bool HasOutputSpace()
    {
        foreach (ItemStack output in recipe.Outputs)
        {
            if (inventory.GetAmount(output.Item) + output.Amount > output.Item.MaxStackSize)
                return false;
        }
        return true;
    }

    // Gibt fertige Outputs an die nächste Maschine weiter, bis deren Puffer (MaxStackSize) voll ist.
    private void PushOutputs()
    {
        if (outputTarget == null || outputTarget.recipe == null)
            return;

        foreach (ItemStack output in recipe.Outputs)
        {
            ItemData item = output.Item;
            if (!outputTarget.recipe.Uses(item))
                continue;

            int space = item.MaxStackSize - outputTarget.inventory.GetAmount(item);
            int amount = Mathf.Min(inventory.GetAmount(item), space);
            if (amount > 0 && inventory.Remove(item, amount))
                outputTarget.inventory.Add(item, amount);
        }
    }

    private void OnGUI()
    {
        const float width = 230f;
        const float height = 260f;
        int index = activeMachines.IndexOf(this);
        int columns = Mathf.Max(1, (int)((Screen.width - 10f) / (width + 10f)));
        Rect area = new Rect(10f + index % columns * (width + 10f), 10f + index / columns * (height + 10f), width, height);

        GUI.Box(area, GUIContent.none);
        GUILayout.BeginArea(new Rect(area.x + 8f, area.y + 6f, area.width - 16f, area.height - 12f));
        GUILayout.Label($"<b>{name}</b>", RichText);
        GUILayout.Label(BuildStatusText(), RichText);
        GUILayout.EndArea();
    }

    private string BuildStatusText()
    {
        StringBuilder text = new StringBuilder();

        if (recipe == null)
            return "Kein Rezept zugewiesen";

        string building = recipe.ProducedIn != null ? recipe.ProducedIn.DisplayName : "?";
        text.AppendLine($"Rezept: {recipe.DisplayName} ({building})");
        text.AppendLine($"{FormatStacks(recipe.Inputs)} → {FormatStacks(recipe.Outputs)}");
        text.AppendLine($"Status: {StateText()}");

        float percent = isCrafting ? Mathf.Clamp01(craftProgress / recipe.CraftTime) : 0f;
        int filled = Mathf.RoundToInt(percent * 10f);
        text.AppendLine($"[{new string('#', filled)}{new string('-', 10 - filled)}] {percent:P0}");
        text.AppendLine($"Hergestellt: {craftedCount}");
        text.AppendLine();
        text.AppendLine("<b>Inventar</b>");

        if (inventory.Contents.Count == 0)
            text.AppendLine("(leer)");
        foreach (KeyValuePair<ItemData, int> entry in inventory.Contents)
            text.AppendLine($"{entry.Value} / {entry.Key.MaxStackSize}  {entry.Key.DisplayName}");

        return text.ToString();
    }

    private string StateText()
    {
        switch (CurrentState)
        {
            case State.Working: return "<color=lime>Arbeitet</color>";
            case State.WaitingForInputs: return "<color=yellow>Wartet auf Inputs</color>";
            case State.OutputFull: return "<color=orange>Output voll</color>";
            default: return "<color=red>Kein Rezept</color>";
        }
    }

    private static string FormatStacks(IReadOnlyList<ItemStack> stacks)
    {
        if (stacks.Count == 0)
            return "–";

        List<string> parts = new List<string>();
        foreach (ItemStack stack in stacks)
            parts.Add(stack.ToString());
        return string.Join(" + ", parts);
    }

    private static GUIStyle richText;
    private static GUIStyle RichText
    {
        get
        {
            if (richText == null)
                richText = new GUIStyle(GUI.skin.label) { richText = true, wordWrap = true };
            return richText;
        }
    }
}
